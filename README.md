
<h2>Introduction</h2><pre style="font-family: Consolas; font-size: 10pt; background-color: white;">Simply .NET Project for Google Analytics API v3 Usage
 
Tired of looking for help, things isn't really working for you! get started to analytics quickly from here.
Want to work with analytics the way it should be, that's the place for you.
 
This article will give you a quick jump to analytics and using it with Simple Analytics open source library that provides a framework on top of the .NET Google Analytics API, simplifying the access, retrieval and working with the Reporting API in a simple way 
 
</pre><h2>Background</h2><p>
        First you need to configure your Google developer account and register a project in the cloud console.</p><p>ps. those instructions are using the new console for old console users you'll find same things but some are under different names in the menu </p><ol><li> create an account on <a href="https://cloud.google.com/console/project">Google Cloud Console</a>  </li><li>Create a new Project from the console or select an existing project </li><li>Under APIs and Auths : Enable the Analytics API </li><li>Go to Credentials Page </li><ol><li>Create New Client ID </li><li>Select Service Account</li><li>you will be prompted to download a Key file store it some where safe as we'll be using it in authentication</li><li>A table for service account credentials will be displayed to you containing Client ID, Email address, and public key finger prints</li></ol><li>Copy the email address then login in to your <a href="http://www.google.com/analytics">Analytics Web Account</a> </li><ol><li> Go the Admin Section under it select the profile you want to access  and under user accounts paste the email provided to you </li><li> It's important to do that on both the profile (site) and the account  </li></ol><li>Go to git hub and download the <a href="https://github.com/rmostafa/DotNetAnalyticsAPI">Simple Analytics Library</a>  </li><li>In Visual Studio select the project you want to work on add the Analytics.dll library  to it</li><li>Run the following command under    </li></ol>Package Manager Console from : Tools -&gt; Library Package Manager -&gt; Package Manager Console <p>and execute the below command this will install and reference the google analytics API in your project </p><pre style="font-family: Consolas; font-size: 10pt; background-color: white;">PM&gt; Install-Package Google.Apis.Analytics.v3 -Pre [PROJECT_NAME] </pre><p><strong> ok now you're ready to go</strong> </p><h2>Using the code  </h2><p>
        Using the library is simple first you need to authenticate your self using the Google Service OAuth2 one line is required, please note that the email below is the email provided to you in the console service account section </p><pre lang="C#">Analytics.AnalyticsManager manager = new Analytics.AnalyticsManager(Server.MapPath(&quot;~/bin/privatekey.p12&quot;), &quot;YOUR_EMAIL&quot;); </pre><p>Now we need to query the profiles you have access to, and set a default Profile to work with you could get the web ID of the profile from analytics console or implementation code. The part you will require is the "UA" number, e.g. "UA-12387431-1"  </p>
<pre lang="C#">manager.LoadAnalyticsProfiles();
manager.SetDefaultAnalyticProfile(&quot;UA-12387431-1&quot;);
</pre>

<span style="color: black; font-family: Consolas, 'Courier New', Courier, mono; font-size: 9pt; white-space: pre;"> </span><span style="color: black; font-family: Consolas, 'Courier New', Courier, mono; font-size: 9pt; white-space: pre;"> </span> <p>now we set the metrics and dimensions, a complete Reporting API Commands are placed in the project so you could easily set  metrics and dimensions </p><pre lang="C#">List&lt;Analytics.Data.DataItem&gt; metrics = new List&lt;Analytics.Data.DataItem&gt;();
metrics.Add(Analytics.Data.Visitor.Metrics.visitors);
metrics.Add(Analytics.Data.Session.Metrics.visits);
List&lt;Analytics.Data.DataItem&gt; dimensions = new List&lt;Analytics.Data.DataItem&gt;();
dimensions.Add(Analytics.Data.GeoNetwork.Dimensions.country);
dimensions.Add(Analytics.Data.GeoNetwork.Dimensions.city);  </pre><p>now you're done get a table with analytics data  </p><pre lang="C++">System.Data.DataTable table = manager.GetGaDataTable(DateTime.Today.AddDays(-3),DateTime.Today, metrics, dimensions, null, metrics); </pre><p> that's it you could easily get any API data by changing/adding the metrics and dimensions properties </p><h2>



