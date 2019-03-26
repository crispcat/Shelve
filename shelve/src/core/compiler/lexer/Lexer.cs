namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal sealed class Lexer
    {
        private readonly Lexica lexica;
        private int position;

        public Lexer()
        {
            lexica = new Lexica();
            inactiveRules = new Dictionary<State, int[]>
            {
                {
                    State.WaitForOperator,
                    new int[] { 0, 1 }
                },
                {
                    State.WaitForOperand,
                    new int[] { 2 }
                }
            };
        }

        private enum State
        {
            WaitForOperand, WaitForOperator
        }

        private State state;

        private Dictionary<State, int[]> inactiveRules;

        private void SwitchStateForReceived(Token token)
        {
            bool operandRecieved = token == Token.Variable || token == Token.Value;
            bool operatorRecieved = token == Token.Operator;

            if (operandRecieved)
            {
                SetState(State.WaitForOperator);
            }
            else if (operatorRecieved)
            {
                SetState(State.WaitForOperand);
            }
        }

        private void SetState(State state)
        {
            if (this.state == state) return;

            lexica.SwitchOnRules(inactiveRules[this.state]);
            lexica.SwitchOffRules(inactiveRules[state]);

            this.state = state; 
        }

        public ProcessedExpression Tokenize(string expression)
        {
            position = 0;
            var processingExpression = new ProcessedExpression(expression);

            processingExpression = ProcessNextToken(processingExpression);

            if (processingExpression.String != string.Empty)
            {
                throw new ArgumentException($"Unable to match against any tokens at " +
                    $"{processingExpression.Initial}. " +
                    $"Position: {position}.");
            }
            else
            {
                return processingExpression;
            }
        }

        private ProcessedExpression ProcessNextToken(ProcessedExpression expression)
        {
            foreach (var rule in lexica.Rules)
            {
                var matched = rule.Matcher.Match(expression.String);

                if (matched > 0)
                {
                    position += matched;

                    var matchedString = expression.String.Substring(0, matched);
                    var lexema = new Lexema(matchedString, rule.Token);

                    expression.String = expression.String.Substring(matched);
                    expression.LexicalQueue.Enqueue(lexema);

                    SwitchStateForReceived(lexema.Token);

                    return ProcessNextToken(expression);
                }
            }

            return expression;
        }
    }
}
