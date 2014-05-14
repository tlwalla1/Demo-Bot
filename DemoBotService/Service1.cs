using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using System.IO;
using BotLibrary;


namespace DemoBotService
{
    public partial class DemoBotService : ServiceBase
    {
        private System.Timers.Timer timer;
        private LinkedList<Bot> bot = new LinkedList<Bot>();
        private Random rand = new Random();
        private int lower = 1, upper = 10;
        private double minTime, maxTime;
        private DateTime shiftAM = Convert.ToDateTime("8:00 AM");
        private DateTime shiftPM = Convert.ToDateTime("5:00 PM");
        private LinkedList<string> endPoints = new LinkedList<string>();
        private LinkedList<string> users = new LinkedList<string>();
        private int endPointIndex = 0;
        // These are the paths for the configuration files of the service
        private string path = @"c:/Swiftpage/RunUserProfile.txt";
        private string threadPath = @"c:/Swiftpage/EndPoints.txt";
        private string serverLog = @"c:/Swiftpage/ServerLog.txt";
        private string compPath = @"c:/Swiftpage/Companies.txt";
        private DateTime previousModification;

        public DemoBotService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Clears the ServerLog.txt file
            StreamWriter write = new StreamWriter(serverLog);
            StreamWriter writeComp = new StreamWriter(compPath);
            write.WriteLine("Starting Server at " + DateTime.Now.ToString());
            write.Close();
            writeComp.Write("");
            writeComp.Close();
            readEndpoints(threadPath);
            setTimer();
        }

        protected override void OnStop()
        {
            stopAll();
        }

        private void readUsers()
        {
            string line;
            int counter = 0;
            if (File.Exists(path))
            {
                StreamReader file = new StreamReader(path);
                Boolean doesExist = false;
                Log("Reading file", serverLog);
                while ((line = file.ReadLine()) != null)
                {
                    doesExist = false;
                    for (int i = 0; i < users.Count; i++)
                    {
                        if (string.Compare(users.ElementAt(i), line) == 0)
                            doesExist = true;
                    }
                    if (!doesExist)
                    {
                        users.AddLast(line);
                    }
                }
                Log("File read", serverLog);
                string text;
                char[] delimeterChars = { '|' };
                Log("Making Bots", serverLog);
                for (int j = endPointIndex; j < endPoints.Count(); j++)
                {
                    for (int i = 0; i < users.Count(); i++)
                    {
                        text = users.ElementAt(i);
                        string[] words = text.Split(delimeterChars);
                        string userName = words[0];
                        string password = words[1];
                        int reliability = Convert.ToInt32(words[2]);
                        Bot tempBot = new Bot(userName, password, reliability, endPoints.ElementAt(j), j);
                        tempBot.setStopCommand(false);
                        bot.AddLast(tempBot);
                        counter++;
                    }
                    endPointIndex++;
                }
                Log("Made " + counter + " bots", serverLog);
                file.Close();
            }
            else
            {
                Log("No RunUserProfile.txt found in C:/Swiftpage/", serverLog);
                Debug.WriteLine("No RunUserProfile.txt found in C:/Swiftpage/");
            }
        }

        public void readEndpoints(string tpath)
        {
            string line;
            if (File.Exists(tpath))
            {
                // Modify value for previous modification to current reading
                previousModification = DateTime.Now;
                StreamReader file = new StreamReader(tpath);
                StreamWriter writeFile = new StreamWriter(compPath, true);
                Boolean doesExist = false;
                LinkedList<string> temp = new LinkedList<string>();

                // Set the delimeter
                char[] delimeterChars = { '|' };

                Log("Reading end points", serverLog);
                // Loop through the file reading in the company name first then the endpoint of that company
                while ((line = file.ReadLine()) != null)
                {
                    doesExist = false;
                    // Write out the companies to the Companies.txt file for Evaluation use
                    string[] words = line.Split(delimeterChars);
                    temp.AddLast(words[1]);
                    for (int i = 0; i < endPoints.Count; i++)
                    {
                        if (string.Compare(endPoints.ElementAt(i), words[1]) == 0)
                        {
                            doesExist = true;
                        }
                    }
                    if (!doesExist)
                    {
                        writeFile.WriteLine(words[0]);
                        endPoints.AddLast(words[1]);
                    }
                }
                for(int j = 0; j < endPoints.Count; j++)
                {
                    Boolean exists = true;
                    int i = 0;
                    for (i = 0; i < temp.Count; i++)
                    {
                        if (string.Compare(temp.ElementAt(i), endPoints.ElementAt(j)) == 0)
                            break;
                    }
                    if (i == temp.Count)
                        exists = false;
                    if (!exists)
                    {
                        string endPoint = endPoints.ElementAt(j);
                        Log("Removing endpoint : " + endPoints.ElementAt(j) + " from service", serverLog);
                        endPoints.Remove(endPoint);
                        Log("Removing bots from service", serverLog);
                        removeBots(endPoint);
                    }
                }
                Log("End points read", serverLog);
                file.Close();
                writeFile.Close();
                Log("Reading from RunUserProfile", serverLog);
                readUsers();
                Log("Set up users and endpoints", serverLog);
            }
            else
            {
                Log("No EndPoints.txt found in C:/Swiftpage/", serverLog);
                Debug.WriteLine("No EndPoints.txt found in C:/Swiftpage/");
            }
        }

        public void setTimer()
        {
            timer = new System.Timers.Timer();
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
                Log("Waiting " + time / 1000 + "seconds", serverLog);
                timer.Elapsed += new ElapsedEventHandler(runBot);
                timer.Enabled = true;
            }
        }

        public void runBot(object source, ElapsedEventArgs z)
        {
            if (DateTime.Compare(File.GetLastWriteTimeUtc(threadPath), previousModification.ToUniversalTime()) > 0 || DateTime.Compare(File.GetLastWriteTimeUtc(path), previousModification.ToUniversalTime()) > 0)
            {
                readEndpoints(threadPath);
            }
            if (DateTime.Compare(DateTime.Now.ToUniversalTime(), shiftAM.ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), shiftPM.ToUniversalTime()) <= 0)
            {
                if (bot.Count > 0)
                {
                    timer.Enabled = false;
                    Log("Running Bots", serverLog);
                    for (int i = 0; i < bot.Count; i++)
                    {
                        bot.ElementAt(i).Run();
                    }
                    Log("Finished Running " + bot.Count + " Bots", serverLog);
                }
                minTime = lower * 60000;
                maxTime = upper * 60000;
                double time = 300000;
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
                Log("Waiting " + time / 1000 + " seconds", serverLog);
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
                Log("Waiting " + time / 1000 + "seconds", serverLog);
                timer.Enabled = true;
            }
            
        }

        public void removeBots(string endPoint)
        {
            int counter = 0;
            for (int i = 0; i < bot.Count; i++)
            {
                if (string.Compare(bot.ElementAt(i).getEndPoint(),endPoint) == 0)
                {
                    bot.ElementAt(i).LogRemoval();
                    bot.Remove(bot.ElementAt(i));
                    counter++;
                }
            }
            Log("Deleted " + counter + " bots with endPoint: " + endPoint, serverLog);
        }

        public void stopAll()
        {
            bot.Clear();
        }

        public static void Log(string message, string filename)
        {
            StreamWriter write = new StreamWriter(filename, true);
            write.WriteLine(message);
            write.Close();
        }
    }
}
