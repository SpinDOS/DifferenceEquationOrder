using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DifferenceEquationOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            // if the program called from the console - handle console arguments
            if (args.Length > 0)
            {
                foreach (var expression in args)
                {
                    // write expression to handle and handle it
                    Console.WriteLine(expression + ": ");
                    HandleExpression(expression);
                }
                // system.pause
                Console.Write("Press any key to continue...");
                Console.ReadKey(true);
                return;
            }
            // write how to use
            Console.WriteLine("Usage: -d^2u(x-2h)+3*du(x+h)");
            // else - read expressions from the console
            while (true)
            {
                Console.WriteLine("Enter expression to evaluate (hit enter to exit): ");
                // read expression
                string expression = Console.ReadLine();
                // quit if nothing was read
                if (string.IsNullOrWhiteSpace(expression))
                    return;
                // handle the expression
                HandleExpression(expression);
            }
        }

        // evaluate expression and write result, if succeeded or write error info
        private static void HandleExpression(string expression)
        {
            try
            {
                // try evaluate
                var result = DifferenceExpressionEvaluator.Evaluate(expression);
                // write result info
                if (result == null)
                    Console.WriteLine("The expression is equavalent to zero");
                else
                {
                    Console.WriteLine("Resulting finite difference is: " + result.ToString());
                    Console.WriteLine("Order of the difference equation is " + result.Order);
                }
            }
            // write info about error 
            catch (OverflowException e)
            {
                Console.WriteLine("Too large coefficients are used in the fragment: " + e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Invalid format of the fragment: " + e.Message);
            }
        }
    }
}
