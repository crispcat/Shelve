namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal sealed class Lexer
    {
        private enum State
        {
            WaitForOperand,
            WaitForVariable,
            WaitForOperandExceptFunction,
            WaitForOperator,
            WaitForOpenBracket
        }

        private State state;

        private int position;

        private readonly Lexica lexica;

        private LexedExpression current;

        private readonly Dictionary<State, int[]> inactiveRules;

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
                    State.WaitForOperandExceptFunction,
                    new int[] { 0, 4 }
                },
                {
                    State.WaitForOpenBracket,
                    new int[] { 0, 1, 6, 7, 8, 9 }
                }
            };

            SetState(State.WaitForVariable);
        }

        private void SwitchStateForReceived(Token token)
        {
            switch (state)
            {
                case State.WaitForVariable:

                    if (token == Token.Variable)
                    {
                        SetState(State.WaitForOperator);
                    }
                    break;

                case State.WaitForOperator:

                    if (token == Token.Binar || token == Token.Divider)
                    {
                        SetState(State.WaitForOperand);
                    }
                    break;

                case State.WaitForOperandExceptFunction:
                case State.WaitForOperand:

                    if (token == Token.Function)
                    {
                        SetState(State.WaitForOpenBracket);
                    }
                    else if (token == Token.Variable || token == Token.Value)
                    {
                        SetState(State.WaitForOperator);
                    }
                    break;

                case State.WaitForOpenBracket:

                    if (token == Token.LeftBracket)
                    {
                        SetState(State.WaitForOperand);
                    }
                    else
                    {
                        AbortTokenProcessing(2);
                        SetState(State.WaitForOperandExceptFunction);
                    }
                    break;
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
            current = new LexedExpression(expression.Replace(" ", string.Empty));

            position = 0;

            ProcessNextToken();

            if (current.ExpressionString != string.Empty)
            {
                throw new ArgumentException($"Unable to match against any tokens in " +
                    $"\"{current.Initial}\". " +
                    $"Position: {position}. Lexer state: {state.ToString()}");
            }
            else
            {
                return current;
            }
        }

        private void ProcessNextToken()
        {
            foreach (var rule in lexica.Rules)
            {
                var matched = rule.Matcher.Match(current.ExpressionString);

                if (matched > 0)
                {
                    position += matched;

                    var matchedString = current.ExpressionString.Substring(0, matched);
                    var lexema = new Lexema(matchedString, rule.Token)
                    {
                        Position = position
                    };

                    current.ExpressionString = current.ExpressionString.Substring(matched);
                    current.LexicalQueue.AddLast(lexema);

                    SwitchStateForReceived(lexema.Token);

                    ProcessNextToken();
                }
            }
        }

        private void AbortTokenProcessing(int steps)
        {
            while (current.LexicalQueue.Count != 0 && steps --> 0)
            {
                var lexema = current.LexicalQueue.Last.Value;
                current.ExpressionString = lexema.Represents + current.ExpressionString;
                position -= lexema.Represents.Length;

                current.LexicalQueue.RemoveLast();
            }
        }
    }
}
