using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using FileMeldingen.Core;


namespace FileMeldingen
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            // Insert code required on object creation below this point
            Items = new ObservableCollection<FileMelding>();
        }

        public ObservableCollection<FileMelding> Items { get; private set; }

      

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}