namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal class ShuntingYard
    {
        private Queue<Lexema> inSequence;
        private Queue<Lexema> outSequence;
        private Stack<Lexema> shuntingMachine;

        private Stack<bool> wereArguments;
        private Stack<int> argumentsCountMachine;
        private Stack<int> argumentsRecievedMachine;

        private LexedExpression lexedExpression;

        public ShuntingYard(LexedExpression lexedExpression)
        {
            this.lexedExpression = lexedExpression;

            outSequence = new Queue<Lexema>();
            shuntingMachine = new Stack<Lexema>();

            wereArguments = new Stack<bool>();
            argumentsCountMachine = new Stack<int>();
            argumentsRecievedMachine = new Stack<int>();

            inSequence = new Queue<Lexema>(lexedExpression.LexicalQueue);
        }

        public LexedExpression ProcessInput()
        {
            while (inSequence.Count != 0)
            {
                var lexema = inSequence.Dequeue();

                switch (lexema.Token)
                {
                    case Token.Value:
                    case Token.Variable:
                        PassOperand(lexema);
                        break;

                    case Token.Function:
                        PassFunction(lexema);
                        break;

                    case Token.Divider:
                        PassDivider();
                        break;

                    case Token.Unar:
                    case Token.Binar:
                        PassOperator(lexema);
                        break;

                    case Token.LeftBracket:
                        shuntingMachine.Push(lexema);
                        break;

                    case Token.RightBracket:
                        PassRightBracket();
                        break;

                    default:
                        throw new ArgumentException($"Wrong input token {lexema.Represents} has arived " +
                            $"while shunting \"{lexedExpression.Initial}\" on position {lexema.Position}! " +
                            $"TargetSet: {lexedExpression.TargetSet}");
                }
            }

            while (shuntingMachine.Count != 0)
            {
                var lexema = shuntingMachine.Pop();

                if (lexema.Token == Token.LeftBracket)
                {
                    throw new ArgumentException($"Missing a bracket pair in expression " +
                        $"\"{lexedExpression.Initial}\" for bracket on position {lexema.Position}! " +
                        $"TargetSet: {lexedExpression.TargetSet}");
                }

                outSequence.Enqueue(lexema);
            }

            lexedExpression.LexicalQueue = new LinkedList<Lexema>(outSequence);

            return lexedExpression;
        }

        private void PassOperand(Lexema lexema)
        {
            outSequence.Enqueue(lexema);

            if (wereArguments.Count != 0 && !wereArguments.Peek())
            {
                wereArguments.Pop();
                wereArguments.Push(true);
            }
        }

        private void PassFunction(Lexema lexema)
        {
            shuntingMachine.Push(lexema);

            var argsCount = MathWrapper.GetFunctorFor(lexema.Represents).ParamsCount;
            argumentsCountMachine.Push(argsCount);
            argumentsRecievedMachine.Push(0);

            if (wereArguments.Count != 0)
            {
                wereArguments.Pop();
                wereArguments.Push(true);
            }
            wereArguments.Push(false);
        }

        private void PassDivider()
        {
            while (shuntingMachine.Count != 0)
            {
                if (shuntingMachine.Peek().Token == Token.LeftBracket)
                {
                    UpdateCurrentRecievedArgumentsCount();
                    return;
                }

                outSequence.Enqueue(shuntingMachine.Pop());
            }

            throw new ArgumentException($"Missing a bracket pair or function divider in expression " +
                $"\"{lexedExpression.Initial}\" on position {ErrorPosition()} !");
        }

        private void UpdateCurrentRecievedArgumentsCount()
        {
            if (argumentsRecievedMachine.Count != 0)
            {
                var argsRecieved = argumentsRecievedMachine.Pop();

                argsRecieved++;
                argumentsRecievedMachine.Push(argsRecieved);
            }
        }

        private void PassOperator(Lexema lexema)
        {
            while (PeekIsOperator())
            {
                var peek = shuntingMachine.Peek();

                bool lexemaIsUnar = lexema.Token == Token.Unar;
                bool peekIsUnar = peek.Token == Token.Unar;

                var lexemaPriority = DefaultOperator.GetPriority(lexema.Represents, lexemaIsUnar);
                var peekPriority = DefaultOperator.GetPriority(peek.Represents, peekIsUnar);
                var lexemaOrder = DefaultOperator.GetAssociativity(lexema.Represents, lexemaIsUnar);
                var peekOrder = DefaultOperator.GetAssociativity(peek.Represents, peekIsUnar);

                bool allowPass = lexemaOrder == Associativity.Left && lexemaPriority <= peekPriority ||
                    lexemaOrder == Associativity.Right && lexemaPriority < peekPriority;

                if (allowPass)
                {
                    outSequence.Enqueue(shuntingMachine.Pop());
                }
                else break;
            }

            shuntingMachine.Push(lexema);
        }

        private void PassRightBracket()
        {
            PassDivider();
            shuntingMachine.Pop();

            if (PeekIsFunction())
            {
                if (argumentsCountMachine.Peek() == argumentsRecievedMachine.Peek() && wereArguments.Peek() == true)
                {
                    outSequence.Enqueue(shuntingMachine.Pop());

                    wereArguments.Pop();
                    argumentsCountMachine.Pop();
                    argumentsRecievedMachine.Pop();
                }
                else
                {
                    var ex = new ArgumentException($"Function {shuntingMachine.Peek().Represents} expect to recieve " +
                        $"\"{argumentsCountMachine.Peek()}\" arguments. " +
                        $"You passed {RecievedArguments()} arguments " +
                        $"on position {shuntingMachine.Peek().Position} !");

                    ex.Data.Add("function", shuntingMachine.Peek().Represents);
                    ex.Data.Add("argsExpected", argumentsCountMachine.Peek());
                    ex.Data.Add("argsRecieved", RecievedArguments());

                    throw ex;
                }
            }
        }

        private int RecievedArguments() => argumentsRecievedMachine.Peek() - (wereArguments.Peek() == true ? 0 : 1);

        private int ErrorPosition()
        {
            if (shuntingMachine.Count != 0)
            {
                return shuntingMachine.Peek().Position;
            }
            else return 0;
        }

        private bool PeekIsOperator()
        {
            if (shuntingMachine.Count != 0)
            {
                return shuntingMachine.Peek().Token == Token.Unar || shuntingMachine.Peek().Token == Token.Binar;
            }
            else return false;
        }

        private bool PeekIsFunction()
        {
            if (shuntingMachine.Count != 0)
            {
                return shuntingMachine.Peek().Token == Token.Function;
            }
            else return false;
        }
    }
}
