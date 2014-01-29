using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MarkupLibrary
{
    public class MarkupDocument
    {
        #region Constructors
        /// <summary>
        /// MarkupDocument Constructor
        /// </summary>
        public MarkupDocument()
        {

        }
        /// <summary>
        /// MarkupDocument Constructor which Loads the Document Elements from the Provided Markup String
        /// </summary>
        public MarkupDocument(string markup)
        {
            Load(markup);
        }
        #endregion

        #region Properties
        private string _name = "";
        /// <summary>
        /// The Element Name
        /// </summary>
        public string Name
        {
            set { _name = value.Trim().ToLower(); }
            get { return _name; }
        }
        private List<string> _content = new List<string>();
        /// <summary>
        /// The List of Contents of the Element its placed in a way that the first content element is before the first ChildElement then the second one is after the first ChildElement and Before the SecondChildElement and so on.
        /// </summary>
        public List<string> Content
        {
            set { _content = value; }
            get { return _content; }
        }
        public List<MarkupElement> ChildElements = new List<MarkupElement>();
        public List<string> SpecialElements = new List<string>();
        #endregion

        #region Methods
        /// <summary>
        /// Loads the Document Elements from the Provided Markup String
        /// </summary>
        public void Load(string markup)
        {
            MarkupDocument.ParseString(this,markup);
        }
        /// <summary>
        /// Insert Content in a MarkupElement after removing the tag
        /// </summary>
        private void InsertContent(MarkupElement element, string workingMarkup, string close, int pass)
        {
            int index = workingMarkup.IndexOf(close) + pass;
            int length = workingMarkup.Length - index;
            if (index > -1 && (length + index) <= workingMarkup.Length)
                workingMarkup = workingMarkup.Substring(index, length);
            if (element != null)
                element.Content.Add(workingMarkup);
        }

        /// <summary>
        /// Insert Content in a MarkupDocument after removing the tag
        /// </summary>
        private void InsertContent(string workingMarkup, string close, int pass)
        {
            int index = workingMarkup.IndexOf(close) + pass;
            int length = workingMarkup.Length - index;
            if (index > -1 && (length + index) <= workingMarkup.Length)
                workingMarkup = workingMarkup.Substring(index, length);
            
                this.Content.Add(workingMarkup);
        }
        private bool IsSpecial(string workingMarkup, string close, int pass)
        {
            workingMarkup = workingMarkup.Replace("/", "");
            int index = workingMarkup.IndexOf(close) - pass;
            int length = workingMarkup.Length - index;
            if (index > -1 && index < workingMarkup.Length)
                workingMarkup = workingMarkup.Substring(0, index);

            return this.SpecialElements.Contains(workingMarkup);
        }

        /// <summary>
        /// Overrides the To String Method to retrieve the element tag with its contents and inner elements
        /// </summary>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            int index = 0;
            for (int i = 0; i < ChildElements.Count; i++)
            {
                if (i < Content.Count)
                    str.Append(Content[i]);
                str.Append(ChildElements[i]);
                index++;
            }
            while (index < Content.Count)
            {
                str.Append(Content[index]);
                index++;
            }
            
            return str.ToString();
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// A Static Method that takes a MarkupDocument and a markup string and loads in the document the parsed string
        /// Here I use regular expressions for parsing the documents like retrieving the element name the attributes etc.
        /// </summary>
        public static void ParseString(MarkupDocument document, string markup)
        {
            List<string> result = new List<string>();
            document.ChildElements.Clear();
            Regex r;
            Match m;
            string[] markups = markup.Split('<');
            MarkupElement parentElement = null;
            int markupCounter = 0;
            foreach (string str in markups)
            {
                string workingMarkup = str;
                if (str.Trim().Length == 0)
                    continue;
                #region Closing Tag
                //Check if this is a closing element or not
                if (workingMarkup.TrimStart().StartsWith("/"))
                {
                    //Check if a parent element exists or not
                    if (parentElement != null)
                    {
                        //Navigate up one level
                        if (document.IsSpecial(workingMarkup,">",1))
                        {
                            document.InsertContent(parentElement, workingMarkup, ">", 1);
                            continue;
                        }
                        parentElement = parentElement.ParentElement;
                        //Insert Markup in the parentElement content
                        document.InsertContent(parentElement, workingMarkup, ">", 1);
                    }
                    else
                    {
                        if (document.IsSpecial(workingMarkup, ">", 1))
                        {
                            document.InsertContent(workingMarkup, ">", 1);
                            continue;
                        }
                        //Adding an Element in case a closing tag in the beginning o the document
                        #region Adding The Element
                        r = new Regex("^\\s*\\w*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        m = r.Match(workingMarkup);
                        if (m.Success && m.Groups[0].Value.Trim().Length > 0)
                        {
                            MarkupElement initElement = new MarkupElement();
                            initElement.ParentElement = parentElement;
                            initElement.Name = m.Groups[0].Value;
                            initElement.Document = document;
                            initElement.IsSelfClosed = true;
                            document.ChildElements.Add(initElement);
                        }
                        #endregion
                        //Insert Markup in the document content
                        document.InsertContent(workingMarkup, ">", 1);

                    }
                    continue;
                }
                #endregion

                MarkupElement currentElement = new MarkupElement();
                currentElement.Document = document;
                #region Element Name
                currentElement.ParentElement = parentElement;
                //This regular expression will extract the element name from the tag.
                r = new Regex("^\\s*\\w*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                m = r.Match(workingMarkup);
                if (m.Success && m.Groups[0].Value.Trim().Length > 0)
                {
                    currentElement.Name = m.Groups[0].Value;
                    workingMarkup = workingMarkup.TrimStart(' ');
                }
                else
                    continue;
                if(currentElement.Name != "")
                    workingMarkup = workingMarkup.Substring(currentElement.Name.Length, (workingMarkup.Length - currentElement.Name.Length));
                else
                    workingMarkup = workingMarkup.Replace(currentElement.Name, "");
                #endregion

                #region Retrieve Element Attributes
                //This regular expression will extract an attribute with its value at a time
                workingMarkup = workingMarkup.Replace("\"", "");
                workingMarkup = workingMarkup.Replace("'", "");
                r = new Regex("(\\S+)=[\"']?((?:.(?![\"']?\\s+(?:\\S+)=|[>\"']))+.)[\"']?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    
                //new Regex("\\S*\\s*=\\s*\\S*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                
                for (m = r.Match(workingMarkup); m.Success; m = m.NextMatch())
                {
                    string tag = m.Groups[0].Value;
                    string[] tagSplit = tag.Split('=');
                    MarkupAttribute attribute = new MarkupAttribute();
                    attribute.Name = tagSplit[0];
                    attribute.Value = tagSplit[1];
                    currentElement.Attributes.Add(attribute);
                }


                #endregion

                //Setting the element parent
                currentElement.ParentElement = parentElement;
                #region Add Element
                if (parentElement == null)
                    document.ChildElements.Add(currentElement);
                else
                    parentElement.ChildElements.Add(currentElement);
                #endregion

                #region Add Content
                if (!str.Contains("/>"))
                {
                    if (!document.SpecialElements.Contains(currentElement.Name))
                    {
                        parentElement = currentElement;
                        document.InsertContent(currentElement, workingMarkup, ">", 1);
                    }
                    else if (parentElement != null)
                    {
                        document.InsertContent(parentElement, workingMarkup, ">", 1);
                    }
                    else
                    {
                        document.InsertContent(workingMarkup, ">", 1);
                    }
                }
                else
                {
                    currentElement.IsSelfClosed = true;
                    document.InsertContent(parentElement, workingMarkup, "/>", 2);
                }
                #endregion
                markupCounter++;
            }
        }
        #endregion
    }
}
