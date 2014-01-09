Demo-Bot
========

Populates data on an SData run Server to create Demo Data based off an Eval user set.

## Before Use

Make sure move the RunUserProfile.txt from the DemoBotService folder into C:/Swiftpage. If this folder is not made already, add it then place the text file into it. Furthermore make sure you unlock the DemoBotService.exe, located in the DemoBotService/bin/Debug folder, by right-clicking the file and going to properties. Once there click the unlock button.

## How to Install Open up the Windows Service project to change where the service points to, defaulted to 'https://slx81.saleslogixcloud.com'. To modify, open the file Bot.cs and look under the public Bot(string userID, string password, int reliable) constructor. Modify the service and dynamic addresses to desired. You are now all set to run the service on your server, build and install.

To install, first unlock the DemoBotService.exe by locating it within the DemoBotService/bin/Debug folder and right-clicking the file. Then go to properties and click the 'Unlock' button and finally click apply. Now open up a command prompt window. Navigate the prompt to the folder containing your installutil.exe. This folder will generally be within your windows/microsoft.NET/framework/(version #) folder. Make sure you are using v4.0 or better because the Bot was made to run on this version. Once there type 'installutil.exe C:/...doclocation...' without the apostrophes and substituting the location of the DemoBotService.exe file in for ...doclocation... Hit enter and the command line will prompt you with a successfull install. If not check to make sure you have unlocked the file. Congratulations you have now instlled the Demo-Bot service application. To uninstall, follow the previous steps but type 'installutil.exe /u C:/...docpath...'. To make the service being running, or check its status, click on start, then right click Computer and navigate to Manage. Then go to Services and Applications and there it will be! Right click the service to start or stop it. For Windows 8, search for Server Manager and DemoBotService will be within there. 


## Questions and Answers


<b>Q:</b> HELP! It won't let me install the windows application service! Why?

<b>A:</b> This is because windows, attempting to protect you from bad programs, has blocked the file. To fix this, locate the DemoBotService.exe then right click on it to navigate to properties. Then click the unblock button and it should be all fixed!


<b>
Q:</b> How do I download the Demo-Bot?

<b>
A:</b> Press the download ZIP button on the GitHub page for tlwalla1/Demo-Bot and extract the files.



<b>
Q:</b> How do I get the bot running?

<b>
A:</b> There are multiple ways to get the bot running. If you are looking for an exe to run from, first open the project in Visual Studio. Once open, click the 'Build' drop down and select 'Publish'. If 'Publish' is unavailabe, try selecting BotExecutable in the solution navigator menu. If you want to test out the current bot, or any modifications made by yourself, merely run the program in your favorite compiler. If you are looking for the bot to run on a server without need for much human attention, try out the new 'Service' branch and get that installed on your machine. If you are new to service applications check out this fantastic article from Microsoft detailing how to get a service running using Visual Studio 'http://msdn.microsoft.com/en-us/library/zt39148a(v=vs.110).aspx'.



<b>
Q:</b> How do I add more variables for the bot to use?

<b>
A:</b> I have separated the bot into specific regions for easier use. For this particular question, you would want to navigate to the Random Variable Creation region and select the function that you wish to add data to. Typically these functions will begin with 'random' and should contain a string array of values which you can modify (Just make sure you change the range of the choice in order to include them in the potential options!).



<b>
Q:</b> When I press the run bot button, what is the bot actually doing?


<b>
A:</b> For the non-technical, the bot runs through a series of pre-defined steps, often randomized, that cause the actors to function in a way that best describes their role. Currently each predefined user, such as 'Lee' or 'Pam', have a certain function that they will execute on. Something that is new and quite tasty is the Inside Sales roles which execute Lead Generation and Lead Qualification functions. These functions provide a story in that they complete a series of steps that would most adequately reflect that of an Inside Sales representative. For the more technical, I suggest that you check out the functions within the code itself. These are all layed out within the Roles region which calls functions from the RandomCreation region.



<b>
Q:</b> I set the bot up to run with specific users, but it doesn't appear to be doing anything! What happened?

<b>
A:</b> My guess is that those users are unique to your servers and were not the users I had pre-defined in the bot. To fix this, go into the Roles section. Under the roleSetter function add the users that you are trying to have run into the correct case that allocates to their role as a user.



<b>
Q:</b> I was browsing your code and it looks like there are functions left blank. What is the purpose of that?



<b>
A:</b> I have been coding this project in multiple segments. I am currently a college student and thus have been interning at the fantastic company that has granted me this great opportunity, Swiftpage, over my breaks from school. Thus, these empty functions are solely empty because I was unable to complete them within my limited amount of time. Think of them as nuggets for knowing what the bot will soon be capable of ;)



<b>
Q:</b> HELP! I just downloaded the bot and tried running it, but the bot is giving me an error.

<b>
A:</b> Please check to make sure that you have the bot pointing to the correct server address. Please enter the full base address of the server, ie. for slx81 you would enter 'https://slx81.saleslogixcloud.com'.



<b>
Q:</b> How do I know the bot is doing anything?



<b>
A:</b> If you are running the bot with the UI, it will display incremental values over on the right hand side of the interface. If you are running the bot as a service, there is no interface to display it on. For both the bot as a service and with the interface, the bot logs its actions under 'C:/Swiftpage/' and has a text file for each actor run.



<b>
Q:</b> Can I run more than just 5 actors?

<b>
A:</b> Yes you can, if you publish the program and run multiple exes you can run 5 more per exe. Or you that is too stressfull on your computer, I suggest downloading the 'Service' branch and using that as a service application. The service application runs all known users for Saleslogix demoing. If you need to modify these users please look at the question about running specific users.



<b>
Q:</b> When looking through your code I see variables which, upon first read, seem to have no purpose in the program. Why?

<b>
A:</b> Some variables are remnants from the previous build of the Demo-Bot. Currently the bot is at a functional v2.0 and much has changed since v1.0 in an attempt to make the bot more user friendly and smarter.



<b>
Q:</b> Where are the service application users located?




<b>
A:</b> The bot reads in users from a text file located in the Demo-Bot/Demo Bot/RunUserProfile.txt file.