using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FuzzySharp;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using System.Collections.ObjectModel;

namespace PLinkageApp.ViewModels
{
    public partial class ChatAndroidViewModel : ObservableObject
    {
        private readonly IChatServiceClient _chatServiceClient;
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;

        private const int FuzzySearchCutoff = 70;

        private List<ChatSummaryDto> _chatPreviews = new List<ChatSummaryDto>();

        [ObservableProperty]
        private bool isBusy;
        [ObservableProperty]
        private string searchQuery = "";

        public ObservableCollection<ChatSummaryDto> ChatPreviews { get; set; } = new ObservableCollection<ChatSummaryDto>();

        public ChatAndroidViewModel(IChatServiceClient chatServiceClient, INavigationService navigationService, ISessionService sessionService)
        {
            _chatServiceClient = chatServiceClient;
            _navigationService = navigationService;
            _sessionService = sessionService;
        }

        public async Task InitializeAsync()
        {
            if (_chatPreviews.Any())
                return;

            try
            {
                await GetChatPreviews();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetChatPreviews();
        }
        [RelayCommand]
        private async Task ViewChat(ChatSummaryDto chatSummaryDto)
        {
            await _navigationService.NavigateToAsync("MessagesView", new Dictionary<string, object> { { "ChatId", chatSummaryDto.ChatId }, { "ReceiverId", chatSummaryDto.ReceiverId }, { "ReceiverName", chatSummaryDto.ReceiverFullName } });
        }

        private async Task GetChatPreviews()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                _chatPreviews.Clear();
                ChatPreviews.Clear();
                var userId = _sessionService.GetCurrentUserId();
                var result = await _chatServiceClient.GetChatSummariesAsync(userId);

                if (result.Success && result.Data != null)
                {

                    foreach (var dto in result.Data)
                    {
                        _chatPreviews.Add(dto);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
                FilterChatPreviews();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting chat previews: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }

        partial void OnSearchQueryChanged(string value)
        {
            FilterChatPreviews();
        }

        private void FilterChatPreviews()
        {
            var query = SearchQuery.Trim().ToLowerInvariant();

            IEnumerable<ChatSummaryDto> filteredList;

            if (string.IsNullOrEmpty(query))
            {
                filteredList = _chatPreviews;
            }
            else
            {
                filteredList = _chatPreviews
                    .Where(card => Fuzz.PartialRatio(query, card.ReceiverFullName.ToLowerInvariant())
                                     > FuzzySearchCutoff);

            }
            ChatPreviews.Clear();
            foreach (var card in filteredList)
            {
                ChatPreviews.Add(card);
            }
        }

        

    }
}