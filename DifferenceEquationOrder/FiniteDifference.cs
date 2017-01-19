using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DifferenceEquationOrder
{
    /// <summary>
    /// Represent finite difference d^(order)u[x+minH] = a0*u(x+minH)+a1*u(x+minH+h)+...+an*u(x+maxH)
    /// </summary>
    public sealed class FiniteDifference
    {
        #region Private members

        // array of coefficients of the difference
        private double[] _coefficients;

        // block creating custom FiniteDifference
        private FiniteDifference() { }

        #endregion

        #region Public properties

        /// <summary>
        /// Count of h in the first summand
        /// </summary>
        public int MinimumH { get; private set; }

        /// <summary>
        /// Count of h in the last summand
        /// </summary>
        public int MaximumH { get; private set; }

        /// <summary>
        /// Order of the finite difference
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Coefficient of the function of (x+n*h)
        /// </summary>
        /// <param name="h">Number of h to find coefficient for</param>
        public double this[int h]
        { // just access _coefficients with offset index
            get { return _coefficients[h - MinimumH]; }
            set { _coefficients[h - MinimumH] = value; }
        }

        #endregion

        #region Static creating FiniteDifference

        /// <summary>
        /// Create finite difference d^(order) u(x)
        /// </summary>
        /// <param name="order">Order of the finite difference</param>
        /// <returns></returns>
        public static FiniteDifference GetFiniteDifferenceByOrder(int order)
            => GetFiniteDifferenceByOrderAndMinH(order, 0);

        /// <summary>
        /// Create finite difference d^(order) u(x + minH*h)
        /// </summary>
        /// <param name="order">Order of the finite difference</param>
        /// <param name="minH">Count of h of the finite difference</param>
        /// <returns></returns>
        public static FiniteDifference GetFiniteDifferenceByOrderAndMinH(int order, int minH)
        {
            // check parameters
            if (order < 0)
                return null;
            // set up FiniteDifference
            FiniteDifference result = new FiniteDifference();
            result.Order = order;
            result.MinimumH = minH;
            result.MaximumH = minH + order;
            // get coefficients from PascalTriangle
            result._coefficients = PascalTriangle.GetCoefficients(order).Select((x, i) =>
            { // cast uing to double and make non-even numbers negative
                if (i % 2 == 0)
                    return (double) x;
                return -(double) x;
            }).Reverse().ToArray();
            return result;
        }

        #endregion

        #region Operations overload

        public static bool operator ==(FiniteDifference left, FiniteDifference right)
        {
            // if both are null or the same object
            if (ReferenceEquals(left, right))
                return true;
            // if only one is null
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            // if both are not null

            // check order and minimumH
            if (left.Order != right.Order || left.MinimumH != right.MinimumH)
                return false;
            // check coefficients
            return left._coefficients.SequenceEqual(right._coefficients);
        }

        public static bool operator !=(FiniteDifference left, FiniteDifference right) => !(left == right);

        public static FiniteDifference operator *(double k, FiniteDifference difference)
        {
            // on multiplication by zero or not FiniteDifference - not FiniteDifference
            if (ReferenceEquals(difference, null) || k.IsZero())
                return null;
            // copy difference
            FiniteDifference result = new FiniteDifference();
            result.MinimumH = difference.MinimumH;
            result.MaximumH = difference.MaximumH;
            result.Order = difference.Order;
            // copy coefficients multiplied on k
            result._coefficients = new double[difference._coefficients.Length];
            for (int i = 0; i < result._coefficients.Length; i++)
                result._coefficients[i] = k * difference._coefficients[i];
            return result;
        }
        public static FiniteDifference operator *(FiniteDifference difference, double k) => k * difference;
        public static FiniteDifference operator / (FiniteDifference difference, double k) => (1/k) * difference;

        public static FiniteDifference operator -(FiniteDifference difference) => -1 * difference;
        public static FiniteDifference operator +(FiniteDifference difference) => difference;

        public static FiniteDifference operator +(FiniteDifference left, FiniteDifference right)
        {
            // if one of the operands is null - return another
            if (ReferenceEquals(left, null))
                return right;
            if (ReferenceEquals(right, null))
                return left;
            // if both are not null

            // create FiniteDifference with wide range of h
            FiniteDifference result = new FiniteDifference();
            result.MinimumH = Math.Min(left.MinimumH, right.MinimumH);
            result.MaximumH = Math.Max(left.MaximumH, right.MaximumH);
            // create array for coefficients
            result._coefficients = new double[result.MaximumH - result.MinimumH + 1];

            // copy values from operands
            for (int i = left.MinimumH; i <= left.MaximumH; i++)
                result[i] += left[i];
            for (int i = right.MinimumH; i <= right.MaximumH; i++)
                result[i] += right[i];
            
            // cut order if got zero coefficients

            // move left divider until non-zero or end of array
            int l = result.MinimumH;
            while (result[l].IsZero())
            {
                // if all coefficients are zeros
                if (l == result.MaximumH)
                    return null;
                else
                    l++;
            }

            // move right divider until non-zero
            int r = result.MaximumH;
            while (result[r].IsZero())
                r--;

            // change coefficients array
            double[] newCoefficients = new double[r - l + 1];
            Array.Copy(result._coefficients, l-result.MinimumH, newCoefficients, 0, newCoefficients.Length);
            result._coefficients = newCoefficients;

            // save result parameters
            result.MinimumH = l;
            result.MaximumH = r;
            result.Order = r - l;

            return result;
        }

        public static FiniteDifference operator -(FiniteDifference left, FiniteDifference right) => left + (-right);

        #endregion

        #region Object methods override

        public override int GetHashCode() => MinimumH ^ _coefficients.GetHashCode();

        public override bool Equals(object obj)
        {
            // check if the object is FiniteDifference
            var difference = obj as FiniteDifference;
            if (difference == null)
                return false;
            return this == difference;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            // handle every h section with coefficient starting from maximumH
            for (int i = MaximumH; i >= MinimumH; i--)
            {
                // set up coefficient formatting
                double k = this[i];
                if (k.IsZero()) // 0
                    continue;
                else if ((k - 1).IsZero()) // 1
                    result.Append("+u");
                else if ((k+1).IsZero()) // -1
                    result.Append("-u");
                else // +-2
                    result.Append($"{k:+#;-#}*u");

                // set up h formatting
                if (i == 0) // 0
                    result.Append("(x)");
                else if (i == 1) // 1
                    result.Append("(x+h)");
                else if (i == -1) // -1
                    result.Append("(x-h)");
                else // +-2
                    result.Append($"(x{i:+#;-#}*h)");
            }

            // remove starting +
            if (result[0] == '+')
                result.Remove(0, 1);
            return result.ToString();
        }

        #endregion

        #region String parsing

        /// <summary>
        /// Convert a string representation of a FiniteDifference
        /// </summary>
        /// <param name="s">A string containing a FiniteDifference to convert</param>
        /// <exception cref="FormatException">Invalid format of a FiniteDifference</exception>
        public static FiniteDifference Parse(string s)
        {
            FiniteDifference result;
            if (!TryParse(s, out result))
                throw new FormatException("Input string was not in a correct format");
            return result;
        }

        /// <summary>
        /// Convert a string representation of a FiniteDifference. 
        /// A return value indicated whether the conversion succeeded
        /// </summary>
        /// <param name="s">A string containing a FiniteDifference to convert</param>
        /// <param name="result">When this method returns, contains the 
        /// FiniteDifference equivalent of the FiniteDifference contained in s, 
        /// if the conversion succeeded, or null if s does not contain valid FiniteDifference</param>
        public static bool TryParse(string s, out FiniteDifference result)
        {
            result = null;
            // if no data to parse - nonFiniteDifference object
            if (string.IsNullOrWhiteSpace(s))
                return true;

            int order = 0, h;
            string sArg = s.Substring(1); // remove u

            // parsing argumnet of u
            // **************************************************************************************************

            // check if sArg contains two separated numbers because they will be conctatenated after replace(" ", "")
            int i = 0;
            // find first number
            while (i < sArg.Length && !char.IsDigit(sArg[i]))
                i++;
            // skip first number
            while (i < sArg.Length && char.IsDigit(sArg[i]))
                i++;
            // check end of string
            while (i < sArg.Length)
                if (char.IsDigit(sArg[i++])) // if found second number
                    return false;

            sArg = sArg.Replace(" ", ""); // remove all spaces

            if (sArg == "(x)") // if h = 0 
                h = 0;
            else if (!sArg.StartsWith("(x") || !sArg.EndsWith("h)")) // if format error
                return false;
            else
            {
                string arg = sArg.Substring(2, sArg.Length - 4); // remove (x and h)
                // arg is + or - or +n or -n or +n* or -n*
                if (arg == "+")
                    h = 1;
                else if (arg == "-")
                    h = -1;
                else // arg is +n or -n or +n* or -n*
                {
                    if (arg.Length == 0 || (arg[0] != '+' && arg[0] != '-')) // check for (x2*h)
                        return false;
                    if (arg.EndsWith("*")) // remove last *
                        arg = arg.Remove(arg.Length - 1);
                    // arg is +n or -n
                    if (!int.TryParse(arg, out h)) // parse n to h
                        return false;
                }
            }



            result = GetFiniteDifferenceByOrderAndMinH(order, h);
            return true;
        }

        #endregion
    }
}
