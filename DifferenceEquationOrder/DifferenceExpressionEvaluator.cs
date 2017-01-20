using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// <exception cref="ArgumentNullException">s does not contain any data</exception>
        /// <exception cref="FormatException">Invalid format of a expression. Message contains invalid part</exception>
        /// <exception cref="OverflowException">Format is valid but expression contains too large coefficients</exception>
        /// <returns>FiniteDifference that contains result of the expression</returns>
        public static FiniteDifference Evaluate(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentNullException(nameof(s));

            s = s.Trim();
            // prepare leading sign for first summand
            if (s[0] != '-' && s[0] != '+')
                s = "+" + s;

            FiniteDifference result = null;
            foreach (var summand in GetSummands(s))
                try
                {
                    result += EvaluateSummand(summand);
                }
                // on exception - provide info about summand
                catch (FormatException)
                { throw new FormatException(summand); }
                catch (OverflowException)
                { throw new OverflowException(summand); }

            return result;
        }

        /// <summary>
        /// Evaluate an expression of FiniteDifferences
        /// A return value indicated whether the evaluation succeeded
        /// </summary>
        /// <param name="s">A string containing an expression to evaluate</param>
        /// <param name="result">When this method returns, contains the 
        /// FiniteDifference equivalent of the result of the evaluation of the expression contained in s, 
        /// if the evaluation succeeded, or null if s does not contain valid expression</param>
        public static bool TryEvaluate(string s, out FiniteDifference result)
        {
            result = null;
            try
            {
                result = Evaluate(s);
                return true;
            }
            catch (Exception e) when (e is NullReferenceException || e is FormatException || e is OverflowException)
            { return false; }
        }


        // handle one node - dif or k*dif
        private static FiniteDifference EvaluateSummand(string s)
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

            int iU = s.IndexOf('u');
            if (iU < 0) // if no FiniteDifference at all
                throw new FormatException();

            int iMultiplication = s.IndexOf('*');
            // if (no *) or (* is u(x+n*h))
            if (iMultiplication < 0 || iMultiplication > iU) 
                return sign * FiniteDifference.Parse(s);

            // there is multiplication

            // get coefficient and FiniteDifference strings
            string left = s.Remove(iMultiplication).TrimEnd(),
                right = s.Substring(iMultiplication + 1).TrimStart();
            
            // return multiplication
            return sign * double.Parse(left) * FiniteDifference.Parse(right);
        }

        // split expression string to collection of summands
        private static string[] GetSummands(string s) => Regex.Split(s, @"(?<=[)])");
    }
}
