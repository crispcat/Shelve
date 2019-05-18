namespace Shelve.Core
{
    using Shelve.IO;
    using System.Collections.Generic;

    internal sealed class SetTranslator
    {
        private ParsedSet processedData;

        private VariableSet translatedSet;

        private HashedVariables localVariables;

        private List<LexedExpression> lexedExpressions;

        public SetTranslator(ParsedSet parsedSet)
        {
            processedData = parsedSet;
            lexedExpressions = new List<LexedExpression>();

            Tokenize();
        }

        public VariableSet Translate()
        {
            translatedSet = new VariableSet(processedData.Name)
            {
                members = localVariables = new HashedVariables()
            };

            foreach (var lexed in lexedExpressions)
            {
                TranslateExpression(lexed);
            }
       
            return translatedSet;
        }

        private void Tokenize()
        {
            foreach (var declaredGroup in processedData.Declares)
            {
                translatedSet.declares.Add(declaredGroup.Key, new List<string>());

                foreach (var expression in declaredGroup.Value)
                {
                    var lexed = TokenizeExpression(expression);
                    var targetVariable = lexed.GetTargetVariableName();

                    lexedExpressions.Add(lexed);
                    translatedSet.declares[declaredGroup.Key].Add(targetVariable);
                }
            }

            foreach (var expression in processedData.Expressions)
            {
                lexedExpressions.Add(TokenizeExpression(expression));
            }
        }

        private LexedExpression TokenizeExpression(string expression)
        {
            var lexer = new Lexer();

            var lexedExpression = lexer.Tokenize(expression);
            lexedExpression.TargetSet = translatedSet.Name;

            return lexedExpression;
        }

        private void TranslateExpression(LexedExpression translatedExpression)
        {
            var compiler = new ExpressionTranslator(translatedExpression, localVariables);

            var translated = compiler.CreateCalculationStack();
            localVariables[translated.TargetVariable].AddExpression(translated);
        }
    }
}
