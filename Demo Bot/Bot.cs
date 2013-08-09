using System;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
//Resources 9 and 10 are used for the task of creating the progress bar... nothing else.
using System.Drawing;
using System.Windows.Forms;
using Sage.Platform.Configuration;
using Sage.Platform.Data;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Framework;
using Sage.SData.Client.Metadata;
using Sage.SalesLogix;
using Sage.SalesLogix.Services;
using Sage.SalesLogix.Web.SData;
using ApplicationConetxt = Sage.Platform.Application.ApplicationContext;
// Allows for communication with Google documents (ie. Google Spreadsheets)
using Google.GData.Client;
using Google.GData.Spreadsheets;
// Allows for communication with Excel
using Excel = Microsoft.Office.Interop.Excel;


namespace Demo_Bot
{
    public class Bot
    {
        public SDataService service {get; set; }
        SDataService dynamic;
        public SDataUri Uri {get; set;}
        public string UserID { get; set; }
        public string Password { get; set; }
        public string role = "";
        static string authorizationUri = "", accessCode = "", docName = "";
        bool firstRun;
        decimal activityCompleteAmount;
        System.Windows.Forms.Label progressLabel;
        System.Windows.Forms.Label roleLabel;
        System.Windows.Forms.Label activitiesCreated;
        System.Windows.Forms.Label notesCreated;
        System.Windows.Forms.Label activitiesCompleted;
        System.Windows.Forms.Label leadCreated;
        System.Windows.Forms.Label accountCreated;
        System.Windows.Forms.Label contactCreated;
        System.Windows.Forms.Label oppCreated;
        System.Windows.Forms.Label ticketCreated;
        System.Windows.Forms.Label oppUpdated;
        System.Windows.Forms.Label leadPromoted;
        ComboBox roleSelector;
        bool activityCheckBox, noteCheckBox, completeActivityBox, leadCheckBox, accountCheckBox, contactCheckBox, oppCheckBox, ticketCheckBox, oppUpdateCheckBox, leadPromoteCheckBox;
        private bool stopCommand = false;
        public int notesCount, activitiesCount, activitiesCompleteCount, leadsCount, ticketsCount, opportunitiesCount, oppsUpdatedCount, accountsCount, contactsCount, leadsPromotedCount;
        public double reliability;
        Random rand = new Random();
        StreamWriter writer;
        string docPath = "C:/Swiftpage/";
        int upperBoundMonth = 0;
        DateTime startWork, endWork;
        string language = "English";
        string fileName = "";

        public Bot(string address, string startWorking, string endWorking, string userID, string password, System.Windows.Forms.Label progressLab, System.Windows.Forms.Label activitiesCreate, System.Windows.Forms.Label notesCreate, System.Windows.Forms.Label activitiesComplete, System.Windows.Forms.Label leadCreate, System.Windows.Forms.Label accountCreate, System.Windows.Forms.Label contactCreate, System.Windows.Forms.Label oppCreate, System.Windows.Forms.Label ticketCreate, System.Windows.Forms.Label oppUpdate, System.Windows.Forms.Label leadPromote, System.Windows.Forms.Label role, decimal activityCompleteAm, ComboBox roleSelect, bool noteCheck, bool activityCheck, bool leadCheck, bool accountCheck, bool contactCheck, bool oppCheck, bool ticketCheck, bool oppUpdateCheck, bool actCompleteCheck, bool leadPromoteCheck, decimal reliabilityValue, string creationUpper)
        {
            service = new SDataService("https://" + address + "/sdata/slx/system/-/") { UserName = userID, Password = password };
            dynamic = new SDataService("https://" + address + "/sdata/slx/dynamic/-/") { UserName = userID, Password = password };
            UserID = userID;
            Password = password;
            firstRun = true;
            progressLabel = progressLab;
            activitiesCreated = activitiesCreate;
            activitiesCompleted = activitiesComplete;
            activityCompleteAmount = activityCompleteAm;
            leadCreated = leadCreate;
            accountCreated = accountCreate;
            contactCreated = contactCreate;
            oppCreated = oppCreate;
            ticketCreated = ticketCreate;
            oppUpdated = oppUpdate;
            roleLabel = role;
            notesCreated = notesCreate;
            leadPromoted = leadPromote;
            roleSelector = roleSelect;
            noteCheckBox = noteCheck;
            activityCheckBox = activityCheck;
            leadCheckBox = leadCheck;
            accountCheckBox = accountCheck;
            contactCheckBox = contactCheck;
            oppCheckBox = oppCheck;
            ticketCheckBox = ticketCheck;
            oppUpdateCheckBox = oppUpdateCheck;
            completeActivityBox = actCompleteCheck;
            leadPromoteCheckBox = leadPromoteCheck;
            reliability = Convert.ToDouble(reliabilityValue);
            notesCount = 0;
            activitiesCount = 0;
            activitiesCompleteCount = 0;
            leadsCount = 0;
            ticketsCount = 0;
            opportunitiesCount = 0;
            oppsUpdatedCount = 0;
            accountsCount = 0;
            contactsCount = 0;
            leadsPromotedCount = 0;
            if (creationUpper != "")
                upperBoundMonth = Convert.ToInt32(creationUpper);
            else
                upperBoundMonth = 0;
            if (startWorking == "")
                startWork = Convert.ToDateTime("7:30AM");
            else
                startWork = Convert.ToDateTime(startWorking + "AM");
            if (endWorking == "")
                endWork = Convert.ToDateTime("6:30PM");
            else
                endWork = Convert.ToDateTime(endWorking + "PM");
            //writer = new StreamWriter(@"C:\Swiftpage\" + UserID + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".txt");
            fileName = @"C:\Swiftpage\" + UserID + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".txt";

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

                progressLabel.ForeColor = Color.White;
                SetText("Running Bot");
                SDataRequest request = new SDataRequest(Uri.ToString()) { UserName = UserID, Password = Password };
                try
                {
                    // Simulates the Bot as only functioning during some typical work schedule. This schedule excludes any work off due to holidays and weekends...
                    if (DateTime.Compare(DateTime.Now.ToUniversalTime(), startWork.ToUniversalTime()) >= 0 && DateTime.Compare(DateTime.Now.ToUniversalTime(), endWork.ToUniversalTime()) <= 0)
                    {
                        // Checks to see if the bot was first commanded to run, if so demonstrates that it can connect to the server.
                        if (firstRun)
                        {
                            SetActivitiesCreated("0");
                            SetNotesCreated("0");
                            SetCompletedActivities("0");
                            SetLeadsCreated("0");
                            SetAccountsCreated("0");
                            SetContactsCreated("0");
                            SetOppsCreated("0");
                            SetOppsUpdated("0");
                            SetTicketsCreated("0");
                            SetText("Progress:");
                            role = "General";//getRole();
                            SetRole(role);
                            SetText("Connecting to server...");
                            Log("Logging at: " + DateTime.Now + "\n================================================================\n", fileName);
                            SDataResponse response = request.GetResponse();
                            ok = (response.StatusCode == HttpStatusCode.OK);
                            if (ok == true)
                            {
                                //PerformStep();
                                SetText("Connected to server");
                            }
                            // If it cannot connect it displays an error message to the user and stops the bot from running.
                            else
                            {
                                progressLabel.ForeColor = Color.Crimson;
                                SetText("Unable to connect to server. Please try again.");
                                this.stop();
                            }
                            firstRun = false;
                            // bool value runningHelper needed to clarify which UI label to change.
                            //runningHelper = false;
                            switch (role)
                            {
                                case "General":
                                    runGeneral();
                                    SetText("Done");
                                    break;
                                default:
                                    runGeneral();
                                    SetText("General role ran");
                                    break;
                            }
                        }
                        else
                        {
                            //runningHelper = false;
                            switch (role)
                            {
                                case "General":
                                    runGeneral();
                                    SetText("Done");
                                    break;
                                default:
                                    runGeneral();
                                    SetText("General role ran...");
                                    break;
                            }
                            SetText("Waiting...");
                        }
                    }
                    else
                        return;
                } 
                catch (Sage.SData.Client.Framework.SDataException)
                {
                    progressLabel.ForeColor = Color.Crimson;
                    SetText("Invalid User Name. Please try again...");
                } 
        }

        public virtual void runFromGoogleSpreadSheet()
        {
            // Don't change. This allows the bot to connect to the Google account: Lee Hogan with e-mail saleslogixbotdev@gmail.com and PW: Developer
            // Information grabbed from https://code.google.com/
            string Client_Id = "780347832849.apps.googleusercontent.com";
            string Client_Secret = "b1LqVrqxc7wIggZ48oxi35H3";
            string Scope = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
            string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";

            OAuth2Parameters parameters = new OAuth2Parameters();
            parameters.ClientId = Client_Id;
            parameters.ClientSecret = Client_Secret;
            parameters.RedirectUri = RedirectUri;
            parameters.Scope = Scope;
            authorizationUri = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);

            string title = "Google Verification";
            string message = "Please visit the URL below to authorize your OAuth request token and type in the access code into the textbox.\nThen click OK to continue...";

