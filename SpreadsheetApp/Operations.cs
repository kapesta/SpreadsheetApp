using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetApp
{
    public class Operations
    {
        public void GetDataFromUser()
        {
            bool isEnd;
            do
            {
                var inputString = Console.ReadLine();
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
                var noCommaList = new List<string>();
                separatedString.ToList().ForEach(s => noCommaList.Add(s.Replace(",",".")));
                

                AddNumbersToData(noCommaList);

            } while (!isEnd);
        }

        private void AddNumbersToData(IEnumerable<string> separatedString)
        {
            Data.Numbers.Add(separatedString.ToList());
        }

        public string GetDataToExpression(string expression)
        {
            var regex = new Regex("[A-Za-z]\\d+");
            var match = regex.Match(expression);

            while (match.Success)
            {
                var stringBuilder = new StringBuilder(expression);

                var rowMarkerAsInt = match.Value[0] - 65;
                var column = int.Parse(match.Value.Substring(1)) - 1;
                var row = Data.Numbers[rowMarkerAsInt];
                var number = row[column];

                stringBuilder.Remove(match.Index, match.Length);
                stringBuilder.Insert(match.Index, number);

                expression = stringBuilder.ToString();
                match = regex.Match(expression);
            }
            //foreach (Match match in matches)
            //{
            //    var stringBuilder = new StringBuilder(expression);

            //    var rowMarkerAsInt = match.Value[0] - 65;
            //    var column = int.Parse(match.Value.Substring(1)) - 1;
            //    var row = Data.Numbers[rowMarkerAsInt];
            //    var number = row[column];
                
            //    stringBuilder.Remove(match.Index, match.Length);
            //    stringBuilder.Insert(match.Index, number);

            //    expression = stringBuilder.ToString();
            //}

            return expression;
        }

        public List<string> ConvertExpresionToRpn(string expression)
        {
           expression = new string(expression.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());

            var regex = new Regex("([-, +, *,/,^, (, ')'])");
            var tokens = regex.Split(expression);
            tokens = tokens.Where(x => x != "").ToArray();

            var output = new List<string>();
            var stack = new Stack<string>();

            var operators = new List<Operator>
            {
                new Operator { Symbol = "^",Precedence = 4,RightAssociative = true},
                new Operator { Symbol = "*",Precedence = 3,RightAssociative = false},
                new Operator { Symbol = "/",Precedence = 3,RightAssociative = false},
                new Operator { Symbol = "+",Precedence = 2,RightAssociative = false},
                new Operator { Symbol = "-",Precedence = 2,RightAssociative = false},
            };

            var operatorRegex = new Regex("([-,+,*,/,^])");
            foreach (var token in tokens)
            {
                if (!regex.IsMatch(token))
                {
                    output.Add(token);
                }
                else if (operatorRegex.IsMatch(token))
                {
                    var currentToken = operators.SingleOrDefault(x => x.Symbol == token);

                    if (stack.Count > 0 && operators.Any(x => x.Symbol == stack.Peek()) )
                    {
                        var stackPeek = operators.SingleOrDefault(x => x.Symbol == stack.Peek());
                        while (stack.Count > 0 && 
                               (currentToken.Precedence < stackPeek.Precedence || (currentToken.Precedence == stackPeek.Precedence && !currentToken.RightAssociative) )
                               && stack.Peek() != "(" )
                        {
                            output.Add(stack.Pop());
                        }
                    }
                    stack.Push(token);
                }
                else if (token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        output.Add(stack.Pop());
                    }

                    if (stack.Count > 0)
                    {
                        stack.Pop();
                    }
                }
            }

            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }
            return output;
        }

        public double CalculateRpn(IEnumerable<string> inputData)
        {
            var stack = new Stack<string>();            
            var numberRegex = new Regex(@"(^\d+\.\d*|\.?\d+$)");
            foreach (var token in inputData)
            {
                if (numberRegex.IsMatch(token))
                {
                    stack.Push(token);
                }
                else
                {
                    if (stack.Count > 1)
                    {
                        var a = Convert.ToDouble(stack.Pop());
                        var b = Convert.ToDouble(stack.Pop());
                        double result = 0;
                        switch (token)
                        {
                            case "+":
                                result = a + b;
                                break;
                            case "-":
                                result = b - a;
                                break;
                            case "*":
                                result = a * b;
                                break;
                            case "/":
                                result = b / a;
                                break;
                            case "^":
                                result = Math.Pow(b, a);
                                break;
                        }
                        stack.Push(result.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }

            return Convert.ToDouble(stack.Pop());
        }

        public void ConvertDataExpressionsToNumbers()
        {
            for (var i = 0; i < Data.Numbers.Count; i++)
            {
                for (var j = 0; j < Data.Numbers[i].Count; j++)
                {
                    var stringNumber = Data.Numbers[i][j].ToString();
                    var numberRegex = new Regex("[A-z]");
                    // var numberRegex = new Regex(@"(^\d+\.\d*|\.?\d+$)");
                    if (numberRegex.IsMatch(stringNumber))
                    {
                        var numbersExpression = GetDataToExpression(stringNumber);
                        var rpnTable = ConvertExpresionToRpn(numbersExpression);
                        var result = CalculateRpn(rpnTable);
                        Data.Numbers[i][j] = result.ToString();
                    }
                }
            }
        }
    }

}