</h2><h2>Library Implementation </h2><p>The library implementation has a major class player called AnalyticsManager the analytics manager allows to initiate an authentication through OAuth2 Service Account Authentication</p><pre lang="C#">var certificate = new X509Certificate2(certificateKeyPath, &quot;notasecret&quot;, X509KeyStorageFlags.Exportable);
string x = certificate.IssuerName.Name;
credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(apiEmail)
    {
                    Scopes = new[] { Google.Apis.Analytics.v3.AnalyticsService.Scope.Analytics }
        }.FromCertificate(certificate));

analyticsService = new Google.Apis.Analytics.v3.AnalyticsService(new BaseClientService.Initializer()
    {
            HttpClientInitializer = credential,
                    ApplicationName = &quot;Jareeda&quot;
        });
</pre>
<p> a certificate is created from the passed key the <strong><em>notasecret</em> </strong> password is the same password for all key files from google nevertheless the signature differs in the key it self so the combination gives different signatures.</p><p> a service account credential is initialized and passed to it the Scope &quot;Analytics&quot; that's an enumerator where you could change it to <strong><em>AnalyticsReadOnly</em></strong>, <strong><em>AnalyticsEdit</em></strong>, <strong><em>AnalyticsManageUsers</em></strong>  each will give you different access rights, depending on the permissions given from the Analytics Web.</p><p>an <em style="font-weight: bold;">AnalyticsService </em>object is then created this object will be used for querying along the class afterword</p><p>analytics methods require you pass a table_id or a profile_id</p><p><strong> </strong> </p>

<pre lang="cs">public System.Data.DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List&lt;Data.<strong>DataItem</strong>&gt; metricsList, List&lt;Data.<strong>DataItem</strong>&gt; dimensionsList, List&lt;Data.<strong>DataItem</strong>&gt; filtersList, int? maxResults, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.OutputEnum? output, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.SamplingLevelEnum? samplingLevel, List&lt;Data.<strong>DataItem</strong>&gt; segmentList, List&lt;Data.<strong>DataItem</strong>&gt; sortList, int? startIndex, List&lt;Data.<strong>DataItem</strong>&gt; fieldsList)
        {
            if (DefaultProfile == null)
                throw new Exception(&quot;Please set a default profile first using SetDefaultAnalyticProfile method&quot;);
            
            Google.Apis.Analytics.v3.Data.GaData gaData = GetGaData(&quot;ga:&quot; + DefaultProfile.Id, startDate, endDate, Data.DataItem.GetString(metricsList), Data.DataItem.GetString(dimensionsList), Data.DataItem.GetString(filtersList), maxResults, output, samplingLevel, Data.DataItem.GetString(segmentList), Data.DataItem.GetString(sortList), startIndex, Data.DataItem.GetString(fieldsList));
            System.Data.DataTable table = BuildTableColumns(metricsList, dimensionsList);
            if(gaData != null)
                table = BuildTableRows(gaData, table);
            return table;
        }
</pre>
<p>in this method I pass the max parameters needed  for retrieving GaData from analytics API where the passed DataItem List are the complete Dimensions and Metrics List Organized By Category based on the <a href="https://developers.google.com/analytics/devguides/reporting/core/dimsmets">Google Complete Metrics and Dimensions  Reference Guide</a> I have parsed the complete guide and created an XML File for all the dimensions and metrics </p>

