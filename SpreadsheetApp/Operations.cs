using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

                AddNumbersToData(separatedString);

            } while (!isEnd);
        }

        private void AddNumbersToData(IEnumerable<string> separatedString)
        {
            var listOfDoubles = separatedString.Select(double.Parse).ToList();
            Data.Numbers.Add(listOfDoubles);
        }

        public string GetDataToExpression(string expression)
        {
            var regex = new Regex("[A-Za-z]\\d+");
            var matches = regex.Matches(expression);

            foreach (Match match in matches)
            {
                var stringBuilder = new StringBuilder(expression);

                var rowMarkerAsInt = match.Value[0] - 65;
                var column = int.Parse(match.Value.Substring(1)) - 1;
                var row = Data.Numbers[rowMarkerAsInt];
                var number = row[column];

                stringBuilder.Remove(match.Index, match.Length);
                stringBuilder.Insert(match.Index, number);

                expression = stringBuilder.ToString();
            }

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
    }
}
