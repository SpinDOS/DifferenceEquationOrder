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
            Assert.Equal(0, dif.MinimumH);
            Assert.Equal(order, dif.MaximumH);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-2)]
        [InlineData(5)]
        public void SetMinH(int minH)
        {
            Random rnd = new Random();
            int order = rnd.Next(10);
            FiniteDifference dif = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(order, minH);
            Assert.Equal(minH, dif.MinimumH);
            Assert.Equal(order, dif.Order);
            Assert.Equal(minH + order, dif.MaximumH);
        }
    }
}
