using System.ComponentModel;

namespace Analytics.Data
{
    public sealed class CustomVariablesorColumns
    {
        public sealed class Dimensions
        {

			[DescriptionAttribute("The name of the requested custom dimension, where XX refers the number/index of the custom dimension.")]
			public static DataItem dimensionXX(int index) { return new DataItem("dimensionXX", index); }

			[DescriptionAttribute("The name for the requested custom variable.")]
			public static DataItem customVarNameXX(int index) { return new DataItem("customVarNameXX", index); }

			[DescriptionAttribute("The filter for the requested custom variable.")]
			public static DataItem customVarValueXX(int index) { return new DataItem("customVarValueXX", index); }
                                      
        }

        public sealed class Metrics
        {

			[DescriptionAttribute("The name of the requested custom metric, where XX refers the number/index of the custom metric.")]
			public static DataItem metricXX(int index) { return new DataItem("metricXX", index); }
                                      

            public sealed class Calculated
            {
                
            }
        }
    }
}
