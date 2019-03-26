namespace Shelve.Core
{
    using System.Collections.Generic;

    internal struct ProcessedExpression
    {
        public string String;
        public string Initial { get; }
        public Queue<Lexema> LexicalQueue;

        public ProcessedExpression(string expression)
        {
            Initial = String = expression;
            LexicalQueue = new Queue<Lexema>();
        }

        public string GetTargetVariableName() => LexicalQueue.Peek().Represents;
    }
}
