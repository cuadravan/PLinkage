using Microsoft.Maui.Controls;
using System;
using System.Windows.Input;

namespace PLinkageApp.AndroidControls
{
    public partial class UserProfileCard : ContentView
    {
        public static readonly BindableProperty UserNameProperty =
            BindableProperty.Create(nameof(UserName), typeof(string), typeof(UserProfileCard), string.Empty);

        public static readonly BindableProperty UserSubtitleProperty =
            BindableProperty.Create(nameof(UserSubtitle), typeof(string), typeof(UserProfileCard), string.Empty);

        public static readonly BindableProperty UserRatingProperty =
            BindableProperty.Create(nameof(UserRating), typeof(string), typeof(UserProfileCard), string.Empty);

        public static readonly BindableProperty JoinedDateProperty =
            BindableProperty.Create(nameof(JoinedDate), typeof(DateTime), typeof(UserProfileCard), DateTime.Now);

        public static readonly BindableProperty IsUserActivatedProperty =
            BindableProperty.Create(nameof(IsUserActivated), typeof(bool), typeof(UserProfileCard), true,
                propertyChanged: OnIsUserActivatedChanged);

        public static readonly BindableProperty IsRatingVisibleProperty =
            BindableProperty.Create(nameof(IsRatingVisible), typeof(bool), typeof(UserProfileCard), false);

        public static readonly BindableProperty IsMessageButtonVisibleProperty =
            BindableProperty.Create(nameof(IsMessageButtonVisible), typeof(bool), typeof(UserProfileCard), false);

        public static readonly BindableProperty IsDeactivateActivateButtonVisibleProperty =
            BindableProperty.Create(nameof(IsDeactivateActivateButtonVisible), typeof(bool), typeof(UserProfileCard), false);

        public static readonly BindableProperty IsSendOfferButtonVisibleProperty =
            BindableProperty.Create(nameof(IsSendOfferButtonVisible), typeof(bool), typeof(UserProfileCard), false);

        public static readonly BindableProperty IsUpdateProfileButtonVisibleProperty =
            BindableProperty.Create(nameof(IsUpdateProfileButtonVisible), typeof(bool), typeof(UserProfileCard), false);

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }
        public string UserSubtitle
        {
            get => (string)GetValue(UserSubtitleProperty);
            set => SetValue(UserSubtitleProperty, value);
        }
        public string UserRating
        {
            get => (string)GetValue(UserRatingProperty);
            set => SetValue(UserRatingProperty, value);
        }
        public DateTime JoinedDate
        {
            get => (DateTime)GetValue(JoinedDateProperty);
            set => SetValue(JoinedDateProperty, value);
        }
        public bool IsUserActivated
        {
            get => (bool)GetValue(IsUserActivatedProperty);
            set => SetValue(IsUserActivatedProperty, value);
        }
        public bool IsRatingVisible
        {
            get => (bool)GetValue(IsRatingVisibleProperty);
            set => SetValue(IsRatingVisibleProperty, value);
        }
        public bool IsMessageButtonVisible
        {
            get => (bool)GetValue(IsMessageButtonVisibleProperty);
            set => SetValue(IsMessageButtonVisibleProperty, value);
        }
        public bool IsDeactivateActivateButtonVisible
        {
            get => (bool)GetValue(IsDeactivateActivateButtonVisibleProperty);
            set => SetValue(IsDeactivateActivateButtonVisibleProperty, value);
        }

        public bool IsSendOfferButtonVisible
        {
            get => (bool)GetValue(IsSendOfferButtonVisibleProperty);
            set => SetValue(IsSendOfferButtonVisibleProperty, value);
        }

        public bool IsUpdateProfileButtonVisible
        {
            get => (bool)GetValue(IsUpdateProfileButtonVisibleProperty);
            set => SetValue(IsUpdateProfileButtonVisibleProperty, value);
        }

        public UserProfileCard()
        {
            InitializeComponent();
            UpdateButtonState(IsUserActivated);
        }

        private static void OnIsUserActivatedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (UserProfileCard)bindable;
            control.UpdateButtonState((bool)newValue);
        }

        private void UpdateButtonState(bool isActivated)
        {
            if (isActivated)
            {
                DeactivateActivateBtn.Text = "Deactivate";
                DeactivateActivateBtn.BackgroundColor = Color.FromArgb("#C45B3A");
            }
            else
            {
                DeactivateActivateBtn.Text = "Activate";
                DeactivateActivateBtn.BackgroundColor = Color.FromArgb("#8A2BE2");
            }
        }     

        public static readonly BindableProperty MessageCommandProperty =
            BindableProperty.Create(nameof(MessageCommand), typeof(ICommand), typeof(UserProfileCard), null);

        public static readonly BindableProperty UpdateProfileCommandProperty =
            BindableProperty.Create(nameof(UpdateProfileCommand), typeof(ICommand), typeof(UserProfileCard), null);

        public static readonly BindableProperty SendOfferCommandProperty =
            BindableProperty.Create(nameof(SendOfferCommand), typeof(ICommand), typeof(UserProfileCard), null);

        public static readonly BindableProperty DeactivateActivateCommandProperty =
            BindableProperty.Create(nameof(DeactivateActivateCommand), typeof(ICommand), typeof(UserProfileCard), null);

        public ICommand MessageCommand
        {
            get => (ICommand)GetValue(MessageCommandProperty);
            set => SetValue(MessageCommandProperty, value);
        }

        public ICommand UpdateProfileCommand
        {
            get => (ICommand)GetValue(UpdateProfileCommandProperty);
            set => SetValue(UpdateProfileCommandProperty, value);
        }

        public ICommand SendOfferCommand
        {
            get => (ICommand)GetValue(SendOfferCommandProperty);
            set => SetValue(SendOfferCommandProperty, value);
        }

        public ICommand DeactivateActivateCommand
        {
            get => (ICommand)GetValue(DeactivateActivateCommandProperty);
            set => SetValue(DeactivateActivateCommandProperty, value);
        }



    }
}
