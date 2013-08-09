using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Sage.SData.Client.Core;
using Sage.SData.Client.Framework;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Demo_Bot;

namespace BotExecutable
{
    public partial class AutoBot : Form
    {
        Bot robot, robot2, robot3, robot4, robot5;
        private System.Timers.Timer timer;
        private System.Timers.Timer timer2;
        private System.Timers.Timer timer3;
        private System.Timers.Timer timer4;
        private System.Timers.Timer timer5;
        private double minTime, maxTime;
        private double minTime2, maxTime2;
        private double minTime3, maxTime3;
        private double minTime4, maxTime4;
        private double minTime5, maxTime5;
        private int lower, upper;
        private int lower2, upper2;
        private int lower3, upper3;
        private int lower4, upper4;
        private int lower5, upper5;
        Random rand = new Random();

        public AutoBot()
        {
            InitializeComponent();
        }

        private void runBot(object source, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            // Checks whether button2 was pressed by the user, determining whether or not to continue running the bot.
            if (robot.getStopCommand() == true)
            {
                timer.Dispose();
                timer.Enabled = false;
                return;
            }
            robot.Run();

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
                time = time / 1000;
                string temp = time.ToString("G4");
                SetText("Waiting... " + temp + " seconds");
                timer.Enabled = true;
            }
        }

        private void runBot2(object source, ElapsedEventArgs e)
        {
            timer2.Enabled = false;
            // Checks whether button2 was pressed by the user, determining whether or not to continue running to bot.
            if (robot2.getStopCommand() == true)
            {
                timer2.Dispose();
                timer2.Enabled = false;
                return;
            }
            robot2.Run();

            minTime2 = lower2 * 60000;
            maxTime2 = upper2 * 60000;
            double time2 = 300000;
            if (maxTime2 != 0)
            {
                if (maxTime2 == minTime2)
                {
                    time2 = maxTime2;
                }
                else
                {
                    do
                    {
                        time2 = maxTime2 * rand.NextDouble();
                    } while (time2 < minTime2 || time2 == 0);
                }
                timer2.Interval = time2;
                time2 = time2 / 1000;
                string temp2 = time2.ToString("G4");
                SetText2("Waiting... " + temp2 + " seconds");
                timer2.Enabled = true;
            }
        }

        private void runBot3(object source, ElapsedEventArgs e)
        {
            timer3.Enabled = false;
            // Checks whether button2 was pressed by the user, determining whether or not to continue running to bot.
            if (robot3.getStopCommand() == true)
            {
                timer3.Dispose();
                timer3.Enabled = false;
                return;
            }
            robot3.Run();

            minTime3 = lower3 * 60000;
            maxTime3 = upper3 * 60000;
            double time3 = 300000;
            if (maxTime3 != 0)
            {
                if (maxTime3 == minTime3)
                {
                    time3 = maxTime3;
                }
                else
                {
                    do
                    {
                        time3 = maxTime3 * rand.NextDouble();
                    } while (time3 < minTime3 || time3 == 0);
                }
                timer3.Interval = time3;
                time3 = time3 / 1000;
                string temp3 = time3.ToString("G4");
                SetText3("Waiting... " + temp3 + " seconds");
                timer3.Enabled = true;
            }
        }

        private void runBot4(object source, ElapsedEventArgs e)
        {
            timer4.Enabled = false;
            // Checks whether button2 was pressed by the user, determining whether or not to continue running to bot.
            if (robot4.getStopCommand() == true)
            {
                timer4.Dispose();
                timer4.Enabled = false;
                return;
            }
            robot4.Run();

            minTime4 = lower4 * 60000;
            maxTime4 = upper4 * 60000;
            double time4 = 300000;
            if (maxTime4 != 0)
            {
                if (maxTime4 == minTime4)
                {
                    time4 = maxTime4;
                }
                else
                {
                    do
                    {
                        time4 = maxTime4 * rand.NextDouble();
                    } while (time4 < minTime4 || time4 == 0);
                }
                timer4.Interval = time4;
                time4 = time4 / 1000;
                string temp4 = time4.ToString("G4");
                SetText4("Waiting... " + temp4 + " seconds");
                timer4.Enabled = true;
            }
        }

        private void runBot5(object source, ElapsedEventArgs e)
        {
            timer5.Enabled = false;
            // Checks whether button2 was pressed by the user, determining whether or not to continue running to bot.
            if (robot5.getStopCommand() == true)
            {
                timer5.Dispose();
                timer5.Enabled = false;
                return;
            }
            robot5.Run();

            minTime5 = lower5 * 60000;
            maxTime5 = upper5 * 60000;
            minTime5 = lower5 * 60000;
            maxTime5 = upper5 * 60000;
            double time5 = 300000;
            if (maxTime5 != 0)
            {
                if (maxTime5 == minTime5)
                {
                     time5 = maxTime5;
                }
                else
                {
                     do
                       {
                           time5 = maxTime5 * rand.NextDouble();
                       } while (time5 < minTime5 || time5 == 0);
                }
                timer5.Interval = time5;
                time5 = time5 / 1000;
                string temp5 = time5.ToString("G4");
                SetText5("Waiting... " + temp5 + " seconds");
                timer5.Enabled = true;
            }
        }

