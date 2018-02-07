using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetApp
{
    public class Operator
    {
        public string Symbol { get; set; }
        public int Precedence { get; set; }

        public bool RightAssociative { get; set; }
    }
}
