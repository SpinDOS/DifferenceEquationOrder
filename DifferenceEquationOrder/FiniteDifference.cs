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

        public int MinimumH { get; private set; }
        public int MaximumH { get; private set; }
        public int Order { get; private set; }

        public static FiniteDifference GetFiniteDifferenceByOrder(int order)
            => GetFiniteDifferenceByOrderAndMinH(order, 0);

        public static FiniteDifference GetFiniteDifferenceByOrderAndMinH(int order, int minH)
        {
            if (order < 0)
                throw new ArgumentException("Finite difference order can not be less than zero");
            FiniteDifference result = new FiniteDifference();
            result.Order = order;
            result.MinimumH = minH;
            result.MaximumH = minH + order;
            return result;
        }
    }
}
