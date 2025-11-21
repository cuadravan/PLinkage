using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ForceReset), "ForceReset")]
    public partial class SkillProviderLinkagesViewModel : ObservableObject
    {
        bool _forceReset;

        public bool ForceReset
        {
            get => _forceReset;
            set
            {
                _forceReset = value;
                if (_forceReset)
                {
                    _ = InitializeAsync(); // Trigger logic when property is set
                }
            }
        }

        private bool _isInitialized;

        private ISessionService _sessionService;
        private INavigationService _navigationService;
        private IOfferApplicationServiceClient _offerApplicationServiceClient;

        [ObservableProperty]
        private string selectedTopToggle = "Sent";

        [ObservableProperty]
        private string selectedBottomToggle = "Pending";

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private ObservableCollection<OfferApplicationDisplayDto> offerApplicationCards;

        private OfferApplicationPageDto _allData;

        public SkillProviderLinkagesViewModel(
            ISessionService sessionService,
            IOfferApplicationServiceClient offerApplicationServiceClient,
            INavigationService navigationService)
        {
            _sessionService = sessionService;
            _offerApplicationServiceClient = offerApplicationServiceClient;
            _navigationService = navigationService;

            offerApplicationCards = new ObservableCollection<OfferApplicationDisplayDto>();
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized && !ForceReset) //If already initialized, or not a force reset, then don't initialize
                return;
            ForceReset = false;
            _isInitialized = true;
            await LoadAllData();
        }

        private async Task LoadAllData()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var userId = _sessionService.GetCurrentUserId();
                var userRole = _sessionService.GetCurrentUserRole();

                var response = await _offerApplicationServiceClient.GetOfferApplicationOfUser(userId, userRole);

                if (response.Success && response.Data != null)
                {
                    _allData = response.Data;
                    await UpdateDisplayedCards();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdateDisplayedCards()
        {
            if (_allData == null)
                return;

            var itemsToShow = await Task.Run(() =>
            {
                if (SelectedTopToggle == "Sent" && SelectedBottomToggle == "Pending")
                    return _allData.SentPending ?? new List<OfferApplicationDisplayDto>();
                else if (SelectedTopToggle == "Sent" && SelectedBottomToggle == "History")
                    return _allData.SentHistory ?? new List<OfferApplicationDisplayDto>();
                else if (SelectedTopToggle == "Received" && SelectedBottomToggle == "Pending")
                    return _allData.ReceivedPending ?? new List<OfferApplicationDisplayDto>();
                else
                    return _allData.ReceivedHistory ?? new List<OfferApplicationDisplayDto>();
            });

            // Clear on UI thread
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                OfferApplicationCards.Clear();
            });

            // Add items in batches to prevent UI freeze
            const int batchSize = 15;

            await Task.Run(async () =>
            {
                for (int i = 0; i < itemsToShow.Count; i += batchSize)
                {
                    var batch = itemsToShow.Skip(i).Take(batchSize).ToList();

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        foreach (var item in batch)
                        {
                            OfferApplicationCards.Add(item);
                        }
                    });

                    // Small delay between batches (except for last batch)
                    if (i + batchSize < itemsToShow.Count)
                    {
                        await Task.Delay(5);
                    }
                }
            });
        }

        [RelayCommand]
        private async Task Refresh()
        {
            _allData = null;
            await LoadAllData();
        }

        [RelayCommand]
        private async Task UpdateTopToggle(string selection)
        {
            if (string.IsNullOrEmpty(selection) || SelectedTopToggle == selection)
                return;

            SelectedTopToggle = selection;
            await UpdateDisplayedCards();
        }

        [RelayCommand]
        private async Task UpdateBottomToggle(string selection)
        {
            if (string.IsNullOrEmpty(selection) || SelectedBottomToggle == selection)
                return;

            SelectedBottomToggle = selection;
            await UpdateDisplayedCards();
        }

        [RelayCommand]
        public async Task Accept(OfferApplicationDisplayDto dto)
        {
            try
            {
                string rawRate = Regex.Match(dto.FormattedRate, @"\d+(\.\d+)?").Value;
                string rawTimeFrame = Regex.Match(dto.FormattedTimeFrame, @"\d+(\.\d+)?").Value;

                var processDto = new OfferApplicationProcessDto
                {
                    OfferApplicationId = dto.OfferApplicationId,
                    Process = "Approve",
                    Type = dto.OfferApplicationType,
                    SenderId = dto.SenderId,
                    ReceiverId = dto.ReceiverId,
                    ProjectId = dto.ProjectId,
                    NegotiatedRate = decimal.Parse(rawRate),
                    NegotiatedTimeFrame = int.Parse(rawTimeFrame)
                };

                var result = await _offerApplicationServiceClient.ProcessOfferApplication(processDto);

                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "You have successfully approved this request.", "Ok");
                    ForceReset = true;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"There was an error in processing your request. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"There was an error in processing your request. Error: {ex}", "Ok");
            }
        }

        [RelayCommand]
        public async Task Reject(OfferApplicationDisplayDto dto)
        {
            try
            {
                string rawRate = Regex.Match(dto.FormattedRate, @"\d+(\.\d+)?").Value;
                string rawTimeFrame = Regex.Match(dto.FormattedTimeFrame, @"\d+(\.\d+)?").Value;


                var processDto = new OfferApplicationProcessDto
                {
                    OfferApplicationId = dto.OfferApplicationId,
                    Process = "Reject",
                    Type = dto.OfferApplicationType,
                    SenderId = dto.SenderId,
                    ReceiverId = dto.ReceiverId,
                    ProjectId = dto.ProjectId,
                    NegotiatedRate = decimal.Parse(rawRate),
                    NegotiatedTimeFrame = int.Parse(rawTimeFrame)
                };

                var result = await _offerApplicationServiceClient.ProcessOfferApplication(processDto);

                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Success", "You have successfully rejected this request.", "Ok");
                    ForceReset = true;   
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"There was an error in processing your request. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"There was an error in processing your request. Error: {ex}", "Ok");
            }
        }

        [RelayCommand]
        public async Task Negotiate(OfferApplicationDisplayDto dto)
        {
            await _navigationService.NavigateToAsync("NegotiatingOfferView", new Dictionary<string, object> { { "DisplayDto", dto } });
        }
    }
}
