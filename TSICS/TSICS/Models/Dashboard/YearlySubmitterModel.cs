using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class YearlySubmitterModel
    {
        public class ResponseJson
        {
            [JsonProperty("status")]
            public Status Status { get; set; }

            [JsonProperty("data")] 
            public Data Data { get; set; } 
        }
        public class Status
        {
            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }
        public class Data
        {
            [JsonProperty("doughnut_gauge")]
            public DoughnutGauge DoughnutGauge { get; set; }

            [JsonProperty("combo_bar")]
            public ComboBar ComboBar { get; set; }

            [JsonProperty("percent_bar")]
            public PercentBar PercentBar { get; set; }

        }

        #region DoughnutGauge
        public class DoughnutGauge
        {

            [JsonProperty("background_color")]
            public List<string> BackgroundColor { get; set; }

            [JsonProperty("value")]
            public Double Value { get; set; }

            [JsonProperty("gauge_limits")]
            public List<decimal> GaugeLimits { get; set; }

        }
        #endregion

        #region ComboBar
        public class ComboBar
        {
            [JsonProperty("x_axis")]
            public XaxisComboBar Xaxis { get; set; }

            [JsonProperty("y_axis")]
            public YaxisComboBar Yaxis { get; set; }

            [JsonProperty("data")]
            public DataComboBar Data { get; set; }
        }

        #region X_Axis
        public class XaxisComboBar
        {

            [JsonProperty("label")]
            public List<string> Label { get; set; }

            [JsonProperty("min_target")]
            public MinTargetXaxisComboBar MinTarget { get; set; }
        }
        public class MinTargetXaxisComboBar
        {
            [JsonProperty("value")]
            public decimal Value { get; set; }
            [JsonProperty("border_color")]
            public string BorderColor { get; set; }
            [JsonProperty("border_width")]
            public decimal BorderWidth { get; set; }
        }
        #endregion

        #region Y_Axis
        public class YaxisComboBar
        {
            [JsonProperty("left")]
            public LeftYaxisComboBar Left { get; set; }

            [JsonProperty("right")]
            public RightYaxisComboBar Right { get; set; }
        }
        public class LeftYaxisComboBar
        {
            [JsonProperty("min")]
            public decimal Min { get; set; }

            [JsonProperty("max")]
            public decimal Max { get; set; }
        }
        public class RightYaxisComboBar
        {
            [JsonProperty("min")]
            public decimal Min { get; set; }

            [JsonProperty("max")]
            public decimal Max { get; set; }
        }
        #endregion

        #region Data
        public class DataComboBar
        {
            [JsonProperty("line")]
            public LineDataComboBar Line { get; set; }

            [JsonProperty("bar")]
            public BarDataComboBar Bar { get; set; }
        }
        public class LineDataComboBar
        {
            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }

            [JsonProperty("border_width")]
            public decimal BorderWidth { get; set; }

            [JsonProperty("value")]
            public List<decimal> Value { get; set; }
        }
        public class BarDataComboBar
        {
            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }

            [JsonProperty("border_width")]
            public decimal BorderWidth { get; set; }

            [JsonProperty("value")]
            public List<decimal> Value { get; set; }
        }
        #endregion

        #endregion

        #region PercentBar
        public class PercentBar
        {
            [JsonProperty("y_axis")]
            public YaxisPercentBar YaxisPercentBar { get; set; }

            [JsonProperty("data")]
            public DataPercentBar DataPercentBar { get; set; }

            [JsonProperty("tooltip")]
            public TooltipPercentBar TooltipPercentBar { get; set; }
        }
        public class YaxisPercentBar
        {
            [JsonProperty("line")]
            public LineYaxisPercentBar LineYaxisPercentBar { get; set; }

            [JsonProperty("legend")]
            public string Legend { get; set; }
        }
        public class LineYaxisPercentBar
        {
            [JsonProperty("min")]
            public decimal Min { get; set; }

            [JsonProperty("max")]
            public decimal Max { get; set; }

            [JsonProperty("div")]
            public decimal Div { get; set; }
        }
        public class DataPercentBar
        {
            [JsonProperty("value")]
            public decimal Value { get; set; }
            [JsonProperty("min")]
            public decimal Min { get; set; }
        }
        public class TooltipPercentBar
        {
            [JsonProperty("value")]
            public string Value { get; set; }
        }
        #endregion
    }
}