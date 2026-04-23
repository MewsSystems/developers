Steps required to run the solution:

In order to run the console application and API the following needs to be done:

Open up ExchangeRateUpdater.sln
Right click the solution and select Properties
Inside Common Properties -> Startup Project, change the selection from Single startup project to Multiple Startup Projects

Change the Action to Start for following two projects:
API.Controller
ConsoleAPP

CLick Apply

Upon pressing F5 both projects will run as intended