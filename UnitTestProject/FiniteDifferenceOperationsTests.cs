using System;
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
            // REMOVED AS FiniteDifference indexer changed to read-only
            // *****************************************************************************************************
            ////////// change coefficient
            ////////l[0] = 2;
            ////////Assert.False(l == r);
            ////////// revert coefficient
            ////////l[0] = 1;
            ////////Assert.True(l == r);
            ////////// create FiniteDifference with different order
            ////////r = FiniteDifference.GetFiniteDifferenceByOrder(3);
            ////////Assert.False(l == r);
            ////////// change right's coefficients to match left's
            ////////r[0] = 1;
            ////////r[1] = -2;
            ////////r[2] = 1;
            ////////Assert.False(l == r);
            // *****************************************************************************************************


            // create FiniteDifference with the same order but another minimumH
            r = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 1);
            Assert.False(l == r);
            // create same FiniteDifference
            l = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 1);
            Assert.True(l == r);
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(1, 7)]
        [InlineData(2, 1)]
        [InlineData(5, -7)]
        // check multiplication
        public void CheckMultiply(int order, int h)
        {
            // get first FiniteDifference
            var dif = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(order, h);
            double k = 1.0 / new Random().Next(2, 5000);
            // et multiplied FiniteDifference
            var multiplied = k * dif;
            Assert.Equal(dif.MinimumH, multiplied.MinimumH);
            for (int i = 0; i <= order; i++)
                Assert.Equal(k * dif[h + i], multiplied[h + i]);
        }

        [Fact]
        // check operator - (unary)
        public void CheckNegatiation()
        {
            // get random order and h
            int order = new Random().Next(10);
            int h = new Random().Next(-10, 10);
            // create FiniteDifference and its negatioaton
            FiniteDifference difference = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(order, h);
            FiniteDifference negatioation = -difference;
            // check MinimumH and coefficients
            Assert.Equal(difference.MinimumH, negatioation.MinimumH);
            for (int i = 0; i <= order; i++)
                Assert.Equal(difference[h + i], -(negatioation[h + i]));
        }
        [Fact]
        // check summmation without crossing operands
        public void NoCrossingNoSpaceSummand()
        {
            // get two non crossing FiniteDifferences without space
            FiniteDifference l = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(1, -1);
            FiniteDifference r = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 1);
            // sum
            var sum = l + r;
            var sum2 = r + l;
            // check sum
            Assert.True(sum == sum2);
            Assert.Equal(4, sum.Order);

            Assert.Equal(-1, sum[-1]);
            Assert.Equal(1, sum[0]);
            Assert.Equal(1, sum[1]);
            Assert.Equal(-2, sum[2]);
            Assert.Equal(1, sum[3]);
        }

        [Fact]
        // check summmation with crossing operands
        public void CrossingSummands()
        {
            // get two crossing FiniteDifferences
            FiniteDifference l = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -1);
            FiniteDifference r = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 1);
            // sum
            var sum = l + r;
            // check sum
            Assert.Equal(4, sum.Order);

            Assert.Equal(-1, sum[-1]);
            Assert.Equal(3, sum[0]);
            Assert.Equal(-3+1, sum[1]);
            Assert.Equal(1-2, sum[2]);
            Assert.Equal(1, sum[3]);
        }

        [Fact]
        // check summmation with crossing operands
        public void WithSpacingSummands()
        {
            // get two FiniteDifferences with space between them
            FiniteDifference l = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -4);
            FiniteDifference r = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(2, 1);
            // sum
            var sum = l + r;
            // check sum
            Assert.Equal(7, sum.Order);

            Assert.Equal(-1, sum[-4]);
            Assert.Equal(3, sum[-3]);
            Assert.Equal(-3, sum[-2]);
            Assert.Equal(1, sum[-1]);
            Assert.Equal(0, sum[0]);
            Assert.Equal(1, sum[1]);
            Assert.Equal(-2, sum[2]);
            Assert.Equal(1, sum[3]);
        }
    }
}
