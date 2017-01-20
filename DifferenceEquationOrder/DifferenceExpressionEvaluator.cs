using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DifferenceEquationOrder
{
    /// <summary>
    /// Class to parse difference expression
    /// </summary>
    public static class DifferenceExpressionEvaluator
    {
        /// <summary>
        /// Evaluate an expression of FiniteDifferences
        /// </summary>
        /// <param name="s">A string containing an expression to evaluate</param>
        /// <exception cref="FormatException">Invalid format of a expression. Message contains invalid part</exception>
        /// <exception cref="OverflowException">Format is valid but expression contains too large coefficients</exception>
        /// <returns>FiniteDifference that contains result of the expression</returns>
        public static FiniteDifference Evaluate(string s)
        {
            if (s[0] != '-')
                s = "+" + s;
            return HandleSummand(s);
        }


        // handle one node - dif or k*dif
        private static FiniteDifference HandleSummand(string s)
        {
            // if no data - return nonFiniteDifference
            if (string.IsNullOrWhiteSpace(s))
                return null;

            // trim spaces
            s = s.Trim().ToLower();

            // detect leading sign of the summand
            int sign = s[0] == '-' ? -1: 1;

            // remove sign
            s = s.Substring(1).TrimStart();

            int iU = s.IndexOf('u'), iMultiplication = s.IndexOf('*');

            if (iU < 0) // if no FiniteDifference at all
                throw new FormatException();
            // if no * or * is u(x+n*h)
            if (iMultiplication < 0 || iMultiplication > iU) 
                return sign * FiniteDifference.Parse(s);

            // there is multiplication

            // if * is first or last char
            if (iMultiplication == 0 || iMultiplication == s.Length - 1)
                throw new FormatException();

            // get coefficient and FiniteDifference strings
            string left = s.Remove(iMultiplication).TrimEnd(),
                right = s.Substring(iMultiplication + 1).TrimStart();
            
            // return multiplication
            return sign * double.Parse(left) * FiniteDifference.Parse(right);
        }
    }
}
