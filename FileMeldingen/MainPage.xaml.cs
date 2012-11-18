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
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using FileMeldingen.Core;

namespace FileMeldingen
{
    public partial class MainPage : PhoneApplicationPage
    {
        private FileMeldingReader reader;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            reader = new FileMeldingReader(App.ViewModel.Items);
            reader.Finished += new EventHandler(reader_Finished);
            MainListBox.ItemsSource = App.ViewModel.Items;
        }

        void reader_Finished(object sender, EventArgs e)
        {
            if (App.ViewModel.Items.Count == 0)
            {
                GeenFiles.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                GeenFiles.Visibility = System.Windows.Visibility.Collapsed;
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
    }
}