            if (InputBox(title, message, ref accessCode) == DialogResult.OK)
            {
                parameters.AccessCode = accessCode;

                OAuthUtil.GetAccessToken(parameters);
                string accessToken = parameters.AccessToken;
                Console.WriteLine("OAuth Access Token: " + accessToken);

                GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", parameters);
                SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
                service.RequestFactory = requestFactory;

                SpreadsheetQuery query = new SpreadsheetQuery();
                SpreadsheetFeed feed = service.Query(query);

                string title2 = "Document Name";
                string message2 = "Please enter the name of the document you wish the bot to read from.";
                SpreadsheetEntry doc = new SpreadsheetEntry();

                if (InputBox2(title2, message2, ref docName) == DialogResult.OK)
                {
                    foreach (SpreadsheetEntry entry in feed.Entries)
                    {
                        if (entry.Title.Text == docName)
                        {
                            doc = entry;
                            WorksheetFeed wsFeed = doc.Worksheets;
                            foreach (WorksheetEntry ws in wsFeed.Entries)
                            {
                                
                                CellQuery cellQuery = new CellQuery(ws.CellFeedLink);
                                CellFeed cellFeed = service.Query(cellQuery);
                                int row = 2;
                                List<string> payload = new List<string>();
                                // Iterate through each cell, printing its value. DOES NOT READ THE LAST CELL!
                                foreach (CellEntry cell in cellFeed.Entries)
                                {
                                    
                                    if (cell.Title.Text == "A" + row)
                                    {
                                        completeDocRead(payload);
                                        payload = new List<string>();
                                        row++;
                                    }
                                    payload.Add(cell.InputValue);

                                    // Print the cell's address in A1 notation
                                    //Console.WriteLIne(cell.Title.Text);
                                    // Print the cell's address in R1C1 notation
                                    //Console.WriteLine(cell.Id.Uri.Content.Substring(cell.Id.Uri.Content.LastIndexOf("/") + 1));
                                    // Print the cell's formula or text value
                                    //Console.WriteLIne(cell.InputValue);
                                    // Print the cell's calculated value if the cell's value is numeric
                                    // Prints empty string if cell's value is not numeric
                                   // Console.WriteLine(cell.NumericValue);
                                    // Print the cell's displayed value (useful if the cell has a formula)
                                    //Console.WriteLine(cell.Value);
                                }
                                
                                 
                                /*
                                Google.GData.Client.AtomLink listFeedLink = ws.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

                                ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
                                ListFeed listFeed = service.Query(listQuery);
                                int rowNumber = 1;
                                foreach (ListEntry row in listFeed.Entries)
                                {
                                    string action = row.Title.Text;

                                    switch (action)
                                    {
                                        case "Activity":
                                            activity(rowNumber, ws, service);
                                            break;
                                        case "Complete Activity":
                                            break;
                                        case "Lead":
                                            break;
                                        case "Note":
                                            break;
                                        case "Opportunity":
                                            break;
                                        case "Promote Lead":
                                            break;
                                        case "Ticket":
                                            break;
                                        case "Update Opportunity":
                                            break;
                                    }
                                    rowNumber++;
                                
                                }   */
                            }
                     
                            return;
                        }
                    }
                }

            }
        }

