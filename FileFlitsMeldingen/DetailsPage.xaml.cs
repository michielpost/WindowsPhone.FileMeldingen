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
using System.Windows.Data;
using System.Windows.Navigation;
using FlitsMeldingen.Core;
using FileMeldingen.Core;

namespace FileFlitsMeldingen
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        public string Wegnummer
        {
            get { return (string)GetValue(WegnummerProperty); }
            set { SetValue(WegnummerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Wegnummer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WegnummerProperty =
            DependencyProperty.Register("Wegnummer", typeof(string), typeof(DetailsPage), new PropertyMetadata(string.Empty));

        CollectionViewSource flitsCollectionView = new CollectionViewSource();
        CollectionViewSource fileCollectionView = new CollectionViewSource();

        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
            ListTitle.DataContext = this;
            flitsCollectionView.Filter += new FilterEventHandler(flitsCollectionView_Filter);
            fileCollectionView.Filter += new FilterEventHandler(fileCollectionView_Filter);

        }

        // When page is navigated to, set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
            
                Wegnummer = selectedIndex;
                flitsCollectionView.Source = App.ViewModel.Flitsers;
                fileCollectionView.Source = App.ViewModel.Files;

                FileListBox.ItemsSource = fileCollectionView.View;
                FlitsListBox.ItemsSource = flitsCollectionView.View;

                if (FileListBox.Items.Count == 0)
                    FileHeaderText.Text = ResourceText.NoTraffic;

                if (FlitsListBox.Items.Count == 0)
                    FlitsHeaderText.Text = ResourceText.NoRadar;

            }
        }

        void fileCollectionView_Filter(object sender, FilterEventArgs e)
        {
            FileMelding p = (FileMelding)e.Item;

            if (p.Wegnummer == Wegnummer)
                e.Accepted = true;
            else
                e.Accepted = false;
        }

        void flitsCollectionView_Filter(object sender, FilterEventArgs e)
        {
            FlitsMelding p = (FlitsMelding)e.Item;

            if (p.Wegnummer == Wegnummer)
                e.Accepted = true;
            else
                e.Accepted = false;
        }
    }
}
