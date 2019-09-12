using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace MirrorProgram
{
    class Mirror 
    {
        static System.Windows.Forms.Timer changeLine = new System.Windows.Forms.Timer();

        static int articleNum = 0, timerCount = 0;
        static System.Collections.Generic.List<NewsAPI.Models.Article> results;
        private delegate void SafeCallDelegate(string text);

        static Label newsText = new Label();
        static Label dateText = new Label();
                //initiaize labels and newsAPI results as global
        public partial class mainForm : Form
        {

            public mainForm()
            {
                InitializeComponent();
            }
            
            private void Form1_Load(object sender, EventArgs e)
            {

            }

            private void InitializeComponent()
            {
                this.BackColor = Color.Black;
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None;
                        //set form size w no border
                Font type1 = new Font("century gothic", 20f);
                Font type2 = new Font("century gothic", 15f);
                        //fonts
                newsText.SetBounds(25, 600, 800, 400);
                dateText.SetBounds(25, 25, 200, 200);
                        //set label bounds
                newsText.BorderStyle = BorderStyle.None;
                dateText.BorderStyle = BorderStyle.None;
                        //no label border
                newsText.Font = type2;
                dateText.Font = type1;
                newsText.ForeColor = Color.White;
                dateText.ForeColor = Color.White;
                //newsText.Text = "Test1";
                //dateText.Text = "Test2";

                this.Controls.Add(newsText);
                this.Controls.Add(dateText);
                this.ShowDialog();
            }
        }

        static mainForm onlyForm = new mainForm();
        //global form

        public static void Main(string[] args) 
        {
            News();
                    //call method to fetch news headlines
            Thread newsSide = new Thread(SetNewsTimer);
            newsSide.Start();

            DateAndtime();
        }
        public static void SetNewsTimer()
        {
            changeLine.Tick += new EventHandler(Printnews);
            changeLine.Interval = 10000;
            changeLine.Start();
            while (results != null)
            {
                Application.DoEvents();
                if (timerCount == 120)
                {//if cycle through each headline 15 times, call for more
                    News();
                    timerCount = 0;
                }
            }
        }
        public static void DateAndtime()//Object anObject, EventArgs theArgs)
        {
            while (results != null)
            {
                DateTime current = DateTime.Now;
            
                String info = current.DayOfWeek.ToString() + " " + current.Month + "/" + current.Day + "\n" + current.ToString("t");
                WriteDate(info);
                Thread.Sleep(10000);
            }//while results are returned, print time every 10 seconds
        }
        public static void WriteDate(string text)
        {
            if (dateText.InvokeRequired)
            {//if the thread IDs dont match, cross thread exception
                var passin = new SafeCallDelegate(WriteDate);    //make this method passable
                dateText.Invoke(passin, new object[] { text }); //executes WriteDate on thread with the form controls
            }
            else
            {//if same thread, just write
                dateText.Text = text;
            }
        }//method to safely write the string to onlyForm
        public static void News()
        {
            var NewsApiClient = new NewsApiClient("ENTER API KEY HERE"); //pass API key to NewsAPI
            var articlesResponse = NewsApiClient.GetTopHeadlines(new TopHeadlinesRequest
            {
                Language = Languages.EN,
                PageSize = 8,
                Country = Countries.US
            });     //get 8 headlines from the US in english
            //Console.WriteLine(articlesResponse.Status);
            
            if (articlesResponse.Status == Statuses.Ok)
            {
                results = articlesResponse.Articles;    
            }//sets global variable
            else
            {
                results = null;
            }//or not
        }
        public static void Printnews(Object theObject, EventArgs theArgs)
        {
            changeLine.Stop();
            timerCount++;
                    //stops timer and adds to count of headlines cycled through
            NewsAPI.Models.Article article = results[articleNum];
                    //cycles to next headline in the set of 8

            newsText.Text = article.Title + "\n" + article.Author + "\n" + article.PublishedAt + "\n";
                    //update label text

            onlyForm.Show();
            if (articleNum == 7)
            { articleNum = 0; }//at the 8th article, cycle back to 1
            else if (articleNum < 7)
            { articleNum++; }//go to next article
            changeLine.Enabled = true; //start timer
        }
    }
}
