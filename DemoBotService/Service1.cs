using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using BotLibrary;


namespace DemoBotService
{
    public partial class DemoBotService : ServiceBase
    {
        private System.Timers.Timer timer;
        private Bot[] bot;
        Random rand = new Random();
        private int lower = 1, upper = 10;
        private double minTime, maxTime;
        DateTime shiftAM = Convert.ToDateTime("8:00 AM");
        DateTime shiftPM = Convert.ToDateTime("5:00 PM");

        public DemoBotService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();

            int counter = 0;
            string line;
            string path = @"c:/Swiftpage/RunUserProfile.txt";
            if (File.Exists(path))
            {
                StreamReader file = new StreamReader(path);
                LinkedList<string> users = new LinkedList<string>();

                while ((line = file.ReadLine()) != null)
                {
                    users.AddLast(line);
                    counter++;
                }
                string text;
                bot = new Bot[users.Count()];
                char[] delimeterChars = { '|' };
                for (int i = 0; i < users.Count(); i++)
                {
                    text = users.ElementAt(i);
                    string[] words = text.Split(delimeterChars);
                    string userName = words[0];
                    string password = words[1];
                    int reliability = Convert.ToInt32(words[2]);
                    bot[i] = new Bot(userName, password, reliability);
                    bot[i].setStopCommand(false);
                }
                if (DateTime.Compare(DateTime.Now.ToUniversalTime(), shiftAM.ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), shiftPM.ToUniversalTime()) <= 0)
                {
                    timer.Elapsed += new ElapsedEventHandler(runBot);
                    timer.Enabled = true;
                }
                else
                {
                    int hour = 0, minute = 0;
                    if (DateTime.Now.Hour <= shiftAM.Hour && DateTime.Now.Minute <= shiftAM.Minute)
                    {
                        hour = shiftAM.Hour - DateTime.Now.Hour;
                        minute = shiftAM.Minute - DateTime.Now.Minute;
                    }
                    if (DateTime.Now.Hour >= shiftPM.Hour && DateTime.Now.Minute >= shiftPM.Minute)
                    {
                        hour = (24 - DateTime.Now.Hour) + shiftAM.Hour;
                        minute = shiftAM.Minute - DateTime.Now.Minute;
                        if (minute < 0)
                        {
                            hour--;
                            minute = minute + 60;
                        }
                    }
                    double time = (hour * 3600 * 1000) + (minute * 60 * 1000);
                    timer.Interval = time;
                    timer.Elapsed += new ElapsedEventHandler(runBot);
                    timer.Enabled = true;
                }
            }
            else
            {
                Debug.WriteLine("No RunUserProfile.txt found in C:/Swiftpage/");
            }
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            for (int i = 0; i < bot.Count(); i++)
            {
                bot[i] = null;
            }
            timer = null;
        }

        private void runBot(object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < bot.Count(); i++)
            {
                timer.Enabled = false;
                bot[i].Run();
            }
            minTime = lower * 60000;
            maxTime = upper * 60000;
            double time = 300000;
            if (maxTime != 0)
            {
                if (maxTime == minTime)
                {
                    time = maxTime;
                }
                else
                {
                    do
                    {
                        time = maxTime * rand.NextDouble();
                    } while (time < minTime || time == 0);
                }
                timer.Interval = time;
                timer.Enabled = true;
            }
        }
    }
}
