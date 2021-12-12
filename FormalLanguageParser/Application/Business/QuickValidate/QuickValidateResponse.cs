namespace Application.Business.QuickValidate
{
    public class QuickValidateResponse
    {
        public bool HasTerminalSymbols { get; set; }

        public bool HasNonTerminalSymbols { get; set; }

        public bool HasStartingSymbol { get; set; }

        public bool HasProductions { get; set; }

        public bool HasIdentitySymbol { get; set; }

        public bool HasEndingSymbol { get; set; }
    }
}