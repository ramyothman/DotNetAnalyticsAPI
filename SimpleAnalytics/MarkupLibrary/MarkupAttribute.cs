using System;
using System.Collections.Generic;
using System.Text;

namespace MarkupLibrary
{
    /// <summary>
    /// This Class Represents a Markup Attribute
    /// </summary>
    public class MarkupAttribute
    {
        private string _name = "";
        /// <summary>
        /// The Attribute Name
        /// </summary>
        public string Name
        {
            set { _name = value.Trim().ToLower(); }
            get { return _name; }
        }
        private string _value = "";
        /// <summary>
        /// The Attribute Value
        /// </summary>
        public string Value
        {
            set 
            {
                string myvalue = value.Trim().ToLower();
                if (myvalue.StartsWith("\""))
                    myvalue = myvalue.Remove(0, 1);
                myvalue = myvalue.Replace("//>", "");
                myvalue = myvalue.Replace(">", "");
                if (myvalue.EndsWith("\""))
                    myvalue = myvalue.Remove(myvalue.Length - 1, 1);
                _value = myvalue; 
            }
            get { return _value; }
        }

        /// <summary>
        /// Override the ToString to return the attribute in form of
        /// name="value"
        /// </summary>
        public override string ToString()
        {
            return Name + "=\"" + Value + "\"";
        }
    }
}
