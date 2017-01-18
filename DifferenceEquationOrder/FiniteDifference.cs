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
    }
}
