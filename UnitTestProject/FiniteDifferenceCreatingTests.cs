using System;
using DifferenceEquationOrder;
using Xunit;

namespace UnitTestProject
{
    // test FiniteDifference creating
    public class FiniteDifferenceCreatingTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        // just create FiniteDifference with minimumH=0 and check order, minimumH and maximumH
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
        // create FiniteDifference with custom minimumH and random order
        public void SetMinH(int minH)
        {
            Random rnd = new Random();
            int order = rnd.Next(10);
            FiniteDifference dif = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(order, minH);
            Assert.Equal(minH, dif.MinimumH);
            Assert.Equal(order, dif.Order);
            Assert.Equal(minH + order, dif.MaximumH);
        }

        #region Checking coefficients
        // check created coefficients of the FiniteDifference with custom order and random minimumH

        [Fact]
        // order = 0
        public void CheckCoefsOrder0()
        {
            int minh = new Random().Next(-10, 10);
            FiniteDifference dif = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, minh);
            Assert.Equal(1, dif[minh]);
        }
        [Fact]
        // order = 1
        public void CheckCoefsOrder1()
        {
            int minh = new Random().Next(-10, 10);
            FiniteDifference dif = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(1, minh);
            Assert.Equal(-1, dif[minh]);
            Assert.Equal(1, dif[minh + 1]);
        }

        [Fact]
        // order = 2
        public void CheckCoefsOrder2()
        {
            int minh = new Random().Next(-10, 10);
            FiniteDifference dif = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, minh);
            Assert.Equal(1, dif[minh]);
            Assert.Equal(-2, dif[minh + 1]);
            Assert.Equal(1, dif[minh + 2]);
        }

        [Fact]
        // order = 5
        public void CheckCoefsOrder5()
        {
            int minh = new Random().Next(-10, 10);
            FiniteDifference dif = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(5, minh);
            Assert.Equal(-1, dif[minh]);
            Assert.Equal(5, dif[minh + 1]);
            Assert.Equal(-10, dif[minh + 2]);
            Assert.Equal(10, dif[minh + 3]);
            Assert.Equal(-5, dif[minh + 4]);
            Assert.Equal(1, dif[minh + 5]);
        }

        #endregion
    }
}
