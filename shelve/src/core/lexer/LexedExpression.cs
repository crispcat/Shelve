namespace Shelve.Core
{
    using System.Collections.Generic;

    internal struct LexedExpression
    {
        public string TargetSet;
        public readonly string Initial;
        public string ExpressionString;
        public LinkedList<Lexema> LexicalQueue;

        public LexedExpression(string expression)
        {
            TargetSet = string.Empty;
            Initial = ExpressionString = expression;
            LexicalQueue = new LinkedList<Lexema>();
        }

        public string GetTargetVariableName() => LexicalQueue.First.Value.Represents;
    }
}
