using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asatr
{
    public class WayPointComparer : IComparer<WayPoint>
    {
        public int Compare(WayPoint x, WayPoint y)
        {
            return x.TotalCost.CompareTo(y.TotalCost);
        }
    }
}
