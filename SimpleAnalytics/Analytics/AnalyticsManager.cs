using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Analytics
{
    
    public class AnalyticsManager
    {
        #region Declarations
        ServiceAccountCredential credential;
        public Google.Apis.Analytics.v3.AnalyticsService analyticsService;
        bool IsInitialized = false;

        public  List<Google.Apis.Analytics.v3.Data.Profile> Profiles {get; set;}
        public  Google.Apis.Analytics.v3.Data.Profile DefaultProfile { set; get; }
        public bool InitFailed { get; set; }
        public Exception FailureException { get; set; }

        /// <summary>
        /// Lets you enforce per-user quotas when calling the API from a server-side application, based on IP.
        /// 
        /// Ignored if QuotaUser is set
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// Lets you enforce per-user quotas from a server-side application even in cases when the user's IP address is unknown.
        /// 
        /// You can choose any arbitrary string that uniquely identifies a user, but it is limited to 40 characters.
        /// </summary>
        public string QuotaUser { get; set; }

        #endregion

        #region Constructor & Initialization
        public AnalyticsManager()
        {

        }

        public AnalyticsManager(string certificateKeyPath, string apiEmail)
        {
            InitializeService(certificateKeyPath, apiEmail);
        }

        public void InitializeService(string certificateKeyPath, string apiEmail)
        {
            if (!IsInitialized)
            {
                var certificate = new X509Certificate2(certificateKeyPath, "notasecret", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
                string x = certificate.IssuerName.Name;
                credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(apiEmail)
                {
                    Scopes = new[] { Google.Apis.Analytics.v3.AnalyticsService.Scope.Analytics }
                }.FromCertificate(certificate));

                analyticsService = new Google.Apis.Analytics.v3.AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Jareeda"
                });

                IsInitialized = true;
            }
        }

        public void InitializeService(string certificateKeyPath, string apiEmail, Google.Apis.Analytics.v3.AnalyticsService.Scope scope)
        {
            try
            {
                if (!IsInitialized)
                {
                    var certificate = new X509Certificate2(certificateKeyPath, "notasecret", X509KeyStorageFlags.Exportable);

                    credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(apiEmail)
                    {
                        Scopes = new[] { scope.ToString() }
                    }.FromCertificate(certificate));
                }

                analyticsService = new Google.Apis.Analytics.v3.AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Jareeda"
                });

                IsInitialized = true;
            }
            catch (Exception ex)
            {
                InitFailed = true;
                FailureException = ex;
            }
        }
        #endregion

        #region Management Resource Methods
        public void LoadAnalyticsProfiles()
        {
            Google.Apis.Analytics.v3.ManagementResource.ProfilesResource.ListRequest request = analyticsService.Management.Profiles.List("~all", "~all");
            Google.Apis.Analytics.v3.Data.Profiles profiles = request.Execute();
            if (Profiles == null)
                Profiles = new List<Google.Apis.Analytics.v3.Data.Profile>();
            foreach (Google.Apis.Analytics.v3.Data.Profile p in profiles.Items)
            {
                Profiles.Add(p);
            }
        }

        public bool HasProfile()
        {
            return DefaultProfile != null;
        }
        public void SetDefaultAnalyticProfile(string profileId)
        {
            var profile = (from p in Profiles where p.Id == profileId select p).FirstOrDefault();
            if (profile != null)
                DefaultProfile = profile;
        }
        #endregion

        #region Data Resource Methods

        #region GetGaData
        public Google.Apis.Analytics.v3.Data.GaData GetGaData(string profileid, DateTime startDate, DateTime endDate, string metrics)
        {
            return GetGaData(profileid, startDate, endDate, metrics, "");
        }
        public Google.Apis.Analytics.v3.Data.GaData GetGaData(string profileid, DateTime startDate, DateTime endDate, string metrics, string sort)
        {
            return GetGaData(profileid, startDate, endDate, metrics, "", "", sort);
        }
        public Google.Apis.Analytics.v3.Data.GaData GetGaData(string profileid, DateTime startDate, DateTime endDate, string metrics, string dimensions, string filters, string sort)
        {
            return GetGaData(profileid, startDate, endDate, metrics, dimensions, filters, null, sort, "");
        }
        public Google.Apis.Analytics.v3.Data.GaData GetGaData(string profileid, DateTime startDate, DateTime endDate, string metrics, string dimensions, string filters, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.SamplingLevelEnum? samplingLevel, string sort, string fields)
        {
            return GetGaData(profileid, startDate, endDate, metrics, dimensions, filters, null, null, samplingLevel, "", sort, null, fields);
        }
        public Google.Apis.Analytics.v3.Data.GaData GetGaData(string profileid, DateTime startDate, DateTime endDate, string metrics, string dimensions, string filters, int? maxResults, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.OutputEnum? output, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.SamplingLevelEnum? samplingLevel, string segment, string sort, int? startIndex, string fields)
        {
            Google.Apis.Analytics.v3.Data.GaData data = null;

            
            try
            {
                Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest request = analyticsService.Data.Ga.Get(profileid, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), metrics);
                if (!string.IsNullOrEmpty(dimensions))
                    request.Dimensions = dimensions;
                if (!string.IsNullOrEmpty(filters))
                    request.Filters = filters;
                if (maxResults.HasValue)
                    request.MaxResults = maxResults;
                if(output.HasValue)
                    request.Output = output;
                if(samplingLevel.HasValue)
                    request.SamplingLevel = samplingLevel;
                if(!string.IsNullOrEmpty(segment))
                    request.Segment = segment;
                if(startIndex.HasValue)
                    request.StartIndex = startIndex;
                if(!string.IsNullOrEmpty(sort))
                    request.Sort = sort;
                if(!string.IsNullOrEmpty(fields))
                    request.Fields = fields;
                if (!string.IsNullOrEmpty(this.UserIp))
                    request.UserIp = this.UserIp;
                if (!string.IsNullOrEmpty(this.QuotaUser))
                    request.QuotaUser = this.QuotaUser;

                data = request.Execute();
            }
            catch (Exception ex)
            {
                InitFailed = true;
                FailureException = ex;
            }
            return data;
        }
        #endregion

        #region GetDataTable

        public System.Data.DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List<Data.DataItem> metricsList)
        {
            return GetGaDataTable(startDate, endDate, metricsList, null, null, null, null, null, null, null,false, null, null);
        }
        public System.Data.DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List<Data.DataItem> metricsList, List<Data.DataItem> sortList,bool ascending)
        {
            return GetGaDataTable(startDate, endDate, metricsList, null, null, null, null, null, null, sortList,ascending, null, null);
        }
        public System.Data.DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List<Data.DataItem> metricsList, List<Data.DataItem> dimensionsList, List<Data.DataItem> filtersList, List<Data.DataItem> sortList,bool ascending)
        {
            return GetGaDataTable(startDate, endDate, metricsList, dimensionsList, filtersList, null, null, null, null, sortList,ascending, null, null);
        }
        public System.Data.DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List<Data.DataItem> metricsList, List<Data.DataItem> dimensionsList, List<Data.DataItem> filtersList, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.SamplingLevelEnum? samplingLevel, List<Data.DataItem> sortList,bool ascending, List<Data.DataItem> fields)
        {
            return GetGaDataTable(startDate, endDate, metricsList, dimensionsList, filtersList, null, null, samplingLevel, null, sortList,ascending, null, fields);
        }

        public System.Data.DataTable GetGaDataTable(DateTime startDate, DateTime endDate, List<Data.DataItem> metricsList, List<Data.DataItem> dimensionsList, List<Data.DataItem> filtersList, int? maxResults, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.OutputEnum? output, Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.SamplingLevelEnum? samplingLevel, List<Data.DataItem> segmentList, List<Data.DataItem> sortList,bool ascending, int? startIndex, List<Data.DataItem> fieldsList)
        {
            if (DefaultProfile == null)
                throw new Exception("Please set a default profile first using SetDefaultAnalyticProfile method");
            string sort = "";
            if(ascending)
                sort = Data.DataItem.GetString(sortList);
            else
                sort = Data.DataItem.GetString(sortList, "-");
            Google.Apis.Analytics.v3.Data.GaData gaData = GetGaData("ga:" + DefaultProfile.Id, startDate, endDate, Data.DataItem.GetString(metricsList), Data.DataItem.GetString(dimensionsList), Data.DataItem.GetStringFilter(filtersList), maxResults, output, samplingLevel, Data.DataItem.GetString(segmentList), sort, startIndex, Data.DataItem.GetString(fieldsList));
            System.Data.DataTable table = BuildTableColumns(metricsList, dimensionsList);
            if(gaData != null)
                table = BuildTableRows(gaData, table);
            return table;
        }
        #endregion

        private System.Data.DataTable BuildTableColumns(List<Data.DataItem> metricsList, List<Data.DataItem> dimensionsList)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            if (dimensionsList != null)
            {
                foreach (Data.DataItem item in dimensionsList)
                {
                    System.Data.DataColumn column = new System.Data.DataColumn(item.Name, item.Type);
                    column.Caption = item.WebViewName;
                    table.Columns.Add(column);
                }
            }

            foreach (Data.DataItem item in metricsList)
            {
                System.Data.DataColumn column = new System.Data.DataColumn(item.Name, item.Type);
                column.Caption = item.WebViewName;
                table.Columns.Add(column);
            }

            return table;
        }

        private System.Data.DataTable BuildTableRows(Google.Apis.Analytics.v3.Data.GaData gaData, System.Data.DataTable table)
        {
            if (gaData.Rows == null) return table;

            foreach (var ls in gaData.Rows)
            {
                System.Data.DataRow row = table.NewRow();
                for(int i = 0; i < ls.Count(); i++)
                {
                    if (table.Columns[i].DataType == typeof(DateTime))
                    {
                        row[i] = DateTime.ParseExact(ls[i], "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        row[i] = ls[i];
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }
        #endregion

    }
}
