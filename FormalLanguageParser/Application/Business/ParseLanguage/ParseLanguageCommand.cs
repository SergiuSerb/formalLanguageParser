using System.Collections.Generic;
using System.Linq;
using Application.Business.Models;

namespace Application.Business.ParseLanguage
{
    public class ParseLanguageCommand
    {
        public ParseLanguageResponse Execute(ParseLanguageRequest request)
        {
            ValidationResult requestValidationResult = ValidateRequest(request);

            if (!requestValidationResult.IsValid)
            {
                return new ParseLanguageResponse
                {
                    IsValid = requestValidationResult.IsValid,
                    Reason = requestValidationResult.Reason
                };
            }

            ParseLanguageResponse response = new ParseLanguageResponse
            {
                InitialText = request.TextToParse,
                StartingSymbol = GetStartingSymbol(request),
                IdentitySymbol = GetIdentitySymbol(request),
                TerminalSymbols = GetTerminalSymbols(request),
                NonTerminalSymbols = GetNonTerminalSymbols(request),
                Productions = GetProductions(request)
            };

            response.TerminalSymbols.Add(response.IdentitySymbol);

            ValidationResult validationResult = ValidateResponse(response);
            response.IsValid = validationResult.IsValid;
            response.Reason = validationResult.Reason;

            return response;
        }

        private ValidationResult ValidateRequest(ParseLanguageRequest request)
        {
            if (HasNoText(request))
            {
                return new ValidationResult { IsValid = false, Reason = "Input is empty." };
            }

            if (HasNoEndingSymbol(request))
            {
                return new ValidationResult { IsValid = false, Reason = "Has no ending symbol" };
            }

            return new ValidationResult { IsValid = true };
        }

        private bool HasNoEndingSymbol(ParseLanguageRequest request)
        {
            return request.TextToParse.Last() != '&';
        }

        private static bool HasNoText(ParseLanguageRequest request)
        {
            return request.TextToParse.Length == 0;
        }

        private ValidationResult ValidateResponse(ParseLanguageResponse response)
        {
            if (HasNoStartingSymbol(response))
            {
                return new ValidationResult { IsValid = false, Reason = "Input has no Starting Symbol." };
            }

            if (HasNoIdentitySymbol(response))
            {
                return new ValidationResult { IsValid = false, Reason = "Input has no Identity Symbol." };
            }

            if (HasNoProductions(response))
            {
                return new ValidationResult { IsValid = false, Reason = "Input has no Productions." };
            }

            if (HasNoTerminalSymbols(response))
            {
                return new ValidationResult { IsValid = false, Reason = "Input has no Terminal Symbols." };
            }

            if (HasNoNonTerminalSymbols(response))
            {
                return new ValidationResult { IsValid = false, Reason = "Input has no NonTerminal Symbols." };
            }

            return new ValidationResult { IsValid = true };
        }

        private static bool HasNoNonTerminalSymbols(ParseLanguageResponse response)
        {
            return response.NonTerminalSymbols.Count == 0;
        }

        private static bool HasNoTerminalSymbols(ParseLanguageResponse response)
        {
            return response.TerminalSymbols.Count == 0;
        }

        private static bool HasNoProductions(ParseLanguageResponse response)
        {
            return response.Productions.Count == 0;
        }

        private static bool HasNoIdentitySymbol(ParseLanguageResponse response)
        {
            return response.IdentitySymbol.Length == 0;
        }

        private static bool HasNoStartingSymbol(ParseLanguageResponse response)
        {
            return response.StartingSymbol.Length == 0;
        }

        private List<Production> GetProductions(ParseLanguageRequest request)
        {
            return request.TextToParse.Split('$')
                          .Distinct()
                          .Select(production => CreateProduction(production))
                          .ToList();
        }

        private Production CreateProduction(string productionText)
        {
            Production production = new Production();

            if (productionText.Last() == '&')
            {
                productionText = productionText.Remove(productionText.Length - 1);
            }

            production.Input = productionText[0].ToString();
            production.Output = productionText.Remove(0, 1);
            production.Output = production.Output.Replace('@', 'λ');

            return production;
        }

        private List<string> GetNonTerminalSymbols(ParseLanguageRequest request)
        {
            return request.TextToParse.Where(x => char.IsUpper(x))
                          .Select(x => x.ToString())
                          .Distinct()
                          .ToList();
        }

        private List<string> GetTerminalSymbols(ParseLanguageRequest request)
        {
            return request.TextToParse.Where(x => char.IsLower(x))
                          .Select(x => x.ToString())
                          .Distinct()
                          .ToList();
        }

        private string GetIdentitySymbol(ParseLanguageRequest request)
        {
            return request.TextToParse.Any(x => x == '@')
                ? "λ"
                : "";
        }

        private string GetStartingSymbol(ParseLanguageRequest request)
        {
            return request.TextToParse[0].ToString();
        }
    }
}