using System;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetApp
{
    internal class ProgramLoop
    {
        public void Run()
        {
            var operations = new Operations();
            ;
            Console.WriteLine("Type numbers separated by char | and ; as end.");
          //  operations.GetDataFromUser();
            Console.WriteLine("Expression:");
            //  var exp = Console.ReadLine();
            //  operations.GetDataToExpression(exp);
            Data.Numbers
                .Add(new List<double>
                {
                    11,12,13,14,15
                });
            Data.Numbers
                .Add(new List<double>
                {
                    21,22,23,24,25
                });

            var expression = "A1 + A2 * (A3 +B3)";
            var numbersExpression = operations.GetDataToExpression(expression);
            operations.ConvertExpresionToRpn("((2+7)/3+(14-3)*4)/2");
        }
       

       
    }

}
