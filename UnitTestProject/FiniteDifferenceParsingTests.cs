using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DifferenceEquationOrder;
using Xunit;

namespace UnitTestProject
{
    public class FiniteDifferenceParsingTests
    {
        #region Argument check

        [Fact]
        // check simple u(x)
        public void NodNoH()
        {
            FiniteDifference goodDifference = FiniteDifference.GetFiniteDifferenceByOrder(0);
            string s = "u(x)";
            Assert.True(FiniteDifference.Parse(s) == goodDifference);
        }

        [Theory] // check u(x+-nh)
        [InlineData(0)] // +0*
        [InlineData(1)] // +1*
        [InlineData(-1)] // -1*
        [InlineData(2)] // +2*
        [InlineData(-2)] // -2*
        public void NodWithWithSignH(int h)
        {
            FiniteDifference goodDifference = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, h);
            string s = $"u(x{h:+0;-0}*h)";
            Assert.True(FiniteDifference.Parse(s) == goodDifference);
        }

        [Fact]
        // check for u(x-h), u(x+h), u(x-0*h)
        public void NodWithHNoNumber()
        {
            // -0
            FiniteDifference goodDifference = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 0);
            string s = "u(x-0*h)";
            Assert.True(FiniteDifference.Parse(s) == goodDifference);

            // -h
            goodDifference = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, -1);
            s = "u(x-h)";
            Assert.True(FiniteDifference.Parse(s) == goodDifference);

            // +h
            goodDifference = FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 1);
            s = "u(x+h)";
            Assert.True(FiniteDifference.Parse(s) == goodDifference);
        }

        [Fact]
        // check error in argument
        public void ArgsErrors()
        {
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("u(x+242342342343*h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("u"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("u(x+h"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("ux(   h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("u(   h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("u(x   h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("u(x+*h"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("u(x -432fe3   h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("u(x -43 3   h)"));
            Assert.NotNull(FiniteDifference.Parse("u  (  x -4323 *  h )   "));
        }

        #endregion
    }
}
