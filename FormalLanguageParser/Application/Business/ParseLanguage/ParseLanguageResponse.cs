using System.Collections.Generic;
using Application.Business.Models;

namespace Application.Business.ParseLanguage
{
    public class ParseLanguageResponse
    {
        public List<string> TerminalSymbols { get; set; }

        public List<string> NonTerminalSymbols { get; set; }

        public List<Production> Productions { get; set; }

        public string StartingSymbol { get; set; }

        public string IdentitySymbol { get; set; }

        public bool IsValid { get; set; }

        public string Reason { get; set; }

        public string InitialText { get; set; }
    }
}