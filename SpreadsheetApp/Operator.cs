namespace SpreadsheetApp
{
    public class Operator
    {
        public string Symbol { get; set; }
        public int Precedence { get; set; }
        public bool RightAssociative { get; set; }
    }
}
