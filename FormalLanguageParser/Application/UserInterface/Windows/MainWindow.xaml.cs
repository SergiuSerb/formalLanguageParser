using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Application.Business.Models;
using Application.Business.ParseLanguage;
using Application.Business.QuickValidate;

namespace Application
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void parseButton_Click(object sender, RoutedEventArgs e)
        {
            string textToParse = " ";
            bool hasNoErrors = true;
            if (tabControl.SelectedIndex == 0)
            {
                textToParse = textBoxInputTextBox.Text
                                                 .Replace(" ", "")
                                                 .Replace("\r\n", "");
            }
            else
            {
                try
                {
                    textToParse = File.ReadAllText($"{fileInputTextBox.Text}");
                    textToParse = textToParse.Replace(" ", "")
                                             .Replace("\r\n", "");
                }
                catch (Exception exception)
                {
                    resultTextBox.Text = "Cannot read file. Please check if the path is correct and the file exists.";
                    hasNoErrors = false;
                }
            }

            if (hasNoErrors)
            {
                ParseLanguageRequest request = CreateParseLanguageRequest(textToParse);
                ParseLanguageCommand command = CreateParseLanguageCommand();
                ParseLanguageResponse response = command.Execute(request);

                resultTextBox.Text = ParseResponse(response);
            }
        }

        private static string ParseResponse(ParseLanguageResponse response)
        {
            if (!response.IsValid)
            {
                return response.Reason;
            }

            string parsedInitialText = ParseInitialText(response);
            string parsedProductions = ParseProductions(response);
            string parsedTerminalSymbols = ParseTerminalSymbols(response);
            string parsedNonTerminalSymbols = ParseNonTerminalSymbols(response);

            return $"{parsedInitialText}\n{parsedProductions}\n{parsedNonTerminalSymbols}\n{parsedTerminalSymbols}";
        }

        private static string ParseInitialText(ParseLanguageResponse response)
        {
            return $"Init = {response.InitialText}";
        }

        private static string ParseProductions(ParseLanguageResponse response)
        {
            string parsedResponse = new string(' ', 0);
            foreach (Production production in response.Productions)
            {
                parsedResponse += $"{production.Input}->{production.Output}; ";
            }

            return $"Prod = {{{parsedResponse}}}";
        }

        private static string ParseNonTerminalSymbols(ParseLanguageResponse response)
        {
            string parsedResponse = new string(' ', 0);
            foreach (string symbol in response.NonTerminalSymbols)
            {
                parsedResponse += symbol + ", ";
            }

            parsedResponse = parsedResponse.Remove(parsedResponse.Length - 2);

            return $"Vn = {{{parsedResponse}}}";
        }

        private static string ParseTerminalSymbols(ParseLanguageResponse response)
        {
            string parsedResponse = new string(' ', 0);
            foreach (string symbol in response.TerminalSymbols)
            {
                parsedResponse += symbol + ", ";
            }

            parsedResponse = parsedResponse.Remove(parsedResponse.Length - 2);

            return $"Vt = {{{parsedResponse}}}";
        }

        private static ParseLanguageCommand CreateParseLanguageCommand()
        {
            return new ParseLanguageCommand();
        }

        private static ParseLanguageRequest CreateParseLanguageRequest(string textToParse)
        {
            return new ParseLanguageRequest
            {
                TextToParse = textToParse
            };
        }

        private void textBoxInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textToValidate = textBoxInputTextBox.Text.Replace(" ", "")
                                                       .Replace("\r\n", "");
            QuickValidateRequest request = CreateQuickValidateRequest(textToValidate);
            QuickValidateCommand command = CreateQuickValidateCommand();
            QuickValidateResponse response = command.Execute(request);

            ProcessResponse(response);
        }

        private void ProcessResponse(QuickValidateResponse response)
        {
            endingSymbolLabel.Foreground = new SolidColorBrush(response.HasEndingSymbol
                ? Colors.Green
                : Colors.Red);

            startingSymbolLabel.Foreground = new SolidColorBrush(response.HasStartingSymbol
                ? Colors.Green
                : Colors.Red);

            identitySymbolsLabel.Foreground = new SolidColorBrush(response.HasIdentitySymbol
                ? Colors.Green
                : Colors.Red);

            nonTerminalSymbolsLabel.Foreground = new SolidColorBrush(response.HasNonTerminalSymbols
                ? Colors.Green
                : Colors.Red);

            terminalSymbolsLabel.Foreground = new SolidColorBrush(response.HasTerminalSymbols
                ? Colors.Green
                : Colors.Red);

            productionsLabel.Foreground = new SolidColorBrush(response.HasProductions
                ? Colors.Green
                : Colors.Red);
        }

        private QuickValidateCommand CreateQuickValidateCommand()
        {
            return new QuickValidateCommand();
        }

        private QuickValidateRequest CreateQuickValidateRequest(string textToValidate)
        {
            return new QuickValidateRequest
            {
                TextToValidate = textToValidate
            };
        }
    }
}