<pre lang="xml">&lt;<strong>DataCategory </strong>Name=&quot;Session&quot;&gt;
    &lt;<strong>ItemType </strong>Name=&quot;Dimensions&quot; Value=&quot;1&quot;&gt;
      &lt;DataItem Name=&quot;visitLength&quot; APICommand=&quot;ga:visitLength&quot; WebViewName=&quot;Visit Duration&quot; AppViewName=&quot;Session Duration&quot; DataType=&quot;STRING&quot; AllowedInSegments=&quot;True&quot;&gt;The length of a visit to your property measured in seconds and reported in second increments. The value returned is a string.&lt;/DataItem&gt;
    &lt;/<strong>ItemType</strong>&gt;
    &lt;<strong>ItemType </strong>Name=&quot;Metrics&quot; Value=&quot;2&quot;&gt;
      &lt;DataItem Name=&quot;visits&quot; APICommand=&quot;ga:visits&quot; WebViewName=&quot;Visits&quot; AppViewName=&quot;Sessions&quot; DataType=&quot;INTEGER&quot; AllowedInSegments=&quot;True&quot;&gt;Counts the total number of sessions.&lt;/DataItem&gt;
      &lt;DataItem Name=&quot;bounces&quot; APICommand=&quot;ga:bounces&quot; WebViewName=&quot;Bounces&quot; AppViewName=&quot;&quot; DataType=&quot;INTEGER&quot; AllowedInSegments=&quot;True&quot;&gt;The total number of single page (or single engagement hit) sessions for your property.&lt;/DataItem&gt;
      &lt;DataItem Name=&quot;timeOnSite&quot; APICommand=&quot;ga:timeOnSite&quot; WebViewName=&quot;Visit Duration&quot; AppViewName=&quot;Session Duration&quot; DataType=&quot;TIME&quot; AllowedInSegments=&quot;True&quot;&gt;The total duration of visitor sessions represented in total seconds.&lt;/DataItem&gt;
    &lt;/ItemType&gt;
    &lt;ItemType Name=&quot;Calculated&quot; Value=&quot;3&quot;&gt;
      &lt;DataItem Name=&quot;visitBounceRate&quot; APICommand=&quot;ga:visitBounceRate&quot; WebViewName=&quot;Bounce Rate&quot; AppViewName=&quot;&quot; DataType=&quot;PERCENT&quot; AllowedInSegments=&quot;False&quot;&gt;The percentage of single-page visits (i.e., visits in which the person left your property from the first page). (ga:bounces / ga:visits ) &lt;/DataItem&gt;
      &lt;DataItem Name=&quot;avgTimeOnSite&quot; APICommand=&quot;ga:avgTimeOnSite&quot; WebViewName=&quot;Avg. Visit Duration&quot; AppViewName=&quot;Avg. Session Duration&quot; DataType=&quot;TIME&quot; AllowedInSegments=&quot;False&quot;&gt;The average duration visitor sessions represented in total seconds. (ga:timeOnSite / ga:visits ) &lt;/DataItem&gt;
    &lt;/<strong>ItemType</strong>&gt;
  &lt;/<strong>DataCategory</strong>&gt;&nbsp;</pre>
<p>placed the XML file in the resources of the project and generated Classes based on the Categories containing Classes of Metrics  and Dimensions for each that contains DataItem Objects which contains the definition of the complete properties for the commands retrieving properties from the XML Resource File - giving it the flavor of .NET Resources usage.</p><p><img src="http://ifekra.net/analytics.jpg" style="font-size: 14px;" align="baseline" border="0" hspace="0" vspace="0" />&nbsp;</p><p>all public variables placed in classes like Session, Visitor etc. have description attribute above taken from the analytics API so you could reference features easily without having to get back to the API that allows you to reference features in code with intellisense on it</p><pre lang="C#">List&lt;Analytics.Data.DataItem&gt; metrics = new List&lt;Analytics.Data.DataItem&gt;();
metrics.Add(Analytics.Data.<strong>Session.Metrics</strong>.<strong><em>visits</em></strong>);
metrics.Add(Analytics.Data.<strong>Visitor.Metrics</strong>.<strong><em>newVisits</em></strong>);

List&lt;Analytics.Data.DataItem&gt; dimensions = new List&lt;Analytics.Data.DataItem&gt;();
dimensions.Add(Analytics.Data.<strong>GeoNetwork.Dimensions</strong>.<strong><em>country</em></strong>);
dimensions.Add(Analytics.Data.<strong>Time.Dimensions</strong>.<strong><em>month</em></strong>);&nbsp;</pre><p>there are other 4 Overloaded Methods for the <em style="font-weight: bold;">GetGaDataTable </em>mentioned above</p><pre lang="C#">&nbsp;DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List&lt;Data.DataItem&gt; metricsList)</pre><pre lang="C#">DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List&lt;Data.DataItem&gt; metricsList, List&lt;Data.DataItem&gt; sortList)</pre><pre lang="C#">DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List&lt;Data.DataItem&gt; metricsList, List&lt;Data.DataItem&gt; dimensionsList, List&lt;Data.DataItem&gt; filtersList, List&lt;Data.DataItem&gt; sortList)</pre><pre lang="C#">DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List&lt;Data.DataItem&gt; metricsList, List&lt;Data.DataItem&gt; dimensionsList, List&lt;Data.DataItem&gt; filtersList, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.SamplingLevelEnum? samplingLevel, List&lt;Data.DataItem&gt; sortList, List&lt;Data.DataItem&gt; fields)&nbsp;</pre><p>the returned data table contains not just the data but preserving the data types of the data too, so string, integer, float etc. types are assigned to the columns this means you could bind it directly and start operating on the tables&nbsp;&nbsp;</p>


