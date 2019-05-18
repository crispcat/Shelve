namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal sealed class Lexer
    {
        private readonly Lexica lexica;
        private int position;

        private enum State
        {
            WaitForVariable,
            WaitForOperand,
            WaitForOperator,
            WaitForOpenBracket,
            WaitForDivider,
            WaitForCloseBracket
        }

        public Lexer()
        {
            lexica = new Lexica();
            inactiveRules = new Dictionary<State, int[]>
            {
                {
                    State.WaitForVariable,
                    new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 9 }
                },
                {
                    State.WaitForOperator,
                    new int[] { 1, 2, 3 }
                },
                {
                    State.WaitForOperand,
                    new int[] { 4 }
                },
                {
                    State.WaitForOpenBracket,
                    new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9 }
                },
                {
                    State.WaitForCloseBracket,
                    new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9 }
                },
                {
                    State.WaitForDivider,
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }
                }
            };

            isProcessFunction = false;
            SetState(State.WaitForVariable);
        }

        private State state;

        private Dictionary<State, int[]> inactiveRules;

        private bool isProcessFunction;

        private void SwitchStateForReceived(Token token)
        {
            bool operandRecieved = token == Token.Variable || token == Token.Value;
            bool operatorRecieved = token == Token.Binar;

            bool dividerRecieved = token == Token.Divider;
            bool functionRecieved = token == Token.Function;
            bool openBracketRecieved = token == Token.LeftBracket;
            bool closeBracketRecieved = token == Token.RightBracket;

            if (operandRecieved)
            {
                SetState(State.WaitForOperator);
            }
            else if (operatorRecieved || isProcessFunction && dividerRecieved)
            {
                SetState(State.WaitForOperand);
            }
            else if (functionRecieved)
            {
                isProcessFunction = true;
                SetState(State.WaitForOpenBracket);
            }
            else if (openBracketRecieved)
            {
                SetState(State.WaitForOperand);
            }
            else if (closeBracketRecieved)
            {
                isProcessFunction = isProcessFunction ? false : isProcessFunction;
                SetState(State.WaitForOperator);
            }
        }

        private void SetState(State state)
        {
            if (this.state == state) return;

            lexica.SwitchOnRules(inactiveRules[this.state]);
            lexica.SwitchOffRules(inactiveRules[state]);

            this.state = state; 
        }

        public LexedExpression Tokenize(string expression)
        {
            var processingExpression = new LexedExpression(expression.Replace(" ", string.Empty));

            position = 0;

            processingExpression = ProcessNextToken(processingExpression);

            if (processingExpression.ExpressionString != string.Empty)
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

        private LexedExpression ProcessNextToken(LexedExpression expression)
        {
            foreach (var rule in lexica.Rules)
            {
                var matched = rule.Matcher.Match(expression.ExpressionString);

                if (matched > 0)
                {
                    position += matched;

                    var matchedString = expression.ExpressionString.Substring(0, matched);
                    var lexema = new Lexema(matchedString, rule.Token);

                    expression.ExpressionString = expression.ExpressionString.Substring(matched);
                    expression.LexicalQueue.Enqueue(lexema);

                    SwitchStateForReceived(lexema.Token);

                    return ProcessNextToken(expression);
                }
            }

            return expression;
        }
    }
}
