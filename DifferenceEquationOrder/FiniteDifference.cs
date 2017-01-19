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
                throw new ArgumentException("Finite difference order can not be less than zero");
            // set up FiniteDifference
            FiniteDifference result = new FiniteDifference();
            result.Order = order;
            result.MinimumH = minH;
            result.MaximumH = minH + order;
            // get coefficients from PascalTriangle
            result._coefficients = PascalTriangle.GetCoefficients(order).Select((x, i) =>
            { // cast uing to double and make even numbers negative
                if (i % 2 == 0)
                    return (double) x;
                return -(double) x;
            }).ToArray();
            return result;
        }

        #endregion

        #region Operations overload

        public static bool operator ==(FiniteDifference left, FiniteDifference right)
        {
            // if argument if null
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                throw new NullReferenceException("Comparing null object");
            // check order and minimumH
            if (left.Order != right.Order || left.MinimumH != right.MinimumH)
                return false;
            // check coefficients
            return left._coefficients.SequenceEqual(right._coefficients);
        }

        public static bool operator !=(FiniteDifference left, FiniteDifference right) => !(left == right);
        public override bool Equals(object obj)
        {
            // check if the object is FiniteDifference
            var difference = obj as FiniteDifference;
            if (difference == null)
                return false;
            return this == difference;
        }
        public override int GetHashCode() => MinimumH ^ _coefficients.GetHashCode();

        public static FiniteDifference operator *(double k, FiniteDifference difference)
        {
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
                if (l == result.MaximumH)
                    throw new ArgumentException("Sum is not FiniteDifference");
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
    }
}