        // button1 is Run Bot
        private void button1_Click(object sender, EventArgs e)
        {
            string client = serverAddress.Text;
            Random rand = new Random();

            switch(botNumber.SelectedIndex)
            {
                case 4:
                    if (shiftAM5.Text == "")
                        shiftAM5.Text = "7:30";
                    if (shiftPM5.Text == "")
                        shiftPM5.Text = "5:30";

                    if (DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftAM5.Text + "AM") ).ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftPM5.Text + "PM")).ToUniversalTime()) <= 0)
                    {
                        userLabel5.Text = "User: " + userIDBox5.Text;
                        robot5 = new Bot(client, shiftAM5.Text, shiftPM5.Text, userIDBox5.Text, passwordBox5.Text, progressLabel5, activitiesCreated5, notesCreated5, activitiesCompleted5, leadsCreated5, accountsCreated5, contactsCreated5, oppsCreated5, ticketsCreated5, oppsUpdated5, leadsPromoted5, role5, activityCompleteAmount5.Value, roleSelector5, noteCheckBox5.Checked, activityCheckBox5.Checked, leadCheckBox5.Checked, accountCheckBox5.Checked, contactCheckBox5.Checked, oppCheckBox5.Checked, ticketCheckBox5.Checked, oppUpdateCheckBox5.Checked, completeActCheckBox5.Checked, promoteLeadCheckBox5.Checked, reliabilityValue5.Value, creationUpperBound.Text);
                        robot5.setStopCommand(false);
                        lower5 = Convert.ToInt32(lowerBound5.Value);
                        upper5 = Convert.ToInt32(upperBound5.Value);
                        timer5 = new System.Timers.Timer();
                        timer5.Elapsed += new ElapsedEventHandler(runBot5);
                        timer5.Enabled = true;
                    }
                    else
                    {
                        DateTime morning = Convert.ToDateTime(shiftAM5.Text + "AM");
                        DateTime afternoon = Convert.ToDateTime(shiftPM5.Text + "PM");
                        int hour = 0, minute = 0;
                        if (DateTime.Now.Hour < morning.Hour && DateTime.Now.Minute < morning.Minute)
                        {
                            hour = morning.Hour - DateTime.Now.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        if (DateTime.Now.Hour > afternoon.Hour && DateTime.Now.Minute > afternoon.Minute)
                        {
                            hour = (24 - DateTime.Now.Hour) + morning.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        double time5 = (hour * 3600 * 100) + (minute * 60 * 100);
                        timer5.Interval = time5;
                        time5 = time5 / 1000;
                        string temp5 = time5.ToString("G4");
                        SetText5("Waiting... " + temp5 + " seconds");
                        timer5.Enabled = true;
                    }
                    goto case 3;
                case 3:
                    if (shiftAM4.Text == "")
                        shiftAM4.Text = "7:30";
                    if (shiftPM4.Text == "")
                        shiftPM4.Text = "5:30";

                    if (DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftAM4.Text + "AM")).ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftPM4.Text + "PM")).ToUniversalTime()) <= 0)
                    {
                        userLabel4.Text = "User: " + userIDBox4.Text;
                        robot4 = new Bot(client, shiftAM4.Text, shiftPM4.Text, userIDBox4.Text, passwordBox4.Text, progressLabel4, activitiesCreated4, notesCreated4, activitiesCompleted4, leadsCreated4, accountsCreated4, contactsCreated4, oppsCreated4, ticketsCreated4, oppsUpdated4, leadsPromoted4, role4, activityCompleteAmount4.Value, roleSelector4, noteCheckBox4.Checked, activityCheckBox4.Checked, leadCheckBox4.Checked, accountCheckBox4.Checked, contactCheckBox4.Checked, oppCheckBox4.Checked, ticketCheckBox4.Checked, oppUpdateCheckBox4.Checked, completeActCheckBox4.Checked, promoteLeadCheckBox4.Checked, reliabilityValue4.Value, creationUpperBound.Text);
                        robot4.setStopCommand(false);
                        lower4 = Convert.ToInt32(lowerBound4.Value);
                        upper4 = Convert.ToInt32(upperBound4.Value);
                        timer4 = new System.Timers.Timer();
                        timer4.Elapsed += new ElapsedEventHandler(runBot4);
                        timer4.Enabled = true;
                    }
                    else
                    {
                        DateTime morning = Convert.ToDateTime(shiftAM4.Text + "AM");
                        DateTime afternoon = Convert.ToDateTime(shiftPM4.Text + "PM");
                        int hour = 0, minute = 0;
                        if (DateTime.Now.Hour < morning.Hour && DateTime.Now.Minute < morning.Minute)
                        {
                            hour = morning.Hour - DateTime.Now.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        if (DateTime.Now.Hour > afternoon.Hour && DateTime.Now.Minute > afternoon.Minute)
                        {
                            hour = (24 - DateTime.Now.Hour) + morning.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        double time4 = (hour * 3600 * 100) + (minute * 60 * 100);
                        timer4.Interval = time4;
                        time4 = time4 / 1000;
                        string temp4 = time4.ToString("G4");
                        SetText4("Waiting... " + temp4 + " seconds");
                        timer4.Enabled = true;
                    }
                    goto case 2;
                case 2:
                    if (shiftAM3.Text == "")
                        shiftAM3.Text = "7:30";
                    if (shiftPM3.Text == "")
                        shiftPM3.Text = "5:30";

                    if (DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftAM3.Text + "AM")).ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftPM3.Text + "PM")).ToUniversalTime()) <= 0)
                    {
                        userLabel3.Text = "User: " + userIDBox3.Text;
                        robot3 = new Bot(client, shiftAM3.Text, shiftPM3.Text, userIDBox3.Text, passwordBox3.Text, progressLabel3, activitiesCreated3, notesCreated3, activitiesCompleted3, leadsCreated3, accountsCreated3, contactsCreated3, oppsCreated3, ticketsCreated3, oppsUpdated3, leadsPromoted3, role3, activityCompleteAmount3.Value, roleSelector3, noteCheckBox3.Checked, activityCheckBox3.Checked, leadCheckBox3.Checked, accountCheckBox3.Checked, contactCheckBox3.Checked, oppCheckBox3.Checked, ticketCheckBox3.Checked, oppUpdateCheckBox3.Checked, completeActCheckBox3.Checked, promoteLeadCheckBox3.Checked, reliabilityValue3.Value, creationUpperBound.Text);
                        robot3.setStopCommand(false);
                        lower3 = Convert.ToInt32(lowerBound3.Value);
                        upper3 = Convert.ToInt32(upperBound3.Value);
                        timer3 = new System.Timers.Timer();
                        timer3.Elapsed += new ElapsedEventHandler(runBot3);
                        timer3.Enabled = true;
                    }
                    else
                    {
                        DateTime morning = Convert.ToDateTime(shiftAM3.Text + "AM");
                        DateTime afternoon = Convert.ToDateTime(shiftPM3.Text + "PM");
                        int hour = 0, minute = 0;
                        if (DateTime.Now.Hour < morning.Hour && DateTime.Now.Minute < morning.Minute)
                        {
                            hour = morning.Hour - DateTime.Now.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        if (DateTime.Now.Hour > afternoon.Hour && DateTime.Now.Minute > afternoon.Minute)
                        {
                            hour = (24 - DateTime.Now.Hour) + morning.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        double time3 = (hour * 3600 * 100) + (minute * 60 * 100);
                        timer3.Interval = time3;
                        time3 = time3 / 1000;
                        string temp3 = time3.ToString("G4");
                        SetText3("Waiting... " + temp3 + " seconds");
                        timer3.Enabled = true;
                    }
                    goto case 1;
                case 1:
                    if (shiftAM2.Text == "")
                        shiftAM2.Text = "7:30";
                    if (shiftPM2.Text == "")
                        shiftPM2.Text = "5:30";

                    if (DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftAM2.Text + "AM")).ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftPM2.Text + "PM")).ToUniversalTime()) <= 0)
                    {
                        userLabel2.Text = "User: " + userIDBox2.Text;
                        robot2 = new Bot(client, shiftAM2.Text, shiftPM2.Text, userIDBox2.Text, passwordBox2.Text, progressLabel2, activitiesCreated2, notesCreated2, activitiesCompleted2, leadsCreated2, accountsCreated2, contactsCreated2, oppsCreated2, ticketsCreated2, oppsUpdated2, leadsPromoted2, role2, activityCompleteAmount2.Value, roleSelector2, noteCheckBox2.Checked, activityCheckBox2.Checked, leadCheckBox2.Checked, accountCheckBox2.Checked, contactCheckBox2.Checked, oppCheckBox2.Checked, ticketCheckBox2.Checked, oppUpdateCheckBox2.Checked, completeActCheckBox2.Checked, promoteLeadCheckBox2.Checked, reliabilityValue2.Value, creationUpperBound.Text);
                        robot2.setStopCommand(false);
                        lower2 = Convert.ToInt32(lowerBound2.Value);
                        upper2 = Convert.ToInt32(upperBound2.Value);
                        timer2 = new System.Timers.Timer();
                        timer2.Elapsed += new ElapsedEventHandler(runBot2);
                        timer2.Enabled = true;
                    }
                    else
                    {
                        DateTime morning = Convert.ToDateTime(shiftAM2.Text + "AM");
                        DateTime afternoon = Convert.ToDateTime(shiftPM2.Text + "PM");
                        int hour = 0, minute = 0;
                        if (DateTime.Now.Hour < morning.Hour && DateTime.Now.Minute < morning.Minute)
                        {
                            hour = morning.Hour - DateTime.Now.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        if (DateTime.Now.Hour > afternoon.Hour && DateTime.Now.Minute > afternoon.Minute)
                        {
                            hour = (24 - DateTime.Now.Hour) + morning.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        double time2 = (hour * 3600 * 100) + (minute * 60 * 100);
                        timer2.Interval = time2;
                        time2 = time2 / 1000;
                        string temp2 = time2.ToString("G4");
                        SetText2("Waiting... " + temp2 + " seconds");
                        timer2.Enabled = true;
                    }
                    goto case 0;
                case 0:
                    if (shiftAM.Text == "")
                        shiftAM.Text = "7:30";
                    if (shiftPM.Text == "")
                        shiftPM.Text = "5:30";

                    if (DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftAM.Text + "AM")).ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), (Convert.ToDateTime(shiftPM.Text + "PM")).ToUniversalTime()) <= 0)
                    {
                        userLabel.Text = "User: " + userIDBox.Text;
                        robot = new Bot(client, shiftAM.Text, shiftPM.Text, userIDBox.Text, passwordBox.Text, progressLabel, activitiesCreated, notesCreated, activitiesCompleted, leadsCreated, accountsCreated, contactsCreated, oppsCreated, ticketsCreated, oppsUpdated, leadsPromoted, role, activityCompleteAmount.Value, roleSelector, noteCheckBox.Checked, activityCheckBox.Checked, leadCheckBox.Checked, accountCheckBox.Checked, contactCheckBox.Checked, oppCheckBox.Checked, ticketCheckBox.Checked, oppUpdateCheckBox.Checked, completeActCheckBox.Checked, promoteLeadCheckBox.Checked, reliabilityValue.Value, creationUpperBound.Text);
                        robot.setStopCommand(false);
                        lower = Convert.ToInt32(lowerBound.Value);
                        upper = Convert.ToInt32(upperBound.Value);
                        timer = new System.Timers.Timer();
                        timer.Elapsed += new ElapsedEventHandler(runBot);
                        timer.Enabled = true;
                    }
                    else
                    {
                        DateTime morning = Convert.ToDateTime(shiftAM.Text + "AM");
                        DateTime afternoon = Convert.ToDateTime(shiftPM.Text + "PM");
                        int hour = 0, minute = 0;
                        if (DateTime.Now.Hour < morning.Hour && DateTime.Now.Minute < morning.Minute)
                        {
                            hour = morning.Hour - DateTime.Now.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                        }
                        if (DateTime.Now.Hour > afternoon.Hour && DateTime.Now.Minute > afternoon.Minute)
                        {
                            hour = (24 - DateTime.Now.Hour) + morning.Hour;
                            minute = morning.Minute - DateTime.Now.Minute;
                            if (minute < 0)
                            {
                                hour--;
                                minute = minute * -1;
                            }
                        }
                        double time = (hour * 3600 * 100) + (minute * 60 * 100);
                        timer.Interval = time;
                        time = time / 1000;
                        string temp = time.ToString("G4");
                        SetText("Waiting... " + temp + " seconds");
                        timer.Enabled = true;
                    }
                    break; 
            } 
        }

        // button2 is Stop Bot
        private void button2_Click(object sender, EventArgs e)
        {
            if (robot != null)
            {
                robot.stop();
                robot.remove();
                SetText("Progress:");
            }
            if (robot2 != null)
            {
                robot2.stop();
                robot2.remove();
                SetText2("Progress:");
            }
            if (robot3 != null)
            {
                robot3.stop();
                robot3.remove();
                SetText3("Progress:");
            }
            if (robot4 != null)
            {
                robot4.stop();
                robot4.remove();
                SetText4("Progress:");
            }
            if (robot5 != null)
            {
                robot5.stop();
                robot5.remove();
                SetText5("Progress:");
            }
        }

        private void dataRefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                activitiesCreated.Text = "0";
                notesCreated.Text = "0";
                activitiesCompleted.Text = "0";
                robot.setActivitiesCount(0);
                robot.setNotesCount(0);
                robot.setActivitiesCompleteCount(0);
                robot.setLeadsCount(0);
                robot.setOpportunityCount(0);
                robot.setTicketsCount(0);
            }
            catch (Exception)
            {
                progressLabel.ForeColor = Color.Crimson;
                SetText("Please run the bot first.");
            }
        }

        #region Threading
        delegate void SetTextCallback(string text);

        
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.progressLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.progressLabel.Invoke(d, new object[] { text });
            }
            else
            {
                this.progressLabel.Text = text;
            }
        }

        private void SetText2(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.progressLabel2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText2);
                this.progressLabel2.Invoke(d, new object[] { text });
            }
            else
            {
                this.progressLabel2.Text = text;
            }
        }

        private void SetText3(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.progressLabel3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText3);
                this.progressLabel3.Invoke(d, new object[] { text });
            }
            else
            {
                this.progressLabel3.Text = text;
            }
        }

        private void SetText4(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.progressLabel4.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText4);
                this.progressLabel4.Invoke(d, new object[] { text });
            }
            else
            {
                this.progressLabel4.Text = text;
            }
        }

        private void SetText5(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.progressLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText5);
                this.progressLabel5.Invoke(d, new object[] { text });
            }
            else
            {
                this.progressLabel5.Text = text;
            }
        }

        private void SetRole(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.role.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetRole);
                this.role.Invoke(d, new object[] { text });
            }
            else
            {
                this.role.Text = text;
            }
        }

        private void SetRole2(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.role2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetRole2);
                this.role2.Invoke(d, new object[] { text });
            }
            else
            {
                this.role2.Text = text;
            }
        }

        private void SetRole3(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.role3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetRole3);
                this.role3.Invoke(d, new object[] { text });
            }
            else
            {
                this.role3.Text = text;
            }
        }

        private void SetRole4(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.role4.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetRole4);
                this.role4.Invoke(d, new object[] { text });
            }
            else
            {
                this.role4.Text = text;
            }
        }

        private void SetRole5(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.role5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetRole5);
                this.role5.Invoke(d, new object[] { text });
            }
            else
            {
                this.role5.Text = text;
            }
        }

        private void SetLabelText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.notesCreatedLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelText);
                this.notesCreatedLabel.Invoke(d, new object[] { text });
            }
            else
            {
                this.notesCreatedLabel.Text = text;
            }
        }

        private void SetLabelText2(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.notesCreatedLabel2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelText2);
                this.notesCreatedLabel2.Invoke(d, new object[] { text });
            }
            else
            {
                this.notesCreatedLabel2.Text = text;
            }
        }

        private void SetLabelText3(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.notesCreatedLabel3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelText3);
                this.notesCreatedLabel3.Invoke(d, new object[] { text });
            }
            else
            {
                this.notesCreatedLabel3.Text = text;
            }
        }

        private void SetLabelText4(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.notesCreatedLabel4.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelText4);
                this.notesCreatedLabel4.Invoke(d, new object[] { text });
            }
            else
            {
                this.notesCreatedLabel4.Text = text;
            }
        }

        private void SetLabelText5(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.notesCreatedLabel5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelText5);
                this.notesCreatedLabel5.Invoke(d, new object[] { text });
            }
            else
            {
                this.notesCreatedLabel5.Text = text;
            }
        }

        private void SetActivitiesCreatedText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCreatedLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCreatedText);
                this.activitiesCreatedLabel.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCreatedLabel.Text = text;
            }
        }

        private void SetActivitiesCreatedText2(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCreatedLabel2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCreatedText2);
                this.activitiesCreatedLabel2.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCreatedLabel2.Text = text;
            }
        }

        private void SetActivitiesCreatedText3(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCreatedLabel3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCreatedText3);
                this.activitiesCreatedLabel3.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCreatedLabel3.Text = text;
            }
        }

        private void SetActivitiesCreatedText4(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCreatedLabel4.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCreatedText4);
                this.activitiesCreatedLabel4.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCreatedLabel4.Text = text;
            }
        }

        private void SetActivitiesCreatedText5(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCreatedLabel5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCreatedText5);
                this.activitiesCreatedLabel5.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCreatedLabel5.Text = text;
            }
        }

        private void SetActivitiesCompletedText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCompletedLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCompletedText);
                this.activitiesCompletedLabel.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCompletedLabel.Text = text;
            }
        }

        private void SetActivitiesCompletedText2(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCompletedLabel2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCompletedText2);
                this.activitiesCompletedLabel2.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCompletedLabel2.Text = text;
            }
        }

        private void SetActivitiesCompletedText3(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCompletedLabel3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCompletedText3);
                this.activitiesCompletedLabel3.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCompletedLabel3.Text = text;
            }
        }

        private void SetActivitiesCompletedText4(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCompletedLabel4.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCompletedText4);
                this.activitiesCompletedLabel4.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCompletedLabel4.Text = text;
            }
        }

        private void SetActivitiesCompletedText5(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.activitiesCompletedLabel5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCompletedText5);
                this.activitiesCompletedLabel5.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCompletedLabel5.Text = text;
            }
        }

        private void botNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region VisibilityChanges
            switch (botNumber.SelectedIndex)
            {
                case 0:
                    userIDBox.Visible = true;
                    userID.Visible = true;
                    passwordBox.Visible = true;
                    password.Visible = true;
                    //roleSelector.Visible = true;
                    //roleDropDownLabel.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    lowerBound.Visible = true;
                    upperBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    activityCompleteAmountLabel.Visible = true;
                    activityCompleteAmount.Visible = true;
                    userLabel.Visible = true;
                    roleLabel.Visible = true;
                    role.Visible = true;
                    notesCreatedLabel.Visible = true;
                    notesCreated.Visible = true;
                    activitiesCreatedLabel.Visible = true;
                    activitiesCreated.Visible = true;
                    activitiesCompletedLabel.Visible = true;
                    activitiesCompleted.Visible = true;
                    progressLabel.Visible = true;
                    accountsCreatedLabel.Visible = true;
                    accountsCreated.Visible = true;
                    contactsCreatedLabel.Visible = true;
                    contactsCreated.Visible = true;
                    leadsCreatedLabel.Visible = true;
                    leadsCreated.Visible = true;
                    oppsCreatedLabel.Visible = true;
                    oppsCreated.Visible = true;
                    oppsUpdatedLabel.Visible = true;
                    oppsUpdated.Visible = true;
                    ticketsCreated.Visible = true;
                    ticketsCreatedLabel.Visible = true;
                    //leadsPromoted.Visible = true;
                    //leadsPromotedLabel.Visible = true;
                    noteCheckBox.Visible = true;
                    activityCheckBox.Visible = true;
                    leadCheckBox.Visible = true;
                    accountCheckBox.Visible = true;
                    contactCheckBox.Visible = true;
                    oppCheckBox.Visible = true;
                    ticketCheckBox.Visible = true;
                    oppUpdateCheckBox.Visible = true;
                    completeActCheckBox.Visible = true;
                    //promoteLeadCheckBox.Visible = true;
                    reliabilityLabel.Visible = true;
                    reliabilityValue.Visible = true;
                    dataRefreshButton.Visible = true;
                    shift.Visible = true;
                    AM.Visible = true;
                    PM.Visible = true;
                    shiftAM.Visible = true;
                    shiftPM.Visible = true;
                    userIDBox2.Visible = false;
                    userID2.Visible = false;
                    passwordBox2.Visible = false;
                    password2.Visible = false;
                    //roleSelector2.Visible = false;
                    //roleDropDownLabel2.Visible = false;
                    dataCreationTimerLabel2.Visible = false;
                    lowerBound2.Visible = false;
                    upperBound2.Visible = false;
                    minLabel2.Visible = false;
                    maxLabel2.Visible = false;
                    minutesLabel3.Visible = false;
                    minutesLabel4.Visible = false;
                    activityCompleteAmountLabel2.Visible = false;
                    activityCompleteAmount2.Visible = false;
                    userLabel2.Visible = false;
                    roleLabel2.Visible = false;
                    role2.Visible = false;
                    notesCreatedLabel2.Visible = false;
                    notesCreated2.Visible = false;
                    activitiesCreatedLabel2.Visible = false;
                    activitiesCreated2.Visible = false;
                    activitiesCompletedLabel2.Visible = false;
                    activitiesCompleted2.Visible = false;
                    accountsCreatedLabel2.Visible = false;
                    accountsCreated2.Visible = false;
                    contactsCreatedLabel2.Visible = false;
                    contactsCreated2.Visible = false;
                    leadsCreatedLabel2.Visible = false;
                    leadsCreated2.Visible = false;
                    oppsCreatedLabel2.Visible = false;
                    oppsCreated2.Visible = false;
                    oppsUpdatedLabel2.Visible = false;
                    oppsUpdated2.Visible = false;
                    ticketsCreated2.Visible = false;
                    ticketsCreatedLabel2.Visible = false;
                    //leadsPromoted2.Visible = false;
                    //leadsPromotedLabel2.Visible = false;
                    progressLabel2.Visible = false;
                    noteCheckBox2.Visible = false;
                    activityCheckBox2.Visible = false;
                    leadCheckBox2.Visible = false;
                    accountCheckBox2.Visible = false;
                    contactCheckBox2.Visible = false;
                    oppCheckBox2.Visible = false;
                    ticketCheckBox2.Visible = false;
                    oppUpdateCheckBox2.Visible = false;
                    completeActCheckBox2.Visible = false;
                    //promoteLeadCheckBox2.Visible = false;
                    reliabilityLabel2.Visible = false;
                    reliabilityValue2.Visible = false;
                    dataRefreshButton2.Visible = false;
                    shift2.Visible = false;
                    AM2.Visible = false;
                    PM2.Visible = false;
                    shiftAM2.Visible = false;
                    shiftPM2.Visible = false;
                    userIDBox3.Visible = false;
                    userID3.Visible = false;
                    passwordBox3.Visible = false;
                    password3.Visible = false;
                    //roleSelector3.Visible = false;
                    //roleDropDownLabel3.Visible = false;
                    dataCreationTimerLabel3.Visible = false;
                    lowerBound3.Visible = false;
                    upperBound3.Visible = false;
                    minLabel3.Visible = false;
                    maxLabel3.Visible = false;
                    minutesLabel5.Visible = false;
                    minutesLabel6.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    activityCompleteAmount3.Visible = false;
                    userLabel3.Visible = false;
                    roleLabel3.Visible = false;
                    role3.Visible = false;
                    notesCreatedLabel3.Visible = false;
                    notesCreated3.Visible = false;
                    activitiesCreatedLabel3.Visible = false;
                    activitiesCreated3.Visible = false;
                    activitiesCompletedLabel3.Visible = false;
                    activitiesCompleted3.Visible = false;
                    accountsCreatedLabel3.Visible = false;
                    accountsCreated3.Visible = false;
                    contactsCreatedLabel3.Visible = false;
                    contactsCreated3.Visible = false;
                    leadsCreatedLabel3.Visible = false;
                    leadsCreated3.Visible = false;
                    oppsCreatedLabel3.Visible = false;
                    oppsCreated3.Visible = false;
                    oppsUpdatedLabel3.Visible = false;
                    oppsUpdated3.Visible = false;
                    ticketsCreated3.Visible = false;
                    ticketsCreatedLabel3.Visible = false;
                    //leadsPromoted3.Visible = false;
                    //leadsPromotedLabel3.Visible = false;
                    noteCheckBox3.Visible = false;
                    activityCheckBox3.Visible = false;
                    leadCheckBox3.Visible = false;
                    accountCheckBox3.Visible = false;
                    contactCheckBox3.Visible = false;
                    oppCheckBox3.Visible = false;
                    ticketCheckBox3.Visible = false;
                    oppUpdateCheckBox3.Visible = false;
                    completeActCheckBox3.Visible = false;
                    //promoteLeadCheckBox3.Visible = false;
                    reliabilityLabel3.Visible = false;
                    reliabilityValue3.Visible = false;
                    progressLabel3.Visible = false;
                    dataRefreshButton3.Visible = false;
                    shift3.Visible = false;
                    AM3.Visible = false;
                    PM3.Visible = false;
                    shiftAM3.Visible = false;
                    shiftPM3.Visible = false;
                    userIDBox4.Visible = false;
                    userID4.Visible = false;
                    dataCreationTimerLabel4.Visible = false;
                    lowerBound4.Visible = false;
                    upperBound4.Visible = false;
                    minLabel4.Visible = false;
                    maxLabel4.Visible = false;
                    minutesLabel7.Visible = false;
                    minutesLabel8.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    activityCompleteAmount4.Visible = false;
                    userLabel4.Visible = false;
                    roleLabel4.Visible = false;
                    role4.Visible = false;
                    notesCreatedLabel4.Visible = false;
                    notesCreated4.Visible = false;
                    activitiesCreatedLabel4.Visible = false;
                    activitiesCreated4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                    activitiesCompleted4.Visible = false;
                    accountsCreatedLabel4.Visible = false;
                    accountsCreated4.Visible = false;
                    contactsCreatedLabel4.Visible = false;
                    contactsCreated4.Visible = false;
                    leadsCreatedLabel4.Visible = false;
                    leadsCreated4.Visible = false;
                    oppsCreatedLabel4.Visible = false;
                    oppsCreated4.Visible = false;
                    oppsUpdatedLabel4.Visible = false;
                    oppsUpdated4.Visible = false;
                    ticketsCreated4.Visible = false;
                    ticketsCreatedLabel4.Visible = false;
                    //leadsPromoted4.Visible = false;
                    //leadsPromotedLabel4.Visible = false;
                    noteCheckBox4.Visible = false;
                    activityCheckBox4.Visible = false;
                    leadCheckBox4.Visible = false;
                    accountCheckBox4.Visible = false;
                    contactCheckBox4.Visible = false;
                    oppCheckBox4.Visible = false;
                    ticketCheckBox4.Visible = false;
                    oppUpdateCheckBox4.Visible = false;
                    completeActCheckBox4.Visible = false;
                    //promoteLeadCheckBox4.Visible = false;
                    reliabilityLabel4.Visible = false;
                    reliabilityValue4.Visible = false;
                    progressLabel4.Visible = false;
                    dataRefreshButton4.Visible = false;
                    passwordBox4.Visible = false;
                    password4.Visible = false;
                    shift4.Visible = false;
                    AM4.Visible = false;
                    PM4.Visible = false;
                    shiftAM4.Visible = false;
                    shiftPM4.Visible = false;
                    //roleSelector4.Visible = false;
                    //roleDropDownLabel4.Visible = false;
                    userIDBox5.Visible = false;
                    userID5.Visible = false;
                    passwordBox5.Visible = false;
                    password5.Visible = false;
                    //roleSelector5.Visible = false;
                    //roleDropDownLabel5.Visible = false;
                    dataCreationTimerLabel5.Visible = false;
                    lowerBound5.Visible = false;
                    upperBound5.Visible = false;
                    minLabel5.Visible = false;
                    maxLabel5.Visible = false;
                    minutesLabel9.Visible = false;
                    minutesLabel10.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    activityCompleteAmount5.Visible = false;
                    userLabel5.Visible = false;
                    roleLabel5.Visible = false;
                    role5.Visible = false;
                    notesCreatedLabel5.Visible = false;
                    notesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    accountsCreatedLabel5.Visible = false;
                    accountsCreated5.Visible = false;
                    contactsCreatedLabel5.Visible = false;
                    contactsCreated5.Visible = false;
                    leadsCreatedLabel5.Visible = false;
                    leadsCreated5.Visible = false;
                    oppsCreatedLabel5.Visible = false;
                    oppsCreated5.Visible = false;
                    oppsUpdatedLabel5.Visible = false;
                    oppsUpdated5.Visible = false;
                    ticketsCreated5.Visible = false;
                    ticketsCreatedLabel5.Visible = false;
                    //leadsPromoted5.Visible = false;
                    //leadsPromotedLabel5.Visible = false;
                    noteCheckBox5.Visible = false;
                    activityCheckBox5.Visible = false;
                    leadCheckBox5.Visible = false;
                    accountCheckBox5.Visible = false;
                    contactCheckBox5.Visible = false;
                    oppCheckBox5.Visible = false;
                    ticketCheckBox5.Visible = false;
                    oppUpdateCheckBox5.Visible = false;
                    completeActCheckBox5.Visible = false;
                    //promoteLeadCheckBox5.Visible = false;
                    reliabilityLabel5.Visible = false;
                    reliabilityValue5.Visible = false;
                    progressLabel5.Visible = false;
                    dataRefreshButton5.Visible = false;
                    shift5.Visible = false;
                    AM5.Visible = false;
                    PM5.Visible = false;
                    shiftAM5.Visible = false;
                    shiftPM5.Visible = false;
                    break;
                case 1:
                    userIDBox.Visible = true;
                    userID.Visible = true;
                    passwordBox.Visible = true;
                    password.Visible = true;
                    //roleSelector.Visible = true;
                    //roleDropDownLabel.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    lowerBound.Visible = true;
                    upperBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    activityCompleteAmountLabel.Visible = true;
                    activityCompleteAmount.Visible = true;
                    userLabel.Visible = true;
                    roleLabel.Visible = true;
                    role.Visible = true;
                    notesCreatedLabel.Visible = true;
                    notesCreated.Visible = true;
                    activitiesCreatedLabel.Visible = true;
                    activitiesCreated.Visible = true;
                    activitiesCompletedLabel.Visible = true;
                    activitiesCompleted.Visible = true;
                    accountsCreatedLabel.Visible = true;
                    accountsCreated.Visible = true;
                    contactsCreatedLabel.Visible = true;
                    contactsCreated.Visible = true;
                    leadsCreatedLabel.Visible = true;
                    leadsCreated.Visible = true;
                    oppsCreatedLabel.Visible = true;
                    oppsCreated.Visible = true;
                    oppsUpdatedLabel.Visible = true;
                    oppsUpdated.Visible = true;
                    ticketsCreated.Visible = true;
                    ticketsCreatedLabel.Visible = true;
                    //leadsPromoted.Visible = true;
                    //leadsPromotedLabel.Visible = true;
                    noteCheckBox.Visible = true;
                    activityCheckBox.Visible = true;
                    leadCheckBox.Visible = true;
                    accountCheckBox.Visible = true;
                    contactCheckBox.Visible = true;
                    oppCheckBox.Visible = true;
                    ticketCheckBox.Visible = true;
                    oppUpdateCheckBox.Visible = true;
                    completeActCheckBox.Visible = true;
                    //promoteLeadCheckBox.Visible = true;
                    reliabilityLabel.Visible = true;
                    reliabilityValue.Visible = true;
                    progressLabel.Visible = true;
                    dataRefreshButton.Visible = true;
                    shift.Visible = true;
                    AM.Visible = true;
                    PM.Visible = true;
                    shiftAM.Visible = true;
                    shiftPM.Visible = true;
                    userIDBox2.Visible = true;
                    userID2.Visible = true;
                    passwordBox2.Visible = true;
                    password2.Visible = true;
                    //roleSelector2.Visible = true;
                    //roleDropDownLabel2.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    lowerBound2.Visible = true;
                    upperBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    activityCompleteAmountLabel2.Visible = true;
                    activityCompleteAmount2.Visible = true;
                    userLabel2.Visible = true;
                    roleLabel2.Visible = true;
                    role2.Visible = true;
                    notesCreatedLabel2.Visible = true;
                    notesCreated2.Visible = true;
                    activitiesCreatedLabel2.Visible = true;
                    activitiesCreated2.Visible = true;
                    activitiesCompletedLabel2.Visible = true;
                    activitiesCompleted2.Visible = true;
                    accountsCreatedLabel2.Visible = true;
                    accountsCreated2.Visible = true;
                    contactsCreatedLabel2.Visible = true;
                    contactsCreated2.Visible = true;
                    leadsCreatedLabel2.Visible = true;
                    leadsCreated2.Visible = true;
                    oppsCreatedLabel2.Visible = true;
                    oppsCreated2.Visible = true;
                    oppsUpdatedLabel2.Visible = true;
                    oppsUpdated2.Visible = true;
                    ticketsCreated2.Visible = true;
                    ticketsCreatedLabel2.Visible = true;
                    //leadsPromoted2.Visible = true;
                    //leadsPromotedLabel2.Visible = true;
                    accountsCreatedLabel2.Visible = true;
                    accountsCreated2.Visible = true;
                    contactsCreatedLabel2.Visible = true;
                    contactsCreated2.Visible = true;
                    leadsCreatedLabel2.Visible = true;
                    leadsCreated2.Visible = true;
                    oppsCreatedLabel2.Visible = true;
                    oppsCreated2.Visible = true;
                    oppsUpdatedLabel2.Visible = true;
                    oppsUpdated2.Visible = true;
                    ticketsCreated2.Visible = true;
                    ticketsCreatedLabel2.Visible = true;
                    noteCheckBox2.Visible = true;
                    activityCheckBox2.Visible = true;
                    leadCheckBox2.Visible = true;
                    accountCheckBox2.Visible = true;
                    contactCheckBox2.Visible = true;
                    oppCheckBox2.Visible = true;
                    ticketCheckBox2.Visible = true;
                    oppUpdateCheckBox2.Visible = true;
                    completeActCheckBox2.Visible = true;
                    //promoteLeadCheckBox2.Visible = true;
                    reliabilityLabel2.Visible = true;
                    reliabilityValue2.Visible = true;
                    progressLabel2.Visible = true;
                    dataRefreshButton2.Visible = true;
                    shift2.Visible = true;
                    AM2.Visible = true;
                    PM2.Visible = true;
                    shiftAM2.Visible = true;
                    shiftPM2.Visible = true;
                    userIDBox3.Visible = false;
                    userID3.Visible = false;
                    passwordBox3.Visible = false;
                    password3.Visible = false;
                    //roleSelector3.Visible = false;
                    //roleDropDownLabel3.Visible = false;
                    dataCreationTimerLabel3.Visible = false;
                    lowerBound3.Visible = false;
                    upperBound3.Visible = false;
                    minLabel3.Visible = false;
                    maxLabel3.Visible = false;
                    minutesLabel5.Visible = false;
                    minutesLabel6.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    activityCompleteAmount3.Visible = false;
                    userLabel3.Visible = false;
                    roleLabel3.Visible = false;
                    role3.Visible = false;
                    notesCreatedLabel3.Visible = false;
                    notesCreated3.Visible = false;
                    activitiesCreatedLabel3.Visible = false;
                    activitiesCreated3.Visible = false;
                    activitiesCompletedLabel3.Visible = false;
                    activitiesCompleted3.Visible = false;
                    accountsCreatedLabel3.Visible = false;
                    accountsCreated3.Visible = false;
                    contactsCreatedLabel3.Visible = false;
                    contactsCreated3.Visible = false;
                    leadsCreatedLabel3.Visible = false;
                    leadsCreated3.Visible = false;
                    oppsCreatedLabel3.Visible = false;
                    oppsCreated3.Visible = false;
                    oppsUpdatedLabel3.Visible = false;
                    oppsUpdated3.Visible = false;
                    ticketsCreated3.Visible = false;
                    ticketsCreatedLabel3.Visible = false;
                    //leadsPromoted3.Visible = false;
                    //leadsPromotedLabel3.Visible = false;
                    noteCheckBox3.Visible = false;
                    activityCheckBox3.Visible = false;
                    leadCheckBox3.Visible = false;
                    accountCheckBox3.Visible = false;
                    contactCheckBox3.Visible = false;
                    oppCheckBox3.Visible = false;
                    ticketCheckBox3.Visible = false;
                    oppUpdateCheckBox3.Visible = false;
                    completeActCheckBox3.Visible = false;
                    //promoteLeadCheckBox3.Visible = false;
                    reliabilityLabel3.Visible = false;
                    reliabilityValue3.Visible = false;
                    progressLabel3.Visible = false;
                    dataRefreshButton3.Visible = false;
                    shift3.Visible = false;
                    AM3.Visible = false;
                    PM3.Visible = false;
                    shiftAM3.Visible = false;
                    shiftPM3.Visible = false;
                    userIDBox4.Visible = false;
                    userID4.Visible = false;
                    dataCreationTimerLabel4.Visible = false;
                    lowerBound4.Visible = false;
                    upperBound4.Visible = false;
                    minLabel4.Visible = false;
                    maxLabel4.Visible = false;
                    minutesLabel7.Visible = false;
                    minutesLabel8.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    activityCompleteAmount4.Visible = false;
                    userLabel4.Visible = false;
                    roleLabel4.Visible = false;
                    role4.Visible = false;
                    notesCreatedLabel4.Visible = false;
                    notesCreated4.Visible = false;
                    activitiesCreatedLabel4.Visible = false;
                    activitiesCreated4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                    activitiesCompleted4.Visible = false;
                    accountsCreatedLabel4.Visible = false;
                    accountsCreated4.Visible = false;
                    contactsCreatedLabel4.Visible = false;
                    contactsCreated4.Visible = false;
                    leadsCreatedLabel4.Visible = false;
                    leadsCreated4.Visible = false;
                    oppsCreatedLabel4.Visible = false;
                    oppsCreated4.Visible = false;
                    oppsUpdatedLabel4.Visible = false;
                    oppsUpdated4.Visible = false;
                    ticketsCreated4.Visible = false;
                    ticketsCreatedLabel4.Visible = false;
                    //leadsPromoted4.Visible = false;
                    //leadsPromotedLabel4.Visible = false;
                    noteCheckBox4.Visible = false;
                    activityCheckBox4.Visible = false;
                    leadCheckBox4.Visible = false;
                    accountCheckBox4.Visible = false;
                    contactCheckBox4.Visible = false;
                    oppCheckBox4.Visible = false;
                    ticketCheckBox4.Visible = false;
                    oppUpdateCheckBox4.Visible = false;
                    completeActCheckBox4.Visible = false;
                    //promoteLeadCheckBox4.Visible = false;
                    reliabilityLabel4.Visible = false;
                    reliabilityValue4.Visible = false;
                    progressLabel4.Visible = false;
                    dataRefreshButton4.Visible = false;
                    passwordBox4.Visible = false;
                    password4.Visible = false;
                    shift4.Visible = false;
                    AM4.Visible = false;
                    PM4.Visible = false;
                    shiftAM4.Visible = false;
                    shiftPM4.Visible = false;
                    //roleSelector4.Visible = false;
                    //roleDropDownLabel4.Visible = false;
                    userIDBox5.Visible = false;
                    userID5.Visible = false;
                    passwordBox5.Visible = false;
                    password5.Visible = false;
                    //roleSelector5.Visible = false;
                    //roleDropDownLabel5.Visible = false;
                    dataCreationTimerLabel5.Visible = false;
                    lowerBound5.Visible = false;
                    upperBound5.Visible = false;
                    minLabel5.Visible = false;
                    maxLabel5.Visible = false;
                    minutesLabel9.Visible = false;
                    minutesLabel10.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    activityCompleteAmount5.Visible = false;
                    userLabel5.Visible = false;
                    roleLabel5.Visible = false;
                    role5.Visible = false;
                    notesCreatedLabel5.Visible = false;
                    notesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    accountsCreatedLabel5.Visible = false;
                    accountsCreated5.Visible = false;
                    contactsCreatedLabel5.Visible = false;
                    contactsCreated5.Visible = false;
                    leadsCreatedLabel5.Visible = false;
                    leadsCreated5.Visible = false;
                    oppsCreatedLabel5.Visible = false;
                    oppsCreated5.Visible = false;
                    oppsUpdatedLabel5.Visible = false;
                    oppsUpdated5.Visible = false;
                    ticketsCreated5.Visible = false;
                    ticketsCreatedLabel5.Visible = false;
                    //leadsPromoted5.Visible = false;
                    //leadsPromotedLabel5.Visible = false;
                    noteCheckBox5.Visible = false;
                    activityCheckBox5.Visible = false;
                    leadCheckBox5.Visible = false;
                    accountCheckBox5.Visible = false;
                    contactCheckBox5.Visible = false;
                    oppCheckBox5.Visible = false;
                    ticketCheckBox5.Visible = false;
                    oppUpdateCheckBox5.Visible = false;
                    completeActCheckBox5.Visible = false;
                    //promoteLeadCheckBox5.Visible = false;
                    reliabilityLabel5.Visible = false;
                    reliabilityValue5.Visible = false;
                    progressLabel5.Visible = false;
                    dataRefreshButton5.Visible = false;
                    shift5.Visible = false;
                    AM5.Visible = false;
                    PM5.Visible = false;
                    shiftAM5.Visible = false;
                    shiftPM5.Visible = false;
                    break;
                case 2:
                    userIDBox.Visible = true;
                    userID.Visible = true;
                    passwordBox.Visible = true;
                    password.Visible = true;
                    //roleSelector.Visible = true;
                    //roleDropDownLabel.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    lowerBound.Visible = true;
                    upperBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    activityCompleteAmountLabel.Visible = true;
                    activityCompleteAmount.Visible = true;
                    userLabel.Visible = true;
                    roleLabel.Visible = true;
                    role.Visible = true;
                    notesCreatedLabel.Visible = true;
                    notesCreated.Visible = true;
                    activitiesCreatedLabel.Visible = true;
                    activitiesCreated.Visible = true;
                    activitiesCompletedLabel.Visible = true;
                    activitiesCompleted.Visible = true;
                    accountsCreatedLabel.Visible = true;
                    accountsCreated.Visible = true;
                    contactsCreatedLabel.Visible = true;
                    contactsCreated.Visible = true;
                    leadsCreatedLabel.Visible = true;
                    leadsCreated.Visible = true;
                    oppsCreatedLabel.Visible = true;
                    oppsCreated.Visible = true;
                    oppsUpdatedLabel.Visible = true;
                    oppsUpdated.Visible = true;
                    ticketsCreated.Visible = true;
                    ticketsCreatedLabel.Visible = true;
                    //leadsPromoted.Visible = true;
                    //leadsPromotedLabel.Visible = true;
                    noteCheckBox.Visible = true;
                    activityCheckBox.Visible = true;
                    leadCheckBox.Visible = true;
                    accountCheckBox.Visible = true;
                    contactCheckBox.Visible = true;
                    oppCheckBox.Visible = true;
                    ticketCheckBox.Visible = true;
                    oppUpdateCheckBox.Visible = true;
                    completeActCheckBox.Visible = true;
                    //promoteLeadCheckBox.Visible = true;
                    reliabilityLabel.Visible = true;
                    reliabilityValue.Visible = true;
                    progressLabel.Visible = true;
                    dataRefreshButton.Visible = true;
                    shift.Visible = true;
                    AM.Visible = true;
                    PM.Visible = true;
                    shiftAM.Visible = true;
                    shiftPM.Visible = true;
                    userIDBox2.Visible = true;
                    userID2.Visible = true;
                    passwordBox2.Visible = true;
                    password2.Visible = true;
                    //roleSelector2.Visible = true;
                    //roleDropDownLabel2.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    lowerBound2.Visible = true;
                    upperBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    activityCompleteAmountLabel2.Visible = true;
                    activityCompleteAmount2.Visible = true;
                    userLabel2.Visible = true;
                    roleLabel2.Visible = true;
                    role2.Visible = true;
                    notesCreatedLabel2.Visible = true;
                    notesCreated2.Visible = true;
                    activitiesCreatedLabel2.Visible = true;
                    activitiesCreated2.Visible = true;
                    activitiesCompletedLabel2.Visible = true;
                    activitiesCompleted2.Visible = true;
                    accountsCreatedLabel2.Visible = true;
                    accountsCreated2.Visible = true;
                    contactsCreatedLabel2.Visible = true;
                    contactsCreated2.Visible = true;
                    leadsCreatedLabel2.Visible = true;
                    leadsCreated2.Visible = true;
                    oppsCreatedLabel2.Visible = true;
                    oppsCreated2.Visible = true;
                    oppsUpdatedLabel2.Visible = true;
                    oppsUpdated2.Visible = true;
                    ticketsCreated2.Visible = true;
                    ticketsCreatedLabel2.Visible = true;
                    //leadsPromoted2.Visible = true;
                    //leadsPromotedLabel2.Visible = true;
                    noteCheckBox2.Visible = true;
                    activityCheckBox2.Visible = true;
                    leadCheckBox2.Visible = true;
                    accountCheckBox2.Visible = true;
                    contactCheckBox2.Visible = true;
                    oppCheckBox2.Visible = true;
                    ticketCheckBox2.Visible = true;
                    oppUpdateCheckBox2.Visible = true;
                    completeActCheckBox2.Visible = true;
                    //promoteLeadCheckBox2.Visible = true;
                    reliabilityLabel2.Visible = true;
                    reliabilityValue2.Visible = true;
                    progressLabel2.Visible = true;
                    dataRefreshButton2.Visible = true;
                    shift2.Visible = true;
                    AM2.Visible = true;
                    PM2.Visible = true;
                    shiftAM2.Visible = true;
                    shiftPM2.Visible = true;
                    userIDBox3.Visible = true;
                    userID3.Visible = true;
                    passwordBox3.Visible = true;
                    password3.Visible = true;
                    //roleSelector3.Visible = true;
                    //roleDropDownLabel3.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    lowerBound3.Visible = true;
                    upperBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    activityCompleteAmountLabel3.Visible = true;
                    activityCompleteAmount3.Visible = true;
                    userLabel3.Visible = true;
                    roleLabel3.Visible = true;
                    role3.Visible = true;
                    notesCreatedLabel3.Visible = true;
                    notesCreated3.Visible = true;
                    activitiesCreatedLabel3.Visible = true;
                    activitiesCreated3.Visible = true;
                    activitiesCompletedLabel3.Visible = true;
                    activitiesCompleted3.Visible = true;
                    accountsCreatedLabel3.Visible = true;
                    accountsCreated3.Visible = true;
                    contactsCreatedLabel3.Visible = true;
                    contactsCreated3.Visible = true;
                    leadsCreatedLabel3.Visible = true;
                    leadsCreated3.Visible = true;
                    oppsCreatedLabel3.Visible = true;
                    oppsCreated3.Visible = true;
                    oppsUpdatedLabel3.Visible = true;
                    oppsUpdated3.Visible = true;
                    ticketsCreated3.Visible = true;
                    ticketsCreatedLabel3.Visible = true;
                    //leadsPromoted3.Visible = true;
                    //leadsPromotedLabel3.Visible = true;
                    noteCheckBox3.Visible = true;
                    activityCheckBox3.Visible = true;
                    leadCheckBox3.Visible = true;
                    accountCheckBox3.Visible = true;
                    contactCheckBox3.Visible = true;
                    oppCheckBox3.Visible = true;
                    ticketCheckBox3.Visible = true;
                    oppUpdateCheckBox3.Visible = true;
                    completeActCheckBox3.Visible = true;
                    //promoteLeadCheckBox3.Visible = true;
                    reliabilityLabel3.Visible = true;
                    reliabilityValue3.Visible = true;
                    progressLabel3.Visible = true;
                    dataRefreshButton3.Visible = true;
                    shift3.Visible = true;
                    AM3.Visible = true;
                    PM3.Visible = true;
                    shiftAM3.Visible = true;
                    shiftPM3.Visible = true;
                    userIDBox4.Visible = false;
                    userID4.Visible = false;
                    dataCreationTimerLabel4.Visible = false;
                    lowerBound4.Visible = false;
                    upperBound4.Visible = false;
                    minLabel4.Visible = false;
                    maxLabel4.Visible = false;
                    minutesLabel7.Visible = false;
                    minutesLabel8.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    activityCompleteAmount4.Visible = false;
                    userLabel4.Visible = false;
                    roleLabel4.Visible = false;
                    role4.Visible = false;
                    notesCreatedLabel4.Visible = false;
                    notesCreated4.Visible = false;
                    activitiesCreatedLabel4.Visible = false;
                    activitiesCreated4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                    activitiesCompleted4.Visible = false;
                    accountsCreatedLabel4.Visible = false;
                    accountsCreated4.Visible = false;
                    contactsCreatedLabel4.Visible = false;
                    contactsCreated4.Visible = false;
                    leadsCreatedLabel4.Visible = false;
                    leadsCreated4.Visible = false;
                    oppsCreatedLabel4.Visible = false;
                    oppsCreated4.Visible = false;
                    oppsUpdatedLabel4.Visible = false;
                    oppsUpdated4.Visible = false;
                    ticketsCreated4.Visible = false;
                    ticketsCreatedLabel4.Visible = false;
                    //leadsPromoted4.Visible = false;
                    //leadsPromotedLabel4.Visible = false;
                    noteCheckBox4.Visible = false;
                    activityCheckBox4.Visible = false;
                    leadCheckBox4.Visible = false;
                    accountCheckBox4.Visible = false;
                    contactCheckBox4.Visible = false;
                    oppCheckBox4.Visible = false;
                    ticketCheckBox4.Visible = false;
                    oppUpdateCheckBox4.Visible = false;
                    completeActCheckBox4.Visible = false;
                    //promoteLeadCheckBox4.Visible = false;
                    reliabilityLabel4.Visible = false;
                    reliabilityValue4.Visible = false;
                    progressLabel4.Visible = false;
                    dataRefreshButton4.Visible = false;
                    passwordBox4.Visible = false;
                    password4.Visible = false;
                    shift4.Visible = false;
                    AM4.Visible = false;
                    PM4.Visible = false;
                    shiftAM4.Visible = false;
                    shiftPM4.Visible = false;
                    //roleSelector4.Visible = false;
                    //roleDropDownLabel4.Visible = false;
                    userIDBox5.Visible = false;
                    userID5.Visible = false;
                    passwordBox5.Visible = false;
                    password5.Visible = false;
                    //roleSelector5.Visible = false;
                    //roleDropDownLabel5.Visible = false;
                    dataCreationTimerLabel5.Visible = false;
                    lowerBound5.Visible = false;
                    upperBound5.Visible = false;
                    minLabel5.Visible = false;
                    maxLabel5.Visible = false;
                    minutesLabel9.Visible = false;
                    minutesLabel10.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    activityCompleteAmount5.Visible = false;
                    userLabel5.Visible = false;
                    roleLabel5.Visible = false;
                    role5.Visible = false;
                    notesCreatedLabel5.Visible = false;
                    notesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    accountsCreatedLabel5.Visible = false;
                    accountsCreated5.Visible = false;
                    contactsCreatedLabel5.Visible = false;
                    contactsCreated5.Visible = false;
                    leadsCreatedLabel5.Visible = false;
                    leadsCreated5.Visible = false;
                    oppsCreatedLabel5.Visible = false;
                    oppsCreated5.Visible = false;
                    oppsUpdatedLabel5.Visible = false;
                    oppsUpdated5.Visible = false;
                    ticketsCreated5.Visible = false;
                    ticketsCreatedLabel5.Visible = false;
                    //leadsPromoted5.Visible = false;
                    //leadsPromotedLabel5.Visible = false;
                    noteCheckBox5.Visible = false;
                    activityCheckBox5.Visible = false;
                    leadCheckBox5.Visible = false;
                    accountCheckBox5.Visible = false;
                    contactCheckBox5.Visible = false;
                    oppCheckBox5.Visible = false;
                    ticketCheckBox5.Visible = false;
                    oppUpdateCheckBox5.Visible = false;
                    completeActCheckBox5.Visible = false;
                    //promoteLeadCheckBox5.Visible = false;
                    reliabilityLabel5.Visible = false;
                    reliabilityValue5.Visible = false;
                    progressLabel5.Visible = false;
                    dataRefreshButton5.Visible = false;
                    shift5.Visible = false;
                    AM5.Visible = false;
                    PM5.Visible = false;
                    shiftAM5.Visible = false;
                    shiftPM5.Visible = false;
                    break;
                case 3:
                    userIDBox.Visible = true;
                    userID.Visible = true;
                    passwordBox.Visible = true;
                    password.Visible = true;
                    //roleSelector.Visible = true;
                    //roleDropDownLabel.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    lowerBound.Visible = true;
                    upperBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    activityCompleteAmountLabel.Visible = true;
                    activityCompleteAmount.Visible = true;
                    userLabel.Visible = true;
                    roleLabel.Visible = true;
                    role.Visible = true;
                    notesCreatedLabel.Visible = true;
                    notesCreated.Visible = true;
                    activitiesCreatedLabel.Visible = true;
                    activitiesCreated.Visible = true;
                    activitiesCompletedLabel.Visible = true;
                    activitiesCompleted.Visible = true;
                    accountsCreatedLabel.Visible = true;
                    accountsCreated.Visible = true;
                    contactsCreatedLabel.Visible = true;
                    contactsCreated.Visible = true;
                    leadsCreatedLabel.Visible = true;
                    leadsCreated.Visible = true;
                    oppsCreatedLabel.Visible = true;
                    oppsCreated.Visible = true;
                    oppsUpdatedLabel.Visible = true;
                    oppsUpdated.Visible = true;
                    ticketsCreated.Visible = true;
                    ticketsCreatedLabel.Visible = true;
                    //leadsPromoted.Visible = true;
                    //leadsPromotedLabel.Visible = true;
                    noteCheckBox.Visible = true;
                    activityCheckBox.Visible = true;
                    leadCheckBox.Visible = true;
                    accountCheckBox.Visible = true;
                    contactCheckBox.Visible = true;
                    oppCheckBox.Visible = true;
                    ticketCheckBox.Visible = true;
                    oppUpdateCheckBox.Visible = true;
                    completeActCheckBox.Visible = true;
                    //promoteLeadCheckBox.Visible = true;
                    reliabilityLabel.Visible = true;
                    reliabilityValue.Visible = true;
                    progressLabel.Visible = true;
                    dataRefreshButton.Visible = true;
                    shift.Visible = true;
                    AM.Visible = true;
                    PM.Visible = true;
                    shiftAM.Visible = true;
                    shiftPM.Visible = true;
                    userIDBox2.Visible = true;
                    userID2.Visible = true;
                    passwordBox2.Visible = true;
                    password2.Visible = true;
                    //roleSelector2.Visible = true;
                    //roleDropDownLabel2.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    lowerBound2.Visible = true;
                    upperBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    activityCompleteAmountLabel2.Visible = true;
                    activityCompleteAmount2.Visible = true;
                    userLabel2.Visible = true;
                    roleLabel2.Visible = true;
                    role2.Visible = true;
                    notesCreatedLabel2.Visible = true;
                    notesCreated2.Visible = true;
                    activitiesCreatedLabel2.Visible = true;
                    activitiesCreated2.Visible = true;
                    activitiesCompletedLabel2.Visible = true;
                    activitiesCompleted2.Visible = true;
                    accountsCreatedLabel2.Visible = true;
                    accountsCreated2.Visible = true;
                    contactsCreatedLabel2.Visible = true;
                    contactsCreated2.Visible = true;
                    leadsCreatedLabel2.Visible = true;
                    leadsCreated2.Visible = true;
                    oppsCreatedLabel2.Visible = true;
                    oppsCreated2.Visible = true;
                    oppsUpdatedLabel2.Visible = true;
                    oppsUpdated2.Visible = true;
                    ticketsCreated2.Visible = true;
                    ticketsCreatedLabel2.Visible = true;
                    //leadsPromoted2.Visible = true;
                    //leadsPromotedLabel2.Visible = true;
                    noteCheckBox2.Visible = true;
                    activityCheckBox2.Visible = true;
                    leadCheckBox2.Visible = true;
                    accountCheckBox2.Visible = true;
                    contactCheckBox2.Visible = true;
                    oppCheckBox2.Visible = true;
                    ticketCheckBox2.Visible = true;
                    oppUpdateCheckBox2.Visible = true;
                    completeActCheckBox2.Visible = true;
                    //promoteLeadCheckBox2.Visible = true;
                    reliabilityLabel2.Visible = true;
                    reliabilityValue2.Visible = true;
                    progressLabel2.Visible = true;
                    dataRefreshButton2.Visible = true;
                    shift2.Visible = true;
                    AM2.Visible = true;
                    PM2.Visible = true;
                    shiftAM2.Visible = true;
                    shiftPM2.Visible = true;
                    userIDBox3.Visible = true;
                    userID3.Visible = true;
                    passwordBox3.Visible = true;
                    password3.Visible = true;
                    //roleSelector3.Visible = true;
                    //roleDropDownLabel3.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    lowerBound3.Visible = true;
                    upperBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    activityCompleteAmountLabel3.Visible = true;
                    activityCompleteAmount3.Visible = true;
                    userLabel3.Visible = true;
                    roleLabel3.Visible = true;
                    role3.Visible = true;
                    notesCreatedLabel3.Visible = true;
                    notesCreated3.Visible = true;
                    activitiesCreatedLabel3.Visible = true;
                    activitiesCreated3.Visible = true;
                    activitiesCompletedLabel3.Visible = true;
                    activitiesCompleted3.Visible = true;
                    accountsCreatedLabel3.Visible = true;
                    accountsCreated3.Visible = true;
                    contactsCreatedLabel3.Visible = true;
                    contactsCreated3.Visible = true;
                    leadsCreatedLabel3.Visible = true;
                    leadsCreated3.Visible = true;
                    oppsCreatedLabel3.Visible = true;
                    oppsCreated3.Visible = true;
                    oppsUpdatedLabel3.Visible = true;
                    oppsUpdated3.Visible = true;
                    ticketsCreated3.Visible = true;
                    ticketsCreatedLabel3.Visible = true;
                    //leadsPromoted3.Visible = true;
                    //leadsPromotedLabel3.Visible = true;
                    noteCheckBox3.Visible = true;
                    activityCheckBox3.Visible = true;
                    leadCheckBox3.Visible = true;
                    accountCheckBox3.Visible = true;
                    contactCheckBox3.Visible = true;
                    oppCheckBox3.Visible = true;
                    ticketCheckBox3.Visible = true;
                    oppUpdateCheckBox3.Visible = true;
                    completeActCheckBox3.Visible = true;
                    //promoteLeadCheckBox3.Visible = true;
                    reliabilityLabel3.Visible = true;
                    reliabilityValue3.Visible = true;
                    progressLabel3.Visible = true;
                    dataRefreshButton3.Visible = true;
                    shift3.Visible = true;
                    AM3.Visible = true;
                    PM3.Visible = true;
                    shiftAM3.Visible = true;
                    shiftPM3.Visible = true;
                    userIDBox4.Visible = true;
                    userID4.Visible = true;
                    dataCreationTimerLabel4.Visible = true;
                    lowerBound4.Visible = true;
                    upperBound4.Visible = true;
                    minLabel4.Visible = true;
                    maxLabel4.Visible = true;
                    minutesLabel7.Visible = true;
                    minutesLabel8.Visible = true;
                    activityCompleteAmountLabel4.Visible = true;
                    activityCompleteAmount4.Visible = true;
                    userLabel4.Visible = true;
                    roleLabel4.Visible = true;
                    role4.Visible = true;
                    notesCreatedLabel4.Visible = true;
                    notesCreated4.Visible = true;
                    activitiesCreatedLabel4.Visible = true;
                    activitiesCreated4.Visible = true;
                    activitiesCompletedLabel4.Visible = true;
                    activitiesCompleted4.Visible = true;
                    accountsCreatedLabel4.Visible = true;
                    accountsCreated4.Visible = true;
                    contactsCreatedLabel4.Visible = true;
                    contactsCreated4.Visible = true;
                    leadsCreatedLabel4.Visible = true;
                    leadsCreated4.Visible = true;
                    oppsCreatedLabel4.Visible = true;
                    oppsCreated4.Visible = true;
                    oppsUpdatedLabel4.Visible = true;
                    oppsUpdated4.Visible = true;
                    ticketsCreated4.Visible = true;
                    ticketsCreatedLabel4.Visible = true;
                    //leadsPromoted4.Visible = true;
                    //leadsPromotedLabel4.Visible = true;
                    noteCheckBox4.Visible = true;
                    activityCheckBox4.Visible = true;
                    leadCheckBox4.Visible = true;
                    accountCheckBox4.Visible = true;
                    contactCheckBox4.Visible = true;
                    oppCheckBox4.Visible = true;
                    ticketCheckBox4.Visible = true;
                    oppUpdateCheckBox4.Visible = true;
                    completeActCheckBox4.Visible = true;
                    //promoteLeadCheckBox4.Visible = true;
                    reliabilityLabel4.Visible = true;
                    reliabilityValue4.Visible = true;
                    progressLabel4.Visible = true;
                    dataRefreshButton4.Visible = true;
                    passwordBox4.Visible = true;
                    password4.Visible = true;
                    shift4.Visible = true;
                    AM4.Visible = true;
                    PM4.Visible = true;
                    shiftAM4.Visible = true;
                    shiftPM4.Visible = true;
                    //roleSelector4.Visible = true;
                    //roleDropDownLabel4.Visible = true;
                    userIDBox5.Visible = false;
                    userID5.Visible = false;
                    passwordBox5.Visible = false;
                    password5.Visible = false;
                    //roleSelector5.Visible = false;
                    //roleDropDownLabel5.Visible = false;
                    dataCreationTimerLabel5.Visible = false;
                    lowerBound5.Visible = false;
                    upperBound5.Visible = false;
                    minLabel5.Visible = false;
                    maxLabel5.Visible = false;
                    minutesLabel9.Visible = false;
                    minutesLabel10.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    activityCompleteAmount5.Visible = false;
                    userLabel5.Visible = false;
                    roleLabel5.Visible = false;
                    role5.Visible = false;
                    notesCreatedLabel5.Visible = false;
                    notesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    accountsCreatedLabel5.Visible = false;
                    accountsCreated5.Visible = false;
                    contactsCreatedLabel5.Visible = false;
                    contactsCreated5.Visible = false;
                    leadsCreatedLabel5.Visible = false;
                    leadsCreated5.Visible = false;
                    oppsCreatedLabel5.Visible = false;
                    oppsCreated5.Visible = false;
                    oppsUpdatedLabel5.Visible = false;
                    oppsUpdated5.Visible = false;
                    ticketsCreated5.Visible = false;
                    ticketsCreatedLabel5.Visible = false;
                    //leadsPromoted5.Visible = false;
                    //leadsPromotedLabel5.Visible = false;
                    noteCheckBox5.Visible = false;
                    activityCheckBox5.Visible = false;
                    leadCheckBox5.Visible = false;
                    accountCheckBox5.Visible = false;
                    contactCheckBox5.Visible = false;
                    oppCheckBox5.Visible = false;
                    ticketCheckBox5.Visible = false;
                    oppUpdateCheckBox5.Visible = false;
                    completeActCheckBox5.Visible = false;
                    //promoteLeadCheckBox5.Visible = false;
                    reliabilityLabel5.Visible = false;
                    reliabilityValue5.Visible = false;
                    progressLabel5.Visible = false;
                    dataRefreshButton5.Visible = false;
                    shift5.Visible = false;
                    AM5.Visible = false;
                    PM5.Visible = false;
                    shiftAM5.Visible = false;
                    shiftPM5.Visible = false;
                    break;
                case 4:
                    userIDBox.Visible = true;
                    userID.Visible = true;
                    passwordBox.Visible = true;
                    password.Visible = true;
                    //roleSelector.Visible = true;
                    //roleDropDownLabel.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    lowerBound.Visible = true;
                    upperBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    activityCompleteAmountLabel.Visible = true;
                    activityCompleteAmount.Visible = true;
                    userLabel.Visible = true;
                    roleLabel.Visible = true;
                    role.Visible = true;
                    notesCreatedLabel.Visible = true;
                    notesCreated.Visible = true;
                    activitiesCreatedLabel.Visible = true;
                    activitiesCreated.Visible = true;
                    activitiesCompletedLabel.Visible = true;
                    activitiesCompleted.Visible = true;
                    accountsCreatedLabel.Visible = true;
                    accountsCreated.Visible = true;
                    contactsCreatedLabel.Visible = true;
                    contactsCreated.Visible = true;
                    leadsCreatedLabel.Visible = true;
                    leadsCreated.Visible = true;
                    oppsCreatedLabel.Visible = true;
                    oppsCreated.Visible = true;
                    oppsUpdatedLabel.Visible = true;
                    oppsUpdated.Visible = true;
                    ticketsCreated.Visible = true;
                    ticketsCreatedLabel.Visible = true;
                    //leadsPromoted.Visible = true;
                    //leadsPromotedLabel.Visible = true;
                    noteCheckBox.Visible = true;
                    activityCheckBox.Visible = true;
                    leadCheckBox.Visible = true;
                    accountCheckBox.Visible = true;
                    contactCheckBox.Visible = true;
                    oppCheckBox.Visible = true;
                    ticketCheckBox.Visible = true;
                    oppUpdateCheckBox.Visible = true;
                    completeActCheckBox.Visible = true;
                    //promoteLeadCheckBox.Visible = true;
                    reliabilityLabel.Visible = true;
                    reliabilityValue.Visible = true;
                    progressLabel.Visible = true;
                    dataRefreshButton.Visible = true;
                    shift.Visible = true;
                    AM.Visible = true;
                    PM.Visible = true;
                    shiftAM.Visible = true;
                    shiftPM.Visible = true;
                    userIDBox2.Visible = true;
                    userID2.Visible = true;
                    passwordBox2.Visible = true;
                    password2.Visible = true;
                    //roleSelector2.Visible = true;
                    //roleDropDownLabel2.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    lowerBound2.Visible = true;
                    upperBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    activityCompleteAmountLabel2.Visible = true;
                    activityCompleteAmount2.Visible = true;
                    userLabel2.Visible = true;
                    roleLabel2.Visible = true;
                    role2.Visible = true;
                    notesCreatedLabel2.Visible = true;
                    notesCreated2.Visible = true;
                    activitiesCreatedLabel2.Visible = true;
                    activitiesCreated2.Visible = true;
                    activitiesCompletedLabel2.Visible = true;
                    activitiesCompleted2.Visible = true;
                    accountsCreatedLabel2.Visible = true;
                    accountsCreated2.Visible = true;
                    contactsCreatedLabel2.Visible = true;
                    contactsCreated2.Visible = true;
                    leadsCreatedLabel2.Visible = true;
                    leadsCreated2.Visible = true;
                    oppsCreatedLabel2.Visible = true;
                    oppsCreated2.Visible = true;
                    oppsUpdatedLabel2.Visible = true;
                    oppsUpdated2.Visible = true;
                    ticketsCreated2.Visible = true;
                    ticketsCreatedLabel2.Visible = true;
                    //leadsPromoted2.Visible = true;
                    //leadsPromotedLabel2.Visible = true;
                    noteCheckBox2.Visible = true;
                    activityCheckBox2.Visible = true;
                    leadCheckBox2.Visible = true;
                    accountCheckBox2.Visible = true;
                    contactCheckBox2.Visible = true;
                    oppCheckBox2.Visible = true;
                    ticketCheckBox2.Visible = true;
                    oppUpdateCheckBox2.Visible = true;
                    completeActCheckBox2.Visible = true;
                    //promoteLeadCheckBox2.Visible = true;
                    reliabilityLabel2.Visible = true;
                    reliabilityValue2.Visible = true;
                    progressLabel2.Visible = true;
                    dataRefreshButton2.Visible = true;
                    shift2.Visible = true;
                    AM2.Visible = true;
                    PM2.Visible = true;
                    shiftAM2.Visible = true;
                    shiftPM2.Visible = true;
                    userIDBox3.Visible = true;
                    userID3.Visible = true;
                    passwordBox3.Visible = true;
                    password3.Visible = true;
                    //roleSelector3.Visible = true;
                    //roleDropDownLabel3.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    lowerBound3.Visible = true;
                    upperBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    activityCompleteAmountLabel3.Visible = true;
                    activityCompleteAmount3.Visible = true;
                    userLabel3.Visible = true;
                    roleLabel3.Visible = true;
                    role3.Visible = true;
                    notesCreatedLabel3.Visible = true;
                    notesCreated3.Visible = true;
                    activitiesCreatedLabel3.Visible = true;
                    activitiesCreated3.Visible = true;
                    activitiesCompletedLabel3.Visible = true;
                    activitiesCompleted3.Visible = true;
                    accountsCreatedLabel3.Visible = true;
                    accountsCreated3.Visible = true;
                    contactsCreatedLabel3.Visible = true;
                    contactsCreated3.Visible = true;
                    leadsCreatedLabel3.Visible = true;
                    leadsCreated3.Visible = true;
                    oppsCreatedLabel3.Visible = true;
                    oppsCreated3.Visible = true;
                    oppsUpdatedLabel3.Visible = true;
                    oppsUpdated3.Visible = true;
                    ticketsCreated3.Visible = true;
                    ticketsCreatedLabel3.Visible = true;
                    //leadsPromoted3.Visible = true;
                    //leadsPromotedLabel3.Visible = true;
                    noteCheckBox3.Visible = true;
                    activityCheckBox3.Visible = true;
                    leadCheckBox3.Visible = true;
                    accountCheckBox3.Visible = true;
                    contactCheckBox3.Visible = true;
                    oppCheckBox3.Visible = true;
                    ticketCheckBox3.Visible = true;
                    oppUpdateCheckBox3.Visible = true;
                    completeActCheckBox3.Visible = true;
                    //promoteLeadCheckBox3.Visible = true;
                    reliabilityLabel3.Visible = true;
                    reliabilityValue3.Visible = true;
                    progressLabel3.Visible = true;
                    dataRefreshButton3.Visible = true;
                    shift3.Visible = true;
                    AM3.Visible = true;
                    PM3.Visible = true;
                    shiftAM3.Visible = true;
                    shiftPM3.Visible = true;
                    userIDBox4.Visible = true;
                    userID4.Visible = true;
                    dataCreationTimerLabel4.Visible = true;
                    lowerBound4.Visible = true;
                    upperBound4.Visible = true;
                    minLabel4.Visible = true;
                    maxLabel4.Visible = true;
                    minutesLabel7.Visible = true;
                    minutesLabel8.Visible = true;
                    activityCompleteAmountLabel4.Visible = true;
                    activityCompleteAmount4.Visible = true;
                    userLabel4.Visible = true;
                    roleLabel4.Visible = true;
                    role4.Visible = true;
                    notesCreatedLabel4.Visible = true;
                    notesCreated4.Visible = true;
                    activitiesCreatedLabel4.Visible = true;
                    activitiesCreated4.Visible = true;
                    activitiesCompletedLabel4.Visible = true;
                    activitiesCompleted4.Visible = true;
                    accountsCreatedLabel4.Visible = true;
                    accountsCreated4.Visible = true;
                    contactsCreatedLabel4.Visible = true;
                    contactsCreated4.Visible = true;
                    leadsCreatedLabel4.Visible = true;
                    leadsCreated4.Visible = true;
                    oppsCreatedLabel4.Visible = true;
                    oppsCreated4.Visible = true;
                    oppsUpdatedLabel4.Visible = true;
                    oppsUpdated4.Visible = true;
                    ticketsCreated4.Visible = true;
                    ticketsCreatedLabel4.Visible = true;
                    //leadsPromoted4.Visible = true;
                    //leadsPromotedLabel4.Visible = true;
                    noteCheckBox4.Visible = true;
                    activityCheckBox4.Visible = true;
                    leadCheckBox4.Visible = true;
                    accountCheckBox4.Visible = true;
                    contactCheckBox4.Visible = true;
                    oppCheckBox4.Visible = true;
                    ticketCheckBox4.Visible = true;
                    oppUpdateCheckBox4.Visible = true;
                    completeActCheckBox4.Visible = true;
                    //promoteLeadCheckBox4.Visible = true;
                    reliabilityLabel4.Visible = true;
                    reliabilityValue4.Visible = true;
                    progressLabel4.Visible = true;
                    dataRefreshButton4.Visible = true;
                    passwordBox4.Visible = true;
                    password4.Visible = true;
                    shift4.Visible = true;
                    AM4.Visible = true;
                    PM4.Visible = true;
                    shiftAM4.Visible = true;
                    shiftPM4.Visible = true;
                    //roleSelector4.Visible = true;
                    //roleDropDownLabel4.Visible = true;
                    userIDBox5.Visible = true;
                    userID5.Visible = true;
                    passwordBox5.Visible = true;
                    password5.Visible = true;
                    //roleSelector5.Visible = true;
                    //roleDropDownLabel5.Visible = true;
                    dataCreationTimerLabel5.Visible = true;
                    lowerBound5.Visible = true;
                    upperBound5.Visible = true;
                    minLabel5.Visible = true;
                    maxLabel5.Visible = true;
                    minutesLabel9.Visible = true;
                    minutesLabel10.Visible = true;
                    activityCompleteAmountLabel5.Visible = true;
                    activityCompleteAmount5.Visible = true;
                    userLabel5.Visible = true;
                    roleLabel5.Visible = true;
                    role5.Visible = true;
                    notesCreatedLabel5.Visible = true;
                    notesCreated5.Visible = true;
                    activitiesCreatedLabel5.Visible = true;
                    activitiesCreated5.Visible = true;
                    activitiesCompletedLabel5.Visible = true;
                    activitiesCompleted5.Visible = true;
                    accountsCreatedLabel5.Visible = true;
                    accountsCreated5.Visible = true;
                    contactsCreatedLabel5.Visible = true;
                    contactsCreated5.Visible = true;
                    leadsCreatedLabel5.Visible = true;
                    leadsCreated5.Visible = true;
                    oppsCreatedLabel5.Visible = true;
                    oppsCreated5.Visible = true;
                    oppsUpdatedLabel5.Visible = true;
                    oppsUpdated5.Visible = true;
                    ticketsCreated5.Visible = true;
                    ticketsCreatedLabel5.Visible = true;
                    //leadsPromoted5.Visible = true;
                    //leadsPromotedLabel5.Visible = true;
                    noteCheckBox5.Visible = true;
                    activityCheckBox5.Visible = true;
                    leadCheckBox5.Visible = true;
                    accountCheckBox5.Visible = true;
                    contactCheckBox5.Visible = true;
                    oppCheckBox5.Visible = true;
                    ticketCheckBox5.Visible = true;
                    oppUpdateCheckBox5.Visible = true;
                    completeActCheckBox5.Visible = true;
                    //promoteLeadCheckBox5.Visible = true;
                    reliabilityLabel5.Visible = true;
                    reliabilityValue5.Visible = true;
                    progressLabel5.Visible = true;
                    dataRefreshButton5.Visible = true;
                    shift5.Visible = true;
                    AM5.Visible = true;
                    PM5.Visible = true;
                    shiftAM5.Visible = true;
                    shiftPM5.Visible = true;
                    break;
            }
            #endregion
        }
        #endregion

        // Currently unused
        private void roleSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (roleSelector.SelectedIndex)
            {
                    // General
                case 0:
                    SetRole("General");
                    //SetLabelText("Notes Created:");
                    chooseDoc1.Visible = false;
                    activitiesCreated.Visible = true;
                    activitiesCreatedLabel.Visible = true;
                    activitiesCompleted.Visible = true;
                    activitiesCompletedLabel.Visible = true;
                    activityCompleteAmount.Visible = true;
                    activityCompleteAmountLabel.Visible = true;
                    upperBound.Visible = true;
                    lowerBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    reliabilityLabel.Visible = true;
                    reliabilityValue.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    noteCheckBox.Visible = true;
                    activityCheckBox.Visible = true;
                    leadCheckBox.Visible = true;
                    accountCheckBox.Visible = true;
                    contactCheckBox.Visible = true;
                    oppCheckBox.Visible = true;
                    ticketCheckBox.Visible = true;
                    oppUpdateCheckBox.Visible = true;
                    completeActCheckBox.Visible = true;
                    //promoteLeadCheckBox.Visible = true;
                    break;

                    /*
                    // Data Creator
                case 1:
                    SetRole("Data Creator");
                    //SetLabelText("Notes Created:");
                    chooseDoc1.Visible = false;
                    activitiesCreated.Visible = true;
                    activitiesCreatedLabel.Visible = true;
                    activitiesCompleted.Visible = false;
                    activitiesCompletedLabel.Visible = false;
                    activityCompleteAmount.Visible = false;
                    activityCompleteAmountLabel.Visible = false;
                    upperBound.Visible = true;
                    lowerBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    noteCheckBox.Enabled = true;
                    activityCheckBox.Enabled = true;
                    leadCheckBox.Enabled = true;
                    accountCheckBox.Enabled = true;
                    contactCheckBox.Enabled = true;
                    oppCheckBox.Enabled = true;
                    ticketCheckBox.Enabled = true;
                    oppUpdateCheckBox.Enabled = true;
                    completeActCheckBox.Enabled = true;
                    break;
                    // Lead Generator
                case 2:
                    SetRole("Lead Generator");
                    //SetLabelText("Leads Generated:");
                    chooseDoc1.Visible = false;
                    activitiesCreated.Visible = false;
                    activitiesCreatedLabel.Visible = false;
                    activitiesCompleted.Visible = false;
                    activitiesCompletedLabel.Visible = false;
                    activityCompleteAmount.Visible = false;
                    activityCompleteAmountLabel.Visible = false;
                    upperBound.Visible = true;
                    lowerBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    break;
                    // Helper
                case 3:
                    SetRole("Helper");
                    //SetLabelText("Tickets Made:");
                    //SetActivitiesCreatedText("Leads Generated:");
                    //SetActivitiesCompletedText("Opportunities Made:");
                    chooseDoc1.Visible = false;
                    activitiesCreated.Visible = true;
                    activitiesCreatedLabel.Visible = true;
                    activitiesCompleted.Visible = true;
                    activitiesCompletedLabel.Visible = true;
                    activityCompleteAmount.Visible = false;
                    activityCompleteAmountLabel.Visible = false;
                    upperBound.Visible = true;
                    lowerBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    break;
                case 4:
                    SetRole("Ticket Maker");
                    //SetLabelText("Tickets Made:");
                    chooseDoc1.Visible = false;
                    activitiesCreated.Visible = false;
                    activitiesCreatedLabel.Visible = false;
                    activitiesCompleted.Visible = false;
                    activitiesCompletedLabel.Visible = false;
                    activityCompleteAmount.Visible = false;
                    activityCompleteAmountLabel.Visible = false;
                    upperBound.Visible = true;
                    lowerBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    break;
                case 5:
                    SetRole("Opportunity Generator");
                    //SetLabelText("Opportunities Made:");
                    chooseDoc1.Visible = false;
                    activitiesCreated.Visible = false;
                    activitiesCreatedLabel.Visible = false;
                    activitiesCompleted.Visible = false;
                    activitiesCompletedLabel.Visible = false;
                    activityCompleteAmount.Visible = false;
                    activityCompleteAmountLabel.Visible = false;
                    upperBound.Visible = true;
                    lowerBound.Visible = true;
                    minLabel.Visible = true;
                    maxLabel.Visible = true;
                    minutesLabel.Visible = true;
                    minutesLabel2.Visible = true;
                    dataCreationTimerLabel.Visible = true;
                    break;
                     * */
                case 1:
                    SetRole("From GoogleDoc");
                    chooseDoc1.Visible = true;
                    /*notesCreated.Visible = false;
                    notesCreatedLabel.Visible = false;
                    activitiesCreated.Visible = false;
                    activitiesCreatedLabel.Visible = false;
                    activitiesCompleted.Visible = false;
                    activitiesCompletedLabel.Visible = false;
                     * */
                    activityCompleteAmount.Visible = false;
                    activityCompleteAmountLabel.Visible = false;
                    upperBound.Visible = false;
                    lowerBound.Visible = false;
                    minLabel.Visible = false;
                    maxLabel.Visible = false;
                    minutesLabel.Visible = false;
                    minutesLabel2.Visible = false;
                    reliabilityLabel.Visible = false;
                    reliabilityValue.Visible = false;
                    dataCreationTimerLabel.Visible = false;
                    noteCheckBox.Visible = false;
                    activityCheckBox.Visible = false;
                    leadCheckBox.Visible = false;
                    accountCheckBox.Visible = false;
                    contactCheckBox.Visible = false;
                    oppCheckBox.Visible = false;
                    ticketCheckBox.Visible = false;
                    oppUpdateCheckBox.Visible = false;
                    completeActCheckBox.Visible = false;
                    //promoteLeadCheckBox.Visible = false;
                    break;
                case 2:
                    SetRole("From Excel");
                    chooseDoc1.Visible = true;
                    /*
                    notesCreated.Visible = false;
                    notesCreatedLabel.Visible = false;
                    activitiesCreated.Visible = false;
                    activitiesCreatedLabel.Visible = false;
                    activitiesCompleted.Visible = false;
                    activitiesCompletedLabel.Visible = false;
                     * */
                    activityCompleteAmount.Visible = false;
                    activityCompleteAmountLabel.Visible = false;
                    upperBound.Visible = false;
                    lowerBound.Visible = false;
                    minLabel.Visible = false;
                    maxLabel.Visible = false;
                    minutesLabel.Visible = false;
                    minutesLabel2.Visible = false;
                    reliabilityLabel.Visible = false;
                    reliabilityValue.Visible = false;
                    dataCreationTimerLabel.Visible = false;
                    noteCheckBox.Visible = false;
                    activityCheckBox.Visible = false;
                    leadCheckBox.Visible = false;
                    accountCheckBox.Visible = false;
                    contactCheckBox.Visible = false;
                    oppCheckBox.Visible = false;
                    ticketCheckBox.Visible = false;
                    oppUpdateCheckBox.Visible = false;
                    completeActCheckBox.Visible = false;
                    //promoteLeadCheckBox.Visible = false;
                    break;
            }
        }

        // Currently unused
        private void roleSelector2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (roleSelector2.SelectedIndex)
            {
                // General
                case 0:
                    SetRole2("General");
                    SetLabelText2("Notes Created:");
                    chooseDoc2.Visible = false;
                    activitiesCreated2.Visible = true;
                    activitiesCreatedLabel2.Visible = true;
                    activitiesCompleted2.Visible = true;
                    activitiesCompletedLabel2.Visible = true;
                    activityCompleteAmount2.Visible = true;
                    activityCompleteAmountLabel2.Visible = true;
                    upperBound2.Visible = true;
                    lowerBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    reliabilityLabel2.Visible = true;
                    reliabilityValue2.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    noteCheckBox2.Visible = true;
                    activityCheckBox2.Visible = true;
                    leadCheckBox2.Visible = true;
                    accountCheckBox2.Visible = true;
                    contactCheckBox2.Visible = true;
                    oppCheckBox2.Visible = true;
                    ticketCheckBox2.Visible = true;
                    oppUpdateCheckBox2.Visible = true;
                    completeActCheckBox2.Visible = true;
                    //promoteLeadCheckBox2.Visible = true;
                    break;
                    /*
                // Data Creator
                case 1:
                    SetRole2("Data Creator");
                    SetLabelText2("Notes Created:");
                    chooseDoc2.Visible = false;
                    activitiesCreated2.Visible = true;
                    activitiesCreatedLabel2.Visible = true;
                    activitiesCompleted2.Visible = false;
                    activitiesCompletedLabel2.Visible = false;
                    activityCompleteAmount2.Visible = false;
                    activityCompleteAmountLabel2.Visible = false;
                    upperBound2.Visible = true;
                    lowerBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    break;
                // Lead Generator
                case 2:
                    SetRole2("Lead Generator");
                    SetLabelText2("Leads Generated:");
                    chooseDoc2.Visible = false;
                    activitiesCreated2.Visible = false;
                    activitiesCreatedLabel2.Visible = false;
                    activitiesCompleted2.Visible = false;
                    activitiesCompletedLabel2.Visible = false;
                    activityCompleteAmount2.Visible = false;
                    activityCompleteAmountLabel2.Visible = false;
                    upperBound2.Visible = true;
                    lowerBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    break;
                // Helper
                case 3:
                    SetRole2("Helper");
                    SetLabelText2("Tickets Made:");
                    SetActivitiesCreatedText2("Leads Generated:");
                    SetActivitiesCompletedText2("Opportunities Made:");
                    chooseDoc2.Visible = false;
                    activitiesCreated2.Visible = true;
                    activitiesCreatedLabel2.Visible = true;
                    activitiesCompleted2.Visible = true;
                    activitiesCompletedLabel2.Visible = true;
                    activityCompleteAmount2.Visible = false;
                    activityCompleteAmountLabel2.Visible = false;
                    upperBound2.Visible = true;
                    lowerBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    break;
                case 4:
                    SetRole2("Ticket Maker");
                    SetLabelText2("Tickets Made:");
                    chooseDoc2.Visible = false;
                    activitiesCreated2.Visible = false;
                    activitiesCreatedLabel2.Visible = false;
                    activitiesCompleted2.Visible = false;
                    activitiesCompletedLabel2.Visible = false;
                    activityCompleteAmount2.Visible = false;
                    activityCompleteAmountLabel2.Visible = false;
                    upperBound2.Visible = true;
                    lowerBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    break;
                case 5:
                    SetRole2("Opportunity Generator");
                    SetLabelText2("Opportunities Made:");
                    chooseDoc2.Visible = false;
                    activitiesCreated2.Visible = false;
                    activitiesCreatedLabel2.Visible = false;
                    activitiesCompleted2.Visible = false;
                    activitiesCompletedLabel2.Visible = false;
                    activityCompleteAmount2.Visible = false;
                    activityCompleteAmountLabel2.Visible = false;
                    upperBound2.Visible = true;
                    lowerBound2.Visible = true;
                    minLabel2.Visible = true;
                    maxLabel2.Visible = true;
                    minutesLabel3.Visible = true;
                    minutesLabel4.Visible = true;
                    dataCreationTimerLabel2.Visible = true;
                    break;
                     * */
                case 1:
                    SetRole2("From GoogleDoc");
                    chooseDoc2.Visible = true;
                    /*
                    notesCreated2.Visible = false;
                    notesCreatedLabel2.Visible = false;
                    activitiesCreated2.Visible = false;
                    activitiesCreatedLabel2.Visible = false;
                    activitiesCompleted2.Visible = false;
                    activitiesCompletedLabel2.Visible = false;
                     * */
                    activityCompleteAmount2.Visible = false;
                    activityCompleteAmountLabel2.Visible = false;
                    upperBound2.Visible = false;
                    lowerBound2.Visible = false;
                    minLabel2.Visible = false;
                    maxLabel2.Visible = false;
                    minutesLabel3.Visible = false;
                    minutesLabel4.Visible = false;
                    reliabilityLabel2.Visible = false;
                    reliabilityValue2.Visible = false;
                    dataCreationTimerLabel2.Visible = false;
                    noteCheckBox2.Visible = false;
                    activityCheckBox2.Visible = false;
                    leadCheckBox2.Visible = false;
                    accountCheckBox2.Visible = false;
                    contactCheckBox2.Visible = false;
                    oppCheckBox2.Visible = false;
                    ticketCheckBox2.Visible = false;
                    oppUpdateCheckBox2.Visible = false;
                    completeActCheckBox2.Visible = false;
                    //promoteLeadCheckBox2.Visible = false;
                    break;
                case 2:
                    SetRole2("From Excel");
                    chooseDoc2.Visible = true;
                    /*
                    notesCreated2.Visible = false;
                    notesCreatedLabel2.Visible = false;
                    activitiesCreated2.Visible = false;
                    activitiesCreatedLabel2.Visible = false;
                    activitiesCompleted2.Visible = false;
                    activitiesCompletedLabel2.Visible = false;
                     * */
                    activityCompleteAmount2.Visible = false;
                    activityCompleteAmountLabel2.Visible = false;
                    upperBound2.Visible = false;
                    lowerBound2.Visible = false;
                    minLabel2.Visible = false;
                    maxLabel2.Visible = false;
                    minutesLabel3.Visible = false;
                    minutesLabel4.Visible = false;
                    reliabilityLabel2.Visible = false;
                    reliabilityValue2.Visible = false;
                    dataCreationTimerLabel2.Visible = false;
                    noteCheckBox2.Visible = false;
                    activityCheckBox2.Visible = false;
                    leadCheckBox2.Visible = false;
                    accountCheckBox2.Visible = false;
                    contactCheckBox2.Visible = false;
                    oppCheckBox2.Visible = false;
                    ticketCheckBox2.Visible = false;
                    oppUpdateCheckBox2.Visible = false;
                    completeActCheckBox2.Visible = false;
                    //promoteLeadCheckBox2.Visible = false;
                    break;
            }
        }

        // Currently unused
        private void roleSelector3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (roleSelector3.SelectedIndex)
            {
                // General
                case 0:
                    SetRole3("General");
                    SetLabelText3("Notes Created:");
                    chooseDoc3.Visible = false;
                    activitiesCreated3.Visible = true;
                    activitiesCreatedLabel3.Visible = true;
                    activitiesCompleted3.Visible = true;
                    activitiesCompletedLabel3.Visible = true;
                    activityCompleteAmount3.Visible = true;
                    activityCompleteAmountLabel3.Visible = true;
                    upperBound3.Visible = true;
                    lowerBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    reliabilityLabel3.Visible = true;
                    reliabilityValue3.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    noteCheckBox3.Visible = true;
                    activityCheckBox3.Visible = true;
                    leadCheckBox3.Visible = true;
                    accountCheckBox3.Visible = true;
                    contactCheckBox3.Visible = true;
                    oppCheckBox3.Visible = true;
                    ticketCheckBox3.Visible = true;
                    oppUpdateCheckBox3.Visible = true;
                    completeActCheckBox3.Visible = true;
                    //promoteLeadCheckBox3.Visible = true;
                    break;
                    /*
                // Data Creator
                case 1:
                    SetRole3("Data Creator");
                    SetLabelText3("Notes Created:");
                    chooseDoc3.Visible = false;
                    activitiesCreated3.Visible = true;
                    activitiesCreatedLabel3.Visible = true;
                    activitiesCompleted3.Visible = false;
                    activitiesCompletedLabel3.Visible = false;
                    activityCompleteAmount3.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    upperBound3.Visible = true;
                    lowerBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    break;
                // Lead Generator
                case 2:
                    SetRole3("Lead Generator");
                    SetLabelText3("Leads Generated:");
                    chooseDoc3.Visible = false;
                    activitiesCreated3.Visible = false;
                    activitiesCreatedLabel3.Visible = false;
                    activitiesCompleted3.Visible = false;
                    activitiesCompletedLabel3.Visible = false;
                    activityCompleteAmount3.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    upperBound3.Visible = true;
                    lowerBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    break;
                // Helper
                case 3:
                    SetRole3("Helper");
                    SetLabelText3("Tickets Made:");
                    SetActivitiesCreatedText3("Leads Generated:");
                    SetActivitiesCompletedText3("Opportunities Made:");
                    chooseDoc3.Visible = false;
                    activitiesCreated3.Visible = true;
                    activitiesCreatedLabel3.Visible = true;
                    activitiesCompleted3.Visible = true;
                    activitiesCompletedLabel3.Visible = true;
                    activityCompleteAmount3.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    upperBound3.Visible = true;
                    lowerBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    break;
                case 4:
                    SetRole3("Ticket Maker");
                    SetLabelText3("Tickets Made:");
                    chooseDoc3.Visible = false;
                    activitiesCreated3.Visible = false;
                    activitiesCreatedLabel3.Visible = false;
                    activitiesCompleted3.Visible = false;
                    activitiesCompletedLabel3.Visible = false;
                    activityCompleteAmount3.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    upperBound3.Visible = true;
                    lowerBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    break;
                case 5:
                    SetRole3("Opportunity Generator");
                    SetLabelText3("Opportunities Made:");
                    chooseDoc3.Visible = false;
                    activitiesCreated3.Visible = false;
                    activitiesCreatedLabel3.Visible = false;
                    activitiesCompleted3.Visible = false;
                    activitiesCompletedLabel3.Visible = false;
                    activityCompleteAmount3.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    upperBound3.Visible = true;
                    lowerBound3.Visible = true;
                    minLabel3.Visible = true;
                    maxLabel3.Visible = true;
                    minutesLabel5.Visible = true;
                    minutesLabel6.Visible = true;
                    dataCreationTimerLabel3.Visible = true;
                    break;
                     * */
                case 1:
                    SetRole3("From GoogleDoc");
                    chooseDoc3.Visible = true;
                    /*
                    notesCreated3.Visible = false;
                    notesCreatedLabel3.Visible = false;
                    activitiesCreated3.Visible = false;
                    activitiesCreatedLabel3.Visible = false;
                    activitiesCompleted3.Visible = false;
                    activitiesCompletedLabel3.Visible = false;
                     * */
                    activityCompleteAmount3.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    upperBound3.Visible = false;
                    lowerBound3.Visible = false;
                    minLabel3.Visible = false;
                    maxLabel3.Visible = false;
                    minutesLabel5.Visible = false;
                    minutesLabel6.Visible = false;
                    reliabilityLabel3.Visible = false;
                    reliabilityValue3.Visible = false;
                    dataCreationTimerLabel3.Visible = false;
                    noteCheckBox3.Visible = false;
                    activityCheckBox3.Visible = false;
                    leadCheckBox3.Visible = false;
                    accountCheckBox3.Visible = false;
                    contactCheckBox3.Visible = false;
                    oppCheckBox3.Visible = false;
                    ticketCheckBox3.Visible = false;
                    oppUpdateCheckBox3.Visible = false;
                    completeActCheckBox3.Visible = false;
                    //promoteLeadCheckBox3.Visible = false;
                    break;
                case 2:
                    SetRole3("From Excel");
                    chooseDoc3.Visible = true;
                    /*
                    notesCreated3.Visible = false;
                    notesCreatedLabel3.Visible = false;
                    activitiesCreated3.Visible = false;
                    activitiesCreatedLabel3.Visible = false;
                    activitiesCompleted3.Visible = false;
                    activitiesCompletedLabel3.Visible = false;
                     * */
                    activityCompleteAmount3.Visible = false;
                    activityCompleteAmountLabel3.Visible = false;
                    upperBound3.Visible = false;
                    lowerBound3.Visible = false;
                    minLabel3.Visible = false;
                    maxLabel3.Visible = false;
                    minutesLabel5.Visible = false;
                    minutesLabel6.Visible = false;
                    reliabilityLabel3.Visible = false;
                    reliabilityValue3.Visible = false;
                    dataCreationTimerLabel3.Visible = false;
                    noteCheckBox3.Visible = false;
                    activityCheckBox3.Visible = false;
                    leadCheckBox3.Visible = false;
                    accountCheckBox3.Visible = false;
                    contactCheckBox3.Visible = false;
                    oppCheckBox3.Visible = false;
                    ticketCheckBox3.Visible = false;
                    oppUpdateCheckBox3.Visible = false;
                    completeActCheckBox3.Visible = false;
                    //promoteLeadCheckBox3.Visible = false;
                    break;
            }
        }

        // Currently unused
        private void roleSelector4_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (roleSelector4.SelectedIndex)
            {
                // General
                case 0:
                    SetRole4("General");
                    SetLabelText4("Notes Created:");
                    chooseDoc4.Visible = false;
                    activitiesCreated4.Visible = true;
                    activitiesCreatedLabel4.Visible = true;
                    activitiesCompleted4.Visible = true;
                    activitiesCompletedLabel4.Visible = true;
                    activityCompleteAmount4.Visible = true;
                    activityCompleteAmountLabel4.Visible = true;
                    upperBound4.Visible = true;
                    lowerBound4.Visible = true;
                    minLabel4.Visible = true;
                    maxLabel4.Visible = true;
                    minutesLabel7.Visible = true;
                    minutesLabel8.Visible = true;
                    reliabilityLabel4.Visible = true;
                    reliabilityValue4.Visible = true;
                    dataCreationTimerLabel4.Visible = true;
                    noteCheckBox4.Visible = true;
                    activityCheckBox4.Visible = true;
                    leadCheckBox4.Visible = true;
                    accountCheckBox4.Visible = true;
                    contactCheckBox4.Visible = true;
                    oppCheckBox4.Visible = true;
                    ticketCheckBox4.Visible = true;
                    oppUpdateCheckBox4.Visible = true;
                    completeActCheckBox4.Visible = true;
                    //promoteLeadCheckBox4.Visible = true;
                    break;
                    /*
                // Data Creator
                case 1:
                    SetRole4("Data Creator");
                    SetLabelText4("Notes Created:");
                    chooseDoc4.Visible = false;
                    activitiesCreated4.Visible = true;
                    activitiesCreatedLabel4.Visible = true;
                    activitiesCompleted4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                    activityCompleteAmount4.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    upperBound4.Visible = true;
                    lowerBound4.Visible = true;
                    minLabel4.Visible = true;
                    maxLabel4.Visible = true;
                    minutesLabel7.Visible = true;
                    minutesLabel8.Visible = true;
                    dataCreationTimerLabel4.Visible = true;
                    break;
                // Lead Generator
                case 2:
                    SetRole4("Lead Generator");
                    SetLabelText4("Leads Generated:");
                    chooseDoc4.Visible = false;
                    activitiesCreated4.Visible = false;
                    activitiesCreatedLabel4.Visible = false;
                    activitiesCompleted4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                    activityCompleteAmount4.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    upperBound4.Visible = true;
                    lowerBound4.Visible = true;
                    minLabel4.Visible = true;
                    maxLabel4.Visible = true;
                    minutesLabel7.Visible = true;
                    minutesLabel8.Visible = true;
                    dataCreationTimerLabel4.Visible = true;
                    break;
                // Helper
                case 3:
                    SetRole4("Helper");
                    SetLabelText4("Tickets Made:");
                    SetActivitiesCreatedText4("Leads Generated:");
                    SetActivitiesCompletedText4("Opportunities Made:");
                    chooseDoc4.Visible = false;
                    activitiesCreated4.Visible = true;
                    activitiesCreatedLabel4.Visible = true;
                    activitiesCompleted4.Visible = true;
                    activitiesCompletedLabel4.Visible = true;
                    activityCompleteAmount4.Visible = false;
                    upperBound4.Visible = true;
                    lowerBound4.Visible = true;
                    minLabel4.Visible = true;
                    maxLabel4.Visible = true;
                    minutesLabel7.Visible = true;
                    minutesLabel8.Visible = true;
                    dataCreationTimerLabel4.Visible = true;
                    activityCompleteAmountLabel4.Visible = false;
                    break;
                case 4:
                    SetRole4("Ticket Maker");
                    SetLabelText4("Tickets Made:");
                    chooseDoc4.Visible = false;
                    activitiesCreated4.Visible = false;
                    activitiesCreatedLabel4.Visible = false;
                    activitiesCompleted4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                    activityCompleteAmount4.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    upperBound4.Visible = true;
                    lowerBound4.Visible = true;
                    minLabel4.Visible = true;
                    maxLabel4.Visible = true;
                    minutesLabel7.Visible = true;
                    minutesLabel8.Visible = true;
                    dataCreationTimerLabel4.Visible = true;
                    break;
                case 5:
                    SetRole4("Opportunity Generator");
                    SetLabelText4("Opportunities Made:");
                    chooseDoc4.Visible = false;
                    activitiesCreated4.Visible = false;
                    activitiesCreatedLabel4.Visible = false;
                    activitiesCompleted4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                    activityCompleteAmount4.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    upperBound4.Visible = true;
                    lowerBound4.Visible = true;
                    minLabel4.Visible = true;
                    maxLabel4.Visible = true;
                    minutesLabel7.Visible = true;
                    minutesLabel8.Visible = true;
                    dataCreationTimerLabel4.Visible = true;
                    break;
                     * */
                case 1:
                    SetRole4("From GoogleDoc");
                    chooseDoc4.Visible = true;
                    /*
                    notesCreated4.Visible = false;
                    notesCreatedLabel4.Visible = false;
                    activitiesCreated4.Visible = false;
                    activitiesCreatedLabel4.Visible = false;
                    activitiesCompleted4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                     * */
                    activityCompleteAmount4.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    upperBound4.Visible = false;
                    lowerBound4.Visible = false;
                    minLabel4.Visible = false;
                    maxLabel4.Visible = false;
                    minutesLabel7.Visible = false;
                    minutesLabel8.Visible = false;
                    reliabilityLabel4.Visible = false;
                    reliabilityValue4.Visible = false;
                    dataCreationTimerLabel4.Visible = false;
                    noteCheckBox4.Visible = false;
                    activityCheckBox4.Visible = false;
                    leadCheckBox4.Visible = false;
                    accountCheckBox4.Visible = false;
                    contactCheckBox4.Visible = false;
                    oppCheckBox4.Visible = false;
                    ticketCheckBox4.Visible = false;
                    oppUpdateCheckBox4.Visible = false;
                    completeActCheckBox4.Visible = false;
                    //promoteLeadCheckBox4.Visible = false;
                    break;
                case 2:
                    SetRole4("From Excel");
                    chooseDoc4.Visible = true;
                    /*
                    notesCreated4.Visible = false;
                    notesCreatedLabel4.Visible = false;
                    activitiesCreated4.Visible = false;
                    activitiesCreatedLabel4.Visible = false;
                    activitiesCompleted4.Visible = false;
                    activitiesCompletedLabel4.Visible = false;
                     * */
                    activityCompleteAmount4.Visible = false;
                    activityCompleteAmountLabel4.Visible = false;
                    upperBound4.Visible = false;
                    lowerBound4.Visible = false;
                    minLabel4.Visible = false;
                    maxLabel4.Visible = false;
                    minutesLabel7.Visible = false;
                    minutesLabel8.Visible = false;
                    reliabilityLabel4.Visible = false;
                    reliabilityValue4.Visible = false;
                    dataCreationTimerLabel4.Visible = false;
                    noteCheckBox4.Visible = false;
                    activityCheckBox4.Visible = false;
                    leadCheckBox4.Visible = false;
                    accountCheckBox4.Visible = false;
                    contactCheckBox4.Visible = false;
                    oppCheckBox4.Visible = false;
                    ticketCheckBox4.Visible = false;
                    oppUpdateCheckBox4.Visible = false;
                    completeActCheckBox4.Visible = false;
                    //promoteLeadCheckBox4.Visible = false;
                    break;
            }
        }

        // Currently unused
        private void roleSelector5_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (roleSelector5.SelectedIndex)
            {
                // General
                case 0:
                    SetRole5("General");
                    SetLabelText5("Notes Created:");
                    chooseDoc5.Visible = false;
                    activitiesCreated5.Visible = true;
                    activitiesCreatedLabel5.Visible = true;
                    activitiesCompleted5.Visible = true;
                    activitiesCompletedLabel5.Visible = true;
                    activityCompleteAmount5.Visible = true;
                    activityCompleteAmountLabel5.Visible = true;
                    upperBound5.Visible = true;
                    lowerBound5.Visible = true;
                    minLabel5.Visible = true;
                    maxLabel5.Visible = true;
                    minutesLabel9.Visible = true;
                    minutesLabel10.Visible = true;
                    reliabilityLabel5.Visible = true;
                    reliabilityValue5.Visible = true;
                    dataCreationTimerLabel5.Visible = true;
                    noteCheckBox5.Visible = true;
                    activityCheckBox5.Visible = true;
                    leadCheckBox5.Visible = true;
                    accountCheckBox5.Visible = true;
                    contactCheckBox5.Visible = true;
                    oppCheckBox5.Visible = true;
                    ticketCheckBox5.Visible = true;
                    oppUpdateCheckBox5.Visible = true;
                    completeActCheckBox5.Visible = true;
                    //promoteLeadCheckBox5.Visible = true;
                    break;
                    /*
                // Data Creator
                case 1:
                    SetRole5("Data Creator");
                    SetLabelText5("Notes Created:");
                    chooseDoc5.Visible = false;
                    activitiesCreated5.Visible = true;
                    activitiesCreatedLabel5.Visible = true;
                    activitiesCompleted5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                    activityCompleteAmount5.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    upperBound5.Visible = true;
                    lowerBound5.Visible = true;
                    minLabel5.Visible = true;
                    maxLabel5.Visible = true;
                    minutesLabel9.Visible = true;
                    minutesLabel10.Visible = true;
                    dataCreationTimerLabel5.Visible = true;
                    break;
                // Lead Generator
                case 2:
                    SetRole5("Lead Generator");
                    SetLabelText5("Leads Generated:");
                    chooseDoc5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                    activityCompleteAmount5.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    upperBound5.Visible = true;
                    lowerBound5.Visible = true;
                    minLabel5.Visible = true;
                    maxLabel5.Visible = true;
                    minutesLabel9.Visible = true;
                    minutesLabel10.Visible = true;
                    dataCreationTimerLabel5.Visible = true;
                    break;
                // Helper
                case 3:
                    SetRole5("Helper");
                    SetLabelText5("Tickets Made:");
                    SetActivitiesCreatedText5("Leads Generated:");
                    SetActivitiesCompletedText5("Opportunities Made:");
                    chooseDoc5.Visible = false;
                    activitiesCreated5.Visible = true;
                    activitiesCreatedLabel5.Visible = true;
                    activitiesCompleted5.Visible = true;
                    activitiesCompletedLabel5.Visible = true;
                    activityCompleteAmount5.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    upperBound5.Visible = true;
                    lowerBound5.Visible = true;
                    minLabel5.Visible = true;
                    maxLabel5.Visible = true;
                    minutesLabel9.Visible = true;
                    minutesLabel10.Visible = true;
                    dataCreationTimerLabel5.Visible = true;
                    break;
                case 4:
                    SetRole5("Ticket Maker");
                    SetLabelText5("Tickets Made:");
                    chooseDoc5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                    activityCompleteAmount5.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    upperBound5.Visible = true;
                    lowerBound5.Visible = true;
                    minLabel5.Visible = true;
                    maxLabel5.Visible = true;
                    minutesLabel9.Visible = true;
                    minutesLabel10.Visible = true;
                    dataCreationTimerLabel5.Visible = true;
                    break;
                case 5:
                    SetRole5("Opportunity Generator");
                    SetLabelText5("Opportunities Made:");
                    chooseDoc5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                    activityCompleteAmount5.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    upperBound5.Visible = true;
                    lowerBound5.Visible = true;
                    minLabel5.Visible = true;
                    maxLabel5.Visible = true;
                    minutesLabel9.Visible = true;
                    minutesLabel10.Visible = true;
                    dataCreationTimerLabel5.Visible = true;
                    break;
                     * */
                case 1:
                    SetRole5("From GoogleDoc");
                    chooseDoc5.Visible = true;
                    /*
                    notesCreated5.Visible = false;
                    notesCreatedLabel5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                     * */
                    activityCompleteAmount5.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    upperBound5.Visible = false;
                    lowerBound5.Visible = false;
                    minLabel5.Visible = false;
                    maxLabel5.Visible = false;
                    minutesLabel9.Visible = false;
                    minutesLabel10.Visible = false;
                    reliabilityLabel5.Visible = false;
                    reliabilityValue5.Visible = false;
                    dataCreationTimerLabel5.Visible = false;
                    noteCheckBox5.Visible = false;
                    activityCheckBox5.Visible = false;
                    leadCheckBox5.Visible = false;
                    accountCheckBox5.Visible = false;
                    contactCheckBox5.Visible = false;
                    oppCheckBox5.Visible = false;
                    ticketCheckBox5.Visible = false;
                    oppUpdateCheckBox5.Visible = false;
                    completeActCheckBox5.Visible = false;
                    //promoteLeadCheckBox5.Visible = false;
                    break;
                case 2:
                    SetRole5("From Excel");
                    chooseDoc5.Visible = true;
                    /*
                    notesCreated5.Visible = false;
                    notesCreatedLabel5.Visible = false;
                    activitiesCreated5.Visible = false;
                    activitiesCreatedLabel5.Visible = false;
                    activitiesCompleted5.Visible = false;
                    activitiesCompletedLabel5.Visible = false;
                     * */
                    activityCompleteAmount5.Visible = false;
                    activityCompleteAmountLabel5.Visible = false;
                    upperBound5.Visible = false;
                    lowerBound5.Visible = false;
                    minLabel5.Visible = false;
                    maxLabel5.Visible = false;
                    minutesLabel9.Visible = false;
                    minutesLabel10.Visible = false;
                    reliabilityLabel5.Visible = false;
                    reliabilityValue5.Visible = false;
                    dataCreationTimerLabel5.Visible = false;
                    noteCheckBox5.Visible = false;
                    activityCheckBox5.Visible = false;
                    leadCheckBox5.Visible = false;
                    accountCheckBox5.Visible = false;
                    contactCheckBox5.Visible = false;
                    oppCheckBox5.Visible = false;
                    ticketCheckBox5.Visible = false;
                    oppUpdateCheckBox5.Visible = false;
                    completeActCheckBox5.Visible = false;
                    //promoteLeadCheckBox5.Visible = false;
                    break;
            }
        }

        // Currently unused
        private void chooseDoc1_Click(object sender, EventArgs e)
        {
            string client = serverAddress.Text;
            userLabel.Text = "User: " + userIDBox.Text;
            robot = new Bot(client, shiftAM.Text, shiftPM.Text, userIDBox.Text, passwordBox.Text, progressLabel, activitiesCreated, notesCreated, activitiesCompleted, leadsCreated, accountsCreated, contactsCreated, oppsCreated, ticketsCreated, oppsUpdated, leadsPromoted, role, activityCompleteAmount.Value, roleSelector, noteCheckBox.Checked, activityCheckBox.Checked, leadCheckBox.Checked, accountCheckBox.Checked, contactCheckBox.Checked, oppCheckBox.Checked, ticketCheckBox.Checked, oppUpdateCheckBox.Checked, completeActCheckBox.Checked, promoteLeadCheckBox.Checked, reliabilityValue.Value, creationUpperBound.Text);
            robot.setStopCommand(false);
            if (role.Text == "From GoogleDoc")
                robot.runFromGoogleSpreadSheet();
            if (role.Text == "From Excel")
                robot.runFromExcel();
        }
    }
}
