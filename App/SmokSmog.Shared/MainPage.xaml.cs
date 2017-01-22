﻿using SmokSmog.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += MainPageLoaded;
            this.Unloaded += MainPageUnloaded;
        }

        private ViewModel.ViewModelLocator ViewModelLocator { get; } = new ViewModel.ViewModelLocator();

        public void MenuCloseHelper(object sender, object parameters)
        {
#if (WINDOWS_APP)
            MenuClose.Storyboard?.Begin();
#endif
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += MainPageSizeChanged;
            SetLayoutVisualState();
            SetSearchState();

            var navProvider = App.Current as INavigationProvider;
            navProvider?.NavigationService?.NavigateTo("StationPage", 4);
        }

        private void MainPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetLayoutVisualState();
            SetSearchState();
        }

        private void MainPageUnloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= MainPageSizeChanged;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SetSearchState(true);
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSearchState();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchString = SearchTextBox.Text;
            ViewModelLocator.SearchViewModel.SearchString = searchString;

            if (string.IsNullOrWhiteSpace(searchString))
            {
                if (!SecondFrame.CurrentSourcePageType.Equals(typeof(Views.SearchPage)) && SecondFrame.CanGoBack)
                    SecondFrame.GoBack();
            }
            else
            {
                if (!SecondFrame.CurrentSourcePageType.Equals(typeof(Views.SearchPage)))
                    SecondFrame.Navigate(typeof(Views.SearchPage));
            }
        }

        private void SetLayoutVisualState()
        {
            double width = ActualWidth;
            string stateName = "Default";
            if (width >= 850) stateName = "Wide";
            if (width <= 440) stateName = "Small";

            VisualStateManager.GoToState(this, stateName, true);
        }

        private void SetSearchState(bool open = false)
        {
            var stateBefore = SearchVisualStateGroup.CurrentState?.Name;

            //if (_searchable == null)
            //{
            //    SearchTextBox.Text = string.Empty;
            //    VisualStateManager.GoToState(this, "DisableSearchState", true);
            //    return;
            //}

            if ((SearchTextBox.FocusState == FocusState.Unfocused && string.IsNullOrWhiteSpace(SearchTextBox.Text)) && !open)
            {
                VisualStateManager.GoToState(this, "ClosedSearchState", true);
                //if (!ContentFrame2.CurrentSourcePageType.Equals(typeof(Views.SearchPage)) && ContentFrame2.CanGoBack)
                //    ContentFrame2.GoBack();
                return;
            }

            //if (!ContentFrame2.CurrentSourcePageType.Equals(typeof(Views.SearchPage)))
            //    ContentFrame2.Navigate(typeof(Views.SearchPage));

            if (ActualWidth > 520)
                VisualStateManager.GoToState(this, "WideSearchState", true);
            else
                VisualStateManager.GoToState(this, "NarrowSearchState", true);

            if (stateBefore != "WideSearchState" && stateBefore != "NarrowSearchState")
                SearchTextBox.Focus(FocusState.Keyboard);
        }
    }
}