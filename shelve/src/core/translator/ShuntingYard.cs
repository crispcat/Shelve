namespace Shelve.Core
{
    using System;
    using System.Collections.Generic;

    internal class ShuntingYard
    {
        private Queue<Lexema> inSequence;
        private Queue<Lexema> outSequence;
        private Stack<Lexema> shuntingMachine;

        public ShuntingYard(Queue<Lexema> inSequence)
        {
            outSequence = new Queue<Lexema>();
            shuntingMachine = new Stack<Lexema>();
            this.inSequence = new Queue<Lexema>(inSequence);
        }

        public Queue<Lexema> ProcessInput()
        {
            while (inSequence.Count != 0)
            {
                var lexema = inSequence.Dequeue();

                switch (lexema.Token)
                {
                    case Token.Value:
                    case Token.Variable:
                        outSequence.Enqueue(lexema);
                        break;

                    case Token.Function:
                        shuntingMachine.Push(lexema);
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
                        throw new ArgumentException($"Wrong input token {lexema.Represents}.");
                }
            }

            while (shuntingMachine.Count != 0)
            {
                var lexema = shuntingMachine.Pop();

                if (lexema.Token == Token.LeftBracket)
                {
                    throw new ArgumentException("Missing bracket!");
                }

                outSequence.Enqueue(lexema);
            }

            return outSequence;
        }

        private void PassDivider()
        {
            while (shuntingMachine.Count != 0)
            {
                if (shuntingMachine.Peek().Token == Token.LeftBracket)
                {
                    return;
                }

                outSequence.Enqueue(shuntingMachine.Pop());
            }

            throw new ArgumentException("Missing a bracket or function divider!");
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
            }

            if (shuntingMachine.Peek().Token == Token.LeftBracket || shuntingMachine.Count == 0)
            {
                shuntingMachine.Push(lexema);
            }
        }

        private void PassRightBracket()
        {
            PassDivider();
            shuntingMachine.Pop();

            if (shuntingMachine.Peek().Token == Token.Function)
            {
                outSequence.Enqueue(shuntingMachine.Pop());
            }
        }

        private bool PeekIsOperator()
        {
            return shuntingMachine.Peek().Token == Token.Unar || shuntingMachine.Peek().Token == Token.Binar;
        }
    }
}
