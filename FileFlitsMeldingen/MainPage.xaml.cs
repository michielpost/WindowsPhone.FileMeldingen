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
using FlitsMeldingen.Core;
using FileMeldingen.Core;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;

namespace FileFlitsMeldingen
{
    public partial class MainPage : PhoneApplicationPage
    {
        private FlitsMeldingReader flitsReader;
        private FileMeldingReader fileReader;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            flitsReader = new FlitsMeldingReader(App.ViewModel.Flitsers);
            flitsReader.Finished += new EventHandler(flitsReader_Finished);

            fileReader = new FileMeldingReader(App.ViewModel.Files);
            fileReader.Finished += new EventHandler(fileReader_Finished);



            FileListBox.ItemsSource = App.ViewModel.Files;
            FlitsListBox.ItemsSource = App.ViewModel.Flitsers;

            ApplicationBarIconButton appBarButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            appBarButton.Text = ResourceText.Refresh;

            ApplicationBarMenuItem menuItem = ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            menuItem.Text = ResourceText.about;
        }

        void fileReader_Finished(object sender, EventArgs e)
        {
            if (App.ViewModel.Files.Count == 0)
            {
                GeenFiles.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                GeenFiles.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void flitsReader_Finished(object sender, EventArgs e)
        {
            if (App.ViewModel.Flitsers.Count == 0)
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
        private void FileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (FileListBox.SelectedIndex == -1)
                return;

            string wegnummer = App.ViewModel.Files[FileListBox.SelectedIndex].Wegnummer;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + wegnummer, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            FileListBox.SelectedIndex = -1;
        }

        private void FlitsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (FlitsListBox.SelectedIndex == -1)
                return;

            string wegnummer = App.ViewModel.Flitsers[FlitsListBox.SelectedIndex].Wegnummer;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + wegnummer, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            FlitsListBox.SelectedIndex = -1;
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            flitsReader.Update();
            fileReader.Update();
        }
    }
}