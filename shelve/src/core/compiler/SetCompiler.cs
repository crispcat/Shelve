namespace Shelve.Core
{
    using System;
    using Shelve.IO;
    using System.Collections.Generic;

    internal class SetCompiler
    {
        private ParsedSet processedData;

        public VariableSet TranslatedSet { get; }

        public SetCompiler(ParsedSet parsedSet)
        {
            processedData = parsedSet;
            TranslatedSet = new VariableSet();
        }

        public void Compile()
        {
            TranslateDeclares();
            TranslateDependent();
        }

        public void TranslateDeclares()
        {
            foreach (var declaredGroup in processedData.Declares)
            {
                TranslatedSet.declares.Add(declaredGroup.Key, new List<string>());

                foreach (var expression in declaredGroup.Value)
                {
                    var translatedExpression = TranslateAndValidate(expression);
                    var targetVariable = translatedExpression.GetTargetVariableName();

                    TranslatedSet.declares[declaredGroup.Key].Add(targetVariable);

                    CompileExpression(translatedExpression);
                }
            }
        }

        public void TranslateDependent()
        {
            foreach (var expression in processedData.Expressions)
            {
                CompileExpression(TranslateAndValidate(expression));
            }
        }

        private ProcessedExpression TranslateAndValidate(string expression)
        {
            var lexer = new Lexer();
            var validator = new Validator();

            var tokenizedExpression = lexer.Tokenize(expression);
            
            if (!validator.Check(tokenizedExpression))
            {
                throw validator.GeneratedException;
            }

            return tokenizedExpression;
        }

        private void CompileExpression(ProcessedExpression translatedExpression)
        {
            throw new NotImplementedException();
        }

        private void LinkVariables()
        {

        }
    }
}
