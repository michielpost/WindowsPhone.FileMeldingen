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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;


namespace FileMeldingen.Core
{
    public class FileMeldingReader
    {
        WebClient webClient = new WebClient();
       
        public ObservableCollection<FileMelding> FileMeldingCollection { get; set; }

        public event EventHandler Finished;

        public FileMeldingReader(ObservableCollection<FileMelding> fileMeldingCollection)
        {
            this.FileMeldingCollection = fileMeldingCollection;

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            Update();
        }

        public void Update()
        {
            webClient.DownloadStringAsync(new Uri("http://www.verkeerplaza.nl/filelijst"));
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {

                    string content = e.Result;
                    int beginMeldingen = content.IndexOf("<div class=\"filelijst\">");
                    int endMeldingen = content.IndexOf("<div class=\"rightFrame\">", beginMeldingen);

                    string meldingenHtml = content.Substring(beginMeldingen, endMeldingen - beginMeldingen);

                    string[] splitString = new string[1] { "<ul class=\"melding\">" };
                    string[] splitMeldingen = meldingenHtml.Split(splitString, StringSplitOptions.RemoveEmptyEntries);

                    FileMeldingCollection.Clear();

                    foreach (var item in splitMeldingen)
                    {
                        ParseMelding(item);
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
            ParseMelding(item, string.Empty);
        }
        private void ParseMelding(string item, string wegnummer)
        {
            if(string.IsNullOrEmpty(wegnummer))
                wegnummer = ParseLi(item, "wegnummer");

            FileMelding melding = new FileMelding();
            melding.Wegnummer = wegnummer;
            melding.Locatie = ParseLi(item, "locatie");
            melding.Details = ParseLi(item, "list-detail");
            melding.Lengte = ParseLi(item, "situatie");
            melding.Vertraging = ParseLi(item, "vertraging");
            melding.Oorzaak = ParseLi(item, "list-detail", item.IndexOf("list-detail") + 15);

            if (string.IsNullOrEmpty(melding.Wegnummer) && FileMeldingCollection.Count > 0)
            {
                melding.Wegnummer = FileMeldingCollection.Last().Wegnummer;
            }

            if (!string.IsNullOrEmpty(melding.Wegnummer) && !string.IsNullOrEmpty(melding.Wegnummer))
                FileMeldingCollection.Add(melding);

            //Second melding
            int beginBr = item.IndexOf("<br />");
            if (beginBr > 0)
            {
                int beginSecond = item.IndexOf("locatie", beginBr);
                if (beginSecond > 0)
                    ParseMelding(item.Substring(beginBr), wegnummer);
            }
        }

        private string ParseLi(string content, string name, int from = 0)
        {
            int begin = content.IndexOf(name, from);

            if (begin > 0)
            {
                int beginContent = content.IndexOf(">", begin) + 1;
                int endContent = content.IndexOf("<", beginContent);

                string detailContent = content.Substring(beginContent, endContent - beginContent);
                detailContent = detailContent.Replace("\r\n", string.Empty).Trim();
                detailContent = detailContent.Replace("\t", " ").Trim();

                detailContent =  detailContent.Replace("&nbsp;", " ");

                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"[ ]{2,}", options);
                detailContent = regex.Replace(detailContent, @" ");

                return detailContent;
            }

            return string.Empty;
        }
    }
}

