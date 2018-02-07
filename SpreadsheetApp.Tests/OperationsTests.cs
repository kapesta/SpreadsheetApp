using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpreadsheetApp.Tests
{
    [TestClass]
    public class OperationsTests
    {
        [TestMethod]
        public void ConvertExpresionToRPN_ValidExpression_ValidResult()
        {
            var input = "(A1+B2*A2)/B3";
            var expectedResult = "A1 B2 A2 * + B3 /";

            var operations = new Operations();

            var result = operations.ConvertExpresionToRpn(input);

            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void GetDataToExpression_ValidData_ValidResult()
        {
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

            var firstExpression = "(A1+A3)*B3/B5";
            var secondExpression = "(A1 + A3) * B3 / B5";

            var operations = new Operations();

            var firstResult = operations.GetDataToExpression(firstExpression);
            var secondResult = operations.GetDataToExpression(secondExpression);

            Assert.AreEqual("(11+13)*23/25", firstResult);
            Assert.AreEqual("(11 + 13) * 23 / 25", secondResult);
        }
    }

    
}
