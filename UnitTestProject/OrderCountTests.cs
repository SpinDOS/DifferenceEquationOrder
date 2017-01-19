using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DifferenceEquationOrder;
using Xunit;

namespace UnitTestProject
{
    // tests to control order counting
    public class OrderCountTests
    {
        [Fact]
        public void CheckOrderChange()
        {
            // cut one order from right
            var sum =
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, 1) +
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 4);
            Assert.Equal(2, sum.Order);

            // cut two orders from right
            sum =
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, 1) +
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 2) -
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 3);
            Assert.Equal(1, sum.Order);

            // cut one from right and one from left
            sum =
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, 1) +
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 2) -
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 1);
            Assert.Equal(1, sum.Order);
        }

    }
}
