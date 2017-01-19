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
        // check order changing on summation
        public void CheckOrderChange()
        {
            // cut one order from right
            var sum =
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, 1) +
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 1);
            Assert.Equal(2, sum.Order);

            // cut one order from left
            sum =
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, 1) -
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 4);
            Assert.Equal(2, sum.Order);

            // cut two orders from right
            sum =
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, 1) -
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 2) +
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 3);
            Assert.Equal(1, sum.Order);

            // cut one from right and one from left
            sum =
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, 1) -
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 2) +
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 1);
            Assert.Equal(1, sum.Order);
        }

        [Fact]
        // check returning null on creating bad order
        public void CheckBadOrderCreating()
        {
            Assert.Null(FiniteDifference.GetFiniteDifferenceByOrder(-1));
            Assert.Null(0 * FiniteDifference.GetFiniteDifferenceByOrder(1));
            // check summations
            Assert.Null(FiniteDifference.GetFiniteDifferenceByOrder(3) - FiniteDifference.GetFiniteDifferenceByOrder(3));
            Assert.Null(FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, 2) -
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 3) +
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 2));
        }

        [Fact]
        public void CheckOperationsWithNotFiniteDifference()
        {
            // create normal FiniteDifference and two non-FiniteDifferences
            FiniteDifference order11 = FiniteDifference.GetFiniteDifferenceByOrder(1);
            FiniteDifference not1 = FiniteDifference.GetFiniteDifferenceByOrder(-1);
            FiniteDifference not2 = FiniteDifference.GetFiniteDifferenceByOrder(-1);
            // check equality
            Assert.True(order11 != not1);
            Assert.True(not1 == not2);
            // check multiplication
            Assert.Null(not1 * 2);
            // check summation
            Assert.True(order11 == order11 + not1);
            Assert.Null(not1 + not2);
        }

    }
}
