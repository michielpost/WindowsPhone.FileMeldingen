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

namespace FlitsMeldingen
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

        CollectionViewSource collectionView = new CollectionViewSource();

        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
            ListTitle.DataContext = this;
            collectionView.Filter += new FilterEventHandler(collectionView_Filter);

        }

        // When page is navigated to, set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                int index = int.Parse(selectedIndex);
                Wegnummer = App.ViewModel.Items[index].Wegnummer;
                collectionView.Source = App.ViewModel.Items;
                MainListBox.ItemsSource = collectionView.View;

            }
        }

        void collectionView_Filter(object sender, FilterEventArgs e)
        {
            FlitsMelding p = (FlitsMelding)e.Item;

            if (p.Wegnummer == Wegnummer)
                e.Accepted = true;
            else
                e.Accepted = false;
        }
    }
}
