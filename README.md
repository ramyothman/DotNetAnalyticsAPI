Simple Analytics
==================

Simply .NET Project for Google Analytics API Usage

Tired of looking for help, things isn't really working for you! get started to analytics quickly from here.
Want to work with analytics the way it should be, that's the project for you.

This project simply provides a framework on top of the .NET Google Analytics API, simplifying the access, retrieval and working with the Reporting API in a simple way feel free to contribute :) 

The project uses Service Authentication so you'll need to get client id, email and certifcate
first from google api console

Settings
===============================

After cloning the solution you need to open the Package Manager Console from
Tools -> Library Package Manager -> Package Manager Console

PM> Install-Package Google.Apis.Analytics.v3 -Pre [PROJECT_NAME]

Add Reference for the Analytics.dll library and you're ready to go direct data binding with analytics data

Authenticate with one line, ps. you need to give the email account taken from the console 
https://cloud.google.com/console/project
from the web control panel in google analytics give the email access

Analytics.AnalyticsManager manager = new Analytics.AnalyticsManager(Server.MapPath("~/bin/privatekey.p12"), "YOUR_EMAIL");

/then load your profiles
manager.LoadAnalyticsProfiles();

//set default profile that's the number after the p in google analytics url
manager.SetDefaultAnalyticProfile("80425770");

All metrics, calculated fields are placed as DataItems so don't know about what metrics you can access
no problem all properites placed categorized by feature, and organized by metrics and dimensions

List<Analytics.Data.DataItem> metrics = new List<Analytics.Data.DataItem>();
metrics.Add(Analytics.Data.Session.Metrics.visits);
List<Analytics.Data.DataItem> dimensions = new List<Analytics.Data.DataItem>();
dimensions.Add(Analytics.Data.GeoNetwork.Dimensions.country);


you will notice that column headers have the web names for the properties so you could just concentrate now on
the display
System.Data.DataTable table = manager.GetGaDataTable(DateTime.Today.AddDays(-3), DateTime.Today, metrics, dimensions, null, metrics);
GridViewControl.DataSource = table;






=================================
ps. the markup project has nothing to do with the analytics, it's just an old code i implemented before,
I used it to query the google analytics api documentation pages to form an XML File with all api features

then i generated from it classes corresponding to each category so you could reference API features 
as you access resources
