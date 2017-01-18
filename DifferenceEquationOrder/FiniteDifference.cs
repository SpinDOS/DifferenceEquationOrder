using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DifferenceEquationOrder
{
    public sealed class FiniteDifference
    {
        private FiniteDifference() { }
        public int Order { get; private set; }

        public static FiniteDifference GetFiniteDifferenceByOrder(int order)
        {
            if (order < 0)
                throw new ArgumentException("Finite difference order can not be less than zero");
            FiniteDifference result = new FiniteDifference();
            result.Order = order;
            return result;
        }
    }
}
