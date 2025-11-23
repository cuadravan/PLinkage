using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FuzzySharp;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using System.Collections.ObjectModel;

namespace PLinkageApp.ViewModels
{
    public partial class ChatWindowsViewModel : ObservableObject
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

        // 1. PROPERTY TO HOLD THE ACTIVE CHAT (RIGHT SIDE)
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsChatSelected))]
        private MessagesViewModel? _selectedMessageViewModel;

        // Helper to toggle visibility in XAML
        public bool IsChatSelected => SelectedMessageViewModel != null;

        public ObservableCollection<ChatSummaryDto> ChatPreviews { get; set; } = new ObservableCollection<ChatSummaryDto>();

        public ChatWindowsViewModel(IChatServiceClient chatServiceClient, INavigationService navigationService, ISessionService sessionService)
        {
            _chatServiceClient = chatServiceClient;
            _navigationService = navigationService;
            _sessionService = sessionService;
        }

        public async Task InitializeAsync()
        {
            if (_chatPreviews.Any()) return;
            try { await GetChatPreviews(); }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetChatPreviews();
        }

        // 2. UPDATED COMMAND: LOAD CHAT INTO RIGHT PANE
        [RelayCommand]
        private async Task ViewChat(ChatSummaryDto chatSummaryDto)
        {
            if (IsBusy) return;

            // Option A: If you are on Mobile, keep the old navigation
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                await _navigationService.NavigateToAsync("MessagesView", new Dictionary<string, object>
                 {
                     { "ChatId", chatSummaryDto.ChatId },
                     { "ReceiverId", chatSummaryDto.ReceiverId },
                     { "ReceiverName", chatSummaryDto.ReceiverFullName }
                 });
                return;
            }

            // Option B: Desktop/Tablet - Load into the Right Pane
            try
            {
                IsBusy = true;

                // Manually create the Child ViewModel. 
                // Since ChatViewModel already has the dependencies (ChatService, SessionService), we pass them down.
                var messageVm = new MessagesViewModel(_chatServiceClient, _sessionService);

                // Manually set the data (simulating QueryProperties)
                messageVm.ChatId = chatSummaryDto.ChatId;
                messageVm.ReceiverId = chatSummaryDto.ReceiverId;
                messageVm.ReceiverName = chatSummaryDto.ReceiverFullName;

                // Load the messages
                await messageVm.InitializeAsync();

                // Assign to property to update UI
                SelectedMessageViewModel = messageVm;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetChatPreviews()
        {
            if (IsBusy) return;
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
                FilterChatPreviews();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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
                    .Where(card => Fuzz.PartialRatio(query, card.ReceiverFullName.ToLowerInvariant()) > FuzzySearchCutoff);
            }
            ChatPreviews.Clear();
            foreach (var card in filteredList)
            {
                ChatPreviews.Add(card);
            }
        }
    }
}