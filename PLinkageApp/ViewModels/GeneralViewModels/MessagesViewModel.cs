using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using System.Collections.ObjectModel;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ReceiverName),"ReceiverName")]
    [QueryProperty(nameof(ChatId),"ChatId")]
    [QueryProperty(nameof(ReceiverId), "ReceiverId")]
    public partial class MessagesViewModel: ObservableObject
    {
        private readonly IChatServiceClient _chatServiceClient;
        private readonly ISessionService _sessionService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<ChatMessageDto> Messages { get; set; } = new ObservableCollection<ChatMessageDto>();

        [ObservableProperty]
        private bool isBusy = false;
        [ObservableProperty]
        private string receiverName = string.Empty;
        [ObservableProperty]
        private string messageToSend = string.Empty;

        public Guid ChatId { get; set; } = Guid.Empty;
        public Guid ReceiverId { get; set; } = Guid.Empty;

        public MessagesViewModel(IDialogService dialogService, IChatServiceClient chatServiceClient, ISessionService sessionService)
        {
            _chatServiceClient = chatServiceClient;
            _sessionService = sessionService;
            _dialogService = dialogService;
        }
        public async Task InitializeAsync()
        {
            if (Messages.Any())
                return;
            try
            {
                await GetChatMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetChatMessages();
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (IsBusy)
                return;
            if (ReceiverId == Guid.Empty)
                return;
            IsBusy = true;
            try
            {
                var userId = _sessionService.GetCurrentUserId();

                var newMessageDto = new SendMessageDto
                {
                    ReceiverId = ReceiverId,
                    Content = MessageToSend
                };
                var result = await _chatServiceClient.SendMessageAsync(userId, newMessageDto);

                if (result.Success && result.Data != null)
                {
                    if (this.ChatId == Guid.Empty)
                    {
                        this.ChatId = result.Data.ChatId;
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Failed to Send/Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending chat messages: {ex.Message}");
                await _dialogService.ShowAlertAsync("Error", $"An error occurred while sending and/or fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                MessageToSend = string.Empty;
                IsBusy = false;
                await GetChatMessages();
            }
        }

        private async Task GetChatMessages()
        {  
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                ApiResponse<Guid> response = null;
                if (ChatId == Guid.Empty){
                    response = await _chatServiceClient.GetChatIdAsync(_sessionService.GetCurrentUserId(), ReceiverId);
                    if(response.Success && response.Data != Guid.Empty)
                    {
                        ChatId = response.Data;
                    }
                    else
                    {
                        IsBusy = false;
                        return;
                    }            
                }
                Messages.Clear();
                var userId = _sessionService.GetCurrentUserId();
                var result = await _chatServiceClient.GetChatMessagesAsync(ChatId, userId);

                if (result.Success && result.Data != null)
                {

                    foreach (var dto in result.Data)
                    {
                        Messages.Add(dto);
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
