using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GranDen.Blazor.Bootstrap.SwitchButton.Options
{
    /// <summary>
    /// Switch button option data type class
    /// </summary>
    public class SwitchOption
    {
        /// <summary>
        /// Set UI when Switch State is On
        /// </summary>
        public string Onlabel { get; set; } = "On";

        /// <summary>
        /// Set UI when Switch State is Off
        /// </summary>
        public string Offlabel { get; set; } = "Off";

        /// <summary>
        /// Set CSS class names when Switch State is On
        /// </summary>
        public string Onstyle { get; set; } = "primary";

        /// <summary>
        /// Set CSS class names when Switch State is Off
        /// </summary>
        public string Offstyle { get; set; } = "light";

        /// <summary>
        /// (Optional) Set Switch size in CSS string representation
        /// </summary>
        public string Size { get; set; } = null;

        /// <summary>
        /// (Optional) Set Switch CSS style
        /// </summary>
        public string Style { get; set; } = null;

        /// <summary>
        /// (Optional) Set Switch width
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// (Optional) Set Switch height
        /// </summary>
        public int? Height { get; set; }
    }
}