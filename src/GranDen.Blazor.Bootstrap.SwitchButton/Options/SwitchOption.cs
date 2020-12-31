using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GranDen.Blazor.Bootstrap.SwitchButton.Options
{
    public class SwitchOption
    {
        public string Onlabel { get; set; } = "On";

        public string Offlabel { get; set; } = "Off";

        public string Onstyle { get; set; } = "primary";

        public string Offstyle { get; set; } = "light";

        public string Size { get; set; } = null;

        public string Style { get; set; } = null;

        public int? Width { get; set; }

        public int? Height { get; set; }
    }
}
