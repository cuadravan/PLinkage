using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FuzzySharp;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using System.Collections.ObjectModel;

namespace PLinkageApp.ViewModels
{
    public partial class ChatViewModel : ObservableObject
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

        // ==========================================================
        // DESKTOP / SPLIT-VIEW SPECIFIC PROPERTIES
        // (These will just be null/unused on Android, which is safe)
        // ==========================================================
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsChatSelected))]
        private MessagesViewModel? _selectedMessageViewModel;

        public bool IsChatSelected => SelectedMessageViewModel != null;

        // ==========================================================
        // SHARED COLLECTIONS
        // ==========================================================
        public ObservableCollection<ChatSummaryDto> ChatPreviews { get; set; } = new ObservableCollection<ChatSummaryDto>();

        public ChatViewModel(IChatServiceClient chatServiceClient, INavigationService navigationService, ISessionService sessionService)
        {
            _chatServiceClient = chatServiceClient;
            _navigationService = navigationService;
            _sessionService = sessionService;
        }

        public async Task InitializeAsync()
        {
            if (_chatPreviews.Any()) return;
            await GetChatPreviews();
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetChatPreviews();
        }

        // ==========================================================
        // THE MERGED LOGIC
        // ==========================================================
        [RelayCommand]
        private async Task ViewChat(ChatSummaryDto chatSummaryDto)
        {
            if (IsBusy) return;

            // STRATEGY: Check the Device Type to decide behavior
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                // --- ANDROID / PHONE BEHAVIOR (Navigate) ---
                await _navigationService.NavigateToAsync("MessagesView", new Dictionary<string, object>
                {
                    { "ChatId", chatSummaryDto.ChatId },
                    { "ReceiverId", chatSummaryDto.ReceiverId },
                    { "ReceiverName", chatSummaryDto.ReceiverFullName }
                });
            }
            else
            {
                // --- WINDOWS / DESKTOP BEHAVIOR (Split View) ---
                try
                {
                    IsBusy = true;

                    // Manually create the Child ViewModel
                    var messageVm = new MessagesViewModel(_chatServiceClient, _sessionService)
                    {
                        ChatId = chatSummaryDto.ChatId,
                        ReceiverId = chatSummaryDto.ReceiverId,
                        ReceiverName = chatSummaryDto.ReceiverFullName
                    };

                    await messageVm.InitializeAsync();

                    SelectedMessageViewModel = messageVm;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading chat details: {ex.Message}");
                    await Shell.Current.DisplayAlert("Error", "Could not load chat details.", "OK");
                }
                finally
                {
                    IsBusy = false;
                }
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
                else
                {
                    // Only show alert if it's a real error, distinct from empty list
                    if (!result.Success)
                        await Shell.Current.DisplayAlert("Error", result.Message, "Ok");
                }
                FilterChatPreviews();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting chat previews: {ex.Message}");
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