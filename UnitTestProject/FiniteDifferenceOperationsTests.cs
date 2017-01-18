using DifferenceEquationOrder;
using Xunit;

namespace UnitTestProject
{
    // check FiniteDifference summation subtraction multiplication
    public class FiniteDifferenceOperationsTests
    {
        [Fact]
        // check equality operator
        public void CheckEquality()
        {
            // create two equal FiniteDifference
            FiniteDifference l = FiniteDifference.GetFiniteDifferenceByOrder(2);
            FiniteDifference r = FiniteDifference.GetFiniteDifferenceByOrder(2);
            Assert.True(l == r);
            // change coefficient
            l[0] = 2;
            Assert.False(l == r);
            // revert coefficient
            l[0] = 1;
            Assert.True(l == r);
            // create FiniteDifference with different order
            r = FiniteDifference.GetFiniteDifferenceByOrder(3);
            Assert.False(l == r);
            // change right's coefficients to match left's
            r[0] = 1;
            r[1] = -2;
            r[2] = 1;
            Assert.False(l == r);
            // create FiniteDifference with the same order but another minimumH
            r = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 1);
            Assert.False(l == r);
            // create same FiniteDifference
            l = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 1);
            Assert.True(l == r);
        }
    }
}
