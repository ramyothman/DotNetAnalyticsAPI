using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
       
        
        static void Main(string[] args)
        {
            string code = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Analytics.Data
{
    public sealed class CLASSNAME
    {
        public sealed class Dimensions
        {
            PROPEERTYDIMENSIONS
        }

        public sealed class Metrics
        {
            PROPERTYMETRICS

            public sealed class Calculated
            {
                PROPERTYCALCULATED
            }
        }
    }
}
";
            

            foreach (var element in Analytics.Data.DataItem.Document.Elements().FirstOrDefault().Elements())
            {
                string classCode = code;
                classCode = classCode.Replace("CLASSNAME", element.Attribute("Name").Value.Replace(" ",""));
                foreach (var etype in element.Elements())
                {
                    string property = @"
            [DescriptionAttribute(""DESCRIPTION"")]
            public static DataItem PROPERTYNAME = new DataItem(""PROPERTYNAME"");
                                      ";
                    string dimensions = "";
                    string metrics = "";
                    string calculated = "";
                    if (etype.Attribute("Value").Value == "1")
                    {
                        foreach (var item in etype.Elements())
                        {
                            dimensions += property.Replace("PROPERTYNAME", item.Attribute("Name").Value).Replace("DESCRIPTION",item.Value);
                        }
                        classCode = classCode.Replace("PROPEERTYDIMENSIONS", dimensions);
                    }

                    if (etype.Attribute("Value").Value == "2")
                    {
                        foreach (var item in etype.Elements())
                        {
                            metrics += property.Replace("PROPERTYNAME", item.Attribute("Name").Value).Replace("DESCRIPTION", item.Value);
                        }
                        classCode = classCode.Replace("PROPERTYMETRICS", metrics);
                    }

                    if (etype.Attribute("Value").Value == "3")
                    {
                        foreach (var item in etype.Elements())
                        {
                            calculated += property.Replace("PROPERTYNAME", item.Attribute("Name").Value).Replace("DESCRIPTION", item.Value);
                        }
                        classCode = classCode.Replace("PROPERTYCALCULATED", calculated);
                    }
                }
                System.IO.File.WriteAllText(element.Attribute("Name").Value.Replace(" ", "") + ".cs", classCode, Encoding.Unicode);
            }

        }


        static void Main1(string[] args)
        {
           
        }

        
    }
}
