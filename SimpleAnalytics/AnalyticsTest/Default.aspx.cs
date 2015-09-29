using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AnalyticsTest
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Analytics.AnalyticsManager manager = new Analytics.AnalyticsManager(Server.MapPath("~/bin/privatekey.p12"), "600980447623-ide8u9tp3ud9ven0v640r64hqn0cb7pr@developer.gserviceaccount.com");

            //that's the number after the p in google analytics url
            //ps. you need to give your email account taken from the service access from the web control panel in google analytics
            manager.LoadAnalyticsProfiles();
            manager.SetDefaultAnalyticProfile("80425770");
            List<Analytics.Data.DataItem> metrics = new List<Analytics.Data.DataItem>();
            metrics.Add(Analytics.Data.Session.Metrics.visits);
            metrics.Add(Analytics.Data.Session.Metrics.timeOnSite);
            metrics.Add(Analytics.Data.Adsense.Metrics.adsenseRevenue);
            List<Analytics.Data.DataItem> dimensions = new List<Analytics.Data.DataItem>();
            dimensions.Add(Analytics.Data.GeoNetwork.Dimensions.country);

            List<Analytics.Data.DataItem> filters = new List<Analytics.Data.DataItem>();
            Analytics.Data.DataItem country = new Analytics.Data.DataItem(Analytics.Data.GeoNetwork.Dimensions.country.Name);
            country.Equals("Saudi Arabia");
            filters.Add(country);
            System.Data.DataTable table = manager.GetGaDataTable(DateTime.Today.AddDays(-3), DateTime.Today, metrics, dimensions, filters, metrics, true);
            GridViewControl.DataSource = table;
            GridViewControl.DataBind();
        }
    }
}