        public virtual void runFromExcel()
        {
            Excel.Application application = new Excel.Application();

            string title = "Excel Document Lookup";
            string message = "Please enter the name of the Excel file you with to load.\nAlso make sure the file is located in C:'\'Swiftpage";

            if (InputBox2(title, message, ref docName) == DialogResult.OK)
            {
                Excel.Workbook workBook = application.Workbooks.Open(@"C:\Swiftpage\" + docName + ".xlsx");
                Excel._Worksheet workSheet = workBook.Sheets[1];
                Excel.Range range = workSheet.UsedRange;

                int rowCount = range.Rows.Count;
                int columnCount = range.Columns.Count;
                List<string> payload = new List<string>();

                for (int i = 1; i <= rowCount; i++)
                {
                    if (i >= 2)
                    {
                        completeDocRead(payload);
                        payload = new List<string>();
                    }

                    for (int j = 1; j <= columnCount; j++)
                    {
                        payload.Add(range.Cells[i, j].Value2.ToString());
                    }
                }
            }
	        
        }

        private void completeDocRead(List<string> payload)
        {
            string action = payload[0];
            switch (action.ToLower())
            {
                    // From here the program should read each payload value that are desired for each case and then call the function
                    // by passing each payload value into the function as well.
                case "activity":
                    break;
                case "complete activity":
                    break;
                case "contact":
                    break;
                case "lead":
                    break;
                case "note":
                    break;
                case "opportunity":
                    break;
                case "promote lead":
                    break;
                case "ticket":
                    break;
                case "update opportunity":
                    break;
            }
        }

        // Below are the roles of the bot
        #region Roles
        /*
        private void runDataCreator()
        {
            
            double value = 4 * rand.NextDouble();
            if (value < 1.5)
                return;
            if (value < 2.5)
            {
                SetText("Making activity");
                makeActivity();
                SetText("Activity created");
                return;
            }
            if (value < 3.5)
            {
                SetText("Making note");
                makeNote();
                SetText("Note created");
                return;
            }
            if (value < 4)
            {
                SetText("Making activity");
                makeActivity();
                SetText("Activity created");
                SetText("Making note");
                makeNote();
                SetText("Note created");
                return;
            }
        }

        private bool runTicketMaker()
        {
            int choice = rand.Next(0, 100);
            if (choice < 10)
            {
                makeTicket();
                return true;
            }
            else
                return false;
        }

        private void runHelper()
        {
            runningHelper = true;
            bool didRun = false;
            SetText("Trying making a ticket");
            didRun = runTicketMaker();
            if (didRun)
                SetText("Ticket created");
            else
                SetText("Did not create ticket");
            SetText("Trying making a lead");
            didRun = runLeadGenerator();
            if (didRun)
                SetText("Lead created");
            else
                SetText("Did not create lead");
            SetText("Trying making an opportunity");
            //didRun = runOpportunityGenerator();
            if (didRun)
                SetText("Opportunity created");
            else
            {
                SetText("Did not create opportunity");
                int temp = 34;//rand.Next(0,100);
                if (temp < 35)
                {
                    SetText("Updating opportunity");
                    updateOpportunity();
                    SetText("Opportunity updated");
                }
            }
        }

        private bool runLeadGenerator()
        {
            makeContact();
            promoteLead();
            return true;
            int choice = rand.Next(0, 100);
            if (choice < 10)
            {
                makeLead();
                return true;
            }
            else
                return false; 
        }

        private bool runOpportunityGenerator()
        {
            //makeAccount();
            //deleteContact();
            //deleteActivity();
            //deleteAccount();
            deleteNote();
            deleteTicket();
            deleteLead();
            deleteOpportunity();
            return true;
            
            int choice = rand.Next(0, 100);
            if (choice < 10)
            {
                makeOpportunity();
                return true;
            }
            else
                return false; 
            
        }
    */
        private void runGeneral()
        {
            double tempReliability = reliability / 100;
            double reliable = rand.NextDouble();
            if (noteCheckBox == true && reliable < tempReliability)
                makeNote();

            reliable = rand.NextDouble();
            if (activityCheckBox == true && reliable < tempReliability)
                makeActivity();

            reliable = rand.NextDouble();
            if (leadCheckBox == true && reliable < tempReliability)
                makeLead();

            reliable = rand.NextDouble();
            if (accountCheckBox == true && reliable < tempReliability)
                makeAccount();

            reliable = rand.NextDouble();
            if (contactCheckBox == true && reliable < tempReliability)
                makeContact();

            reliable = rand.NextDouble();
            if (oppCheckBox == true && reliable < tempReliability)
                makeOpportunity();

            reliable = rand.NextDouble();
            if (completeActivityBox == true && reliable < tempReliability)
                completeActivity();

            reliable = rand.NextDouble();
            if (oppUpdateCheckBox == true && reliable < tempReliability)
                updateOpportunity();

            reliable = rand.NextDouble();
            if (leadPromoteCheckBox == true && reliable < tempReliability)
                promoteLead();

            reliable = rand.NextDouble();
            if (ticketCheckBox == true && reliable < tempReliability)
                makeTicket();


            /*
            // Previously used when was implementing 'Roles'
            double value = 8 * rand.NextDouble();
            if (value < 3.5)
                return;
            if (value < 4.75)
            {
                SetText("Making activity");
                makeActivity();
                SetText("Activity created");
                return;
            }
            if (value < 6)
            {
                SetText("Making note");
                makeNote();
                SetText("Note created");
                return;
            }
            if (value < 7.25)
            {
                SetText("Completing previous activities");
                completeActivity();
                SetText("Previous activities completed");
                return;
            }
            if (value < 7.75)
            {
                SetText("Making activity");
                makeActivity();
                SetText("Activity created");
                SetText("Making note");
                makeNote();
                SetText("Note created");
                return;
            }
            if (value < 8.25)
            {
                SetText("Making activity");
                makeActivity();
                SetText("Activity created");
                SetText("Completing previous activities");
                completeActivity();
                SetText("Previous activities completed");
                return;
            }
            if (value < 8.75)
            {
                SetText("Making note");
                makeNote();
                SetText("Note created");
                SetText("Completing previous activities");
                completeActivity();
                SetText("Previous activities completed");
                return;
            }
            if (value < 9)
            {
                SetText("Making activity");
                makeActivity();
                SetText("Activity created");
                SetText("Making note");
                makeNote();
                SetText("Note created");
                SetText("Completing previous activities");
                completeActivity();
                SetText("Previous activities completed");
                return;
            }
            */
        }

        public string roleSetter()
        {
            string role = "";
            
            double value = 4 * rand.NextDouble();
            if (value >= 3)
                role = "Data Creator";
            else
            {
                if (value >= 2)
                    role = "Helper";
                else
                {
                    if (value >= 1)
                        role = "Lead Generator";
                    else
                        role = "General";
                }
            }
            return role;
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

        // Not utilized in this build, was a POC that did not get implemented/was found to be unnecessary upon creation of
        // a viable UI for the program.
        #region Spreadsheet Data Creation
        // Functions that create entries into the database.
        public void note(List<string> write)
        {
            try
            {
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value generator as defined below the creation functions.
                string type = "atNote";
                string category = "Note";
                //string description = randomDescriptionGenerator(category);
                SDataPayload accountPayload = null;
                int i = 0;
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);

                if (i == 50)
                    return;
                //string notes = randomNoteGenerator(category, accountPayload, description);

                int accId = rand.Next(2000);

                SDataTemplateResourceRequest noteHistoryTemplate = new SDataTemplateResourceRequest(dynamic);
                noteHistoryTemplate.ResourceKind = "history";
                Sage.SData.Client.Atom.AtomEntry tempEntry = noteHistoryTemplate.Read();

                SDataPayload payload = tempEntry.GetSDataPayload();
                payload.Values["Type"] = type;
                payload.Values["Category"] = category;
                payload.Values["Description"] = write[3];
                payload.Values["Notes"] = write[2];
                payload.Values["LongNotes"] = write[3];
                payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                payload.Values["AccountId"] = accountPayload.Key;

                // Checks if there is an associated contact with the account.
                if (accountPayload.Values["Contacts"] != null)
                {
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
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
                        payload.Values["ContactName"] = contactPayload.Values["Name"];
                        payload.Values["ContactId"] = contactPayload.Key;
                    }
                }

                // Checks if there is an associated opportunity with the account, similar to how the contact was found.
                if (accountPayload.Values["Opportunities"] != null)
                {
                    SDataResourceCollectionRequest opp = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "opportunities",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = opp.Read();
                    SDataPayload oppPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            oppPayload = entry.GetSDataPayload();
                            break;
                        }
                        payload.Values["OpportunityName"] = oppPayload.Values["Description"];
                        payload.Values["OpportunityId"] = oppPayload.Key;
                    }
                }

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
                payload.Values["StartDate"] = DateTimeOffset.Now.ToUniversalTime();
                payload.Values["CompletedDate"] = DateTime.Now.ToUniversalTime();

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "history",
                    Entry = tempEntry
                };

                request.Create();
                notesCount++;
                SetNotesCreated(notesCount.ToString());
                Log("Created Note: " + payload.Values["Description"] + "... at time " + DateTime.Now, fileName);
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }

        }

        // Functional
        public void noteFor(SDataPayload opportunityPayload)
        {
            try
            {
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value generator as defined below the creation functions.
                string type = "atNote";
                string category = "Note";
                string description = randomDescriptionGenerator(category);
                SDataPayload key = (SDataPayload)opportunityPayload.Values["Account"];
                SDataSingleResourceRequest getAccount = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    ResourceSelector = "'" + key.Key + "'"
                };
                var rawr = getAccount.Read();
                SDataPayload accountPayload = rawr.GetSDataPayload();
                string notes = randomNoteGenerator(category, accountPayload, description);

                int accId = rand.Next(2000);

                SDataTemplateResourceRequest noteHistoryTemplate = new SDataTemplateResourceRequest(dynamic);
                noteHistoryTemplate.ResourceKind = "history";
                Sage.SData.Client.Atom.AtomEntry tempEntry = noteHistoryTemplate.Read();

                SDataPayload payload = tempEntry.GetSDataPayload();
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
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
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
                        payload.Values["ContactName"] = contactPayload.Values["Name"];
                        payload.Values["ContactId"] = contactPayload.Key;
                    }
                }

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "history",
                    Entry = tempEntry
                };

                request.Create();
                notesCount++;
                SetNotesCreated(notesCount.ToString());
                Log("Created Note: " + payload.Values["Description"] + "... at time " + DateTime.Now, fileName);
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }
        }

        // Functional
        public void activity(List<string> write)
        {
            try
            {
                SDataTemplateResourceRequest activityTemplate = new SDataTemplateResourceRequest(service);
                activityTemplate.ResourceKind = "activities";
                Sage.SData.Client.Atom.AtomEntry tempEntry = activityTemplate.Read();

                SDataPayload payload = tempEntry.GetSDataPayload();

                payload.Values["Type"] = write[1];
                
                /*
                payload.Values["Category"] = category;
                // Get the program to query the server for the contact name, account name, and retrieve the respective ids for each.
                payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                payload.Values["AccountId"] = accountPayload.Key;
                // Checks to make sure there is a contact associated with the account, and if so calls a request to get the payload associated to that contact; then filling in payload.Values
                if (accountPayload.Values["Contacts"] != null)
                {
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
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
                        payload.Values["ContactName"] = contactPayload.Values["Name"];
                        payload.Values["ContactId"] = contactPayload.Key;
                    }
                }

                if (temp != "Personal")
                {
                    // Checks if there is an associated opportunity with the account, similar to how the contact was found.
                    if (accountPayload.Values["Opportunities"] != null)
                    {
                        SDataResourceCollectionRequest opp = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "opportunities",
                            QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                        };
                        var feed = opp.Read();
                        SDataPayload oppPayload = null;
                        if (feed.Entries.Count() != 0)
                        {
                            foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                            {
                                oppPayload = entry.GetSDataPayload();
                                break;
                            }
                            payload.Values["OpportunityName"] = oppPayload.Values["Description"];
                            payload.Values["OpportunityId"] = oppPayload.Key;
                        }
                    }

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
                }
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

                //Creating the entry...
                request.Create();
                activitiesCount++;
                SetActivitiesCreated(activitiesCount.ToString());
                 * */
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }
                
        }

        // Functional
        public void activityFor(SDataPayload opportunityPayload)
        {
            try
            {
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value generator as defined below the creation functions.
                string temp = randomTypeGenerator();
                string category = randomCategoryGenerator(temp);
                string description = randomDescriptionGenerator(temp);
                //if (temp == "ToDo" && (description != "Send proposal" || description != "Send quote"))
                //{ temp = todoDecoder(description); }
                string type = "at" + temp;
                string location = randomLocationGenerator(temp);
                DateTime startTime = randomDateGenerator();
                string priority = randomPriorityGenerator();
                SDataPayload key = (SDataPayload)opportunityPayload.Values["Account"];
                SDataSingleResourceRequest getAccount = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    ResourceSelector = "'" + key.Key + "'"
                };
                var rawr = getAccount.Read();
                SDataPayload accountPayload = rawr.GetSDataPayload();
                string notes = randomNoteGenerator(temp, accountPayload, description);
                DateTime alarm = startTime.AddMinutes(-15);
                DateTime duration;

                SDataTemplateResourceRequest activityTemplate = new SDataTemplateResourceRequest(service);
                activityTemplate.ResourceKind = "activities";
                Sage.SData.Client.Atom.AtomEntry tempEntry = activityTemplate.Read();

                SDataPayload payload = tempEntry.GetSDataPayload();

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

                //Creating the entry...
                request.Create();
                activitiesCount++;
                //SetActivitiesCreated(activitiesCount.ToString());
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }
        }

        // Needs help!
        public void contact()
        {
            SDataTemplateResourceRequest contactTemplate = new SDataTemplateResourceRequest(dynamic);
            contactTemplate.ResourceKind = "contacts";
            Sage.SData.Client.Atom.AtomEntry tempEntry = contactTemplate.Read();
            SDataPayload payload = tempEntry.GetSDataPayload();
            SDataPayload accountPayload = null;
            int i = 0;
            do
            {
                accountPayload = fetchAccount();
                i++;
            } while (accountPayload == null && i < 50);

            if (i == 50)
                return;

            string firstName = GetFakeFirstName();
            string lastName = GetFakeLastName();

            if (accountPayload.Values["Contacts"] != null)
            {
                SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "contacts",
                    QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                };
                var feed = contact.Read();
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
                                firstName = GetFakeFirstName();
                                lastName = GetFakeLastName();
                            } while (contactPayload.Values["FirstName"] == firstName && contactPayload.Values["LastName"] == lastName);
                        }

                    }
                    payload.Values["FirstName"] = firstName;
                    payload.Values["LastName"] = lastName;
                    payload.Values["LastNameUpper"] = lastName.ToUpper();
                    payload.Values["NameLF"] = lastName + ", " + firstName;
                    payload.Values["Name"] = firstName + " " + lastName;
                    payload.Values["FullName"] = lastName + " , " + firstName;
                    payload.Values["NamePFL"] = " " + firstName + " " + lastName;
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

            payload.Values["AccountName"] = accountPayload.Values["AccountName"];
            payload.Values["Account"] = accountPayload;
            payload.Values["CreateDate"] = DateTime.Now;
            payload.Values["CreateUser"] = UserID;
            payload.Values["Email"] = firstName + lastName + "@" + emailProvider + ".com";
            string phone = rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString();
            payload.Values["WorkPhone"] = phone;
            payload.Values["Mobile"] = phone;
            payload.Values["DoNotEmail"] = false;
            payload.Values["DoNotFax"] = false;
            payload.Values["DoNotMail"] = false;
            payload.Values["DoNotPhone"] = false;
            payload.Values["DoNotSolicit"] = false;
            payload.Values["IsServiceAuthorized"] = false;
            payload.Values["IsPrimary"] = false;
            payload.Values["Status"] = "Active";
            payload.Values["Owner"] = "Everyone";
            payload.Values["PreferredContact"] = "Unknown";


            tempEntry.SetSDataPayload(payload);

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "contacts",
                Entry = tempEntry
            };
            request.Create();
            Log("Created contact: " + payload.Values["Name"], fileName);

        }

        // Functional
        public void lead()
        {
            SDataTemplateResourceRequest leadsTemplate = new SDataTemplateResourceRequest(dynamic);
            leadsTemplate.ResourceKind = "leads";

            bool checker = true;
            string firstName = "";
            string lastName = "";

            Sage.SData.Client.Atom.AtomEntry tempEntry = leadsTemplate.Read();
            SDataPayload payload = tempEntry.GetSDataPayload();
            // Checks to see if there is a lead with that name already created
            do
            {
                firstName = GetFakeFirstName();
                lastName = GetFakeLastName();
                SDataResourceCollectionRequest check = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "contacts",
                    QueryValues = { { "where", "LastName eq '" + lastName + "'" } }
                };
                var feed = check.Read();
                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                {
                    SDataPayload tempPayload = entry.GetSDataPayload();
                    if ((string)tempPayload.Values["FirstName"] == firstName)
                    {
                        checker = true;
                        break;
                    }
                    else
                        checker = false;
                }
            } while (checker);

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
            string phone = rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString();
            payload.Values["CreateUser"] = UserID;
            payload.Values["CreateDate"] = DateTime.Now.ToUniversalTime();
            payload.Values["Company"] = GetFakeCompanyName();
            payload.Values["Email"] = firstName.ToLower() + lastName.ToLower() + "@" + emailProvider + ".com";
            payload.Values["FirstName"] = firstName;
            payload.Values["LastName"] = lastName;
            payload.Values["LastNameUpper"] = lastName.ToUpper();
            payload.Values["Mobile"] = phone;
            payload.Values["LeadNameFirstLast"] = firstName + " " + lastName;
            payload.Values["LeadNameLastFirst"] = lastName + ", " + firstName;

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "leads",
                Entry = tempEntry
            };

            /*SDataServiceOperationRequest request = new SDataServiceOperationRequest(service)
            {
                ResourceKind = "leads",
                //OperationName = "Save",
                Entry = tempEntry
            }; */
            request.Create();
            Debug.WriteLine(payload.Values["Company"] + ", " + payload.Values["LeadNameFirstLast"]);
            leadsCount++;
                // In this case the notes created label becomes the leads generated label
            SetLeadsCreated(leadsCount.ToString());
        }

        // Functional
        public void opportunity()
        {
            SDataTemplateResourceRequest opportunityTemplate = new SDataTemplateResourceRequest(dynamic);
            opportunityTemplate.ResourceKind = "opportunities";

            Sage.SData.Client.Atom.AtomEntry tempEntry = opportunityTemplate.Read();
            SDataPayload payload = tempEntry.GetSDataPayload();

            SDataPayload accountPayload = null;
            int i = 0;
            do
            {
                accountPayload = fetchAccount();
                i++;
            } while (accountPayload == null & i < 50);

            if (i == 50)
                return;

            int oppValue = 500 * rand.Next(1, 1000);
            DateTime closeDate = DateTime.Now;
            closeDate = closeDate.AddMonths(3);
            int month = rand.Next(0, 12);
            int day = rand.Next(0, 30);
            closeDate = closeDate.AddMonths(month);
            closeDate = closeDate.AddDays(day);

            payload.Values["ActualAmount"] = oppValue;
            payload.Values["CreateDate"] = DateTime.Now;
            payload.Values["CreateUser"] = UserID;
            payload.Values["Description"] = accountPayload.Values["AccountName"] + " - Phase " + rand.Next(0, 10);
            payload.Values["Account"] = accountPayload;
            payload.Values["Owner"] = UserID;
            payload.Values["SalesAmount"] = oppValue;
            payload.Values["SalesPotential"] = oppValue;
            payload.Values["CloseProbability"] = 5 * rand.Next(0, 20);
            payload.Values["EstimatedClose"] = closeDate;

            if (accountPayload.Values["Contacts"] != null)
            {
                SDataBatchRequest contact = new SDataBatchRequest(dynamic)
                {
                    ResourceKind = "contacts",
                    QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                };


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

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "opportunities",
                Entry = tempEntry
            };
            request.Create();
            opportunitiesCount++;
            SetOppsCreated(opportunitiesCount.ToString());
            Debug.WriteLine("Opportunity made for account: " + accountPayload.Values["AccountName"]);
        }

        // Functional
        public void ticket()
        {
            SDataTemplateResourceRequest ticketTemplate = new SDataTemplateResourceRequest(dynamic);
            ticketTemplate.ResourceKind = "tickets";

            Sage.SData.Client.Atom.AtomEntry tempEntry = ticketTemplate.Read();
            SDataPayload payload = tempEntry.GetSDataPayload();

            SDataPayload accountPayload = null;
            int i = 0;
            do
            {
                accountPayload = fetchAccount();
                i++;
            } while (accountPayload == null && i < 50);

            if (i == 50)
                return;

            //accountPayload.Values["UserField1"] = UserID;

            // Only need account name for the payload to be complete
            payload.Values["Account"] = accountPayload;
            try
            {
                if (accountPayload.Values["Contacts"] != null)
                {
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
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
                }
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }

            tempEntry.SetSDataPayload(payload);

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
            {
                ResourceKind = "tickets",
                Entry = tempEntry
            };
            request.Create();
            //Sage.SData.Client.Atom.AtomEntry ent = request.Read();
            //SDataPayload tempLoad = ent.GetSDataPayload();
            Debug.WriteLine("Created ticket number: " + accountPayload.Values["AccountName"]);
            ticketsCount++;
            // in this case notes created is for tickets for helper and for ticker maker
            SetNotesCreated(ticketsCount.ToString());
            //SDataServiceOperationRequest

        }

        // Functional
        public void completeSpecificActivity()
        {
            try
            {
                // Initiates a value to keep track of amount of activities created.
                int i = 0;

                var request = new SDataServiceOperationRequest(service)
                {
                    ResourceKind = "activities",
                    OperationName = "Complete",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };

                SDataResourceCollectionRequest activities = new SDataResourceCollectionRequest(service)
                {
                    ResourceKind = "activities",
                    QueryValues = { { "where", "CreateUser eq '" + UserID + "'" }, { "orderBy", "StartDate" } }
                };

                var feed = activities.Read();

                // From the Whitepaper pdf
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
                    if (UserID == "admin" && (string)payload.Values["Type"] == "atPersonal")
                        allow = false;


                    // Checks if the amount of activities created is equal to the amount desired.
                    // Current problem resultant from changing the service to /system/
                    if (allow && (string)payload.Values["Description"] != "")
                    {
                        if (DateTime.Compare(stime, DateTime.Now) < 0)
                        {

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
                               {"UserId", UserID},
                               {"Result", "Complete"},
                               {"CompleteDate", DateTime.Now.ToUniversalTime()}
                                    }
                           }
                        }
                                 }
                                   });

                                request.Create();
                                i++;
                                activitiesCompleteCount++;
                                SetCompletedActivities(activitiesCompleteCount.ToString());
                            }
                            catch (Exception e)
                            {
                                Log(e.ToString(),fileName);;
                            }
                        }
                    }

                }
            }
            catch (ArgumentNullException e)
            {
            }
        }

        // Needs help!
        public void promoteSpecificLead()
        {
            SDataTemplateResourceRequest contactTemplate = new SDataTemplateResourceRequest(dynamic);
            contactTemplate.ResourceKind = "contacts";

            Sage.SData.Client.Atom.AtomEntry tempEntry = contactTemplate.Read();
            SDataPayload payload = tempEntry.GetSDataPayload();

            Sage.SData.Client.Atom.AtomEntry leadEntry = null;
            do
            {
                leadEntry = fetchLead();
            } while (leadEntry == null);

            SDataPayload leadPayload = leadEntry.GetSDataPayload();

            SDataResourceCollectionRequest search = new SDataResourceCollectionRequest(dynamic)
            {
                ResourceKind = "accounts",
                QueryValues = { { "where", "AccountName eq '" + leadPayload.Values["Company"] + "'" } }
            };

            var feed = search.Read();
            bool test = false;
            foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
            {
                if (entry != null)
                {
                    test = true;
                    break;
                }
                else
                    test = false;
            }

            if (test)
            {
                SDataServiceOperationRequest request = new SDataServiceOperationRequest(service)
                {
                    ResourceKind = "leads",
                    Entry = leadEntry,
                    OperationName = "ConvertLeadToContact"
                };
                request.Create();
                Debug.WriteLine("Converted " + leadPayload.Values["FirstName"] + " " + leadPayload.Values["LastName"] + " to a contact");
                /*
                payload.Values["AccountName"] = leadPayload.Values["Company"];
                payload.Values["CreateDate"] = DateTime.Now;
                payload.Values["CreateUser"] = UserID;
                payload.Values["CuisinePreference"] = "Asian, Grill, or Mexican";
                payload.Values["Email"] = leadPayload.Values["Email"];
                payload.Values[""] = "";*/
            }
            else
            {
                SDataServiceOperationRequest request = new SDataServiceOperationRequest(service)
                {
                    ResourceKind = "leads",
                    Entry = leadEntry,
                    OperationName = "ConvertLeadToAccount"
                };
                request.Create();
                Debug.WriteLine("Converted " + leadPayload.Values["FirstName"] + " " + leadPayload.Values["LastName"] + " to a contact with Account " + leadPayload.Values["Company"]);
            }


        }

        // Functional, updated to include transitions between stages...
        public void updateSpecificOpportunity()
        {
            Sage.SData.Client.Atom.AtomEntry entry = null;
            SDataPayload payload = null;
            int counter = 0;
            int counter2 = 0;
            do
            {
                do
                {
                    do
                    {
                        entry = fetchOpportunity();
                    } while (entry == null);
                    counter++;
                    if (counter == 50)
                    {
                        Debug.WriteLine("Unable to locate a valid opportunity.");
                        return;
                    }
                } while (entry == null);
                payload = entry.GetSDataPayload();
                counter2++;
                if (counter2 == 50)
                {
                    Debug.WriteLine("Unable to locate a valid opportunity (second do-while)");
                    return;
                }
            } while (((string)payload.Values["Closed"]) == "true");

            if ((string)payload.Values["CloseProbability"] == 100.ToString())
            {
                string reason = randomReason(true);
                payload.Values["Closed"] = true;
                payload.Values["CloseProbability"] = 100;
                payload.Values["ActualClose"] = DateTime.Now;
                payload.Values["Win"] = true;
                payload.Values["Reason"] = reason;
                payload.Values["Status"] = "Closed - Won";

                entry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "opportunities",
                    Entry = entry
                };

                request.Update();

                Debug.WriteLine("Updated: " + payload.Values["Description"]);
                return;
            }
            else
            {

                double choice = 4 * rand.NextDouble();
                if (choice < 1.6)
                {
                    if (choice < .3)
                    {
                        // Close the opportunity (either winning or losing it based on the 'close prob'
                        string percent = payload.Values["CloseProbability"].ToString();
                        int testMyLuck = rand.Next(0, 100);
                        if (testMyLuck <= Convert.ToInt32(percent))
                        {
                            // Winner winner! Close the opportunity with a win.
                            // Is this a service request? Close-win?
                            string reason = randomReason(true);
                            payload.Values["Closed"] = true;
                            payload.Values["CloseProbability"] = 100;
                            payload.Values["ActualClose"] = DateTime.Now;
                            payload.Values["Win"] = true;
                            payload.Values["Reason"] = reason;
                            payload.Values["Status"] = "Closed - Won";

                            entry.SetSDataPayload(payload);

                            SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                            {
                                ResourceKind = "opportunities",
                                Entry = entry
                            };

                            request.Update();

                            Debug.WriteLine("Updated: " + payload.Values["Description"]);
                            return;
                        }
                        else
                        {
                            // Ruh roh! You lost, close the opportunity with a 'lost' value
                            string reason = randomReason(false);
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

                            request.Update();

                            Debug.WriteLine("Updated: " + payload.Values["Description"]);
                            return;
                        }
                    }
                    else
                    {
                        // Add a note to the opportunity
                        makeNoteFor(payload);
                        Debug.WriteLine("Note made for: " + payload.Values["Description"]);
                        return;
                    }
                }
                else
                {
                    if (choice >= 2.9)
                    {
                        // Updates the sales process
                        progressStage(payload);

                        entry.SetSDataPayload(payload);

                        SDataSingleResourceRequest requested = new SDataSingleResourceRequest(dynamic)
                        {
                            ResourceKind = "opportunities",
                            Entry = entry
                        };
                        requested.Update();
                        Debug.WriteLine("Opportunity: " + payload.Values["Description"] + " moved to the next stage");
                    }
                    else
                    {
                        // Add an activity to the opportunity
                        makeActivityFor(payload);
                        Debug.WriteLine("Activity made for: " + payload.Values["Description"]);
                    }
                }
            }
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
                Debug.WriteLine("Deleted: " + name);
            }
            catch (Exception e) {
                Log(e.ToString(),fileName);; 
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
                Debug.WriteLine("Deleted: " + accountName);
            }
            catch (Exception e) {
                Log(e.ToString(),fileName); 
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
                Debug.WriteLine("Deleted: " + activity + " " + startDate);
            }
            catch (Exception e) { 
                Log(e.ToString(),fileName);
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
                Debug.WriteLine("Deleted: " + note);
            }
            catch (Exception e) { 
                Log(e.ToString(),fileName); 
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
                Debug.WriteLine("Deleted: " + lead);
            }
            catch (Exception e) { 
                Log(e.ToString(),fileName);
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
                Debug.WriteLine("Deleted: " + opportunity);
            }
            catch (Exception e) { 
                Log(e.ToString(),fileName);
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
                Debug.WriteLine("Deleted: " + ticket);
            }
            catch (Exception e) { 
                Log(e.ToString(),fileName);
            }
        }

        #endregion

        #region RandomCreation

        // Functions that create entries into the database.
        public void makeNote()
        {
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
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);

                if (i == 50)
                    return;

                string notes = localize(language, "Note Generator", accountPayload, category, description, true);
                
                int accId = rand.Next(2000);

                SDataTemplateResourceRequest noteHistoryTemplate = new SDataTemplateResourceRequest(dynamic);
                noteHistoryTemplate.ResourceKind = "history";
                Sage.SData.Client.Atom.AtomEntry tempEntry = noteHistoryTemplate.Read();

                SDataPayload payload = tempEntry.GetSDataPayload();
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
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
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
                        payload.Values["ContactName"] = contactPayload.Values["Name"];
                        payload.Values["ContactId"] = contactPayload.Key;
                    }
                }

                // Checks if there is an associated opportunity with the account, similar to how the contact was found.
                if (accountPayload.Values["Opportunities"] != null)
                {
                    SDataResourceCollectionRequest opp = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "opportunities",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };
                    var feed = opp.Read();
                    SDataPayload oppPayload = null;
                    if (feed.Entries.Count() != 0)
                    {
                        foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                        {
                            oppPayload = entry.GetSDataPayload();
                            break;
                        }
                        payload.Values["OpportunityName"] = oppPayload.Values["Description"];
                        payload.Values["OpportunityId"] = oppPayload.Key;
                    }
                }

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
                payload.Values["StartDate"] = DateTimeOffset.Now.ToUniversalTime();
                payload.Values["CompletedDate"] = DateTime.Now.ToUniversalTime();

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "history",
                    Entry = tempEntry
                };

                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                notesCount++;
                SetNotesCreated(notesCount.ToString());
                Log(DateTime.Now + " - Created Note: " + payload.Values["Description"] + " - " + timed + " seconds", fileName);
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }

        }

        // Functional
        public void makeNoteFor(SDataPayload opportunityPayload)
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                // Initializing the variables used to populate the payload. Each variable gets a value using a random value generator as defined below the creation functions.
                string type = "atNote";
                string category = "Note";
                string description = localize(language, "Description Generator", null, category, null, true);
                SDataPayload key = (SDataPayload)opportunityPayload.Values["Account"];
                SDataSingleResourceRequest getAccount = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    ResourceSelector = "'" + key.Key + "'"
                };
                var rawr = getAccount.Read();
                SDataPayload accountPayload = rawr.GetSDataPayload();
                string notes = localize(language, "Note Generator", accountPayload, category, description, true);

                int accId = rand.Next(2000);

                SDataTemplateResourceRequest noteHistoryTemplate = new SDataTemplateResourceRequest(dynamic);
                noteHistoryTemplate.ResourceKind = "history";
                Sage.SData.Client.Atom.AtomEntry tempEntry = noteHistoryTemplate.Read();

                SDataPayload payload = tempEntry.GetSDataPayload();
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
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
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
                        payload.Values["ContactName"] = contactPayload.Values["Name"];
                        payload.Values["ContactId"] = contactPayload.Key;
                    }
                }

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "history",
                    Entry = tempEntry
                };

                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                notesCount++;
                SetNotesCreated(notesCount.ToString());
                Log(DateTime.Now + " - Created Note: " + payload.Values["Description"] + " - " + timed + " seconds", fileName);
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }
        }
                
        // Functional
        public void makeActivity()
        {
            try
            {
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
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);

                if (i == 50)
                    return;

                string notes = randomNoteGenerator(temp, accountPayload, description);
                DateTime alarm = startTime.AddMinutes(-15);

                SDataTemplateResourceRequest activityTemplate = new SDataTemplateResourceRequest(service);
                activityTemplate.ResourceKind = "activities";
                Sage.SData.Client.Atom.AtomEntry tempEntry = activityTemplate.Read();

                SDataPayload payload = tempEntry.GetSDataPayload();

                payload.Values["Type"] = type;
                payload.Values["Category"] = category;
                // Get the program to query the server for the contact name, account name, and retrieve the respective ids for each.
                payload.Values["AccountName"] = accountPayload.Values["AccountName"];
                payload.Values["AccountId"] = accountPayload.Key;
                // Checks to make sure there is a contact associated with the account, and if so calls a request to get the payload
                // associated to that contact; then filling in payload.Values
                if (accountPayload.Values["Contacts"] != null)
                {
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
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
                        payload.Values["ContactName"] = contactPayload.Values["Name"];
                        payload.Values["ContactId"] = contactPayload.Key;
                    }
                }

                if (temp != "Personal")
                {
                    // Checks if there is an associated opportunity with the account, similar to how the contact was found.
                    if (accountPayload.Values["Opportunities"] != null)
                    {
                        SDataResourceCollectionRequest opp = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "opportunities",
                            QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                        };
                        var feed = opp.Read();
                        SDataPayload oppPayload = null;
                        if (feed.Entries.Count() != 0)
                        {
                            foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                            {
                                oppPayload = entry.GetSDataPayload();
                                break;
                            }
                            payload.Values["OpportunityName"] = oppPayload.Values["Description"];
                            payload.Values["OpportunityId"] = oppPayload.Key;
                        }
                    }

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
                }
                payload.Values["Description"] = description;
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

                //Creating the entry...
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                activitiesCount++;
                SetActivitiesCreated(activitiesCount.ToString());
                Log(DateTime.Now + " - Created Activity: " + payload.Values["Type"] + " - " + timed + " seconds", fileName);
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }
   
        }

        // Functional
        public void makeActivityFor(SDataPayload opportunityPayload)
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
                SDataSingleResourceRequest getAccount = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    ResourceSelector = "'" + key.Key + "'"
                };
                var rawr = getAccount.Read();
                SDataPayload accountPayload = rawr.GetSDataPayload();
                string notes = randomNoteGenerator(temp, accountPayload, description);
                DateTime alarm = startTime.AddMinutes(-15);
                DateTime duration;

                SDataTemplateResourceRequest activityTemplate = new SDataTemplateResourceRequest(service);
                activityTemplate.ResourceKind = "activities";
                Sage.SData.Client.Atom.AtomEntry tempEntry = activityTemplate.Read();

                SDataPayload payload = tempEntry.GetSDataPayload();

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

                //Creating the entry...
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                activitiesCount++;
                SetActivitiesCreated(activitiesCount.ToString());
                Log(DateTime.Now + " - Created Activity: " + payload.Values["Type"] +  " - " + timed + " seconds", fileName);
            }
            catch (Exception e)
            {
                Log(e.ToString(),fileName);;
            }
        }

        // Functional
        public void makeAccount()
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest accountTemplate = new SDataTemplateResourceRequest(dynamic);
                accountTemplate.ResourceKind = "accounts";
                Sage.SData.Client.Atom.AtomEntry tempEntry = accountTemplate.Read();
                SDataPayload payload = tempEntry.GetSDataPayload();
                bool checker = false;
                string accountName = "";

                do
                {
                    accountName = localize(language, "Fake Company Name", null, null, null, true);
                    try
                    {
                        SDataResourceCollectionRequest check = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "accounts",
                            QueryValues = { { "where", "AccountNameUpper eq '" + accountName.ToUpper() + "'" } }
                        };
                        var feed = check.Read();
                        if (feed.Entries.Count() == 0)
                            checker = false;
                        else
                            checker = true;
                    }
                    catch (Exception e) { 
                        Log(e.ToString(),fileName);
                    }
                } while (checker == true);

                payload.Values["AccountName"] = accountName;
                payload.Values["AccountNameUpper"] = accountName.ToUpper();
                payload.Values["CreateDate"] = DateTime.Now;
                payload.Values["CreateUser"] = UserID;
                payload.Values["Type"] = localize(language, "Account Type", null, null, null, true);
                payload.Values["Status"] = localize(language, "Account Status", null, null, null, true);

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    Entry = tempEntry
                };
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                accountsCount++;
                SetAccountsCreated(accountsCount.ToString());
                Log(DateTime.Now + " - Created new account: " + payload.Values["AccountName"] + " - " + timed + " seconds", fileName);
            }
            catch (Exception e) { 
                Log(e.ToString(),fileName);
            }
        }

        // Functional
        public SDataPayload makeAccountWithName(string accountName)
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest accountTemplate = new SDataTemplateResourceRequest(dynamic);
                accountTemplate.ResourceKind = "accounts";
                Sage.SData.Client.Atom.AtomEntry tempEntry = accountTemplate.Read();
                SDataPayload payload = tempEntry.GetSDataPayload();

                payload.Values["AccountName"] = accountName;
                payload.Values["AccountNameUpper"] = accountName.ToUpper();
                payload.Values["CreateDate"] = DateTime.Now;
                payload.Values["CreateUser"] = UserID;
                payload.Values["Type"] = localize(language, "Account Type", null, null, null, true);
                payload.Values["Status"] = localize(language, "Account Status", null, null, null, true);

                tempEntry.SetSDataPayload(payload);

                /*SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    Entry = tempEntry
                };
                request.Create(); */
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                accountsCount++;
                SetAccountsCreated(accountsCount.ToString());
                Log(DateTime.Now + " - Created new account: " + payload.Values["AccountName"] +  " - " + timed + " seconds", fileName);
                return payload;
            }
            catch { return null; }
        }

        // Functional
        public void makeContact()
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest contactTemplate = new SDataTemplateResourceRequest(dynamic);
                contactTemplate.ResourceKind = "contacts";
                Sage.SData.Client.Atom.AtomEntry tempEntry = contactTemplate.Read();
                SDataPayload payload = tempEntry.GetSDataPayload();
                SDataPayload accountPayload = null;
                int i = 0;
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);

                if (i == 50)
                    return;

                string firstName = localize(language, "Fake First Name", null, null, null, true);
                string lastName = localize(language, "Fake Last Name", null, null, null, true);

                if (accountPayload.Values["Contacts"] != null)
                {
                    SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "contacts",
                            QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                        };
                    var feed = contact.Read();
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
                string phone = rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + 
                    rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + 
                    rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString();
                payload.Values["WorkPhone"] = phone;
                payload.Values["Mobile"] = phone;
                payload.Values["DoNotEmail"] = false;
                payload.Values["DoNotFax"] = false;
                payload.Values["DoNotMail"] = false;
                payload.Values["DoNotPhone"] = false;
                payload.Values["DoNotSolicit"] = false;
                payload.Values["IsServiceAuthorized"] = false;
                payload.Values["WebAddress"] = accountPayload.Values["WebAddress"];
                payload.Values["Status"] = "Active";
                payload.Values["Address"] = new SDataPayload
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
                                        };

                payload.Values["Description"] = accountPayload.Values["Description"];
                payload.Values["PreferredContact"] = "Unknown";

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "contacts",
                    Entry = tempEntry
                };
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                contactsCount++;
                SetContactsCreated(contactsCount.ToString());
                Log(DateTime.Now + " - Created contact: " + payload.Values["Name"] +  " - " + timed + " seconds", fileName);
            }
            catch (Exception e) {
                Log(e.ToString(), fileName);
            }

        }

        // Functional
        public void makeLead()
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest leadsTemplate = new SDataTemplateResourceRequest(dynamic);
                leadsTemplate.ResourceKind = "leads";

                bool checker = true;
                string firstName = "";
                string lastName = "";

                Sage.SData.Client.Atom.AtomEntry tempEntry = leadsTemplate.Read();
                SDataPayload payload = tempEntry.GetSDataPayload();
                // Checks to see if there is a lead with that name already created
                do
                {
                    firstName = localize(language, "Fake First Name", null, null, null, true);
                    lastName = localize(language, "Fake Last Name", null, null, null, true);
                    SDataResourceCollectionRequest check = new SDataResourceCollectionRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "LastName eq '" + lastName + "'" } }
                    };
                    var feed = check.Read();
                    foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                    {
                        SDataPayload tempPayload = entry.GetSDataPayload();
                        if ((string)tempPayload.Values["FirstName"] == firstName)
                        {
                            checker = true;
                            break;
                        }
                        else
                            checker = false;
                    }
                } while (checker);

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
                string phone = rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString()
                    + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString()
                    + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString() + rand.Next(9).ToString();
                payload.Values["CreateUser"] = UserID;
                payload.Values["CreateDate"] = DateTime.Now.ToUniversalTime();
                payload.Values["Company"] = localize(language, "Fake Company Name", null, null, null, true);
                payload.Values["Email"] = firstName.ToLower() + lastName.ToLower() + "@" + emailProvider + ".com";
                payload.Values["FirstName"] = firstName;
                payload.Values["LastName"] = lastName;
                payload.Values["LastNameUpper"] = lastName.ToUpper();
                payload.Values["Mobile"] = phone;
                payload.Values["LeadNameFirstLast"] = firstName + " " + lastName;
                payload.Values["LeadNameLastFirst"] = lastName + ", " + firstName;

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "leads",
                    Entry = tempEntry
                };

                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " - Created lead: " + payload.Values["Company"] + ", " + payload.Values["LeadNameFirstLast"] + " - " + timed + " seconds", fileName);
                leadsCount++;
                SetLeadsCreated(leadsCount.ToString());
            }
            catch (Exception e) { 
                Log(e.ToString(), fileName); 
            }
        }

        // Functional
        public void makeOpportunity()
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest opportunityTemplate = new SDataTemplateResourceRequest(dynamic);
                opportunityTemplate.ResourceKind = "opportunities";

                Sage.SData.Client.Atom.AtomEntry tempEntry = opportunityTemplate.Read();
                SDataPayload payload = tempEntry.GetSDataPayload();

                SDataPayload accountPayload = null;
                int i = 0;
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);

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

                var getUserRequest = new SDataServiceOperationRequest(service)
                {
                    OperationName = "getCurrentUser",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };
                var temp = getUserRequest.Create();
                var userPayload = temp.GetSDataPayload();
                userPayload = (SDataPayload)userPayload.Values["response"];

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
                //payload.Values["Weighted"] = oppValue / 100;
                //payload.Values["OverrideSalesPotential"] = false;
                //payload.Values["EstimatedClose"] = randomDateGenerator();

                if (accountPayload.Values["Contacts"] != null)
                {
                    SDataBatchRequest contact = new SDataBatchRequest(dynamic)
                    {
                        ResourceKind = "contacts",
                        QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
                    };


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

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "opportunities",
                    Entry = tempEntry
                };
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                opportunitiesCount++;
                SetOppsCreated(opportunitiesCount.ToString());
                Log(DateTime.Now + " - Opportunity made for account: " + accountPayload.Values["AccountName"] + " - " + timed + " seconds", fileName);
            }
            catch (Exception e) { 
                Log(e.ToString(), fileName); 
            }
        }

        // Functional
        public void makeTicket()
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                SDataTemplateResourceRequest ticketTemplate = new SDataTemplateResourceRequest(dynamic);
                ticketTemplate.ResourceKind = "tickets";

                Sage.SData.Client.Atom.AtomEntry tempEntry = ticketTemplate.Read();
                SDataPayload payload = tempEntry.GetSDataPayload();

                SDataPayload accountPayload = null;
                int j = 0;
                do
                {
                    accountPayload = fetchAccount();
                    j++;
                } while (accountPayload == null && j < 50);
                //accountPayload.Values["UserField1"] = UserID;
                if (j == 50)
                    return;

                // Only need account name for the payload to be complete
                payload.Values["Account"] = accountPayload;
                try
                {
                    if (accountPayload.Values["Contacts"] != null)
                    {
                        SDataResourceCollectionRequest contact = new SDataResourceCollectionRequest(dynamic)
                        {
                            ResourceKind = "contacts",
                            QueryValues = { { "where", "Account.Id eq '" + accountPayload.Key + "'" } }
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
                    }
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
                    }
                }
                catch (Exception e)
                {
                    Log(e.ToString(), fileName);
                }

                tempEntry.SetSDataPayload(payload);

                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                {
                    ResourceKind = "tickets",
                    Entry = tempEntry
                };
                request.Create();
                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                float timed = (after - previous) / 1000;
                Log(DateTime.Now + " - Created ticket number: " + accountPayload.Values["AccountName"] +  " - " + timed + " seconds", fileName);
                ticketsCount++;
                SetTicketsCreated(ticketsCount.ToString());
            }
            catch (Exception e) { 
                Log(e.ToString(), fileName); 
            }

        }

        // Functional
        public void completeActivity()
        {
            try
            {
                // Initiates a value to keep track of amount of activities created.
                int i = 0;
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

                var request = new SDataServiceOperationRequest(service)
                {
                    ResourceKind = "activities",
                    OperationName = "Complete",
                    Entry = new Sage.SData.Client.Atom.AtomEntry()
                };

                // From the Whitepaper pdf to get the user payload
                var getUserRequest = new SDataServiceOperationRequest(service)
                { OperationName = "getCurrentUser", 
                 Entry = new Sage.SData.Client.Atom.AtomEntry() }; 
                 var temp = getUserRequest.Create();
                 var userPayload = temp.GetSDataPayload();
                 userPayload = (SDataPayload)userPayload.Values["response"];

                SDataResourceCollectionRequest activities = new SDataResourceCollectionRequest(service)
                {
                    ResourceKind = "activities",
                    QueryValues = { { "where", "Leader eq '" + userPayload.Values["userId"] + "'" }, { "orderBy", "StartDate" } }
                };

                var feed = activities.Read();

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
                                var response = request.Create();
                                var responsePayload = response.GetSDataPayload();
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;
                                Log(DateTime.Now + " - Completed activity: " + payload.Values["Description"] + " - " + timed + "seconds", fileName);
                                i++;
                                activitiesCompleteCount++;
                                SetCompletedActivities(activitiesCompleteCount.ToString());
                            }
                            catch (Exception e)
                            {
                                Log(e.ToString(),fileName);
                            }
                        }
                    }

                }
            }
            catch (ArgumentNullException e)
            {
                Log(e.ToString(), fileName);
            }
        }

        // Needs help!
        public void promoteLead()
        {
            try
            {
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
                int i = 0;
                do
                {
                    accountPayload = fetchAccount();
                    i++;
                } while (accountPayload == null && i < 50);

                if (i == 50)
                    return;

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
                    catch { check = true; }
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

                if (!test)
                {
                    var request = new SDataServiceOperationRequest(dynamic)
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
                    Log(DateTime.Now + " - Converted " + leadPayload.Values["FirstName"] + " " + leadPayload.Values["LastName"] + " to a contact - " + timed + " seconds", fileName);
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
                               {"account", accountPayload.Key}
                                    }
                           }
                        }
                                 }
                       });
                    request.Create();
                    float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    float timed = (after - previous) / 1000;
                    Log(DateTime.Now + " - Converted " + leadPayload.Values["FirstName"] + " " + leadPayload.Values["LastName"] 
                        + " to a contact with Account " + leadPayload.Values["Company"] + " - " + timed + " seconds", fileName);
                }
                leadsPromotedCount++;
                SetLeadsPromoted(leadsPromotedCount.ToString());
            }
            catch (Exception e) { 
                Log(e.ToString(), fileName); 
            }
        }

        // Functional, updated to include transitions between stages...
        public void updateOpportunity()
        {
            try
            {
                float previous = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                Sage.SData.Client.Atom.AtomEntry entry = null;
                SDataPayload payload = null;
                int counter = 0;
                int counter2 = 0;
                bool checker = false;
                do
                {
                    checker = false;
                        do
                        {
                            entry = fetchOpportunity();
                            counter++;
                        } while (entry == null || counter == 50);
                        
                        if (counter == 50)
                        {
                            Log("Unable to locate a valid opportunity at " + DateTime.Now, fileName);
                            return;
                        }
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
                } while (((string)payload.Values["Closed"]) == "true" || checker == true);

                int x = rand.Next(0, 100);
                if (x < (Convert.ToInt32(payload.Values["CloseProbability"]) + 20) && payload.Values["AddToForecast"].ToString() == "false")
                {
                    payload.Values["AddToForecast"] = true;
                    float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    float timed = (after - previous) / 1000;
                    Log(DateTime.Now + " - Added " + payload.Values["Description"] + " to forecast - " + timed + " seconds of searching", fileName);
                    Debug.WriteLine("Added " + payload.Values["Description"] + " to forecast");
                }

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
                    payload.Values["SalesAmount"] = payload.Values["SalesPotential"];
                    payload.Values["ActualAmount"] = payload.Values["SalesAmount"];

                    entry.SetSDataPayload(payload);

                    SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                    {
                        ResourceKind = "opportunities",
                        Entry = entry
                    };

                    request.Update();
                    float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                    float timed = (after - previous) / 1000;
                    Log(DateTime.Now + " - Updated Opp with win: " + payload.Values["Description"]  + " - " + timed + " seconds", fileName);
                    Debug.WriteLine("Updated Opp with win: " + payload.Values["Description"]);
                    oppsUpdatedCount++;
                    SetOppsUpdated(oppsUpdatedCount.ToString());
                    return;
                }
                else
                {
                    double choice = 5 * rand.NextDouble();
                    if (choice < 1.6)
                    {
                        if (choice < .3)
                        {
                            // Close the opportunity (either winning or losing it based on the 'close prob'
                            int luckyOne = Convert.ToInt32(payload.Values["CloseProbability"]) + 10;
                            int testMyLuck = rand.Next(0, 100);
                            if (testMyLuck <= luckyOne)
                            {
                                // Winner winner! Close the opportunity with a win.
                                // Is this a service request? Close-win?
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
                                payload.Values["SalesAmount"] = payload.Values["SalesPotential"];
                                payload.Values["ActualAmount"] = payload.Values["SalesAmount"];

                                entry.SetSDataPayload(payload);

                                SDataSingleResourceRequest request = new SDataSingleResourceRequest(dynamic)
                                {
                                    ResourceKind = "opportunities",
                                    Entry = entry
                                };

                                request.Update();
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;
                                Log(DateTime.Now + " - Updated Opp with win: " + payload.Values["Description"] +  " - " + timed + " seconds", fileName);
                                Debug.WriteLine("Updated Opp with win: " + payload.Values["Description"]);
                                oppsUpdatedCount++;
                                SetOppsUpdated(oppsUpdatedCount.ToString());
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

                                request.Update();
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;
                                Log(DateTime.Now + " - Updated Opp with loss: " + payload.Values["Description"]  + " - " + timed + " seconds", fileName);
                                Debug.WriteLine("Updated Opp with loss: " + payload.Values["Description"]);
                                oppsUpdatedCount++;
                                SetOppsUpdated(oppsUpdatedCount.ToString());
                                return;
                            }
                        }
                        else
                        {
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

                                request.Update();
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;
                                Log(DateTime.Now + " - Updated Opp with loss: " + payload.Values["Description"]  + " - " + timed + " seconds", fileName);
                                Debug.WriteLine("Updated Opp with loss: " + payload.Values["Description"]);
                                oppsUpdatedCount++;
                                SetOppsUpdated(oppsUpdatedCount.ToString());
                                return;
                            }
                            // Add a note to the opportunity
                            makeNoteFor(payload);
                            Log(DateTime.Now + " - Note made for: " + payload.Values["Description"], fileName);
                            Debug.WriteLine("Note made for: " + payload.Values["Description"]);
                            return;
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
                            requested.Update();
                            float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                            float timed = (after - previous) / 1000;
                            Log(DateTime.Now + " - Opportunity: " + payload.Values["Description"] + " moved to the next stage - " + timed + " seconds", fileName);
                            Debug.WriteLine("Opportunity: " + payload.Values["Description"] + " moved to the next stage");
                            oppsUpdatedCount++;
                            SetOppsUpdated(oppsUpdatedCount.ToString());
                        }
                        else
                        {
                            int unlucky = rand.Next(0, 100);
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

                                request.Update();
                                float after = DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                                float timed = (after - previous) / 1000;
                                Log(DateTime.Now + " - Updated Opp with loss: " + payload.Values["Description"]  + " - " + timed + " seconds", fileName);
                                Debug.WriteLine("Updated Opp with loss: " + payload.Values["Description"]);
                                oppsUpdatedCount++;
                                SetOppsUpdated(oppsUpdatedCount.ToString());
                                return;
                            }
                            // Add an activity to the opportunity
                            makeActivityFor(payload);
                            Log(DateTime.Now + " - Activity made for: " + payload.Values["Description"], fileName);
                            Debug.WriteLine("Activity made for: " + payload.Values["Description"]);
                        }
                    }
                }
            }
            catch (Exception e) { 
                Log(e.ToString(), fileName); 
            }
        }
        #endregion

        #region SDataDecoding
        // Unnecessary because there are predefined categories, though this can help to put the types into pretty names.
        private string categoryDecoder(string type)
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
        private string todoDecoder(string description)
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
        private SDataPayload fetchAccount()
        {
            Random rand = new Random();
            int i = rand.Next(1139); // Where 'i' is based on the number of accounts
            //int count = 0;
            SDataPayload payload = null;

            try
            {
                SDataResourceCollectionRequest accounts = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "accounts",
                    QueryValues = { { "startIndex", i.ToString() } }
                    //QueryValues = { { "orderBy", "StartDate" } }
                };

                var feed = accounts.Read();
                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                {
                    payload = entry.GetSDataPayload();
                    if (payload != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                                {
                                    file.WriteLine(e);
                                }
                //Log(e.ToString(),fileName);
                SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }

            return payload;
        }

        private Sage.SData.Client.Atom.AtomEntry fetchLead()
        {
            Random rand = new Random();
            int i = rand.Next(250);
            Sage.SData.Client.Atom.AtomEntry tempEntry = null;

            try
            {
                SDataResourceCollectionRequest leads = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "leads",
                    QueryValues = { { "startIndex", i.ToString() } }
                };

                var feed = leads.Read();
                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                {
                    tempEntry = entry;
                    break;
                }
            }
            catch (Exception e)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                                {
                                    file.WriteLine(e);
                                }
                //Log(e.ToString(),fileName);;
                SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }

            return tempEntry;
        }

        private SDataPayload fetchLeadSource()
        {
            Random rand = new Random();
            int i = rand.Next(0, 8);
            SDataPayload payload = null;

            try
            {
                SDataResourceCollectionRequest leadSources = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "leadSources",
                    QueryValues = { { "startIndex", i.ToString() } }
                    //QueryValues = { { "orderBy", "StartDate" } }
                };

                var feed = leadSources.Read();
                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                {
                    payload = entry.GetSDataPayload();
                    if (payload != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                                {
                                    file.WriteLine(e);
                                }
                //Log(e.ToString(), fileName);
                SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }

            return payload;
        }

        private Sage.SData.Client.Atom.AtomEntry fetchOpportunity()
        {
            Random rand = new Random();
            Sage.SData.Client.Atom.AtomEntry returnEntry = null;
            //SDataPayload payload = null;
            int i = rand.Next(100);

            try
            {
                SDataResourceCollectionRequest opportunities = new SDataResourceCollectionRequest(dynamic)
                {
                    ResourceKind = "opportunities",
                    QueryValues = { {"where", "Status eq 'Open'"} , { "startIndex", i.ToString() } }
                };

                var feed = opportunities.Read();
                foreach (Sage.SData.Client.Atom.AtomEntry entry in feed.Entries)
                {
                    if (entry != null)
                    {
                        returnEntry = entry;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                                {
                                    file.WriteLine(e);
                                }
                //Log(e.ToString(),fileName);;
                SetText("Connection to server lost... Please check your connection");
                //this.stop();
            }

            return returnEntry;
        }

        private void progressStage(SDataPayload oppPayload)
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

        private void progressChineseStage(SDataPayload oppPayload)
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

        // Currently excludes Event Activity due to it causing problems on activity creation.
        private string randomTypeGenerator()
        {
            Random rand = new Random();
            // Random number to determine which type will be created.
            
            double choice = 5 * rand.NextDouble();
            string returnType;

            if (choice >= 4)
                returnType = "Appointment";
            else
            {
                if (choice >= 2.5)
                    returnType = "PhoneCall";
                else
                {
                    if (choice >= 1)
                        returnType = "ToDo";
                    //else
                    //{
                    //if (choice >= 1)
                    //returnType = "Internal";
                    else
                    {
                        if (UserID == "admin")
                            returnType = "ToDo";
                        else
                        returnType = "Personal";
                    }
                    //}
                }
            }

            return returnType;
        }

        private string randomChineseTypeGenerator()
        {
            Random rand = new Random();
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
                    if (choice >= 1)
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

        private string randomAccountType()
        {
            Random rand = new Random();
            int choice = rand.Next(0,8);
            string type = "";
            switch(choice)
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
                case 7 :
                    type = "Competitor";
                    break;
                case 8:
                    type = "Other";
                    break;
            }
            return type;
        }

        private string randomChineseAccountType()
        {
            Random rand = new Random();
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

        private string randomAccountStatus()
        {
            Random rand = new Random();
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

        private string randomChineseAccountStatus()
        {
            Random rand = new Random();
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

        private string randomCategoryGenerator(string type)
        {
            Random rand = new Random();
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
                    index = rand.Next(16,29);
                    category = categories[index];
                    break;
                case "Personal":
                    goto case "Appointment";
                case "PhoneCall":
                    index = rand.Next(30,34);
                    category = categories[index];
                    break;
                default:
                    category = "";
                    break;
            }
            return category;
        }

        private string randomChineseCategoryGenerator(string type)
        {
            Random rand = new Random();
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

        private string randomDescriptionGenerator(string type)
        {
            Random rand = new Random();
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
                "Send fax",
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
                    index = rand.Next(10,20);
                    returnDescription = descriptions[index];
                    break;
                case "ToDo":
                    index = rand.Next(21,26);
                    returnDescription = descriptions[index];
                    break;
                case "Schedule":
                    returnDescription = "";
                    break;
                case "Personal":
                    index = rand.Next(27,44);
                    returnDescription = descriptions[index];
                    break;
                case "Note":
                    index = rand.Next(45,50);
                    returnDescription = descriptions[index];
                    break;
                default:
                    returnDescription = "";
                    break;
            }

            return returnDescription;
        }

        private string randomChineseDescriptionGenerator(string type)
        {
            Random rand = new Random();
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

        /*
        private string randomLeadSource()
        {
            Random rand = new Random();
            int x = rand.Next(0, 8);
            string source = "E-mail";

            int value = rand.Next(8);
            switch (value)
            {
                case 0:
                    source = "Advertising";
                    break;
                case 1:
                    source = "Direct Mail";
                    break;
                case 2:
                    source = "E-mail";
                    break;
                case 3:
                    source = "Event";
                    break;
                case 4:
                    source = "Seminar";
                    break;
                case 5:
                    source = "Telemarketing";
                    break;
                case 6:
                    source = "Trade Show";
                    break;
                case 7:
                    source = "Web";
                    break;
                case 8:
                    source = "Word of Mouth /Referral";
                    break;
                default:
                    source = "E-mail";
                    break;
            }
            return source;
        }

        private string randomChineseLeadSource()
        {
            Random rand = new Random();
            string source = "电子邮件";

            int value = rand.Next(8);
            switch (value)
            {
                case 0:
                    source = "广告";
                    break;
                case 1:
                    source = "商函";
                    break;
                case 2:
                    source = "电子邮件";
                    break;
                case 3:
                    source = "事件";
                    break;
                case 4:
                    source = "研讨会";
                    break;
                case 5:
                    source = "电话营销";
                    break;
                case 6:
                    source = "展会";
                    break;
                case 7:
                    source = "卷筒纸";
                    break;
                case 8:
                    source = "口碑 /转介";
                    break;
                default:
                    source = "电子邮件";
                    break;
            }
            return source;
        }
        */

        private string randomNoteGenerator(string type, SDataPayload payload, string description)
        {
            Random rand = new Random();
            string note = "";
            // Array of note structures to be used
            string[] notes = new string[]
            {
                #region Notes
                "",
                // Meeting notes: (5 total)
                "Representatives from " + payload.Values["AccountName"] + " wanted to get together to talk about the product.",
                "Meeting with " + payload.Values["AccountName"] + " should help move along business with them.",
                "Talked with " + payload.Values["AccountName"] + " on the phone and they wanted to speak in person for clarifications.",
                payload.Values["AccountName"] + " wanted to congratulate us in person on our product.",
                "Checking up on " + payload.Values["AccountName"] + ". Have not heard from them in a while.",
                // Phone call notes: (3 total)
                payload.Values["AccountName"] + " wanted clarifications",
                "Following up on " + payload.Values["AccountName"] + " because they seemed interested in the product",
                payload.Values["AccountName"] + " wanted to talk about mobile implementation",
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
                    index = rand.Next(1,5);
                    note = notes[index];
                    break;
                case "PhoneCall":
                    if (description == "Discuss Opportunities")
                        index = 6;
                    if (description == "Follow-up" || description == "Follow up - next step" || description == "Follow up on proposal")
                    {
                        int temp = rand.Next(0,2);
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
                        index = rand.Next(9,12);
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
                        int temp = rand.Next(0,2);
                        if (temp == 0)
                            index = 16;
                        if (temp == 1)
                            index = 17;
                        if (temp == 2)
                            index = 18;
                    }
                    if (description == "Questions")
                    {
                        int temp = rand.Next(0,3);
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
                        int temp = rand.Next(0,2);
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

        private string randomChineseNoteGenerator(string type, SDataPayload payload, string description)
        {
            Random rand = new Random();
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

        private string randomLocationGenerator(string type)
        {
            Random rand = new Random();
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

        private string randomChineseLocationGenerator(string type)
        {
            Random rand = new Random();
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

        private string randomReason(bool won)
        {
            Random rand = new Random();
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
                    int j = rand.Next(0, 15);
                    do
                    {
                        reason2 = reasons[j];
                    } while (reason == reason2);
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
                    do
                    {
                        reason2 = reasons[j];
                    } while (reason == reason2);
                    reason = reason + " and " + reason2;
                    return reason;
                }
            }
        }

        private string randomChineseReason(bool won)
        {
            Random rand = new Random();
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
        private DateTime randomDateGenerator()
        {
            Random rand = new Random();
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

        private DateTime randomEuroDateGenerator()
        {
            Random rand = new Random();
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

        private string randomPriorityGenerator()
        {
            Random rand = new Random();
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

        private string randomChinesePriorityGenerator()
        {
            Random rand = new Random();
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
            Random rand = new Random();
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
            Random rand = new Random();
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
            Random rand = new Random();
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
            Random rand = new Random();
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
            Random rand = new Random();
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
            Random rand = new Random();
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
                        value = randomNoteGenerator(type, payload, description);
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
        public int getActivitiesCount()
        {
            return activitiesCount;
        }
        public int getNotesCount()
        {
            return notesCount;
        }
        public int getActivitiesCompleteCount()
        {
            return activitiesCompleteCount;
        }
        public bool getStopCommand()
        {
            return stopCommand;
        }
        public bool getFirstRun()
        {
            return firstRun;
        }
        public void setActivitiesCount(int value)
        {
            activitiesCount = value;
        }
        public void setNotesCount(int value)
        {
            notesCount = value;
        }
        public void setActivitiesCompleteCount(int value)
        {
            activitiesCompleteCount = value;
        }
        public void setTicketsCount(int value)
        {
            ticketsCount = value;
        }
        public void setOpportunityCount(int value)
        {
            opportunitiesCount = value;
        }
        public void setLeadsCount(int value)
        {
            leadsCount = value;
        }
        public void setStopCommand(bool value)
        {
            stopCommand = value;
        }
        public void setFirstRun(bool value)
        {
            firstRun = value;
        }
        #endregion

        #region Threading
        delegate void SetTextCallback(string text);
        delegate void SetStepCallback(int step);
        delegate void PerformStepCallback();
        delegate string GetRoleSelector();

        // Sets the text of the progress Label
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

        /*
        // Changes the progressBar step. Used for threading
        private void SetStep(int step)
        {
            if (this.progressBar.InvokeRequired)
            {
                SetStepCallback d = new SetStepCallback(SetStep);
                progressBar.Invoke(d, new object[] { step });
            }
            else
            {
                this.progressBar.Step = step;
            }
        }
         * */

        // Sets the text of the activities created value.
        private void SetActivitiesCreated(string text)
        { 
            if (this.activitiesCreated.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActivitiesCreated);
                this.activitiesCreated.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCreated.Text = text;
            }
        }

        // Sets the text of the notes created value.
        private void SetNotesCreated(string text)
        {
            if (this.notesCreated.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetNotesCreated);
                this.notesCreated.Invoke(d, new object[] { text });
            }
            else
            {
                this.notesCreated.Text = text;
            }
        }

        // Sets the text of the contacts created value.
        private void SetContactsCreated(string text)
        {
            if (this.contactCreated.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetContactsCreated);
                this.contactCreated.Invoke(d, new object[] { text });
            }
            else
            {
                this.contactCreated.Text = text;
            }
        }

        // Sets the text of the leads created value.
        private void SetLeadsCreated(string text)
        {
            if (this.leadCreated.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLeadsCreated);
                this.leadCreated.Invoke(d, new object[] { text });
            }
            else
            {
                this.leadCreated.Text = text;
            }
        }

        // Sets the text of the opportunities created value.
        private void SetOppsCreated(string text)
        {
            if (this.oppCreated.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetOppsCreated);
                this.oppCreated.Invoke(d, new object[] { text });
            }
            else
            {
                this.oppCreated.Text = text;
            }
        }

        // Sets the text of the tickets created value.
        private void SetTicketsCreated(string text)
        {
            if (this.ticketCreated.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTicketsCreated);
                this.ticketCreated.Invoke(d, new object[] { text });
            }
            else
            {
                this.ticketCreated.Text = text;
            }
        }

        // Sets the text of the accounts created value.
        private void SetAccountsCreated(string text)
        {
            if (this.accountCreated.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetAccountsCreated);
                this.accountCreated.Invoke(d, new object[] { text });
            }
            else
            {
                this.accountCreated.Text = text;
            }
        }

        // Sets the text of the opps updated value.
        private void SetOppsUpdated(string text)
        {
            if (this.oppUpdated.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetOppsUpdated);
                this.oppUpdated.Invoke(d, new object[] { text });
            }
            else
            {
                this.oppUpdated.Text = text;
            }
        }

        // Sets the text of the activities completed value.
        private void SetCompletedActivities(string text)
        {
            if (this.activitiesCompleted.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetCompletedActivities);
                this.activitiesCompleted.Invoke(d, new object[] { text });
            }
            else
            {
                this.activitiesCompleted.Text = text;
            }
        }

        // Sets the text of the leads promoted value.
        private void SetLeadsPromoted(string text)
        {
            if (this.leadPromoted.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLeadsPromoted);
                this.leadPromoted.Invoke(d, new object[] { text });
            }
            else
            {
                this.leadPromoted.Text = text;
            }
        }

        private void SetRole(string text)
        {
            if (this.roleLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetRole);
                this.roleLabel.Invoke(d, new object[] { text });
            }
            else
            {
                this.roleLabel.Text = text;
            }
        }

        private string getRole()
        {
            try
            {
                if (this.roleSelector.InvokeRequired)
                {
                    GetRoleSelector d = new GetRoleSelector(getRole);
                    return this.roleSelector.Invoke(d).ToString();
                }
                else
                {
                    return this.roleSelector.SelectedItem.ToString();
                }
            }
            catch { return null; }
        }

        /*private void PerformStep()
        {
            if (this.progressBar.InvokeRequired)
            {
                PerformStepCallback d = new PerformStepCallback(PerformStep);
                progressBar.Invoke(d, new object[] { });
            }
            else
            {
                this.PerformStep();
            }
        }*/
        #endregion

        #region Logging
        public static void Log(string message, string filename)
        {
            StreamWriter write = new StreamWriter(filename, true);
            write.WriteLine(message);
            write.Close();
        }
        #endregion

        #region Displaying Specific Code
        // Code to allow an InputBox like that from Visual Basic
        public static DialogResult InputBox(string title, string promptText, ref string accessCode)
        {
            Form form = new Form();
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            System.Windows.Forms.Label hyperLink = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button();
            System.Windows.Forms.Button buttonCancel = new System.Windows.Forms.Button();

            form.Text = title;
            label.Text = promptText;
            hyperLink.Font = new System.Drawing.Font(hyperLink.Font.Name, hyperLink.Font.Size, FontStyle.Underline | FontStyle.Italic);
            hyperLink.Text = "Click here...";
            hyperLink.ForeColor = Color.Blue;
            hyperLink.Cursor = Cursors.Hand;
            //textBox.Text = value;

            hyperLink.Click += new System.EventHandler(hyperLink_Click);

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(10, 20, 100, 50);
            hyperLink.SetBounds(10, 65, 25, 25);
            textBox.SetBounds(10, 100, 180, 20);
            buttonOk.SetBounds(90, 150, 50, 23);
            buttonCancel.SetBounds(140, 150, 50, 23);

            label.AutoSize = true;
            hyperLink.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(200, 175);
            form.Controls.AddRange(new Control[] { label, hyperLink, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(form.ClientSize.Width, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            
            accessCode = textBox.Text;
            return dialogResult;
        }

        public static DialogResult InputBox2(string title, string promptText, ref string docName)
        {
            Form form = new Form();
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button();
            System.Windows.Forms.Button buttonCancel = new System.Windows.Forms.Button();

            form.Text = title;
            label.Text = promptText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(10, 20, 100, 50);
            textBox.SetBounds(10, 100, 180, 20);
            buttonOk.SetBounds(90, 150, 50, 23);
            buttonCancel.SetBounds(140, 150, 50, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(200, 175);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(form.ClientSize.Width, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();

            docName = textBox.Text;
            return dialogResult;
        }

        private static void hyperLink_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(authorizationUri);
            }
            catch (Exception ev) { Debug.WriteLine(ev); }
        }
        #endregion

    }
}
