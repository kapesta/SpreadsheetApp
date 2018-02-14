using System;
using System.Text.RegularExpressions;

namespace SpreadsheetApp
{
    internal class ProgramLoop
    {
        public void Run()
        {
            var operations = new Operations();
            
            Console.WriteLine("Type numbers separated by char | and ; as end.");
            operations.GetDataFromUser();
            Console.WriteLine("Expression:");
            var exp = Console.ReadLine();


            operations.ConvertDataExpressionsToNumbers();

            var numbersExpression = operations.GetDataToExpression(exp);
            var rpnTable = operations.ConvertExpresionToRpn(numbersExpression);
            var result = operations.CalculateRpn(rpnTable);
            Console.WriteLine($"Result: {result}");
            Console.ReadLine();
        }
    }

}
