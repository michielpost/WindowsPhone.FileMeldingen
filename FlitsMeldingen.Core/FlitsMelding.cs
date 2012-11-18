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

namespace FlitsMeldingen.Core
{
    public class FlitsMelding
    {
        public string Wegnummer { get; set; }
        public int WegnummerSort { get; set; }
        public string Details { get; set; }
        public string Tijdstip { get; set; }
        public string HMP { get; set; }

    }
}
