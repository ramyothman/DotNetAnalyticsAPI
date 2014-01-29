using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DesktopTest
{
    public partial class Browser : Form
    {
        public Browser()
        {
            InitializeComponent();
            document.SpecialElements.Add("br");
            document.SpecialElements.Add("hr");
            document.SpecialElements.Add("img");
            document.SpecialElements.Add("meta");
            document.SpecialElements.Add("link");
            document.SpecialElements.Add("input");
        }
        MarkupLibrary.MarkupDocument document = new MarkupLibrary.MarkupDocument();
        MarkupLibrary.AnalyzeAPI analyzeApi = new MarkupLibrary.AnalyzeAPI();
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            webBrowser1.ResetText();
            webBrowser1.WebSession.ClearCache();
            
            webBrowser1.Source = new Uri(txtAddress.Text);
            btnGetXML.Enabled = false;
            i = 1;
        }

        private void btnGetXML_Click(object sender, EventArgs e)
        {
            document.Load(source);
            GetSource();
        }

        public void GetSource()
        {
            Stream outStream;
            MarkupLibrary.MarkupElement element = analyzeApi.SearchTree(document, "h1", "itemprop", "name");
            string category = analyzeApi.GetMarkupElementValuePure(element);
            category = category.Substring(0, category.IndexOf('-')).Trim();


            MarkupLibrary.MarkupElement detailedSession = analyzeApi.SearchTree(document, "div", "id", "detail" + category.Replace(" ","_").ToLower());
            List<MarkupLibrary.Data.DataItem> dimensions = new List<MarkupLibrary.Data.DataItem>();
            List<MarkupLibrary.Data.DataItem> metrics = new List<MarkupLibrary.Data.DataItem>();
            List<MarkupLibrary.Data.DataItem> calculated = new List<MarkupLibrary.Data.DataItem>();
            int i = 0;
            MarkupLibrary.Data.ItemType type = MarkupLibrary.Data.ItemType.Dimensions;
            while (i < detailedSession.ChildElements.Count)
            {
                #region Loading Data Items
                bool itemSet = false;
                MarkupLibrary.Data.DataItem CurrentSource = new MarkupLibrary.Data.DataItem();
                CurrentSource.Category = category;
                
                if (detailedSession.ChildElements[i].Name == "h2")
                {
                    string value = analyzeApi.GetMarkupAttributeValue(detailedSession.ChildElements[i], "id");
                    if (value == category.Replace(" ", "_").ToLower() + "metrics")
                        type = MarkupLibrary.Data.ItemType.Metric;
                    else if (value == category.Replace(" ", "_").ToLower() + "calculated_metrics")
                        type = MarkupLibrary.Data.ItemType.Calculated;
                    i++;
                }
                CurrentSource.ItemType = type;
                if (detailedSession.ChildElements[i].Name == "h3")
                {


                    CurrentSource.APICommand = analyzeApi.GetMarkupElementValuePure(detailedSession.ChildElements[i].ChildElements[0]).Trim();
                    string deprecatedString = analyzeApi.GetMarkupAttributeValue(detailedSession.ChildElements[i], "class");
                    if (deprecatedString != "deprecated")
                        itemSet = true;
                    i++;
                }
                if (detailedSession.ChildElements[i].Name == "div")
                {
                    string value = detailedSession.ChildElements[i].Attributes[0].Value;
                    if (value == "ind")
                    {
                        int j = 0;
                        
                        while (j < detailedSession.ChildElements[i].ChildElements.Count)
                        {
                            MarkupLibrary.MarkupElement e = detailedSession.ChildElements[i].ChildElements[j];
                            if (e.Name == "div")
                            {
                                if (analyzeApi.GetMarkupElementValuePure(e).Trim().Contains("Web View Name"))
                                {
                                    CurrentSource.WebViewName = analyzeApi.GetMarkupElementValuePure(e.ChildElements[0]).Trim();
                                    j++;
                                    e = detailedSession.ChildElements[i].ChildElements[j];
                                }

                                if (analyzeApi.GetMarkupElementValuePure(e).Trim().Contains("App View Name"))
                                {
                                    CurrentSource.AppViewName = analyzeApi.GetMarkupElementValuePure(e.ChildElements[0]).Trim();
                                    j++;
                                    e = detailedSession.ChildElements[i].ChildElements[j];
                                }
                            }
                            if (e.Name == "table")
                            {
                                CurrentSource.DataType = analyzeApi.GetMarkupElementValuePure(e.ChildElements[0].ChildElements[1].ChildElements[0].ChildElements[0]).Trim();
                                CurrentSource.AllowedInSegments = analyzeApi.GetMarkupElementValuePure(e.ChildElements[0].ChildElements[1].ChildElements[1]).Trim() == "Yes";

                            }
                            if (e.Name == "p")
                            {
                                string desc = analyzeApi.GetMarkupElementValuePure(e);

                                CurrentSource.Description += desc.Trim() == "Attributes\r\n:" ? "" : desc;
                            }
                            j++;
                        }

                        MarkupLibrary.MarkupElement calculationElement = analyzeApi.SearchTreeNodes(detailedSession.ChildElements[i], "pre", "class", "prettyprint");
                        if (calculationElement != null)
                        {
                            CurrentSource.Description += " (" + analyzeApi.GetMarkupElementValuePure(calculationElement) + " ) ";
                        }

                    }
                }
                if (type == MarkupLibrary.Data.ItemType.Dimensions && itemSet)
                    dimensions.Add(CurrentSource);
                else if (type == MarkupLibrary.Data.ItemType.Metric && itemSet)
                    metrics.Add(CurrentSource);
                else if (type == MarkupLibrary.Data.ItemType.Calculated && itemSet)
                    calculated.Add(CurrentSource);
                i++;
                #endregion
            }

            XmlDocument xml = new XmlDocument();
            XmlElement root;
            if (System.IO.File.Exists("DataItems.xml"))
                xml.Load("DataItems.xml");
                
            if (!xml.HasChildNodes)
            {
                xml.AppendChild(xml.CreateXmlDeclaration("1.0", "", ""));
                root = xml.CreateElement("DataItems");
                xml.AppendChild(root);
            }
            else
                root = xml.DocumentElement;
            XmlElement categoryElement = xml.CreateElement("DataCategory");
            XmlAttribute categoryName = xml.CreateAttribute("Name");
            categoryName.Value = category;
            categoryElement.Attributes.Append(categoryName);

            XmlElement itemTypeElement = xml.CreateElement("ItemType");
            XmlAttribute itemTypeNameAttribute = xml.CreateAttribute("Name");
            itemTypeNameAttribute.Value = "Dimensions";
            itemTypeElement.Attributes.Append(itemTypeNameAttribute);

            XmlAttribute itemTypeValueAttribute = xml.CreateAttribute("Value");
            itemTypeValueAttribute.Value = "1";
            itemTypeElement.Attributes.Append(itemTypeValueAttribute);
            categoryElement.AppendChild(itemTypeElement);
            foreach (MarkupLibrary.Data.DataItem item in dimensions)
            {
                XmlElement dataItemElement = xml.CreateElement("DataItem");
                dataItemElement.InnerText = item.Description;

                XmlAttribute attribute = xml.CreateAttribute("Name");
                attribute.Value = item.APICommand.Replace("ga:","");
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("APICommand");
                attribute.Value = item.APICommand;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("WebViewName");
                attribute.Value = item.WebViewName;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("AppViewName");
                attribute.Value = item.AppViewName;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("DataType");
                attribute.Value = item.DataType;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("AllowedInSegments");
                attribute.Value = item.AllowedInSegments.ToString();
                dataItemElement.Attributes.Append(attribute);

                itemTypeElement.AppendChild(dataItemElement);
                
            }


            itemTypeElement = xml.CreateElement("ItemType");
            itemTypeNameAttribute = xml.CreateAttribute("Name");
            itemTypeNameAttribute.Value = "Metrics";
            itemTypeElement.Attributes.Append(itemTypeNameAttribute);

            itemTypeValueAttribute = xml.CreateAttribute("Value");
            itemTypeValueAttribute.Value = "2";
            itemTypeElement.Attributes.Append(itemTypeValueAttribute);
            categoryElement.AppendChild(itemTypeElement);
            foreach (MarkupLibrary.Data.DataItem item in metrics)
            {
                XmlElement dataItemElement = xml.CreateElement("DataItem");
                dataItemElement.InnerText = item.Description;

                XmlAttribute attribute = xml.CreateAttribute("Name");
                attribute.Value = item.APICommand.Replace("ga:", "");
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("APICommand");
                attribute.Value = item.APICommand;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("WebViewName");
                attribute.Value = item.WebViewName;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("AppViewName");
                attribute.Value = item.AppViewName;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("DataType");
                attribute.Value = item.DataType;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("AllowedInSegments");
                attribute.Value = item.AllowedInSegments.ToString();
                dataItemElement.Attributes.Append(attribute);

                itemTypeElement.AppendChild(dataItemElement);

            }


            itemTypeElement = xml.CreateElement("ItemType");
            itemTypeNameAttribute = xml.CreateAttribute("Name");
            itemTypeNameAttribute.Value = "Calculated";
            itemTypeElement.Attributes.Append(itemTypeNameAttribute);

            itemTypeValueAttribute = xml.CreateAttribute("Value");
            itemTypeValueAttribute.Value = "3";
            itemTypeElement.Attributes.Append(itemTypeValueAttribute);
            categoryElement.AppendChild(itemTypeElement);
            foreach (MarkupLibrary.Data.DataItem item in calculated)
            {
                XmlElement dataItemElement = xml.CreateElement("DataItem");
                dataItemElement.InnerText = item.Description;

                XmlAttribute attribute = xml.CreateAttribute("Name");
                attribute.Value = item.APICommand.Replace("ga:", "");
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("APICommand");
                attribute.Value = item.APICommand;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("WebViewName");
                attribute.Value = item.WebViewName;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("AppViewName");
                attribute.Value = item.AppViewName;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("DataType");
                attribute.Value = item.DataType;
                dataItemElement.Attributes.Append(attribute);

                attribute = xml.CreateAttribute("AllowedInSegments");
                attribute.Value = item.AllowedInSegments.ToString();
                dataItemElement.Attributes.Append(attribute);

                itemTypeElement.AppendChild(dataItemElement);

            }

            root.AppendChild(categoryElement);

            xml.Save("DataItems.xml");

            
           
        }

       
        int i = 1;
        string source = "";
        private void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender, Awesomium.Core.FrameEventArgs e)
        {
            if (i < 3)
                i++;
            else
            {
                i = 1;
                if (isDomReady)
                    source = webBrowser1.ExecuteJavascriptWithResult("document.documentElement.outerHTML").ToString();
                btnGetXML.Enabled = true;
            }
        }
        bool isDomReady = false;
        private void Awesomium_Windows_Forms_WebControl_DocumentReady(object sender, Awesomium.Core.UrlEventArgs e)
        {
            isDomReady = true;
        }
    }
}
    