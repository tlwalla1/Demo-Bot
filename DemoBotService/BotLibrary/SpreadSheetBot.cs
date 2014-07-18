using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Framework;
using Sage.SData.Client.Metadata;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using Excel = Microsoft.Office.Interop.Excel;

namespace BotLibrary
{
    public class SpreadSheetBot : Bot
    {
        private string companyName;
        private bool readSpreadSheet;
        private LinkedList<LinkedList<string>> tasks = new LinkedList<LinkedList<string>>();
        private Excel.Workbook workBook;
        private Excel.Application application;

        public SpreadSheetBot(string userID, string password, int reliable, string endpoint, int server, string compName)
        {
            endPoint = endpoint;
            service = new SDataService(endpoint + "/sdata/slx/system/-/") { UserName = userID, Password = password };
            dynamic = new SDataService(endpoint + "/sdata/slx/dynamic/-/") { UserName = userID, Password = password };
            UserID = userID;
            Password = password;
            companyName = compName;
            firstRun = true;
            readSpreadSheet = false;
            activityCompleteAmount = 1;
            reliability = reliable;
            dirty = 0;
            upperBoundMonth = 2;
            fileName = @"C:\Swiftpage\UserLogs\" + UserID + server + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".txt";
            // Change which user creates data in Chinese (Simplified) by adding '|| UserID == "user"' after current value, or merely replace 'China' with the desired user.
            if (UserID == "China")
                language = "Chinese";
        }

        override public void Run()
        {
            bool ok = false;
            SDataUri Uri = new SDataUri(service.Url.ToString());

            SDataRequest request = new SDataRequest(Uri.ToString()) { UserName = UserID, Password = Password };
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
                if (File.Exists(@"C:\Swiftpage\Spreadsheets\" + companyName + ".xlsx"))
                {
                    runFromExcel();
                }
            }
            else
            {
                if (File.Exists(@"C:\Swiftpage\Spreadsheets\" + companyName + ".xlsx"))
                {
                    runFromExcel();
                }
            }
        }


        private void runSpreadSheet()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                switch (tasks.ElementAt(i).ElementAt(0).ToLower())
                {
                    case "account":
                        break;
                    case "activity":
                        break;
                    case "contact":
                        break;
                    case "lead":
                        break;
                    case "note":
                        break;
                    case "opportunity":
                        break;
                    case "ticket":
                        break;
                    case "update opportunity":
                        break;
                    case "complete activity":
                        break;
                }

            }
        }

        // Function for running from a Google Spreadsheet
        private void runFromGoogleSpreadSheet()
        {
            // Don't change. This allows the bot to connect to the Google account: Lee Hogan with e-mail saleslogixbotdev@gmail.com and PW: Developer
            // Information grabbed from https://code.google.com/
            /*
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
                                
                                }   
                            }

                            return;
                        }
                    }
                }

            }*/
        }

        // Function for running from an Excel Spreadsheet
        private void runFromExcel()
        {
            if (!readSpreadSheet)
            {
                Log("Reading Excel file", fileName);
                readFromExcel();
                readSpreadSheet = true;
                Log("Excel file read", fileName);
                Log("Running bot from Excel file", fileName);
                runSpreadSheet();
                Log("Bot ran from Excel file", fileName);
            }
            else
            {
                Log("Running bot from Excel file", fileName);
                runSpreadSheet();
                Log("Bot ran from Excel file", fileName);
            }
        }

        private void readFromExcel()
        {
            try
            {
                application = new Excel.Application();
                workBook = application.Workbooks.Open(@"C:\Swiftpage\Spreadsheets\" + companyName + ".xlsx");
                for (int i = 1; i <= workBook.Worksheets.Count; i++)
                {
                    Excel.Worksheet workSheet = workBook.Sheets[1] as Excel.Worksheet;
                    Excel.Range range = workSheet.UsedRange;

                    foreach (Excel.Range row in range.Rows)
                    {
                        LinkedList<string> rowTask = new LinkedList<string>();
                        foreach (Excel.Range cell in row.Cells)
                        {
                            rowTask.AddLast(cell.Text.ToString());
                        }
                        tasks.AddLast(rowTask);
                    }
                }
                workBook.Close();
                application.Quit();
                printTasks();
            }
            catch (Exception e)
            {
                Log(e.ToString(), fileName);
                application.Quit();
            }
        }

        private void printTasks()
        {
            Log("Tasks to run: ", fileName);
            for (int i = 0; i < tasks.Count; i++)
            {
                string temp = "";
                for (int j = 0; j < tasks.ElementAt(i).Count; j++)
                {
                    temp += tasks.ElementAt(i).ElementAt(j) + " ";
                }
                Log(temp, fileName);
            }
        }

        override public void Log(string message, string filename)
        {
            StreamWriter write = new StreamWriter(filename, true);
            write.WriteLine(message);
            write.Close();
        }
    }
}
