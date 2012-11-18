using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using FlitsMeldingen.Core;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace FlitsMeldingen
{
    public partial class MainPage : PhoneApplicationPage
    {
        private FlitsMeldingReader reader;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            reader = new FlitsMeldingReader(App.ViewModel.Items);
            reader.Finished += new EventHandler(reader_Finished);

            MainListBox.ItemsSource = App.ViewModel.Items;

            ApplicationBarIconButton appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            appBarButton.Text = ResourceText.Refresh;

            ApplicationBarMenuItem menuItem0 = ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            menuItem0.Text = ResourceText.review;

            ApplicationBarMenuItem menuItem = ApplicationBar.MenuItems[1] as ApplicationBarMenuItem;
            menuItem.Text = ResourceText.about;

        }

        void reader_Finished(object sender, EventArgs e)
        {
            if (App.ViewModel.Items.Count == 0)
            {
                GeenFlitsers.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                GeenFlitsers.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
       

        // When page is navigated to, set data context 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Set the data context of the listbox control to the sample data
            if (DataContext == null)
                DataContext = App.ViewModel;
        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            reader.Update();
        }

        private void Review_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();

        }
    }
}
