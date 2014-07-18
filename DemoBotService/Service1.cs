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
        private DateTime shiftAM;
        private DateTime shiftPM;
        // Modify AM and PM string values to change when the worker's shift is
        private string AM = "8:00 AM";
        private string PM = "5:00 PM";
        private LinkedList<string> endPoints = new LinkedList<string>();
        private LinkedList<string> companies = new LinkedList<string>();
        private LinkedList<string> users = new LinkedList<string>();
        private LinkedList<bool> fromSpreadSheet = new LinkedList<bool>();
        private int endPointIndex = 0;
        // These are the paths for the configuration files of the service
        private string path = @"c:/Swiftpage/Configuration/RunUserProfile.txt";
        private string threadPath = @"c:/Swiftpage/Configuration/EndPoints.txt";
        private string serverLog = @"c:/Swiftpage/Configuration/ServerLog.txt";
        private string compPath = @"c:/Swiftpage/Configuration/Companies.txt";
        private string spreadSheetPath = @"c:/Swiftpage/Spreadsheets";
        private DateTime previousModification;

        public DemoBotService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Clears the ServerLog.txt file and Companies.txt file (which is written to after reading EndPoints.txt)
            try
            {
                StreamWriter write = new StreamWriter(serverLog);
                write.WriteLine("Starting Server at " + DateTime.Now.ToString());
                write.Close(); 
                StreamWriter writeComp = new StreamWriter(compPath);
                writeComp.Write("");
                writeComp.Close();
            }catch (Exception e)
            {
                System.IO.Directory.CreateDirectory(@"c:\Swiftpage\Configuration");
                StreamWriter write = new StreamWriter(serverLog);
                write.WriteLine("Please enter new values for Endpoints.txt and RunUserProfile.txt " + DateTime.Now.ToString());
                write.Close();
                write = new StreamWriter(compPath);
                write.Write("");
                write.Close();
                write = new StreamWriter(path);
                write.Write("user|password|reliability");
                write.Close();
                write = new StreamWriter(threadPath);
                write.Write("Company Name|Endpoint|Read from spreasheet? (true/false)");
                write.Close();
                System.IO.Directory.CreateDirectory(spreadSheetPath);
            }
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
                        {
                            Log("User exists: " + line, serverLog);
                            doesExist = true;
                        }
                    }
                    if (string.Compare(line, "user|password|reliability") == 0)
                        doesExist = true;
                    if (!doesExist)
                    {
                        users.AddLast(line);
                    }
                }
                Log("File read", serverLog);
                string text;
                char[] delimeterChars = { '|' };
                Log("Making " + (endPoints.Count() * users.Count()) + " Bots", serverLog);
                for (int j = endPointIndex; j < endPoints.Count(); j++)
                {
                    int spreadCounter = 0;
                    for (int i = 0; i < users.Count(); i++)
                    {
                        text = users.ElementAt(i);
                        string[] words = text.Split(delimeterChars);
                        string userName = words[0];
                        string password = words[1];
                        int reliability = Convert.ToInt32(words[2]);
                        Bot tempBot;
                        //if (fromSpreadSheet.ElementAt(j))
                        //{
                            //tempBot = new SpreadSheetBot(userName, password, reliability, endPoints.ElementAt(j), j, companies.ElementAt(j));
                            //spreadCounter++;
                        //}
                        //else
                        //{
                            tempBot = new Bot(userName, password, reliability, endPoints.ElementAt(j), j);
                        //}
                        tempBot.setStopCommand(false);
                        bot.AddLast(tempBot);
                        counter++;
                    }
                    Log("Made " + spreadCounter + " spreadsheet bots for company " + companies.ElementAt(j), serverLog);
                    endPointIndex++;
                }
                Log("Made " + counter + " bots", serverLog);
                file.Close();
            }
            else
            {
                Log("No RunUserProfile.txt found in C:/Swiftpage/Configuration", serverLog);
                Debug.WriteLine("No RunUserProfile.txt found in C:/Swiftpage/Configuration");
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
                            Log("Endpoint exists: " + words[1], serverLog);
                            doesExist = true;
                        }
                    }
                    if (string.Compare(words[1], "Endpoint") == 0)
                        doesExist = true;
                    if (!doesExist)
                    {
                        writeFile.WriteLine(words[0]);
                        companies.AddLast(words[0]);
                        endPoints.AddLast(words[1]);
                        //if (string.Compare(words[2].ToLower(), "true") == 0 || string.Compare(words[2].ToLower(), "y") == 0 || string.Compare(words[2].ToLower(), "yes") == 0)
                            //fromSpreadSheet.AddLast(true);
                        //else
                            //fromSpreadSheet.AddLast(false);
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
                        //endPointIndex--;
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
                Log("No EndPoints.txt found in C:/Swiftpage/Configuration", serverLog);
                Debug.WriteLine("No EndPoints.txt found in C:/Swiftpage/");
            }
        }

        // This function is only called once when the program initially starts, its algorithms are used again within the RunBots function
        // (not very well planned by me) but that is how it was created.
        public void setTimer()
        {
            updateShift();
            timer = new System.Timers.Timer();
            /*if (DateTime.Compare(DateTime.Now.ToUniversalTime(), shiftAM.ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), shiftPM.ToUniversalTime()) <= 0)
            {
                timer.Elapsed += new ElapsedEventHandler(runBot);
                timer.Enabled = true;
            }
            else
            {
                int hour = 0, minute = 0;
                if (DateTime.Now.Hour <= shiftAM.Hour)
                {
                    if (DateTime.Now.Minute <= shiftAM.Minute)
                    {
                        hour = shiftAM.Hour - DateTime.Now.Hour;
                        minute = shiftAM.Minute - DateTime.Now.Minute;
                    }
                    else
                    {
                        hour = shiftAM.Hour - (DateTime.Now.Hour + 1);
                        minute = 60 - DateTime.Now.Minute;
                    }
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
                Log("Waiting " + time / 1000 + " seconds", serverLog); */
                timer.Elapsed += new ElapsedEventHandler(runBot);
                timer.Enabled = true;
            //}
        }

        // This function will loop through all of the bots located in the bot array and call the Run function. It first checks to ensure that the time
        // is correct for the bot to run (i.e. it is between the shiftAM time and shiftPM and is a weekday [based on the local machine time])
        public void runBot(object source, ElapsedEventArgs z)
        {
            // Update the shift in order to account for day changes
            updateShift();
            timer.Enabled = false;
            if (DateTime.Compare(File.GetLastWriteTimeUtc(threadPath), previousModification.ToUniversalTime()) > 0 || DateTime.Compare(File.GetLastWriteTimeUtc(path), previousModification.ToUniversalTime()) > 0)
            {
                readEndpoints(threadPath);
            }
            int day = (int)DateTime.Now.DayOfWeek;

            // Day 0 is Sunday and day 6 is Saturday
            if (day > 0 && day < 6)
            {
                if (DateTime.Compare(DateTime.Now.ToUniversalTime(), shiftAM.ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), shiftPM.ToUniversalTime()) <= 0)
                {
                    int counter = 0;
                    if (bot.Count > 0)
                    {
                        Log("Running " + bot.Count + " Bots at: " + DateTime.Now.ToString(), serverLog);
                        for (int i = 0; i < bot.Count; i++)
                        {
                            try
                            {
                                bot.ElementAt(i).Run();
                                counter++;
                            }
                            catch (Exception e)
                            {
                                bot.ElementAt(i).LogException(e);
                            }
                        }
                        Log("Finished Running " + counter + " Bots of " + bot.Count, serverLog);
                    }
                    /*
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
                    } */
                    double time = 10000;
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
                        hour = 24 - DateTime.Now.Hour + shiftAM.Hour;
                        minute = shiftAM.Minute - DateTime.Now.Minute;
                        if (minute < 0)
                        {
                            hour--;
                            minute = minute + 60;
                        }
                    }
                    double time = (hour * 3600 * 1000) + (minute * 60 * 1000);
                    timer.Interval = time;
                    Log("Waiting " + time / 1000 + " seconds", serverLog);
                    timer.Enabled = true;
                }
            }
            else
            {
                int hour = 0;
                int minute = 0;
                double time;
                if (day == 6) // Saturday
                {
                    if (DateTime.Now.Hour <= shiftAM.Hour && DateTime.Now.Minute <= shiftAM.Minute)
                    {
                        hour = 24 + shiftAM.Hour - DateTime.Now.Hour;
                        minute = shiftAM.Minute - DateTime.Now.Minute;
                    }
                    if (DateTime.Now.Hour >= shiftPM.Hour && DateTime.Now.Minute >= shiftPM.Minute)
                    {
                        hour = 48 - DateTime.Now.Hour + shiftAM.Hour;
                        minute = shiftAM.Minute - DateTime.Now.Minute;
                        if (minute < 0)
                        {
                            hour--;
                            minute = minute + 60;
                        }
                    }
                }
                else // day == 0 or Sunday
                {
                    if (DateTime.Now.Hour <= shiftAM.Hour && DateTime.Now.Minute <= shiftAM.Minute)
                    {
                        hour = shiftAM.Hour - DateTime.Now.Hour;
                        minute = shiftAM.Minute - DateTime.Now.Minute;
                    }
                    if (DateTime.Now.Hour >= shiftPM.Hour && DateTime.Now.Minute >= shiftPM.Minute)
                    {
                        hour = 24 - DateTime.Now.Hour + shiftAM.Hour;
                        minute = shiftAM.Minute - DateTime.Now.Minute;
                        if (minute < 0)
                        {
                            hour--;
                            minute = minute + 60;
                        }
                    }
                }
                time = (hour * 3600 * 1000) + (minute * 60 * 1000);
                timer.Interval = time;
                Log("Waiting " + time / 1000 + " seconds", serverLog);
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

        public void updateShift()
        {
            shiftAM = Convert.ToDateTime(AM);
            shiftPM = Convert.ToDateTime(PM);
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
