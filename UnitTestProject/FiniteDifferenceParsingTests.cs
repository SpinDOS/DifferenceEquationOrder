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
            Assert.Throws<OverflowException>(() => FiniteDifference.Parse("u(x+242342342343*h)"));
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

        #region Order check

        [Fact]
        // check parsing simple du
        public void Order1CheckWithoutH()
        {
            FiniteDifference goodDifference = FiniteDifference.GetFiniteDifferenceByOrder(1);
            string s = "du(x)";
            Assert.True(FiniteDifference.Parse(s) == goodDifference);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(11)]
        // check parsing d^nu and dnu
        public void OrderCheckWithoutH(int order)
        {
            FiniteDifference goodDifference = FiniteDifference.GetFiniteDifferenceByOrder(order);
            // check d^nu
            string s = $"d^{order}u(x)";
            Assert.True(FiniteDifference.Parse(s) == goodDifference);
            // check dnu
            s = $"d{order}u(x)";
            Assert.True(FiniteDifference.Parse(s) == goodDifference);
        }

        [Fact]
        // check if order parsing throws exceptions on errors
        public void OrderErrors()
        {
            Assert.Throws<OverflowException>(() => FiniteDifference.Parse("d^43245u(x)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("du"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("ddu(x)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse(" d^u(x)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("d^^2u(x)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse(" d^  2 2u(x)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse(" d^  - 2u(x)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("ud(x"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("^d2u(x)"));
            Assert.Null(FiniteDifference.Parse("  d ^  -2  u  (  x -4323 *  h )   "));
            Assert.NotNull(FiniteDifference.Parse("  d  32  u  (  x -4323 *  h )   "));
        }

        #endregion

        [Fact]
        // integration test of parsing to check correctness of all parts of parsing
        public void OrderAndArgChecks()
        {
            // check different orders with not null arg
            Assert.Equal(FiniteDifference.Parse(" d  ^ 21 u  ( x - 3h)"), 
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(21, -3));
            Assert.Equal(FiniteDifference.Parse(" d   21 u  ( x - 3h)"),
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(21, -3));

            Assert.Equal(FiniteDifference.Parse(" d u  ( x - 3h)"),
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(1, -3));

            Assert.Equal(FiniteDifference.Parse(" u  ( x - 3h)"),
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, -3));

            Assert.Equal(FiniteDifference.Parse(" d  ^ -21 u  ( x - 3h)"), null);
            Assert.Equal(FiniteDifference.Parse(" d  -21 u  ( x - 3h)"), null);

            // check different args with not null orders

            Assert.Equal(FiniteDifference.Parse(" d  ^ 21 u  ( x - h)"),
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(21, -1));

            Assert.Equal(FiniteDifference.Parse(" d   u  ( x + h )  "),
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(1, 1));

            Assert.Equal(FiniteDifference.Parse(" d  ^ 21 u  ( x + 24 h)"),
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(21, 24));

            // check u absence error
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("d2(x+2h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("d(x+2h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("(x+2h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("(x)"));

            // check args errors with good nont null orders

            Assert.Throws<OverflowException>(() => FiniteDifference.Parse(" d^  5u(x + 24243243242432 h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("d2u(x 2h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse(" d 2u(x + 2 2h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("d^  11 u(x - - 2h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("d  2u(xh)"));

            // check order errors with good nont null args
            Assert.Throws<OverflowException>(() => FiniteDifference.Parse(" d  513244u(x+23h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("ddu(x  -21h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("2u(x+h)"));
            Assert.Throws<FormatException>(() => FiniteDifference.Parse("^1u(x-h)"));

        }
    }
}
