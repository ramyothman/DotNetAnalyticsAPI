using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MarkupLibrary
{
    /// <summary>
    /// This Class Represents a Markup Element
    /// </summary>
    public class MarkupElement
    {
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
        private int _level = -1;
        /// <summary>
        /// The Level of the Element in the document
        /// </summary>
        public int Level
        {
            get 
            {
                if (_level != -1)
                    return _level;
                int result = 0;
                if (ParentElement == null)
                    return result;

                MarkupElement current = ParentElement;
                while (current != null)
                {
                    current = current.ParentElement;
                    result++;
                }
                _level = result;
                return result;
            }
        }
        private bool _isSelfClosed = false;
        /// <summary>
        /// This value is true if the element should be closed without a closing element ex: <Customer/>
        /// </summary>
        public bool IsSelfClosed
        {
            set { _isSelfClosed = value; }
            get 
            {
                if (Document != null)
                {
                    if (Document.SpecialElements.Contains(Name))
                        _isSelfClosed = true;
                }
                return _isSelfClosed; 
            }
        }
        
        /// <summary>
        /// The Element Tag (it's the string of all attributes inside the element) ex: in tag <img src="http://www.ramymostafa.com/img1.jpg" width="123px"/>
        /// The tag value will hold [src="http://www.ramymostafa.com/img1.jpg" width="123px"]
        /// </summary>
        public string Tag
        {
            get 
            {
                StringBuilder result = new StringBuilder();
                foreach (MarkupAttribute attribute in Attributes)
                {
                    result.Append(" " + attribute.ToString());
                }
                return result.ToString(); 
            }
        }

        public List<MarkupElement> ChildElements = new List<MarkupElement>();
        public List<MarkupAttribute> Attributes = new List<MarkupAttribute>();
        
        public MarkupElement ParentElement = null;
        public MarkupDocument Document = null;

        private List<string> _content = new List<string>();
        /// <summary>
        /// The List of Contents of the Element its placed in a way that the first content element is before the first ChildElement then the second one is after the first ChildElement and Before the SecondChildElement and so on.
        /// </summary>
        public List<string> Content
        {
            set { _content = value; }
            get { return _content; }
        }
        
        #endregion

        #region Methods
        public string ToTextString()
        {
            return ToTextString(true);
        }
        public string ToTextString(bool brSeperator)
        {
            StringBuilder resultElement = new StringBuilder();
            string result = "";
            
            
            int index = 0;
            foreach (MarkupElement element in ChildElements)
            {
                if (index < Content.Count)
                    resultElement.Append(Content[index]);
                if (!brSeperator)
                    resultElement.Append(Environment.NewLine);
                else
                    resultElement.Append("<br />");
                resultElement.Append(element.ToTextString(brSeperator));
                index++;
            }
            while (index < Content.Count)
            {

                resultElement.Append(Content[index]);
                if (!brSeperator)
                    resultElement.Append(Environment.NewLine);
                else
                    resultElement.Append("<br />");
                index++;
            }
            result += resultElement.ToString();
            
            return result;
        }
        public override string ToString()
        {
            StringBuilder resultElement = new StringBuilder();
            string result = "";
            bool isSolfClosed = IsSelfClosed;
            if (this.Content.Count == 0 && this.ChildElements.Count == 0)
                isSolfClosed = true;
            if (!isSolfClosed)
                result = "<" + Name + Tag + ">";
            else
                result = "<" + Name + Tag + "/>";
            int index = 0;
            foreach (MarkupElement element in ChildElements)
            {
                if(index < Content.Count)
                    resultElement.Append(Content[index]);
                resultElement.Append(element.ToString());
                index++;
            }
            while (index < Content.Count)
            {
                
                resultElement.Append(Content[index]);
                index++;
            }
            result += resultElement.ToString();
            if (!isSolfClosed)
                result += "</" + Name + ">"; 
            
            return result;
        }
        #endregion
    }
}
