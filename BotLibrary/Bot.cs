using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Framework;
using Sage.SData.Client.Metadata;

namespace BotLibrary
{
    public class Bot
    {
        #region Declaration of Variables
        protected SDataService service { get; set; }
        protected SDataService dynamic;
        protected string endPoint;
        protected SDataUri Uri { get; set; }
        protected string UserID { get; set; }
        protected string Password { get; set; }
        protected string role = "";
        protected bool firstRun;
        protected decimal activityCompleteAmount;
        protected SDataPayload activityPayload;
        protected SDataPayload contactPayload;
        protected SDataPayload actPayload;
        protected bool stopCommand = false;
        protected double reliability;
        protected int upperBoundMonth = 0;
        protected string language = "English";
        protected string fileName = "";
        protected int dirty;
        protected static Random rand;
        #endregion

        public Bot()
        {

        }

        public Bot(string userID, string password, int reliable, string endpoint, int server)
        {
            endPoint = endpoint;
            service = new SDataService(endpoint + "/sdata/slx/system/-/") { UserName = userID, Password = password };
            dynamic = new SDataService(endpoint + "/sdata/slx/dynamic/-/") { UserName = userID, Password = password };
            UserID = userID;
            Password = password;
            firstRun = true;
            activityCompleteAmount = 1;
            reliability = reliable;
            dirty = 0;
            upperBoundMonth = 2;
            rand = new Random();
            //writer = new StreamWriter(@"C:\Swiftpage\" + UserID + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".txt");
            fileName = @"C:\Swiftpage\UserLogs\" + UserID + server + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".txt";
            // Change which user creates data in Chinese (Simplified) by adding '|| UserID == "user"' after current value, or merely replace 'China' with the desired user.
            if (UserID == "China")
                language = "Chinese";
        }

        // Example site to view the templates for the different entries.
        // https://trinity.sagesaleslogixcloud.com/sdata/slx/dynamic/-/history/$template

        #region BotFunctions
        public virtual void Run()
        {
            bool ok = false;
            SDataUri Uri = new SDataUri(service.Url.ToString());

            SDataRequest request = new SDataRequest(Uri.ToString()) { UserName = UserID, Password = Password };
            try
            {
                // Simulates the Bot as only functioning during some typical work schedule. This schedule excludes any work off due to holidays and weekends...
                //if ((DateTime.Now.Hour >= startWork.Hour) && (DateTime.Now.Hour <= endWork.Hour) && (int)DateTime.Now.DayOfWeek >= 1 && (int)DateTime.Now.DayOfWeek <= 5)
                //{
                    //if (DateTime.Now.Hour == startWork.Hour)
                    //{
                        //if (DateTime.Now.Minute <= startWork.Minute)
                            //return;
                    //}
                    //if (DateTime.Now.Hour == endWork.Hour)
                    //{
                        //if (DateTime.Now.Minute >= endWork.Minute)
                            //return;
                    //}
                    // Checks to see if the bot was first commanded to run, if so demonstrates that it can connect to the server.
                    if (firstRun)
                    {
                        try
                        {
                            Log("Logging at: " + DateTime.Now + "\n================================================================\n", fileName);
                        }
                        catch (Exception e)
                        {
                            System.IO.Directory.CreateDirectory(@"c:\Swiftpage\UserLogs");
                            Log("Logging at: " + DateTime.Now + "\n================================================================\n", fileName);
                        }
                        Log("Connecting to: " + service.Url, fileName);
                        try
                        {
                            SDataResponse response = request.GetResponse();
                            ok = (response.StatusCode == HttpStatusCode.OK);
                        }
                        catch (Exception e)
                        {
                            Log("Unable to Connect for reason: " + e.ToString(), fileName);
                            return;
                        }
                        if (ok != true)
                        {
                            Log("Failed to Connect", fileName);
                            return;
                        }
                        Log("Connected", fileName);
                        firstRun = false;
                        roleSetter(UserID);
                    }
                    else
                    {
                        try
                        {
                            roleSetter(UserID);
                        }
                        catch (Exception e)
                        {
                            Log(e.ToString(), fileName);
                        }
                    }
                //}
                //else
                    //return;
            }
            catch (Sage.SData.Client.Framework.SDataException e)
            {
                Log(e.ToString(), fileName);
            }
        }

        

        // Below are the roles of the bot
        #region Roles
        // Go here to add names for which role each name will play.
        protected void roleSetter(string username)
        {
            switch (username.ToLower())
            {
                // Sales
                case "lee":
                case "derek":
                case "hans":
                case "manuel":
                case "cathy":
                case "dan":
                case "ed":
                case "john":
                case "lou":
                case "linda":
                case "georgine":
                case "kim":
                case "rajeev":
                    runSalesRep();
                    break;
                // Sales Management
                case "pam":
                case "loup":
                case "ken":
                case "jean":
                    runSalesManager();
                    break;
                // Support
                case "joan":
                case "robert":
                case "jay":
                    runSupportRep();
                    break;
                // Support Management
                case "samantha":
                    runSupportManager();
                    break;
                // Marketing
                case "larry":
                    runMarketingRep();
                    break;
                // Marketing Management
                case "brian":
                    runMarketingManager();
                    break;
                case "tom":
                    leadQual();
                    break;
                case "barb":
                case "lois":
                    leadGen();
                    break;
                default:
                    runSalesRep();
                    break;
            }
        }

        protected void runSalesRep()
        {
            
            double reliable = rand.NextDouble() * 100;
            int choice = rand.Next(5);

            if (reliable <= reliability)
            {
                switch (choice)
                {
                    case 0:
                        makeNote();
                        break;
                    case 1:
                        makeActivity();
                        break;
                    case 2:
                        makeOpportunity();
                        break;
                    case 3:
                        completeActivity();
                        break;
                    case 4:
                        updateOpportunity();
                        break;
                }
            } 
        }

        protected void runSalesManager()
        {
            
            double reliable = rand.NextDouble() * 100;
            int choice = rand.Next(5);

            if (reliable <= reliability)
            {
                switch (choice)
                {
                    case 0:
                        makeNote();
                        break;
                    case 1:
                        makeActivity();
                        break;
                    case 2:
                        makeOpportunity();
                        break;
                    case 3:
                        updateOpportunity();
                        break;
                    case 4:
                        completeActivity();
                        break;
                }
            }

        }

        protected void runSupportRep()
        {
            
            double reliable = rand.NextDouble() * 100;
            int choice = rand.Next(3);

            if (reliable <= reliability)
            {
                switch (choice)
                {
                    case 0:
                        makeNote();
                        break;
                    case 1:
                        makeActivity();
                        break;
                    case 2:
                        makeTicket();
                        break;
                    // Won't go beyond case 2 until changed above...
                    case 3:
                        makeTicketActivity();
                        break;
                    case 4:
                        completeTicketActivity();
                        break;
                }
            }
        }

        protected void runSupportManager()
        {
            
            double reliable = rand.NextDouble() * 100;
            int choice = rand.Next(3);

            if (reliable <= reliability)
            {
                switch (choice)
                {
                    case 0:
                        makeNote();
                        break;
                    case 1:
                        makeActivity();
                        break;
                    case 2:
                        makeTicket();
                        break;
                    // Won't go beyond case 2 until changed above...
                    case 3:
                        makeTicketActivity();
                        break;
                }
            }
        }

        protected void runMarketingRep()
        {
            
            double reliable = rand.NextDouble() * 100;
            int choice = rand.Next(3);

            if (reliable <= reliability)
            {
                switch (choice)
                {
                    case 0:
                        makeNote();
                        break;
                    case 1:
                        makeActivity();
                        break;
                    case 2:
                        makeCampaign();
                        break;
                    case 3:
                        updateCampaign();
                        break;
                }
            }
        }

        protected void runMarketingManager()
        {
            
            double reliable = rand.NextDouble() * 100;
            int choice = rand.Next(4);

            if (reliable <= reliability)
            {
                switch (choice)
                {
                    case 0:
                        makeNote();
                        break;
                    case 1:
                        makeActivity();
                        break;
                    case 2:
                        makeCampaign();
                        break;
                    case 3:
                        updateCampaign();
                        break;
                }
            }
        }

        protected void fieldRep()
        {
            
            // Run update opportunity. Also include random event to create activity and complete it, such as phone call, and ability to do notes.
            int choice = rand.Next(4);
            switch (choice)
            {
                case 0:
                    updateOpportunity();
                    break;
                case 1:
                    makeActivity();
                    break;
                case 2:
                    makeNote();
                    break;
                case 3:
                    completeActivity();
                    break;
                default:
                    completeActivity();
                    break;
            }
        }

        protected void leadQual()
        {
            
            double reliable = rand.NextDouble() * 100;

            if (reliable < reliability)
            {
                // Runs update lead. Include random chance to write note about current stage
                switch (dirty)
                {
                    case 0:
                        SDataPayload leadPayload = null;
                        do
                        {
                            leadPayload = fetchLead().GetSDataPayload();
                        } while (leadPayload.Values["Company"] == null);
                        int choice = rand.Next(1, 3);
                        switch (choice)
                        {
                            case 1:
                                makeNoteLead("Lead", leadPayload, "Follow-Up");
                                dirty = 0;
                                break;
                            case 2:
                                string action = "";
                                int pick = rand.Next(1, 3);
                                if (pick == 1)
                                    action = "Appointment";
                                if (pick == 2)
                                    action = "PhoneCall";
                                activityPayload = makeActivityLead("Lead", leadPayload, action);
                                dirty = 1;
                                break;
                            case 3:
                                activityPayload = makeActivityLead("Lead", leadPayload, "ColdCall");
                                dirty = 1;
                                break;
                            default:
                                makeNoteLead("Lead", leadPayload, "Follow-Up");
                                dirty = 0;
                                break;
                        }
                        break;
                    case 1:
                        int choice2 = rand.Next(1, 3);
                        switch (choice2)
                        {
                            case 1:
                                completeSpecificActivity(activityPayload);
                                dirty = 2;
                                break;
                            case 2:
                                completeSpecificActivity(activityPayload);
                                string action = "";
                                if (string.Compare((string)activityPayload.Values["Type"], "atAppointment") == 0)
                                    action = "Appointment";
                                if (string.Compare((string)activityPayload.Values["Type"], "atPhoneCall") == 0)
                                    action = "PhoneCall";
                                makeNoteLead("Activity", activityPayload, action);
                                dirty = 2;
                                break;
                            case 3:
                                completeSpecificActivity(activityPayload);
                                makeNoteLead("Activity", activityPayload, "NewActivity");
                                dirty = 0;
                                break;
                            default:
                                completeSpecificActivity(activityPayload);
                                string action2 = "";
                                if (string.Compare((string)activityPayload.Values["Type"], "atAppointment") == 0)
                                    action = "Appointment";
                                if (string.Compare((string)activityPayload.Values["Type"], "atPhoneCall") == 0)
                                    action = "PhoneCall";
                                makeNoteLead("Activity", activityPayload, action2);
                                dirty = 2;
                                break;
                        }
                        break;
                    case 2:
                        // Convert lead to contact
                        var leadId = activityPayload.Values["LeadId"];
                        // Get the lead payload then call makeContactWithName(leadPayload)
                        SDataPayload conversionPayload = null;
                        try
                        {
                            SDataResourceCollectionRequest leads = new SDataResourceCollectionRequest(dynamic)
                            {
                                ResourceKind = "leads"
                            };

                            var feed2 = leads.Read();
                            int count = feed2.Entries.Count();
                            if (count != 0)
                            {
                                int i = rand.Next(count);
                                conversionPayload = feed2.Entries.ElementAt(i).GetSDataPayload();
                            }
                        }
                        catch (Exception e)
                        {
                            Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                            //SetText("Connection to server lost... Please check your connection");
                            //this.stop();
                        }
                        contactPayload = makeContactWithName(conversionPayload);
                        // Delete the current lead
                        deleteSpecificLead(conversionPayload);
                        dirty = 3;
                        break;
                    case 3:
                        //Schedule a follow-up and make a note or just create an opportunity
                        int random = rand.Next(2);
                        if (random == 0)
                        {
                            makeActivityLead("Contact", contactPayload, "Follow-Up");
                            dirty = 4;
                        }
                        if (random == 1)
                        {
                            makeActivityLead("Contact", contactPayload, "Follow-Up");
                            makeNoteLead("Contact", contactPayload, "Follow-Up");
                            dirty = 4;
                        }
                        break;
                    case 4:
                        makeOpportunityFor(actPayload);
                        dirty = 0;
                        break;
                }
            }

        }

        protected void leadGen()
        {
            
            double reliable = rand.NextDouble() * 100;

            if (reliable < reliability)
            {
                // Runs the makelead. Creates the first activity for the lead.
                SDataPayload leadPayload = null;
                if (dirty == 0)
                {
                    leadPayload = makeLead();
                    dirty = 1;
                }
                if (dirty == 1 && leadPayload != null)
                {
                    makeActivityLead("Lead", leadPayload, "Follow-Up");
                    dirty = 0;
                }
                if (leadPayload == null)
                {
                    dirty = 0;
                }
            }
        }

        protected void marketingRep()
        {
            
            double tempReliability = reliability / 100;
            double reliable = rand.NextDouble();
            int choice = rand.Next(1, 4);

            if (reliable < tempReliability)
            {
                switch (choice)
                {
                    case 1:
                        makeNote();
                        break;
                    case 2:
                        makeActivity();
                        break;
                    case 3:
                        makeCampaign();
                        break;
                    case 4:
                        updateCampaign();
                        break;
                    case 5:
                        updateCampaign();
                        completeActivity();
                        break;
                    case 6:
                        completeActivity();
                        makeNote();
                        break;
                }
            }
        }
        #endregion

        // Functions allocated for the button interfaces, providing interactions for the user with the bot.
        public virtual void stop()
        {
            firstRun = true;
            stopCommand = true;
            //progressBar.Value = 0;
        }

        public virtual void remove()
        {
            service = null;
            Uri = null;
            UserID = null;
            Password = null;
        }
        #endregion

        // Demonstrates how to delete
        #region Deletion
        // Every function has certain business restrictions placed on it that may not allow it to complete

        // Functional
        public void deleteContact()
        {
            SDataResourceCollectionRequest collection = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "contacts"
            };
            var feed = collection.Read();
            // If you want to change the entry being deleted from the first one in the query to another, change the value of ElementAt(value)
            var entry = feed.Entries.ElementAt(0);
            var payload = entry.GetSDataPayload();
            var name = payload.Values["Name"];

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "contacts",
                Entry = entry
            };
            try
            {
                request.Delete();
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void deleteAccount()
        {
            SDataResourceCollectionRequest collection = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "accounts"
            };
            var feed = collection.Read();
            // If you want to change the entry being deleted from the first one in the query to another, change the value of ElementAt(value)
            var entry = feed.Entries.ElementAt(0);
            var payload = entry.GetSDataPayload();
            var accountName = payload.Values["AccountName"];

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "accounts",
                Entry = entry
            };
            try
            {
                request.Delete();
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void deleteSpecificAccount(SDataPayload accountPayload)
        {
            Guid guid = Guid.NewGuid();
            float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            SDataTemplateResourceRequest accountTemplate = new SDataTemplateResourceRequest(dynamic)
            {
                ResourceKind = "accounts"
            };
            var entry = accountTemplate.Read();
            entry.SetSDataPayload(accountPayload);
            var accountName = accountPayload.Values["AccountName"];


            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "accounts",
                Entry = entry
            };
            try
            {
                request.Delete();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Delete | Account | " + accountName + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void deleteActivity()
        {
            SDataResourceCollectionRequest collection = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "activities"
            };
            var feed = collection.Read();
            // If you want to change the entry being deleted from the first one in the query to another, change the value of ElementAt(value)
            var entry = feed.Entries.ElementAt(0);
            var payload = entry.GetSDataPayload();
            var activity = payload.Values["Description"];
            var startDate = payload.Values["StartDate"];

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "activities",
                Entry = entry
            };
            try
            {
                request.Delete();
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void deleteNote()
        {
            SDataResourceCollectionRequest collection = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "history"
            };
            var feed = collection.Read();
            // If you want to change the entry being deleted from the first one in the query to another, change the value of ElementAt(value)
            var entry = feed.Entries.ElementAt(0);
            var payload = entry.GetSDataPayload();
            var note = payload.Values["Description"];

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "history",
                Entry = entry
            };
            try
            {
                request.Delete();
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void deleteLead()
        {
            SDataResourceCollectionRequest collection = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "leads"
            };
            var feed = collection.Read();
            // If you want to change the entry being deleted from the first one in the query to another, change the value of ElementAt(value)
            var entry = feed.Entries.ElementAt(0);
            var payload = entry.GetSDataPayload();
            var lead = payload.Values["LeadFullName"];

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "leads",
                Entry = entry
            };
            try
            {
                request.Delete();
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        public void deleteSpecificLead(SDataPayload leadPayload)
        {
            Guid guid = Guid.NewGuid();
            float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            SDataTemplateResourceRequest leadTemplate = new SDataTemplateResourceRequest(dynamic)
            {
                ResourceKind = "leads"
            };
            var entry = leadTemplate.Read();
            entry.SetSDataPayload(leadPayload);
            var lead = leadPayload.Values["LeadFullName"];

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "leads",
                Entry = entry
            };
            try
            {
                request.Delete();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Delete | Lead | " + lead + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void deleteOpportunity()
        {
            SDataResourceCollectionRequest collection = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "opportunities"
            };
            var feed = collection.Read();
            // If you want to change the entry being deleted from the first one in the query to another, change the value of ElementAt(value)
            var entry = feed.Entries.ElementAt(0);
            var payload = entry.GetSDataPayload();
            var opportunity = payload.Values["Description"];

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "opportunities",
                Entry = entry
            };
            try
            {
                request.Delete();
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void deleteTicket()
        {
            SDataResourceCollectionRequest collection = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "tickets"
            };
            var feed = collection.Read();
            // If you want to change the entry being deleted from the first one in the query to another, change the value of ElementAt(value)
            var entry = feed.Entries.ElementAt(0);
            var payload = entry.GetSDataPayload();
            var ticket = payload.Values["TicketNumber"];

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "tickets",
                Entry = entry
            };
            try
            {
                request.Delete();
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        #endregion

        #region RandomCreation

        // Functions that create entries into the database.
        public void makeNote()
        {
            Guid guid = Guid.NewGuid();
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value
                // generator as defined below the creation functions.
                string type = "atNote";
                string category = "Note";
                string description = localize(language, "Description Generator", null, category, null, true);
                SDataPayload accountPayload = null;
                int i = 0;
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account |  | " + tempTime, fileName);


                if (i == 50)
                    return;

                string notes = localize(language, "Note Generator", accountPayload, category, description, true);

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest noteHistoryTemplate = new SDataTemplateResourceRequest(dynamic);
                noteHistoryTemplate.ResourceKind = "history";
                Sage.SData.Client.Atom.AtomEntry tempEntry = noteHistoryTemplate.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | History Template |  | " + tempTime, fileName);

                SDataPayload payload = tempEntry.GetSDataPayload();
                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["Type"] = type;
                payload.Values["Category"] = category;
                payload.Values["Description"] = description;
                payload.Values["Notes"] = notes;
                payload.Values["LongNotes"] = notes;
                payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                payload.Values["AccountId"] = accountPayload.Key;

                // Checks if there is an associated contact with the account.
                if (accountPayload.Values["Contacts"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = contact.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | Contact |  | " + tempTime, fileName);
                    SDataPayload contactPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        contactPayload = feed.Entries.ElementAt(0).GetSDataPayload();
                        payload.Values["ContactName"] = contactPayload.Values["Name"];
                        payload.Values["ContactId"] = contactPayload.Key;
                    }
                }

                // Checks if there is an associated opportunity with the account, similar to how the contact was found.
                if (accountPayload.Values["Opportunities"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest opp = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "opportunities",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = opp.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | Opportunity |  | " + tempTime, fileName);
                    SDataPayload oppPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        oppPayload = feed.Entries.ElementAt(0).GetSDataPayload();
                        payload.Values["OpportunityName"] = oppPayload.Values["Description"];
                        payload.Values["OpportunityId"] = oppPayload.Key;
                    }
                }

                // Checks if there is an associated ticket with the account, similar to how the contact was found.
                if (accountPayload.Values["Tickets"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest tick = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "tickets",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = tick.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | Opportunity |  | " + tempTime, fileName);
                    SDataPayload ticketPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        ticketPayload = feed.Entries.ElementAt(0).GetSDataPayload();
                        payload.Values["TicketNumber"] = ticketPayload.Values["TicketNumber"];
                        payload.Values["TicketId"] = ticketPayload.Key;
                    }
                }
                payload.Values["StartDate"] = DateTimeOffset.Now.ToUniversalTime();
                payload.Values["CompletedDate"] = DateTime.Now.ToUniversalTime();

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "history",
                    Entry = tempEntry
                };
                float requestPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var response = request.Create();
                SDataPayload responsePayload = response.GetSDataPayload();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                float requestTime = (after - requestPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Note | " + payload.Values["Description"] + " | " + requestTime, fileName);
                Log(DateTime.Now + " | " + guid + " | Total | Note | " + payload.Values["Description"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }

        }

        // Functional
        public void makeNoteFor(SDataPayload opportunityPayload, Guid guid)
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value generator as defined below the creation functions.
                string type = "atNote";
                string category = "Note";
                string description = localize(language, "Description Generator", null, category, null, true);
                SDataPayload key = (SDataPayload)opportunityPayload.Values["Account"];
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataSingleResourceRequest getAccount = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    ResourceSelector = "'" + key.Key + "'"
                };
                var rawr = getAccount.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account |  | " + tempTime, fileName);
                SDataPayload accountPayload = rawr.GetSDataPayload();
                string notes = localize(language, "Note Generator", accountPayload, category, description, true);

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest noteHistoryTemplate = new SDataTemplateResourceRequest(dynamic);
                noteHistoryTemplate.ResourceKind = "history";
                Sage.SData.Client.Atom.AtomEntry tempEntry = noteHistoryTemplate.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | History Template |  | " + tempTime, fileName);

                SDataPayload payload = tempEntry.GetSDataPayload();
                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["OpportunityName"] = opportunityPayload.Values["Description"];
                payload.Values["OpportunityId"] = opportunityPayload.Key;
                payload.Values["Type"] = type;
                payload.Values["Category"] = category;
                payload.Values["Description"] = description;
                payload.Values["Notes"] = notes;
                payload.Values["LongNotes"] = notes;
                payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                payload.Values["AccountId"] = accountPayload.Key;
                payload.Values["StartDate"] = DateTimeOffset.Now.ToUniversalTime();
                payload.Values["CompletedDate"] = DateTime.Now.ToUniversalTime();
                if (accountPayload.Values["Contacts"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = contact.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | Contact |  | " + tempTime, fileName);
                    SDataPayload contactPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            contactPayload = entry.GetSDataPayload();
                            break;
                        }
                        payload.Values["ContactName"] = contactPayload.Values["Name"];
                        payload.Values["ContactId"] = contactPayload.Key;
                    }
                }

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "history",
                    Entry = tempEntry
                };
                float requestPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float requestTime = (after - previous) / 1000;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Note |  | " + requestTime, fileName);
                Log(DateTime.Now + " | " + guid + " | Total | Note | " + payload.Values["Description"] + " | " + timed, fileName);

            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        public void makeNoteLead(string value, SDataPayload leadPayload, string action)
        { // Value is either Lead or Contact
            // Make a note about the lead after making the lead
            try
            {
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value generator as defined below the creation functions.
                string type = "atNote";
                string category = "Note";
                string description = localize(language, "Description Generator", null, category, null, true);
                string notes = randomNoteforLeadGenerator(value, leadPayload, action);

                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest noteHistoryTemplate = new SDataTemplateResourceRequest(dynamic);
                noteHistoryTemplate.ResourceKind = "history";
                Sage.SData.Client.Atom.AtomEntry tempEntry = noteHistoryTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | History Template |  | " + tempTime, fileName);

                SDataPayload payload = tempEntry.GetSDataPayload();
                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["Type"] = type;
                payload.Values["Category"] = category;
                payload.Values["Description"] = description;
                payload.Values["Notes"] = notes;
                payload.Values["LongNotes"] = notes;
                payload.Values["StartDate"] = DateTimeOffset.Now.ToUniversalTime();
                payload.Values["CompletedDate"] = DateTime.Now.ToUniversalTime();
                if (string.Compare("Lead", value) == 0)
                {
                    payload.Values["AccountName"] = leadPayload.Values["Company"];
                    payload.Values["LeadName"] = leadPayload.Values["FirstName"] + " " + leadPayload.Values["LastName"];
                    payload.Values["LeadId"] = leadPayload.Key;
                }
                if (string.Compare("Activity", value) == 0)
                {
                    payload.Values["AccountName"] = leadPayload.Values["AccountName"];
                    payload.Values["AccountId"] = leadPayload.Values["AccountId"];
                    payload.Values["LeadName"] = leadPayload.Values["LeadName"];
                    payload.Values["LeadId"] = leadPayload.Values["LeadId"];
                }
                if (string.Compare("Contact", value) == 0)
                {
                    SDataPayload temp = (SDataPayload)leadPayload.Values["Account"];
                    payload.Values["AccountName"] = leadPayload.Values["AccountName"];
                    payload.Values["AccountId"] = temp.Key;
                    payload.Values["ContactName"] = leadPayload.Values["Name"];
                    payload.Values["ContactId"] = leadPayload.Key;
                }

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "history",
                    Entry = tempEntry
                };
                float requestPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float requestTime = (after - previous) / 1000;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Note |  | " + requestTime, fileName);
                Log(DateTime.Now + " | " + guid + " | Total | Note | " + payload.Values["Description"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void makeActivity()
        {
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value
                // generator as defined below the creation functions.
                string temp = localize(language, "Type Generator", null, null, null, true);
                string category = localize(language, "Category Generator", null, temp, null, true);
                string description = localize(language, "Description Generator", null, temp, null, true);
                string type = "at" + temp;
                string location = localize(language, "Location Generator", null, temp, null, true);
                DateTime startTime = randomDateGenerator();
                string priority = localize(language, "Priority Generator", null, null, null, true);
                SDataPayload accountPayload = null;
                int i = 0;

                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account |  | " + tempTime, fileName);

                if (i == 50)
                    return;

                string notes = randomNoteGenerator(temp, accountPayload.Values["AccountName"].ToString(), description);
                DateTime alarm = startTime.AddMinutes(-15);

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest activityTemplate = new SDataTemplateResourceRequest(service);
                activityTemplate.ResourceKind = "activities";
                Sage.SData.Client.Atom.AtomEntry tempEntry = activityTemplate.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Activity Template |  | " + tempTime, fileName);

                SDataPayload payload = tempEntry.GetSDataPayload();

                payload.Values["Type"] = type;
                payload.Values["Category"] = category;



                if (temp != "Personal")
                {
                    // Get the program to query the server for the contact name, account name, and retrieve the respective ids for each.
                    payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                    payload.Values["AccountId"] = accountPayload.Key;
                    // Checks to make sure there is a contact associated with the account, and if so calls a request to get the payload
                    // associated to that contact; then filling in payload.Values
                    if (accountPayload.Values["Contacts"] != null)
                    {
                        tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "contacts",
                            QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                        };
                        var feed = contact.Read();
                        tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        tempTime = (tempAfter - tempPre) / 1000;
                        Log(DateTime.Now + " | " + guid + " | Get | Contact |  | " + tempTime, fileName);
                        SDataPayload contactPayload = null;
                        if (feed.Entries.Count() != 0)
                        {
                            contactPayload = feed.Entries.ElementAt(0).GetSDataPayload();
                            payload.Values["ContactName"] = contactPayload.Values["Name"];
                            payload.Values["ContactId"] = contactPayload.Key;
                        }
                    }

                    // Checks if there is an associated opportunity with the account, similar to how the contact was found.
                    if (accountPayload.Values["Opportunities"] != null)
                    {
                        tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        SDataResourceCollectionRequest opp = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "opportunities",
                            QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                        };
                        var feed2 = opp.Read();
                        tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        tempTime = (tempAfter - tempPre) / 1000;
                        Log(DateTime.Now + " | " + guid + " | Get | Opportunity |  | " + tempTime, fileName);
                        SDataPayload oppPayload = null;
                        if (feed2.Entries.Count() != 0)
                        {
                            oppPayload = feed2.Entries.ElementAt(0).GetSDataPayload();
                            payload.Values["OpportunityName"] = oppPayload.Values["Description"];
                            payload.Values["OpportunityId"] = oppPayload.Key;
                        }
                    }

                    /*
                    // Checks if there is an associated ticket with the account, similar to how the contact was found.
                    if (accountPayload.Values["Tickets"] != null)
                    {
                        SDataResourceCollectionRequest tick = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "tickets",
                            QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                        };
                        var feed = tick.Read();
                        SDataPayload ticketPayload = null;
                        if (feed.Entries.Count() != 0)
                        {
                            foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                            {
                                ticketPayload = entry.GetSDataPayload();
                                break;
                            }
                            payload.Values["TicketNumber"] = ticketPayload.Values["TicketNumber"];
                            payload.Values["TicketId"] = ticketPayload.Key;
                        }
                    }
                     * */

                    payload.Values["CreateSource"] = "Demo-Bot";
                    payload.Values["Location"] = location;
                    payload.Values["Priority"] = priority;
                    payload.Values["LongNotes"] = notes;
                    payload.Values["Notes"] = notes;
                    payload.Values["AlarmTime"] = alarm;
                }
                payload.Values["Description"] = description;
                payload.Values["StartDate"] = startTime;


                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(service)
                {
                    ResourceKind = "activities",
                    Entry = tempEntry
                };

                //Creating the entry...
                float requestPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float requestTime = (after - requestPre) / 1000;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Activity | " + payload.Values["Description"] + " | " + requestTime, fileName);
                Log(DateTime.Now + " | " + guid + " | Total | Activity | " + payload.Values["Description"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }

        }

        // Test Functionality... in development
        public void makeTicketActivity()
        {
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

                // Get the ticket payload, item payload, and rate payload
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataPayload ticketPayload = fetchTicket();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Ticket |  | " + tempTime, fileName);
                SDataPayload ratePayload = fetchTicketActivityRate();
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempPre - tempAfter) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | TicketActivityRate |  | " + tempTime, fileName);
                SDataPayload itemPayload = fetchTicketActivityItem();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | TicketActivityItem |  | " + tempTime, fileName);

                // Add activity to the ticket 
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest ticketActivityTemplate = new SDataTemplateResourceRequest(dynamic);
                ticketActivityTemplate.ResourceKind = "ticketActivities";
                Sage.SData.Client.Atom.AtomEntry tempEntry = ticketActivityTemplate.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | TicketActivityItem |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();

                payload.Values["CreateSource"] = "Demo-Bot";
                //payload.Values["ActivityTypeCode"] = ratePayload.Values["RateTypeCode"];
                payload.Values["Ticket"] = ticketPayload.Key;
                payload.Values["CreateDate"] = DateTime.Now;
                payload.Values["DateDue"] = DateTime.Now.AddDays(rand.Next(1, 100));
                //payload.Values["RateTypeDescription"] = ratePayload.Key;
                //payload.Values["Items"] = itemPayload.Key;
                payload.Values["ActivityDescription"] = "Tech problems impeding work";

                tempEntry.SetSDataPayload(payload);

                // Maybe AddTicketActivity service request
                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "ticketActivities",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var response = request.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | TicketActivity |  | " + tempTime, fileName);
                Log(DateTime.Now + " | " + guid + " | Total | TicketActivity | " + payload.Values["ActivityDescription"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void makeActivityFor(SDataPayload opportunityPayload, Guid guid)
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value generator as defined below the creation functions.
                string temp = localize(language, "Type Generator", null, null, null, true);
                string category = localize(language, "Category Generator", null, temp, null, true);
                string description = localize(language, "Description Generator", null, temp, null, true);
                string type = "at" + temp;
                string location = localize(language, "Location Generator", null, temp, null, true);
                DateTime startTime = randomDateGenerator();
                string priority = localize(language, "Priority Generator", null, null, null, true);
                SDataPayload key = (SDataPayload)opportunityPayload.Values["Account"];
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataSingleResourceRequest getAccount = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    ResourceSelector = "'" + key.Key + "'"
                };
                var rawr = getAccount.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account |  | " + tempTime, fileName);
                SDataPayload accountPayload = rawr.GetSDataPayload();
                string notes = randomNoteGenerator(temp, accountPayload.Values["AccountName"].ToString(), description);
                DateTime alarm = startTime.AddMinutes(-15);
                DateTime duration;

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest activityTemplate = new SDataTemplateResourceRequest(service);
                activityTemplate.ResourceKind = "activities";
                Sage.SData.Client.Atom.AtomEntry tempEntry = activityTemplate.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Activity Template |  | " + tempTime, fileName);

                SDataPayload payload = tempEntry.GetSDataPayload();

                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["OpportunityName"] = opportunityPayload.Values["Description"];
                payload.Values["OpportunityId"] = opportunityPayload.Key;
                payload.Values["Type"] = type;
                payload.Values["Category"] = category;
                // Get the program to query the server for the contact name, account name, and retrieve the respective ids for each.
                payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                payload.Values["AccountId"] = accountPayload.Key;
                payload.Values["Description"] = description;
                //payload.Values["Duration"] = "15 minutes";
                payload.Values["StartDate"] = startTime;
                payload.Values["Location"] = location;
                payload.Values["Priority"] = priority;
                payload.Values["LongNotes"] = notes;
                payload.Values["Notes"] = notes;
                payload.Values["AlarmTime"] = alarm;


                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(service)
                {
                    ResourceKind = "activities",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                //Creating the entry...
                request.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Activity |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Activity | " + payload.Values["Type"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        public SDataPayload makeActivityLead(string value, SDataPayload thePayload, string action)
        {
            SDataPayload activityPayload = null;
            // Make an activity about the lead after making the lead
            try
            {
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value generator as defined below the creation functions.
                string temp;
                if (action != "Follow-Up")
                {
                    temp = leadTypeGenerator();
                }
                else
                {
                    temp = "PhoneCall";
                }
                string category = leadCategoryGenerator(action);
                string description = leadDescriptionGenerator(action);
                string type = "at" + temp;
                string location = localize(language, "Location Generator", null, temp, null, true);
                DateTime startTime = randomDateGenerator();
                string priority = localize(language, "Priority Generator", null, null, null, true);
                string notes = randomNoteforLeadGenerator(value, thePayload, action);
                DateTime alarm = startTime.AddMinutes(-15);
                DateTime duration;

                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest activityTemplate = new SDataTemplateResourceRequest(service);
                activityTemplate.ResourceKind = "activities";
                Sage.SData.Client.Atom.AtomEntry tempEntry = activityTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Activity Template |  | " + tempTime, fileName);

                SDataPayload payload = tempEntry.GetSDataPayload();

                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["Type"] = type;
                payload.Values["Category"] = category;
                // Get the program to query the server for the contact name, account name, and retrieve the respective ids for each.
                payload.Values["Description"] = description;
                //payload.Values["Duration"] = "15 minutes";
                payload.Values["StartDate"] = startTime;
                payload.Values["Location"] = location;
                payload.Values["Priority"] = priority;
                payload.Values["LongNotes"] = notes;
                payload.Values["Notes"] = notes;
                payload.Values["AlarmTime"] = alarm;
                if (string.Compare(value, "Lead") == 0)
                {
                    payload.Values["LeadName"] = thePayload.Values["FirstName"] + " " + thePayload.Values["LastName"];
                    payload.Values["LeadId"] = thePayload.Key;
                    payload.Values["AccountName"] = thePayload.Values["Company"];
                }
                if (string.Compare(value, "Contact") == 0)
                {
                    payload.Values["ContactName"] = thePayload.Values["Name"];
                    payload.Values["ContactId"] = thePayload.Key;
                    payload.Values["AccountName"] = thePayload.Values["AccountName"];
                }


                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(service)
                {
                    ResourceKind = "activities",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                //Creating the entry...
                var response = request.Create();
                activityPayload = response.GetSDataPayload();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Activity |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Activity | " + payload.Values["Type"] + " | " + tempTime, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
            return activityPayload;
        }

        // Functional
        public void makeAccount()
        {
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest accountTemplate = new SDataTemplateResourceRequest(dynamic);
                accountTemplate.ResourceKind = "accounts";
                Sage.SData.Client.Atom.AtomEntry tempEntry = accountTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();
                bool checker = false;
                string accountName = "";

                do
                {
                    accountName = localize(language, "Fake Company Name", null, null, null, true);
                    try
                    {
                        tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        SDataResourceCollectionRequest check = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "accounts",
                            QueryValues = { { "where", "AccountNameUpper eq '" + accountName.ToUpper() + "'" } }
                        };
                        var feed = check.Read();
                        tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        tempTime = (tempAfter - tempPre) / 1000;
                        Log(DateTime.Now + " | " + guid + " | Check | AccountName |  | " + tempTime, fileName);
                        if (feed.Entries.Count() == 0)
                            checker = false;
                        else
                            checker = true;
                    }
                    catch (Exception e)
                    {
                        Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                    }
                } while (checker == true);


                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["AccountName"] = accountName;
                payload.Values["AccountNameUpper"] = accountName.ToUpper();
                //payload.Values["CreateDate"] = DateTime.Now;
                //payload.Values["CreateUser"] = UserID;
                payload.Values["Type"] = localize(language, "Account Type", null, null, null, true);
                payload.Values["SubType"] = getSubType();
                payload.Values["Status"] = localize(language, "Account Status", null, null, null, true);
                payload.Values["WebAddress"] = createWebAddress(accountName);
                payload.Values["MainPhone"] = phoneNumberGenerator();
                payload.Values["Employees"] = rand.Next(1000) + 1;
                SDataPayload address = fetchAddress();
                payload.Values["Address"] = address;
                string[] region = getRegion((string)address.Values["State"]);
                payload.Values["Region"] = region[0];
                string accountMan = region[1];

                SDataResourceCollectionRequest users = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "users",
                    QueryValues = { { "where", "UserName eq '" + accountMan.ToLower() + "'" } }
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var feed5 = users.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Users |  | " + tempTime, fileName);
                SDataPayload accountManager = feed5.Entries.ElementAt(0).GetSDataPayload();
                payload.Values["AccountManager"] = accountManager;

                SDataPayload owner = null;

                SDataResourceCollectionRequest owners = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "owners",
                    QueryValues = { { "where", "OwnerDescription eq '" + region[0] + "'" } }
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var feed3 = owners.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Owners |  | " + tempTime, fileName);
                owner = feed3.Entries.ElementAt(0).GetSDataPayload();

                payload.Values["Owner"] = owner;

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                request.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Account |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Account | " + payload.Values["AccountName"] + " | " + tempTime, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public SDataPayload makeAccountWithName(SDataPayload leadPayload)
        {
            SDataPayload accountPayload = null;
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest accountTemplate = new SDataTemplateResourceRequest(dynamic);
                accountTemplate.ResourceKind = "accounts";
                Sage.SData.Client.Atom.AtomEntry tempEntry = accountTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();
                string accountName = (string)leadPayload.Values["Company"];

                try
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest accounts = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "accounts",
                        QueryValues = { { "where", "AccountNameUpper eq '" + accountName.ToUpper() + "'" } }
                    };

                    var feed = accounts.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Check | AccountName |  | " + tempTime, fileName);
                    int count = feed.Entries.Count();
                    if (count != 0)
                    {
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            SDataPayload tempPayload = entry.GetSDataPayload();
                            if (string.Compare((string)tempPayload.Values["Status"], "Duplicated") == 0)
                            {
                                deleteSpecificAccount(tempPayload);
                            }
                            else
                            {
                                accountPayload = entry.GetSDataPayload();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                    //SetText("Connection to server lost... Please check your connection");
                    //this.stop();
                }
                if (accountPayload != null)
                {
                    return accountPayload;
                }

                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["AccountName"] = accountName;
                payload.Values["AccountNameUpper"] = accountName.ToUpper();
                //payload.Values["CreateDate"] = DateTime.Now;
                //payload.Values["CreateUser"] = UserID;
                payload.Values["Type"] = localize(language, "Account Type", null, null, null, true);
                payload.Values["SubType"] = getSubType();
                payload.Values["Status"] = localize(language, "Account Status", null, null, null, true);
                payload.Values["WebAddress"] = createWebAddress(accountName);
                payload.Values["MainPhone"] = phoneNumberGenerator();
                payload.Values["Employees"] = rand.Next(1000) + 1;
                SDataPayload address = fetchAddress();
                payload.Values["Address"] = address;
                string[] region = getRegion((string)address.Values["State"]);
                payload.Values["Region"] = region[0];
                string accountMan = region[1];

                SDataResourceCollectionRequest users = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "users",
                    QueryValues = { { "where", "UserName eq '" + accountMan.ToLower() + "'" } }
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var feed5 = users.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Users |  | " + tempTime, fileName);
                SDataPayload accountManager = feed5.Entries.ElementAt(0).GetSDataPayload();
                payload.Values["AccountManager"] = accountManager;

                SDataPayload owner = null;

                SDataResourceCollectionRequest owners = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "owners",
                    QueryValues = { { "where", "OwnerDescription eq '" + region[0] + "'" } }
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var feed3 = owners.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Owners |  | " + tempTime, fileName);
                owner = feed3.Entries.ElementAt(0).GetSDataPayload();

                payload.Values["Owner"] = owner;

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var response = request.Create();
                accountPayload = response.GetSDataPayload();
                actPayload = accountPayload;
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Account |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Account | " + payload.Values["AccountName"] + " | " + tempTime, fileName);
            }
            catch (Exception e) { Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName); }
            return accountPayload;
        }

        // Functional
        public void makeContact()
        {
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest contactTemplate = new SDataTemplateResourceRequest(dynamic);
                contactTemplate.ResourceKind = "contacts";
                Sage.SData.Client.Atom.AtomEntry tempEntry = contactTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Contact Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();
                SDataPayload accountPayload = null;
                int i = 0;
                do
                {
                    accountPayload = fetchAccount();
                    i++; 
                    if (i == 50)
                    {
                        return;
                    }
                } while (accountPayload == null);

                

                string firstName = localize(language, "Fake First Name", null, null, null, true);
                string lastName = localize(language, "Fake Last Name", null, null, null, true);

                if (accountPayload.Values["Contacts"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = contact.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Check | ContactName |  | " + tempTime, fileName);
                    SDataPayload contactPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            contactPayload = entry.GetSDataPayload();
                            if (contactPayload.Values["FirstName"] == firstName && contactPayload.Values["LastName"] == lastName)
                            {
                                do
                                {
                                    firstName = localize(language, "Fake First Name", null, null, null, true);
                                    lastName = localize(language, "Fake Last Name", null, null, null, true);
                                } while (contactPayload.Values["FirstName"] == firstName && contactPayload.Values["LastName"] == lastName);
                            }

                        }
                    }
                }

                string emailProvider = "gmail";
                int temp = rand.Next(0, 4);
                switch (temp)
                {
                    case 0:
                        emailProvider = "yahoo";
                        break;
                    case 1:
                        emailProvider = "gmail";
                        break;
                    case 2:
                        emailProvider = "mail";
                        break;
                    case 3:
                        emailProvider = "me";
                        break;
                    default:
                        emailProvider = "hotmail";
                        break;
                }

                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["FirstName"] = firstName;
                payload.Values["LastName"] = lastName;
                payload.Values["LastNameUpper"] = lastName.ToUpper();
                payload.Values["NameLF"] = lastName + ", " + firstName;
                payload.Values["Name"] = firstName + " " + lastName;
                payload.Values["FullName"] = lastName + " , " + firstName;
                payload.Values["NamePFL"] = " " + firstName + " " + lastName;
                payload.Values["IsPrimary"] = false;
                payload.Values["Salutation"] = firstName;
                payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                payload.Values["Account"] = accountPayload;
                payload.Values["CreateDate"] = DateTime.Now;
                payload.Values["ModifyDate"] = DateTime.Now;
                payload.Values["ModifyUser"] = UserID;
                payload.Values["CreateUser"] = UserID;
                payload.Values["Email"] = firstName + lastName + "@" + emailProvider + ".com";
                payload.Values["WorkPhone"] = phoneNumberGenerator();
                payload.Values["Mobile"] = phoneNumberGenerator();
                payload.Values["DoNotEmail"] = false;
                payload.Values["DoNotFax"] = false;
                payload.Values["DoNotMail"] = false;
                payload.Values["DoNotPhone"] = false;
                payload.Values["DoNotSolicit"] = false;
                payload.Values["IsServiceAuthorized"] = false;
                payload.Values["WebAddress"] = accountPayload.Values["WebAddress"];
                payload.Values["Status"] = "Active";
                payload.Values["Address"] = accountPayload.Values["Address"];
                /*payload.Values["Address"] = new SDataPayload
                {
                    ResourceName = "addresses",
                    Values = {
                                            {"Description", "Office"},
                                            {"CreateDate", DateTime.Now},
                                            {"CreateUser", UserID},
                                            {"IsMailing", true},
                                            {"IsPrimary", true},
                                            {"AddressType", "Billing &amp; Shipping"}
                                        }
                };*/

                payload.Values["Description"] = accountPayload.Values["Description"];
                payload.Values["PreferredContact"] = "Unknown";

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "contacts",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                request.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Contact |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Contact | " + payload.Values["Name"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }

        }

        public SDataPayload makeContactWithName(SDataPayload leadPayload)
        {
            SDataPayload contactPayload = null;
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest contactTemplate = new SDataTemplateResourceRequest(dynamic);
                contactTemplate.ResourceKind = "contacts";
                Sage.SData.Client.Atom.AtomEntry tempEntry = contactTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Contact Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();

                SDataPayload accountPayload = makeAccountWithName(leadPayload);
                string firstName = (string)leadPayload.Values["FirstName"];
                string lastName = (string)leadPayload.Values["LastName"];

                if (accountPayload.Values["Contacts"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = contact.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Check | ContactName |  | " + tempTime, fileName);
                    SDataPayload tempPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            tempPayload = entry.GetSDataPayload();
                            if (string.Compare((string)tempPayload.Values["FirstName"], firstName) == 0 && string.Compare((string)tempPayload.Values["LastName"], lastName) == 0)
                            {
                                return tempPayload;
                            }
                        }
                    }
                }

                string emailProvider = "gmail";
                int temp = rand.Next(0, 4);
                switch (temp)
                {
                    case 0:
                        emailProvider = "yahoo";
                        break;
                    case 1:
                        emailProvider = "gmail";
                        break;
                    case 2:
                        emailProvider = "mail";
                        break;
                    case 3:
                        emailProvider = "me";
                        break;
                    default:
                        emailProvider = "hotmail";
                        break;
                }

                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["FirstName"] = firstName;
                payload.Values["LastName"] = lastName;
                payload.Values["LastNameUpper"] = lastName.ToUpper();
                payload.Values["NameLF"] = lastName + ", " + firstName;
                payload.Values["Name"] = firstName + " " + lastName;
                payload.Values["FullName"] = lastName + " , " + firstName;
                payload.Values["NamePFL"] = " " + firstName + " " + lastName;
                payload.Values["IsPrimary"] = false;
                payload.Values["Salutation"] = firstName;
                payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                payload.Values["Account"] = accountPayload;
                payload.Values["CreateDate"] = DateTime.Now;
                payload.Values["ModifyDate"] = DateTime.Now;
                payload.Values["ModifyUser"] = UserID;
                payload.Values["CreateUser"] = UserID;
                payload.Values["Email"] = firstName + lastName + "@" + emailProvider + ".com";
                payload.Values["WorkPhone"] = phoneNumberGenerator();
                payload.Values["Mobile"] = phoneNumberGenerator();
                payload.Values["DoNotEmail"] = false;
                payload.Values["DoNotFax"] = false;
                payload.Values["DoNotMail"] = false;
                payload.Values["DoNotPhone"] = false;
                payload.Values["DoNotSolicit"] = false;
                payload.Values["IsServiceAuthorized"] = false;
                payload.Values["WebAddress"] = accountPayload.Values["WebAddress"];
                payload.Values["Status"] = "Active";
                payload.Values["Address"] = accountPayload.Values["Address"];
                /*payload.Values["Address"] = new SDataPayload
                {
                    ResourceName = "addresses",
                    Values = {
                                            {"Description", "Office"},
                                            {"CreateDate", DateTime.Now},
                                            {"CreateUser", UserID},
                                            {"IsMailing", true},
                                            {"IsPrimary", true},
                                            {"AddressType", "Billing &amp; Shipping"}
                                        }
                };*/

                payload.Values["Description"] = accountPayload.Values["Description"];
                payload.Values["PreferredContact"] = "Unknown";

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "contacts",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var response = request.Create();
                contactPayload = response.GetSDataPayload();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Contact |  | " + tempTime, fileName);

                SDataResourceCollectionRequest notes = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "history",
                    QueryValues = { { "where", "LeadId eq '" + leadPayload.Key + "'" } }
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var feed2 = notes.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | History |  | " + tempTime, fileName);
                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed2.Entries)
                {
                    SDataPayload tempPayload = entry.GetSDataPayload();

                    tempPayload.Values["ContactName"] = contactPayload.Values["NameLF"];
                    tempPayload.Values["ContactId"] = contactPayload.Key;
                    tempPayload.Values["LeadName"] = null;
                    tempPayload.Values["LeadId"] = null;
                    tempPayload.Values["AccountId"] = accountPayload.Key;
                    tempPayload.Values["AccountName"] = accountPayload.Values["AccountName"];

                    entry.SetSDataPayload(tempPayload);
                    SDataSingleResourceRequest request2 = new SDataSingleResourceRequest(dynamic)
                    {
                        ResourceKind = "history",
                        Entry = entry
                    };
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    var response2 = request2.Create();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Put | Note | " + tempPayload.Values["Description"] + " | " + tempTime, fileName);
                }

                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;

                /*
                SDataResourceCollectionRequest getContact = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "contacts",
                    QueryValues = { { "where", "Contact.Id eq '" + contactPayload.Key + "'" } }
                };
                var feed4 = getContact.Read();
                contactPayload = feed4.Entries.ElementAt(0).GetSDataPayload(); */

                Log(DateTime.Now + " | " + guid + " | Total | Contact | " + payload.Values["Name"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
            return contactPayload;
        }

        // Functional
        public SDataPayload makeLead()
        {
            SDataPayload leadPayload = null;
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre;
                SDataTemplateResourceRequest leadsTemplate = new SDataTemplateResourceRequest(dynamic);
                leadsTemplate.ResourceKind = "leads";

                bool checker = false;
                string firstName = "";
                string lastName = "";

                Sage.SData.Client.Atom.AtomEntry tempEntry = leadsTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Lead Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();
                // Checks to see if there is a lead with that name already created
                do
                {
                    firstName = localize(language, "Fake First Name", null, null, null, true);
                    lastName = localize(language, "Fake Last Name", null, null, null, true);
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest check = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "LastName eq '" + lastName + "'" } }
                    };
                    var feed = check.Read();
                    foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                    {
                        SDataPayload tempPayload = entry.GetSDataPayload();
                        if (string.Compare((string)tempPayload.Values["FirstName"], firstName) == 0)
                        {
                            checker = true;
                            break;
                        }
                        else
                            checker = false;
                    }
                } while (checker);
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Check | LeadName |  | " + tempTime, fileName);

                string emailProvider = "gmail";
                int temp = rand.Next(0, 4);
                switch (temp)
                {
                    case 0:
                        emailProvider = "yahoo";
                        break;
                    case 1:
                        emailProvider = "gmail";
                        break;
                    case 2:
                        emailProvider = "mail";
                        break;
                    case 3:
                        emailProvider = "me";
                        break;
                    default:
                        emailProvider = "hotmail";
                        break;

                }
                payload.Values["CreateSource"] = "Demo-Bot";
                string phone = phoneNumberGenerator();
                string company = localize(language, "Fake Company Name", null, null, null, true);
                payload.Values["CreateUser"] = UserID;
                payload.Values["CreateDate"] = DateTime.Now.ToUniversalTime();
                payload.Values["Company"] = company;
                payload.Values["Email"] = firstName.ToLower() + lastName.ToLower() + "@" + emailProvider + ".com";
                payload.Values["FirstName"] = firstName;
                payload.Values["LastName"] = lastName;
                payload.Values["LastNameUpper"] = lastName.ToUpper();
                payload.Values["Mobile"] = phone;
                payload.Values["WorkPhone"] = phone;
                payload.Values["LeadNameFirstLast"] = firstName + " " + lastName;
                payload.Values["LeadNameLastFirst"] = lastName + ", " + firstName;
                payload.Values["LeadSource"] = fetchLeadSource();
                payload.Values["Title"] = randomTitle();
                payload.Values["WebAddress"] = createWebAddress(company);
                //tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                //SDataPayload address = fetchAddress();
                //payload.Values["Address"] = address;
                //tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                //tempTime = (tempAfter - tempPre) / 1000;
                //Log(DateTime.Now + " | " + guid + " | Get | Addresses |  | " + tempTime, fileName);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "leads",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var response = request.Create();
                leadPayload = response.GetSDataPayload();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Lead |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Lead | " + payload.Values["Company"] + ", " + payload.Values["LeadNameFirstLast"] + " | " + timed, fileName);
                return leadPayload;
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
            return leadPayload;
        }

        // Roundabout way of doing this method is used in LeadQual above. promoteLead cannot be done using SData right now. (this function is not called)
        public void promoteLead()
        {
            try
            {
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest contactTemplate = new SDataTemplateResourceRequest(dynamic);
                contactTemplate.ResourceKind = "contacts";

                Sage.SData.Client.Atom.AtomEntry tempEntry = contactTemplate.Read();
                //SDataPayload payload = tempEntry.GetSDataPayload();

                Sage.SData.Client.Atom.AtomEntry leadEntry = null;
                do
                {
                    leadEntry = fetchLead();
                } while (leadEntry == null);

                SDataPayload leadPayload = leadEntry.GetSDataPayload();
                bool check = false;
                var feed = new Sage.SData.Client.Atom.AtomFeed();


                SDataPayload accountPayload = null;

                do
                {
                    try
                    {
                        SDataResourceCollectionRequest search = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "accounts",
                            QueryValues = { { "where", "AccountName eq '" + leadPayload.Values["Company"] + "'" } }
                        };

                        feed = search.Read();
                    }
                    catch { return; }
                } while (check);

                bool test = false;
                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                {
                    if (entry != null)
                    {
                        accountPayload = entry.GetSDataPayload();
                        test = true;
                        break;
                    }
                    else
                        test = false;
                }

                if (test)
                {
                    SDataServiceOperationRequest request = new SDataServiceOperationRequest(dynamic)
                    {
                        ResourceKind = "leads",
                        Entry = new Sage.SData.Client.Atom.AtomEntry(),
                        OperationName = "ConvertLeadToContact"
                    };


                    //if (leadPayload.Values["Company"] != null)
                    //{
                    //    accountPayload = makeAccountWithName((string)leadPayload.Values["Company"]);
                    //}

                    var entity = new SDataPayload()
                    {
                        Key = leadPayload.Key
                    };
                    request.Entry.SetSDataPayload(
                       new SDataPayload
                       {
                           ResourceName = "LeadConvertLeadToContact",
                           Namespace = "http://schemas.sage.com/dynamic/2007",
                           Values = {
                       {"request", new SDataPayload
                           {
                           Values = {
                               {"entity", leadPayload},
                               {"LeadId", entity},
                               {"contact", tempEntry},
                               {"account", leadPayload.Values["Company"]},
                               {"rule", ""}
                                    }
                           }
                        }
                                 }
                       });
                    request.Create();
                    float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    float timed = (after - previous) / 1000;
                    Log(DateTime.Now + " | Converted " + leadPayload.Values["FirstName"] + " " + leadPayload.Values["LastName"] + " to a contact | " + timed + " seconds", fileName);
                }
                else
                {
                    SDataServiceOperationRequest request = new SDataServiceOperationRequest(dynamic)
                    {
                        ResourceKind = "leads",
                        //Entry = leadEntry,
                        Entry = new Sage.SData.Client.Atom.AtomEntry(),
                        OperationName = "ConvertLeadToAccount"
                    };
                    var entity = new SDataPayload()
                    {
                        Key = leadPayload.Key
                    };

                    request.Entry.SetSDataPayload(
                       new SDataPayload
                       {
                           ResourceName = "LeadConvertLeadToAccount",
                           Namespace = "http://schemas.sage.com/dynamic/2007",
                           Values = {
                       {"request", new SDataPayload
                           {
                           Values = {
                               {"entity", leadPayload},
                               {"LeadId", entity},
                               {"contact", tempEntry},
                               {"account", accountPayload.Key},
                               {"rule", ""}
                                    }
                           }
                        }
                                 }
                       });
                    request.Create();
                    float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    float timed = (after - previous) / 1000;
                    Log(DateTime.Now + " | Converted " + leadPayload.Values["FirstName"] + " " + leadPayload.Values["LastName"]
                        + " to a contact with Account " + leadPayload.Values["Company"] + " | " + timed + " seconds", fileName);
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void makeOpportunity()
        {
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre;
                SDataTemplateResourceRequest opportunityTemplate = new SDataTemplateResourceRequest(dynamic);
                opportunityTemplate.ResourceKind = "opportunities";

                Sage.SData.Client.Atom.AtomEntry tempEntry = opportunityTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Opportunity Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();

                SDataPayload accountPayload = null;
                int i = 0;
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account |  | " + tempTime, fileName);

                if (i == 50)
                    return;

                int oppValue = 500 * rand.Next(5, 1000);
                DateTime closeDate = DateTime.Now;
                closeDate = closeDate.AddMonths(3);
                int month = rand.Next(0, 12);
                int day = rand.Next(0, 30);
                closeDate = closeDate.AddMonths(month);
                closeDate = closeDate.AddDays(day);

                string type = "";
                int x = rand.Next(1, 2);
                switch (language)
                {
                    case "English":
                        if (x == 1)
                            type = "Add-On";
                        else
                            type = "New";
                        break;
                    case "Chinese":
                        if (x == 1)
                            type = "附加";
                        else
                            type = "新";
                        break;
                }

                /*
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | CurrentUser |  | " + tempTime, fileName);
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"]; */

                //payload.Values["ActualAmount"] = oppValue;
                payload.Values["CreateSource"] = "Demo-Bot";
                payload.Values["CreateUser"] = UserID;
                payload.Values["Description"] = accountPayload.Values["AccountName"] + " - Phase " + rand.Next(0, 10) + "." + rand.Next(0,10);
                payload.Values["Account"] = accountPayload;
                payload.Values["Owner"] = accountPayload.Values["Owner"];
                //payload.Values["SalesAmount"] = oppValue;
                payload.Values["SalesPotential"] = oppValue;
                payload.Values["CloseProbability"] = 1;//5 * rand.Next(0, 20);
                payload.Values["EstimatedClose"] = closeDate;
                payload.Values["Stage"] = "1-Prospect";
                payload.Values["LeadSource"] = fetchLeadSource();
                payload.Values["Type"] = type;
                payload.Values["AccountManager"] = accountPayload.Values["AccountManager"];
                payload.Values["Campaign"] = fetchCampaign();
                //payload.Values["Weighted"] = oppValue / 100;
                //payload.Values["OverrideSalesPotential"] = false;
                //payload.Values["EstimatedClose"] = randomDateGenerator();

                if (accountPayload.Values["Contacts"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataBatchRequest contact = new SDataBatchRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | ContactBatch |  | " + tempTime, fileName);

                    /*
                    var feed = contact.Read();
                    SDataPayload contactPayload = ;
                    if (feed.Entries.Count() != 0)
                    {
                        int i = 1;
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            contactPayload.Values["Contact" + i] = entry.GetSDataPayload();
                            i++;
                        } */
                    payload.Values["Contacts"] = contact;
                    //}
                }

                tempEntry.SetSDataPayload(payload);

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "opportunities",
                    Entry = tempEntry
                };
                request.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Opportunity | " + accountPayload.Values["AccountName"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        public void makeOpportunityFor(SDataPayload accountPayload)
        {
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre;
                SDataTemplateResourceRequest opportunityTemplate = new SDataTemplateResourceRequest(dynamic);
                opportunityTemplate.ResourceKind = "opportunities";
                SDataPayload address = (SDataPayload)accountPayload.Values["Address"];

                if (address.Values.Count == 0)
                {
                    accountPayload = updateAccount(accountPayload);
                }

                Sage.SData.Client.Atom.AtomEntry tempEntry = opportunityTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Opportunity Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();

                int oppValue = 500 * rand.Next(5, 1000);
                DateTime closeDate = DateTime.Now;
                closeDate = closeDate.AddMonths(3);
                int month = rand.Next(0, 12);
                int day = rand.Next(0, 30);
                closeDate = closeDate.AddMonths(month);
                closeDate = closeDate.AddDays(day);

                string type = "";
                int x = rand.Next(1, 2);
                switch (language)
                {
                    case "English":
                        if (x == 1)
                            type = "Add-On";
                        else
                            type = "New";
                        break;
                    case "Chinese":
                        if (x == 1)
                            type = "附加";
                        else
                            type = "新";
                        break;
                }

                /*
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | CurrentUser |  | " + tempTime, fileName);
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"]; */

                payload.Values["CreateSource"] = "Demo-Bot";
                //payload.Values["ActualAmount"] = oppValue;
                //payload.Values["CreateUser"] = UserID;
                payload.Values["Description"] = accountPayload.Values["AccountName"] + " - Phase " + rand.Next(0, 10);
                payload.Values["Account"] = accountPayload;
                payload.Values["Owner"] = accountPayload.Values["Owner"];
                //payload.Values["SalesAmount"] = oppValue;
                payload.Values["SalesPotential"] = oppValue;
                payload.Values["CloseProbability"] = 1;//5 * rand.Next(0, 20);
                payload.Values["EstimatedClose"] = closeDate;
                payload.Values["Stage"] = "1-Prospect";
                payload.Values["LeadSource"] = fetchLeadSource();
                payload.Values["Type"] = type;
                payload.Values["AccountManager"] = accountPayload.Values["AccountManager"];
                payload.Values["Campaign"] = fetchCampaign();
                //payload.Values["Weighted"] = oppValue / 100;
                //payload.Values["OverrideSalesPotential"] = false;
                //payload.Values["EstimatedClose"] = randomDateGenerator();

                /*
                if (accountPayload.Values["Contacts"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataSingleResourceRequest contact = new SDataSingleResourceRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = contact.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | ContactBatch |  | " + tempTime, fileName);

                    var feed = contact.Read();
                    SDataPayload contactPayload = ;
                    if (feed.Entries.Count() != 0)
                    {
                        int i = 1;
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            contactPayload.Values["Contact" + i] = entry.GetSDataPayload();
                            i++;
                        } 
                    payload.Values["Contacts"] = feed;
                    //}
                } */

                tempEntry.SetSDataPayload(payload);

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "opportunities",
                    Entry = tempEntry
                };
                request.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Opportunity | " + accountPayload.Values["AccountName"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        public void makeMarketingOpportunity()
        {
            try
            {
                
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre;
                SDataTemplateResourceRequest opportunityTemplate = new SDataTemplateResourceRequest(dynamic);
                opportunityTemplate.ResourceKind = "opportunities";

                Sage.SData.Client.Atom.AtomEntry tempEntry = opportunityTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Opportunity Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();

                SDataPayload accountPayload = null;
                int i = 0;
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account |  | " + tempTime, fileName);

                if (i == 50)
                    return;

                int oppValue = 500 * rand.Next(5, 1000);
                DateTime closeDate = DateTime.Now;
                closeDate = closeDate.AddMonths(3);
                int month = rand.Next(0, 12);
                int day = rand.Next(0, 30);
                closeDate = closeDate.AddMonths(month);
                closeDate = closeDate.AddDays(day);

                string type = "";
                int x = rand.Next(1, 2);
                switch (language)
                {
                    case "English":
                        if (x == 1)
                            type = "Add-On";
                        else
                            type = "New";
                        break;
                    case "Chinese":
                        if (x == 1)
                            type = "附加";
                        else
                            type = "新";
                        break;
                }

                /*
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | CurrentUser |  | " + tempTime, fileName);
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"]; */

                payload.Values["CreateSource"] = "Demo-Bot";
                //payload.Values["ActualAmount"] = oppValue;
                payload.Values["CreateUser"] = UserID;
                payload.Values["Description"] = accountPayload.Values["AccountName"] + " - Phase " + rand.Next(0, 10);
                payload.Values["Account"] = accountPayload;
                payload.Values["Owner"] = accountPayload.Values["Owner"];
                //payload.Values["SalesAmount"] = oppValue;
                payload.Values["SalesPotential"] = oppValue;
                payload.Values["CloseProbability"] = 1;//5 * rand.Next(0, 20);
                payload.Values["EstimatedClose"] = closeDate;
                payload.Values["Stage"] = "1-Prospect";
                payload.Values["LeadSource"] = fetchLeadSource();
                payload.Values["Type"] = type;
                payload.Values["AccountManager"] = accountPayload.Values["AccountManager"];
                payload.Values["Campaign"] = fetchCampaign();
                //payload.Values["Weighted"] = oppValue / 100;
                //payload.Values["OverrideSalesPotential"] = false;
                //payload.Values["EstimatedClose"] = randomDateGenerator();

                if (accountPayload.Values["Contacts"] != null)
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataBatchRequest contact = new SDataBatchRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | ContactBatch |  | " + tempTime, fileName);

                    /*
                    var feed = contact.Read();
                    SDataPayload contactPayload = ;
                    if (feed.Entries.Count() != 0)
                    {
                        int i = 1;
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            contactPayload.Values["Contact" + i] = entry.GetSDataPayload();
                            i++;
                        } */
                    payload.Values["Contacts"] = contact;
                    //}
                }

                tempEntry.SetSDataPayload(payload);

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "opportunities",
                    Entry = tempEntry
                };
                request.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Opportunity | " + accountPayload.Values["AccountName"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public void makeTicket()
        {
            try
            {
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempPre;
                SDataTemplateResourceRequest ticketTemplate = new SDataTemplateResourceRequest(dynamic);
                ticketTemplate.ResourceKind = "tickets";

                Sage.SData.Client.Atom.AtomEntry tempEntry = ticketTemplate.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Ticket Template |  | " + tempTime, fileName);
                SDataPayload payload = tempEntry.GetSDataPayload();

                payload.Values["CreateSource"] = "Demo-Bot";

                SDataPayload accountPayload = null;
                int j = 0;
                var contacts = "";
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                do
                {
                    accountPayload = fetchAccount();
                    contacts = (String)accountPayload.Values["Contacts"];
                    j++;
                } while (accountPayload == null && contacts == null && j < 50);
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Account |  | " + tempTime, fileName);
                //accountPayload.Values["UserField1"] = UserID;
                if (j == 50)
                    return;

                // Only need account name for the payload to be complete
                payload.Values["Account"] = accountPayload;
                try
                {
                    //if (accountPayload.Values["Contacts"] != null)
                    //{
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = contact.Read();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | Contact |  | " + tempTime, fileName);
                    SDataPayload contactPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        contactPayload = feed.Entries.ElementAt(0).GetSDataPayload();
                        payload.Values["Contact"] = contactPayload;
                    }
                    //}
                    /*
                else
                {
                    int i = rand.Next(0, 150);
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "startIndex", i.ToString() } }
                    };
                    var feed = contact.Read();
                    SDataPayload contactPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            contactPayload = entry.GetSDataPayload();
                            break;
                        }
                        payload.Values["Contact"] = contactPayload;
                    }
                } */
                }
                catch (Exception e)
                {
                    Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                }

                tempEntry.SetSDataPayload(payload);

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "tickets",
                    Entry = tempEntry
                };
                request.Create();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Ticket |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Push | Ticket | " + accountPayload.Values["AccountName"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }

        }

        // Test Functionality... in development
        public void updateTicket()
        {
            try
            {
                
                // Include adding TicketActivity and completing TicketActivities
                int chance = rand.Next(0, 7);
                if (chance <= 7)
                {
                    makeTicketActivity();
                }
                chance = rand.Next(0, 7);
                if (chance <= 7)
                {
                    completeTicketActivity();
                }
            }
            catch (Exception e)
            {
                Log(e.ToString(), fileName);
            }

        }

        // Test Functionality... in development
        public void completeTicketActivity()
        {
            try
            {
                // Initiates a value to keep track of amount of activities created.
                int i = 0;
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

                var request = new SDataServiceOperationRequest(dynamic)
                {
                    ResourceKind = "ticketActivities",
                    OperationName = "CompleteTicketActivity",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };

                // From the Whitepaper pdf to get the user payload
                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"];

                SDataResourceCollectionRequest ticketActivities = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "ticketActivities",
                    QueryValues = { { "where", "CreateUser eq '" + userPayload.Values["userId"] + "'" } }
                };

                var feed = ticketActivities.Read();

                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                {
                    var payload = entry.GetSDataPayload();
                    var time = payload.Values["AssignedDate"];
                    DateTime stime = Convert.ToDateTime(time);

                    if (payload != null)
                    {
                        if (DateTime.Compare(stime, DateTime.Now) < 0)
                        {
                            if (i >= 1)
                                previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

                            // Change in the future to allow for multiple completion of ticket activities (similar to regular activities)
                            if (1 == i)
                                break;

                            try
                            {
                                var entity = new SDataPayload()
                                {
                                    Key = payload.Key
                                };
                                request.Entry.SetSDataPayload(
                                   new SDataPayload
                                   {
                                       ResourceName = "TicketActivityComplete",
                                       Namespace = "http://schemas.sage.com/slx/system/2010",
                                       Values = {
                       {"Request", new SDataPayload
                           {
                           Values = {
                               {"Entity", entity},
                               {"UserId", userPayload.Values["userId"]},
                               {"Result", "Complete"},
                               {"CompleteDate", DateTime.Now.ToUniversalTime()}
                                    }
                           }
                        }
                                 }
                                   });
                                var response = request.Create();
                                var responsePayload = response.GetSDataPayload();
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;
                                Log(DateTime.Now + " | Completed Ticket Activity: " + payload.Values["Description"] + " | " + timed + "seconds", fileName);
                                i++;
                            }
                            catch (Exception e)
                            {
                                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                            }
                        }
                    }

                }
            }
            catch (ArgumentNullException e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        // Functional
        public SDataPayload completeActivity()
        {
            SDataPayload activityPayload = null;
            try
            {
                Guid guid = Guid.NewGuid();
                // Initiates a value to keep track of amount of activities created.
                int i = 0;
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

                var request = new SDataServiceOperationRequest(service)
                {
                    ResourceKind = "activities",
                    OperationName = "Complete",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };

                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // From the Whitepaper pdf to get the user payload
                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"];
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | CurrentUser |  | " + tempTime, fileName);

                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataResourceCollectionRequest activities = new SDataResourceCollectionRequest(service)
                {
                    ResourceKind = "activities",
                    QueryValues = { { "where", "Leader eq '" + userPayload.Values["userId"] + "'" }, { "orderBy", "StartDate" } }
                };

                var feed = activities.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Activities |  | " + tempTime, fileName);

                // From the Whitepaper pdf to get the user payload
                //var getUserRequest = new SDataServiceOperationRequest(service)
                //{ OperationName = "getCurrentUser", 
                // Entry = new Sage.SData.Client.Atom.AtomEntry() }; 
                // var temp = getUserRequest.Create();
                // var userPayload = temp.GetSDataPayload();
                // userPayload = (SDataPayload)userPayload.Values["response"];
                // var user = userPayload.Values["userName"];
                // string userID = user.ToString().ToLower();


                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                {
                    var payload = entry.GetSDataPayload();
                    var time = payload.Values["StartDate"];
                    DateTime stime = Convert.ToDateTime(time);
                    bool allow = true;

                    // Can the user complete personal activities?
                    if (UserID == "admin" && ((string)payload.Values["Type"] == "atPersonal" || (string)payload.Values["Type"] == "个人"))
                        allow = false;


                    // Checks if the amount of activities created is equal to the amount desired.
                    // Current problem resultant from changing the service to /system/
                    if (allow && payload != null)//(string)payload.Values["Description"] != "")
                    {
                        if (DateTime.Compare(stime, DateTime.Now) < 0)
                        {
                            if (i >= 1)
                                previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

                            if (activityCompleteAmount == i)
                                break;

                            try
                            {
                                var entity = new SDataPayload()
                                {
                                    Key = payload.Key
                                };
                                request.Entry.SetSDataPayload(
                                   new SDataPayload
                                   {
                                       ResourceName = "ActivityComplete",
                                       Namespace = "http://schemas.sage.com/slx/system/2010",
                                       Values = {
                       {"Request", new SDataPayload
                           {
                           Values = {
                               {"Entity", entity},
                               {"UserId", userPayload.Values["userId"]},
                               {"Result", "Complete"},
                               {"CompleteDate", DateTime.Now.ToUniversalTime()}
                                    }
                           }
                        }
                                 }
                                   });
                                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                var response = request.Create();
                                activityPayload = response.GetSDataPayload();
                                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                tempTime = (tempAfter - tempPre) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Push | CompleteActivity |  | " + tempTime, fileName);
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Total | CompleteActivity | " + payload.Values["Description"] + " | " + timed, fileName);
                                i++;
                            }
                            catch (Exception e)
                            {
                                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                            }
                        }
                    }

                }
            }
            catch (ArgumentNullException e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
            return activityPayload;
        }

        public SDataPayload completeSpecificActivity(SDataPayload activityPayload)
        {
            SDataPayload completedActivityPayload = null;
            try
            {
                Guid guid = Guid.NewGuid();
                // Initiates a value to keep track of amount of activities created.
                int i = 0;
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

                var request = new SDataServiceOperationRequest(service)
                {
                    ResourceKind = "activities",
                    OperationName = "Complete",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };

                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // From the Whitepaper pdf to get the user payload
                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"];
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | CurrentUser |  | " + tempTime, fileName);


                if (activityPayload != null)//(string)payload.Values["Description"] != "")
                {
                    try
                    {
                        var entity = new SDataPayload()
                        {
                            Key = activityPayload.Key
                        };
                        request.Entry.SetSDataPayload(
                           new SDataPayload
                           {
                               ResourceName = "ActivityComplete",
                               Namespace = "http://schemas.sage.com/slx/system/2010",
                               Values = {
                       {"Request", new SDataPayload
                           {
                           Values = {
                               {"Entity", entity},
                               {"UserId", userPayload.Values["userId"]},
                               {"Result", "Complete"},
                               {"CompleteDate", DateTime.Now.ToUniversalTime()}
                                    }
                           }
                        }
                                 }
                           });
                        tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        var response = request.Create();
                        completedActivityPayload = (SDataPayload)response.GetSDataPayload().Values["Response"];
                        tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        tempTime = (tempAfter - tempPre) / 1000;
                        Log(DateTime.Now + " | " + guid + " | Push | CompleteActivity |  | " + tempTime, fileName);
                        float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        float timed = (after - previous) / 1000;
                        Log(DateTime.Now + " | " + guid + " | Total | CompleteActivity | " + activityPayload.Values["Description"] + " | " + timed, fileName);
                        i++;
                    }
                    catch (Exception e)
                    {
                        Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                    }
                }

            }
            catch (ArgumentNullException e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
            return completedActivityPayload;
        }

        // Functional, updated to include transitions between stages...
        public void updateOpportunity()
        {
            try
            {
                Guid guid = Guid.NewGuid();
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                Sage.SData.Client.Atom.AtomEntry entry = null;
                SDataPayload payload = null;
                int counter = 0;
                int counter2 = 0;
                bool checker = false;
                float tempPre;
                float tempAfter;
                float tempTime;
                do
                {
                    checker = false;
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    counter = 0;
                    // Get a valid Opportunity from the Users list of opportunities, ensure that the opportunity is Open (this is because the fetchOpportunity
                    // function only gets the opportunities for the currentUser and doesn't limit it based on the Status being Open)
                    do
                    {
                        entry = fetchOpportunity();
                        counter++;
                        if (counter == 50)
                        {
                            Log("Unable to locate a valid opportunity at " + DateTime.Now, fileName);
                            return;
                        }
                    } while (entry == null && (string)entry.GetSDataPayload().Values["Status"] != "Open");
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Get | Opportunity |  | " + tempTime, fileName);
                    payload = entry.GetSDataPayload();
                    counter2++;
                    if (counter2 == 50)
                    {
                        Log("Unable to locate a valid opportunity (second do-while) at " + DateTime.Now, fileName);
                        return;
                    }
                    string temp = payload.Values["SalesPotential"].ToString();

                    if (temp == "0.0000")
                        checker = true;
                } while (string.Compare((string)payload.Values["Closed"],"true") == 0 || checker == true);

                int x = rand.Next(0, 100);

                // If the opportunity is near closing, add it to the forecast
                if (x < (Convert.ToInt32(payload.Values["CloseProbability"]) + 20) && payload.Values["AddToForecast"].ToString() == "false")
                {
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    double days = rand.Next(7, 31);
                    int months = rand.Next(2);
                    DateTime closeDate = DateTime.Now;
                    closeDate = closeDate.AddDays(days);
                    closeDate = closeDate.AddMonths(months);
                    payload.Values["AddToForecast"] = true;
                    payload.Values["EstimatedClose"] = closeDate;
                    payload.Values["EstimatedCloseDateProbability"] = payload.Values["CloseProbability"];
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Put | Opportunity | Added " + payload.Values["Description"] + " to forecast | " + tempTime, fileName);
                }

                // If the close probability is 100 (went through all of the stages) then close the opportunity with a win
                if ((string)payload.Values["CloseProbability"] == 100.ToString())
                {
                    string reason = localize(language, "Reason", null, null, null, true);
                    payload.Values["Closed"] = true;
                    payload.Values["CloseProbability"] = 100;
                    payload.Values["ActualClose"] = DateTime.Now;
                    payload.Values["Win"] = true;
                    payload.Values["Reason"] = reason;
                    switch (language)
                    {
                        case "English":
                            payload.Values["Status"] = "Closed - Won";
                            break;
                        case "Chinese":
                            payload.Values["Status"] = "关闭 - 韩元";
                            break;
                    }
                    payload.Values["SalesAmount"] = payload.Values["SalesPotential"].ToString();
                    payload.Values["ActualAmount"] = payload.Values["SalesAmount"].ToString();

                    entry.SetSDataPayload(payload);

                    SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                    {
                        ResourceKind = "opportunities",
                        Entry = entry
                    };
                    tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    request.Update();
                    tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    tempTime = (tempAfter - tempPre) / 1000;
                    Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                    float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    float timed = (after - previous) / 1000;

                    Log(DateTime.Now + " | " + guid + " | Put | Opportunity | Updated Opp with win: " + payload.Values["Description"] + " | " + timed, fileName);
                    return;
                }
                else
                {
                    double choice = 5 * rand.NextDouble();
                    if (choice < 1.6)
                    {
                        if (choice < .3)
                        {
                            // Close the opportunity (either winning or losing it based on the 'close prob')
                            int luckyOne = Convert.ToInt32(payload.Values["CloseProbability"]) + 10;
                            int testMyLuck = rand.Next(0, 100);
                            if (testMyLuck <= luckyOne)
                            {
                                // Winner winner! Close the opportunity with a win.
                                string reason = localize(language, "Reason", null, null, null, true);
                                payload.Values["Closed"] = true;
                                payload.Values["CloseProbability"] = 100;
                                payload.Values["ActualClose"] = DateTime.Now;
                                payload.Values["Win"] = true;
                                payload.Values["Reason"] = reason;
                                switch (language)
                                {
                                    case "English":
                                        payload.Values["Status"] = "Closed - Won";
                                        break;
                                    case "Chinese":
                                        payload.Values["Status"] = "关闭 - 韩元";
                                        break;
                                }
                                payload.Values["SalesAmount"] = payload.Values["SalesPotential"].ToString();
                                payload.Values["ActualAmount"] = payload.Values["SalesAmount"].ToString();

                                entry.SetSDataPayload(payload);

                                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                                {
                                    ResourceKind = "opportunities",
                                    Entry = entry
                                };
                                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                request.Update();
                                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                tempTime = (tempAfter - tempPre) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;

                                Log(DateTime.Now + " | " + guid + " | Put | Opportunity | Updated Opp with win: " + payload.Values["Description"] + " | " + timed, fileName);
                                return;
                            }
                            else
                            {
                                // Ruh roh! You lost, close the opportunity with a 'lost' value
                                string reason = localize(language, "Reason", null, null, null, false);
                                payload.Values["Closed"] = true;
                                payload.Values["CloseProbability"] = 0;
                                payload.Values["ActualClose"] = DateTime.Now;
                                payload.Values["Win"] = false;
                                payload.Values["Reason"] = reason;
                                switch (language)
                                {
                                    case "English":
                                        payload.Values["Status"] = "Closed - Lost";
                                        break;
                                    case "Chinese":
                                        payload.Values["Status"] = "关闭 - 失落";
                                        break;
                                }

                                entry.SetSDataPayload(payload);

                                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                                {
                                    ResourceKind = "opportunities",
                                    Entry = entry
                                };
                                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                request.Update();
                                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                tempTime = (tempAfter - tempPre) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Put | Opportunity | Updated Opp with loss: " + payload.Values["Description"] + " | " + tempTime, fileName);
                                return;
                            }
                        }
                        else
                        {
                            // Random occurance of the opportunity being lost
                            float after;
                            float timed;
                            int unlucky = rand.Next(0, 100);
                            if (unlucky < 3)
                            {
                                string reason = localize(language, "Reason", null, null, null, false);
                                payload.Values["Closed"] = true;
                                payload.Values["CloseProbability"] = 0;
                                payload.Values["ActualClose"] = DateTime.Now;
                                payload.Values["Win"] = false;
                                payload.Values["Reason"] = reason;
                                payload.Values["Status"] = "Closed - Lost";

                                entry.SetSDataPayload(payload);

                                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                                {
                                    ResourceKind = "opportunities",
                                    Entry = entry
                                };
                                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                request.Update();
                                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                tempTime = (tempAfter - tempPre) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                                after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                timed = (after - previous) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Put | Opportunity | Updated Opp with loss: " + payload.Values["Description"] + " | " + timed, fileName);
                                return;
                            }
                            SDataPayload account = (SDataPayload)payload.Values["Account"];
                            if (string.Compare((string)account.Values["AccountName"], "") == 0)
                            {
                                // Add a note to the opportunity
                                makeNoteFor(payload, guid);
                                after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                timed = (after - previous) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Total | Note | For: " + payload.Values["Description"] + " | " + timed, fileName);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (choice >= 2.9)
                        {
                            // Updates the sales process
                            localize(language, "Progress Stage", payload, null, null, true);

                            entry.SetSDataPayload(payload);

                            SDataSingleResourceRequest requested = new SDataSingleResourceRequest(dynamic)
                            {
                                ResourceKind = "opportunities",
                                Entry = entry
                            };
                            tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                            requested.Update();
                            tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                            tempTime = (tempAfter - tempPre) / 1000;
                            Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                            float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                            float timed = (after - previous) / 1000;
                            Log(DateTime.Now + " | " + guid + " | Put | Opportunity | " + payload.Values["Description"] + " moved to the next stage | " + timed, fileName);
                            return;
                        }
                        else
                        {
                            // Another random chance of being unlucky and losing the opportunity
                            int unlucky = rand.Next(0, 100);
                            float after;
                            float timed;
                            if (unlucky < 3)
                            {
                                string reason = localize(language, "Reason", null, null, null, false);
                                payload.Values["Closed"] = true;
                                payload.Values["CloseProbability"] = 0;
                                payload.Values["ActualClose"] = DateTime.Now;
                                payload.Values["Win"] = false;
                                payload.Values["Reason"] = reason;
                                switch (language)
                                {
                                    case "English":
                                        payload.Values["Status"] = "Closed - Lost";
                                        break;
                                    case "Chinese":
                                        payload.Values["Status"] = "关闭 - 失落";
                                        break;
                                }

                                entry.SetSDataPayload(payload);

                                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                                {
                                    ResourceKind = "opportunities",
                                    Entry = entry
                                };
                                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                request.Update();
                                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                tempTime = (tempAfter - tempPre) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Push | Opportunity |  | " + tempTime, fileName);
                                after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                timed = (after - previous) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Put | Opportunity | " + payload.Values["Description"] + " updated with loss | " + timed, fileName);
                                return;
                            } 
                            SDataPayload account = (SDataPayload)payload.Values["Account"];
                            if (string.Compare((string)account.Values["AccountName"], "") == 0)
                            {
                                // Add an activity to the opportunity
                                makeActivityFor(payload, guid);
                                after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                timed = (after - previous) / 1000;
                                Log(DateTime.Now + " | " + guid + " | Total | Activity |  For: " + payload.Values["Description"] + " | " + timed, fileName);
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        public void makeProduct()
        {
            SDataTemplateResourceRequest productsTemplate = new SDataTemplateResourceRequest(dynamic);
            productsTemplate.ResourceKind = "products";
            Sage.SData.Client.Atom.AtomEntry tempEntry = productsTemplate.Read();
            SDataPayload payload = tempEntry.GetSDataPayload();

            //payload.Values["Description"] = 

            //tempEntry.SetSDataPayload(payload);

            // Maybe AddTicketActivity service request
            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "products",
                Entry = tempEntry
            };
            var response = request.Create();
        }

        public void updateProduct()
        {

        }

        public void makeCampaign()
        {
            try
            {
                
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest campaignTemplate = new SDataTemplateResourceRequest(dynamic);
                campaignTemplate.ResourceKind = "campaigns";

                Sage.SData.Client.Atom.AtomEntry tempEntry = campaignTemplate.Read();
                SDataPayload payload = tempEntry.GetSDataPayload();

                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"];

                payload.Values["CreateDate"] = DateTime.Now;
                payload.Values["CreateUser"] = userPayload.Values["userId"];
                payload.Values["Description"] = randomCampaignDescriptionGenerator();
                payload.Values["CampaignName"] = payload.Values["Description"];
                payload.Values["Objectives"] = randomCampaignObjective();
                payload.Values["CampaignLeadSources"] = fetchLeadSource();
                payload.Values["CallToAction"] = getCallToAction(payload.Values["Objectives"].ToString());
                payload.Values["ForecastBudget"] = rand.Next(1000, 10000);
                payload.Values["ExpensesBudget"] = (int)payload.Values["ForecastBudget"] * rand.NextDouble();
                payload.Values["Status"] = "Active";
                // Use CampaignTargets to calculate the amount of ExpectedContactResponses and ExpectedLeadResponses
                //payload.Values["CampaignProducts"] = fetchCampaignProduct();
                //payload.Values["CampaignResponses"] = fetchCampaignResponse();
                //payload.Values["CampaignTargets"] = fetchCampaignTarget();
                //payload.Values["Opportunities"] = fetchOpportunityCampaign(payload.Key);

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "campaigns",
                    Entry = tempEntry
                };
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | Campaign made with name: " + payload.Values["CampaignName"] + " | " + timed + " seconds", fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        public void updateCampaign()
        {
            try
            {
                
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest campaignProductsTemplate = new SDataTemplateResourceRequest(dynamic);
                campaignProductsTemplate.ResourceKind = "campaignProducts";
                Sage.SData.Client.Atom.AtomEntry tempEntry = campaignProductsTemplate.Read();
                SDataPayload payload = tempEntry.GetSDataPayload();

                var product = fetchCampaignProduct();

                payload.Values["Product"] = product;
                payload.Values["Name"] = product.Values["Description"];
                payload.Values["Family"] = product.Values["Family"];

                SDataResourceCollectionRequest campaign = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "campaigns"
                };

                SDataPayload campaignPayload = new SDataPayload();
                var feed = campaign.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    campaignPayload = feed.Entries.ElementAt(i).GetSDataPayload();
                }

                payload.Values["Campaign"] = campaignPayload;

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "campaignProducts",
                    Entry = tempEntry
                };
                var response = request.Create();

                //campaignPayload.Values["CampaignProducts"] = payload;

                SDataTemplateResourceRequest campaignResponsesTemplate = new SDataTemplateResourceRequest(dynamic);
                campaignProductsTemplate.ResourceKind = "targetResponses";
                Sage.SData.Client.Atom.AtomEntry tempEntry2 = campaignProductsTemplate.Read();
                SDataPayload payload2 = tempEntry.GetSDataPayload();


                payload2.Values["Campaign"] = campaignPayload;
                payload2.Values["CampaignTarget"] = product.Values["Description"];
                payload2.Values["Family"] = product.Values["Family"];


                tempEntry2.SetSDataPayload(payload);

                SDataSingleResourceRequest request2 = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "campaignProduct",
                    Entry = tempEntry
                };
                var response2 = request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | Updated Campaign: " + campaignPayload.Values["CampaignName"] + " | " + timed + " seconds", fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
        }

        public SDataPayload updateAccount(SDataPayload accountPayload)
        {
            
            Guid guid = Guid.NewGuid();
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataPayload address = null;

                SDataTemplateResourceRequest account = new SDataTemplateResourceRequest(dynamic)
                {
                    ResourceKind = "accounts"
                };
                var tempEntry = account.Read();

                SDataResourceCollectionRequest addresses = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "addresses",
                    QueryValues = { { "where", "Country eq 'USA'" } }
                };
                float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var feed = addresses.Read();
                float tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Addresses |  | " + tempTime, fileName);
                int count = feed.Entries.Count();
                int choice = rand.Next(count);
                address = feed.Entries.ElementAt(choice).GetSDataPayload();
                accountPayload.Values["Address"] = address;
                string[] region = getRegion((string)address.Values["State"]);
                accountPayload.Values["Region"] = region[0];
                accountPayload.Values["CreateSource"] = "Demo-Bot";
                string accountMan = region[1];

                SDataResourceCollectionRequest users = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "users",
                    QueryValues = { { "where", "UserName eq '" + accountMan.ToLower() + "'" } }
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var feed5 = users.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Users |  | " + tempTime, fileName);
                SDataPayload accountManager = feed5.Entries.ElementAt(0).GetSDataPayload();

                accountPayload.Values["AccountManager"] = accountManager;

                SDataPayload owner = null;

                SDataResourceCollectionRequest owners = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "owners",
                    QueryValues = { { "where", "OwnerDescription eq '" + region[0] + "'" } }
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var feed3 = owners.Read();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Get | Owners |  | " + tempTime, fileName);
                owner = feed3.Entries.ElementAt(0).GetSDataPayload();

                accountPayload.Values["Owner"] = owner;

                tempEntry.SetSDataPayload(accountPayload);


                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    Entry = tempEntry
                };
                tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                var response = request.Update();
                accountPayload = (SDataPayload)response.GetSDataPayload();
                tempAfter = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                tempTime = (tempAfter - tempPre) / 1000;
                Log(DateTime.Now + " | " + guid + " | Put | Account |  | " + tempTime, fileName);
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " | " + guid + " | Total | Account | " + accountPayload.Values["AccountName"] + " | " + timed, fileName);
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
            }
            return accountPayload;
        }

        #endregion

        #region SDataDecoding
        // Unnecessary because there are predefined categories, though this can help to put the types into pretty names.
        protected string categoryDecoder(string type)
        {
            string category = type;
            switch (type)
            {
                case "Appointment":
                    category = "Meeting";
                    break;
                case "ToDo":
                    category = "To Do";
                    break;
                case "PhoneCall":
                    category = "Phone Call";
                    break;
                case "Personal":
                    category = "Personal Activity";
                    break;
                case "Doc":
                    category = "Document";
                    break;
                case "Internal":
                    category = "Event";
                    break;
            }
            return category;
        }

        // Necessary to determine the correct type of the To-Do (since there are multiple types)
        protected string todoDecoder(string description)
        {
            string type = "ToDo";
            //if (description == "Send e-mail message")
            // type = "EMail";
            //if (description == "Send fax")
            //type = "Fax";
            // if (description == "Send letter")
            //type = "Letter";
            //if (description == "Send literature")
            //type = "Literature";

            return type;
        }

        #endregion

        #region RandomDataGenerators
        protected SDataPayload fetchAddress()
        {
            
            SDataPayload address;
            SDataResourceCollectionRequest addresses = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "addresses",
                QueryValues = { { "where", "Country eq 'USA'" } }
            };
            var feed = addresses.Read();
            int count = feed.Entries.Count();
            int choice = rand.Next(count);
            address = feed.Entries.ElementAt(choice).GetSDataPayload();
            return address;
        }

        // So far does nothing, would be nice to have an array of locations to pull from...
        protected SDataPayload fetchLeadAddress()
        {
            SDataPayload address;
            address = new SDataPayload
            {
                ResourceName = "leadAddresses",
                Values = {
                                            {"Description", "Office"},
                                            {"CreateDate", DateTime.Now},
                                            {"CreateUser", UserID},
                                            {"IsMailing", true},
                                            {"IsPrimary", true},
                                            {"AddressType", "Billing &amp; Shipping"}
                                        }
            };
            return address;
        }

        protected SDataPayload fetchAccount()
        {
            SDataPayload payload = null;

            
            try
            {
                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"];

                SDataResourceCollectionRequest accounts = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    QueryValues = { { "where", "AccountManager.Id eq '" + userPayload.Values["userId"] + "'" } }
                };

                var feed = accounts.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    payload = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }

            return payload;
        }

        protected Sage.SData.Client.Atom.AtomEntry fetchLead()
        {
            Sage.SData.Client.Atom.AtomEntry tempEntry = null;

            
            try
            {
                SDataResourceCollectionRequest leads = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "leads"
                };

                var feed = leads.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    tempEntry = feed.Entries.ElementAt(i);
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }

            return tempEntry;
        }

        protected SDataPayload fetchLeadSource()
        {
            SDataPayload payload = null;

            
            try
            {
                SDataResourceCollectionRequest leadSources = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "leadSources"
                };

                var feed = leadSources.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    payload = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }

            return payload;
        }

        protected Sage.SData.Client.Atom.AtomEntry fetchOpportunity()
        {
            Sage.SData.Client.Atom.AtomEntry returnEntry = null;

            
            try
            {
                 float tempPre = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // From the Whitepaper pdf to get the user payload
                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"];;

                SDataResourceCollectionRequest opportunities = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "opportunities",
                    QueryValues = { { "where", "AccountManager.Id eq '" + userPayload.Values["userId"] + "'" } }
                };

                var feed = opportunities.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(count);
                    returnEntry = feed.Entries.ElementAt(i);
                }
                return returnEntry;
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }

            return returnEntry;
        }

        protected SDataPayload fetchTicket()
        {
            
            SDataPayload ticketPayload = null;
            try
            {
                SDataResourceCollectionRequest tick = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "tickets"
                };

                var feed = tick.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    // Get the ticket payload at the random element at i
                    ticketPayload = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }
            return ticketPayload;
        }

        protected SDataPayload fetchTicketActivityRate()
        {
            SDataPayload rate = null;

            
            try
            {
                SDataResourceCollectionRequest ticketActivityRate = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "ticketActivityRates"
                };

                var feed = ticketActivityRate.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    rate = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
            }
            return rate;
        }

        protected SDataPayload fetchTicketActivityItem()
        {
            SDataPayload item = null;

            
            try
            {
                SDataResourceCollectionRequest ticketActivityItem = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "ticketActivityItems"
                };

                var feed = ticketActivityItem.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    item = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
            }
            return item;
        }

        protected SDataPayload fetchCampaign()
        {
            SDataPayload item = null;

            
            try
            {
                SDataResourceCollectionRequest campaigns = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "campaigns"
                };

                var feed = campaigns.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    item = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
            }
            return item;
        }

        protected SDataPayload fetchCampaignProduct()
        {
            SDataPayload item = null;

            
            try
            {
                SDataResourceCollectionRequest campaignProducts = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "products"
                };

                var feed = campaignProducts.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    item = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
            }
            return item;
        }

        protected SDataPayload fetchCampaignResponse()
        {
            SDataPayload item = null;

            
            try
            {
                SDataResourceCollectionRequest targetResponses = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "targetResponses"
                };

                var feed = targetResponses.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    item = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
            }
            return item;
        }

        protected SDataPayload fetchCampaignTarget()
        {
            SDataPayload item = null;

            
            try
            {
                SDataResourceCollectionRequest campaignTargets = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "campaignTargets"
                };

                var feed = campaignTargets.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    item = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
            }
            return item;
        }

        protected SDataPayload fetchOpportunityCampaign(SDataPayload campaignPayload)
        {
            SDataPayload item = null;

            
            try
            {
                SDataResourceCollectionRequest opportunityCampaigns = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "opportunityCampaigns",
                    QueryValues = { { "where", "Campaign.Id eq '" + campaignPayload.Key + "'" } }
                };

                var feed = opportunityCampaigns.Read();
                int count = feed.Entries.Count();
                if (count != 0)
                {
                    int i = rand.Next(0, count);
                    item = feed.Entries.ElementAt(i).GetSDataPayload();
                }
            }
            catch (Exception e)
            {
                Log(DateTime.Now + " |  |  | Error | " + e.Message, fileName);
                //SetText("Connection to server lost... Please check your connection");
            }
            return item;
        }

        protected string getSubType()
        {
            string type = "";
            String[] types = { "Hardware", "Type A", "Type B", "Software", "Network", "Type C", "Services" };
            int choice = rand.Next(7);
            type = types[choice];
            return type;
        }

        protected string getCallToAction(string objective)
        {
            
            string call = null;
            switch (objective)
            {
                case "Increase sales to new and existing customers":
                    int discount = rand.Next(50, 500);
                    call = "$" + discount + " off select models";
                    break;
                case "Rase awareness of the product":
                    int choice = rand.Next(1, 4);
                    switch (choice)
                    {
                        case 1:
                            call = "Increase TV advertisement presence";
                            break;
                        case 2:
                            call = "Increase mailing presence";
                            break;
                        case 3:
                            call = "Increase web advertisement";
                            break;
                        case 4:
                            call = "Increase e-mail marketing";
                            break;
                    }
                    break;
                case "Increase credibility":
                    int choice2 = rand.Next(1, 3);
                    switch (choice2)
                    {
                        case 1:
                            call = "Get real customer stories";
                            break;
                        case 2:
                            call = "Quote customer service successes";
                            break;
                        case 3:
                            call = "Highlight pros of product over competition";
                            break;
                    }
                    break;
                case "Compete in the market":
                    int discount2 = rand.Next(10, 25);
                    int length = rand.Next(1, 15);
                    call = "Provide product discount of $" + discount2 + " for " + length + " days";
                    break;
                case "Increase revenue":
                    int choice3 = rand.Next(1, 2);
                    switch (choice3)
                    {
                        case 1:
                            call = "Provide limited time cloud free support for new signups";
                            break;
                        case 2:
                            int discount3 = rand.Next(10, 25);
                            int length2 = rand.Next(1, 15);
                            call = "Provide product discount of $" + discount3 + " for " + length2 + " days";
                            break;
                    }
                    break;
                case "Become a recognised member of society":
                    int choice4 = rand.Next(1, 2);
                    switch (choice4)
                    {
                        case 1:
                            int amount = rand.Next(1000, 10000);
                            call = "Donate " + amount + " to a local charity";
                            break;
                        case 2:
                            call = "Setup a charity day for workers to volunteer locally";
                            break;
                    }
                    break;
            }
            return call;
        }

        protected void progressStage(SDataPayload oppPayload)
        {
            string currentStage = (string)oppPayload.Values["Stage"];
            switch (currentStage)
            {
                case null:
                    oppPayload.Values["Stage"] = "1-Prospect";
                    oppPayload.Values["CloseProbability"] = 1;
                    oppPayload.Values["LeadSource"] = fetchLeadSource();
                    break;
                case "1-Prospect":
                    oppPayload.Values["Stage"] = "2-Qualification";
                    oppPayload.Values["CloseProbability"] = 10;
                    break;
                case "2-Qualification":
                    oppPayload.Values["Stage"] = "3-Needs Analysis";
                    oppPayload.Values["CloseProbability"] = 25;
                    break;
                case "3-Needs Analysis":
                    oppPayload.Values["Stage"] = "4-Demonstration";
                    oppPayload.Values["CloseProbability"] = 50;
                    break;
                case "4-Demonstration":
                    oppPayload.Values["Stage"] = "5-Negotiation";
                    oppPayload.Values["CloseProbability"] = 75;
                    break;
                case "5-Negotiation":
                    oppPayload.Values["Stage"] = "6-Decision";
                    oppPayload.Values["CloseProbability"] = 100;
                    break;
                case "6-Decision":
                    oppPayload.Values["Stage"] = "6-Decision";
                    oppPayload.Values["CloseProbability"] = 100;
                    break;
            }
        }

        protected void progressChineseStage(SDataPayload oppPayload)
        {
            string currentStage = (string)oppPayload.Values["Stage"];
            switch (currentStage)
            {
                case null:
                    oppPayload.Values["Stage"] = "1-展望";
                    oppPayload.Values["CloseProbability"] = 1;
                    oppPayload.Values["LeadSource"] = fetchLeadSource();
                    break;
                case "1-展望":
                    oppPayload.Values["Stage"] = "2-合格";
                    oppPayload.Values["CloseProbability"] = 10;
                    break;
                case "2-合格":
                    oppPayload.Values["Stage"] = "3-需求分析";
                    oppPayload.Values["CloseProbability"] = 25;
                    break;
                case "3-需求分析":
                    oppPayload.Values["Stage"] = "4-示范";
                    oppPayload.Values["CloseProbability"] = 50;
                    break;
                case "4-示范":
                    oppPayload.Values["Stage"] = "5-谈判";
                    oppPayload.Values["CloseProbability"] = 75;
                    break;
                case "5-谈判":
                    oppPayload.Values["Stage"] = "6-决策";
                    oppPayload.Values["CloseProbability"] = 100;
                    break;
                case "6-决策":
                    oppPayload.Values["Stage"] = "6-决策";
                    oppPayload.Values["CloseProbability"] = 100;
                    break;
            }
        }

        protected string[] getRegion(string state)
        {
            string[] region = new string[2];
            switch (state)
            {
                case "ME":
                case "VT":
                case "NH":
                case "MA":
                case "RI":
                case "NY":
                case "CT":
                case "PA":
                case "NJ":
                case "DE":
                case "MD":
                    region[0] = "Northeast";
                    region[1] = "Dan";
                    break;
                case "WV":
                case "VA":
                case "KY":
                case "NC":
                case "SC":
                case "TN":
                case "AR":
                case "LA":
                case "MS":
                case "AL":
                case "GA":
                case "FL":
                    region[0] = "Southeast";
                    region[1] = "Linda";
                    break;
                case "ND":
                case "SD":
                case "NE":
                case "KS":
                case "MN":
                case "IA":
                case "MO":
                case "WI":
                case "IL":
                case "MI":
                case "IN":
                case "OH":
                    region[0] = "Midwest";
                    region[1] = "Lee";
                    break;
                case "AZ":
                case "NM":
                case "OK":
                case "TX":
                    region[0] = "Southwest";
                    region[1] = "Ed";
                    break;
                case "WA":
                case "OR":
                case "CA":
                case "NV":
                case "ID":
                case "MT":
                case "WY":
                case "UT":
                case "CO":
                    region[0] = "Northwest";
                    region[1] = "Cathy";
                    break;
                default:
                    region[0] = "Midwest";
                    region[1] = "Lee";
                    break;
            }
            return region;
        }

        // Currently excludes Event Activity due to it causing problems on activity creation.
        protected string randomTypeGenerator()
        {
            
            // Random number to determine which type will be created.

            double choice = 5 * rand.NextDouble();
            string returnType;

            if (choice >= 4)
            {
                returnType = "Appointment";
            }
            else if (choice >= 2.5)
            {
                returnType = "PhoneCall";
            }
            else if (choice >= 0.0000015) 
            {
                returnType = "ToDo";
            }
            else if (UserID == "admin")
            {
                returnType = "ToDo";
            }
            else
            {
                returnType = "Personal";
            }

            return returnType;
        }

        protected string leadTypeGenerator()
        {
            
            // Random number to determine which type will be created.

            int choice = rand.Next(1, 2);
            string returnType;

            if (choice == 1)
                returnType = "Appointment";
            else
                returnType = "PhoneCall";

            return returnType;
        }

        protected string randomChineseTypeGenerator()
        {
            
            // Random number to determine which type will be created.

            double choice = 5 * rand.NextDouble();
            string returnType;

            if (choice >= 4)
                returnType = "任命";
            else
            {
                if (choice >= 2.5)
                    returnType = "电话呼叫";
                else
                {
                    if (choice >= 0.0015)
                        returnType = "要不要";
                    //else
                    //{
                    //if (choice >= 1)
                    //returnType = "Internal";
                    else
                    {
                        if (UserID == "admin")
                            returnType = "要不要";
                        else
                            returnType = "个人";
                    }
                    //}
                }
            }

            return returnType;
        }

        protected string randomAccountType()
        {
            
            int choice = rand.Next(0, 8);
            string type = "";
            switch (choice)
            {
                case 0:
                    type = "Suspect";
                    break;
                case 1:
                    type = "Lead";
                    break;
                case 2:
                    type = "Prospect";
                    break;
                case 3:
                    type = "Customer";
                    break;
                case 4:
                    type = "Partner";
                    break;
                case 5:
                    type = "Vendor";
                    break;
                case 6:
                    type = "Influencer";
                    break;
                case 7:
                    type = "Competitor";
                    break;
                case 8:
                    type = "Other";
                    break;
            }
            return type;
        }

        protected string randomChineseAccountType()
        {
            
            int choice = rand.Next(0, 8);
            string type = "";
            switch (choice)
            {
                case 0:
                    type = "怀疑";
                    break;
                case 1:
                    type = "带领";
                    break;
                case 2:
                    type = "前景";
                    break;
                case 3:
                    type = "顾客";
                    break;
                case 4:
                    type = "伙伴";
                    break;
                case 5:
                    type = "供应商";
                    break;
                case 6:
                    type = "影响";
                    break;
                case 7:
                    type = "竞争者";
                    break;
                case 8:
                    type = "其他";
                    break;
            }
            return type;
        }

        protected string randomAccountStatus()
        {
            
            int choice = rand.Next(0, 4);
            string status = "";
            switch (choice)
            {
                case 0:
                    status = "Active";
                    break;
                case 1:
                    status = "Deleted";
                    break;
                case 2:
                    status = "Duplicated";
                    break;
                case 3:
                    status = "Inactive";
                    break;
                case 4:
                    status = "Purge";
                    break;
            }
            return status;
        }

        protected string randomChineseAccountStatus()
        {
            
            int choice = rand.Next(0, 4);
            string status = "";
            switch (choice)
            {
                case 0:
                    status = "活跃";
                    break;
                case 1:
                    status = "删除";
                    break;
                case 2:
                    status = "重复";
                    break;
                case 3:
                    status = "暂无";
                    break;
                case 4:
                    status = "清洗";
                    break;
            }
            return status;
        }

        protected string randomCategoryGenerator(string type)
        {
            
            string category = "";
            int index;

            string[] categories = new string[]
            {
                #region Categories
                // To-Do categories: (16 total)
                "",
                "Billable Time",
                "Conference",
                "Demo",
                "Fax Response",
                "Goodwill Visit",
                "Info Requested",
                "Lead Processing",
                "Non-Billable Time",
                "Onsite Service",
                "Other Visit",
                "Personal",
                "Sales Visit",
                "Trade Show",
                "Training",
                "Web Response",
                // Meeting categories and Personal: (14 total)
                "",
                "Goodwill Visit",
                "Installation",
                "Non-Billable Time",
                "Onsite Service",
                "Other Visit",
                "Personal",
                "Project Implementation",
                "Sales Visit",
                "Strategic Relationship",
                "Trade Show",
                "Training",
                "Travel",
                "Venture Capitol",
                // Phone call categories: (5 total)
                "",
                "Cold Call",
                "Follow-up",
                "Leads",
                "Personal",
                #endregion
            };
            switch (type)
            {
                case "ToDo":
                    index = rand.Next(15);
                    category = categories[index];
                    break;
                case "Appointment":
                    index = rand.Next(16, 29);
                    category = categories[index];
                    break;
                case "Personal":
                    goto case "Appointment";
                case "PhoneCall":
                    index = rand.Next(30, 34);
                    category = categories[index];
                    break;
                default:
                    category = "";
                    break;
            }
            return category;
        }

        protected string leadCategoryGenerator(string type)
        {
            
            string category = "";
            int index;

            string[] categories = new string[]
            {
                #region Categories
                // Meeting categories and Personal: (14 total)
                "",
                "Goodwill Visit",
                "Other Visit",
                "Sales Visit",
                "Strategic Relationship",
                "Trade Show",
                "Travel",
                "Venture Capitol",
                // Phone call categories: (5 total)
                "",
                "Cold Call",
                "Follow-up",
                "Leads",
                #endregion
            };
            switch (type)
            {
                case "Appointment":
                    index = rand.Next(0, 7);
                    category = categories[index];
                    break;
                case "Follow-Up":
                    category = categories[8];
                    break;
                case "ColdCall":
                    category = categories[9];
                    break;
                case "LeadCall":
                    category = categories[10];
                    break;
                default:
                    category = "";
                    break;
            }
            return category;
        }

        protected string randomChineseCategoryGenerator(string type)
        {
            
            string category = "";
            int index;

            string[] categories = new string[]
            {
                #region Categories
                // To-Do categories: (16 total)
                "",
                "计费时间",
                "会议",
                "演示",
                "传真响应",
                "友好访问",
                "所请求的信息",
                "铅处理",
                "非计费时间",
                "上门服务",
                "其他访问",
                "个人",
                "销售访问",
                "展会",
                "训练",
                "互联网响应",
                // Meeting categories and Personal: (14 total)
                "",
                "友好访问",
                "安装",
                "非计费时间",
                "上门服务",
                "其他访问",
                "个人",
                "项目实施",
                "销售访问",
                "战略合作伙伴关系",
                "展会",
                "训练",
                "旅行",
                "创业国会",
                // Phone call categories: (5 total)
                "",
                "冷呼叫",
                "后续",
                "信息",
                "个人",
                #endregion
            };
            switch (type)
            {
                case "要不要":
                    index = rand.Next(15);
                    category = categories[index];
                    break;
                case "任命":
                    index = rand.Next(16, 29);
                    category = categories[index];
                    break;
                case "个人":
                    goto case "任命";
                case "电话呼叫":
                    index = rand.Next(30, 34);
                    category = categories[index];
                    break;
                default:
                    category = "";
                    break;
            }
            return category;
        }

        protected string randomDescriptionGenerator(string type)
        {
            
            // For the random number generation

            int index;
            string returnDescription = "";
            string[] descriptions = new string[]
            {
                #region Descriptions
                // Listed are the predefined 'regarding' values on the mobile client.
                // For Meeting: (10 total)
                "Breakfast meeting",
                "Demonstration",
                "Dinner meeting",
                "Discuss Opportunities",
                "Lunch Meeting",
                "Presentation",
                "Review proposal",
                "Review requirements",
                "Review ticket",
                "Training",
                // For Phone Call: (11 total)
                "Confirm Literature Received",
                "Confirm Meeting",
                "Discuss Opportunities",
                "Follow up - next step",
                "Follow up on proposal",
                "Follow up on Ticket",
                "Follow-up",
                "Qualify for needs",
                "Request Payment for invoice",
                "Return voice mail message",
                "Schedule a Meeting",
                // For To-Do: (6 total)
                "Send e-mail message",
                "Send e-mail message",
                // Outdated: "Send fax",
                "Send letter",
                "Send literature",
                "Send proposal",
                "Send quote",
                // None for Event
                // For Personal Activity: (18 total)
                "Anniversary Reminder",
                "Birthday Reminder",
                "Breakfast",
                "Buy Gift",
                "Dentist Appointment",
                "Dinner",
                "Doctor Appointment",
                "Get car fixed",
                "Interview",
                "Lunch",
                "Make travel Arrangements",
                "Pay Bills",
                "Pick up groceries",
                "Send Flowers",
                "Send thank you note",
                "Staff Meeting",
                "Submit expenses",
                "Update forecast",
                // For Notes: (6 total)
                "Meeting notes",
                "Phone meeting",
                "Proposal",
                "Qualification",
                "Questions",
                "Technical notes"
            #endregion
            };


            switch (type)
            {
                case "Appointment":
                    index = rand.Next(9);
                    returnDescription = descriptions[index];
                    break;
                case "PhoneCall":
                    index = rand.Next(10, 20);
                    returnDescription = descriptions[index];
                    break;
                case "ToDo":
                    index = rand.Next(21, 26);
                    returnDescription = descriptions[index];
                    break;
                case "Schedule":
                    returnDescription = "";
                    break;
                case "Personal":
                    index = rand.Next(27, 44);
                    returnDescription = descriptions[index];
                    break;
                case "Note":
                    index = rand.Next(45, 50);
                    returnDescription = descriptions[index];
                    break;
                default:
                    returnDescription = "";
                    break;
            }

            return returnDescription;
        }

        protected string leadDescriptionGenerator(string type)
        {
            
            // For the random number generation

            int index;
            string returnDescription = "";
            string[] descriptions = new string[]
            {
                #region Descriptions
                // Listed are the predefined 'regarding' values on the mobile client.
                // For Meeting: (8 total)
                "Breakfast meeting",
                "Demonstration",
                "Dinner meeting",
                "Discuss Opportunities",
                "Lunch Meeting",
                "Presentation",
                "Review proposal",
                "Review requirements",
                // For Phone Call: (9 total)
                "Confirm Literature Received",
                "Confirm Meeting",
                "Discuss Opportunities",
                "Return voice mail message",
                "Schedule a Meeting",
                "Follow up - next step",
                "Follow up on proposal",
                "Follow-up",
                // For Notes: (5 total)
                "Meeting notes",
                "Phone meeting",
                "Proposal",
                "Qualification",
                "Questions"
            #endregion
            };


            switch (type)
            {
                case "Appointment":
                    index = rand.Next(8);
                    returnDescription = descriptions[index];
                    break;
                case "PhoneCall":
                    index = rand.Next(8, 12);
                    returnDescription = descriptions[index];
                    break;
                case "Follow-Up":
                    index = rand.Next(13, 15);
                    returnDescription = descriptions[index];
                    break;
                case "Note":
                    index = rand.Next(16, 21);
                    returnDescription = descriptions[index];
                    break;
                default:
                    returnDescription = "";
                    break;
            }

            return returnDescription;
        }

        protected string randomCampaignDescriptionGenerator()
        {
            
            string value = null;
            string[] description = new string[]
            {
                "E-mail marketing",
                "Television advertisements",
                "Website advertisements",
                "Fundraising for Charity",
                "Reeling in new prospects"
            };
            int i = rand.Next(0, description.Length - 1);
            value = description[i];
            return value;
        }

        protected string randomCampaignObjective()
        {
            
            string value = null;
            string[] objective = new string[]
            {
                "Increase sales to new and existing customers",
                "Raise awareness of the product",
                "Increase credibility",
                "Compete in the market",
                "Increase revenue",
                "Become a recognised member of society"
            };
            int i = rand.Next(0, objective.Length - 1);
            value = objective[i];
            return value;
        }

        protected string randomChineseDescriptionGenerator(string type)
        {
            
            // For the random number generation

            int index;
            string returnDescription = "";
            string[] descriptions = new string[]
            {
                #region Descriptions
                // Listed are the predefined 'regarding' values on the mobile client.
                // For Meeting: (10 total)
                "早餐会",
                "示范",
                "晚餐会议",
                "讨论的机会",
                "午餐会上",
                "演示",
                "审查的建议",
                "审查要求",
                "回顾票",
                "培训",
                // For Phone Call: (11 total)
                "确认收到的文学",
                "确认会议",
                "讨论的机会",
                "随访 - 下一步",
                "跟进提案",
                "跟进票",
                "后续",
                "资格需要",
                "请求付款发票",
                "返回语音邮件",
                "安排会议",
                // For To-Do: (6 total)
                "发送电子邮件消息",
                "发送传真",
                "发信",
                "发送文学",
                "发送建议书",
                "发送报价",
                // None for Event
                // For Personal Activity: (18 total)
                "周年纪念提醒",
                "生日提醒",
                "早餐",
                "买赠",
                "牙医",
                "晚餐",
                "看病",
                "车固定",
                "面试",
                "午餐",
                "安排旅游行程",
                "收费票据",
                "拿起杂货",
                "鲜花",
                "感谢信",
                "员工大会",
                "提交费用",
                "更新预测",
                // For Notes: (6 total)
                "会议记录",
                "电话会议",
                "建议",
                "资格",
                "问题",
                "技术说明"
            #endregion
            };


            switch (type)
            {
                case "任命":
                    index = rand.Next(9);
                    returnDescription = descriptions[index];
                    break;
                case "电话呼叫":
                    index = rand.Next(10, 20);
                    returnDescription = descriptions[index];
                    break;
                case "要不要":
                    index = rand.Next(21, 26);
                    returnDescription = descriptions[index];
                    break;
                case "时间表":
                    returnDescription = "";
                    break;
                case "个人":
                    index = rand.Next(27, 44);
                    returnDescription = descriptions[index];
                    break;
                case "注意":
                    index = rand.Next(45, 50);
                    returnDescription = descriptions[index];
                    break;
                default:
                    returnDescription = "";
                    break;
            }

            return returnDescription;
        }

        protected string randomNoteGenerator(string type, string accountName, string description)
        {
            
            string note = "";
            // Array of note structures to be used
            string[] notes = new string[]
            {
                #region Notes
                "",
                // Meeting notes: (5 total)
                "Representatives from " + accountName + " wanted to get together to talk about the product.",
                "Meeting with " + accountName + " should help move along business with them.",
                "Talked with " + accountName + " on the phone and they wanted to speak in person for clarifications.",
                accountName + " wanted to congratulate us in person on our product.",
                "Checking up on " + accountName + ". Have not heard from them in a while.",
                // Phone call notes: (3 total)
                accountName + " wanted clarifications",
                "Following up on " + accountName + " because they seemed interested in the product",
                accountName + " wanted to talk about mobile implementation",
                // Personal notes: (4 total)
                "So excited for " + description,
                "Can't wait to " + description,
                "Looking forward to " + description,
                "Definitely need " + description,
                // Notes/History notes: (12 total)
                "Need answers before being able to continue",
                "Wanted to clarify a couple things before moving on",
                "A couple hazy topics that should be discussed",
                "Desire to weed out any uncertainties about the proposal",
                "Went well",
                "Seemed to enjoy the prospect of working with us",
                "Thought the product was more than adequate for the desired tasks",
                "Talked about the product and got lots of constructive feedback",
                "The chat was brief, but overall seemed optimistic",
                "A couple technical things to note...",
                "Just wanted to list a couple technical things I found...",
                "Tech notes..."
                #endregion
            };

            int index = 0;


            switch (type)
            {
                case "Appointment":
                    index = rand.Next(1, 5);
                    note = notes[index];
                    break;
                case "PhoneCall":
                    if (description == "Discuss Opportunities")
                        index = 6;
                    if (description == "Follow-up" || description == "Follow up - next step" || description == "Follow up on proposal")
                    {
                        int temp = rand.Next(0, 2);
                        if (temp == 0)
                            index = 6;
                        if (temp == 1)
                            index = 7;
                        if (temp == 2)
                            index = 8;
                    }
                    if (description == "Discuss Opportunities")
                        index = 8;

                    note = notes[index];
                    break;
                case "Personal":
                    if (description == "Pay Bills" || description == "Submit expenses" || description == "Birthday Reminder" || description == "Anniversary Reminder")
                        note = "";
                    else
                    {
                        index = rand.Next(9, 12);
                        note = notes[index];
                    }
                    break;
                case "Note":
                    if (description == "Meeting notes" || description == "Phone meeting")
                    {
                        int temp = rand.Next(7);
                        if (temp == 0)
                            index = 14;
                        if (temp == 1)
                            index = 15;
                        if (temp == 2)
                            index = 16;
                        if (temp == 3)
                            index = 17;
                        if (temp == 4)
                            index = 18;
                        if (temp == 5)
                            index = 19;
                        if (temp == 6)
                            index = 20;
                    }
                    if (description == "Proposal" || description == "Qualification")
                    {
                        int temp = rand.Next(0, 2);
                        if (temp == 0)
                            index = 16;
                        if (temp == 1)
                            index = 17;
                        if (temp == 2)
                            index = 18;
                    }
                    if (description == "Questions")
                    {
                        int temp = rand.Next(0, 3);
                        if (temp == 0)
                            index = 12;
                        if (temp == 1)
                            index = 13;
                        if (temp == 2)
                            index = 14;
                        if (temp == 3)
                            index = 15;
                    }
                    if (description == "Technical notes")
                    {
                        int temp = rand.Next(0, 2);
                        if (temp == 0)
                            index = 21;
                        if (temp == 1)
                            index = 22;
                        if (temp == 2)
                            index = 23;
                    }
                    note = notes[index];
                    break;
                default:
                    note = "";
                    break;
            }

            return note;
        }

        protected string randomNoteforLeadGenerator(string value, SDataPayload payload, string type)
        {
            
            string note = "";
            string company = "";
            string name = "";
            if (string.Compare(value, "Lead") == 0)
            {
                company = (string)payload.Values["Company"];
                name = (string)payload.Values["FirstName"] + " " + (string)payload.Values["LastName"];
            }
            if (string.Compare(value, "Contact") == 0)
            {
                company = (string)payload.Values["AccountName"];
                name = (string)payload.Values["Name"];
            }
            if (string.Compare(value, "Activity") == 0)
            {
                company = (string)payload.Values["AccountName"];
                name = (string)payload.Values["LeadName"];
            }

            // Array of note structures to be used
            string[] notes = new string[17]
            {
                #region Notes
                "",
                // Meeting notes: (5 total)
                "Representatives from " + company + " wanted to get together to talk about the product.",
                "Meeting with " + name + " should help move along business with them.",
                "Talked with " + name + " on the phone and they wanted to speak in person for clarifications.",
                name + " wanted to congratulate us in person on our product.",
                "Went well",
                "Seemed to enjoy the prospect of working with us",
                "Thought the product was more than adequate for the desired tasks",
                "Talked about the product and got lots of constructive feedback",
                "The chat was brief, but overall seemed optimistic",
                // Phone call notes: (3 total)
                name + " wanted clarifications",
                name + " wanted to talk about mobile implementation",
                "Following up on " + name + " because they seemed interested in the product",
                // No answer / New activity: ( total)
                "Didn't answer, set up new activity to check in again",
                "Couldn't talk at that time, will call again later",
                "Wanted answers to questions, made a new activity to account for this",
                "Lead wanted to meet in person to talk about the product, made an appointment"
                #endregion
            };

            int choice;
            switch (type)
            {
                case "Appointment":
                    choice = rand.Next(10);
                    note = notes[choice];
                    break;
                case "PhoneCall":
                    choice = rand.Next(5, 13);
                    note = notes[choice];
                    break;
                case "Follow-Up":
                    note = notes[13];
                    break;
                case "NewActivity":
                    choice = rand.Next(13, 17);
                    note = notes[choice];
                    break;
                default:
                    choice = rand.Next(8, 13);
                    note = notes[choice];
                    break;
            }


            return note;
        }

        protected string randomChineseNoteGenerator(string type, SDataPayload payload, string description)
        {
            
            string note = "";
            // Array of note structures to be used
            string[] notes = new string[]
            {
                #region Notes
                "",
                // Meeting notes: (5 total)
                payload.Values["AccountName"] + "的代表通缉令扎堆谈论一下产品",
                "会议" + payload.Values["AccountName"] + " 应该帮助沿着与他们做生意",
                "跟" + payload.Values["AccountName"] + " 在手机上他们想说话亲自澄清",
                payload.Values["AccountName"] + " 想祝贺我们的人对我们的产品",
                "检查" + payload.Values["AccountName"] + " 有没有听说过他们在一段时间",
                // Phone call notes: (3 total)
                payload.Values["AccountName"] + " 想澄清",
                "继 " + payload.Values["AccountName"] + " 因为他们似乎对产品感兴趣的",
                payload.Values["AccountName"] + " 想谈谈移动实施",
                // Personal notes: (4 total)
                "如此兴奋" + description,
                "不能等待" + description,
                "期待" + description,
                "绝对需要" + description,
                // Notes/History notes: (12 total)
                "如果你需要能够继续之前",
                "在移动之前有两件事情想澄清",
                "一对夫妇朦胧应该讨论的主题",
                "欲望淘汰任何的建议的不确定性",
                "进展顺利",
                "似乎享受与我们合作的前景",
                "思想的产品所需的任务绰绰有余",
                "谈到该产品并得到了很多建设性的反馈意见",
                "聊天是短暂的但总体而言似乎是乐观的",
                "一对夫妇技术方面的东西要注意......",
                "只是想列出两个技术方面的东西我发现......",
                "科技笔记......"
                #endregion
            };

            int index = 0;


            switch (type)
            {
                case "任命":
                    index = rand.Next(1, 5);
                    note = notes[index];
                    break;
                case "电话呼叫":
                    if (description == "讨论机会")
                        index = 6;
                    if (description == "后续" || description == "随访 - 下一步" || description == "跟进建议")
                    {
                        int temp = rand.Next(0, 2);
                        if (temp == 0)
                            index = 6;
                        if (temp == 1)
                            index = 7;
                        if (temp == 2)
                            index = 8;
                    }
                    if (description == "讨论机会")
                        index = 8;

                    note = notes[index];
                    break;
                case "个人":
                    if (description == "支付帐单" || description == "提交费用" || description == "生日提醒" || description == "周年纪念提醒")
                        note = "";
                    else
                    {
                        index = rand.Next(9, 12);
                        note = notes[index];
                    }
                    break;
                case "注意":
                    if (description == "会议记录" || description == "电话会议")
                    {
                        int temp = rand.Next(7);
                        if (temp == 0)
                            index = 14;
                        if (temp == 1)
                            index = 15;
                        if (temp == 2)
                            index = 16;
                        if (temp == 3)
                            index = 17;
                        if (temp == 4)
                            index = 18;
                        if (temp == 5)
                            index = 19;
                        if (temp == 6)
                            index = 20;
                    }
                    if (description == "建议" || description == "合格")
                    {
                        int temp = rand.Next(0, 2);
                        if (temp == 0)
                            index = 16;
                        if (temp == 1)
                            index = 17;
                        if (temp == 2)
                            index = 18;
                    }
                    if (description == "问题")
                    {
                        int temp = rand.Next(0, 3);
                        if (temp == 0)
                            index = 12;
                        if (temp == 1)
                            index = 13;
                        if (temp == 2)
                            index = 14;
                        if (temp == 3)
                            index = 15;
                    }
                    if (description == "技术说明")
                    {
                        int temp = rand.Next(0, 2);
                        if (temp == 0)
                            index = 21;
                        if (temp == 1)
                            index = 22;
                        if (temp == 2)
                            index = 23;
                    }
                    note = notes[index];
                    break;
                default:
                    note = "";
                    break;
            }

            return note;
        }

        protected string randomLocationGenerator(string type)
        {
            
            string location = "";

            if (type == "Appointment" || type == "Personal")
            {
                // Array of locations such as meeting places and restaraunts: (22 total)
                string[] locations = new string[]
            {
                #region Locations
                "Starbucks",
                "Wally's",
                "Park",
                "Dutch Bro's Coffee",
                "Wildflower Bread Co.",
                "America's Taco Shop",
                "The office",
                "Paradise Bakery",
                "Islands Fine Burgers and Drinks",
                "Headquarters",
                "Rubio's",
                "P.F. Changs",
                "Panda Express",
                "Pei Wei",
                "Einstein Bro's Bagles",
                "El Encanto",
                "Bryan's Barbeque",
                "Chiles",
                "Four Peaks",
                "Two Peaks",
                "Five Guys Burgers",
                "In N' Out Burger"
                #endregion
            };
                // Array of streets: (26 total)
                string[] streets = new string[]
            {
                #region Streets
                "Scottsdale Rd.",
                "Loop 101",
                "Dynamite Rd.",
                "Tatum Rd.",
                "56th Street",
                "Bell Rd.",
                "Washington Street",
                "Cartwheel Drive",
                "Cave Creek Rd.",
                "Carefree Hwy.",
                "Spur Cross Rd.",
                "7th Avenue",
                "Grapevine Dr.",
                "Lone Mountain Dr.",
                "Schoolhouse Rd.",
                "Shea Blvd.",
                "Ashler Hills,",
                "Dixaleta Dr.",
                "Jomax Rd.",
                "Pima Rd.",
                "Mill Ave.",
                "College Ave.",
                "Priest Dr.",
                "Southern Ave.",
                "Broadway Rd.",
                "Lemon Street"
                #endregion
            };


                int locationChoice = rand.Next(21);
                int streetChoice1 = rand.Next(25);
                int streetChoice2;
                do
                {
                    streetChoice2 = rand.Next(25);
                } while (streetChoice1 == streetChoice2);

                location = locations[locationChoice] + " at " + streets[streetChoice1] + " and " + streets[streetChoice2];
            }
            else
            {
                location = "The office";
            }

            return location;
        }

        protected string randomChineseLocationGenerator(string type)
        {
            
            string location = "";

            if (type == "任命" || type == "个人")
            {
                // Array of locations such as meeting places and restaraunts: (22 total)
                string[] locations = new string[]
            {
                #region Locations
                "星巴克",
                "沃利",
                "公园",
                "荷兰弟兄的咖啡",
                "野花面包有限公司",
                "美国的塔可店",
                "办公室",
                "天堂面包",
                "群岛精细的汉堡和饮料",
                "总部",
                "卢比奥",
                "P.F.长沙",
                "熊猫快递",
                "裴炜",
                "爱因斯坦弟兄Bagles的",
                "埃尔恩坎托",
                "布莱恩的烧烤",
                "智利",
                "四峰",
                "两峰",
                "五个人汉堡",
                "N'日期汉堡"
                #endregion
            };
                // Array of streets: (26 total)
                string[] streets = new string[]
            {
                #region Streets
                "斯科茨代尔路。",
                "环路",
                "的炸药路。",
                "塔图姆的路。",
                "街",
                "贝尔路。",
                "华盛顿街",
                "车轮驱动",
                "洞溪路。",
                "逍遥游的高速公路。",
                "支线交叉路。",
                "第七大道",
                "葡萄博士",
                "龙山区博士",
                "校舍路。",
                "佘大道",
                "山",
                "的博士",
                "Jomax路。",
                "皮马路。",
                "穆勒大道。",
                "学院",
                "牧师博士",
                "南大街",
                "百老汇路。",
                "柠檬街"
                #endregion
            };


                int locationChoice = rand.Next(21);
                int streetChoice1 = rand.Next(25);
                int streetChoice2;
                do
                {
                    streetChoice2 = rand.Next(25);
                } while (streetChoice1 == streetChoice2);

                location = locations[locationChoice] + "在" + streets[streetChoice1] + "和" + streets[streetChoice2];
            }
            else
            {
                location = "办公室";
            }

            return location;
        }

        protected string randomTitle()
        {
            
            string title = "";
            string[] titles = new string[]
            {
                "Assistant",
                "CEO",
                "CFO",
                "Director of Cust Service",
                "Director of Finance",
                "Director of IT",
                "Director of Marketing",
                "Director of Sales",
                "Manager",
                "Managing Director",
                "Owner",
                "President",
                "Principal",
                "VP Finance",
                "VP Marketing",
                "VP Sales",
                "VP of Cust Service",
                "Vice President"
            };
            int i = rand.Next(titles.Length);
            title = titles[i];
            return title;
        }

        protected string randomReason(bool won)
        {
            
            string reason = "";
            string[] reasons = new string[]
            {
                // Reasons won: (16 total)
                "Better product",
                "Superior support",
                "Better relationships",
                "Sales team was effective",
                "Arguably more effective than competitors",
                "Performace of product greater than competitors",
                "Fit the needs of the client",
                "Superior product line",
                "References",
                "Don't know",
                "Superior product features",
                "Better price",
                "Better references",
                "Superior salesmanship",
                "Uknown",
                "Relationship",
                // Reasons lost: (10 total)
                "Don't know",
                "Inferior/Missing product features",
                "Priced too high",
                "No budget or lost budget",
                "Lack of references",
                "Unfair tactics used by competitor",
                "Other",
                "Unknown",
                "Inferior/Missing Products",
                "Relationship"
            };
            if (won)
            {
                int i = rand.Next(0, 15);
                reason = reasons[i];
                if (reason == "Don't know" || reason == "Uknown")
                    return reason;
                else
                {
                    string reason2 = "";
                    int count = 0;
                    int j = rand.Next(0, 15);
                    do
                    {
                        reason2 = reasons[j];
                        count++;
                    } while (reason == reason2 && count < 10);
                    reason = reason + " and " + reason2;
                    return reason;
                }
            }
            else
            {
                int i = rand.Next(16, 26);
                reason = reasons[i];
                if (reason == "Don't know" || reason == "Uknown")
                    return reason;
                else
                {
                    string reason2 = "";
                    int j = rand.Next(16, 26);
                    int count = 0;
                    do
                    {
                        reason2 = reasons[j];
                        count++;
                    } while (reason == reason2 && count < 10);
                    reason = reason + " and " + reason2;
                    return reason;
                }
            }
        }

        protected string randomChineseReason(bool won)
        {
            
            string reason = "";
            string[] reasons = new string[]
            {
                // Reasons won: (16 total)
                "更好的产品",
                "高级支持",
                "更好的关系",
                "销售团队是有效的",
                "可以说比竞争对手更有效",
                "性能指标大于竞争对手的产品",
                "适合客户端的需求",
                "卓越的产品线",
                "参考",
                "不知道",
                "卓越的产品功能",
                "更好的价格",
                "引用",
                "高级推销",
                "尚不清楚",
                "关系",
                // Reasons lost: (10 total)
                "不知道",
                "劣质/失踪的产品功能",
                "定价过高",
                "没有预算或丢失预算",
                "缺乏引用",
                "不正当竞争者所使用的战术",
                "其他",
                "未知",
                "劣质/失踪的产品",
                "关系"
            };
            if (won)
            {
                int i = rand.Next(0, 15);
                reason = reasons[i];
                if (reason == "不知道" || reason == "未知")
                    return reason;
                else
                {
                    string reason2 = "";
                    int j = rand.Next(0, 15);
                    do
                    {
                        reason2 = reasons[j];
                    } while (reason == reason2);
                    reason = reason + "和" + reason2;
                    return reason;
                }
            }
            else
            {
                int i = rand.Next(16, 26);
                reason = reasons[i];
                if (reason == "不知道" || reason == "未知")
                    return reason;
                else
                {
                    string reason2 = "";
                    int j = rand.Next(16, 26);
                    do
                    {
                        reason2 = reasons[j];
                    } while (reason == reason2);
                    reason = reason + "和" + reason2;
                    return reason;
                }
            }
        }

        // Creates a random date for the activity to occur
        protected DateTime randomDateGenerator()
        {
            
            DateTime lowerLim = Convert.ToDateTime("8:00AM");
            DateTime upperLim = Convert.ToDateTime("5:00PM");
            int hour;
            int minute;
            int lowerhour = lowerLim.Hour;
            int upperhour = upperLim.Hour;
            int min = lowerLim.Minute;
            bool check = true;


            DateTime date = DateTime.Now;

            int months = rand.Next(upperBoundMonth);
            double days = 31 * rand.NextDouble();
            double hours = 24 * rand.NextDouble();
            double minutes = 60 * rand.NextDouble();

            date = date.AddMonths(months);
            date = date.AddDays(days);
            do
            {
                check = true;
                date = date.AddHours(hours);
                date = date.AddMinutes(minutes);
                hour = date.Hour;
                minute = date.Minute;

                if (hour == lowerhour || hour == upperhour)
                {
                    if (minute <= min)
                        check = true;
                    else
                        check = false;
                }
            } while (hour < lowerhour || hour > upperhour || !check);

            return date;
        }

        protected DateTime randomEuroDateGenerator()
        {
            
            DateTime lowerLim = Convert.ToDateTime("8:00AM");
            DateTime upperLim = Convert.ToDateTime("5:00PM");
            int hour;
            int minute;
            int lowerhour = lowerLim.Hour;
            int upperhour = upperLim.Hour;
            int min = lowerLim.Minute;
            bool check = true;


            DateTime date = DateTime.Now;

            int months = rand.Next(upperBoundMonth);
            double days = 31 * rand.NextDouble();
            double hours = 24 * rand.NextDouble();
            double minutes = 60 * rand.NextDouble();

            date = date.AddMonths(months);
            date = date.AddDays(days);
            do
            {
                check = true;
                date = date.AddHours(hours);
                date = date.AddMinutes(minutes);
                hour = date.Hour;
                minute = date.Minute;

                if (hour == lowerhour || hour == upperhour)
                {
                    if (minute <= min)
                        check = true;
                    else
                        check = false;
                }
            } while (hour < lowerhour || hour > upperhour || !check);
            date = date.ToUniversalTime();
            return date;
        }

        // This function randomly creates a phone number based on US phone lines
        protected string phoneNumberGenerator()
        {
            
            string number = "(";
            rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                if (i == 3)
                    number += ") ";
                if (i == 6)
                    number += "-";

                number += rand.Next(10);
            }
            return number;
        }

        // This function takes an input string and outputs that string without spaces in it
        protected string removeSpaces(string input)
        {
            string spacesRemoved = "";
            char[] characters = input.ToLower().ToCharArray();
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != ' ')
                {
                    spacesRemoved += characters[i];
                }
            }
            return spacesRemoved;
        }

        // This function takes an input string (company name) and returns a URL address as a string
        protected string createWebAddress(string company)
        {
            string nameToLowerWithoutSpaces = removeSpaces(company);
            string webAddress = "www." + nameToLowerWithoutSpaces + ".com";
            return webAddress;
        }

        protected string randomPriorityGenerator()
        {
            
            string priority = "None";

            int value = rand.Next(4);
            if (value >= 3)
                priority = "None";
            else
            {
                if (value >= 2)
                    priority = "High";
                else
                {
                    if (value >= 1)
                        priority = "Medium";
                    else
                        priority = "Low";
                }
            }
            return priority;
        }

        protected string randomChinesePriorityGenerator()
        {
            
            string priority = "无";

            int value = rand.Next(4);
            if (value >= 3)
                priority = "无";
            else
            {
                if (value >= 2)
                    priority = "高";
                else
                {
                    if (value >= 1)
                        priority = "中";
                    else
                        priority = "低";
                }
            }
            return priority;
        }

        public string GetFakeCompanyName()
        {
            
            // Want to make a request to see if the account already exists
            SDataSingleResourceRequest request = new SDataSingleResourceRequest(service)
            {
                ResourceKind = "accounts"
            };

            var names = new[]
                        {
                            #region Values
                            "Acme, Inc.",
                            "Widget Corp",
                            "123 Warehousing",
                            "Demo Company",
                            "Smith and Co.",
                            "Foo Bars",
                            "ABC Telecom",
                            "Fake Brothers",
                            "QWERTY Logistics",
                            "Demo, inc.",
                            "Sample Company",
                            "Sample, inc",
                            "Acme Corp",
                            "Allied Biscuit",
                            "Ankh-Sto Associates",
                            "Extensive Enterprise",
                            "Galaxy Corp",
                            "Globo-Chem",
                            "Mr. Sparkle",
                            "Globex Corporation",
                            "LexCorp",
                            "LuthorCorp",
                            "North Central Positronics",
                            "Omni Consumer Products",
                            "Praxis Corporation",
                            "Sombra Corporation",
                            "Sto Plains Holdings",
                            "Tessier-Ashpool",
                            "Wayne Enterprises",
                            "Wentworth Industries",
                            "ZiffCorp",
                            "Bluth Company",
                            "Strickland Propane",
                            "Thatherton Fuels",
                            "Three Waters",
                            "Water and Power",
                            "Western Gas & Electric",
                            "Mammoth Pictures",
                            "Mooby Corp",
                            "Gringotts",
                            "Thrift Bank",
                            "Flowers By Irene",
                            "The Legitimate Businessmens Club",
                            "Osato Chemicals",
                            "Transworld Consortium",
                            "Universal Export",
                            "United Fried Chicken",
                            "Virtucon",
                            "Kumatsu Motors",
                            "Keedsler Motors",
                            "Powell Motors",
                            "Industrial Automation",
                            "Sirius Cybernetics Corporation",
                            "U.S. Robotics and Mechanical Men",
                            "Colonial Movers",
                            "Corellian Engineering Corporation",
                            "Incom Corporation",
                            "General Products",
                            "Leeding Engines Ltd.",
                            "Blammo",
                            "Input, Inc.",
                            "Mainway Toys",
                            "Videlectrix",
                            "Zevo Toys",
                            "Ajax",
                            "Axis Chemical Co.",
                            "Barrytron",
                            "Carrys Candles",
                            "Cogswell Cogs",
                            "Spacely Sprockets",
                            "General Forge and Foundry",
                            "Duff Brewing Company",
                            "Dunder Mifflin",
                            "General Services Corporation",
                            "Monarch Playing Card Co.",
                            "Krustyco",
                            "Initech",
                            "Roboto Industries",
                            "Primatech",
                            "Sonky Rubber Goods",
                            "St. Anky Beer",
                            "Stay Puft Corporation",
                            "Vandelay Industries",
                            "Wernham Hogg",
                            "Gadgetron",
                            "Burleigh and Stronginthearm",
                            "BLAND Corporation",
                            "Nordyne Defense Dynamics",
                            "Petrox Oil Company",
                            "Roxxon",
                            "McMahon and Tate",
                            "Sixty Second Avenue",
                            "Charles Townsend Agency",
                            "Spade and Archer",
                            "Megadodo Publications",
                            "Rouster and Sideways",
                            "C.H. Lavatory and Sons",
                            "Globo Gym American Corp",
                            "The New Firm",
                            "SpringShield",
                            "Compuglobalhypermeganet",
                            "Data Systems",
                            "Gizmonic Institute",
                            "Initrode",
                            "Taggart Transcontinental",
                            "Atlantic Northern",
                            "Niagular",
                            "Plow King",
                            "Big Kahuna Burger",
                            "Big T Burgers and Fries",
                            "Chez Quis",
                            "Chotchkies",
                            "The Frying Dutchman",
                            "Klimpys",
                            "The Krusty Krab",
                            "Monks Diner",
                            "Milliways",
                            "Minuteman Cafe",
                            "Taco Grande",
                            "Tip Top Cafe",
                            "Moes Tavern",
                            "Central Perk",
                            "Chasers",
                            "Wal-Mart Stores, Inc.",
                            "Exxon Mobil Corporation",
                            "Chevron Corp.",
                            "General Electric Company",
                            "Bank of America",
                            "ConocoPhillips",
                            "AT&T Inc.",
                            "Ford Motor Company",
                            "JPMorgan Chase & Co.",
                            "Hewlett-Packard Co.",
                            "Berkshire Hathaway Inc.",
                            "Citigroup Incorporated",
                            "Verizon Communications, Incorporated",
                            "McKesson Corporation",
                            "General Motors Company",
                            "American International Group",
                            "Cardinal Health",
                            "CVS Caremark Corporation",
                            "Wells Fargo & Company",
                            "International Business Machines Corp.",
                            "UnitedHealth Group",
                            "Procter & Gamble Co.",
                            "The Kroger Co.",
                            "AmerisourceBergen Corporation",
                            "Costco Wholesale Corporation",
                            "Valero Energy Corporation",
                            "Archer Daniels Midland Company",
                            "Home Depot U S A , Inc.",
                            "Target Corporation, Inc.",
                            "WellPoint, Inc.",
                            "Walgreen Company",
                            "Johnson & Johnson",
                            "State Farm Mutual Automobile Insurance Company",
                            "Medco Health Solutions, Inc.",
                            "Microsoft Corporation",
                            "United Technologies Corporation",
                            "Dell Inc",
                            "The Goldman Sachs Group, Inc.",
                            "Pfizer Incorporated",
                            "Marathon Oil Corporation",
                            "Lowes Companies, Inc.",
                            "United Parcel Service, Inc.",
                            "Lockheed Martin Corp.",
                            "Best Buy Co. Inc.",
                            "Dow Chemical Company",
                            "Supervalu Inc.",
                            "Sears Holdings Corporation",
                            "International Assets Holding Corporation (Orland...",
                            "PepsiCo Inc.",
                            "Metlife, Incorporated",
                            "Safeway, Inc.",
                            "Freddie Mac",
                            "SYSCO Corporation",
                            "Apple Inc.",
                            "The Walt Disney Co.",
                            "Cisco Systems, Inc.",
                            "Comcast Corporation",
                            "FedEx Corporation",
                            "Northrop Grumman Corporation",
                            "Intel Corp",
                            "Aetna Inc.",
                            "New York Life Insurance Company",
                            "Prudential Financial, Incorporated",
                            "Caterpillar",
                            "Sprint Nextel Corporation",
                            "Allstate Insurance Company",
                            "General Dynamics Corporation",
                            "Morgan Stanley",
                            "Liberty Mutual Holding Company Inc",
                            "The Coca-Cola Co.",
                            "Humana Inc.",
                            "Honeywell International Inc.",
                            "Abbott Laboratories",
                            "News Corporation",
                            "HCA Inc.",
                            "Sunoco, Inc.",
                            "Hess Corporation",
                            "Ingram Micro Inc.",
                            "Fannie Mae",
                            "Time Warner Inc.",
                            "Johnson Controls, Inc.",
                            "Delta Air Lines, Inc.",
                            "Merck & Co",
                            "DuPont",
                            "Tyson Foods, Inc.",
                            "American Express",
                            "Rite Aid Corporation",
                            "TIAA-CREF",
                            "CHS Inc.",
                            "Massachusetts Mutual Life Insurance Company",
                            "Philip Morris International Inc",
                            "Raytheon Company",
                            "Express Scripts, Inc.",
                            "The Hartford Financial Services Group Inc.",
                            "The Travelers Companies, Inc.",
                            "Publix Super Markets, Inc.",
                            "Amazon.com, Inc.",
                            "Staples, Inc.",
                            "Google Inc.",
                            "Macys, Inc.",
                            "International Paper Company",
                            "Oracle Corporation",
                            "3M Company",
                            "John Deere Company",
                            "McDonalds Corporation",
                            "Tech Data Corporation",
                            "Motorola Mobility, Inc",
                            "Fluor Corporation",
                            "Eli Lilly and Company",
                            "Coca-Cola Enterprises",
                            "Bristol-Myers Squibb Company",
                            "Northwestern Mutual Life Insurance Company",
                            "DIRECTV",
                            "Emerson Electric Co.",
                            "Nationwide Mutual Insurance Company",
                            "The TJX Companies, Inc.",
                            "American Airlines",
                            "U.S. Bancorp",
                            "PNC Financial Services Group Inc",
                            "Nike, Inc.",
                            "Murphy Oil Corporation",
                            "Kimberly-Clark Corporation",
                            "Alcoa Inc.",
                            "Plains All American Pipeline, L.P.",
                            "CIGNA Corporation",
                            "AFLAC Incorporated",
                            "Time Warner Cable, Inc.",
                            "USAA",
                            "J. C. Penney Company, Inc.",
                            "Exelon Corporation",
                            "Kohls Corporation",
                            "Whirlpool Corporation",
                            "Altria Group, Inc.",
                            "Computer Sciences Corporation",
                            "Tesoro Corporation",
                            "United Continental Holdings, Inc.",
                            "Goodyear Tire & Rubber Co.",
                            "Avnet, Inc.",
                            "ManpowerGroup",
                            "Capital One",
                            "Southern Company",
                            "Health Net, Inc.",
                            "Florida Power & Light Company",
                            "L-3 Communications Inc",
                            "Occidental Petroleum Corp",
                            "Colgate-Palmolive Company",
                            "Xerox Corporation",
                            "Dominion",
                            "Freeport-McMoRan Copper & Gold",
                            "General Mills, Inc.",
                            "AES Corporation",
                            "Arrow Electronics, Inc.",
                            "Halliburton",
                            "Amgen Inc.",
                            "Medtronic Inc.",
                            "Progressive Corporation",
                            "GAP, Inc. / Banana Republic / Old Navy",
                            "Smithfield Foods, Inc.",
                            "Union Pacific Corporation",
                            "Loews Corporation",
                            "EMC Corporation",
                            "BNSF Railway Company",
                            "Coventry Health Care, Inc.",
                            "Illinois Tool Works Inc.",
                            "Viacom Inc.",
                            "Toys R Us, Incorporated",
                            "American Electric Power Company, Inc.",
                            "Pacific Gas and Electric Corp",
                            "ConEdison Solutions",
                            "Chubb Corporation",
                            "CBS Corporation",
                            "ConAgra Foods, Inc.",
                            "FirstEnergy Corp",
                            "Sara Lee Corporation",
                            "Duke Energy Corporation",
                            "National Oilwell Varco",
                            "Continental Airlines, Inc.",
                            "Kellogg Company",
                            "Baxter International Inc.",
                            "Public Service Enterprise Group Incorporated.",
                            "Edison International",
                            "Aramark Corporation",
                            "PPG Industries, Inc.",
                            "Community Health Systems, Incorporated",
                            "Office Depot, Inc.",
                            "KBR, Inc.",
                            "Eaton Corporation",
                            "Dollar General Corporation",
                            "Waste Management, Inc.",
                            "Monsanto Company",
                            "Omnicom Group Inc.",
                            "Jabil Circuit, Inc.",
                            "DISH Network Corporation",
                            "TRW Automotive Holdings Corp.",
                            "Navistar, Inc.",
                            "Jacobs Engineering Group Inc.",
                            "World Fuel Services Corporation",
                            "Nucor Corporation",
                            "Danaher Corporation",
                            "Dean Foods Company",
                            "ONEOK, Inc.",
                            "Liberty Global, Inc.",
                            "United States Steel Corporation",
                            "AutoNation, Inc.",
                            "Marriott International, Inc",
                            "SAIC, Inc.",
                            "YUM! Brands, Inc.",
                            "Branch Banking & Trust (BB&T)",
                            "Cummins, Inc.",
                            "Entergy Corporation",
                            "Textron Inc.",
                            "Marsh & McLennan Companies",
                            "US Airways Group, Inc.",
                            "Texas Instruments Incorporated",
                            "SunTrust Banks, Inc.",
                            "QUALCOMM Incorporated",
                            "Land OLakes Inc",
                            "Liberty Media Corporation",
                            "Avon Products, Inc.",
                            "Southwest Airlines Co.",
                            "Parker Hannifin Corporation",
                            "The Mosaic Company",
                            "BJs Wholesale Club, Inc.",
                            "Heinz North America",
                            "Thermo Fisher Scientific Inc.",
                            "UNUM Group",
                            "Genuine Parts Company",
                            "Guardian Life",
                            "Kiewit Building Group Inc.",
                            "Progress Energy Inc.",
                            "RR Donnelley & Sons Company",
                            "Starbucks Corporation",
                            "Lear Corporation",
                            "Baker Hughes",
                            "Xcel Energy Inc.",
                            "Penske Automotive Group, Inc.",
                            "Energy Future Holdings",
                            "The Great Atlantic & Pacific Tea Co. - A&P",
                            "Fifth Third Bancorp",
                            "State Street Corporation",
                            "First Data Corporation",
                            "Pepco Holdings, Inc.",
                            "URS Corporation",
                            "Tenet Healthcare Corporation",
                            "Regions Financial Corporation",
                            "GameStop Corp.",
                            "Lincoln Financial Group",
                            "Genworth Financial, Inc.",
                            "XTO Energy Inc.",
                            "CSX Corporation",
                            "Anadarko Petroleum Corporation",
                            "Devon Energy Corp",
                            "Praxair, Inc.",
                            "NRG Energy, Incorporated",
                            "Caesars Entertainment, Inc.",
                            "Automatic Data Processing, Incorporated (ADP)",
                            "The Principal Financial Group, Inc.",
                            "eBay Inc.",
                            "Assurant Inc.",
                            "Limited Brands, Inc.",
                            "Nordstrom, Inc.",
                            "Apache Corporation",
                            "R.J. Reynolds Tobacco Company",
                            "Air Products and Chemicals, Inc.",
                            "The Bank of New York Mellon",
                            "CenterPoint Energy, Inc.",
                            "Williams Companies, Inc.",
                            "Smith International, Inc.",
                            "Republic Services, Inc.",
                            "Boston Scientific Corporation",
                            "Ashland Chemical Company",
                            "Sempra Energy",
                            "PACCAR Inc.",
                            "Owens & Minor, Inc.",
                            "Whole Foods Market, Inc.",
                            "DTE Energy Company",
                            "Discover Financial Services",
                            "Norfolk Southern Corporation",
                            "Ameriprise Financial Services Inc.",
                            "Crown Holdings, Inc.",
                            "Icahn Enterprises L.P.",
                            "Masco Corporation",
                            "Cablevision Systems Corporation",
                            "Huntsman Corporation",
                            "SYNNEX Corporation",
                            "Newmont Mining Corporation",
                            "Chesapeake Energy Corporation",
                            "Eastman Kodak Company",
                            "AON Corporation",
                            "Campbell Soup Company",
                            "PPL Corporation",
                            "C.H. Robinson Worldwide, Inc",
                            "Integrys Energy Group, Inc.",
                            "Quest Diagnostics Incorporated",
                            "Western Digital Corp.",
                            "Family Dollar Stores, Inc.",
                            "Winn-Dixie Stores, Inc.",
                            "Ball Corporation",
                            "The Estee Lauder Companies Inc.",
                            "The Shaw Group Inc.",
                            "VF Corporation",
                            "Darden Restaurants, Inc.",
                            "Becton, Dickinson and Company (BD)",
                            "OfficeMax, Inc.",
                            "Bed Bath and Beyond",
                            "Kinder Morgan",
                            "Ross Stores, Inc",
                            "Pilgrims Pride Corporation",
                            "Hertz Corporation",
                            "The Sherwin-Williams Company",
                            "Ameren Corporation",
                            "Reinsurance Group of America, Incorporated",
                            "O-I (Owens Illinois), Inc.",
                            "CarMax, Inc.",
                            "Gilead Sciences, Inc.",
                            "Precision Castparts Corp.",
                            "Visa, Inc.",
                            "Commercial Metals Company",
                            "WellCare Health Plans, Inc.",
                            "AutoZone, Inc.",
                            "Western Refining, Inc.",
                            "Dole Food Company, Inc.",
                            "Charter Communications, Inc.",
                            "Stryker Corporation",
                            "Goodrich Corporation",
                            "Visteon Corporation",
                            "NiSource Inc.",
                            "AGCO Corporation",
                            "Calpine Corporation",
                            "Henry Schein Medical Systems, Inc.",
                            "Hormel Foods Corporation",
                            "ACS Government Systems, Inc.",
                            "Thrivent Financial for Lutherans",
                            "Yahoo!, Inc.",
                            "American Family Insurance Group",
                            "Sonic Automotive, Inc.",
                            "Peabody Energy Corporation",
                            "Omnicare, Inc.",
                            "Dillards, Inc.",
                            "W.W. Grainger, Inc.",
                            "CMS Energy Corporation",
                            "Fortune Brands, Inc.",
                            "AECOM",
                            "Symantec Corporation",
                            "DaVita, Inc.",
                            "KeyCorp",
                            "MEADWESTVACO",
                            "Interpublic Group of Companies Inc",
                            "Virgin Media Inc.",
                            "MGM Resorts International",
                            "The First American Financial Corporation",
                            "Avery Dennison Corporation",
                            "The McGraw-Hill Companies, Inc.",
                            "Enbridge Energy Partners, L.P.",
                            "Ecolab Inc.",
                            "Fidelity National Financial, Inc.",
                            "Dover Corporation",
                            "Global Partners LP",
                            "UGI Corporation",
                            "Gannett Company, Inc.",
                            "Harris Corporation",
                            "Barnes & Noble, Inc.",
                            "Newell Rubbermaid Inc.",
                            "Smurfit-Stone Container Enterprises Inc.",
                            "Pitney Bowes Inc.",
                            "EMCOR Group, Inc.",
                            "Dr Pepper Snapple Group Inc.",
                            "Weyerhaeuser Company",
                            "SunGard Data Systems Inc.",
                            "CH2M HILL Companies, Ltd.",
                            "The Pantry, Inc.",
                            "Domtar Corp.",
                            "The Clorox Company",
                            "Northeast Utilities",
                            "Oshkosh Corporation",
                            "Mattel, Inc",
                            "Energy Transfer Partners, L.P.",
                            "Advance Auto Parts, Inc",
                            "Advanced Micro Devices, Inc.",
                            "Corning Incorporated",
                            "Mohawk Industries, Inc.",
                            "PetSmart, Inc.",
                            "Reliance Steel and Aluminum",
                            "The Hershey Company",
                            "YRC WorldWide Inc.",
                            "Dollar Tree, Inc.",
                            "Dana Holding Company",
                            "Cameron International Corporation",
                            "Nash Finch Company",
                            "Pacific Life Insurance Company",
                            "Terex Corporation",
                            "Universal Health Services, Inc.",
                            "Amerigroup Corporation",
                            "Sanmina-SCI Corporation",
                            "Jarden Corporation",
                            "Tutor Perini Corporation",
                            "Mutual of Omaha Insurance Company",
                            "Avis Budget Group, Inc.",
                            "Autoliv ASP, Inc.",
                            "Mylan Inc.",
                            "Western Union Company",
                            "Celanese Corporation",
                            "Eastman Chemical Company",
                            "Telephone and Data Systems, Inc.",
                            "Polo Ralph Lauren Corporation",
                            "Auto-Owners Insurance Group",
                            "Core-Mark International Inc",
                            "Applied Materials Inc.",
                            "CenturyLink, Inc.",
                            "Atmos Energy Corporation",
                            "Ryder System, Inc.",
                            "SPX Corporation",
                            "Foot Locker, Inc.",
                            "OReilly Automotive, Inc.",
                            "Harley-Davidson, Inc.",
                            "HollyFrontier Corporation",
                            "Micron Technology, Inc.",
                            "Owens Corning",
                            "EOG Resources, Inc.",
                            "Big Lots Stores, Inc.",
                            "Spectra Energy Partners, L.P.",
                            "Starwood Hotels & Resorts Worldwide, Inc.",
                            "United Stationers Inc.",
                            "TravelCenters of America, Inc.",
                            "BlackRock, Inc.",
                            "Laboratory Corporation of America",
                            "Health Management Associates, Inc.",
                            "NYSE Euronext, Inc.",
                            "St. Jude Medical, Inc.",
                            "Tenneco Incorporated",
                            "El Paso Corporation",
                            "WESCO Distribution, Inc.",
                            "CONSOL Energy Inc.",
                            "Meritor, Inc.",
                            "NCR Corporation",
                            "Unisys Corporation",
                            "The Lubrizol Corporation",
                            "Alliant Techsystems, Inc.",
                            "The Washington Post Company",
                            "Las Vegas Sands, Inc.",
                            "Group 1 Automotive, Inc.",
                            "Genzyme Corporation",
                            "Allergan, Inc.",
                            "Broadcom Corporation",
                            "Agilent Technologies, Inc.",
                            "Rockwell Collins, Inc.",
                            "W. R. Berkley Corporation",
                            "Charles Schwab Corporation",
                            "Dicks Sporting Goods Inc",
                            "FMC Technologies, Inc.",
                            "NII Holdings, Inc.",
                            "General Cable Corporation",
                            "Graybar Electric Company, Inc.",
                            "Biogen Idec Inc.",
                            "AbitibiBowater",
                            "Flowserve Corporation",
                            "Airgas Inc",
                            "CNO Financial Group, Inc.",
                            "Rockwell Automation, Inc.",
                            "Kindred Healthcare, Inc.",
                            "American Financial Group, Inc.",
                            "Kelly Services, Inc.",
                            "Spectrum Group International",
                            "RadioShack Corporation",
                            "CA Technologies, Inc.",
                            "Con-Way Freight Inc.",
                            "Erie Insurance Group",
                            "Caseys General Stores, Inc.",
                            "Centene Corporation",
                            "Sealed Air Corporation",
                            "Frontier Oil Corporation",
                            "SCANA Corporation",
                            "Live Nation, Inc.",
                            "Fiserv, Inc.",
                            "Host Hotels & Resorts, Inc.",
                            "H&R Block",
                            "Electronic Arts Inc.",
                            "Franklin Templeton Investments",
                            "Wisconsin Energy Corporation (We Energies)",
                            "Northern Trust Corporation",
                            "MDU Resources Group, Inc.",
                            "CB Richard Ellis Group, Inc.",
                            "Blockbuster LLC",
                            "Baker Hughes",
                            "Insight Enterprises, Inc.",
                            "Levi Strauss and Co.",
                            "Graphic Packaging Holding Company",
                            "Targa Resources Inc.",
                            "Zimmer Holdings, Inc.",
                            "Expeditors International of Washington, Inc.",
                            "Pulte Group",
                            "Ruddick Corporation",
                            "AK Steel Corporation",
                            "Hasbro, Inc.",
                            "Unified Grocers, Inc.",
                            "Energizer Holdings, Inc.",
                            "CIT Group Inc.",
                            "Apollo Group, Inc.",
                            "BorgWarner Inc.",
                            "Steel Dynamics, Inc.",
                            "Realogy Corporation",
                            "Forest Laboratories, Inc.",
                            "Del Monte Foods Company",
                            "Cincinnati Financial Corporation",
                            "Ralcorp a/k/a Bremner Food Group",
                            "Hanesbrands Inc.",
                            "Michaels Stores, Inc.",
                            "Lexmark International, Inc.",
                            "Hospira, Inc.",
                            "NuStar Energy L.P.",
                            "Centex Corporation",
                            "Old Republic International Corporation",
                            "Asbury Automotive Group, Inc.",
                            "Certegy",
                            "Manitowoc Company, Inc",
                            "Simon Property Group, Inc.",
                            "Cintas Corporation",
                            "M&T Bank Corporation",
                            "Stater Brothers Markets",
                            "Level 3 Communications, Inc",
                            "The J. M. Smucker Company",
                            "Wyndham Worldwide Corp.",
                            "Nalco Holding Company",
                            "Stanley Black & Decker, Inc.",
                            "Lorillard Tobacco Company",
                            "FM Global",
                            "Corn Products International, Inc.",
                            "Molina Healthcare, Inc.",
                            "D.R. Horton, Inc.",
                            "Constellation Brands, Inc.",
                            "The Neiman Marcus Group, Inc.",
                            "Brinker International, Inc.",
                            "Seaboard Corporation",
                            "Joy Global Inc",
                            "Sonoco Products Co.",
                            "Edward Jones",
                            "NV Energy Inc",
                            "The Wendys Company",
                            "Temple-Inland Inc.",
                            "Burlington Coat Factory Corporation",
                            "SanDisk Corporation",
                            "VWR International, LLC",
                            "The Timken Company",
                            "Discovery Communications, Inc.",
                            "Bemis Company, Inc.",
                            "US Oncology Inc",
                            "ABM Industries Incorporated",
                            "MetroPCS Communications, Inc.",
                            "Chiquita Brands International, Inc.",
                            "UNFI",
                            "Alliant Energy Corporation",
                            "Allegheny Energy, Inc.",
                            "Annaly Mortgage Management, Inc.",
                            "THE NASDAQ STOCK MARKET INTERNATIONAL LTD.",
                            "NetApp Inc",
                            "Alaska Air Group, Inc.",
                            "Marshall & Ilsley Bank",
                            "RPM International Inc.",
                            "Pactiv Corporation",
                            "Legg Mason",
                            "Belk, Inc.",
                            "Hyatt Hotels Corporation",
                            "Puget Sound Energy Inc",
                            "Jones Group Inc",
                            "NVIDIA Corporation",
                            "Exide Technologies",
                            "Zions Bancorporation",
                            "Quanta Services, Inc.",
                            "Juniper Networks, Inc.",
                            "TECO Energy, Inc.",
                            "Pinnacle West Capital Corporation",
                            "Collective Brands Inc.",
                            "JetBlue Airways Corporation",
                            "Life Technologies Corporation",
                            "Cognizant Technology Solutions Corp.",
                            "Beckman Coulter, Inc.",
                            "Ingles Markets, Inc.",
                            "Huntington Bancshares Inc.",
                            "The ServiceMaster Company",
                            "COUNTRY Financial",
                            "USG Corporation",
                            "Coach, Inc.",
                            "Torchmark Corporation",
                            "Brightpoint",
                            "Tractor Supply Company, Inc.",
                            "J.B. Hunt Transport Services, Incorporated",
                            "Vanguard Health Systems, Inc.",
                            "McCormick & Company Inc",
                            "Berry Plastics Corporation",
                            "Steelcase Inc.",
                            "Intuit Inc.",
                            "Warner Music Group",
                            "Securian Financial",
                            "NSTAR Gas & Electric Company",
                            "Systemax Inc.",
                            "Comerica Incorporated",
                            "Ambac Financial Group, Inc.",
                            "The Scotts Miracle-Gro Company",
                            "CVR Energy, Inc.",
                            "General Growth Properties, Inc"

                            #endregion            };
                        };

            var index = rand.Next(1, names.Count());
            return names[index];
        }

        public string GetFakeChineseCompanyName()
        {
            
            // Want to make a request to see if the account already exists
            SDataSingleResourceRequest request = new SDataSingleResourceRequest(service)
            {
                ResourceKind = "accounts"
            };

            var names = new[]
                        {
                            #region Values
                             "魏桥纺织股份有限公司",
                            "马鞍山钢铁股份有限公司",	
                               "首都信息发展股份有限公司",	
    "首创置业股份有限公司	",
    "鞍钢新轧钢股份有限公司",	
    "青岛啤酒股份有限公司	",
    "长春达兴药业股份有限公司	",
"长城科技股份有限公司",	
"长城汽车股份有限公司",	
"重庆钢铁股份有限公司",	
"郑州燃气股份有限公司",	
"赛迪顾问股份有限公司",	
"联华超市股份有限公司",	
"经纬纺织机械股份有限公司",
"紫金矿业集团股份有限公司",
            "第一拖拉机股份有限公司",
"皖通高速公路股份有限公司",
"牡丹汽车股份有限公司",
"烟台北方安德利果汁股份有限公司",	
"潍柴动力股份有限公司",
"深圳高速公路股份有限公司",
"深圳市宝德科技股份有限公司",
"深圳市元征科技股份有限公司",
"深圳市东江环保股份有限公司",
"深圳中航实业股份有限公司",
"海南美兰机场股份有限公司",
"浙江玻璃股份有限公司",
"浙江浙大网新兰德股份有限公司",
"浙江沪杭甬高速公路股份有限公司",	
"浙江永隆实业股份有限公司",
"沈阳公用发展股份有限公司",
"江西铜业股份有限公司",	
"江苏宁沪高速公路股份有限公司",	
"江苏南大苏富特软件股份有限公司",
"比亚迪股份有限公司",
"成都普天电缆股份有限公司",
"成都托普科技股份有限公司",
"成渝高速公路股份有限公司",
"庆铃汽车股份有限公司",
"广深铁路股份有限公司",
"广州药业股份有限公司",
"广州广船国际股份有限公司",
"广东科龙电器股份有限公司",
"常茂生物化学工程股份有限公司",
"山东新华制药股份有限公司",
"山东国际电源股份有限公司",
"宝业集团股份有限公司",
"安徽海螺水泥股份有限公司",
"天津泰达生物医学工程股份有限公司",
"天津创业环保股份有限公司",
"天津中新药业集团股份有限公司",
"复地(集团)股份有限公司",
"哈尔滨动力设备股份有限公司",
"吉林省辉南长龙生化药业股份有限公司",	
"吉林化学工业股份有限公司",
"南京熊猫电子股份有限公司",
"华能国际电力股份有限公司",
"华电国际电力股份有限公司",
"北人印刷机械股份有限公司",
"北京首都国际机场股份有限公司",
"北京大唐发电股份有限公司",
"北京同仁堂科技股份有限公司",
"北京北辰实业股份有限公司",
"兖州煤业股份有限公司",
"交大昆机科技股份有限公司",
"中海集装箱运输股份有限公司",
"中海油田服务股份有限公司",
"中海发展股份有限公司",
"中国铝业股份有限公司",
"中国航空科技工业股份有限公司",
"中国网络通信集团",
"中国石油天然气股份有限公司",
"中国石油化工股份有限公司",
"中国石化镇海炼油化工股份有限公司",
"中国石化北京燕化石油化工股份有限公司",
"中国石化仪征化纤股份有限公司",
"中国石化上海石油化工股份有限公司",
"中国电信股份有限公司",
"中国民航信息网络股份有限公司",
"中国平安保险(集团)股份有限公司",
"中国太平洋保险(集团)股份有限公司",
"中国外运股份有限公司",
"中国南方航空股份有限公司",	
"中国人民财产保险股份有限公司",
"中国东方航空股份有限公司",
"东方电机股份有限公司",
"东北虎药业股份有限公司",
"东北电气发展股份有限公司",
"上海复旦微电子股份有限公司",
"上海复旦张江生物医药股份有限公司",
"上海交大慧谷信息产业股份有限公司"

                            #endregion            };
                        };

            var index = rand.Next(1, names.Count());
            return names[index];
        }

        public string GetFakeFirstName()
        {
            
            var firstNames = new[]
                             {
                                 #region FirstNames
                                 "Aaron",
                                 "Aaron",
                                 "Abbey",
                                 "Abbie",
                                 "Abby",
                                 "Abdul",
                                 "Abe",
                                 "Abel",
                                 "Abigail",
                                 "Abraham",
                                 "Abram",
                                 "Ada",
                                 "Adah",
                                 "Adalberto",
                                 "Adaline",
                                 "Adam",
                                 "Adam",
                                 "Adan",
                                 "Addie",
                                 "Adela",
                                 "Adelaida",
                                 "Adelaide",
                                 "Adele",
                                 "Adelia",
                                 "Adelina",
                                 "Adeline",
                                 "Adell",
                                 "Adella",
                                 "Adelle",
                                 "Adena",
                                 "Adina",
                                 "Adolfo",
                                 "Adolph",
                                 "Adria",
                                 "Adrian",
                                 "Adrian",
                                 "Adriana",
                                 "Adriane",
                                 "Adrianna",
                                 "Adrianne",
                                 "Adrien",
                                 "Adriene",
                                 "Adrienne",
                                 "Afton",
                                 "Agatha",
                                 "Agnes",
                                 "Agnus",
                                 "Agripina",
                                 "Agueda",
                                 "Agustin",
                                 "Agustina",
                                 "Ahmad",
                                 "Ahmed",
                                 "Ai",
                                 "Aida",
                                 "Aide",
                                 "Aiko",
                                 "Aileen",
                                 "Ailene",
                                 "Aimee",
                                 "Aisha",
                                 "Aja",
                                 "Akiko",
                                 "Akilah",
                                 "Al",
                                 "Alaina",
                                 "Alaine",
                                 "Alan",
                                 "Alana",
                                 "Alane",
                                 "Alanna",
                                 "Alayna",
                                 "Alba",
                                 "Albert",
                                 "Albert",
                                 "Alberta",
                                 "Albertha",
                                 "Albertina",
                                 "Albertine",
                                 "Alberto",
                                 "Albina",
                                 "Alda",
                                 "Alden",
                                 "Aldo",
                                 "Alease",
                                 "Alec",
                                 "Alecia",
                                 "Aleen",
                                 "Aleida",
                                 "Aleisha",
                                 "Alejandra",
                                 "Alejandrina",
                                 "Alejandro",
                                 "Alena",
                                 "Alene",
                                 "Alesha",
                                 "Aleshia",
                                 "Alesia",
                                 "Alessandra",
                                 "Aleta",
                                 "Aletha",
                                 "Alethea",
                                 "Alethia",
                                 "Alex",
                                 "Alex",
                                 "Alexa",
                                 "Alexander",
                                 "Alexander",
                                 "Alexandra",
                                 "Alexandria",
                                 "Alexia",
                                 "Alexis",
                                 "Alexis",
                                 "Alfonso",
                                 "Alfonzo",
                                 "Alfred",
                                 "Alfreda",
                                 "Alfredia",
                                 "Alfredo",
                                 "Ali",
                                 "Ali",
                                 "Alia",
                                 "Alica",
                                 "Alice",
                                 "Alicia",
                                 "Alida",
                                 "Alina",
                                 "Aline",
                                 "Alisa",
                                 "Alise",
                                 "Alisha",
                                 "Alishia",
                                 "Alisia",
                                 "Alison",
                                 "Alissa",
                                 "Alita",
                                 "Alix",
                                 "Aliza",
                                 "Alla",
                                 "Allan",
                                 "Alleen",
                                 "Allegra",
                                 "Allen",
                                 "Allen",
                                 "Allena",
                                 "Allene",
                                 "Allie",
                                 "Alline",
                                 "Allison",
                                 "Allyn",
                                 "Allyson",
                                 "Alma",
                                 "Almeda",
                                 "Almeta",
                                 "Alona",
                                 "Alonso",
                                 "Alonzo",
                                 "Alpha",
                                 "Alphonse",
                                 "Alphonso",
                                 "Alta",
                                 "Altagracia",
                                 "Altha",
                                 "Althea",
                                 "Alton",
                                 "Alva",
                                 "Alva",
                                 "Alvaro",
                                 "Alvera",
                                 "Alverta",
                                 "Alvin",
                                 "Alvina",
                                 "Alyce",
                                 "Alycia",
                                 "Alysa",
                                 "Alyse",
                                 "Alysha",
                                 "Alysia",
                                 "Alyson",
                                 "Alyssa",
                                 "Amada",
                                 "Amado",
                                 "Amal",
                                 "Amalia",
                                 "Amanda",
                                 "Amber",
                                 "Amberly",
                                 "Ambrose",
                                 "Amee",
                                 "Amelia",
                                 "America",
                                 "Ami",
                                 "Amie",
                                 "Amiee",
                                 "Amina",
                                 "Amira",
                                 "Ammie",
                                 "Amos",
                                 "Amparo",
                                 "Amy",
                                 "An",
                                 "Ana",
                                 "Anabel",
                                 "Analisa",
                                 "Anamaria",
                                 "Anastacia",
                                 "Anastasia",
                                 "Andera",
                                 "Anderson",
                                 "Andra",
                                 "Andre",
                                 "Andre",
                                 "Andrea",
                                 "Andrea",
                                 "Andreas",
                                 "Andree",
                                 "Andres",
                                 "Andrew",
                                 "Andrew",
                                 "Andria",
                                 "Andy",
                                 "Anette",
                                 "Angel",
                                 "Angel",
                                 "Angela",
                                 "Angele",
                                 "Angelena",
                                 "Angeles",
                                 "Angelia",
                                 "Angelic",
                                 "Angelica",
                                 "Angelika",
                                 "Angelina",
                                 "Angeline",
                                 "Angelique",
                                 "Angelita",
                                 "Angella",
                                 "Angelo",
                                 "Angelo",
                                 "Angelyn",
                                 "Angie",
                                 "Angila",
                                 "Angla",
                                 "Angle",
                                 "Anglea",
                                 "Anh",
                                 "Anibal",
                                 "Anika",
                                 "Anisa",
                                 "Anisha",
                                 "Anissa",
                                 "Anita",
                                 "Anitra",
                                 "Anja",
                                 "Anjanette",
                                 "Anjelica",
                                 "Ann",
                                 "Anna",
                                 "Annabel",
                                 "Annabell",
                                 "Annabelle",
                                 "Annalee",
                                 "Annalisa",
                                 "Annamae",
                                 "Annamaria",
                                 "Annamarie",
                                 "Anne",
                                 "Anneliese",
                                 "Annelle",
                                 "Annemarie",
                                 "Annett",
                                 "Annetta",
                                 "Annette",
                                 "Annice",
                                 "Annie",
                                 "Annika",
                                 "Annis",
                                 "Annita",
                                 "Annmarie",
                                 "Anthony",
                                 "Anthony",
                                 "Antione",
                                 "Antionette",
                                 "Antoine",
                                 "Antoinette",
                                 "Anton",
                                 "Antone",
                                 "Antonetta",
                                 "Antonette",
                                 "Antonia",
                                 "Antonia",
                                 "Antonietta",
                                 "Antonina",
                                 "Antonio",
                                 "Antonio",
                                 "Antony",
                                 "Antwan",
                                 "Anya",
                                 "Apolonia",
                                 "April",
                                 "Apryl",
                                 "Ara",
                                 "Araceli",
                                 "Aracelis",
                                 "Aracely",
                                 "Arcelia",
                                 "Archie",
                                 "Ardath",
                                 "Ardelia",
                                 "Ardell",
                                 "Ardella",
                                 "Ardelle",
                                 "Arden",
                                 "Ardis",
                                 "Ardith",
                                 "Aretha",
                                 "Argelia",
                                 "Argentina",
                                 "Ariana",
                                 "Ariane",
                                 "Arianna",
                                 "Arianne",
                                 "Arica",
                                 "Arie",
                                 "Ariel",
                                 "Ariel",
                                 "Arielle",
                                 "Arla",
                                 "Arlean",
                                 "Arleen",
                                 "Arlen",
                                 "Arlena",
                                 "Arlene",
                                 "Arletha",
                                 "Arletta",
                                 "Arlette",
                                 "Arlie",
                                 "Arlinda",
                                 "Arline",
                                 "Arlyne",
                                 "Armand",
                                 "Armanda",
                                 "Armandina",
                                 "Armando",
                                 "Armida",
                                 "Arminda",
                                 "Arnetta",
                                 "Arnette",
                                 "Arnita",
                                 "Arnold",
                                 "Arnoldo",
                                 "Arnulfo",
                                 "Aron",
                                 "Arron",
                                 "Art",
                                 "Arthur",
                                 "Arthur",
                                 "Artie",
                                 "Arturo",
                                 "Arvilla",
                                 "Asa",
                                 "Asha",
                                 "Ashanti",
                                 "Ashely",
                                 "Ashlea",
                                 "Ashlee",
                                 "Ashleigh",
                                 "Ashley",
                                 "Ashley",
                                 "Ashli",
                                 "Ashlie",
                                 "Ashly",
                                 "Ashlyn",
                                 "Ashton",
                                 "Asia",
                                 "Asley",
                                 "Assunta",
                                 "Astrid",
                                 "Asuncion",
                                 "Athena",
                                 "Aubrey",
                                 "Aubrey",
                                 "Audie",
                                 "Audra",
                                 "Audrea",
                                 "Audrey",
                                 "Audria",
                                 "Audrie",
                                 "Audry",
                                 "August",
                                 "Augusta",
                                 "Augustina",
                                 "Augustine",
                                 "Augustine",
                                 "Augustus",
                                 "Aundrea",
                                 "Aura",
                                 "Aurea",
                                 "Aurelia",
                                 "Aurelio",
                                 "Aurora",
                                 "Aurore",
                                 "Austin",
                                 "Austin",
                                 "Autumn",
                                 "Ava",
                                 "Avelina",
                                 "Avery",
                                 "Avery",
                                 "Avis",
                                 "Avril",
                                 "Awilda",
                                 "Ayako",
                                 "Ayana",
                                 "Ayanna",
                                 "Ayesha",
                                 "Azalee",
                                 "Azucena",
                                 "Azzie",
                                 "Babara",
                                 "Babette",
                                 "Bailey",
                                 "Bambi",
                                 "Bao",
                                 "Barabara",
                                 "Barb",
                                 "Barbar",
                                 "Barbara",
                                 "Barbera",
                                 "Barbie",
                                 "Barbra",
                                 "Bari",
                                 "Barney",
                                 "Barrett",
                                 "Barrie",
                                 "Barry",
                                 "Bart",
                                 "Barton",
                                 "Basil",
                                 "Basilia",
                                 "Bea",
                                 "Beata",
                                 "Beatrice",
                                 "Beatris",
                                 "Beatriz",
                                 "Beau",
                                 "Beaulah",
                                 "Bebe",
                                 "Becki",
                                 "Beckie",
                                 "Becky",
                                 "Bee",
                                 "Belen",
                                 "Belia",
                                 "Belinda",
                                 "Belkis",
                                 "Bell",
                                 "Bella",
                                 "Belle",
                                 "Belva",
                                 "Ben",
                                 "Benedict",
                                 "Benita",
                                 "Benito",
                                 "Benjamin",
                                 "Bennett",
                                 "Bennie",
                                 "Bennie",
                                 "Benny",
                                 "Benton",
                                 "Berenice",
                                 "Berna",
                                 "Bernadette",
                                 "Bernadine",
                                 "Bernard",
                                 "Bernarda",
                                 "Bernardina",
                                 "Bernardine",
                                 "Bernardo",
                                 "Berneice",
                                 "Bernetta",
                                 "Bernice",
                                 "Bernie",
                                 "Bernie",
                                 "Berniece",
                                 "Bernita",
                                 "Berry",
                                 "Berry",
                                 "Bert",
                                 "Berta",
                                 "Bertha",
                                 "Bertie",
                                 "Bertram",
                                 "Beryl",
                                 "Bess",
                                 "Bessie",
                                 "Beth",
                                 "Bethanie",
                                 "Bethann",
                                 "Bethany",
                                 "Bethel",
                                 "Betsey",
                                 "Betsy",
                                 "Bette",
                                 "Bettie",
                                 "Bettina",
                                 "Betty",
                                 "Bettyann",
                                 "Bettye",
                                 "Beula",
                                 "Beulah",
                                 "Bev",
                                 "Beverlee",
                                 "Beverley",
                                 "Beverly",
                                 "Bianca",
                                 "Bibi",
                                 "Bill",
                                 "Billi",
                                 "Billie",
                                 "Billie",
                                 "Billy",
                                 "Billy",
                                 "Billye",
                                 "Birdie",
                                 "Birgit",
                                 "Blaine",
                                 "Blair",
                                 "Blair",
                                 "Blake",
                                 "Blake",
                                 "Blanca",
                                 "Blanch",
                                 "Blanche",
                                 "Blondell",
                                 "Blossom",
                                 "Blythe",
                                 "Bo",
                                 "Bob",
                                 "Bobbi",
                                 "Bobbie",
                                 "Bobbie",
                                 "Bobby",
                                 "Bobby",
                                 "Bobbye",
                                 "Bobette",
                                 "Bok",
                                 "Bong",
                                 "Bonita",
                                 "Bonnie",
                                 "Bonny",
                                 "Booker",
                                 "Boris",
                                 "Boyce",
                                 "Boyd",
                                 "Brad",
                                 "Bradford",
                                 "Bradley",
                                 "Bradly",
                                 "Brady",
                                 "Brain",
                                 "Branda",
                                 "Brande",
                                 "Brandee",
                                 "Branden",
                                 "Brandi",
                                 "Brandie",
                                 "Brandon",
                                 "Brandon",
                                 "Brandy",
                                 "Brant",
                                 "Breana",
                                 "Breann",
                                 "Breanna",
                                 "Breanne",
                                 "Bree",
                                 "Brenda",
                                 "Brendan",
                                 "Brendon",
                                 "Brenna",
                                 "Brent",
                                 "Brenton",
                                 "Bret",
                                 "Brett",
                                 "Brett",
                                 "Brian",
                                 "Brian",
                                 "Briana",
                                 "Brianna",
                                 "Brianne",
                                 "Brice",
                                 "Bridget",
                                 "Bridgett",
                                 "Bridgette",
                                 "Brigette",
                                 "Brigid",
                                 "Brigida",
                                 "Brigitte",
                                 "Brinda",
                                 "Britany",
                                 "Britney",
                                 "Britni",
                                 "Britt",
                                 "Britt",
                                 "Britta",
                                 "Brittaney",
                                 "Brittani",
                                 "Brittanie",
                                 "Brittany",
                                 "Britteny",
                                 "Brittney",
                                 "Brittni",
                                 "Brittny",
                                 "Brock",
                                 "Broderick",
                                 "Bronwyn",
                                 "Brook",
                                 "Brooke",
                                 "Brooks",
                                 "Bruce",
                                 "Bruna",
                                 "Brunilda",
                                 "Bruno",
                                 "Bryan",
                                 "Bryanna",
                                 "Bryant",
                                 "Bryce",
                                 "Brynn",
                                 "Bryon",
                                 "Buck",
                                 "Bud",
                                 "Buddy",
                                 "Buena",
                                 "Buffy",
                                 "Buford",
                                 "Bula",
                                 "Bulah",
                                 "Bunny",
                                 "Burl",
                                 "Burma",
                                 "Burt",
                                 "Burton",
                                 "Buster",
                                 "Byron",
                                 "Caitlin",
                                 "Caitlyn",
                                 "Calandra",
                                 "Caleb",
                                 "Calista",
                                 "Callie",
                                 "Calvin",
                                 "Camelia",
                                 "Camellia",
                                 "Cameron",
                                 "Cameron",
                                 "Cami",
                                 "Camie",
                                 "Camila",
                                 "Camilla",
                                 "Camille",
                                 "Cammie",
                                 "Cammy",
                                 "Candace",
                                 "Candance",
                                 "Candelaria",
                                 "Candi",
                                 "Candice",
                                 "Candida",
                                 "Candie",
                                 "Candis",
                                 "Candra",
                                 "Candy",
                                 "Candyce",
                                 "Caprice",
                                 "Cara",
                                 "Caren",
                                 "Carey",
                                 "Carey",
                                 "Cari",
                                 "Caridad",
                                 "Carie",
                                 "Carin",
                                 "Carina",
                                 "Carisa",
                                 "Carissa",
                                 "Carita",
                                 "Carl",
                                 "Carl",
                                 "Carla",
                                 "Carlee",
                                 "Carleen",
                                 "Carlena",
                                 "Carlene",
                                 "Carletta",
                                 "Carley",
                                 "Carli",
                                 "Carlie",
                                 "Carline",
                                 "Carlita",
                                 "Carlo",
                                 "Carlos",
                                 "Carlos",
                                 "Carlota",
                                 "Carlotta",
                                 "Carlton",
                                 "Carly",
                                 "Carlyn",
                                 "Carma",
                                 "Carman",
                                 "Carmel",
                                 "Carmela",
                                 "Carmelia",
                                 "Carmelina",
                                 "Carmelita",
                                 "Carmella",
                                 "Carmelo",
                                 "Carmen",
                                 "Carmen",
                                 "Carmina",
                                 "Carmine",
                                 "Carmon",
                                 "Carol",
                                 "Carol",
                                 "Carola",
                                 "Carolann",
                                 "Carole",
                                 "Carolee",
                                 "Carolin",
                                 "Carolina",
                                 "Caroline",
                                 "Caroll",
                                 "Carolyn",
                                 "Carolyne",
                                 "Carolynn",
                                 "Caron",
                                 "Caroyln",
                                 "Carri",
                                 "Carrie",
                                 "Carrol",
                                 "Carrol",
                                 "Carroll",
                                 "Carroll",
                                 "Carry",
                                 "Carson",
                                 "Carter",
                                 "Cary",
                                 "Cary",
                                 "Caryl",
                                 "Carylon",
                                 "Caryn",
                                 "Casandra",
                                 "Casey",
                                 "Casey",
                                 "Casie",
                                 "Casimira",
                                 "Cassandra",
                                 "Cassaundra",
                                 "Cassey",
                                 "Cassi",
                                 "Cassidy",
                                 "Cassie",
                                 "Cassondra",
                                 "Cassy",
                                 "Catalina",
                                 "Catarina",
                                 "Caterina",
                                 "Catharine",
                                 "Catherin",
                                 "Catherina",
                                 "Catherine",
                                 "Cathern",
                                 "Catheryn",
                                 "Cathey",
                                 "Cathi",
                                 "Cathie",
                                 "Cathleen",
                                 "Cathrine",
                                 "Cathryn",
                                 "Cathy",
                                 "Catina",
                                 "Catrice",
                                 "Catrina",
                                 "Cayla",
                                 "Cecelia",
                                 "Cecil",
                                 "Cecil",
                                 "Cecila",
                                 "Cecile",
                                 "Cecilia",
                                 "Cecille",
                                 "Cecily",
                                 "Cedric",
                                 "Cedrick",
                                 "Celena",
                                 "Celesta",
                                 "Celeste",
                                 "Celestina",
                                 "Celestine",
                                 "Celia",
                                 "Celina",
                                 "Celinda",
                                 "Celine",
                                 "Celsa",
                                 "Ceola",
                                 "Cesar",
                                 "Chad",
                                 "Chadwick",
                                 "Chae",
                                 "Chan",
                                 "Chana",
                                 "Chance",
                                 "Chanda",
                                 "Chandra",
                                 "Chanel",
                                 "Chanell",
                                 "Chanelle",
                                 "Chang",
                                 "Chang",
                                 "Chantal",
                                 "Chantay",
                                 "Chante",
                                 "Chantel",
                                 "Chantell",
                                 "Chantelle",
                                 "Chara",
                                 "Charis",
                                 "Charise",
                                 "Charissa",
                                 "Charisse",
                                 "Charita",
                                 "Charity",
                                 "Charla",
                                 "Charleen",
                                 "Charlena",
                                 "Charlene",
                                 "Charles",
                                 "Charles",
                                 "Charlesetta",
                                 "Charlette",
                                 "Charley",
                                 "Charlie",
                                 "Charlie",
                                 "Charline",
                                 "Charlott",
                                 "Charlotte",
                                 "Charlsie",
                                 "Charlyn",
                                 "Charmain",
                                 "Charmaine",
                                 "Charolette",
                                 "Chas",
                                 "Chase",
                                 "Chasidy",
                                 "Chasity",
                                 "Chassidy",
                                 "Chastity",
                                 "Chau",
                                 "Chauncey",
                                 "Chaya",
                                 "Chelsea",
                                 "Chelsey",
                                 "Chelsie",
                                 "Cher",
                                 "Chere",
                                 "Cheree",
                                 "Cherelle",
                                 "Cheri",
                                 "Cherie",
                                 "Cherilyn",
                                 "Cherise",
                                 "Cherish",
                                 "Cherly",
                                 "Cherlyn",
                                 "Cherri",
                                 "Cherrie",
                                 "Cherry",
                                 "Cherryl",
                                 "Chery",
                                 "Cheryl",
                                 "Cheryle",
                                 "Cheryll",
                                 "Chester",
                                 "Chet",
                                 "Cheyenne",
                                 "Chi",
                                 "Chi",
                                 "Chia",
                                 "Chieko",
                                 "Chin",
                                 "China",
                                 "Ching",
                                 "Chiquita",
                                 "Chloe",
                                 "Chong",
                                 "Chong",
                                 "Chris",
                                 "Chris",
                                 "Chrissy",
                                 "Christa",
                                 "Christal",
                                 "Christeen",
                                 "Christel",
                                 "Christen",
                                 "Christena",
                                 "Christene",
                                 "Christi",
                                 "Christia",
                                 "Christian",
                                 "Christian",
                                 "Christiana",
                                 "Christiane",
                                 "Christie",
                                 "Christin",
                                 "Christina",
                                 "Christine",
                                 "Christinia",
                                 "Christoper",
                                 "Christopher",
                                 "Christopher",
                                 "Christy",
                                 "Chrystal",
                                 "Chu",
                                 "Chuck",
                                 "Chun",
                                 "Chung",
                                 "Chung",
                                 "Ciara",
                                 "Cicely",
                                 "Ciera",
                                 "Cierra",
                                 "Cinda",
                                 "Cinderella",
                                 "Cindi",
                                 "Cindie",
                                 "Cindy",
                                 "Cinthia",
                                 "Cira",
                                 "Clair",
                                 "Clair",
                                 "Claire",
                                 "Clara",
                                 "Clare",
                                 "Clarence",
                                 "Clarence",
                                 "Claretha",
                                 "Claretta",
                                 "Claribel",
                                 "Clarice",
                                 "Clarinda",
                                 "Clarine",
                                 "Claris",
                                 "Clarisa",
                                 "Clarissa",
                                 "Clarita",
                                 "Clark",
                                 "Classie",
                                 "Claud",
                                 "Claude",
                                 "Claude",
                                 "Claudette",
                                 "Claudia",
                                 "Claudie",
                                 "Claudine",
                                 "Claudio",
                                 "Clay",
                                 "Clayton",
                                 "Clelia",
                                 "Clemencia",
                                 "Clement",
                                 "Clemente",
                                 "Clementina",
                                 "Clementine",
                                 "Clemmie",
                                 "Cleo",
                                 "Cleo",
                                 "Cleopatra",
                                 "Cleora",
                                 "Cleotilde",
                                 "Cleta",
                                 "Cletus",
                                 "Cleveland",
                                 "Cliff",
                                 "Clifford",
                                 "Clifton",
                                 "Clint",
                                 "Clinton",
                                 "Clora",
                                 "Clorinda",
                                 "Clotilde",
                                 "Clyde",
                                 "Clyde",
                                 "Codi",
                                 "Cody",
                                 "Cody",
                                 "Colby",
                                 "Colby",
                                 "Cole",
                                 "Coleen",
                                 "Coleman",
                                 "Colene",
                                 "Coletta",
                                 "Colette",
                                 "Colin",
                                 "Colleen",
                                 "Collen",
                                 "Collene",
                                 "Collette",
                                 "Collin",
                                 "Colton",
                                 "Columbus",
                                 "Concepcion",
                                 "Conception",
                                 "Concetta",
                                 "Concha",
                                 "Conchita",
                                 "Connie",
                                 "Connie",
                                 "Conrad",
                                 "Constance",
                                 "Consuela",
                                 "Consuelo",
                                 "Contessa",
                                 "Cora",
                                 "Coral",
                                 "Coralee",
                                 "Coralie",
                                 "Corazon",
                                 "Cordelia",
                                 "Cordell",
                                 "Cordia",
                                 "Cordie",
                                 "Coreen",
                                 "Corene",
                                 "Coretta",
                                 "Corey",
                                 "Corey",
                                 "Cori",
                                 "Corie",
                                 "Corina",
                                 "Corine",
                                 "Corinna",
                                 "Corinne",
                                 "Corliss",
                                 "Cornelia",
                                 "Cornelius",
                                 "Cornell",
                                 "Corrie",
                                 "Corrin",
                                 "Corrina",
                                 "Corrine",
                                 "Corrinne",
                                 "Cortez",
                                 "Cortney",
                                 "Cory",
                                 "Cory",
                                 "Courtney",
                                 "Courtney",
                                 "Coy",
                                 "Craig",
                                 "Creola",
                                 "Cris",
                                 "Criselda",
                                 "Crissy",
                                 "Crista",
                                 "Cristal",
                                 "Cristen",
                                 "Cristi",
                                 "Cristie",
                                 "Cristin",
                                 "Cristina",
                                 "Cristine",
                                 "Cristobal",
                                 "Cristopher",
                                 "Cristy",
                                 "Cruz",
                                 "Cruz",
                                 "Crysta",
                                 "Crystal",
                                 "Crystle",
                                 "Cuc",
                                 "Curt",
                                 "Curtis",
                                 "Curtis",
                                 "Cyndi",
                                 "Cyndy",
                                 "Cynthia",
                                 "Cyril",
                                 "Cyrstal",
                                 "Cyrus",
                                 "Cythia",
                                 "Dacia",
                                 "Dagmar",
                                 "Dagny",
                                 "Dahlia",
                                 "Daina",
                                 "Daine",
                                 "Daisey",
                                 "Daisy",
                                 "Dakota",
                                 "Dale",
                                 "Dale",
                                 "Dalene",
                                 "Dalia",
                                 "Dalila",
                                 "Dallas",
                                 "Dallas",
                                 "Dalton",
                                 "Damaris",
                                 "Damian",
                                 "Damien",
                                 "Damion",
                                 "Damon",
                                 "Dan",
                                 "Dan",
                                 "Dana",
                                 "Dana",
                                 "Danae",
                                 "Dane",
                                 "Danelle",
                                 "Danette",
                                 "Dani",
                                 "Dania",
                                 "Danial",
                                 "Danica",
                                 "Daniel",
                                 "Daniel",
                                 "Daniela",
                                 "Daniele",
                                 "Daniell",
                                 "Daniella",
                                 "Danielle",
                                 "Danika",
                                 "Danille",
                                 "Danilo",
                                 "Danita",
                                 "Dann",
                                 "Danna",
                                 "Dannette",
                                 "Dannie",
                                 "Dannie",
                                 "Dannielle",
                                 "Danny",
                                 "Dante",
                                 "Danuta",
                                 "Danyel",
                                 "Danyell",
                                 "Danyelle",
                                 "Daphine",
                                 "Daphne",
                                 "Dara",
                                 "Darby",
                                 "Darcel",
                                 "Darcey",
                                 "Darci",
                                 "Darcie",
                                 "Darcy",
                                 "Darell",
                                 "Daren",
                                 "Daria",
                                 "Darin",
                                 "Dario",
                                 "Darius",
                                 "Darla",
                                 "Darleen",
                                 "Darlena",
                                 "Darlene",
                                 "Darline",
                                 "Darnell",
                                 "Darnell",
                                 "Daron",
                                 "Darrel",
                                 "Darrell",
                                 "Darren",
                                 "Darrick",
                                 "Darrin",
                                 "Darron",
                                 "Darryl",
                                 "Darwin",
                                 "Daryl",
                                 "Daryl",
                                 "Dave",
                                 "David",
                                 "David",
                                 "Davida",
                                 "Davina",
                                 "Davis",
                                 "Dawn",
                                 "Dawna",
                                 "Dawne",
                                 "Dayle",
                                 "Dayna",
                                 "Daysi",
                                 "Deadra",
                                 "Dean",
                                 "Dean",
                                 "Deana",
                                 "Deandra",
                                 "Deandre",
                                 "Deandrea",
                                 "Deane",
                                 "Deangelo",
                                 "Deann",
                                 "Deanna",
                                 "Deanne",
                                 "Deb",
                                 "Debbi",
                                 "Debbie",
                                 "Debbra",
                                 "Debby",
                                 "Debera",
                                 "Debi",
                                 "Debora",
                                 "Deborah",
                                 "Debra",
                                 "Debrah",
                                 "Debroah",
                                 "Dede",
                                 "Dedra",
                                 "Dee",
                                 "Dee",
                                 "Deeann",
                                 "Deeanna",
                                 "Deedee",
                                 "Deedra",
                                 "Deena",
                                 "Deetta",
                                 "Deidra",
                                 "Deidre",
                                 "Deirdre",
                                 "Deja",
                                 "Del",
                                 "Delaine",
                                 "Delana",
                                 "Delbert",
                                 "Delcie",
                                 "Delena",
                                 "Delfina",
                                 "Delia",
                                 "Delicia",
                                 "Delila",
                                 "Delilah",
                                 "Delinda",
                                 "Delisa",
                                 "Dell",
                                 "Della",
                                 "Delma",
                                 "Delmar",
                                 "Delmer",
                                 "Delmy",
                                 "Delois",
                                 "Deloise",
                                 "Delora",
                                 "Deloras",
                                 "Delores",
                                 "Deloris",
                                 "Delorse",
                                 "Delpha",
                                 "Delphia",
                                 "Delphine",
                                 "Delsie",
                                 "Delta",
                                 "Demarcus",
                                 "Demetra",
                                 "Demetria",
                                 "Demetrice",
                                 "Demetrius",
                                 "Demetrius",
                                 "Dena",
                                 "Denae",
                                 "Deneen",
                                 "Denese",
                                 "Denice",
                                 "Denis",
                                 "Denise",
                                 "Denisha",
                                 "Denisse",
                                 "Denita",
                                 "Denna",
                                 "Dennis",
                                 "Dennis",
                                 "Dennise",
                                 "Denny",
                                 "Denny",
                                 "Denver",
                                 "Denyse",
                                 "Deon",
                                 "Deon",
                                 "Deonna",
                                 "Derek",
                                 "Derick",
                                 "Derrick",
                                 "Deshawn",
                                 "Desirae",
                                 "Desire",
                                 "Desiree",
                                 "Desmond",
                                 "Despina",
                                 "Dessie",
                                 "Destiny",
                                 "Detra",
                                 "Devin",
                                 "Devin",
                                 "Devon",
                                 "Devon",
                                 "Devona",
                                 "Devora",
                                 "Devorah",
                                 "Dewayne",
                                 "Dewey",
                                 "Dewitt",
                                 "Dexter",
                                 "Dia",
                                 "Diamond",
                                 "Dian",
                                 "Diana",
                                 "Diane",
                                 "Diann",
                                 "Dianna",
                                 "Dianne",
                                 "Dick",
                                 "Diedra",
                                 "Diedre",
                                 "Diego",
                                 "Dierdre",
                                 "Digna",
                                 "Dillon",
                                 "Dimple",
                                 "Dina",
                                 "Dinah",
                                 "Dino",
                                 "Dinorah",
                                 "Dion",
                                 "Dion",
                                 "Dione",
                                 "Dionna",
                                 "Dionne",
                                 "Dirk",
                                 "Divina",
                                 "Dixie",
                                 "Dodie",
                                 "Dollie",
                                 "Dolly",
                                 "Dolores",
                                 "Doloris",
                                 "Domenic",
                                 "Domenica",
                                 "Dominga",
                                 "Domingo",
                                 "Dominic",
                                 "Dominica",
                                 "Dominick",
                                 "Dominique",
                                 "Dominique",
                                 "Dominque",
                                 "Domitila",
                                 "Domonique",
                                 "Don",
                                 "Dona",
                                 "Donald",
                                 "Donald",
                                 "Donella",
                                 "Donetta",
                                 "Donette",
                                 "Dong",
                                 "Dong",
                                 "Donita",
                                 "Donn",
                                 "Donna",
                                 "Donnell",
                                 "Donnetta",
                                 "Donnette",
                                 "Donnie",
                                 "Donnie",
                                 "Donny",
                                 "Donovan",
                                 "Donte",
                                 "Donya",
                                 "Dora",
                                 "Dorathy",
                                 "Dorcas",
                                 "Doreatha",
                                 "Doreen",
                                 "Dorene",
                                 "Doretha",
                                 "Dorethea",
                                 "Doretta",
                                 "Dori",
                                 "Doria",
                                 "Dorian",
                                 "Dorian",
                                 "Dorie",
                                 "Dorinda",
                                 "Dorine",
                                 "Doris",
                                 "Dorla",
                                 "Dorotha",
                                 "Dorothea",
                                 "Dorothy",
                                 "Dorris",
                                 "Dorsey",
                                 "Dortha",
                                 "Dorthea",
                                 "Dorthey",
                                 "Dorthy",
                                 "Dot",
                                 "Dottie",
                                 "Dotty",
                                 "Doug",
                                 "Douglas",
                                 "Douglass",
                                 "Dovie",
                                 "Doyle",
                                 "Dreama",
                                 "Drema",
                                 "Drew",
                                 "Drew",
                                 "Drucilla",
                                 "Drusilla",
                                 "Duane",
                                 "Dudley",
                                 "Dulce",
                                 "Dulcie",
                                 "Duncan",
                                 "Dung",
                                 "Dusti",
                                 "Dustin",
                                 "Dusty",
                                 "Dusty",
                                 "Dwain",
                                 "Dwana",
                                 "Dwayne",
                                 "Dwight",
                                 "Dyan",
                                 "Dylan",
                                 "Earl",
                                 "Earle",
                                 "Earlean",
                                 "Earleen",
                                 "Earlene",
                                 "Earlie",
                                 "Earline",
                                 "Earnest",
                                 "Earnestine",
                                 "Eartha",
                                 "Easter",
                                 "Eboni",
                                 "Ebonie",
                                 "Ebony",
                                 "Echo",
                                 "Ed",
                                 "Eda",
                                 "Edda",
                                 "Eddie",
                                 "Eddie",
                                 "Eddy",
                                 "Edelmira",
                                 "Eden",
                                 "Edgar",
                                 "Edgardo",
                                 "Edie",
                                 "Edison",
                                 "Edith",
                                 "Edmond",
                                 "Edmund",
                                 "Edmundo",
                                 "Edna",
                                 "Edra",
                                 "Edris",
                                 "Eduardo",
                                 "Edward",
                                 "Edward",
                                 "Edwardo",
                                 "Edwin",
                                 "Edwina",
                                 "Edyth",
                                 "Edythe",
                                 "Effie",
                                 "Efrain",
                                 "Efren",
                                 "Ehtel",
                                 "Eileen",
                                 "Eilene",
                                 "Ela",
                                 "Eladia",
                                 "Elaina",
                                 "Elaine",
                                 "Elana",
                                 "Elane",
                                 "Elanor",
                                 "Elayne",
                                 "Elba",
                                 "Elbert",
                                 "Elda",
                                 "Elden",
                                 "Eldon",
                                 "Eldora",
                                 "Eldridge",
                                 "Eleanor",
                                 "Eleanora",
                                 "Eleanore",
                                 "Elease",
                                 "Elena",
                                 "Elene",
                                 "Eleni",
                                 "Elenor",
                                 "Elenora",
                                 "Elenore",
                                 "Eleonor",
                                 "Eleonora",
                                 "Eleonore",
                                 "Elfreda",
                                 "Elfrieda",
                                 "Elfriede",
                                 "Eli",
                                 "Elia",
                                 "Eliana",
                                 "Elias",
                                 "Elicia",
                                 "Elida",
                                 "Elidia",
                                 "Elijah",
                                 "Elin",
                                 "Elina",
                                 "Elinor",
                                 "Elinore",
                                 "Elisa",
                                 "Elisabeth",
                                 "Elise",
                                 "Eliseo",
                                 "Elisha",
                                 "Elisha",
                                 "Elissa",
                                 "Eliz",
                                 "Eliza",
                                 "Elizabet",
                                 "Elizabeth",
                                 "Elizbeth",
                                 "Elizebeth",
                                 "Elke",
                                 "Ella",
                                 "Ellamae",
                                 "Ellan",
                                 "Ellen",
                                 "Ellena",
                                 "Elli",
                                 "Ellie",
                                 "Elliot",
                                 "Elliott",
                                 "Ellis",
                                 "Ellis",
                                 "Ellsworth",
                                 "Elly",
                                 "Ellyn",
                                 "Elma",
                                 "Elmer",
                                 "Elmer",
                                 "Elmira",
                                 "Elmo",
                                 "Elna",
                                 "Elnora",
                                 "Elodia",
                                 "Elois",
                                 "Eloisa",
                                 "Eloise",
                                 "Elouise",
                                 "Eloy",
                                 "Elroy",
                                 "Elsa",
                                 "Else",
                                 "Elsie",
                                 "Elsy",
                                 "Elton",
                                 "Elva",
                                 "Elvera",
                                 "Elvia",
                                 "Elvie",
                                 "Elvin",
                                 "Elvina",
                                 "Elvira",
                                 "Elvis",
                                 "Elwanda",
                                 "Elwood",
                                 "Elyse",
                                 "Elza",
                                 "Ema",
                                 "Emanuel",
                                 "Emelda",
                                 "Emelia",
                                 "Emelina",
                                 "Emeline",
                                 "Emely",
                                 "Emerald",
                                 "Emerita",
                                 "Emerson",
                                 "Emery",
                                 "Emiko",
                                 "Emil",
                                 "Emile",
                                 "Emilee",
                                 "Emilia",
                                 "Emilie",
                                 "Emilio",
                                 "Emily",
                                 "Emma",
                                 "Emmaline",
                                 "Emmanuel",
                                 "Emmett",
                                 "Emmie",
                                 "Emmitt",
                                 "Emmy",
                                 "Emogene",
                                 "Emory",
                                 "Ena",
                                 "Enda",
                                 "Enedina",
                                 "Eneida",
                                 "Enid",
                                 "Enoch",
                                 "Enola",
                                 "Enrique",
                                 "Enriqueta",
                                 "Epifania",
                                 "Era",
                                 "Erasmo",
                                 "Eric",
                                 "Eric",
                                 "Erica",
                                 "Erich",
                                 "Erick",
                                 "Ericka",
                                 "Erik",
                                 "Erika",
                                 "Erin",
                                 "Erin",
                                 "Erinn",
                                 "Erlene",
                                 "Erlinda",
                                 "Erline",
                                 "Erma",
                                 "Ermelinda",
                                 "Erminia",
                                 "Erna",
                                 "Ernest",
                                 "Ernestina",
                                 "Ernestine",
                                 "Ernesto",
                                 "Ernie",
                                 "Errol",
                                 "Ervin",
                                 "Erwin",
                                 "Eryn",
                                 "Esmeralda",
                                 "Esperanza",
                                 "Essie",
                                 "Esta",
                                 "Esteban",
                                 "Estefana",
                                 "Estela",
                                 "Estell",
                                 "Estella",
                                 "Estelle",
                                 "Ester",
                                 "Esther",
                                 "Estrella",
                                 "Etha",
                                 "Ethan",
                                 "Ethel",
                                 "Ethelene",
                                 "Ethelyn",
                                 "Ethyl",
                                 "Etsuko",
                                 "Etta",
                                 "Ettie",
                                 "Eufemia",
                                 "Eugena",
                                 "Eugene",
                                 "Eugene",
                                 "Eugenia",
                                 "Eugenie",
                                 "Eugenio",
                                 "Eula",
                                 "Eulah",
                                 "Eulalia",
                                 "Eun",
                                 "Euna",
                                 "Eunice",
                                 "Eura",
                                 "Eusebia",
                                 "Eusebio",
                                 "Eustolia",
                                 "Eva",
                                 "Evalyn",
                                 "Evan",
                                 "Evan",
                                 "Evangelina",
                                 "Evangeline",
                                 "Eve",
                                 "Evelia",
                                 "Evelin",
                                 "Evelina",
                                 "Eveline",
                                 "Evelyn",
                                 "Evelyne",
                                 "Evelynn",
                                 "Everett",
                                 "Everette",
                                 "Evette",
                                 "Evia",
                                 "Evie",
                                 "Evita",
                                 "Evon",
                                 "Evonne",
                                 "Ewa",
                                 "Exie",
                                 "Ezekiel",
                                 "Ezequiel",
                                 "Ezra",
                                 "Fabian",
                                 "Fabiola",
                                 "Fae",
                                 "Fairy",
                                 "Faith",
                                 "Fallon",
                                 "Fannie",
                                 "Fanny",
                                 "Farah",
                                 "Farrah",
                                 "Fatima",
                                 "Fatimah",
                                 "Faustina",
                                 "Faustino",
                                 "Fausto",
                                 "Faviola",
                                 "Fawn",
                                 "Fay",
                                 "Faye",
                                 "Fe",
                                 "Federico",
                                 "Felecia",
                                 "Felica",
                                 "Felice",
                                 "Felicia",
                                 "Felicidad",
                                 "Felicita",
                                 "Felicitas",
                                 "Felipa",
                                 "Felipe",
                                 "Felisa",
                                 "Felisha",
                                 "Felix",
                                 "Felton",
                                 "Ferdinand",
                                 "Fermin",
                                 "Fermina",
                                 "Fern",
                                 "Fernanda",
                                 "Fernande",
                                 "Fernando",
                                 "Ferne",
                                 "Fidel",
                                 "Fidela",
                                 "Fidelia",
                                 "Filiberto",
                                 "Filomena",
                                 "Fiona",
                                 "Flavia",
                                 "Fleta",
                                 "Fletcher",
                                 "Flo",
                                 "Flor",
                                 "Flora",
                                 "Florance",
                                 "Florence",
                                 "Florencia",
                                 "Florencio",
                                 "Florene",
                                 "Florentina",
                                 "Florentino",
                                 "Floretta",
                                 "Floria",
                                 "Florida",
                                 "Florinda",
                                 "Florine",
                                 "Florrie",
                                 "Flossie",
                                 "Floy",
                                 "Floyd",
                                 "Fonda",
                                 "Forest",
                                 "Forrest",
                                 "Foster",
                                 "Fran",
                                 "France",
                                 "Francene",
                                 "Frances",
                                 "Frances",
                                 "Francesca",
                                 "Francesco",
                                 "Franchesca",
                                 "Francie",
                                 "Francina",
                                 "Francine",
                                 "Francis",
                                 "Francis",
                                 "Francisca",
                                 "Francisco",
                                 "Francisco",
                                 "Francoise",
                                 "Frank",
                                 "Frank",
                                 "Frankie",
                                 "Frankie",
                                 "Franklin",
                                 "Franklyn",
                                 "Fransisca",
                                 "Fred",
                                 "Fred",
                                 "Freda",
                                 "Fredda",
                                 "Freddie",
                                 "Freddie",
                                 "Freddy",
                                 "Frederic",
                                 "Frederica",
                                 "Frederick",
                                 "Fredericka",
                                 "Fredia",
                                 "Fredric",
                                 "Fredrick",
                                 "Fredricka",
                                 "Freeda",
                                 "Freeman",
                                 "Freida",
                                 "Frida",
                                 "Frieda",
                                 "Fritz",
                                 "Fumiko",
                                 "Gabriel",
                                 "Gabriel",
                                 "Gabriela",
                                 "Gabriele",
                                 "Gabriella",
                                 "Gabrielle",
                                 "Gail",
                                 "Gail",
                                 "Gala",
                                 "Gale",
                                 "Gale",
                                 "Galen",
                                 "Galina",
                                 "Garfield",
                                 "Garland",
                                 "Garnet",
                                 "Garnett",
                                 "Garret",
                                 "Garrett",
                                 "Garry",
                                 "Garth",
                                 "Gary",
                                 "Gary",
                                 "Gaston",
                                 "Gavin",
                                 "Gay",
                                 "Gaye",
                                 "Gayla",
                                 "Gayle",
                                 "Gayle",
                                 "Gaylene",
                                 "Gaylord",
                                 "Gaynell",
                                 "Gaynelle",
                                 "Gearldine",
                                 "Gema",
                                 "Gemma",
                                 "Gena",
                                 "Genaro",
                                 "Gene",
                                 "Gene",
                                 "Genesis",
                                 "Geneva",
                                 "Genevie",
                                 "Genevieve",
                                 "Genevive",
                                 "Genia",
                                 "Genie",
                                 "Genna",
                                 "Gennie",
                                 "Genny",
                                 "Genoveva",
                                 "Geoffrey",
                                 "Georgann",
                                 "George",
                                 "George",
                                 "Georgeann",
                                 "Georgeanna",
                                 "Georgene",
                                 "Georgetta",
                                 "Georgette",
                                 "Georgia",
                                 "Georgiana",
                                 "Georgiann",
                                 "Georgianna",
                                 "Georgianne",
                                 "Georgie",
                                 "Georgina",
                                 "Georgine",
                                 "Gerald",
                                 "Gerald",
                                 "Geraldine",
                                 "Geraldo",
                                 "Geralyn",
                                 "Gerard",
                                 "Gerardo",
                                 "Gerda",
                                 "Geri",
                                 "Germaine",
                                 "German",
                                 "Gerri",
                                 "Gerry",
                                 "Gerry",
                                 "Gertha",
                                 "Gertie",
                                 "Gertrud",
                                 "Gertrude",
                                 "Gertrudis",
                                 "Gertude",
                                 "Ghislaine",
                                 "Gia",
                                 "Gianna",
                                 "Gidget",
                                 "Gigi",
                                 "Gil",
                                 "Gilbert",
                                 "Gilberte",
                                 "Gilberto",
                                 "Gilda",
                                 "Gillian",
                                 "Gilma",
                                 "Gina",
                                 "Ginette",
                                 "Ginger",
                                 "Ginny",
                                 "Gino",
                                 "Giovanna",
                                 "Giovanni",
                                 "Gisela",
                                 "Gisele",
                                 "Giselle",
                                 "Gita",
                                 "Giuseppe",
                                 "Giuseppina",
                                 "Gladis",
                                 "Glady",
                                 "Gladys",
                                 "Glayds",
                                 "Glen",
                                 "Glenda",
                                 "Glendora",
                                 "Glenn",
                                 "Glenn",
                                 "Glenna",
                                 "Glennie",
                                 "Glennis",
                                 "Glinda",
                                 "Gloria",
                                 "Glory",
                                 "Glynda",
                                 "Glynis",
                                 "Golda",
                                 "Golden",
                                 "Goldie",
                                 "Gonzalo",
                                 "Gordon",
                                 "Grace",
                                 "Gracia",
                                 "Gracie",
                                 "Graciela",
                                 "Grady",
                                 "Graham",
                                 "Graig",
                                 "Grant",
                                 "Granville",
                                 "Grayce",
                                 "Grazyna",
                                 "Greg",
                                 "Gregg",
                                 "Gregoria",
                                 "Gregorio",
                                 "Gregory",
                                 "Gregory",
                                 "Greta",
                                 "Gretchen",
                                 "Gretta",
                                 "Gricelda",
                                 "Grisel",
                                 "Griselda",
                                 "Grover",
                                 "Guadalupe",
                                 "Guadalupe",
                                 "Gudrun",
                                 "Guillermina",
                                 "Guillermo",
                                 "Gus",
                                 "Gussie",
                                 "Gustavo",
                                 "Guy",
                                 "Gwen",
                                 "Gwenda",
                                 "Gwendolyn",
                                 "Gwenn",
                                 "Gwyn",
                                 "Gwyneth",
                                 "Ha",
                                 "Hae",
                                 "Hai",
                                 "Hailey",
                                 "Hal",
                                 "Haley",
                                 "Halina",
                                 "Halley",
                                 "Hallie",
                                 "Han",
                                 "Hana",
                                 "Hang",
                                 "Hanh",
                                 "Hank",
                                 "Hanna",
                                 "Hannah",
                                 "Hannelore",
                                 "Hans",
                                 "Harlan",
                                 "Harland",
                                 "Harley",
                                 "Harmony",
                                 "Harold",
                                 "Harold",
                                 "Harriet",
                                 "Harriett",
                                 "Harriette",
                                 "Harris",
                                 "Harrison",
                                 "Harry",
                                 "Harvey",
                                 "Hassan",
                                 "Hassie",
                                 "Hattie",
                                 "Haydee",
                                 "Hayden",
                                 "Hayley",
                                 "Haywood",
                                 "Hazel",
                                 "Heath",
                                 "Heather",
                                 "Hector",
                                 "Hedwig",
                                 "Hedy",
                                 "Hee",
                                 "Heide",
                                 "Heidi",
                                 "Heidy",
                                 "Heike",
                                 "Helaine",
                                 "Helen",
                                 "Helena",
                                 "Helene",
                                 "Helga",
                                 "Hellen",
                                 "Henrietta",
                                 "Henriette",
                                 "Henry",
                                 "Henry",
                                 "Herb",
                                 "Herbert",
                                 "Heriberto",
                                 "Herlinda",
                                 "Herma",
                                 "Herman",
                                 "Hermelinda",
                                 "Hermila",
                                 "Hermina",
                                 "Hermine",
                                 "Herminia",
                                 "Herschel",
                                 "Hershel",
                                 "Herta",
                                 "Hertha",
                                 "Hester",
                                 "Hettie",
                                 "Hiedi",
                                 "Hien",
                                 "Hilaria",
                                 "Hilario",
                                 "Hilary",
                                 "Hilda",
                                 "Hilde",
                                 "Hildegard",
                                 "Hildegarde",
                                 "Hildred",
                                 "Hillary",
                                 "Hilma",
                                 "Hilton",
                                 "Hipolito",
                                 "Hiram",
                                 "Hiroko",
                                 "Hisako",
                                 "Hoa",
                                 "Hobert",
                                 "Holley",
                                 "Holli",
                                 "Hollie",
                                 "Hollis",
                                 "Hollis",
                                 "Holly",
                                 "Homer",
                                 "Honey",
                                 "Hong",
                                 "Hong",
                                 "Hope",
                                 "Horace",
                                 "Horacio",
                                 "Hortencia",
                                 "Hortense",
                                 "Hortensia",
                                 "Hosea",
                                 "Houston",
                                 "Howard",
                                 "Hoyt",
                                 "Hsiu",
                                 "Hubert",
                                 "Hue",
                                 "Huey",
                                 "Hugh",
                                 "Hugo",
                                 "Hui",
                                 "Hulda",
                                 "Humberto",
                                 "Hung",
                                 "Hunter",
                                 "Huong",
                                 "Hwa",
                                 "Hyacinth",
                                 "Hye",
                                 "Hyman",
                                 "Hyo",
                                 "Hyon",
                                 "Hyun",
                                 "Ian",
                                 "Ida",
                                 "Idalia",
                                 "Idell",
                                 "Idella",
                                 "Iesha",
                                 "Ignacia",
                                 "Ignacio",
                                 "Ike",
                                 "Ila",
                                 "Ilana",
                                 "Ilda",
                                 "Ileana",
                                 "Ileen",
                                 "Ilene",
                                 "Iliana",
                                 "Illa",
                                 "Ilona",
                                 "Ilse",
                                 "Iluminada",
                                 "Ima",
                                 "Imelda",
                                 "Imogene",
                                 "In",
                                 "Ina",
                                 "India",
                                 "Indira",
                                 "Inell",
                                 "Ines",
                                 "Inez",
                                 "Inga",
                                 "Inge",
                                 "Ingeborg",
                                 "Inger",
                                 "Ingrid",
                                 "Inocencia",
                                 "Iola",
                                 "Iona",
                                 "Ione",
                                 "Ira",
                                 "Ira",
                                 "Iraida",
                                 "Irena",
                                 "Irene",
                                 "Irina",
                                 "Iris",
                                 "Irish",
                                 "Irma",
                                 "Irmgard",
                                 "Irvin",
                                 "Irving",
                                 "Irwin",
                                 "Isa",
                                 "Isaac",
                                 "Isabel",
                                 "Isabell",
                                 "Isabella",
                                 "Isabelle",
                                 "Isadora",
                                 "Isaiah",
                                 "Isaias",
                                 "Isaura",
                                 "Isela",
                                 "Isiah",
                                 "Isidra",
                                 "Isidro",
                                 "Isis",
                                 "Ismael",
                                 "Isobel",
                                 "Israel",
                                 "Isreal",
                                 "Issac",
                                 "Iva",
                                 "Ivan",
                                 "Ivana",
                                 "Ivelisse",
                                 "Ivette",
                                 "Ivey",
                                 "Ivonne",
                                 "Ivory",
                                 "Ivory",
                                 "Ivy",
                                 "Izetta",
                                 "Izola",
                                 "Ja",
                                 "Jacalyn",
                                 "Jacelyn",
                                 "Jacinda",
                                 "Jacinta",
                                 "Jacinto",
                                 "Jack",
                                 "Jack",
                                 "Jackeline",
                                 "Jackelyn",
                                 "Jacki",
                                 "Jackie",
                                 "Jackie",
                                 "Jacklyn",
                                 "Jackqueline",
                                 "Jackson",
                                 "Jaclyn",
                                 "Jacob",
                                 "Jacqualine",
                                 "Jacque",
                                 "Jacquelin",
                                 "Jacqueline",
                                 "Jacquelyn",
                                 "Jacquelyne",
                                 "Jacquelynn",
                                 "Jacques",
                                 "Jacquetta",
                                 "Jacqui",
                                 "Jacquie",
                                 "Jacquiline",
                                 "Jacquline",
                                 "Jacqulyn",
                                 "Jada",
                                 "Jade",
                                 "Jadwiga",
                                 "Jae",
                                 "Jae",
                                 "Jaime",
                                 "Jaime",
                                 "Jaimee",
                                 "Jaimie",
                                 "Jake",
                                 "Jaleesa",
                                 "Jalisa",
                                 "Jama",
                                 "Jamaal",
                                 "Jamal",
                                 "Jamar",
                                 "Jame",
                                 "Jame",
                                 "Jamee",
                                 "Jamel",
                                 "James",
                                 "James",
                                 "Jamey",
                                 "Jamey",
                                 "Jami",
                                 "Jamie",
                                 "Jamie",
                                 "Jamika",
                                 "Jamila",
                                 "Jamison",
                                 "Jammie",
                                 "Jan",
                                 "Jan",
                                 "Jana",
                                 "Janae",
                                 "Janay",
                                 "Jane",
                                 "Janean",
                                 "Janee",
                                 "Janeen",
                                 "Janel",
                                 "Janell",
                                 "Janella",
                                 "Janelle",
                                 "Janene",
                                 "Janessa",
                                 "Janet",
                                 "Janeth",
                                 "Janett",
                                 "Janetta",
                                 "Janette",
                                 "Janey",
                                 "Jani",
                                 "Janice",
                                 "Janie",
                                 "Janiece",
                                 "Janina",
                                 "Janine",
                                 "Janis",
                                 "Janise",
                                 "Janita",
                                 "Jann",
                                 "Janna",
                                 "Jannet",
                                 "Jannette",
                                 "Jannie",
                                 "January",
                                 "Janyce",
                                 "Jaqueline",
                                 "Jaquelyn",
                                 "Jared",
                                 "Jarod",
                                 "Jarred",
                                 "Jarrett",
                                 "Jarrod",
                                 "Jarvis",
                                 "Jasmin",
                                 "Jasmine",
                                 "Jason",
                                 "Jason",
                                 "Jasper",
                                 "Jaunita",
                                 "Javier",
                                 "Jay",
                                 "Jay",
                                 "Jaye",
                                 "Jayme",
                                 "Jaymie",
                                 "Jayna",
                                 "Jayne",
                                 "Jayson",
                                 "Jazmin",
                                 "Jazmine",
                                 "Jc",
                                 "Jean",
                                 "Jean",
                                 "Jeana",
                                 "Jeane",
                                 "Jeanelle",
                                 "Jeanene",
                                 "Jeanett",
                                 "Jeanetta",
                                 "Jeanette",
                                 "Jeanice",
                                 "Jeanie",
                                 "Jeanine",
                                 "Jeanmarie",
                                 "Jeanna",
                                 "Jeanne",
                                 "Jeannetta",
                                 "Jeannette",
                                 "Jeannie",
                                 "Jeannine",
                                 "Jed",
                                 "Jeff",
                                 "Jefferey",
                                 "Jefferson",
                                 "Jeffery",
                                 "Jeffie",
                                 "Jeffrey",
                                 "Jeffrey",
                                 "Jeffry",
                                 "Jen",
                                 "Jena",
                                 "Jenae",
                                 "Jene",
                                 "Jenee",
                                 "Jenell",
                                 "Jenelle",
                                 "Jenette",
                                 "Jeneva",
                                 "Jeni",
                                 "Jenice",
                                 "Jenifer",
                                 "Jeniffer",
                                 "Jenine",
                                 "Jenise",
                                 "Jenna",
                                 "Jennefer",
                                 "Jennell",
                                 "Jennette",
                                 "Jenni",
                                 "Jennie",
                                 "Jennifer",
                                 "Jenniffer",
                                 "Jennine",
                                 "Jenny",
                                 "Jerald",
                                 "Jeraldine",
                                 "Jeramy",
                                 "Jere",
                                 "Jeremiah",
                                 "Jeremy",
                                 "Jeremy",
                                 "Jeri",
                                 "Jerica",
                                 "Jerilyn",
                                 "Jerlene",
                                 "Jermaine",
                                 "Jerold",
                                 "Jerome",
                                 "Jeromy",
                                 "Jerrell",
                                 "Jerri",
                                 "Jerrica",
                                 "Jerrie",
                                 "Jerrod",
                                 "Jerrold",
                                 "Jerry",
                                 "Jerry",
                                 "Jesenia",
                                 "Jesica",
                                 "Jess",
                                 "Jesse",
                                 "Jesse",
                                 "Jessenia",
                                 "Jessi",
                                 "Jessia",
                                 "Jessica",
                                 "Jessie",
                                 "Jessie",
                                 "Jessika",
                                 "Jestine",
                                 "Jesus",
                                 "Jesus",
                                 "Jesusa",
                                 "Jesusita",
                                 "Jetta",
                                 "Jettie",
                                 "Jewel",
                                 "Jewel",
                                 "Jewell",
                                 "Jewell",
                                 "Ji",
                                 "Jill",
                                 "Jillian",
                                 "Jim",
                                 "Jimmie",
                                 "Jimmie",
                                 "Jimmy",
                                 "Jimmy",
                                 "Jin",
                                 "Jina",
                                 "Jinny",
                                 "Jo",
                                 "Joan",
                                 "Joan",
                                 "Joana",
                                 "Joane",
                                 "Joanie",
                                 "Joann",
                                 "Joanna",
                                 "Joanne",
                                 "Joannie",
                                 "Joaquin",
                                 "Joaquina",
                                 "Jocelyn",
                                 "Jodee",
                                 "Jodi",
                                 "Jodie",
                                 "Jody",
                                 "Jody",
                                 "Joe",
                                 "Joe",
                                 "Joeann",
                                 "Joel",
                                 "Joel",
                                 "Joella",
                                 "Joelle",
                                 "Joellen",
                                 "Joesph",
                                 "Joetta",
                                 "Joette",
                                 "Joey",
                                 "Joey",
                                 "Johana",
                                 "Johanna",
                                 "Johanne",
                                 "John",
                                 "John",
                                 "Johna",
                                 "Johnathan",
                                 "Johnathon",
                                 "Johnetta",
                                 "Johnette",
                                 "Johnie",
                                 "Johnie",
                                 "Johnna",
                                 "Johnnie",
                                 "Johnnie",
                                 "Johnny",
                                 "Johnny",
                                 "Johnsie",
                                 "Johnson",
                                 "Joi",
                                 "Joie",
                                 "Jolanda",
                                 "Joleen",
                                 "Jolene",
                                 "Jolie",
                                 "Joline",
                                 "Jolyn",
                                 "Jolynn",
                                 "Jon",
                                 "Jon",
                                 "Jona",
                                 "Jonah",
                                 "Jonas",
                                 "Jonathan",
                                 "Jonathon",
                                 "Jone",
                                 "Jonell",
                                 "Jonelle",
                                 "Jong",
                                 "Joni",
                                 "Jonie",
                                 "Jonna",
                                 "Jonnie",
                                 "Jordan",
                                 "Jordan",
                                 "Jordon",
                                 "Jorge",
                                 "Jose",
                                 "Jose",
                                 "Josef",
                                 "Josefa",
                                 "Josefina",
                                 "Josefine",
                                 "Joselyn",
                                 "Joseph",
                                 "Joseph",
                                 "Josephina",
                                 "Josephine",
                                 "Josette",
                                 "Josh",
                                 "Joshua",
                                 "Joshua",
                                 "Josiah",
                                 "Josie",
                                 "Joslyn",
                                 "Jospeh",
                                 "Josphine",
                                 "Josue",
                                 "Jovan",
                                 "Jovita",
                                 "Joy",
                                 "Joya",
                                 "Joyce",
                                 "Joycelyn",
                                 "Joye",
                                 "Juan",
                                 "Juan",
                                 "Juana",
                                 "Juanita",
                                 "Jude",
                                 "Jude",
                                 "Judi",
                                 "Judie",
                                 "Judith",
                                 "Judson",
                                 "Judy",
                                 "Jule",
                                 "Julee",
                                 "Julene",
                                 "Jules",
                                 "Juli",
                                 "Julia",
                                 "Julian",
                                 "Julian",
                                 "Juliana",
                                 "Juliane",
                                 "Juliann",
                                 "Julianna",
                                 "Julianne",
                                 "Julie",
                                 "Julieann",
                                 "Julienne",
                                 "Juliet",
                                 "Julieta",
                                 "Julietta",
                                 "Juliette",
                                 "Julio",
                                 "Julio",
                                 "Julissa",
                                 "Julius",
                                 "June",
                                 "Jung",
                                 "Junie",
                                 "Junior",
                                 "Junita",
                                 "Junko",
                                 "Justa",
                                 "Justin",
                                 "Justin",
                                 "Justina",
                                 "Justine",
                                 "Jutta",
                                 "Ka",
                                 "Kacey",
                                 "Kaci",
                                 "Kacie",
                                 "Kacy",
                                 "Kai",
                                 "Kaila",
                                 "Kaitlin",
                                 "Kaitlyn",
                                 "Kala",
                                 "Kaleigh",
                                 "Kaley",
                                 "Kali",
                                 "Kallie",
                                 "Kalyn",
                                 "Kam",
                                 "Kamala",
                                 "Kami",
                                 "Kamilah",
                                 "Kandace",
                                 "Kandi",
                                 "Kandice",
                                 "Kandis",
                                 "Kandra",
                                 "Kandy",
                                 "Kanesha",
                                 "Kanisha",
                                 "Kara",
                                 "Karan",
                                 "Kareem",
                                 "Kareen",
                                 "Karen",
                                 "Karena",
                                 "Karey",
                                 "Kari",
                                 "Karie",
                                 "Karima",
                                 "Karin",
                                 "Karina",
                                 "Karine",
                                 "Karisa",
                                 "Karissa",
                                 "Karl",
                                 "Karl",
                                 "Karla",
                                 "Karleen",
                                 "Karlene",
                                 "Karly",
                                 "Karlyn",
                                 "Karma",
                                 "Karmen",
                                 "Karol",
                                 "Karole",
                                 "Karoline",
                                 "Karolyn",
                                 "Karon",
                                 "Karren",
                                 "Karri",
                                 "Karrie",
                                 "Karry",
                                 "Kary",
                                 "Karyl",
                                 "Karyn",
                                 "Kasandra",
                                 "Kasey",
                                 "Kasey",
                                 "Kasha",
                                 "Kasi",
                                 "Kasie",
                                 "Kassandra",
                                 "Kassie",
                                 "Kate",
                                 "Katelin",
                                 "Katelyn",
                                 "Katelynn",
                                 "Katerine",
                                 "Kathaleen",
                                 "Katharina",
                                 "Katharine",
                                 "Katharyn",
                                 "Kathe",
                                 "Katheleen",
                                 "Katherin",
                                 "Katherina",
                                 "Katherine",
                                 "Kathern",
                                 "Katheryn",
                                 "Kathey",
                                 "Kathi",
                                 "Kathie",
                                 "Kathleen",
                                 "Kathlene",
                                 "Kathline",
                                 "Kathlyn",
                                 "Kathrin",
                                 "Kathrine",
                                 "Kathryn",
                                 "Kathryne",
                                 "Kathy",
                                 "Kathyrn",
                                 "Kati",
                                 "Katia",
                                 "Katie",
                                 "Katina",
                                 "Katlyn",
                                 "Katrice",
                                 "Katrina",
                                 "Kattie",
                                 "Katy",
                                 "Kay",
                                 "Kayce",
                                 "Kaycee",
                                 "Kaye",
                                 "Kayla",
                                 "Kaylee",
                                 "Kayleen",
                                 "Kayleigh",
                                 "Kaylene",
                                 "Kazuko",
                                 "Kecia",
                                 "Keeley",
                                 "Keely",
                                 "Keena",
                                 "Keenan",
                                 "Keesha",
                                 "Keiko",
                                 "Keila",
                                 "Keira",
                                 "Keisha",
                                 "Keith",
                                 "Keith",
                                 "Keitha",
                                 "Keli",
                                 "Kelle",
                                 "Kellee",
                                 "Kelley",
                                 "Kelley",
                                 "Kelli",
                                 "Kellie",
                                 "Kelly",
                                 "Kelly",
                                 "Kellye",
                                 "Kelsey",
                                 "Kelsi",
                                 "Kelsie",
                                 "Kelvin",
                                 "Kemberly",
                                 "Ken",
                                 "Kena",
                                 "Kenda",
                                 "Kendal",
                                 "Kendall",
                                 "Kendall",
                                 "Kendra",
                                 "Kendrick",
                                 "Keneth",
                                 "Kenia",
                                 "Kenisha",
                                 "Kenna",
                                 "Kenneth",
                                 "Kenneth",
                                 "Kennith",
                                 "Kenny",
                                 "Kent",
                                 "Kenton",
                                 "Kenya",
                                 "Kenyatta",
                                 "Kenyetta",
                                 "Kera",
                                 "Keren",
                                 "Keri",
                                 "Kermit",
                                 "Kerri",
                                 "Kerrie",
                                 "Kerry",
                                 "Kerry",
                                 "Kerstin",
                                 "Kesha",
                                 "Keshia",
                                 "Keturah",
                                 "Keva",
                                 "Keven",
                                 "Kevin",
                                 "Kevin",
                                 "Khadijah",
                                 "Khalilah",
                                 "Kia",
                                 "Kiana",
                                 "Kiara",
                                 "Kiera",
                                 "Kiersten",
                                 "Kiesha",
                                 "Kieth",
                                 "Kiley",
                                 "Kim",
                                 "Kim",
                                 "Kimber",
                                 "Kimberely",
                                 "Kimberlee",
                                 "Kimberley",
                                 "Kimberli",
                                 "Kimberlie",
                                 "Kimberly",
                                 "Kimbery",
                                 "Kimbra",
                                 "Kimi",
                                 "Kimiko",
                                 "Kina",
                                 "Kindra",
                                 "King",
                                 "Kip",
                                 "Kira",
                                 "Kirby",
                                 "Kirby",
                                 "Kirk",
                                 "Kirsten",
                                 "Kirstie",
                                 "Kirstin",
                                 "Kisha",
                                 "Kit",
                                 "Kittie",
                                 "Kitty",
                                 "Kiyoko",
                                 "Kizzie",
                                 "Kizzy",
                                 "Klara",
                                 "Korey",
                                 "Kori",
                                 "Kortney",
                                 "Kory",
                                 "Kourtney",
                                 "Kraig",
                                 "Kris",
                                 "Kris",
                                 "Krishna",
                                 "Krissy",
                                 "Krista",
                                 "Kristal",
                                 "Kristan",
                                 "Kristeen",
                                 "Kristel",
                                 "Kristen",
                                 "Kristi",
                                 "Kristian",
                                 "Kristie",
                                 "Kristin",
                                 "Kristina",
                                 "Kristine",
                                 "Kristle",
                                 "Kristofer",
                                 "Kristopher",
                                 "Kristy",
                                 "Kristyn",
                                 "Krysta",
                                 "Krystal",
                                 "Krysten",
                                 "Krystin",
                                 "Krystina",
                                 "Krystle",
                                 "Krystyna",
                                 "Kum",
                                 "Kurt",
                                 "Kurtis",
                                 "Kyla",
                                 "Kyle",
                                 "Kyle",
                                 "Kylee",
                                 "Kylie",
                                 "Kym",
                                 "Kymberly",
                                 "Kyoko",
                                 "Kyong",
                                 "Kyra",
                                 "Kyung",
                                 "Lacey",
                                 "Lachelle",
                                 "Laci",
                                 "Lacie",
                                 "Lacresha",
                                 "Lacy",
                                 "Lacy",
                                 "Ladawn",
                                 "Ladonna",
                                 "Lady",
                                 "Lael",
                                 "Lahoma",
                                 "Lai",
                                 "Laila",
                                 "Laine",
                                 "Lajuana",
                                 "Lakeesha",
                                 "Lakeisha",
                                 "Lakendra",
                                 "Lakenya",
                                 "Lakesha",
                                 "Lakeshia",
                                 "Lakia",
                                 "Lakiesha",
                                 "Lakisha",
                                 "Lakita",
                                 "Lala",
                                 "Lamar",
                                 "Lamonica",
                                 "Lamont",
                                 "Lan",
                                 "Lana",
                                 "Lance",
                                 "Landon",
                                 "Lane",
                                 "Lane",
                                 "Lanell",
                                 "Lanelle",
                                 "Lanette",
                                 "Lang",
                                 "Lani",
                                 "Lanie",
                                 "Lanita",
                                 "Lannie",
                                 "Lanny",
                                 "Lanora",
                                 "Laquanda",
                                 "Laquita",
                                 "Lara",
                                 "Larae",
                                 "Laraine",
                                 "Laree",
                                 "Larhonda",
                                 "Larisa",
                                 "Larissa",
                                 "Larita",
                                 "Laronda",
                                 "Larraine",
                                 "Larry",
                                 "Larry",
                                 "Larue",
                                 "Lasandra",
                                 "Lashanda",
                                 "Lashandra",
                                 "Lashaun",
                                 "Lashaunda",
                                 "Lashawn",
                                 "Lashawna",
                                 "Lashawnda",
                                 "Lashay",
                                 "Lashell",
                                 "Lashon",
                                 "Lashonda",
                                 "Lashunda",
                                 "Lasonya",
                                 "Latanya",
                                 "Latarsha",
                                 "Latasha",
                                 "Latashia",
                                 "Latesha",
                                 "Latia",
                                 "Laticia",
                                 "Latina",
                                 "Latisha",
                                 "Latonia",
                                 "Latonya",
                                 "Latoria",
                                 "Latosha",
                                 "Latoya",
                                 "Latoyia",
                                 "Latrice",
                                 "Latricia",
                                 "Latrina",
                                 "Latrisha",
                                 "Launa",
                                 "Laura",
                                 "Lauralee",
                                 "Lauran",
                                 "Laure",
                                 "Laureen",
                                 "Laurel",
                                 "Lauren",
                                 "Lauren",
                                 "Laurena",
                                 "Laurence",
                                 "Laurence",
                                 "Laurene",
                                 "Lauretta",
                                 "Laurette",
                                 "Lauri",
                                 "Laurice",
                                 "Laurie",
                                 "Laurinda",
                                 "Laurine",
                                 "Lauryn",
                                 "Lavada",
                                 "Lavelle",
                                 "Lavenia",
                                 "Lavera",
                                 "Lavern",
                                 "Lavern",
                                 "Laverna",
                                 "Laverne",
                                 "Laverne",
                                 "Laveta",
                                 "Lavette",
                                 "Lavina",
                                 "Lavinia",
                                 "Lavon",
                                 "Lavona",
                                 "Lavonda",
                                 "Lavone",
                                 "Lavonia",
                                 "Lavonna",
                                 "Lavonne",
                                 "Lawana",
                                 "Lawanda",
                                 "Lawanna",
                                 "Lawerence",
                                 "Lawrence",
                                 "Lawrence",
                                 "Layla",
                                 "Layne",
                                 "Lazaro",
                                 "Le",
                                 "Lea",
                                 "Leah",
                                 "Lean",
                                 "Leana",
                                 "Leandra",
                                 "Leandro",
                                 "Leann",
                                 "Leanna",
                                 "Leanne",
                                 "Leanora",
                                 "Leatha",
                                 "Leatrice",
                                 "Lecia",
                                 "Leda",
                                 "Lee",
                                 "Lee",
                                 "Leeann",
                                 "Leeanna",
                                 "Leeanne",
                                 "Leena",
                                 "Leesa",
                                 "Leia",
                                 "Leida",
                                 "Leif",
                                 "Leigh",
                                 "Leigh",
                                 "Leigha",
                                 "Leighann",
                                 "Leila",
                                 "Leilani",
                                 "Leisa",
                                 "Leisha",
                                 "Lekisha",
                                 "Lela",
                                 "Lelah",
                                 "Leland",
                                 "Lelia",
                                 "Lemuel",
                                 "Len",
                                 "Lena",
                                 "Lenard",
                                 "Lenita",
                                 "Lenna",
                                 "Lennie",
                                 "Lenny",
                                 "Lenora",
                                 "Lenore",
                                 "Leo",
                                 "Leo",
                                 "Leola",
                                 "Leoma",
                                 "Leon",
                                 "Leon",
                                 "Leona",
                                 "Leonard",
                                 "Leonarda",
                                 "Leonardo",
                                 "Leone",
                                 "Leonel",
                                 "Leonia",
                                 "Leonida",
                                 "Leonie",
                                 "Leonila",
                                 "Leonor",
                                 "Leonora",
                                 "Leonore",
                                 "Leontine",
                                 "Leopoldo",
                                 "Leora",
                                 "Leota",
                                 "Lera",
                                 "Leroy",
                                 "Les",
                                 "Lesa",
                                 "Lesha",
                                 "Lesia",
                                 "Leslee",
                                 "Lesley",
                                 "Lesley",
                                 "Lesli",
                                 "Leslie",
                                 "Leslie",
                                 "Lessie",
                                 "Lester",
                                 "Lester",
                                 "Leta",
                                 "Letha",
                                 "Leticia",
                                 "Letisha",
                                 "Letitia",
                                 "Lettie",
                                 "Letty",
                                 "Levi",
                                 "Lewis",
                                 "Lewis",
                                 "Lexie",
                                 "Lezlie",
                                 "Li",
                                 "Lia",
                                 "Liana",
                                 "Liane",
                                 "Lianne",
                                 "Libbie",
                                 "Libby",
                                 "Liberty",
                                 "Librada",
                                 "Lida",
                                 "Lidia",
                                 "Lien",
                                 "Lieselotte",
                                 "Ligia",
                                 "Lila",
                                 "Lili",
                                 "Lilia",
                                 "Lilian",
                                 "Liliana",
                                 "Lilla",
                                 "Lilli",
                                 "Lillia",
                                 "Lilliam",
                                 "Lillian",
                                 "Lilliana",
                                 "Lillie",
                                 "Lilly",
                                 "Lily",
                                 "Lin",
                                 "Lina",
                                 "Lincoln",
                                 "Linda",
                                 "Lindsay",
                                 "Lindsay",
                                 "Lindsey",
                                 "Lindsey",
                                 "Lindsy",
                                 "Lindy",
                                 "Linette",
                                 "Ling",
                                 "Linh",
                                 "Linn",
                                 "Linnea",
                                 "Linnie",
                                 "Lino",
                                 "Linsey",
                                 "Linwood",
                                 "Lionel",
                                 "Lisa",
                                 "Lisabeth",
                                 "Lisandra",
                                 "Lisbeth",
                                 "Lise",
                                 "Lisette",
                                 "Lisha",
                                 "Lissa",
                                 "Lissette",
                                 "Lita",
                                 "Livia",
                                 "Liz",
                                 "Liza",
                                 "Lizabeth",
                                 "Lizbeth",
                                 "Lizeth",
                                 "Lizette",
                                 "Lizzette",
                                 "Lizzie",
                                 "Lloyd",
                                 "Loan",
                                 "Logan",
                                 "Logan",
                                 "Loida",
                                 "Lois",
                                 "Loise",
                                 "Lola",
                                 "Lolita",
                                 "Loma",
                                 "Lon",
                                 "Lona",
                                 "Londa",
                                 "Long",
                                 "Loni",
                                 "Lonna",
                                 "Lonnie",
                                 "Lonnie",
                                 "Lonny",
                                 "Lora",
                                 "Loraine",
                                 "Loralee",
                                 "Lore",
                                 "Lorean",
                                 "Loree",
                                 "Loreen",
                                 "Lorelei",
                                 "Loren",
                                 "Loren",
                                 "Lorena",
                                 "Lorene",
                                 "Lorenza",
                                 "Lorenzo",
                                 "Loreta",
                                 "Loretta",
                                 "Lorette",
                                 "Lori",
                                 "Loria",
                                 "Loriann",
                                 "Lorie",
                                 "Lorilee",
                                 "Lorina",
                                 "Lorinda",
                                 "Lorine",
                                 "Loris",
                                 "Lorita",
                                 "Lorna",
                                 "Lorraine",
                                 "Lorretta",
                                 "Lorri",
                                 "Lorriane",
                                 "Lorrie",
                                 "Lorrine",
                                 "Lory",
                                 "Lottie",
                                 "Lou",
                                 "Lou",
                                 "Louann",
                                 "Louanne",
                                 "Louella",
                                 "Louetta",
                                 "Louie",
                                 "Louie",
                                 "Louis",
                                 "Louis",
                                 "Louisa",
                                 "Louise",
                                 "Loura",
                                 "Lourdes",
                                 "Lourie",
                                 "Louvenia",
                                 "Love",
                                 "Lovella",
                                 "Lovetta",
                                 "Lovie",
                                 "Lowell",
                                 "Loyce",
                                 "Loyd",
                                 "Lu",
                                 "Luana",
                                 "Luann",
                                 "Luanna",
                                 "Luanne",
                                 "Luba",
                                 "Lucas",
                                 "Luci",
                                 "Lucia",
                                 "Luciana",
                                 "Luciano",
                                 "Lucie",
                                 "Lucien",
                                 "Lucienne",
                                 "Lucila",
                                 "Lucile",
                                 "Lucilla",
                                 "Lucille",
                                 "Lucina",
                                 "Lucinda",
                                 "Lucio",
                                 "Lucius",
                                 "Lucrecia",
                                 "Lucretia",
                                 "Lucy",
                                 "Ludie",
                                 "Ludivina",
                                 "Lue",
                                 "Luella",
                                 "Luetta",
                                 "Luigi",
                                 "Luis",
                                 "Luis",
                                 "Luisa",
                                 "Luise",
                                 "Luke",
                                 "Lula",
                                 "Lulu",
                                 "Luna",
                                 "Lupe",
                                 "Lupe",
                                 "Lupita",
                                 "Lura",
                                 "Lurlene",
                                 "Lurline",
                                 "Luther",
                                 "Luvenia",
                                 "Luz",
                                 "Lyda",
                                 "Lydia",
                                 "Lyla",
                                 "Lyle",
                                 "Lyman",
                                 "Lyn",
                                 "Lynda",
                                 "Lyndia",
                                 "Lyndon",
                                 "Lyndsay",
                                 "Lyndsey",
                                 "Lynell",
                                 "Lynelle",
                                 "Lynetta",
                                 "Lynette",
                                 "Lynn",
                                 "Lynn",
                                 "Lynna",
                                 "Lynne",
                                 "Lynnette",
                                 "Lynsey",
                                 "Lynwood",
                                 "Ma",
                                 "Mabel",
                                 "Mabelle",
                                 "Mable",
                                 "Mac",
                                 "Machelle",
                                 "Macie",
                                 "Mack",
                                 "Mackenzie",
                                 "Macy",
                                 "Madalene",
                                 "Madaline",
                                 "Madalyn",
                                 "Maddie",
                                 "Madelaine",
                                 "Madeleine",
                                 "Madelene",
                                 "Madeline",
                                 "Madelyn",
                                 "Madge",
                                 "Madie",
                                 "Madison",
                                 "Madlyn",
                                 "Madonna",
                                 "Mae",
                                 "Maegan",
                                 "Mafalda",
                                 "Magali",
                                 "Magaly",
                                 "Magan",
                                 "Magaret",
                                 "Magda",
                                 "Magdalen",
                                 "Magdalena",
                                 "Magdalene",
                                 "Magen",
                                 "Maggie",
                                 "Magnolia",
                                 "Mahalia",
                                 "Mai",
                                 "Maia",
                                 "Maida",
                                 "Maile",
                                 "Maira",
                                 "Maire",
                                 "Maisha",
                                 "Maisie",
                                 "Major",
                                 "Majorie",
                                 "Makeda",
                                 "Malcolm",
                                 "Malcom",
                                 "Malena",
                                 "Malia",
                                 "Malik",
                                 "Malika",
                                 "Malinda",
                                 "Malisa",
                                 "Malissa",
                                 "Malka",
                                 "Mallie",
                                 "Mallory",
                                 "Malorie",
                                 "Malvina",
                                 "Mamie",
                                 "Mammie",
                                 "Man",
                                 "Man",
                                 "Mana",
                                 "Manda",
                                 "Mandi",
                                 "Mandie",
                                 "Mandy",
                                 "Manie",
                                 "Manual",
                                 "Manuel",
                                 "Manuela",
                                 "Many",
                                 "Mao",
                                 "Maple",
                                 "Mara",
                                 "Maragaret",
                                 "Maragret",
                                 "Maranda",
                                 "Marc",
                                 "Marcel",
                                 "Marcela",
                                 "Marcelene",
                                 "Marcelina",
                                 "Marceline",
                                 "Marcelino",
                                 "Marcell",
                                 "Marcella",
                                 "Marcelle",
                                 "Marcellus",
                                 "Marcelo",
                                 "Marcene",
                                 "Marchelle",
                                 "Marci",
                                 "Marcia",
                                 "Marcie",
                                 "Marco",
                                 "Marcos",
                                 "Marcus",
                                 "Marcy",
                                 "Mardell",
                                 "Maren",
                                 "Marg",
                                 "Margaret",
                                 "Margareta",
                                 "Margarete",
                                 "Margarett",
                                 "Margaretta",
                                 "Margarette",
                                 "Margarita",
                                 "Margarite",
                                 "Margarito",
                                 "Margart",
                                 "Marge",
                                 "Margene",
                                 "Margeret",
                                 "Margert",
                                 "Margery",
                                 "Marget",
                                 "Margherita",
                                 "Margie",
                                 "Margit",
                                 "Margo",
                                 "Margorie",
                                 "Margot",
                                 "Margret",
                                 "Margrett",
                                 "Marguerita",
                                 "Marguerite",
                                 "Margurite",
                                 "Margy",
                                 "Marhta",
                                 "Mari",
                                 "Maria",
                                 "Maria",
                                 "Mariah",
                                 "Mariam",
                                 "Marian",
                                 "Mariana",
                                 "Marianela",
                                 "Mariann",
                                 "Marianna",
                                 "Marianne",
                                 "Mariano",
                                 "Maribel",
                                 "Maribeth",
                                 "Marica",
                                 "Maricela",
                                 "Maricruz",
                                 "Marie",
                                 "Mariel",
                                 "Mariela",
                                 "Mariella",
                                 "Marielle",
                                 "Marietta",
                                 "Mariette",
                                 "Mariko",
                                 "Marilee",
                                 "Marilou",
                                 "Marilu",
                                 "Marilyn",
                                 "Marilynn",
                                 "Marin",
                                 "Marina",
                                 "Marinda",
                                 "Marine",
                                 "Mario",
                                 "Mario",
                                 "Marion",
                                 "Marion",
                                 "Maris",
                                 "Marisa",
                                 "Marisela",
                                 "Marisha",
                                 "Marisol",
                                 "Marissa",
                                 "Marita",
                                 "Maritza",
                                 "Marivel",
                                 "Marjorie",
                                 "Marjory",
                                 "Mark",
                                 "Mark",
                                 "Marketta",
                                 "Markita",
                                 "Markus",
                                 "Marla",
                                 "Marlana",
                                 "Marleen",
                                 "Marlen",
                                 "Marlena",
                                 "Marlene",
                                 "Marlin",
                                 "Marlin",
                                 "Marline",
                                 "Marlo",
                                 "Marlon",
                                 "Marlyn",
                                 "Marlys",
                                 "Marna",
                                 "Marni",
                                 "Marnie",
                                 "Marquerite",
                                 "Marquetta",
                                 "Marquis",
                                 "Marquita",
                                 "Marquitta",
                                 "Marry",
                                 "Marsha",
                                 "Marshall",
                                 "Marshall",
                                 "Marta",
                                 "Marth",
                                 "Martha",
                                 "Marti",
                                 "Martin",
                                 "Martin",
                                 "Martina",
                                 "Martine",
                                 "Marty",
                                 "Marty",
                                 "Marva",
                                 "Marvel",
                                 "Marvella",
                                 "Marvin",
                                 "Marvis",
                                 "Marx",
                                 "Mary",
                                 "Mary",
                                 "Marya",
                                 "Maryalice",
                                 "Maryam",
                                 "Maryann",
                                 "Maryanna",
                                 "Maryanne",
                                 "Marybelle",
                                 "Marybeth",
                                 "Maryellen",
                                 "Maryetta",
                                 "Maryjane",
                                 "Maryjo",
                                 "Maryland",
                                 "Marylee",
                                 "Marylin",
                                 "Maryln",
                                 "Marylou",
                                 "Marylouise",
                                 "Marylyn",
                                 "Marylynn",
                                 "Maryrose",
                                 "Masako",
                                 "Mason",
                                 "Matha",
                                 "Mathew",
                                 "Mathilda",
                                 "Mathilde",
                                 "Matilda",
                                 "Matilde",
                                 "Matt",
                                 "Matthew",
                                 "Matthew",
                                 "Mattie",
                                 "Maud",
                                 "Maude",
                                 "Maudie",
                                 "Maura",
                                 "Maureen",
                                 "Maurice",
                                 "Maurice",
                                 "Mauricio",
                                 "Maurine",
                                 "Maurita",
                                 "Mauro",
                                 "Mavis",
                                 "Max",
                                 "Maxie",
                                 "Maxima",
                                 "Maximina",
                                 "Maximo",
                                 "Maxine",
                                 "Maxwell",
                                 "May",
                                 "Maya",
                                 "Maybell",
                                 "Maybelle",
                                 "Maye",
                                 "Mayme",
                                 "Maynard",
                                 "Mayola",
                                 "Mayra",
                                 "Mazie",
                                 "Mckenzie",
                                 "Mckinley",
                                 "Meagan",
                                 "Meaghan",
                                 "Mechelle",
                                 "Meda",
                                 "Mee",
                                 "Meg",
                                 "Megan",
                                 "Meggan",
                                 "Meghan",
                                 "Meghann",
                                 "Mei",
                                 "Mel",
                                 "Melaine",
                                 "Melani",
                                 "Melania",
                                 "Melanie",
                                 "Melany",
                                 "Melba",
                                 "Melda",
                                 "Melia",
                                 "Melida",
                                 "Melina",
                                 "Melinda",
                                 "Melisa",
                                 "Melissa",
                                 "Melissia",
                                 "Melita",
                                 "Mellie",
                                 "Mellisa",
                                 "Mellissa",
                                 "Melodee",
                                 "Melodi",
                                 "Melodie",
                                 "Melody",
                                 "Melonie",
                                 "Melony",
                                 "Melva",
                                 "Melvin",
                                 "Melvin",
                                 "Melvina",
                                 "Melynda",
                                 "Mendy",
                                 "Mercedes",
                                 "Mercedez",
                                 "Mercy",
                                 "Meredith",
                                 "Meri",
                                 "Merideth",
                                 "Meridith",
                                 "Merilyn",
                                 "Merissa",
                                 "Merle",
                                 "Merle",
                                 "Merlene",
                                 "Merlin",
                                 "Merlyn",
                                 "Merna",
                                 "Merri",
                                 "Merrie",
                                 "Merrilee",
                                 "Merrill",
                                 "Merrill",
                                 "Merry",
                                 "Mertie",
                                 "Mervin",
                                 "Meryl",
                                 "Meta",
                                 "Mi",
                                 "Mia",
                                 "Mica",
                                 "Micaela",
                                 "Micah",
                                 "Micah",
                                 "Micha",
                                 "Michael",
                                 "Michael",
                                 "Michaela",
                                 "Michaele",
                                 "Michal",
                                 "Michal",
                                 "Michale",
                                 "Micheal",
                                 "Micheal",
                                 "Michel",
                                 "Michel",
                                 "Michele",
                                 "Michelina",
                                 "Micheline",
                                 "Michell",
                                 "Michelle",
                                 "Michiko",
                                 "Mickey",
                                 "Mickey",
                                 "Micki",
                                 "Mickie",
                                 "Miesha",
                                 "Migdalia",
                                 "Mignon",
                                 "Miguel",
                                 "Miguelina",
                                 "Mika",
                                 "Mikaela",
                                 "Mike",
                                 "Mike",
                                 "Mikel",
                                 "Miki",
                                 "Mikki",
                                 "Mila",
                                 "Milagro",
                                 "Milagros",
                                 "Milan",
                                 "Milda",
                                 "Mildred",
                                 "Miles",
                                 "Milford",
                                 "Milissa",
                                 "Millard",
                                 "Millicent",
                                 "Millie",
                                 "Milly",
                                 "Milo",
                                 "Milton",
                                 "Mimi",
                                 "Min",
                                 "Mina",
                                 "Minda",
                                 "Mindi",
                                 "Mindy",
                                 "Minerva",
                                 "Ming",
                                 "Minh",
                                 "Minh",
                                 "Minna",
                                 "Minnie",
                                 "Minta",
                                 "Miquel",
                                 "Mira",
                                 "Miranda",
                                 "Mireille",
                                 "Mirella",
                                 "Mireya",
                                 "Miriam",
                                 "Mirian",
                                 "Mirna",
                                 "Mirta",
                                 "Mirtha",
                                 "Misha",
                                 "Miss",
                                 "Missy",
                                 "Misti",
                                 "Mistie",
                                 "Misty",
                                 "Mitch",
                                 "Mitchel",
                                 "Mitchell",
                                 "Mitchell",
                                 "Mitsue",
                                 "Mitsuko",
                                 "Mittie",
                                 "Mitzi",
                                 "Mitzie",
                                 "Miyoko",
                                 "Modesta",
                                 "Modesto",
                                 "Mohamed",
                                 "Mohammad",
                                 "Mohammed",
                                 "Moira",
                                 "Moises",
                                 "Mollie",
                                 "Molly",
                                 "Mona",
                                 "Monet",
                                 "Monica",
                                 "Monika",
                                 "Monique",
                                 "Monnie",
                                 "Monroe",
                                 "Monserrate",
                                 "Monte",
                                 "Monty",
                                 "Moon",
                                 "Mora",
                                 "Morgan",
                                 "Morgan",
                                 "Moriah",
                                 "Morris",
                                 "Morton",
                                 "Mose",
                                 "Moses",
                                 "Moshe",
                                 "Mozell",
                                 "Mozella",
                                 "Mozelle",
                                 "Mui",
                                 "Muoi",
                                 "Muriel",
                                 "Murray",
                                 "My",
                                 "Myesha",
                                 "Myles",
                                 "Myong",
                                 "Myra",
                                 "Myriam",
                                 "Myrl",
                                 "Myrle",
                                 "Myrna",
                                 "Myron",
                                 "Myrta",
                                 "Myrtice",
                                 "Myrtie",
                                 "Myrtis",
                                 "Myrtle",
                                 "Myung",
                                 "Na",
                                 "Nada",
                                 "Nadene",
                                 "Nadia",
                                 "Nadine",
                                 "Naida",
                                 "Nakesha",
                                 "Nakia",
                                 "Nakisha",
                                 "Nakita",
                                 "Nam",
                                 "Nan",
                                 "Nana",
                                 "Nancee",
                                 "Nancey",
                                 "Nanci",
                                 "Nancie",
                                 "Nancy",
                                 "Nanette",
                                 "Nannette",
                                 "Nannie",
                                 "Naoma",
                                 "Naomi",
                                 "Napoleon",
                                 "Narcisa",
                                 "Natacha",
                                 "Natalia",
                                 "Natalie",
                                 "Natalya",
                                 "Natasha",
                                 "Natashia",
                                 "Nathalie",
                                 "Nathan",
                                 "Nathanael",
                                 "Nathanial",
                                 "Nathaniel",
                                 "Natisha",
                                 "Natividad",
                                 "Natosha",
                                 "Neal",
                                 "Necole",
                                 "Ned",
                                 "Neda",
                                 "Nedra",
                                 "Neely",
                                 "Neida",
                                 "Neil",
                                 "Nelda",
                                 "Nelia",
                                 "Nelida",
                                 "Nell",
                                 "Nella",
                                 "Nelle",
                                 "Nellie",
                                 "Nelly",
                                 "Nelson",
                                 "Nena",
                                 "Nenita",
                                 "Neoma",
                                 "Neomi",
                                 "Nereida",
                                 "Nerissa",
                                 "Nery",
                                 "Nestor",
                                 "Neta",
                                 "Nettie",
                                 "Neva",
                                 "Nevada",
                                 "Neville",
                                 "Newton",
                                 "Nga",
                                 "Ngan",
                                 "Ngoc",
                                 "Nguyet",
                                 "Nia",
                                 "Nichelle",
                                 "Nichol",
                                 "Nicholas",
                                 "Nichole",
                                 "Nicholle",
                                 "Nick",
                                 "Nicki",
                                 "Nickie",
                                 "Nickolas",
                                 "Nickole",
                                 "Nicky",
                                 "Nicky",
                                 "Nicol",
                                 "Nicola",
                                 "Nicolas",
                                 "Nicolasa",
                                 "Nicole",
                                 "Nicolette",
                                 "Nicolle",
                                 "Nida",
                                 "Nidia",
                                 "Niesha",
                                 "Nieves",
                                 "Nigel",
                                 "Niki",
                                 "Nikia",
                                 "Nikita",
                                 "Nikki",
                                 "Nikole",
                                 "Nila",
                                 "Nilda",
                                 "Nilsa",
                                 "Nina",
                                 "Ninfa",
                                 "Nisha",
                                 "Nita",
                                 "Noah",
                                 "Noble",
                                 "Nobuko",
                                 "Noe",
                                 "Noel",
                                 "Noel",
                                 "Noelia",
                                 "Noella",
                                 "Noelle",
                                 "Noemi",
                                 "Nohemi",
                                 "Nola",
                                 "Nolan",
                                 "Noma",
                                 "Nona",
                                 "Nora",
                                 "Norah",
                                 "Norbert",
                                 "Norberto",
                                 "Noreen",
                                 "Norene",
                                 "Noriko",
                                 "Norine",
                                 "Norma",
                                 "Norman",
                                 "Norman",
                                 "Normand",
                                 "Norris",
                                 "Nova",
                                 "Novella",
                                 "Nu",
                                 "Nubia",
                                 "Numbers",
                                 "Numbers",
                                 "Nydia",
                                 "Nyla",
                                 "Obdulia",
                                 "Ocie",
                                 "Octavia",
                                 "Octavio",
                                 "Oda",
                                 "Odelia",
                                 "Odell",
                                 "Odell",
                                 "Odessa",
                                 "Odette",
                                 "Odilia",
                                 "Odis",
                                 "Ofelia",
                                 "Ok",
                                 "Ola",
                                 "Olen",
                                 "Olene",
                                 "Oleta",
                                 "Olevia",
                                 "Olga",
                                 "Olimpia",
                                 "Olin",
                                 "Olinda",
                                 "Oliva",
                                 "Olive",
                                 "Oliver",
                                 "Olivia",
                                 "Ollie",
                                 "Ollie",
                                 "Olympia",
                                 "Oma",
                                 "Omar",
                                 "Omega",
                                 "Omer",
                                 "Ona",
                                 "Oneida",
                                 "Onie",
                                 "Onita",
                                 "Opal",
                                 "Ophelia",
                                 "Ora",
                                 "Oralee",
                                 "Oralia",
                                 "Oren",
                                 "Oretha",
                                 "Orlando",
                                 "Orpha",
                                 "Orval",
                                 "Orville",
                                 "Oscar",
                                 "Oscar",
                                 "Ossie",
                                 "Osvaldo",
                                 "Oswaldo",
                                 "Otelia",
                                 "Otha",
                                 "Otha",
                                 "Otilia",
                                 "Otis",
                                 "Otto",
                                 "Ouida",
                                 "Owen",
                                 "Ozell",
                                 "Ozella",
                                 "Ozie",
                                 "Pa",
                                 "Pablo",
                                 "Page",
                                 "Paige",
                                 "Palma",
                                 "Palmer",
                                 "Palmira",
                                 "Pam",
                                 "Pamala",
                                 "Pamela",
                                 "Pamelia",
                                 "Pamella",
                                 "Pamila",
                                 "Pamula",
                                 "Pandora",
                                 "Pansy",
                                 "Paola",
                                 "Paris",
                                 "Paris",
                                 "Parker",
                                 "Parthenia",
                                 "Particia",
                                 "Pasquale",
                                 "Pasty",
                                 "Pat",
                                 "Pat",
                                 "Patience",
                                 "Patria",
                                 "Patrica",
                                 "Patrice",
                                 "Patricia",
                                 "Patricia",
                                 "Patrick",
                                 "Patrick",
                                 "Patrina",
                                 "Patsy",
                                 "Patti",
                                 "Pattie",
                                 "Patty",
                                 "Paul",
                                 "Paul",
                                 "Paula",
                                 "Paulene",
                                 "Pauletta",
                                 "Paulette",
                                 "Paulina",
                                 "Pauline",
                                 "Paulita",
                                 "Paz",
                                 "Pearl",
                                 "Pearle",
                                 "Pearlene",
                                 "Pearlie",
                                 "Pearline",
                                 "Pearly",
                                 "Pedro",
                                 "Peg",
                                 "Peggie",
                                 "Peggy",
                                 "Pei",
                                 "Penelope",
                                 "Penney",
                                 "Penni",
                                 "Pennie",
                                 "Penny",
                                 "Percy",
                                 "Perla",
                                 "Perry",
                                 "Perry",
                                 "Pete",
                                 "Peter",
                                 "Peter",
                                 "Petra",
                                 "Petrina",
                                 "Petronila",
                                 "Phebe",
                                 "Phil",
                                 "Philip",
                                 "Phillip",
                                 "Phillis",
                                 "Philomena",
                                 "Phoebe",
                                 "Phung",
                                 "Phuong",
                                 "Phylicia",
                                 "Phylis",
                                 "Phyliss",
                                 "Phyllis",
                                 "Pia",
                                 "Piedad",
                                 "Pierre",
                                 "Pilar",
                                 "Ping",
                                 "Pinkie",
                                 "Piper",
                                 "Pok",
                                 "Polly",
                                 "Porfirio",
                                 "Porsche",
                                 "Porsha",
                                 "Porter",
                                 "Portia",
                                 "Precious",
                                 "Preston",
                                 "Pricilla",
                                 "Prince",
                                 "Princess",
                                 "Priscila",
                                 "Priscilla",
                                 "Providencia",
                                 "Prudence",
                                 "Pura",
                                 "Qiana",
                                 "Queen",
                                 "Queenie",
                                 "Quentin",
                                 "Quiana",
                                 "Quincy",
                                 "Quinn",
                                 "Quinn",
                                 "Quintin",
                                 "Quinton",
                                 "Quyen",
                                 "Rachael",
                                 "Rachal",
                                 "Racheal",
                                 "Rachel",
                                 "Rachele",
                                 "Rachell",
                                 "Rachelle",
                                 "Racquel",
                                 "Rae",
                                 "Raeann",
                                 "Raelene",
                                 "Rafael",
                                 "Rafaela",
                                 "Raguel",
                                 "Raina",
                                 "Raisa",
                                 "Raleigh",
                                 "Ralph",
                                 "Ramiro",
                                 "Ramon",
                                 "Ramona",
                                 "Ramonita",
                                 "Rana",
                                 "Ranae",
                                 "Randa",
                                 "Randal",
                                 "Randall",
                                 "Randee",
                                 "Randell",
                                 "Randi",
                                 "Randolph",
                                 "Randy",
                                 "Randy",
                                 "Ranee",
                                 "Raphael",
                                 "Raquel",
                                 "Rashad",
                                 "Rasheeda",
                                 "Rashida",
                                 "Raul",
                                 "Raven",
                                 "Ray",
                                 "Ray",
                                 "Raye",
                                 "Rayford",
                                 "Raylene",
                                 "Raymon",
                                 "Raymond",
                                 "Raymond",
                                 "Raymonde",
                                 "Raymundo",
                                 "Rayna",
                                 "Rea",
                                 "Reagan",
                                 "Reanna",
                                 "Reatha",
                                 "Reba",
                                 "Rebbeca",
                                 "Rebbecca",
                                 "Rebeca",
                                 "Rebecca",
                                 "Rebecka",
                                 "Rebekah",
                                 "Reda",
                                 "Reed",
                                 "Reena",
                                 "Refugia",
                                 "Refugio",
                                 "Refugio",
                                 "Regan",
                                 "Regena",
                                 "Regenia",
                                 "Reggie",
                                 "Regina",
                                 "Reginald",
                                 "Regine",
                                 "Reginia",
                                 "Reid",
                                 "Reiko",
                                 "Reina",
                                 "Reinaldo",
                                 "Reita",
                                 "Rema",
                                 "Remedios",
                                 "Remona",
                                 "Rena",
                                 "Renae",
                                 "Renaldo",
                                 "Renata",
                                 "Renate",
                                 "Renato",
                                 "Renay",
                                 "Renda",
                                 "Rene",
                                 "Rene",
                                 "Renea",
                                 "Renee",
                                 "Renetta",
                                 "Renita",
                                 "Renna",
                                 "Ressie",
                                 "Reta",
                                 "Retha",
                                 "Retta",
                                 "Reuben",
                                 "Reva",
                                 "Rex",
                                 "Rey",
                                 "Reyes",
                                 "Reyna",
                                 "Reynalda",
                                 "Reynaldo",
                                 "Rhea",
                                 "Rheba",
                                 "Rhett",
                                 "Rhiannon",
                                 "Rhoda",
                                 "Rhona",
                                 "Rhonda",
                                 "Ria",
                                 "Ricarda",
                                 "Ricardo",
                                 "Rich",
                                 "Richard",
                                 "Richard",
                                 "Richelle",
                                 "Richie",
                                 "Rick",
                                 "Rickey",
                                 "Ricki",
                                 "Rickie",
                                 "Rickie",
                                 "Ricky",
                                 "Rico",
                                 "Rigoberto",
                                 "Rikki",
                                 "Riley",
                                 "Rima",
                                 "Rina",
                                 "Risa",
                                 "Rita",
                                 "Riva",
                                 "Rivka",
                                 "Rob",
                                 "Robbi",
                                 "Robbie",
                                 "Robbie",
                                 "Robbin",
                                 "Robby",
                                 "Robbyn",
                                 "Robena",
                                 "Robert",
                                 "Robert",
                                 "Roberta",
                                 "Roberto",
                                 "Roberto",
                                 "Robin",
                                 "Robin",
                                 "Robt",
                                 "Robyn",
                                 "Rocco",
                                 "Rochel",
                                 "Rochell",
                                 "Rochelle",
                                 "Rocio",
                                 "Rocky",
                                 "Rod",
                                 "Roderick",
                                 "Rodger",
                                 "Rodney",
                                 "Rodolfo",
                                 "Rodrick",
                                 "Rodrigo",
                                 "Rogelio",
                                 "Roger",
                                 "Roland",
                                 "Rolanda",
                                 "Rolande",
                                 "Rolando",
                                 "Rolf",
                                 "Rolland",
                                 "Roma",
                                 "Romaine",
                                 "Roman",
                                 "Romana",
                                 "Romelia",
                                 "Romeo",
                                 "Romona",
                                 "Ron",
                                 "Rona",
                                 "Ronald",
                                 "Ronald",
                                 "Ronda",
                                 "Roni",
                                 "Ronna",
                                 "Ronni",
                                 "Ronnie",
                                 "Ronnie",
                                 "Ronny",
                                 "Roosevelt",
                                 "Rory",
                                 "Rory",
                                 "Rosa",
                                 "Rosalba",
                                 "Rosalee",
                                 "Rosalia",
                                 "Rosalie",
                                 "Rosalina",
                                 "Rosalind",
                                 "Rosalinda",
                                 "Rosaline",
                                 "Rosalva",
                                 "Rosalyn",
                                 "Rosamaria",
                                 "Rosamond",
                                 "Rosana",
                                 "Rosann",
                                 "Rosanna",
                                 "Rosanne",
                                 "Rosaria",
                                 "Rosario",
                                 "Rosario",
                                 "Rosaura",
                                 "Roscoe",
                                 "Rose",
                                 "Roseann",
                                 "Roseanna",
                                 "Roseanne",
                                 "Roselee",
                                 "Roselia",
                                 "Roseline",
                                 "Rosella",
                                 "Roselle",
                                 "Roselyn",
                                 "Rosemarie",
                                 "Rosemary",
                                 "Rosena",
                                 "Rosenda",
                                 "Rosendo",
                                 "Rosetta",
                                 "Rosette",
                                 "Rosia",
                                 "Rosie",
                                 "Rosina",
                                 "Rosio",
                                 "Rosita",
                                 "Roslyn",
                                 "Ross",
                                 "Rossana",
                                 "Rossie",
                                 "Rosy",
                                 "Rowena",
                                 "Roxana",
                                 "Roxane",
                                 "Roxann",
                                 "Roxanna",
                                 "Roxanne",
                                 "Roxie",
                                 "Roxy",
                                 "Roy",
                                 "Roy",
                                 "Royal",
                                 "Royce",
                                 "Royce",
                                 "Rozanne",
                                 "Rozella",
                                 "Ruben",
                                 "Rubi",
                                 "Rubie",
                                 "Rubin",
                                 "Ruby",
                                 "Rubye",
                                 "Rudolf",
                                 "Rudolph",
                                 "Rudy",
                                 "Rudy",
                                 "Rueben",
                                 "Rufina",
                                 "Rufus",
                                 "Rupert",
                                 "Russ",
                                 "Russel",
                                 "Russell",
                                 "Russell",
                                 "Rusty",
                                 "Ruth",
                                 "Rutha",
                                 "Ruthann",
                                 "Ruthanne",
                                 "Ruthe",
                                 "Ruthie",
                                 "Ryan",
                                 "Ryan",
                                 "Ryann",
                                 "Sabina",
                                 "Sabine",
                                 "Sabra",
                                 "Sabrina",
                                 "Sacha",
                                 "Sachiko",
                                 "Sade",
                                 "Sadie",
                                 "Sadye",
                                 "Sage",
                                 "Sal",
                                 "Salena",
                                 "Salina",
                                 "Salley",
                                 "Sallie",
                                 "Sally",
                                 "Salome",
                                 "Salvador",
                                 "Salvatore",
                                 "Sam",
                                 "Sam",
                                 "Samantha",
                                 "Samara",
                                 "Samatha",
                                 "Samella",
                                 "Samira",
                                 "Sammie",
                                 "Sammie",
                                 "Sammy",
                                 "Sammy",
                                 "Samual",
                                 "Samuel",
                                 "Samuel",
                                 "Sana",
                                 "Sanda",
                                 "Sandee",
                                 "Sandi",
                                 "Sandie",
                                 "Sandra",
                                 "Sandy",
                                 "Sandy",
                                 "Sanford",
                                 "Sang",
                                 "Sang",
                                 "Sanjuana",
                                 "Sanjuanita",
                                 "Sanora",
                                 "Santa",
                                 "Santana",
                                 "Santiago",
                                 "Santina",
                                 "Santo",
                                 "Santos",
                                 "Santos",
                                 "Sara",
                                 "Sarah",
                                 "Sarai",
                                 "Saran",
                                 "Sari",
                                 "Sarina",
                                 "Sarita",
                                 "Sasha",
                                 "Saturnina",
                                 "Sau",
                                 "Saul",
                                 "Saundra",
                                 "Savanna",
                                 "Savannah",
                                 "Scarlet",
                                 "Scarlett",
                                 "Scot",
                                 "Scott",
                                 "Scott",
                                 "Scottie",
                                 "Scottie",
                                 "Scotty",
                                 "Sean",
                                 "Sean",
                                 "Season",
                                 "Sebastian",
                                 "Sebrina",
                                 "See",
                                 "Seema",
                                 "Selena",
                                 "Selene",
                                 "Selina",
                                 "Selma",
                                 "Sena",
                                 "Senaida",
                                 "September",
                                 "Serafina",
                                 "Serena",
                                 "Sergio",
                                 "Serina",
                                 "Serita",
                                 "Seth",
                                 "Setsuko",
                                 "Seymour",
                                 "Sha",
                                 "Shad",
                                 "Shae",
                                 "Shaina",
                                 "Shakia",
                                 "Shakira",
                                 "Shakita",
                                 "Shala",
                                 "Shalanda",
                                 "Shalon",
                                 "Shalonda",
                                 "Shameka",
                                 "Shamika",
                                 "Shan",
                                 "Shana",
                                 "Shanae",
                                 "Shanda",
                                 "Shandi",
                                 "Shandra",
                                 "Shane",
                                 "Shane",
                                 "Shaneka",
                                 "Shanel",
                                 "Shanell",
                                 "Shanelle",
                                 "Shani",
                                 "Shanice",
                                 "Shanika",
                                 "Shaniqua",
                                 "Shanita",
                                 "Shanna",
                                 "Shannan",
                                 "Shannon",
                                 "Shannon",
                                 "Shanon",
                                 "Shanta",
                                 "Shantae",
                                 "Shantay",
                                 "Shante",
                                 "Shantel",
                                 "Shantell",
                                 "Shantelle",
                                 "Shanti",
                                 "Shaquana",
                                 "Shaquita",
                                 "Shara",
                                 "Sharan",
                                 "Sharda",
                                 "Sharee",
                                 "Sharell",
                                 "Sharen",
                                 "Shari",
                                 "Sharice",
                                 "Sharie",
                                 "Sharika",
                                 "Sharilyn",
                                 "Sharita",
                                 "Sharla",
                                 "Sharleen",
                                 "Sharlene",
                                 "Sharmaine",
                                 "Sharolyn",
                                 "Sharon",
                                 "Sharonda",
                                 "Sharri",
                                 "Sharron",
                                 "Sharyl",
                                 "Sharyn",
                                 "Shasta",
                                 "Shaun",
                                 "Shaun",
                                 "Shauna",
                                 "Shaunda",
                                 "Shaunna",
                                 "Shaunta",
                                 "Shaunte",
                                 "Shavon",
                                 "Shavonda",
                                 "Shavonne",
                                 "Shawana",
                                 "Shawanda",
                                 "Shawanna",
                                 "Shawn",
                                 "Shawn",
                                 "Shawna",
                                 "Shawnda",
                                 "Shawnee",
                                 "Shawnna",
                                 "Shawnta",
                                 "Shay",
                                 "Shayla",
                                 "Shayna",
                                 "Shayne",
                                 "Shayne",
                                 "Shea",
                                 "Sheba",
                                 "Sheena",
                                 "Sheila",
                                 "Sheilah",
                                 "Shela",
                                 "Shelba",
                                 "Shelby",
                                 "Shelby",
                                 "Sheldon",
                                 "Shelia",
                                 "Shella",
                                 "Shelley",
                                 "Shelli",
                                 "Shellie",
                                 "Shelly",
                                 "Shelton",
                                 "Shemeka",
                                 "Shemika",
                                 "Shena",
                                 "Shenika",
                                 "Shenita",
                                 "Shenna",
                                 "Shera",
                                 "Sheree",
                                 "Sherell",
                                 "Sheri",
                                 "Sherice",
                                 "Sheridan",
                                 "Sherie",
                                 "Sherika",
                                 "Sherill",
                                 "Sherilyn",
                                 "Sherise",
                                 "Sherita",
                                 "Sherlene",
                                 "Sherley",
                                 "Sherly",
                                 "Sherlyn",
                                 "Sherman",
                                 "Sheron",
                                 "Sherrell",
                                 "Sherri",
                                 "Sherrie",
                                 "Sherril",
                                 "Sherrill",
                                 "Sherron",
                                 "Sherry",
                                 "Sherryl",
                                 "Sherwood",
                                 "Shery",
                                 "Sheryl",
                                 "Sheryll",
                                 "Shiela",
                                 "Shila",
                                 "Shiloh",
                                 "Shin",
                                 "Shira",
                                 "Shirely",
                                 "Shirl",
                                 "Shirlee",
                                 "Shirleen",
                                 "Shirlene",
                                 "Shirley",
                                 "Shirley",
                                 "Shirly",
                                 "Shizue",
                                 "Shizuko",
                                 "Shon",
                                 "Shona",
                                 "Shonda",
                                 "Shondra",
                                 "Shonna",
                                 "Shonta",
                                 "Shoshana",
                                 "Shu",
                                 "Shyla",
                                 "Sibyl",
                                 "Sid",
                                 "Sidney",
                                 "Sidney",
                                 "Sierra",
                                 "Signe",
                                 "Sigrid",
                                 "Silas",
                                 "Silva",
                                 "Silvana",
                                 "Silvia",
                                 "Sima",
                                 "Simon",
                                 "Simona",
                                 "Simone",
                                 "Simonne",
                                 "Sina",
                                 "Sindy",
                                 "Siobhan",
                                 "Sirena",
                                 "Siu",
                                 "Sixta",
                                 "Skye",
                                 "Slyvia",
                                 "So",
                                 "Socorro",
                                 "Sofia",
                                 "Soila",
                                 "Sol",
                                 "Sol",
                                 "Solange",
                                 "Soledad",
                                 "Solomon",
                                 "Somer",
                                 "Sommer",
                                 "Son",
                                 "Son",
                                 "Sona",
                                 "Sondra",
                                 "Song",
                                 "Sonia",
                                 "Sonja",
                                 "Sonny",
                                 "Sonya",
                                 "Soo",
                                 "Sook",
                                 "Soon",
                                 "Sophia",
                                 "Sophie",
                                 "Soraya",
                                 "Sparkle",
                                 "Spencer",
                                 "Spring",
                                 "Stacee",
                                 "Stacey",
                                 "Stacey",
                                 "Staci",
                                 "Stacia",
                                 "Stacie",
                                 "Stacy",
                                 "Stacy",
                                 "Stan",
                                 "Stanford",
                                 "Stanley",
                                 "Stanton",
                                 "Star",
                                 "Starla",
                                 "Starr",
                                 "Stasia",
                                 "Stefan",
                                 "Stefani",
                                 "Stefania",
                                 "Stefanie",
                                 "Stefany",
                                 "Steffanie",
                                 "Stella",
                                 "Stepanie",
                                 "Stephaine",
                                 "Stephan",
                                 "Stephane",
                                 "Stephani",
                                 "Stephania",
                                 "Stephanie",
                                 "Stephany",
                                 "Stephen",
                                 "Stephen",
                                 "Stephenie",
                                 "Stephine",
                                 "Stephnie",
                                 "Sterling",
                                 "Steve",
                                 "Steven",
                                 "Steven",
                                 "Stevie",
                                 "Stevie",
                                 "Stewart",
                                 "Stormy",
                                 "Stuart",
                                 "Su",
                                 "Suanne",
                                 "Sudie",
                                 "Sue",
                                 "Sueann",
                                 "Suellen",
                                 "Suk",
                                 "Sulema",
                                 "Sumiko",
                                 "Summer",
                                 "Sun",
                                 "Sunday",
                                 "Sung",
                                 "Sung",
                                 "Sunni",
                                 "Sunny",
                                 "Sunshine",
                                 "Susan",
                                 "Susana",
                                 "Susann",
                                 "Susanna",
                                 "Susannah",
                                 "Susanne",
                                 "Susie",
                                 "Susy",
                                 "Suzan",
                                 "Suzann",
                                 "Suzanna",
                                 "Suzanne",
                                 "Suzette",
                                 "Suzi",
                                 "Suzie",
                                 "Suzy",
                                 "Svetlana",
                                 "Sybil",
                                 "Syble",
                                 "Sydney",
                                 "Sydney",
                                 "Sylvester",
                                 "Sylvia",
                                 "Sylvie",
                                 "Synthia",
                                 "Syreeta",
                                 "Ta",
                                 "Tabatha",
                                 "Tabetha",
                                 "Tabitha",
                                 "Tad",
                                 "Tai",
                                 "Taina",
                                 "Taisha",
                                 "Tajuana",
                                 "Takako",
                                 "Takisha",
                                 "Talia",
                                 "Talisha",
                                 "Talitha",
                                 "Tam",
                                 "Tama",
                                 "Tamala",
                                 "Tamar",
                                 "Tamara",
                                 "Tamatha",
                                 "Tambra",
                                 "Tameika",
                                 "Tameka",
                                 "Tamekia",
                                 "Tamela",
                                 "Tamera",
                                 "Tamesha",
                                 "Tami",
                                 "Tamica",
                                 "Tamie",
                                 "Tamika",
                                 "Tamiko",
                                 "Tamisha",
                                 "Tammara",
                                 "Tammera",
                                 "Tammi",
                                 "Tammie",
                                 "Tammy",
                                 "Tamra",
                                 "Tana",
                                 "Tandra",
                                 "Tandy",
                                 "Taneka",
                                 "Tanesha",
                                 "Tangela",
                                 "Tania",
                                 "Tanika",
                                 "Tanisha",
                                 "Tanja",
                                 "Tanna",
                                 "Tanner",
                                 "Tanya",
                                 "Tara",
                                 "Tarah",
                                 "Taren",
                                 "Tari",
                                 "Tarra",
                                 "Tarsha",
                                 "Taryn",
                                 "Tasha",
                                 "Tashia",
                                 "Tashina",
                                 "Tasia",
                                 "Tatiana",
                                 "Tatum",
                                 "Tatyana",
                                 "Taunya",
                                 "Tawana",
                                 "Tawanda",
                                 "Tawanna",
                                 "Tawna",
                                 "Tawny",
                                 "Tawnya",
                                 "Taylor",
                                 "Taylor",
                                 "Tayna",
                                 "Ted",
                                 "Teddy",
                                 "Teena",
                                 "Tegan",
                                 "Teisha",
                                 "Telma",
                                 "Temeka",
                                 "Temika",
                                 "Tempie",
                                 "Temple",
                                 "Tena",
                                 "Tenesha",
                                 "Tenisha",
                                 "Tennie",
                                 "Tennille",
                                 "Teodora",
                                 "Teodoro",
                                 "Teofila",
                                 "Tequila",
                                 "Tera",
                                 "Tereasa",
                                 "Terence",
                                 "Teresa",
                                 "Terese",
                                 "Teresia",
                                 "Teresita",
                                 "Teressa",
                                 "Teri",
                                 "Terica",
                                 "Terina",
                                 "Terisa",
                                 "Terra",
                                 "Terrance",
                                 "Terrell",
                                 "Terrell",
                                 "Terrence",
                                 "Terresa",
                                 "Terri",
                                 "Terrie",
                                 "Terrilyn",
                                 "Terry",
                                 "Terry",
                                 "Tesha",
                                 "Tess",
                                 "Tessa",
                                 "Tessie",
                                 "Thad",
                                 "Thaddeus",
                                 "Thalia",
                                 "Thanh",
                                 "Thanh",
                                 "Thao",
                                 "Thea",
                                 "Theda",
                                 "Thelma",
                                 "Theo",
                                 "Theo",
                                 "Theodora",
                                 "Theodore",
                                 "Theola",
                                 "Theresa",
                                 "Therese",
                                 "Theresia",
                                 "Theressa",
                                 "Theron",
                                 "Thersa",
                                 "Thi",
                                 "Thomas",
                                 "Thomas",
                                 "Thomasena",
                                 "Thomasina",
                                 "Thomasine",
                                 "Thora",
                                 "Thresa",
                                 "Thu",
                                 "Thurman",
                                 "Thuy",
                                 "Tia",
                                 "Tiana",
                                 "Tianna",
                                 "Tiara",
                                 "Tien",
                                 "Tiera",
                                 "Tierra",
                                 "Tiesha",
                                 "Tifany",
                                 "Tiffaney",
                                 "Tiffani",
                                 "Tiffanie",
                                 "Tiffany",
                                 "Tiffiny",
                                 "Tijuana",
                                 "Tilda",
                                 "Tillie",
                                 "Tim",
                                 "Timika",
                                 "Timmy",
                                 "Timothy",
                                 "Timothy",
                                 "Tina",
                                 "Tinisha",
                                 "Tiny",
                                 "Tisa",
                                 "Tish",
                                 "Tisha",
                                 "Titus",
                                 "Tobi",
                                 "Tobias",
                                 "Tobie",
                                 "Toby",
                                 "Toby",
                                 "Toccara",
                                 "Tod",
                                 "Todd",
                                 "Toi",
                                 "Tom",
                                 "Tomas",
                                 "Tomasa",
                                 "Tomeka",
                                 "Tomi",
                                 "Tomika",
                                 "Tomiko",
                                 "Tommie",
                                 "Tommie",
                                 "Tommy",
                                 "Tommy",
                                 "Tommye",
                                 "Tomoko",
                                 "Tona",
                                 "Tonda",
                                 "Tonette",
                                 "Toney",
                                 "Toni",
                                 "Tonia",
                                 "Tonie",
                                 "Tonisha",
                                 "Tonita",
                                 "Tonja",
                                 "Tony",
                                 "Tony",
                                 "Tonya",
                                 "Tora",
                                 "Tori",
                                 "Torie",
                                 "Torri",
                                 "Torrie",
                                 "Tory",
                                 "Tory",
                                 "Tosha",
                                 "Toshia",
                                 "Toshiko",
                                 "Tova",
                                 "Towanda",
                                 "Toya",
                                 "Tracee",
                                 "Tracey",
                                 "Tracey",
                                 "Traci",
                                 "Tracie",
                                 "Tracy",
                                 "Tracy",
                                 "Tran",
                                 "Trang",
                                 "Travis",
                                 "Travis",
                                 "Treasa",
                                 "Treena",
                                 "Trena",
                                 "Trent",
                                 "Trenton",
                                 "Tresa",
                                 "Tressa",
                                 "Tressie",
                                 "Treva",
                                 "Trevor",
                                 "Trey",
                                 "Tricia",
                                 "Trina",
                                 "Trinh",
                                 "Trinidad",
                                 "Trinidad",
                                 "Trinity",
                                 "Trish",
                                 "Trisha",
                                 "Trista",
                                 "Tristan",
                                 "Tristan",
                                 "Troy",
                                 "Troy",
                                 "Trudi",
                                 "Trudie",
                                 "Trudy",
                                 "Trula",
                                 "Truman",
                                 "Tu",
                                 "Tuan",
                                 "Tula",
                                 "Tuyet",
                                 "Twana",
                                 "Twanda",
                                 "Twanna",
                                 "Twila",
                                 "Twyla",
                                 "Ty",
                                 "Tyesha",
                                 "Tyisha",
                                 "Tyler",
                                 "Tyler",
                                 "Tynisha",
                                 "Tyra",
                                 "Tyree",
                                 "Tyrell",
                                 "Tyron",
                                 "Tyrone",
                                 "Tyson",
                                 "Ula",
                                 "Ulrike",
                                 "Ulysses",
                                 "Un",
                                 "Una",
                                 "Ursula",
                                 "Usha",
                                 "Ute",
                                 "Vada",
                                 "Val",
                                 "Val",
                                 "Valarie",
                                 "Valda",
                                 "Valencia",
                                 "Valene",
                                 "Valentin",
                                 "Valentina",
                                 "Valentine",
                                 "Valentine",
                                 "Valeri",
                                 "Valeria",
                                 "Valerie",
                                 "Valery",
                                 "Vallie",
                                 "Valorie",
                                 "Valrie",
                                 "Van",
                                 "Van",
                                 "Vance",
                                 "Vanda",
                                 "Vanesa",
                                 "Vanessa",
                                 "Vanetta",
                                 "Vania",
                                 "Vanita",
                                 "Vanna",
                                 "Vannesa",
                                 "Vannessa",
                                 "Vashti",
                                 "Vasiliki",
                                 "Vaughn",
                                 "Veda",
                                 "Velda",
                                 "Velia",
                                 "Vella",
                                 "Velma",
                                 "Velva",
                                 "Velvet",
                                 "Vena",
                                 "Venessa",
                                 "Venetta",
                                 "Venice",
                                 "Venita",
                                 "Vennie",
                                 "Venus",
                                 "Veola",
                                 "Vera",
                                 "Verda",
                                 "Verdell",
                                 "Verdie",
                                 "Verena",
                                 "Vergie",
                                 "Verla",
                                 "Verlene",
                                 "Verlie",
                                 "Verline",
                                 "Vern",
                                 "Verna",
                                 "Vernell",
                                 "Vernetta",
                                 "Vernia",
                                 "Vernice",
                                 "Vernie",
                                 "Vernita",
                                 "Vernon",
                                 "Vernon",
                                 "Verona",
                                 "Veronica",
                                 "Veronika",
                                 "Veronique",
                                 "Versie",
                                 "Vertie",
                                 "Vesta",
                                 "Veta",
                                 "Vi",
                                 "Vicenta",
                                 "Vicente",
                                 "Vickey",
                                 "Vicki",
                                 "Vickie",
                                 "Vicky",
                                 "Victor",
                                 "Victor",
                                 "Victoria",
                                 "Victorina",
                                 "Vida",
                                 "Viki",
                                 "Vikki",
                                 "Vilma",
                                 "Vina",
                                 "Vince",
                                 "Vincent",
                                 "Vincenza",
                                 "Vincenzo",
                                 "Vinita",
                                 "Vinnie",
                                 "Viola",
                                 "Violet",
                                 "Violeta",
                                 "Violette",
                                 "Virgen",
                                 "Virgie",
                                 "Virgil",
                                 "Virgil",
                                 "Virgilio",
                                 "Virgina",
                                 "Virginia",
                                 "Vita",
                                 "Vito",
                                 "Viva",
                                 "Vivan",
                                 "Vivian",
                                 "Viviana",
                                 "Vivien",
                                 "Vivienne",
                                 "Von",
                                 "Voncile",
                                 "Vonda",
                                 "Vonnie",
                                 "Wade",
                                 "Wai",
                                 "Waldo",
                                 "Walker",
                                 "Wallace",
                                 "Wally",
                                 "Walter",
                                 "Walter",
                                 "Walton",
                                 "Waltraud",
                                 "Wan",
                                 "Wanda",
                                 "Waneta",
                                 "Wanetta",
                                 "Wanita",
                                 "Ward",
                                 "Warner",
                                 "Warren",
                                 "Wava",
                                 "Waylon",
                                 "Wayne",
                                 "Wei",
                                 "Weldon",
                                 "Wen",
                                 "Wendell",
                                 "Wendi",
                                 "Wendie",
                                 "Wendolyn",
                                 "Wendy",
                                 "Wenona",
                                 "Werner",
                                 "Wes",
                                 "Wesley",
                                 "Wesley",
                                 "Weston",
                                 "Whitley",
                                 "Whitney",
                                 "Whitney",
                                 "Wilber",
                                 "Wilbert",
                                 "Wilbur",
                                 "Wilburn",
                                 "Wilda",
                                 "Wiley",
                                 "Wilford",
                                 "Wilfred",
                                 "Wilfredo",
                                 "Wilhelmina",
                                 "Wilhemina",
                                 "Will",
                                 "Willa",
                                 "Willard",
                                 "Willena",
                                 "Willene",
                                 "Willetta",
                                 "Willette",
                                 "Willia",
                                 "William",
                                 "William",
                                 "Williams",
                                 "Willian",
                                 "Willie",
                                 "Willie",
                                 "Williemae",
                                 "Willis",
                                 "Willodean",
                                 "Willow",
                                 "Willy",
                                 "Wilma",
                                 "Wilmer",
                                 "Wilson",
                                 "Wilton",
                                 "Windy",
                                 "Winford",
                                 "Winfred",
                                 "Winifred",
                                 "Winnie",
                                 "Winnifred",
                                 "Winona",
                                 "Winston",
                                 "Winter",
                                 "Wm",
                                 "Wonda",
                                 "Woodrow",
                                 "Wyatt",
                                 "Wynell",
                                 "Wynona",
                                 "Xavier",
                                 "Xenia",
                                 "Xiao",
                                 "Xiomara",
                                 "Xochitl",
                                 "Xuan",
                                 "Yadira",
                                 "Yaeko",
                                 "Yael",
                                 "Yahaira",
                                 "Yajaira",
                                 "Yan",
                                 "Yang",
                                 "Yanira",
                                 "Yasmin",
                                 "Yasmine",
                                 "Yasuko",
                                 "Yee",
                                 "Yelena",
                                 "Yen",
                                 "Yer",
                                 "Yesenia",
                                 "Yessenia",
                                 "Yetta",
                                 "Yevette",
                                 "Yi",
                                 "Ying",
                                 "Yoko",
                                 "Yolanda",
                                 "Yolande",
                                 "Yolando",
                                 "Yolonda",
                                 "Yon",
                                 "Yong",
                                 "Yong",
                                 "Yoshie",
                                 "Yoshiko",
                                 "Youlanda",
                                 "Young",
                                 "Young",
                                 "Yu",
                                 "Yuette",
                                 "Yuk",
                                 "Yuki",
                                 "Yukiko",
                                 "Yuko",
                                 "Yulanda",
                                 "Yun",
                                 "Yung",
                                 "Yuonne",
                                 "Yuri",
                                 "Yuriko",
                                 "Yvette",
                                 "Yvone",
                                 "Yvonne",
                                 "Zachariah",
                                 "Zachary",
                                 "Zachery",
                                 "Zack",
                                 "Zackary",
                                 "Zada",
                                 "Zaida",
                                 "Zana",
                                 "Zandra",
                                 "Zane",
                                 "Zelda",
                                 "Zella",
                                 "Zelma",
                                 "Zena",
                                 "Zenaida",
                                 "Zenia",
                                 "Zenobia",
                                 "Zetta",
                                 "Zina",
                                 "Zita",
                                 "Zoe",
                                 "Zofia",
                                 "Zoila",
                                 "Zola",
                                 "Zona",
                                 "Zonia",
                                 "Zora",
                                 "Zoraida",
                                 "Zula",
                                 "Zulema",

                                 #endregion
                             };
            var index = rand.Next(1, firstNames.Count());
            return firstNames[index];
        }

        public string GetFakeChineseFirstName()
        {
            
            var firstNames = new[]
                             {
                                 #region FirstNames
                                 "陈",
"郑",
"志",
"丛",
"德威",
"辉",
"广",
"何",
"六月",
"香港",
"观音",
"背风处",
"花环",
"里",
"廉",
"梁",
"乐",
"长",
"上",
"公园",
"沉",
"盛",
"盛",
"叶",
"一个尤伯",
"婵娟",
"张",
"丛",
"大夏",
"方",
"方贤",
"分",
"丰",
"还吁屙",
"惠英",
"李嘉",
"李江",
"角",
"斤",
"京",
"胡安",
"六月",
"背风处",
"花环",
"里",
"李华",
"李月",
"李明",
"李娜",
"李沁",
"李荣",
"李伟",
"廉",
"留置权",
"林",
"林耀",
"凌",
"理学",
"五月",
"花木兰",
"公园",
"平安",
"齐",
"巧",
"清远",
"荣",
"山",
"舒放",
"爽",
"婷",
"苍白",
"伟",
"夏",
"何霞",
"翔",
"萧晨",
"萧红",
"钱欣",
"秀",
"修娟",
"薛",
"薛芳",
"严",
"艳艳",
"易",
"忆捷",
"易敏",
"易泽",
"余杰",
"渔湾",
"悦颜",
"月盈",
"乐你",
"震",
"甄车广",
"志",
"字"

                                 #endregion
                             };
            var index = rand.Next(1, firstNames.Count());
            return firstNames[index];
        }

        public string GetFakeLastName()
        {
            
            var lastNames = new[]
                            {
                                #region LastNames
                                "Aaron",
                                "Abbott",
                                "Abel",
                                "Abell",
                                "Abernathy",
                                "Abney",
                                "Abraham",
                                "Abrams",
                                "Abreu",
                                "Acevedo",
                                "Acker",
                                "Ackerman",
                                "Acosta",
                                "Acuna",
                                "Adair",
                                "Adam",
                                "Adame",
                                "Adams",
                                "Adamson",
                                "Adcock",
                                "Addison",
                                "Adkins",
                                "Adler",
                                "Agee",
                                "Agnew",
                                "Aguiar",
                                "Aguilar",
                                "Aguilera",
                                "Aguirre",
                                "Ahern",
                                "Ahmed",
                                "Ahrens",
                                "Aiello",
                                "Aiken",
                                "Ainsworth",
                                "Akers",
                                "Akin",
                                "Akins",
                                "Alaniz",
                                "Alarcon",
                                "Alba",
                                "Albers",
                                "Albert",
                                "Albertson",
                                "Albrecht",
                                "Albright",
                                "Alcala",
                                "Alcorn",
                                "Alderman",
                                "Aldrich",
                                "Aldridge",
                                "Aleman",
                                "Alexander",
                                "Alfaro",
                                "Alford",
                                "Alger",
                                "Ali",
                                "Alicea",
                                "Allan",
                                "Allard",
                                "Allen",
                                "Alley",
                                "Allison",
                                "Allman",
                                "Allred",
                                "Almanza",
                                "Almeida",
                                "Almond",
                                "Alonso",
                                "Alonzo",
                                "Alston",
                                "Altman",
                                "Alvarado",
                                "Alvarez",
                                "Alves",
                                "Amador",
                                "Amaral",
                                "Amato",
                                "Amaya",
                                "Ambrose",
                                "Ames",
                                "Ammons",
                                "Amos",
                                "Anaya",
                                "Anders",
                                "Andersen",
                                "Anderson",
                                "Andrade",
                                "Andres",
                                "Andrew",
                                "Andrews",
                                "Andrus",
                                "Angel",
                                "Anglin",
                                "Angulo",
                                "Anthony",
                                "Antonio",
                                "Apodaca",
                                "Aponte",
                                "Applegate",
                                "Aquino",
                                "Aragon",
                                "Aranda",
                                "Arce",
                                "Archer",
                                "Archibald",
                                "Archie",
                                "Archuleta",
                                "Arellano",
                                "Arevalo",
                                "Arias",
                                "Armijo",
                                "Armstead",
                                "Armstrong",
                                "Arndt",
                                "Arnett",
                                "Arnold",
                                "Arredondo",
                                "Arreola",
                                "Arriaga",
                                "Arrington",
                                "Arroyo",
                                "Arsenault",
                                "Arteaga",
                                "Arthur",
                                "Artis",
                                "Asbury",
                                "Ash",
                                "Ashby",
                                "Ashcraft",
                                "Ashe",
                                "Asher",
                                "Ashford",
                                "Ashley",
                                "Ashton",
                                "Ashworth",
                                "Askew",
                                "Atchison",
                                "Atherton",
                                "Atkins",
                                "Atkinson",
                                "Atwell",
                                "Atwood",
                                "August",
                                "Augustine",
                                "Austin",
                                "Autry",
                                "Avalos",
                                "Avery",
                                "Avila",
                                "Aviles",
                                "Ayala",
                                "Ayers",
                                "Ayres",
                                "Babb",
                                "Babcock",
                                "Babin",
                                "Baca",
                                "Bach",
                                "Bachman",
                                "Back",
                                "Bacon",
                                "Bader",
                                "Badger",
                                "Baer",
                                "Baez",
                                "Baggett",
                                "Bagley",
                                "Bagwell",
                                "Bailey",
                                "Bain",
                                "Baines",
                                "Bair",
                                "Baird",
                                "Baker",
                                "Balderas",
                                "Baldwin",
                                "Bales",
                                "Ball",
                                "Ballard",
                                "Banda",
                                "Bandy",
                                "Banks",
                                "Bankston",
                                "Bannister",
                                "Banuelos",
                                "Baptiste",
                                "Barajas",
                                "Barbee",
                                "Barber",
                                "Barbosa",
                                "Barbour",
                                "Barclay",
                                "Barfield",
                                "Barger",
                                "Barker",
                                "Barkley",
                                "Barksdale",
                                "Barlow",
                                "Barnard",
                                "Barnes",
                                "Barnett",
                                "Barnette",
                                "Barney",
                                "Barnhart",
                                "Barnhill",
                                "Baron",
                                "Barone",
                                "Barr",
                                "Barraza",
                                "Barrera",
                                "Barrett",
                                "Barrios",
                                "Barron",
                                "Barrow",
                                "Barrows",
                                "Barry",
                                "Bartels",
                                "Barth",
                                "Bartholomew",
                                "Bartlett",
                                "Bartley",
                                "Barton",
                                "Basham",
                                "Bass",
                                "Bassett",
                                "Batchelor",
                                "Bateman",
                                "Bates",
                                "Batista",
                                "Batiste",
                                "Batson",
                                "Battaglia",
                                "Batten",
                                "Battle",
                                "Battles",
                                "Batts",
                                "Bauer",
                                "Baugh",
                                "Baughman",
                                "Baum",
                                "Bauman",
                                "Baumann",
                                "Baumgartner",
                                "Bautista",
                                "Baxter",
                                "Bayer",
                                "Baylor",
                                "Beach",
                                "Beal",
                                "Beale",
                                "Beall",
                                "Beals",
                                "Beam",
                                "Bean",
                                "Beard",
                                "Bearden",
                                "Beasley",
                                "Beattie",
                                "Beatty",
                                "Beaty",
                                "Beauchamp",
                                "Beaudoin",
                                "Beaulieu",
                                "Beauregard",
                                "Beaver",
                                "Beavers",
                                "Becerra",
                                "Beck",
                                "Becker",
                                "Beckett",
                                "Beckham",
                                "Beckman",
                                "Beckwith",
                                "Becnel",
                                "Bedard",
                                "Bedford",
                                "Beebe",
                                "Beeler",
                                "Beers",
                                "Begay",
                                "Begley",
                                "Behrens",
                                "Belanger",
                                "Belcher",
                                "Bell",
                                "Bellamy",
                                "Bello",
                                "Belt",
                                "Belton",
                                "Beltran",
                                "Benavides",
                                "Benavidez",
                                "Bender",
                                "Benedict",
                                "Benefield",
                                "Benitez",
                                "Benjamin",
                                "Benner",
                                "Bennett",
                                "Benoit",
                                "Benson",
                                "Bentley",
                                "Benton",
                                "Berg",
                                "Berger",
                                "Bergeron",
                                "Bergman",
                                "Bergstrom",
                                "Berlin",
                                "Berman",
                                "Bermudez",
                                "Bernal",
                                "Bernard",
                                "Bernier",
                                "Bernstein",
                                "Berrios",
                                "Berry",
                                "Berryman",
                                "Bertram",
                                "Bertrand",
                                "Berube",
                                "Bess",
                                "Best",
                                "Betancourt",
                                "Bethea",
                                "Bethel",
                                "Betts",
                                "Betz",
                                "Beverly",
                                "Beyer",
                                "Bible",
                                "Bickford",
                                "Biddle",
                                "Bigelow",
                                "Biggs",
                                "Billings",
                                "Billingsley",
                                "Bills",
                                "Billups",
                                "Binder",
                                "Bingham",
                                "Binkley",
                                "Birch",
                                "Bird",
                                "Bishop",
                                "Bisson",
                                "Bittner",
                                "Bivens",
                                "Bivins",
                                "Black",
                                "Blackburn",
                                "Blackman",
                                "Blackmon",
                                "Blackwell",
                                "Blackwood",
                                "Blaine",
                                "Blair",
                                "Blais",
                                "Blake",
                                "Blakely",
                                "Blalock",
                                "Blanchard",
                                "Blanchette",
                                "Blanco",
                                "Bland",
                                "Blank",
                                "Blankenship",
                                "Blanton",
                                "Blaylock",
                                "Bledsoe",
                                "Blevins",
                                "Bliss",
                                "Block",
                                "Blocker",
                                "Blodgett",
                                "Bloom",
                                "Blount",
                                "Blue",
                                "Blum",
                                "Blunt",
                                "Blythe",
                                "Boatwright",
                                "Bobbitt",
                                "Bobo",
                                "Bock",
                                "Boehm",
                                "Bogan",
                                "Boggs",
                                "Bohannon",
                                "Bohn",
                                "Boisvert",
                                "Boland",
                                "Bolden",
                                "Bolduc",
                                "Bolen",
                                "Boles",
                                "Bolin",
                                "Boling",
                                "Bollinger",
                                "Bolt",
                                "Bolton",
                                "Bond",
                                "Bonds",
                                "Bone",
                                "Bonilla",
                                "Bonner",
                                "Booker",
                                "Boone",
                                "Booth",
                                "Boothe",
                                "Bordelon",
                                "Borden",
                                "Borders",
                                "Boren",
                                "Borges",
                                "Boss",
                                "Bostic",
                                "Bostick",
                                "Boston",
                                "Boswell",
                                "Bottoms",
                                "Bouchard",
                                "Boucher",
                                "Boudreau",
                                "Boudreaux",
                                "Bounds",
                                "Bourgeois",
                                "Bourne",
                                "Bourque",
                                "Bowden",
                                "Bowen",
                                "Bowens",
                                "Bower",
                                "Bowers",
                                "Bowie",
                                "Bowles",
                                "Bowlin",
                                "Bowling",
                                "Bowman",
                                "Bowser",
                                "Box",
                                "Boyce",
                                "Boyd",
                                "Boyer",
                                "Boykin",
                                "Boyle",
                                "Boyles",
                                "Boynton",
                                "Bozeman",
                                "Brackett",
                                "Bradbury",
                                "Braden",
                                "Bradford",
                                "Bradley",
                                "Bradshaw",
                                "Brady",
                                "Bragg",
                                "Branch",
                                "Brand",
                                "Brandenburg",
                                "Brandon",
                                "Brandt",
                                "Branham",
                                "Brannon",
                                "Branson",
                                "Brant",
                                "Brantley",
                                "Braswell",
                                "Bratcher",
                                "Bratton",
                                "Braun",
                                "Bravo",
                                "Braxton",
                                "Bray",
                                "Breaux",
                                "Breeden",
                                "Breedlove",
                                "Breen",
                                "Brennan",
                                "Brenner",
                                "Brent",
                                "Brewer",
                                "Brewster",
                                "Brice",
                                "Bridges",
                                "Briggs",
                                "Bright",
                                "Brink",
                                "Brinkley",
                                "Brinkman",
                                "Brinson",
                                "Briones",
                                "Briscoe",
                                "Brito",
                                "Britt",
                                "Brittain",
                                "Britton",
                                "Broadway",
                                "Brock",
                                "Brockman",
                                "Broderick",
                                "Brogan",
                                "Bronson",
                                "Brooks",
                                "Broome",
                                "Brothers",
                                "Broughton",
                                "Broussard",
                                "Browder",
                                "Brower",
                                "Brown",
                                "Browne",
                                "Browning",
                                "Brownlee",
                                "Broyles",
                                "Brubaker",
                                "Bruce",
                                "Brumfield",
                                "Bruner",
                                "Brunner",
                                "Bruno",
                                "Bruns",
                                "Brunson",
                                "Bruton",
                                "Bryan",
                                "Bryant",
                                "Bryson",
                                "Buchanan",
                                "Buck",
                                "Buckingham",
                                "Buckley",
                                "Buckner",
                                "Bueno",
                                "Buffington",
                                "Buford",
                                "Bui",
                                "Bull",
                                "Bullard",
                                "Bullock",
                                "Bumgarner",
                                "Bunch",
                                "Bundy",
                                "Bunker",
                                "Bunn",
                                "Bunnell",
                                "Bunting",
                                "Burch",
                                "Burchett",
                                "Burchfield",
                                "Burden",
                                "Burdette",
                                "Burdick",
                                "Burge",
                                "Burger",
                                "Burgess",
                                "Burgos",
                                "Burk",
                                "Burke",
                                "Burkett",
                                "Burkhart",
                                "Burkholder",
                                "Burks",
                                "Burleson",
                                "Burley",
                                "Burnett",
                                "Burnette",
                                "Burney",
                                "Burnham",
                                "Burns",
                                "Burnside",
                                "Burr",
                                "Burrell",
                                "Burris",
                                "Burroughs",
                                "Burrow",
                                "Burrows",
                                "Burt",
                                "Burton",
                                "Busby",
                                "Busch",
                                "Bush",
                                "Buss",
                                "Bussey",
                                "Bustamante",
                                "Bustos",
                                "Butcher",
                                "Butler",
                                "Butterfield",
                                "Button",
                                "Butts",
                                "Byars",
                                "Byers",
                                "Bynum",
                                "Byrd",
                                "Byrne",
                                "Byrnes",
                                "Caballero",
                                "Cable",
                                "Cabral",
                                "Cabrera",
                                "Cade",
                                "Cady",
                                "Cagle",
                                "Cahill",
                                "Cain",
                                "Calderon",
                                "Caldwell",
                                "Calhoun",
                                "Calkins",
                                "Call",
                                "Callahan",
                                "Callaway",
                                "Callender",
                                "Calloway",
                                "Calvert",
                                "Calvin",
                                "Camacho",
                                "Camarillo",
                                "Cambell",
                                "Cameron",
                                "Camp",
                                "Campbell",
                                "Campos",
                                "Canada",
                                "Canady",
                                "Canales",
                                "Candelaria",
                                "Canfield",
                                "Cannon",
                                "Cano",
                                "Cantrell",
                                "Cantu",
                                "Cantwell",
                                "Canty",
                                "Capps",
                                "Caraballo",
                                "Carbajal",
                                "Carbone",
                                "Card",
                                "Carden",
                                "Cardenas",
                                "Carder",
                                "Cardona",
                                "Cardoza",
                                "Cardwell",
                                "Carey",
                                "Carl",
                                "Carlin",
                                "Carlisle",
                                "Carlos",
                                "Carlson",
                                "Carlton",
                                "Carman",
                                "Carmichael",
                                "Carmona",
                                "Carnahan",
                                "Carnes",
                                "Carney",
                                "Caro",
                                "Caron",
                                "Carpenter",
                                "Carr",
                                "Carranza",
                                "Carrasco",
                                "Carrier",
                                "Carrillo",
                                "Carrington",
                                "Carrion",
                                "Carroll",
                                "Carson",
                                "Carswell",
                                "Carter",
                                "Cartwright",
                                "Caruso",
                                "Carvalho",
                                "Carver",
                                "Cary",
                                "Casas",
                                "Case",
                                "Casey",
                                "Cash",
                                "Casillas",
                                "Caskey",
                                "Cason",
                                "Casper",
                                "Cass",
                                "Cassidy",
                                "Castaneda",
                                "Casteel",
                                "Castellano",
                                "Castellanos",
                                "Castillo",
                                "Castle",
                                "Castleberry",
                                "Castro",
                                "Caswell",
                                "Catalano",
                                "Cates",
                                "Cathey",
                                "Cato",
                                "Catron",
                                "Caudill",
                                "Caudle",
                                "Causey",
                                "Cavanaugh",
                                "Cavazos",
                                "Cave",
                                "Cecil",
                                "Centeno",
                                "Cerda",
                                "Cervantes",
                                "Chacon",
                                "Chadwick",
                                "Chaffin",
                                "Chalmers",
                                "Chamberlain",
                                "Chamberlin",
                                "Chambers",
                                "Champagne",
                                "Champion",
                                "Chan",
                                "Chance",
                                "Chandler",
                                "Chaney",
                                "Chang",
                                "Chapa",
                                "Chapin",
                                "Chapman",
                                "Chappell",
                                "Charles",
                                "Charlton",
                                "Chase",
                                "Chastain",
                                "Chatman",
                                "Chau",
                                "Chavarria",
                                "Chavez",
                                "Chavis",
                                "Cheatham",
                                "Cheek",
                                "Chen",
                                "Cheney",
                                "Cheng",
                                "Cherry",
                                "Chesser",
                                "Chester",
                                "Chestnut",
                                "Cheung",
                                "Childers",
                                "Childress",
                                "Childs",
                                "Chilton",
                                "Chin",
                                "Chisholm",
                                "Chism",
                                "Chisolm",
                                "Cho",
                                "Choate",
                                "Choi",
                                "Chong",
                                "Chow",
                                "Christensen",
                                "Christenson",
                                "Christian",
                                "Christiansen",
                                "Christianson",
                                "Christie",
                                "Christman",
                                "Christopher",
                                "Christy",
                                "Chu",
                                "Chun",
                                "Chung",
                                "Church",
                                "Churchill",
                                "Cintron",
                                "Cisneros",
                                "Clancy",
                                "Clanton",
                                "Clapp",
                                "Clark",
                                "Clarke",
                                "Clarkson",
                                "Clary",
                                "Clawson",
                                "Clay",
                                "Clayton",
                                "Cleary",
                                "Clegg",
                                "Clem",
                                "Clemens",
                                "Clement",
                                "Clements",
                                "Clemmons",
                                "Clemons",
                                "Cleveland",
                                "Clevenger",
                                "Click",
                                "Clifford",
                                "Clifton",
                                "Cline",
                                "Clinton",
                                "Close",
                                "Cloud",
                                "Clough",
                                "Cloutier",
                                "Coates",
                                "Coats",
                                "Cobb",
                                "Cobbs",
                                "Coble",
                                "Coburn",
                                "Cochran",
                                "Cochrane",
                                "Cockrell",
                                "Cody",
                                "Coe",
                                "Coffey",
                                "Coffin",
                                "Coffman",
                                "Cohen",
                                "Cohn",
                                "Coker",
                                "Colbert",
                                "Colburn",
                                "Colby",
                                "Cole",
                                "Coleman",
                                "Coles",
                                "Coley",
                                "Collazo",
                                "Colley",
                                "Collier",
                                "Collins",
                                "Colon",
                                "Colson",
                                "Colvin",
                                "Colwell",
                                "Combs",
                                "Comer",
                                "Compton",
                                "Comstock",
                                "Conaway",
                                "Concepcion",
                                "Condon",
                                "Cone",
                                "Conger",
                                "Conklin",
                                "Conley",
                                "Conn",
                                "Connell",
                                "Connelly",
                                "Conner",
                                "Conners",
                                "Connolly",
                                "Connor",
                                "Connors",
                                "Conover",
                                "Conrad",
                                "Conroy",
                                "Conti",
                                "Contreras",
                                "Conway",
                                "Conyers",
                                "Cook",
                                "Cooke",
                                "Cooks",
                                "Cooley",
                                "Coombs",
                                "Coon",
                                "Cooney",
                                "Coons",
                                "Cooper",
                                "Cope",
                                "Copeland",
                                "Copley",
                                "Corbett",
                                "Corbin",
                                "Corbitt",
                                "Corcoran",
                                "Cordell",
                                "Cordero",
                                "Cordova",
                                "Corey",
                                "Corley",
                                "Cormier",
                                "Cornelius",
                                "Cornell",
                                "Cornett",
                                "Cornish",
                                "Cornwell",
                                "Corona",
                                "Coronado",
                                "Corral",
                                "Correa",
                                "Correia",
                                "Corrigan",
                                "Cortes",
                                "Cortez",
                                "Cosby",
                                "Cosgrove",
                                "Costa",
                                "Costello",
                                "Cota",
                                "Cote",
                                "Cotter",
                                "Cotton",
                                "Cottrell",
                                "Couch",
                                "Coughlin",
                                "Coulter",
                                "Council",
                                "Counts",
                                "Courtney",
                                "Cousins",
                                "Couture",
                                "Covert",
                                "Covey",
                                "Covington",
                                "Cowan",
                                "Coward",
                                "Cowart",
                                "Cox",
                                "Coy",
                                "Coyle",
                                "Coyne",
                                "Crabtree",
                                "Craddock",
                                "Craft",
                                "Craig",
                                "Crain",
                                "Cramer",
                                "Crandall",
                                "Crane",
                                "Cranford",
                                "Craven",
                                "Crawford",
                                "Crawley",
                                "Crayton",
                                "Creamer",
                                "Creech",
                                "Creel",
                                "Creighton",
                                "Crenshaw",
                                "Crespo",
                                "Crews",
                                "Crider",
                                "Crisp",
                                "Crist",
                                "Criswell",
                                "Crittenden",
                                "Crocker",
                                "Crockett",
                                "Croft",
                                "Cromer",
                                "Cromwell",
                                "Cronin",
                                "Crook",
                                "Crooks",
                                "Crosby",
                                "Cross",
                                "Croteau",
                                "Crouch",
                                "Crouse",
                                "Crow",
                                "Crowder",
                                "Crowe",
                                "Crowell",
                                "Crowley",
                                "Crum",
                                "Crump",
                                "Cruse",
                                "Crutcher",
                                "Crutchfield",
                                "Cruz",
                                "Cuellar",
                                "Cuevas",
                                "Culbertson",
                                "Cullen",
                                "Culp",
                                "Culpepper",
                                "Culver",
                                "Cummings",
                                "Cummins",
                                "Cunningham",
                                "Cupp",
                                "Curley",
                                "Curran",
                                "Currie",
                                "Currier",
                                "Curry",
                                "Curtin",
                                "Curtis",
                                "Cushman",
                                "Custer",
                                "Cutler",
                                "Cyr",
                                "Dahl",
                                "Daigle",
                                "Dailey",
                                "Daily",
                                "Dale",
                                "Daley",
                                "Dallas",
                                "Dalton",
                                "Daly",
                                "Damico",
                                "Damron",
                                "Dancy",
                                "Dang",
                                "Dangelo",
                                "Daniel",
                                "Daniels",
                                "Danielson",
                                "Danner",
                                "Darby",
                                "Darden",
                                "Darling",
                                "Darnell",
                                "Dasilva",
                                "Daugherty",
                                "Davenport",
                                "David",
                                "Davidson",
                                "Davies",
                                "Davila",
                                "Davis",
                                "Davison",
                                "Dawkins",
                                "Dawson",
                                "Day",
                                "Dayton",
                                "Deal",
                                "Dean",
                                "Deaton",
                                "Decker",
                                "Dees",
                                "Dehart",
                                "Dejesus",
                                "Delacruz",
                                "Delagarza",
                                "Delaney",
                                "Delarosa",
                                "Delatorre",
                                "Deleon",
                                "Delgadillo",
                                "Delgado",
                                "Dell",
                                "Dellinger",
                                "Deloach",
                                "Delong",
                                "Delossantos",
                                "Deluca",
                                "Delvalle",
                                "Demarco",
                                "Demers",
                                "Dempsey",
                                "Denham",
                                "Denney",
                                "Denning",
                                "Dennis",
                                "Dennison",
                                "Denny",
                                "Denson",
                                "Dent",
                                "Denton",
                                "Derr",
                                "Derrick",
                                "Desantis",
                                "Devine",
                                "Devito",
                                "Devlin",
                                "Devore",
                                "Devries",
                                "Dew",
                                "Dewey",
                                "Dewitt",
                                "Dexter",
                                "Dial",
                                "Diamond",
                                "Dias",
                                "Diaz",
                                "Dick",
                                "Dickens",
                                "Dickerson",
                                "Dickey",
                                "Dickinson",
                                "Dickson",
                                "Diehl",
                                "Dietrich",
                                "Dietz",
                                "Diggs",
                                "Dill",
                                "Dillard",
                                "Dillon",
                                "Dion",
                                "Dix",
                                "Dixon",
                                "Do",
                                "Doan",
                                "Dobbins",
                                "Dobbs",
                                "Dobson",
                                "Dockery",
                                "Dodd",
                                "Dodge",
                                "Dodson",
                                "Doe",
                                "Doherty",
                                "Dolan",
                                "Doll",
                                "Dollar",
                                "Domingo",
                                "Dominguez",
                                "Donahue",
                                "Donald",
                                "Donaldson",
                                "Donnell",
                                "Donnelly",
                                "Donohue",
                                "Donovan",
                                "Dooley",
                                "Doran",
                                "Dorman",
                                "Dorn",
                                "Dorris",
                                "Dorsey",
                                "Dortch",
                                "Doss",
                                "Dotson",
                                "Doty",
                                "Dougherty",
                                "Doughty",
                                "Douglas",
                                "Douglass",
                                "Dove",
                                "Dover",
                                "Dow",
                                "Dowd",
                                "Dowdy",
                                "Dowell",
                                "Dowling",
                                "Downey",
                                "Downing",
                                "Downs",
                                "Doyle",
                                "Dozier",
                                "Drake",
                                "Draper",
                                "Drayton",
                                "Drew",
                                "Driscoll",
                                "Driver",
                                "Drummond",
                                "Drury",
                                "Duarte",
                                "Dube",
                                "Dubois",
                                "Dubose",
                                "Duckett",
                                "Duckworth",
                                "Dudley",
                                "Duff",
                                "Duffy",
                                "Dugan",
                                "Duggan",
                                "Dugger",
                                "Duke",
                                "Dukes",
                                "Dumas",
                                "Dunaway",
                                "Dunbar",
                                "Duncan",
                                "Dunham",
                                "Dunlap",
                                "Dunn",
                                "Dunne",
                                "Dunning",
                                "Duong",
                                "Dupont",
                                "Dupre",
                                "Dupree",
                                "Dupuis",
                                "Duran",
                                "Durant",
                                "Durbin",
                                "Durden",
                                "Durham",
                                "Durr",
                                "Dutton",
                                "Duval",
                                "Duvall",
                                "Dwyer",
                                "Dye",
                                "Dyer",
                                "Dykes",
                                "Dyson",
                                "Eagle",
                                "Earl",
                                "Earle",
                                "Earley",
                                "Early",
                                "Easley",
                                "Eason",
                                "East",
                                "Easter",
                                "Easterling",
                                "Eastman",
                                "Easton",
                                "Eaton",
                                "Eaves",
                                "Ebert",
                                "Echevarria",
                                "Echols",
                                "Eckert",
                                "Eddy",
                                "Edgar",
                                "Edge",
                                "Edmond",
                                "Edmonds",
                                "Edmondson",
                                "Edward",
                                "Edwards",
                                "Egan",
                                "Eggleston",
                                "Elam",
                                "Elder",
                                "Eldridge",
                                "Elias",
                                "Elizondo",
                                "Elkins",
                                "Eller",
                                "Ellington",
                                "Elliot",
                                "Elliott",
                                "Ellis",
                                "Ellison",
                                "Ellsworth",
                                "Elmore",
                                "Elrod",
                                "Elston",
                                "Ely",
                                "Emanuel",
                                "Embry",
                                "Emerson",
                                "Emery",
                                "Emmons",
                                "Eng",
                                "Engel",
                                "England",
                                "Engle",
                                "English",
                                "Ennis",
                                "Enos",
                                "Enriquez",
                                "Epperson",
                                "Epps",
                                "Epstein",
                                "Erickson",
                                "Ernst",
                                "Ervin",
                                "Erwin",
                                "Escalante",
                                "Escamilla",
                                "Escobar",
                                "Escobedo",
                                "Esparza",
                                "Espinal",
                                "Espino",
                                "Espinosa",
                                "Espinoza",
                                "Esposito",
                                "Esquivel",
                                "Estep",
                                "Estes",
                                "Estrada",
                                "Estrella",
                                "Etheridge",
                                "Eubanks",
                                "Evans",
                                "Everett",
                                "Everhart",
                                "Evers",
                                "Ewing",
                                "Ezell",
                                "Faber",
                                "Fagan",
                                "Fahey",
                                "Fain",
                                "Fair",
                                "Fairbanks",
                                "Fairchild",
                                "Fairley",
                                "Faison",
                                "Fajardo",
                                "Falcon",
                                "Falk",
                                "Fallon",
                                "Falls",
                                "Fanning",
                                "Farias",
                                "Farley",
                                "Farmer",
                                "Farnsworth",
                                "Farr",
                                "Farrar",
                                "Farrell",
                                "Farrington",
                                "Farris",
                                "Farrow",
                                "Faulk",
                                "Faulkner",
                                "Faust",
                                "Fay",
                                "Feeney",
                                "Felder",
                                "Feldman",
                                "Feliciano",
                                "Felix",
                                "Felton",
                                "Fennell",
                                "Fenner",
                                "Fenton",
                                "Ferguson",
                                "Fernandes",
                                "Fernandez",
                                "Ferrara",
                                "Ferraro",
                                "Ferreira",
                                "Ferrell",
                                "Ferrer",
                                "Ferris",
                                "Ferry",
                                "Field",
                                "Fielder",
                                "Fields",
                                "Fierro",
                                "Fife",
                                "Figueroa",
                                "Finch",
                                "Fincher",
                                "Findley",
                                "Fine",
                                "Fink",
                                "Finley",
                                "Finn",
                                "Finnegan",
                                "Finney",
                                "Fiore",
                                "Fischer",
                                "Fish",
                                "Fisher",
                                "Fishman",
                                "Fisk",
                                "Fitch",
                                "Fitts",
                                "Fitzgerald",
                                "Fitzpatrick",
                                "Fitzsimmons",
                                "Flagg",
                                "Flaherty",
                                "Flanagan",
                                "Flanders",
                                "Flannery",
                                "Fleck",
                                "Fleming",
                                "Flemming",
                                "Fletcher",
                                "Flint",
                                "Flood",
                                "Florence",
                                "Flores",
                                "Florez",
                                "Flowers",
                                "Floyd",
                                "Flynn",
                                "Fogle",
                                "Foley",
                                "Folse",
                                "Folsom",
                                "Fong",
                                "Fonseca",
                                "Fontaine",
                                "Fontenot",
                                "Foote",
                                "Forbes",
                                "Ford",
                                "Foreman",
                                "Foret",
                                "Forman",
                                "Forney",
                                "Forrest",
                                "Forrester",
                                "Forsyth",
                                "Forsythe",
                                "Fort",
                                "Forte",
                                "Fortenberry",
                                "Fortier",
                                "Fortin",
                                "Fortner",
                                "Fortune",
                                "Foss",
                                "Foster",
                                "Fountain",
                                "Fournier",
                                "Foust",
                                "Fowler",
                                "Fox",
                                "Foy",
                                "Fraley",
                                "Frame",
                                "France",
                                "Francis",
                                "Francisco",
                                "Franco",
                                "Francois",
                                "Frank",
                                "Franklin",
                                "Franks",
                                "Frantz",
                                "Franz",
                                "Fraser",
                                "Frazer",
                                "Frazier",
                                "Frederick",
                                "Fredericks",
                                "Fredrickson",
                                "Free",
                                "Freed",
                                "Freedman",
                                "Freeman",
                                "Freitas",
                                "French",
                                "Freund",
                                "Frey",
                                "Frias",
                                "Frick",
                                "Friedman",
                                "Friend",
                                "Frierson",
                                "Fritz",
                                "Frizzell",
                                "Frost",
                                "Fry",
                                "Frye",
                                "Fryer",
                                "Fuchs",
                                "Fuentes",
                                "Fugate",
                                "Fulcher",
                                "Fuller",
                                "Fullerton",
                                "Fulmer",
                                "Fulton",
                                "Fultz",
                                "Funderburk",
                                "Funk",
                                "Fuqua",
                                "Furman",
                                "Furr",
                                "Fusco",
                                "Gable",
                                "Gabriel",
                                "Gaddis",
                                "Gaddy",
                                "Gaffney",
                                "Gage",
                                "Gagne",
                                "Gagnon",
                                "Gaines",
                                "Gaither",
                                "Galarza",
                                "Galbraith",
                                "Gale",
                                "Galindo",
                                "Gallagher",
                                "Gallant",
                                "Gallardo",
                                "Gallegos",
                                "Gallo",
                                "Galloway",
                                "Galvan",
                                "Galvez",
                                "Galvin",
                                "Gamble",
                                "Gamboa",
                                "Gamez",
                                "Gandy",
                                "Gann",
                                "Gannon",
                                "Gant",
                                "Gantt",
                                "Garber",
                                "Garcia",
                                "Gardiner",
                                "Gardner",
                                "Garland",
                                "Garmon",
                                "Garner",
                                "Garnett",
                                "Garrett",
                                "Garris",
                                "Garrison",
                                "Garvey",
                                "Garvin",
                                "Gary",
                                "Garza",
                                "Gaskin",
                                "Gaskins",
                                "Gass",
                                "Gaston",
                                "Gates",
                                "Gatewood",
                                "Gatlin",
                                "Gauthier",
                                "Gavin",
                                "Gay",
                                "Gaylord",
                                "Geary",
                                "Gee",
                                "Geer",
                                "Geiger",
                                "Gentile",
                                "Gentry",
                                "George",
                                "Gerald",
                                "Gerard",
                                "Gerber",
                                "German",
                                "Getz",
                                "Gibbons",
                                "Gibbs",
                                "Gibson",
                                "Gifford",
                                "Gil",
                                "Gilbert",
                                "Gilchrist",
                                "Giles",
                                "Gill",
                                "Gillen",
                                "Gillespie",
                                "Gillette",
                                "Gilley",
                                "Gilliam",
                                "Gilliland",
                                "Gillis",
                                "Gilman",
                                "Gilmore",
                                "Gilson",
                                "Ginn",
                                "Giordano",
                                "Gipson",
                                "Girard",
                                "Giron",
                                "Gist",
                                "Givens",
                                "Gladden",
                                "Gladney",
                                "Glaser",
                                "Glass",
                                "Gleason",
                                "Glenn",
                                "Glover",
                                "Glynn",
                                "Goad",
                                "Goble",
                                "Goddard",
                                "Godfrey",
                                "Godwin",
                                "Goebel",
                                "Goetz",
                                "Goff",
                                "Goforth",
                                "Goins",
                                "Gold",
                                "Goldberg",
                                "Golden",
                                "Goldman",
                                "Goldsmith",
                                "Goldstein",
                                "Gomes",
                                "Gomez",
                                "Gonzales",
                                "Gonzalez",
                                "Gooch",
                                "Good",
                                "Goode",
                                "Gooden",
                                "Goodman",
                                "Goodrich",
                                "Goodson",
                                "Goodwin",
                                "Gordon",
                                "Gore",
                                "Gorham",
                                "Gorman",
                                "Goss",
                                "Gossett",
                                "Gough",
                                "Gould",
                                "Goulet",
                                "Grace",
                                "Gracia",
                                "Grady",
                                "Graf",
                                "Graff",
                                "Graham",
                                "Granados",
                                "Granger",
                                "Grant",
                                "Grantham",
                                "Graves",
                                "Gray",
                                "Grayson",
                                "Greco",
                                "Green",
                                "Greenberg",
                                "Greene",
                                "Greenfield",
                                "Greenlee",
                                "Greenwood",
                                "Greer",
                                "Gregg",
                                "Gregory",
                                "Greiner",
                                "Grenier",
                                "Gresham",
                                "Grey",
                                "Grice",
                                "Grier",
                                "Griffin",
                                "Griffis",
                                "Griffith",
                                "Griffiths",
                                "Griggs",
                                "Grigsby",
                                "Grimes",
                                "Grimm",
                                "Grisham",
                                "Grissom",
                                "Griswold",
                                "Groce",
                                "Grogan",
                                "Grooms",
                                "Gross",
                                "Grossman",
                                "Grove",
                                "Grover",
                                "Groves",
                                "Grubb",
                                "Grubbs",
                                "Gruber",
                                "Guajardo",
                                "Guenther",
                                "Guerin",
                                "Guerra",
                                "Guerrero",
                                "Guest",
                                "Guevara",
                                "Guffey",
                                "Guidry",
                                "Guillen",
                                "Guillory",
                                "Guinn",
                                "Gulley",
                                "Gunderson",
                                "Gunn",
                                "Gunter",
                                "Gunther",
                                "Gurley",
                                "Gustafson",
                                "Guthrie",
                                "Gutierrez",
                                "Guy",
                                "Guyton",
                                "Guzman",
                                "Haag",
                                "Haas",
                                "Hacker",
                                "Hackett",
                                "Hackney",
                                "Hadley",
                                "Hagan",
                                "Hagen",
                                "Hager",
                                "Haggard",
                                "Haggerty",
                                "Hahn",
                                "Haight",
                                "Hailey",
                                "Haines",
                                "Hairston",
                                "Halcomb",
                                "Hale",
                                "Hales",
                                "Haley",
                                "Hall",
                                "Haller",
                                "Hallman",
                                "Halstead",
                                "Halverson",
                                "Ham",
                                "Hamblin",
                                "Hamby",
                                "Hamel",
                                "Hamer",
                                "Hamilton",
                                "Hamlin",
                                "Hamm",
                                "Hammer",
                                "Hammett",
                                "Hammond",
                                "Hammonds",
                                "Hammons",
                                "Hampton",
                                "Hamrick",
                                "Han",
                                "Hancock",
                                "Hand",
                                "Handley",
                                "Handy",
                                "Hanes",
                                "Haney",
                                "Hankins",
                                "Hanks",
                                "Hanley",
                                "Hanlon",
                                "Hanna",
                                "Hannah",
                                "Hannon",
                                "Hansen",
                                "Hanson",
                                "Harbin",
                                "Harden",
                                "Harder",
                                "Hardesty",
                                "Hardin",
                                "Harding",
                                "Hardison",
                                "Hardman",
                                "Hardwick",
                                "Hardy",
                                "Hare",
                                "Hargis",
                                "Hargrove",
                                "Harkins",
                                "Harlan",
                                "Harley",
                                "Harlow",
                                "Harman",
                                "Harmon",
                                "Harms",
                                "Harness",
                                "Harp",
                                "Harper",
                                "Harrell",
                                "Harrington",
                                "Harris",
                                "Harrison",
                                "Harry",
                                "Hart",
                                "Harter",
                                "Hartley",
                                "Hartman",
                                "Hartmann",
                                "Hartwell",
                                "Harvey",
                                "Harwell",
                                "Harwood",
                                "Haskell",
                                "Haskins",
                                "Hass",
                                "Hassell",
                                "Hastings",
                                "Hatch",
                                "Hatcher",
                                "Hatchett",
                                "Hatfield",
                                "Hathaway",
                                "Hatley",
                                "Hatton",
                                "Haugen",
                                "Hauser",
                                "Havens",
                                "Hawes",
                                "Hawk",
                                "Hawkins",
                                "Hawks",
                                "Hawley",
                                "Hawthorne",
                                "Hay",
                                "Hayden",
                                "Hayes",
                                "Haynes",
                                "Hays",
                                "Hayward",
                                "Haywood",
                                "Hazel",
                                "Head",
                                "Headley",
                                "Headrick",
                                "Healey",
                                "Healy",
                                "Heard",
                                "Hearn",
                                "Heath",
                                "Heaton",
                                "Hebert",
                                "Heck",
                                "Heckman",
                                "Hedges",
                                "Hedrick",
                                "Heffner",
                                "Heflin",
                                "Hefner",
                                "Heim",
                                "Hein",
                                "Heinz",
                                "Held",
                                "Heller",
                                "Helm",
                                "Helms",
                                "Helton",
                                "Hemphill",
                                "Henderson",
                                "Hendrick",
                                "Hendricks",
                                "Hendrickson",
                                "Hendrix",
                                "Henke",
                                "Henley",
                                "Henning",
                                "Henry",
                                "Hensley",
                                "Henson",
                                "Her",
                                "Herbert",
                                "Heredia",
                                "Herman",
                                "Hermann",
                                "Hernandez",
                                "Herndon",
                                "Herr",
                                "Herrera",
                                "Herrick",
                                "Herring",
                                "Herrington",
                                "Herrmann",
                                "Herron",
                                "Hershberger",
                                "Herzog",
                                "Hess",
                                "Hester",
                                "Hewitt",
                                "Hiatt",
                                "Hibbard",
                                "Hickey",
                                "Hickman",
                                "Hicks",
                                "Hickson",
                                "Hidalgo",
                                "Higdon",
                                "Higginbotham",
                                "Higgins",
                                "Higgs",
                                "High",
                                "Hightower",
                                "Hildebrand",
                                "Hildreth",
                                "Hill",
                                "Hilliard",
                                "Hillman",
                                "Hills",
                                "Hilton",
                                "Himes",
                                "Hindman",
                                "Hinds",
                                "Hines",
                                "Hinkle",
                                "Hinojosa",
                                "Hinson",
                                "Hinton",
                                "Hirsch",
                                "Hitchcock",
                                "Hite",
                                "Hitt",
                                "Ho",
                                "Hoang",
                                "Hobbs",
                                "Hobson",
                                "Hodge",
                                "Hodges",
                                "Hodgson",
                                "Hoff",
                                "Hoffman",
                                "Hoffmann",
                                "Hogan",
                                "Hogg",
                                "Hogue",
                                "Holbrook",
                                "Holcomb",
                                "Holcombe",
                                "Holden",
                                "Holder",
                                "Holguin",
                                "Holiday",
                                "Holland",
                                "Holley",
                                "Holliday",
                                "Hollingsworth",
                                "Hollins",
                                "Hollis",
                                "Holloman",
                                "Holloway",
                                "Holly",
                                "Holm",
                                "Holman",
                                "Holmes",
                                "Holt",
                                "Holton",
                                "Homan",
                                "Honeycutt",
                                "Hong",
                                "Hood",
                                "Hook",
                                "Hooker",
                                "Hooks",
                                "Hooper",
                                "Hoover",
                                "Hope",
                                "Hopkins",
                                "Hoppe",
                                "Hopper",
                                "Hopson",
                                "Horan",
                                "Horn",
                                "Horne",
                                "Horner",
                                "Hornsby",
                                "Horowitz",
                                "Horsley",
                                "Horton",
                                "Horvath",
                                "Hoskins",
                                "Hostetler",
                                "Houck",
                                "Hough",
                                "Houghton",
                                "Houle",
                                "House",
                                "Houser",
                                "Houston",
                                "Howard",
                                "Howe",
                                "Howell",
                                "Howerton",
                                "Howland",
                                "Hoy",
                                "Hoyle",
                                "Hoyt",
                                "Hsu",
                                "Huang",
                                "Hubbard",
                                "Huber",
                                "Hubert",
                                "Huddleston",
                                "Hudgins",
                                "Hudson",
                                "Huerta",
                                "Huey",
                                "Huff",
                                "Huffman",
                                "Huggins",
                                "Hughes",
                                "Hughey",
                                "Hull",
                                "Hulsey",
                                "Humes",
                                "Hummel",
                                "Humphrey",
                                "Humphreys",
                                "Humphries",
                                "Hundley",
                                "Hunt",
                                "Hunter",
                                "Huntington",
                                "Huntley",
                                "Hurd",
                                "Hurley",
                                "Hurst",
                                "Hurt",
                                "Hurtado",
                                "Huskey",
                                "Hussey",
                                "Huston",
                                "Hutchens",
                                "Hutcherson",
                                "Hutcheson",
                                "Hutchings",
                                "Hutchins",
                                "Hutchinson",
                                "Hutchison",
                                "Hutson",
                                "Hutto",
                                "Hutton",
                                "Huynh",
                                "Hwang",
                                "Hyatt",
                                "Hyde",
                                "Hyman",
                                "Hynes",
                                "Ibarra",
                                "Ingle",
                                "Ingram",
                                "Inman",
                                "Irby",
                                "Ireland",
                                "Irish",
                                "Irizarry",
                                "Irvin",
                                "Irvine",
                                "Irving",
                                "Irwin",
                                "Isaac",
                                "Isaacs",
                                "Isaacson",
                                "Isbell",
                                "Isom",
                                "Ison",
                                "Israel",
                                "Iverson",
                                "Ives",
                                "Ivey",
                                "Ivory",
                                "Ivy",
                                "Jack",
                                "Jacks",
                                "Jackson",
                                "Jacob",
                                "Jacobs",
                                "Jacobsen",
                                "Jacobson",
                                "Jacoby",
                                "Jacques",
                                "Jaeger",
                                "James",
                                "Jameson",
                                "Jamison",
                                "Janes",
                                "Jansen",
                                "Janssen",
                                "Jaramillo",
                                "Jarrell",
                                "Jarrett",
                                "Jarvis",
                                "Jasper",
                                "Jay",
                                "Jaynes",
                                "Jean",
                                "Jefferies",
                                "Jeffers",
                                "Jefferson",
                                "Jeffery",
                                "Jeffrey",
                                "Jeffries",
                                "Jenkins",
                                "Jennings",
                                "Jensen",
                                "Jenson",
                                "Jernigan",
                                "Jessup",
                                "Jeter",
                                "Jett",
                                "Jewell",
                                "Jewett",
                                "Jimenez",
                                "Jobe",
                                "Joe",
                                "Johansen",
                                "John",
                                "Johns",
                                "Johnson",
                                "Johnston",
                                "Joiner",
                                "Jolley",
                                "Jolly",
                                "Jones",
                                "Jordan",
                                "Jordon",
                                "Jorgensen",
                                "Jorgenson",
                                "Jose",
                                "Joseph",
                                "Joy",
                                "Joyce",
                                "Joyner",
                                "Juarez",
                                "Judd",
                                "Jude",
                                "Judge",
                                "Julian",
                                "Jung",
                                "Justice",
                                "Justus",
                                "Kahn",
                                "Kaiser",
                                "Kaminski",
                                "Kane",
                                "Kang",
                                "Kaplan",
                                "Karr",
                                "Kasper",
                                "Katz",
                                "Kauffman",
                                "Kaufman",
                                "Kay",
                                "Kaye",
                                "Keane",
                                "Kearney",
                                "Kearns",
                                "Keating",
                                "Keaton",
                                "Keck",
                                "Kee",
                                "Keefe",
                                "Keefer",
                                "Keegan",
                                "Keel",
                                "Keeler",
                                "Keeling",
                                "Keen",
                                "Keenan",
                                "Keene",
                                "Keener",
                                "Keeney",
                                "Keeton",
                                "Keith",
                                "Kelleher",
                                "Keller",
                                "Kelley",
                                "Kellogg",
                                "Kelly",
                                "Kelsey",
                                "Kelso",
                                "Kemp",
                                "Kemper",
                                "Kendall",
                                "Kendrick",
                                "Kennedy",
                                "Kenney",
                                "Kenny",
                                "Kent",
                                "Kenyon",
                                "Kern",
                                "Kerns",
                                "Kerr",
                                "Kessler",
                                "Ketchum",
                                "Key",
                                "Keyes",
                                "Keys",
                                "Khan",
                                "Kidd",
                                "Kidwell",
                                "Kiefer",
                                "Kilgore",
                                "Killian",
                                "Kilpatrick",
                                "Kim",
                                "Kimball",
                                "Kimble",
                                "Kimbrell",
                                "Kimbrough",
                                "Kimmel",
                                "Kinard",
                                "Kincaid",
                                "Kinder",
                                "King",
                                "Kingsley",
                                "Kinney",
                                "Kinsey",
                                "Kirby",
                                "Kirchner",
                                "Kirk",
                                "Kirkland",
                                "Kirkpatrick",
                                "Kirkwood",
                                "Kiser",
                                "Kitchen",
                                "Kitchens",
                                "Klein",
                                "Kline",
                                "Klinger",
                                "Knapp",
                                "Knight",
                                "Knoll",
                                "Knott",
                                "Knowles",
                                "Knowlton",
                                "Knox",
                                "Knudsen",
                                "Knudson",
                                "Knutson",
                                "Koch",
                                "Koehler",
                                "Koenig",
                                "Kohl",
                                "Kohler",
                                "Kohn",
                                "Kolb",
                                "Koonce",
                                "Koontz",
                                "Kopp",
                                "Kovach",
                                "Kowalski",
                                "Kozlowski",
                                "Kraft",
                                "Kramer",
                                "Kraus",
                                "Krause",
                                "Krauss",
                                "Krebs",
                                "Krieger",
                                "Kroll",
                                "Krueger",
                                "Kruger",
                                "Kruse",
                                "Kuhn",
                                "Kunkel",
                                "Kuntz",
                                "Kunz",
                                "Kurtz",
                                "Kuykendall",
                                "Kyle",
                                "Labbe",
                                "Lacey",
                                "Lachance",
                                "Lackey",
                                "Lacroix",
                                "Lacy",
                                "Ladd",
                                "Ladner",
                                "Lafferty",
                                "Lafleur",
                                "Lai",
                                "Laird",
                                "Lake",
                                "Lam",
                                "Lamar",
                                "Lamb",
                                "Lambert",
                                "Lamm",
                                "Lancaster",
                                "Lance",
                                "Land",
                                "Landers",
                                "Landis",
                                "Landrum",
                                "Landry",
                                "Lane",
                                "Laney",
                                "Lang",
                                "Langdon",
                                "Lange",
                                "Langford",
                                "Langley",
                                "Langlois",
                                "Langston",
                                "Lanham",
                                "Lanier",
                                "Lankford",
                                "Lantz",
                                "Laplante",
                                "Lapointe",
                                "Laporte",
                                "Lara",
                                "Large",
                                "Larkin",
                                "Larose",
                                "Larry",
                                "Larsen",
                                "Larson",
                                "Larue",
                                "Lash",
                                "Lassiter",
                                "Laster",
                                "Latham",
                                "Latimer",
                                "Lattimore",
                                "Lau",
                                "Lauer",
                                "Laughlin",
                                "Lavender",
                                "Lavigne",
                                "Lavoie",
                                "Law",
                                "Lawler",
                                "Lawless",
                                "Lawrence",
                                "Laws",
                                "Lawson",
                                "Lawton",
                                "Lay",
                                "Layman",
                                "Layne",
                                "Layton",
                                "Le",
                                "Lea",
                                "Leach",
                                "Leahy",
                                "Leak",
                                "Leake",
                                "Leal",
                                "Lear",
                                "Leary",
                                "Leavitt",
                                "Leblanc",
                                "Lebron",
                                "Leclair",
                                "Ledbetter",
                                "Ledesma",
                                "Ledford",
                                "Ledoux",
                                "Lee",
                                "Lefebvre",
                                "Leger",
                                "Legg",
                                "Leggett",
                                "Lehman",
                                "Lehmann",
                                "Leigh",
                                "Leighton",
                                "Lemaster",
                                "Lemay",
                                "Lemieux",
                                "Lemke",
                                "Lemmon",
                                "Lemon",
                                "Lemons",
                                "Lemus",
                                "Lennon",
                                "Lentz",
                                "Lenz",
                                "Leon",
                                "Leonard",
                                "Leone",
                                "Lerma",
                                "Lerner",
                                "Leroy",
                                "Leslie",
                                "Lessard",
                                "Lester",
                                "Leung",
                                "Levesque",
                                "Levin",
                                "Levine",
                                "Levy",
                                "Lewandowski",
                                "Lewis",
                                "Leyva",
                                "Li",
                                "Libby",
                                "Light",
                                "Lightfoot",
                                "Ligon",
                                "Liles",
                                "Lilley",
                                "Lilly",
                                "Lim",
                                "Lima",
                                "Limon",
                                "Lin",
                                "Linares",
                                "Lincoln",
                                "Lind",
                                "Lindberg",
                                "Linder",
                                "Lindley",
                                "Lindquist",
                                "Lindsay",
                                "Lindsey",
                                "Link",
                                "Linkous",
                                "Linn",
                                "Linton",
                                "Linville",
                                "Lipscomb",
                                "Lira",
                                "Lister",
                                "Little",
                                "Littlefield",
                                "Littlejohn",
                                "Littleton",
                                "Liu",
                                "Lively",
                                "Livingston",
                                "Lloyd",
                                "Lo",
                                "Locke",
                                "Lockett",
                                "Lockhart",
                                "Locklear",
                                "Lockwood",
                                "Loera",
                                "Loftin",
                                "Loftis",
                                "Lofton",
                                "Logan",
                                "Logsdon",
                                "Logue",
                                "Lomax",
                                "Lombard",
                                "Lombardi",
                                "Lombardo",
                                "London",
                                "Long",
                                "Longo",
                                "Longoria",
                                "Loomis",
                                "Looney",
                                "Loper",
                                "Lopes",
                                "Lopez",
                                "Lord",
                                "Lorenz",
                                "Lorenzo",
                                "Lott",
                                "Louis",
                                "Love",
                                "Lovejoy",
                                "Lovelace",
                                "Loveless",
                                "Lovell",
                                "Lovett",
                                "Loving",
                                "Low",
                                "Lowe",
                                "Lowell",
                                "Lowery",
                                "Lowry",
                                "Loy",
                                "Loyd",
                                "Lozano",
                                "Lu",
                                "Lucas",
                                "Luce",
                                "Lucero",
                                "Luciano",
                                "Luckett",
                                "Ludwig",
                                "Lugo",
                                "Lujan",
                                "Luke",
                                "Lumpkin",
                                "Luna",
                                "Lund",
                                "Lundberg",
                                "Lundy",
                                "Lunsford",
                                "Lusk",
                                "Luster",
                                "Luther",
                                "Luttrell",
                                "Lutz",
                                "Ly",
                                "Lyle",
                                "Lyles",
                                "Lyman",
                                "Lynch",
                                "Lynn",
                                "Lyon",
                                "Lyons",
                                "Lytle",
                                "Ma",
                                "Maas",
                                "Mabe",
                                "Mabry",
                                "Macdonald",
                                "Mace",
                                "Machado",
                                "Macias",
                                "Mack",
                                "Mackay",
                                "Mackenzie",
                                "Mackey",
                                "Mackie",
                                "Macklin",
                                "Maclean",
                                "Macon",
                                "Madden",
                                "Maddox",
                                "Madison",
                                "Madrid",
                                "Madrigal",
                                "Madsen",
                                "Maes",
                                "Maestas",
                                "Magana",
                                "Magee",
                                "Maguire",
                                "Mahan",
                                "Maher",
                                "Mahon",
                                "Mahoney",
                                "Maier",
                                "Main",
                                "Major",
                                "Majors",
                                "Maki",
                                "Malcolm",
                                "Maldonado",
                                "Malley",
                                "Mallory",
                                "Malloy",
                                "Malone",
                                "Maloney",
                                "Mancini",
                                "Mancuso",
                                "Maness",
                                "Mangum",
                                "Manley",
                                "Mann",
                                "Manning",
                                "Manns",
                                "Mansfield",
                                "Manson",
                                "Manuel",
                                "Maples",
                                "March",
                                "Marcotte",
                                "Marcum",
                                "Marcus",
                                "Mares",
                                "Marin",
                                "Marino",
                                "Marion",
                                "Mark",
                                "Markham",
                                "Marks",
                                "Marlow",
                                "Marlowe",
                                "Marquez",
                                "Marquis",
                                "Marr",
                                "Marrero",
                                "Marroquin",
                                "Marsh",
                                "Marshall",
                                "Martel",
                                "Martell",
                                "Martens",
                                "Martin",
                                "Martindale",
                                "Martinez",
                                "Martino",
                                "Martins",
                                "Martz",
                                "Marvin",
                                "Marx",
                                "Mason",
                                "Massey",
                                "Massie",
                                "Masters",
                                "Masterson",
                                "Mata",
                                "Matheny",
                                "Mathews",
                                "Mathias",
                                "Mathis",
                                "Matlock",
                                "Matney",
                                "Matos",
                                "Matson",
                                "Matthew",
                                "Matthews",
                                "Mattingly",
                                "Mattison",
                                "Mattos",
                                "Mattox",
                                "Mattson",
                                "Mauldin",
                                "Maupin",
                                "Maurer",
                                "Mauro",
                                "Maxey",
                                "Maxwell",
                                "May",
                                "Mayberry",
                                "Mayer",
                                "Mayers",
                                "Mayes",
                                "Mayfield",
                                "Mayhew",
                                "Maynard",
                                "Mayo",
                                "Mays",
                                "Mcadams",
                                "Mcafee",
                                "Mcalister",
                                "Mcallister",
                                "Mcarthur",
                                "Mcbee",
                                "Mcbride",
                                "Mccabe",
                                "Mccaffrey",
                                "Mccain",
                                "Mccall",
                                "Mccallister",
                                "Mccallum",
                                "Mccann",
                                "Mccarter",
                                "Mccarthy",
                                "Mccartney",
                                "Mccarty",
                                "Mccaskill",
                                "Mccauley",
                                "Mcclain",
                                "Mcclanahan",
                                "Mcclellan",
                                "Mcclelland",
                                "Mcclendon",
                                "Mcclintock",
                                "Mccloskey",
                                "Mccloud",
                                "Mcclung",
                                "Mcclure",
                                "Mccollum",
                                "Mccombs",
                                "Mcconnell",
                                "Mccool",
                                "Mccord",
                                "Mccorkle",
                                "Mccormack",
                                "Mccormick",
                                "Mccoy",
                                "Mccracken",
                                "Mccrary",
                                "Mccray",
                                "Mccreary",
                                "Mccue",
                                "Mcculloch",
                                "Mccullough",
                                "Mccune",
                                "Mccurdy",
                                "Mccutcheon",
                                "Mcdaniel",
                                "Mcdaniels",
                                "Mcdermott",
                                "Mcdonald",
                                "Mcdonnell",
                                "Mcdonough",
                                "Mcdougal",
                                "Mcdowell",
                                "Mcduffie",
                                "Mcelroy",
                                "Mcewen",
                                "Mcfadden",
                                "Mcfall",
                                "Mcfarland",
                                "Mcfarlane",
                                "Mcgee",
                                "Mcgehee",
                                "Mcghee",
                                "Mcgill",
                                "Mcginnis",
                                "Mcgovern",
                                "Mcgowan",
                                "Mcgrath",
                                "Mcgraw",
                                "Mcgregor",
                                "Mcgrew",
                                "Mcguire",
                                "Mchenry",
                                "Mchugh",
                                "Mcinnis",
                                "Mcintire",
                                "Mcintosh",
                                "Mcintyre",
                                "Mckay",
                                "Mckee",
                                "Mckeever",
                                "Mckenna",
                                "Mckenney",
                                "Mckenzie",
                                "Mckeown",
                                "Mckinley",
                                "Mckinney",
                                "Mckinnon",
                                "Mcknight",
                                "Mclain",
                                "Mclaughlin",
                                "Mclaurin",
                                "Mclean",
                                "Mclemore",
                                "Mclendon",
                                "Mcleod",
                                "Mcmahan",
                                "Mcmahon",
                                "Mcmanus",
                                "Mcmaster",
                                "Mcmillan",
                                "Mcmillen",
                                "Mcmillian",
                                "Mcmullen",
                                "Mcmurray",
                                "Mcnabb",
                                "Mcnair",
                                "Mcnally",
                                "Mcnamara",
                                "Mcneal",
                                "Mcneely",
                                "Mcneil",
                                "Mcneill",
                                "Mcnulty",
                                "Mcnutt",
                                "Mcpherson",
                                "Mcqueen",
                                "Mcrae",
                                "Mcreynolds",
                                "Mcswain",
                                "Mcvay",
                                "Mcvey",
                                "Mcwhorter",
                                "Mcwilliams",
                                "Meacham",
                                "Mead",
                                "Meade",
                                "Meador",
                                "Meadows",
                                "Means",
                                "Mears",
                                "Medeiros",
                                "Medina",
                                "Medley",
                                "Medlin",
                                "Medlock",
                                "Medrano",
                                "Meehan",
                                "Meek",
                                "Meeker",
                                "Meeks",
                                "Meier",
                                "Mejia",
                                "Melancon",
                                "Melendez",
                                "Mello",
                                "Melton",
                                "Melvin",
                                "Mena",
                                "Menard",
                                "Mendenhall",
                                "Mendez",
                                "Mendoza",
                                "Menendez",
                                "Mercado",
                                "Mercer",
                                "Merchant",
                                "Mercier",
                                "Meredith",
                                "Merrick",
                                "Merrill",
                                "Merritt",
                                "Mesa",
                                "Messenger",
                                "Messer",
                                "Messina",
                                "Metcalf",
                                "Metz",
                                "Metzger",
                                "Metzler",
                                "Meyer",
                                "Meyers",
                                "Meza",
                                "Michael",
                                "Michaels",
                                "Michaud",
                                "Michel",
                                "Middleton",
                                "Milam",
                                "Milburn",
                                "Miles",
                                "Millard",
                                "Miller",
                                "Milligan",
                                "Milliken",
                                "Mills",
                                "Milner",
                                "Milton",
                                "Mims",
                                "Miner",
                                "Minnick",
                                "Minor",
                                "Minter",
                                "Minton",
                                "Mintz",
                                "Miranda",
                                "Mireles",
                                "Mitchell",
                                "Mixon",
                                "Mize",
                                "Mobley",
                                "Mock",
                                "Moe",
                                "Moeller",
                                "Moen",
                                "Moffett",
                                "Moffitt",
                                "Mohr",
                                "Mojica",
                                "Molina",
                                "Moll",
                                "Monahan",
                                "Moniz",
                                "Monk",
                                "Monroe",
                                "Monson",
                                "Montague",
                                "Montalvo",
                                "Montanez",
                                "Montano",
                                "Montero",
                                "Montes",
                                "Montgomery",
                                "Montoya",
                                "Moody",
                                "Moon",
                                "Mooney",
                                "Moore",
                                "Mora",
                                "Morales",
                                "Moran",
                                "Moreau",
                                "Morehead",
                                "Moreland",
                                "Moreno",
                                "Morey",
                                "Morgan",
                                "Moriarty",
                                "Morin",
                                "Morley",
                                "Morrell",
                                "Morrill",
                                "Morris",
                                "Morrison",
                                "Morrissey",
                                "Morrow",
                                "Morse",
                                "Mortensen",
                                "Morton",
                                "Mosby",
                                "Moseley",
                                "Moser",
                                "Moses",
                                "Mosher",
                                "Mosier",
                                "Mosley",
                                "Moss",
                                "Motley",
                                "Mott",
                                "Moulton",
                                "Mount",
                                "Mowery",
                                "Moya",
                                "Moye",
                                "Moyer",
                                "Mueller",
                                "Muhammad",
                                "Muir",
                                "Mull",
                                "Mullen",
                                "Muller",
                                "Mulligan",
                                "Mullin",
                                "Mullins",
                                "Mullis",
                                "Muncy",
                                "Mundy",
                                "Muniz",
                                "Munn",
                                "Munoz",
                                "Munson",
                                "Murdock",
                                "Murillo",
                                "Murphy",
                                "Murray",
                                "Murrell",
                                "Murry",
                                "Muse",
                                "Musgrove",
                                "Musser",
                                "Myers",
                                "Myles",
                                "Myrick",
                                "Nabors",
                                "Nadeau",
                                "Nagel",
                                "Nagle",
                                "Nagy",
                                "Najera",
                                "Nall",
                                "Nance",
                                "Napier",
                                "Naquin",
                                "Nash",
                                "Nation",
                                "Nava",
                                "Navarrete",
                                "Navarro",
                                "Naylor",
                                "Neal",
                                "Nealy",
                                "Needham",
                                "Neel",
                                "Neeley",
                                "Neely",
                                "Neff",
                                "Negrete",
                                "Negron",
                                "Neil",
                                "Neill",
                                "Nelms",
                                "Nelson",
                                "Nesbitt",
                                "Nesmith",
                                "Ness",
                                "Nettles",
                                "Neuman",
                                "Neumann",
                                "Nevarez",
                                "Neville",
                                "New",
                                "Newberry",
                                "Newby",
                                "Newcomb",
                                "Newell",
                                "Newkirk",
                                "Newman",
                                "Newsom",
                                "Newsome",
                                "Newton",
                                "Ng",
                                "Ngo",
                                "Nguyen",
                                "Nicholas",
                                "Nichols",
                                "Nicholson",
                                "Nickerson",
                                "Nielsen",
                                "Nielson",
                                "Nieto",
                                "Nieves",
                                "Niles",
                                "Nix",
                                "Nixon",
                                "Noble",
                                "Nobles",
                                "Noe",
                                "Noel",
                                "Nolan",
                                "Noland",
                                "Nolen",
                                "Noonan",
                                "Norfleet",
                                "Noriega",
                                "Norman",
                                "Norris",
                                "North",
                                "Norton",
                                "Norwood",
                                "Novak",
                                "Nowak",
                                "Nowlin",
                                "Noyes",
                                "Nugent",
                                "Numbers",
                                "Nunes",
                                "Nunez",
                                "Nunley",
                                "Nunn",
                                "Nutt",
                                "Nye",
                                "Oakes",
                                "Oakley",
                                "Oaks",
                                "Oates",
                                "Obrien",
                                "Obryan",
                                "Ocampo",
                                "Ocasio",
                                "Ochoa",
                                "Oconnell",
                                "Oconner",
                                "Oconnor",
                                "Odell",
                                "Oden",
                                "Odom",
                                "Odonnell",
                                "Ogden",
                                "Ogle",
                                "Oglesby",
                                "Oh",
                                "Ohara",
                                "Ojeda",
                                "Okeefe",
                                "Oldham",
                                "Olds",
                                "Oleary",
                                "Oliphant",
                                "Oliva",
                                "Olivares",
                                "Olivarez",
                                "Olivas",
                                "Oliveira",
                                "Oliver",
                                "Olivo",
                                "Olmstead",
                                "Olsen",
                                "Olson",
                                "Olvera",
                                "Omalley",
                                "Oneal",
                                "Oneil",
                                "Oneill",
                                "Ontiveros",
                                "Ordonez",
                                "Oreilly",
                                "Orellana",
                                "Orlando",
                                "Ornelas",
                                "Orosco",
                                "Orourke",
                                "Orozco",
                                "Orr",
                                "Orta",
                                "Ortega",
                                "Ortiz",
                                "Osborn",
                                "Osborne",
                                "Osburn",
                                "Osgood",
                                "Oshea",
                                "Osorio",
                                "Osteen",
                                "Ostrander",
                                "Osullivan",
                                "Oswald",
                                "Otero",
                                "Otis",
                                "Otoole",
                                "Ott",
                                "Otto",
                                "Ouellette",
                                "Outlaw",
                                "Overby",
                                "Overstreet",
                                "Overton",
                                "Owen",
                                "Owens",
                                "Pace",
                                "Pacheco",
                                "Pack",
                                "Packard",
                                "Packer",
                                "Padgett",
                                "Padilla",
                                "Pagan",
                                "Page",
                                "Paige",
                                "Paine",
                                "Painter",
                                "Pak",
                                "Palacios",
                                "Palma",
                                "Palmer",
                                "Palumbo",
                                "Pannell",
                                "Pantoja",
                                "Pappas",
                                "Paquette",
                                "Paradis",
                                "Paredes",
                                "Parent",
                                "Parham",
                                "Paris",
                                "Parish",
                                "Park",
                                "Parker",
                                "Parkinson",
                                "Parks",
                                "Parnell",
                                "Parr",
                                "Parra",
                                "Parris",
                                "Parrish",
                                "Parrott",
                                "Parry",
                                "Parson",
                                "Parsons",
                                "Partin",
                                "Partridge",
                                "Pate",
                                "Patel",
                                "Paterson",
                                "Patino",
                                "Patrick",
                                "Patten",
                                "Patterson",
                                "Patton",
                                "Paul",
                                "Pauley",
                                "Paulsen",
                                "Paulson",
                                "Paxton",
                                "Payne",
                                "Payton",
                                "Paz",
                                "Peace",
                                "Peachey",
                                "Peacock",
                                "Peak",
                                "Pearce",
                                "Pearson",
                                "Pease",
                                "Peck",
                                "Pedersen",
                                "Pederson",
                                "Peebles",
                                "Peek",
                                "Peeples",
                                "Pelletier",
                                "Pemberton",
                                "Pena",
                                "Pence",
                                "Pendergrass",
                                "Pendleton",
                                "Penn",
                                "Pennell",
                                "Pennington",
                                "Penny",
                                "Peoples",
                                "Pepper",
                                "Perales",
                                "Peralta",
                                "Perdue",
                                "Pereira",
                                "Perez",
                                "Perkins",
                                "Perreault",
                                "Perrin",
                                "Perron",
                                "Perry",
                                "Perryman",
                                "Person",
                                "Peter",
                                "Peterman",
                                "Peters",
                                "Petersen",
                                "Peterson",
                                "Petit",
                                "Petrie",
                                "Pettis",
                                "Pettit",
                                "Petty",
                                "Peyton",
                                "Pfeifer",
                                "Pfeiffer",
                                "Pham",
                                "Phan",
                                "Phelan",
                                "Phelps",
                                "Phillips",
                                "Phipps",
                                "Picard",
                                "Pickard",
                                "Pickens",
                                "Pickering",
                                "Pickett",
                                "Pierce",
                                "Pierre",
                                "Pierson",
                                "Pike",
                                "Pimentel",
                                "Pina",
                                "Pineda",
                                "Pinkerton",
                                "Pinkston",
                                "Pino",
                                "Pinson",
                                "Pinto",
                                "Piper",
                                "Pipkin",
                                "Pippin",
                                "Pitre",
                                "Pitt",
                                "Pittman",
                                "Pitts",
                                "Plante",
                                "Platt",
                                "Pleasant",
                                "Plummer",
                                "Plunkett",
                                "Poe",
                                "Pogue",
                                "Poindexter",
                                "Pointer",
                                "Poirier",
                                "Polanco",
                                "Polk",
                                "Pollack",
                                "Pollard",
                                "Pollock",
                                "Ponce",
                                "Pond",
                                "Ponder",
                                "Pool",
                                "Poole",
                                "Poore",
                                "Pope",
                                "Porter",
                                "Porterfield",
                                "Portillo",
                                "Posey",
                                "Post",
                                "Poston",
                                "Potter",
                                "Potts",
                                "Poulin",
                                "Pounds",
                                "Powell",
                                "Power",
                                "Powers",
                                "Prado",
                                "Prater",
                                "Prather",
                                "Pratt",
                                "Prentice",
                                "Prescott",
                                "Presley",
                                "Pressley",
                                "Preston",
                                "Prewitt",
                                "Price",
                                "Prichard",
                                "Pride",
                                "Pridgen",
                                "Priest",
                                "Prieto",
                                "Prince",
                                "Pringle",
                                "Pritchard",
                                "Pritchett",
                                "Proctor",
                                "Proffitt",
                                "Prosser",
                                "Provost",
                                "Pruett",
                                "Pruitt",
                                "Pryor",
                                "Puckett",
                                "Puente",
                                "Pugh",
                                "Pulido",
                                "Pullen",
                                "Pulley",
                                "Pulliam",
                                "Purcell",
                                "Purdy",
                                "Purnell",
                                "Purvis",
                                "Putman",
                                "Putnam",
                                "Pyle",
                                "Qualls",
                                "Quarles",
                                "Queen",
                                "Quezada",
                                "Quick",
                                "Quigley",
                                "Quinlan",
                                "Quinn",
                                "Quinones",
                                "Quinonez",
                                "Quintana",
                                "Quintanilla",
                                "Quintero",
                                "Quiroz",
                                "Rader",
                                "Radford",
                                "Rafferty",
                                "Ragan",
                                "Ragland",
                                "Ragsdale",
                                "Raines",
                                "Rainey",
                                "Rains",
                                "Raley",
                                "Ralph",
                                "Ralston",
                                "Ramey",
                                "Ramirez",
                                "Ramon",
                                "Ramos",
                                "Ramsay",
                                "Ramsey",
                                "Rand",
                                "Randall",
                                "Randle",
                                "Randolph",
                                "Raney",
                                "Rangel",
                                "Rankin",
                                "Ransom",
                                "Rapp",
                                "Rash",
                                "Rasmussen",
                                "Ratcliff",
                                "Ratliff",
                                "Rau",
                                "Rauch",
                                "Rawlings",
                                "Rawlins",
                                "Rawls",
                                "Ray",
                                "Rayburn",
                                "Raymond",
                                "Razo",
                                "Rea",
                                "Read",
                                "Reagan",
                                "Reardon",
                                "Reaves",
                                "Rector",
                                "Redd",
                                "Redden",
                                "Reddick",
                                "Redding",
                                "Reddy",
                                "Redman",
                                "Redmon",
                                "Redmond",
                                "Reece",
                                "Reed",
                                "Reeder",
                                "Reedy",
                                "Rees",
                                "Reese",
                                "Reeves",
                                "Regalado",
                                "Regan",
                                "Register",
                                "Reich",
                                "Reichert",
                                "Reid",
                                "Reilly",
                                "Reinhardt",
                                "Reis",
                                "Reiter",
                                "Rendon",
                                "Renfro",
                                "Renner",
                                "Reno",
                                "Renteria",
                                "Reyes",
                                "Reyna",
                                "Reynolds",
                                "Reynoso",
                                "Rhea",
                                "Rhoades",
                                "Rhoads",
                                "Rhodes",
                                "Ricci",
                                "Rice",
                                "Rich",
                                "Richard",
                                "Richards",
                                "Richardson",
                                "Richey",
                                "Richie",
                                "Richmond",
                                "Richter",
                                "Rickard",
                                "Ricker",
                                "Ricketts",
                                "Rickman",
                                "Ricks",
                                "Rico",
                                "Riddick",
                                "Riddle",
                                "Ridenour",
                                "Rider",
                                "Ridgeway",
                                "Ridley",
                                "Rife",
                                "Riggins",
                                "Riggs",
                                "Rigsby",
                                "Riley",
                                "Rinehart",
                                "Ring",
                                "Rios",
                                "Ripley",
                                "Ritchey",
                                "Ritchie",
                                "Ritter",
                                "Rivas",
                                "Rivera",
                                "Rivers",
                                "Rizzo",
                                "Roach",
                                "Roark",
                                "Robb",
                                "Robbins",
                                "Roberge",
                                "Roberson",
                                "Robert",
                                "Roberts",
                                "Robertson",
                                "Robinette",
                                "Robins",
                                "Robinson",
                                "Robison",
                                "Robles",
                                "Robson",
                                "Roby",
                                "Rocha",
                                "Roche",
                                "Rock",
                                "Rockwell",
                                "Roden",
                                "Roderick",
                                "Rodgers",
                                "Rodrigue",
                                "Rodrigues",
                                "Rodriguez",
                                "Rodriquez",
                                "Roe",
                                "Rogers",
                                "Rohr",
                                "Rojas",
                                "Roland",
                                "Roldan",
                                "Roller",
                                "Rollins",
                                "Roman",
                                "Romano",
                                "Romeo",
                                "Romero",
                                "Romo",
                                "Roney",
                                "Rooney",
                                "Root",
                                "Roper",
                                "Rosa",
                                "Rosado",
                                "Rosales",
                                "Rosario",
                                "Rosas",
                                "Rose",
                                "Rosen",
                                "Rosenbaum",
                                "Rosenberg",
                                "Rosenthal",
                                "Ross",
                                "Rosser",
                                "Rossi",
                                "Roth",
                                "Rounds",
                                "Rountree",
                                "Rouse",
                                "Roush",
                                "Rousseau",
                                "Rowan",
                                "Rowe",
                                "Rowell",
                                "Rowland",
                                "Rowley",
                                "Roy",
                                "Royal",
                                "Royer",
                                "Royster",
                                "Rubin",
                                "Rubio",
                                "Ruby",
                                "Rucker",
                                "Rudd",
                                "Rudolph",
                                "Ruff",
                                "Ruffin",
                                "Ruiz",
                                "Runyan",
                                "Runyon",
                                "Rupp",
                                "Rush",
                                "Rushing",
                                "Russ",
                                "Russell",
                                "Russo",
                                "Rust",
                                "Ruth",
                                "Rutherford",
                                "Rutledge",
                                "Ryan",
                                "Ryder",
                                "Saavedra",
                                "Sadler",
                                "Saenz",
                                "Sage",
                                "Sager",
                                "Salas",
                                "Salazar",
                                "Salcedo",
                                "Salcido",
                                "Saldana",
                                "Sales",
                                "Salgado",
                                "Salinas",
                                "Salisbury",
                                "Sallee",
                                "Salley",
                                "Salmon",
                                "Salter",
                                "Sam",
                                "Sammons",
                                "Sample",
                                "Samples",
                                "Sampson",
                                "Sams",
                                "Samson",
                                "Samuel",
                                "Samuels",
                                "Sanborn",
                                "Sanchez",
                                "Sander",
                                "Sanders",
                                "Sanderson",
                                "Sandlin",
                                "Sandoval",
                                "Sands",
                                "Sanford",
                                "Santana",
                                "Santiago",
                                "Santos",
                                "Sapp",
                                "Sargent",
                                "Sasser",
                                "Satterfield",
                                "Saucedo",
                                "Saucier",
                                "Sauer",
                                "Sauls",
                                "Saunders",
                                "Savage",
                                "Savoy",
                                "Sawyer",
                                "Sawyers",
                                "Saxon",
                                "Saxton",
                                "Saylor",
                                "Scales",
                                "Scanlon",
                                "Scarborough",
                                "Scarbrough",
                                "Schaefer",
                                "Schaeffer",
                                "Schafer",
                                "Schaffer",
                                "Schell",
                                "Scherer",
                                "Schilling",
                                "Schmid",
                                "Schmidt",
                                "Schmitt",
                                "Schmitz",
                                "Schneider",
                                "Schofield",
                                "Scholl",
                                "Schott",
                                "Schrader",
                                "Schreiber",
                                "Schroeder",
                                "Schubert",
                                "Schuler",
                                "Schulte",
                                "Schultz",
                                "Schulz",
                                "Schulze",
                                "Schumacher",
                                "Schuster",
                                "Schwab",
                                "Schwartz",
                                "Schwarz",
                                "Schweitzer",
                                "Scoggins",
                                "Scott",
                                "Scroggins",
                                "Scruggs",
                                "Scully",
                                "Seal",
                                "Seals",
                                "Seaman",
                                "Searcy",
                                "Sears",
                                "Seaton",
                                "Seay",
                                "See",
                                "Seeley",
                                "Segura",
                                "Seibert",
                                "Seidel",
                                "Seifert",
                                "Seiler",
                                "Seitz",
                                "Selby",
                                "Self",
                                "Sell",
                                "Sellers",
                                "Sells",
                                "Sena",
                                "Sepulveda",
                                "Serna",
                                "Serrano",
                                "Sessions",
                                "Settle",
                                "Severson",
                                "Seward",
                                "Sewell",
                                "Sexton",
                                "Seymore",
                                "Seymour",
                                "Shackelford",
                                "Shade",
                                "Shafer",
                                "Shaffer",
                                "Shah",
                                "Shank",
                                "Shanks",
                                "Shannon",
                                "Shapiro",
                                "Sharkey",
                                "Sharp",
                                "Sharpe",
                                "Shaver",
                                "Shaw",
                                "Shay",
                                "Shea",
                                "Shearer",
                                "Sheehan",
                                "Sheets",
                                "Sheffield",
                                "Shelby",
                                "Sheldon",
                                "Shell",
                                "Shelley",
                                "Shelton",
                                "Shepard",
                                "Shephard",
                                "Shepherd",
                                "Sheppard",
                                "Sheridan",
                                "Sherman",
                                "Sherrill",
                                "Sherrod",
                                "Sherwood",
                                "Shields",
                                "Shifflett",
                                "Shin",
                                "Shinn",
                                "Shipley",
                                "Shipman",
                                "Shipp",
                                "Shirley",
                                "Shively",
                                "Shockley",
                                "Shoemaker",
                                "Shook",
                                "Shore",
                                "Shores",
                                "Short",
                                "Shorter",
                                "Shrader",
                                "Shuler",
                                "Shull",
                                "Shultz",
                                "Shumaker",
                                "Shuman",
                                "Shumate",
                                "Sibley",
                                "Siegel",
                                "Sierra",
                                "Sigler",
                                "Sikes",
                                "Siler",
                                "Silva",
                                "Silver",
                                "Silverman",
                                "Silvers",
                                "Silvia",
                                "Simmons",
                                "Simms",
                                "Simon",
                                "Simone",
                                "Simons",
                                "Simonson",
                                "Simpkins",
                                "Simpson",
                                "Sims",
                                "Sinclair",
                                "Singer",
                                "Singh",
                                "Singletary",
                                "Singleton",
                                "Sipes",
                                "Sisco",
                                "Sisk",
                                "Sisson",
                                "Sizemore",
                                "Skaggs",
                                "Skelton",
                                "Skidmore",
                                "Skinner",
                                "Skipper",
                                "Slack",
                                "Slade",
                                "Slagle",
                                "Slater",
                                "Slaughter",
                                "Sledge",
                                "Sloan",
                                "Slocum",
                                "Slone",
                                "Small",
                                "Smalley",
                                "Smalls",
                                "Smallwood",
                                "Smart",
                                "Smiley",
                                "Smith",
                                "Smithson",
                                "Smoot",
                                "Smothers",
                                "Smyth",
                                "Snead",
                                "Sneed",
                                "Snell",
                                "Snider",
                                "Snipes",
                                "Snodgrass",
                                "Snow",
                                "Snowden",
                                "Snyder",
                                "Soares",
                                "Solano",
                                "Solis",
                                "Soliz",
                                "Solomon",
                                "Somers",
                                "Sommer",
                                "Sommers",
                                "Song",
                                "Sorensen",
                                "Sorenson",
                                "Soria",
                                "Soriano",
                                "Sorrell",
                                "Sosa",
                                "Sotelo",
                                "Soto",
                                "Sousa",
                                "South",
                                "Southard",
                                "Southerland",
                                "Southern",
                                "Souza",
                                "Sowell",
                                "Sowers",
                                "Spain",
                                "Spalding",
                                "Spangler",
                                "Spann",
                                "Sparkman",
                                "Sparks",
                                "Sparrow",
                                "Spaulding",
                                "Spear",
                                "Spearman",
                                "Spears",
                                "Speed",
                                "Speer",
                                "Spellman",
                                "Spence",
                                "Spencer",
                                "Sperry",
                                "Spicer",
                                "Spinks",
                                "Spivey",
                                "Spooner",
                                "Spradlin",
                                "Sprague",
                                "Spring",
                                "Springer",
                                "Sprouse",
                                "Spruill",
                                "Spurlock",
                                "Squires",
                                "Stacey",
                                "Stack",
                                "Stackhouse",
                                "Stacy",
                                "Stafford",
                                "Staggs",
                                "Stahl",
                                "Staley",
                                "Stallings",
                                "Stallworth",
                                "Stamm",
                                "Stamper",
                                "Stamps",
                                "Stanfield",
                                "Stanford",
                                "Stanley",
                                "Stanton",
                                "Staples",
                                "Stapleton",
                                "Stark",
                                "Starkey",
                                "Starks",
                                "Starling",
                                "Starnes",
                                "Starr",
                                "Staten",
                                "Staton",
                                "Stauffer",
                                "Stclair",
                                "Stearns",
                                "Steed",
                                "Steele",
                                "Steen",
                                "Steffen",
                                "Stegall",
                                "Stein",
                                "Steinberg",
                                "Steiner",
                                "Stephen",
                                "Stephens",
                                "Stephenson",
                                "Stepp",
                                "Sterling",
                                "Stern",
                                "Stevens",
                                "Stevenson",
                                "Steward",
                                "Stewart",
                                "Stidham",
                                "Stiles",
                                "Still",
                                "Stillwell",
                                "Stiltner",
                                "Stine",
                                "Stinnett",
                                "Stinson",
                                "Stitt",
                                "Stjohn",
                                "Stock",
                                "Stockton",
                                "Stoddard",
                                "Stokes",
                                "Stoll",
                                "Stone",
                                "Stoner",
                                "Storey",
                                "Story",
                                "Stout",
                                "Stovall",
                                "Stover",
                                "Stowe",
                                "Stpierre",
                                "Strain",
                                "Strand",
                                "Strange",
                                "Stratton",
                                "Strauss",
                                "Street",
                                "Streeter",
                                "Strickland",
                                "stringer",
                                "Strong",
                                "Strother",
                                "Stroud",
                                "Strunk",
                                "Stuart",
                                "Stubblefield",
                                "Stubbs",
                                "Stuckey",
                                "Stull",
                                "Stump",
                                "Sturgeon",
                                "Sturgill",
                                "Sturm",
                                "Styles",
                                "Suarez",
                                "Suggs",
                                "Sullivan",
                                "Summers",
                                "Sumner",
                                "Sumpter",
                                "Sun",
                                "Sutherland",
                                "Sutter",
                                "Sutton",
                                "Swafford",
                                "Swain",
                                "Swan",
                                "Swank",
                                "Swann",
                                "Swanson",
                                "Swartz",
                                "Sweat",
                                "Sweeney",
                                "Sweet",
                                "Swenson",
                                "Swift",
                                "Swisher",
                                "Switzer",
                                "Sykes",
                                "Sylvester",
                                "Taber",
                                "Tabor",
                                "Tackett",
                                "Taft",
                                "Taggart",
                                "Talbert",
                                "Talbot",
                                "Talbott",
                                "Talley",
                                "Tam",
                                "Tamayo",
                                "Tan",
                                "Tanaka",
                                "Tang",
                                "Tanner",
                                "Tapia",
                                "Tapp",
                                "Tarver",
                                "Tate",
                                "Tatum",
                                "Tavares",
                                "Taylor",
                                "Teague",
                                "Teal",
                                "Teel",
                                "Teeter",
                                "Tejada",
                                "Tellez",
                                "Temple",
                                "Templeton",
                                "Tennant",
                                "Tenney",
                                "Terrell",
                                "Terry",
                                "Thacker",
                                "Thao",
                                "Tharp",
                                "Thatcher",
                                "Thayer",
                                "Theriault",
                                "Thibodeau",
                                "Thibodeaux",
                                "Thigpen",
                                "Thomas",
                                "Thomason",
                                "Thompson",
                                "Thomsen",
                                "Thomson",
                                "Thorn",
                                "Thornburg",
                                "Thorne",
                                "Thornhill",
                                "Thornton",
                                "Thorpe",
                                "Thorton",
                                "Thrash",
                                "Thrasher",
                                "Thurman",
                                "Thurston",
                                "Tibbs",
                                "Tice",
                                "Tidwell",
                                "Tierney",
                                "Tijerina",
                                "Tiller",
                                "Tillery",
                                "Tilley",
                                "Tillman",
                                "Tilton",
                                "Timm",
                                "Timmons",
                                "Tinsley",
                                "Tipton",
                                "Tirado",
                                "Tisdale",
                                "Titus",
                                "Tobias",
                                "Tobin",
                                "Todd",
                                "Tolbert",
                                "Toledo",
                                "Toler",
                                "Toliver",
                                "Tolliver",
                                "Tom",
                                "Tomlin",
                                "Tomlinson",
                                "Tompkins",
                                "Toney",
                                "Tong",
                                "Toro",
                                "Torrence",
                                "Torres",
                                "Torrez",
                                "Toth",
                                "Totten",
                                "Tovar",
                                "Towns",
                                "Townsend",
                                "Tracy",
                                "Trahan",
                                "Trammell",
                                "Tran",
                                "Trapp",
                                "Trask",
                                "Travers",
                                "Travis",
                                "Traylor",
                                "Treadway",
                                "Treadwell",
                                "Trejo",
                                "Tremblay",
                                "Trent",
                                "Trevino",
                                "Tribble",
                                "Trice",
                                "Trimble",
                                "Trinidad",
                                "Triplett",
                                "Tripp",
                                "Trotter",
                                "Trout",
                                "Troutman",
                                "Troy",
                                "Trudeau",
                                "True",
                                "Truitt",
                                "Trujillo",
                                "Truong",
                                "Tubbs",
                                "Tuck",
                                "Tucker",
                                "Tuggle",
                                "Turk",
                                "Turley",
                                "Turnbull",
                                "Turner",
                                "Turney",
                                "Turpin",
                                "Tuttle",
                                "Tyler",
                                "Tyner",
                                "Tyree",
                                "Tyson",
                                "Ulrich",
                                "Underhill",
                                "Underwood",
                                "Unger",
                                "Upchurch",
                                "Upshaw",
                                "Upton",
                                "Urban",
                                "Urbina",
                                "Uribe",
                                "Utley",
                                "Vail",
                                "Valadez",
                                "Valdes",
                                "Valdez",
                                "Valencia",
                                "Valentin",
                                "Valentine",
                                "Valenzuela",
                                "Valerio",
                                "Valle",
                                "Vallejo",
                                "Valles",
                                "Van",
                                "Vance",
                                "Vandyke",
                                "Vang",
                                "Vanhoose",
                                "Vanhorn",
                                "Vanmeter",
                                "Vann",
                                "Vanwinkle",
                                "Varela",
                                "Vargas",
                                "Varner",
                                "Varney",
                                "Vasquez",
                                "Vaughan",
                                "Vaughn",
                                "Vaught",
                                "Vazquez",
                                "Veal",
                                "Vega",
                                "Vela",
                                "Velasco",
                                "Velasquez",
                                "Velazquez",
                                "Velez",
                                "Venable",
                                "Venegas",
                                "Ventura",
                                "Vera",
                                "Verdin",
                                "Vernon",
                                "Vest",
                                "Vetter",
                                "Vick",
                                "Vickers",
                                "Vickery",
                                "Victor",
                                "Vidal",
                                "Viera",
                                "Vigil",
                                "Villa",
                                "Villalobos",
                                "Villanueva",
                                "Villareal",
                                "Villarreal",
                                "Villasenor",
                                "Villegas",
                                "Vincent",
                                "Vines",
                                "Vinson",
                                "Vitale",
                                "Vo",
                                "Vogel",
                                "Vogt",
                                "Voss",
                                "Vu",
                                "Vue",
                                "Waddell",
                                "Wade",
                                "Wadsworth",
                                "Waggoner",
                                "Wagner",
                                "Wagoner",
                                "Wahl",
                                "Waite",
                                "Wakefield",
                                "Walden",
                                "Waldron",
                                "Waldrop",
                                "Walker",
                                "Wall",
                                "Wallace",
                                "Wallen",
                                "Waller",
                                "Walling",
                                "Wallis",
                                "Walls",
                                "Walsh",
                                "Walston",
                                "Walter",
                                "Walters",
                                "Walton",
                                "Wampler",
                                "Wang",
                                "Ward",
                                "Warden",
                                "Ware",
                                "Warfield",
                                "Warner",
                                "Warren",
                                "Washburn",
                                "Washington",
                                "Wasson",
                                "Waterman",
                                "Waters",
                                "Watkins",
                                "Watson",
                                "Watt",
                                "Watters",
                                "Watts",
                                "Waugh",
                                "Way",
                                "Wayne",
                                "Weatherford",
                                "Weatherly",
                                "Weathers",
                                "Weaver",
                                "Webb",
                                "Webber",
                                "Weber",
                                "Webster",
                                "Weddle",
                                "Weed",
                                "Weeks",
                                "Weems",
                                "Weiner",
                                "Weinstein",
                                "Weir",
                                "Weis",
                                "Weiss",
                                "Welch",
                                "Weldon",
                                "Welker",
                                "Weller",
                                "Wellman",
                                "Wells",
                                "Welsh",
                                "Wendt",
                                "Wentworth",
                                "Wentz",
                                "Wenzel",
                                "Werner",
                                "Wertz",
                                "Wesley",
                                "West",
                                "Westbrook",
                                "Wester",
                                "Westfall",
                                "Westmoreland",
                                "Weston",
                                "Wetzel",
                                "Whalen",
                                "Whaley",
                                "Wharton",
                                "Whatley",
                                "Wheat",
                                "Wheatley",
                                "Wheaton",
                                "Wheeler",
                                "Whelan",
                                "Whipple",
                                "Whitaker",
                                "Whitcomb",
                                "White",
                                "Whited",
                                "Whitehead",
                                "Whitehurst",
                                "Whiteside",
                                "Whitfield",
                                "Whiting",
                                "Whitley",
                                "Whitlock",
                                "Whitlow",
                                "Whitman",
                                "Whitmire",
                                "Whitmore",
                                "Whitney",
                                "Whitson",
                                "Whitt",
                                "Whittaker",
                                "Whitten",
                                "Whittington",
                                "Whittle",
                                "Whitworth",
                                "Wicker",
                                "Wicks",
                                "Wiese",
                                "Wiggins",
                                "Wilbanks",
                                "Wilbur",
                                "Wilburn",
                                "Wilcox",
                                "Wild",
                                "Wilde",
                                "Wilder",
                                "Wiles",
                                "Wiley",
                                "Wilhelm",
                                "Wilhite",
                                "Wilke",
                                "Wilkerson",
                                "Wilkes",
                                "Wilkins",
                                "Wilkinson",
                                "Wilks",
                                "Will",
                                "Willard",
                                "Willett",
                                "Willey",
                                "William",
                                "Williams",
                                "Williamson",
                                "Williford",
                                "Willingham",
                                "Willis",
                                "Willoughby",
                                "Wills",
                                "Willson",
                                "Wilmoth",
                                "Wilson",
                                "Wilt",
                                "Wimberly",
                                "Winchester",
                                "Windham",
                                "Winfield",
                                "Winfrey",
                                "Wing",
                                "Wingate",
                                "Wingfield",
                                "Winkler",
                                "Winn",
                                "Winslow",
                                "Winstead",
                                "Winston",
                                "Winter",
                                "Winters",
                                "Wirth",
                                "Wise",
                                "Wiseman",
                                "Wisniewski",
                                "Withers",
                                "Witherspoon",
                                "Witt",
                                "Witte",
                                "Wofford",
                                "Wolf",
                                "Wolfe",
                                "Wolff",
                                "Wolford",
                                "Womack",
                                "Wong",
                                "Woo",
                                "Wood",
                                "Woodall",
                                "Woodard",
                                "Woodbury",
                                "Woodcock",
                                "Wooden",
                                "Woodley",
                                "Woodruff",
                                "Woods",
                                "Woodson",
                                "Woodward",
                                "Woodworth",
                                "Woody",
                                "Wooldridge",
                                "Wooten",
                                "Worden",
                                "Workman",
                                "Worley",
                                "Worrell",
                                "Worth",
                                "Worthington",
                                "Worthy",
                                "Wray",
                                "Wren",
                                "Wright",
                                "Wu",
                                "Wyatt",
                                "Wylie",
                                "Wyman",
                                "Wynn",
                                "Wynne",
                                "Xiong",
                                "Yancey",
                                "Yanez",
                                "Yang",
                                "Yarbrough",
                                "Yates",
                                "Yazzie",
                                "Ybarra",
                                "Yeager",
                                "Yee",
                                "Yi",
                                "Yoder",
                                "Yoo",
                                "Yoon",
                                "York",
                                "Yost",
                                "Young",
                                "Youngblood",
                                "Younger",
                                "Yount",
                                "Yu",
                                "Zamora",
                                "Zapata",
                                "Zaragoza",
                                "Zarate",
                                "Zavala",
                                "Zeigler",
                                "Zeller",
                                "Zepeda",
                                "Zhang",
                                "Ziegler",
                                "Zielinski",
                                "Zimmer",
                                "Zimmerman",
                                "Zook",
                                "Zuniga",

                                #endregion
                            };
            var index = rand.Next(1, lastNames.Count());
            return lastNames[index];
        }

        public string GetFakeChineseLastName()
        {
            
            var lastNames = new[]
                            {
                                #region LastNames
                               "李",
"王",
"张",
"刘",
"陈",
"杨",
"赵",
"黄",
"周",
"吴",
"徐",
"孙",
"胡",
"朱",
"高",
"林",
"何",
"郭",
"马",
"罗",
"梁",
"宋",
"郑",
"谢",
"韩",
"唐",
"冯",
"于",
"董",
"萧",
"程",
"曹",
"袁",
"邓",
"许",
"傅",
"沈",
"曾",
"彭",
"吕",
"苏",
"卢",
"蒋",
"蔡",
"贾",
"丁",
"魏",
"薛",
"叶",
"阎",
"余",
"潘",
"杜",
"戴",
"夏",
"钟",
"汪",
"田",
"任",
"姜",
"范",
"方",
"石",
"姚",
"谭",
"盛",
"邹",
"熊",
"金",
"陆",
"郝",
"孔",
"白",
"崔",
"康",
"毛",
"邱",
"秦",
"江",
"史",
"顾",
"侯",
"邵",
"孟",
"龙",
"万",
"段",
"章",
"钱",
"汤",
"尹",
"黎",
"易",
"常",
"武",
"乔",
"贺",
"赖",
"龚",
"文"

                                #endregion
                            };
            var index = rand.Next(1, lastNames.Count());
            return lastNames[index];
        }

        #endregion

        #region Localization
        public string localize(string language, string action, SDataPayload payload, string type, string description, bool won)
        {
            string value = null;
            switch (language)
            {
                case "English":
                    switch (action)
                    {
                        case "Progress Stage":
                            progressStage(payload);
                            break;
                        case "Type Generator":
                            value = randomTypeGenerator();
                            break;
                        case "Account Type":
                            value = randomAccountType();
                            break;
                        case "Account Status":
                            value = randomAccountStatus();
                            break;
                        case "Category Generator":
                            value = randomCategoryGenerator(type);
                            break;
                        case "Description Generator":
                            value = randomDescriptionGenerator(type);
                            break;
                        /* case "Lead Source":
                             value = fetchLeadSource();
                             break; */
                        case "Note Generator":
                            value = randomNoteGenerator(type, payload.Values["AccountName"].ToString(), description);
                            break;
                        case "Location Generator":
                            value = randomLocationGenerator(type);
                            break;
                        case "Reason":
                            value = randomReason(won);
                            break;
                        case "Priority Generator":
                            value = randomPriorityGenerator();
                            break;
                        case "Fake Company Name":
                            value = GetFakeCompanyName();
                            break;
                        case "Fake First Name":
                            value = GetFakeFirstName();
                            break;
                        case "Fake Last Name":
                            value = GetFakeLastName();
                            break;
                    }
                    break;
                case "Chinese":
                    switch (action)
                    {
                        case "Progress Stage":
                            progressChineseStage(payload);
                            break;
                        case "Type Generator":
                            value = randomChineseTypeGenerator();
                            break;
                        case "Account Type":
                            value = randomChineseAccountType();
                            break;
                        case "Account Status":
                            value = randomChineseAccountStatus();
                            break;
                        case "Category Generator":
                            value = randomChineseCategoryGenerator(type);
                            break;
                        case "Description Generator":
                            value = randomChineseDescriptionGenerator(type);
                            break;
                        /*
                    case "Lead Source":
                        value = randomChineseLeadSource();
                        break; */
                        case "Note Generator":
                            value = randomChineseNoteGenerator(type, payload, description);
                            break;
                        case "Location Generator":
                            value = randomChineseLocationGenerator(type);
                            break;
                        case "Reason":
                            value = randomChineseReason(won);
                            break;
                        case "Priority Generator":
                            value = randomChinesePriorityGenerator();
                            break;
                        case "Fake Company Name":
                            value = GetFakeChineseCompanyName();
                            break;
                        case "Fake First Name":
                            value = GetFakeChineseFirstName();
                            break;
                        case "Fake Last Name":
                            value = GetFakeChineseLastName();
                            break;
                    }
                    break;
            }
            return value;
        }
        #endregion

        #region Getters/Setters
        // Simple accessors for use by AutoBot.cs
        
        public bool getStopCommand()
        {
            return stopCommand;
        }
        public bool getFirstRun()
        {
            return firstRun;
        }

        /*
        public DateTime getShiftAM()
        {
            return startWork;
        }

        public DateTime getShiftPM()
        {
            return endWork;
        } */
        
        public void setStopCommand(bool value)
        {
            stopCommand = value;
        }
        public void setFirstRun(bool value)
        {
            firstRun = value;
        }

        public string getEndPoint()
        {
            return endPoint;
        }
        #endregion

        #region Logging
        public void LogRemoval()
        {
            Log("Bot removed from service", fileName);
        }

        public virtual void Log(string message, string filename)
        {
            StreamWriter write = new StreamWriter(filename, true);
            write.WriteLine(message);
            write.Close();
        }

        public void LogException(Exception e)
        {
            Log(e.ToString(), fileName);
        }
        #endregion

    }
}
