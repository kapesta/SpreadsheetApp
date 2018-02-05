using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    class ProgramLoop
    {
        public void Run()
        {
            Console.WriteLine("Type numbers separated by char | and ; as end.");
            GetDataFromUser();
            Console.WriteLine("Expression:");
            var exp = Console.ReadLine();
            GetDataToExpression(exp);
        }
        private void GetDataFromUser()
        {  
            string inputString;
            bool isEnd;
            do
            {
                inputString = Console.ReadLine();
                isEnd = inputString.Contains(";");
                if (isEnd)
                {
                    inputString = inputString.Remove(inputString.IndexOf(';'));
                }

                if (inputString.LastIndexOf('|') == inputString.Length - 1)
                {
                    inputString = inputString.Remove(inputString.Length - 1);
                }
                var separatedString = inputString.Split('|');

                AddNumbersToData(separatedString);
                
            } while (!isEnd);
        }

        private void AddNumbersToData(string[] separatedString)
        {
            var listOfDoubles = new List<double>();
            for (int i = 0; i < separatedString.Length; i++)
            {
                listOfDoubles.Add(double.Parse(separatedString[i]));
            }
            Data.Numbers.Add(listOfDoubles);
        }

        private List<double> GetDataToExpression(string expression)
        {
            var dataList = expression.Split(new char[] { '+', '*', '-', '/' });
            var result = new List<double>();

            for (int i = 0; i < dataList.Length; i++)
            {
                var argument = dataList[i].ToUpper();
                int intArgument = (int)argument[0] - 65;
                var row = Data.Numbers[intArgument];
                var number = row[int.Parse(argument.Substring(1))-1];
                result.Add(number);
            }
            return result;
        }
    }

}
