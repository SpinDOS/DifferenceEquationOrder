using System;
using DifferenceEquationOrder;
using Xunit;

namespace UnitTestProject
{
    public class FiniteDifferenceTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void CheckOrderAfterCreating(int order)
        {
            FiniteDifference dif = FiniteDifference.GetFiniteDifferenceByOrder(order);
            Assert.Equal(order, dif.Order);
        }
    }
}
