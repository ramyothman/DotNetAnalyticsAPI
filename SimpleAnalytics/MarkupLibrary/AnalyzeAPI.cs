using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
namespace MarkupLibrary
{
    public class AnalyzeAPI
    {
        public static string ReadUrl(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            Stream data = client.OpenRead(url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            request.Accept = "True";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
            return "";
        }

        public static string ReadFile(string url)
        {
            
            using (StreamReader reader =  System.IO.File.OpenText(url))
            {
                return reader.ReadToEnd();
            }
            return "";
        }

        public string GetMarkupAttributeValue(MarkupLibrary.MarkupElement element, string name)
        {
            if (element == null) return "";
            var elementMeta = (from n in element.Attributes where n.Name == name select n).FirstOrDefault();
            return elementMeta == null? "" : elementMeta.Value;
        }

        public string GetMarkupAttributeValueByContains(MarkupLibrary.MarkupElement element, string name)
        {
            if (element == null) return "";
            var elementMeta = (from n in element.Attributes where n.Name == name select n).FirstOrDefault();
            return elementMeta == null ? "" : elementMeta.Value;
        }

        public string GetMarkupElementValue(MarkupLibrary.MarkupElement element)
        {
            if (element == null) return "";
            MarkupLibrary.MarkupElement dostorImageContent = SearchTreeNodes(element, "div", "class", "field field-type-filefield field-field-mainimage");
            element.ChildElements.Remove(dostorImageContent);
            return element.ToTextString();

        }

        public string GetMarkupElementValuePure(MarkupLibrary.MarkupElement element)
        {
            if (element == null) return "";
            return element.ToTextString(false).Trim();
        }

        public MarkupLibrary.MarkupElement SearchTree(MarkupLibrary.MarkupDocument document, string elemName, string attrName, string search)
        {
            foreach (MarkupLibrary.MarkupElement element in document.ChildElements)
            {
                string elementName = element.Name;
                var elementMeta = (from n in element.Attributes where n.Name == attrName select n).FirstOrDefault();
                if (elementMeta != null)
                {
                    if (elementMeta.Value.ToLower() == search.ToLower())
                        return element;
                    else
                        return SearchTreeNodes(element, elemName, attrName, search);
                }
                else
                {
                    MarkupLibrary.MarkupElement elemTemp = SearchTreeNodes(element, elemName, attrName, search);
                    if (elemTemp != null)
                    {
                        return elemTemp;
                    }
                }

            }
            return null;
        }

        public MarkupLibrary.MarkupElement SearchTreeNodes(MarkupLibrary.MarkupElement parentElement, string elemName, string attrName, string search)
        {
            foreach (MarkupLibrary.MarkupElement element in parentElement.ChildElements)
            {
                string elementName = element.Name;
                var elementMeta = (from n in element.Attributes where n.Name == attrName select n).FirstOrDefault();
                if (elementMeta != null)
                {
                    if (elementMeta.Value.ToLower() == search.ToLower())
                        return element;
                    else
                    {
                        MarkupLibrary.MarkupElement elemTemp = SearchTreeNodes(element, elemName, attrName, search);
                        if (elemTemp != null)
                            return elemTemp;
                    }
                }
                else
                {
                    MarkupLibrary.MarkupElement elemTemp = SearchTreeNodes(element, elemName, attrName, search);
                    if (elemTemp != null)
                        return elemTemp;
                }
            }
            return null;
        }

        public MarkupLibrary.MarkupElement SearchTreeByContains(MarkupLibrary.MarkupDocument document, string elemName, string attrName, string search)
        {
            foreach (MarkupLibrary.MarkupElement element in document.ChildElements)
            {
                string elementName = element.Name;
                var elementMeta = (from n in element.Attributes where n.Name == attrName select n).FirstOrDefault();
                if (elementMeta != null)
                {
                    if (elementMeta.Value.ToLower().Contains(search.ToLower()))
                        return element;
                }
                else
                {
                    MarkupLibrary.MarkupElement elemTemp = SearchTreeNodesByContains(element, elemName, attrName, search);
                    if (elemTemp != null)
                    {
                        return elemTemp;
                    }
                }

            }
            return null;
        }

        public MarkupLibrary.MarkupElement SearchTreeNodesByContains(MarkupLibrary.MarkupElement parentElement, string elemName, string attrName, string search)
        {
            foreach (MarkupLibrary.MarkupElement element in parentElement.ChildElements)
            {
                string elementName = element.Name;
                var elementMeta = (from n in element.Attributes where n.Name == attrName select n).FirstOrDefault();
                if (elementMeta != null)
                {
                    if (elementMeta.Value.ToLower().Contains(search.ToLower()))
                        return element;
                    else
                    {
                        MarkupLibrary.MarkupElement elemTemp = SearchTreeNodesByContains(element, elemName, attrName, search);
                        if (elemTemp != null)
                            return elemTemp;
                    }
                }
                else
                {
                    MarkupLibrary.MarkupElement elemTemp = SearchTreeNodesByContains(element, elemName, attrName, search);
                    if (elemTemp != null)
                        return elemTemp;
                }
            }
            return null;
        }

        public MarkupLibrary.MarkupElement SearchTreeNodes(MarkupLibrary.MarkupElement parentElement, string elemName)
        {
            foreach (MarkupLibrary.MarkupElement element in parentElement.ChildElements)
            {
                string elementName = element.Name;
                if (elementName.ToLower() == elemName.ToLower()) return element;
                var elementMeta = (from n in element.ChildElements where n.Name.ToLower() == elemName.ToLower() select n).FirstOrDefault();
                if (elementMeta != null)
                {
                    return elementMeta;
                }
                else
                {
                    MarkupLibrary.MarkupElement elemTemp = SearchTreeNodes(element, elemName);
                    if (elemTemp != null)
                        return elemTemp;
                }
            }
            return null;
        }
    }
}
