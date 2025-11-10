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
    public partial class SkillProviderLinkagesViewModel : ObservableObject
    {
        private ISessionService _sessionService;
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
            IOfferApplicationServiceClient offerApplicationServiceClient)
        {
            _sessionService = sessionService;
            _offerApplicationServiceClient = offerApplicationServiceClient;

            offerApplicationCards = new ObservableCollection<OfferApplicationDisplayDto>();
        }

        public async Task InitializeAsync()
        {
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
    }
}
