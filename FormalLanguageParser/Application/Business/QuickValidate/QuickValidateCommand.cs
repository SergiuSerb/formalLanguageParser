using System.Linq;

namespace Application.Business.QuickValidate
{
    public class QuickValidateCommand
    {
        public QuickValidateResponse Execute(QuickValidateRequest request)
        {
            if (request.TextToValidate.Length == 0)
            {
                return new QuickValidateResponse
                {
                    HasTerminalSymbols = false,
                    HasNonTerminalSymbols = false,
                    HasStartingSymbol = false,
                    HasProductions = false,
                    HasIdentitySymbol = false,
                    HasEndingSymbol = false
                };
            }

            return new QuickValidateResponse
            {
                HasTerminalSymbols = CheckTerminalSymbols(request),
                HasNonTerminalSymbols = CheckNonTerminalSymbols(request),
                HasStartingSymbol = CheckStartingSymbol(request),
                HasProductions = CheckProductions(request),
                HasIdentitySymbol = CheckIdentitySymbol(request),
                HasEndingSymbol = CheckEndingSymbol(request)
            };
        }

        private bool CheckEndingSymbol(QuickValidateRequest request)
        {
            return request.TextToValidate.EndsWith('&');
        }

        private bool CheckIdentitySymbol(QuickValidateRequest request)
        {
            return request.TextToValidate.Contains('@');
        }

        private bool CheckProductions(QuickValidateRequest request)
        {
            return request.TextToValidate.Split('$')
                          .Any(x => x.Length > 1);
        }

        private bool CheckStartingSymbol(QuickValidateRequest request)
        {
            return char.IsUpper(request.TextToValidate[0]);
        }

        private bool CheckNonTerminalSymbols(QuickValidateRequest request)
        {
            return request.TextToValidate.Any(char.IsUpper);
        }

        private bool CheckTerminalSymbols(QuickValidateRequest request)
        {
            return request.TextToValidate.Any(char.IsLower);
        }
    }
}