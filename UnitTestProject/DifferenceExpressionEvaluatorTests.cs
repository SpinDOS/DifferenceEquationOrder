using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DifferenceEquationOrder;
using Xunit;

namespace UnitTestProject
{
    // tests for difference expression evaluator
    public class DifferenceExpressionEvaluatorTests
    {
        [Fact]
        // test simple FiniteDifference parsing
        public void CheckSingleDifference()
        {
            string s = "d3u(x-h)";
            Assert.True(FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -1) == DifferenceExpressionEvaluator.Evaluate(s));
        }

        [Fact]
        // check multiplication
        public void CheckSingleDifferenceMultiplication()
        {
            string s = " -20.2 * d3u (x-2*h) ";
            Assert.True(-20.2 *FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -2) == DifferenceExpressionEvaluator.Evaluate(s));

            s = "  20,2d3u(x-2*h)";
            Assert.True(20.2 * FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -2) == DifferenceExpressionEvaluator.Evaluate(s));
        }

        [Fact]
        // check simple summation
        public void CheckSimpleSummation()
        {
            string s = "d3u(x-2*h) - du(x-h)";
            Assert.True(FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -2) -
                FiniteDifference.GetFiniteDifferenceByOrderAndMinH(1, -1)
                == DifferenceExpressionEvaluator.Evaluate(s));
        }

        [Fact]
        // check summation of multiple summands with coefficients
        public void CheckSummation()
        {
            string s = "  d3 u(x-2*h) - 2 * du(x-h) + u(x+2*h) - 1 * d^2 u(x)   ";
            Assert.True(FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -2)
                -2 * FiniteDifference.GetFiniteDifferenceByOrderAndMinH(1, -1)
                + FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 2)
                - FiniteDifference.GetFiniteDifferenceByOrder(2)
                == DifferenceExpressionEvaluator.Evaluate(s));

            s = "  - d^3u(x-2*h) - 2 * du(x-h) + u(x+2*h) - 1 * d^2 u(x) ";
            Assert.True(-FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -2)
                - 2 * FiniteDifference.GetFiniteDifferenceByOrderAndMinH(1, -1)
                + FiniteDifference.GetFiniteDifferenceByOrderAndMinH(0, 2)
                - FiniteDifference.GetFiniteDifferenceByOrder(2)
                == DifferenceExpressionEvaluator.Evaluate(s));
        }

        [Fact]
        // check whether the DifferenceExpressionEvaluator provide valid error info on big numbers
        public void CheckOverflowErrors()
        {
            // check arg overflow
            try
            {
                DifferenceExpressionEvaluator.Evaluate("-d^5u(x-242h)+3*du(x-42342341423h)");
                Assert.True(false); // report error here
            }
            catch (OverflowException e)
            {
                Assert.Equal("+3*du(x-42342341423h)", e.Message);
            }

            // check order overflow
            try
            {
                DifferenceExpressionEvaluator.Evaluate("-d^55646u(x-242h)+3*du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (OverflowException e)
            {
                Assert.Equal("-d^55646u(x-242h)", e.Message);
            }
        }

        [Fact]
        // check whether the DifferenceExpressionEvaluator provide valid error info on multiplication
        public void CheckMultiplicationErrors()
        {
            // check multiplication errors
            try
            {
                DifferenceExpressionEvaluator.Evaluate("1 1d^5u(x-242h)+3*du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("1 1d^5u(x-242h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("1d^5u(x-242h)+*du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("+*du(x-4223h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("1   d^5u(x-242h)+2,3.3 *  d u(x-4223h)   ");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("+2,3.3 *  d u(x-4223h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("-.d^5u(x-242h)+2,3.3 *  d u(x-4223h)   ");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("-.d^5u(x-242h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("-.2d^5u(x-242h)+2,3.3 *  d u(x-4223h)   ");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("-.2d^5u(x-242h)", e.Message);
            }
            try
            {
                DifferenceExpressionEvaluator.Evaluate("-2.d^5u(x-242h)+2,3.3 *  d u(x-4223h)   ");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("-2.d^5u(x-242h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("1   d^5u(x-242h)+2,3. *du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("+2,3. *du(x-4223h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("1   d^5u(x-242h)+2*2*du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("+2*2*du(x-4223h)", e.Message);
            }
        }

        [Fact]
        // check whether the DifferenceExpressionEvaluator provide valid error info on summation
        public void CheckSummationErrors()
        {
            // check summation errors
            try
            {
                DifferenceExpressionEvaluator.Evaluate("++1d^5u(x-242h)+3*du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("++1d^5u(x-242h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("1d^5u(x-242h)+-du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("+-du(x-4223h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("+1   d^5u(x-242h)+2+2du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("+2+2du(x-4223h)", e.Message);
            }

            try
            {
                DifferenceExpressionEvaluator.Evaluate("1   d^5u(x-24+2)2h)+2*du(x-4223h)");
                Assert.True(false); // report error here
            }
            catch (FormatException e)
            {
                Assert.Equal("1   d^5u(x-24+2)", e.Message);
            }
        }

    }
}
