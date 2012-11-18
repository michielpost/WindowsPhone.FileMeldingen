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

namespace FileMeldingen.Core
{
    public class FileMelding
    {
        public string Wegnummer { get; set; }
        public string Locatie { get; set; }
        public string Details { get; set; }
        public string Lengte { get; set; }
        public string Vertraging { get; set; }
        public string Oorzaak { get; set; }
    }
}
