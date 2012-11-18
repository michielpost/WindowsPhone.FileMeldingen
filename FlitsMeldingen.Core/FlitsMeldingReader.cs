using System;
using System.Net;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace FlitsMeldingen.Core
{
    public class FlitsMeldingReader
    {
        WebClient webClient = new WebClient();

        public ObservableCollection<FlitsMelding> FlitsMeldingCollection { get; set; }

        private List<FlitsMelding> _tempList = new List<FlitsMelding>();

        public event EventHandler Finished;
        
        public FlitsMeldingReader(ObservableCollection<FlitsMelding>  flitsMeldingCollection)
        {
            this.FlitsMeldingCollection = flitsMeldingCollection;

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            Update();
        }

        public void Update()
        {
            webClient.DownloadStringAsync(new Uri("http://www.flitsers.nl/flitsers.rss"));
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {

                    string content = e.Result;
                    //int beginMeldingen = content.IndexOf("trafficListing");
                    //int endMeldingen = content.IndexOf("<div style=\"width: 340px; float:", beginMeldingen);

                    //string meldingenHtml = content.Substring(beginMeldingen, endMeldingen - beginMeldingen);


                    string[] splitString = new string[1] { "<item>" };
                    string[] splitMeldingen = content.Split(splitString, StringSplitOptions.RemoveEmptyEntries);

                    FlitsMeldingCollection.Clear();
                    _tempList.Clear();
                    foreach (var item in splitMeldingen)
                    {
                        try
                        {
                            ParseMelding(item);
                        }
                        catch { }
                    }

                    var list = _tempList.OrderBy(x => x.WegnummerSort).ToList();

                    foreach(var i in list)
                    {
                        FlitsMeldingCollection.Add(i);
                    }

                    if (Finished != null)
                        Finished(this, null);
                }
                else
                {
                    MessageBox.Show(AppResource.Error);
                }
            }
            catch
            {
                MessageBox.Show(AppResource.Error);
            }
        }


        private void ParseMelding(string item)
        {
            int begin = item.IndexOf("<title>") + 7;
            int end = item.IndexOf("</title>");

            string main = item.Substring(begin, end - begin).Trim();

            if (main.ToLower().Contains("advertentie"))
                return;

            string wegnummer = string.Empty;
            string detail = string.Empty;
            string tijd = string.Empty;
            string hmp = string.Empty;

            int eindDubbelePunt = main.IndexOf(":") + 1;
            int eindWeg = main.IndexOf(" ", eindDubbelePunt+1);
            wegnummer = main.Substring(eindDubbelePunt, eindWeg - eindDubbelePunt).Trim();

            int beginThv = main.IndexOf("thv");
            if (beginThv > 1)
            {
                detail = main.Substring(eindWeg, beginThv - eindWeg).Trim();

                beginThv += 3;

                hmp = main.Substring(beginThv).Trim();

                if (string.IsNullOrEmpty(detail))
                {
                    detail = main.Substring(eindWeg).Trim();

                    hmp = string.Empty;
                }
            }
            else
                detail = main.Substring(eindWeg).Trim();



            int beginTijd = item.IndexOf("<pubDate>") + 9;
            int endTijd = item.IndexOf("</pubDate>");

            tijd = item.Substring(beginTijd, endTijd - beginTijd).Trim();

            DateTime realTime = DateTime.Parse(tijd);
            tijd = "Tijdstip melding: " + realTime.ToString("dd-MM-yy HH:mm");



            if (!string.IsNullOrEmpty(wegnummer) && !string.IsNullOrEmpty(detail))
            {
                FlitsMelding melding = new FlitsMelding();
                melding.Wegnummer = wegnummer;
                melding.Details = HttpUtility.HtmlDecode(detail.Replace(wegnummer, string.Empty).Trim());
                melding.Tijdstip = HttpUtility.HtmlDecode(tijd);
                melding.HMP = hmp;

                if (melding.Wegnummer.Length > 1)
                {

                    try
                    {
                        melding.WegnummerSort = int.Parse(wegnummer.Substring(1));
                    }
                    catch
                    {
                        melding.WegnummerSort = 999;
                    }
                }

                _tempList.Add(melding);
            }


        }

    }
}
