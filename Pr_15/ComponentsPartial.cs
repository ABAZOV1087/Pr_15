using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_15
{
    public partial class Components
    {
        public string FullName => $"{Manufacturers.Name} {ModelName}";

        public string DisplayPrice => $"{Price} руб.";

        public bool IsCpu => CategoryID == 1;
        public bool IsMotherboard => CategoryID == 2;
        public bool IsRAM => CategoryID == 3;
        public bool IsCase => CategoryID == 4;
        public bool IsCooler => CategoryID == 5;
    }
}
