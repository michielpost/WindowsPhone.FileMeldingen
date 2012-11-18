using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using FileMeldingen.Core;
using System.Collections.ObjectModel;
using FlitsMeldingen.Core;

namespace FileFlitsMeldingen.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            // Insert code required on object creation below this point
            Flitsers = new ObservableCollection<FlitsMelding>();
            Files = new ObservableCollection<FileMelding>();

            //Files.Add(new FileMelding() { Wegnummer = "A4", Details = "123", Lengte = "5 km", Locatie = "hierzo", Vertraging = "10 min", Oorzaak = "?" });
            //Flitsers.Add(new FlitsMelding() { Wegnummer = "A3", Details = "nee", HMP = "hmp 5",  Tijdstip = "a" });
        }

        public ObservableCollection<FileMelding> Files { get; private set; }
        public ObservableCollection<FlitsMelding> Flitsers { get; private set; }




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
