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
        /// <exception cref="FormatException">Invalid format of a expression. Message contains invalid part</exception>
        /// <exception cref="OverflowException">Format is valid but expression contains too large coefficients</exception>
        /// <returns>FiniteDifference that contains result of the expression</returns>
        public static FiniteDifference Evaluate(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            // split expression for summands and evaluate each of them and sum them
            FiniteDifference result = null;
            foreach (var summand in GetSummands(s))
                try
                {
                    result += EvaluateSummand(summand);
                }
                // on exception - provide info about summand
                catch (FormatException e)
                { throw new FormatException(summand.Trim(), e); }
                catch (OverflowException e)
                { throw new OverflowException(summand.Trim(), e); }

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


        // handle one node - u or ku or k*u or dnu or
        private static FiniteDifference EvaluateSummand(string s)
        {
            // if no data - return nonFiniteDifference
            if (string.IsNullOrWhiteSpace(s))
                return null;

            // trim spaces, change to lower and change , to .
            s = s.Trim().ToLower().Replace(',', '.');

            // detect leading sign of the summand
            int sign = 1;
            if (s[0] == '-' || s[0] == '+')
            {
                if (s[0] == '-')
                    sign = -1;
                // remove sign
                s = s.Substring(1).TrimStart();
            }

            // count leading digits
            int coefficientLength = s.TakeWhile(ch => ch == '.' || char.IsDigit(ch)).Count();
            if (coefficientLength == s.Length) // if only coefficient, no FiniteDifference
                throw new FormatException("Input string was not in a correct format");

            if (coefficientLength == 0) // of there is no coefficient - parse FiniteDifference
                return sign * FiniteDifference.Parse(s);

            if (s[0] == '.' || s[coefficientLength - 1] == '.') // if coefficient starts or ends with . 
                throw new FormatException("Input string was not in a correct format");

            // there is multiplication

            // get coefficient
            string sCoefficient = s.Remove(coefficientLength);
            // if coefficient contains multiple .'s
            if (sCoefficient.Count(ch => ch == '.') > 1)
                throw new FormatException("Input string was not in a correct format");

            double k = double.Parse(sCoefficient);

            // get FiniteDifference and *(if exists)
            string rightPart = s.Substring(coefficientLength).TrimStart();
            // remove * is exists
            if (rightPart.StartsWith("*"))
                rightPart = rightPart.Substring(1).TrimStart();

            if (string.IsNullOrWhiteSpace(rightPart)) // if right part does not contain any data
                throw new FormatException("Input string was not in a correct format");

            return sign * k * FiniteDifference.Parse(rightPart);
        }

        // split expression string to collection of summands
        private static string[] GetSummands(string s) => Regex.Split(s, @"(?<=[)])");
    }
}
