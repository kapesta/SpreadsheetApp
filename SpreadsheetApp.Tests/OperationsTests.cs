using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpreadsheetApp.Tests
{
    [TestClass]
    public class OperationsTests
    {
        [TestMethod]
        public void ConvertExpresionToRpn_ValidExpression_ValidResult()
        {
            var input = "((2+7)/3+(14-3)*4)/2";
            var expectedResult = new List<string> {"2", "7", "+", "3", "/", "14", "3", "-", "4", "*", "+","2", "/"};

            var operations = new Operations();

            var result = operations.ConvertExpresionToRpn(input);

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expectedResult[i], result[i]);
            }

        }

        [TestMethod]
        public void GetDataToExpression_ValidData_ValidResult()
        {
            Data.Numbers
                .Add(new List<string>
                        {
                            "11","12","13","14","15"
                        });
            Data.Numbers
                .Add(new List<string>
                {
                    "21","22","23","24","25"
                });

            var firstExpression = "(A1+A3)*B3/B5";
            var secondExpression = "(A1 + A3) * B3 / B5";

            var operations = new Operations();

            var firstResult = operations.GetDataToExpression(firstExpression);
            var secondResult = operations.GetDataToExpression(secondExpression);

            Assert.AreEqual("(11+13)*23/25", firstResult);
            Assert.AreEqual("(11 + 13) * 23 / 25", secondResult);
        }

        [TestMethod]
        public void CalculateRpn_ValidInput_ValidResult()
        {
            var input = new List<string>
            {
                "12",
                "2",
                "3",
                "4",
                "*",
                "10",
                "5",
                "/",
                "+",
                "*",
                "+"
            };
            var expectedResult = 40.0d;


            var operations = new Operations();
            var result = operations.CalculateRpn(input);

            Assert.AreEqual(expectedResult,result);
        }
    }

    
}
