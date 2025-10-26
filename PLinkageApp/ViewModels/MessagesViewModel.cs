using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ReceiverName),"ReceiverName")]
    [QueryProperty(nameof(ChatId),"ChatId")]
    [QueryProperty(nameof(ReceiverId), "ReceiverId")]
    public partial class MessagesViewModel: ObservableObject
    {
        private readonly IChatServiceClient _chatServiceClient;
        private readonly ISessionService _sessionService;

        public MessagesViewModel(IChatServiceClient chatServiceClient, ISessionService sessionService)
        {
            _chatServiceClient = chatServiceClient;
            _sessionService = sessionService;
            Messages = new ObservableCollection<ChatMessageDto>();
        }
        public ObservableCollection<ChatMessageDto> Messages { get; set; }

        [ObservableProperty]
        public bool isBusy;

        [ObservableProperty]
        public string receiverName = string.Empty;
        [ObservableProperty]
        public string messageToSend = string.Empty;

        public Guid ChatId { get; set; } = Guid.Empty;
        public Guid ReceiverId { get; set; } = Guid.Empty;

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await GetChatMessages();
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

        private async Task GetChatMessages()
        {
            
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                if (ChatId == Guid.Empty)
                    return;
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
                    await Shell.Current.DisplayAlert("Failed to Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting chat messages: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (IsBusy)
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
                    await Shell.Current.DisplayAlert("Failed to Send/Fetch Result", $"The server returned the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending chat messages: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while sending and/or fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                MessageToSend = string.Empty;
                IsBusy = false;
                await GetChatMessages();
            }
        }


    }
}
