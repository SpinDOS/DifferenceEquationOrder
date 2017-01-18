using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DifferenceEquationOrder
{
    /// <summary>
    /// Class for finding binomial coefficients using pascal triangle
    /// </summary>
    public static class PascalTriangle
    {
        // list of rows of the triangle
        private static readonly List<uint[]> Coefficients = new List<uint[]>() {new uint[] {1}};

        /// <summary>
        /// Get binomial coefficients of some order
        /// </summary>
        /// <param name="order">Order to find its binomial coefficients</param>
        /// <returns>Array of coefficients</returns>
        public static uint[] GetCoefficients(int order)
        {
            // check parameter
            if (order < 0)
                throw new ArgumentException("Triangle order can not be less than zero");
            // if the array is already calculated
            if (Coefficients.Count > order)
                return Coefficients[order];
            // not found array - need to calculate

            // get or calculate previous row
            uint[] prev = GetCoefficients(order - 1);
            // length of previous row
            int length = prev.Length;
            // create new array to store binomial coefficients
            uint[] cur = new uint[length + 1];
            // set first and last coefficients to 1
            cur[0] = cur[length] = 1;
            // calculate central coefficients
            for (int i = 1; i < length; i++)
                cur[i] = prev[i - 1] + prev[i];
            // save to list of rows and return
            Coefficients.Add(cur);
            return cur;
        }
    }
}
