using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asatr
{  
        public class WayPoint
        {
            public (int X, int Y) Location { get; set; }
            public float CostFromStart { get; set; } = float.MaxValue;
            public float CostFromGoal { get; set; } = float.MaxValue;
            public float TotalCost => CostFromStart + CostFromGoal;
            public WayPoint Parent { get; set; }
        }
   
}
