using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DifferenceEquationOrder
{
    public static class DoubleIsZeroExtension
    {
        /// <summary>
        /// Check if the double is too close to zero
        /// </summary>
        /// <param name="d">double to check</param>
        /// <returns>true, if d is too close to zero</returns>
        public static bool IsZero(this double d) => Math.Abs(d) < 0.0000000001;
    }
}
