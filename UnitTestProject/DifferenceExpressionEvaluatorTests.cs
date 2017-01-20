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
            string s = "-20.2 * d3u(x-2*h)";
            Assert.True(-20.2 *FiniteDifference.GetFiniteDifferenceByOrderAndMinH(3, -2) == DifferenceExpressionEvaluator.Evaluate(s));
        }
    }
}
