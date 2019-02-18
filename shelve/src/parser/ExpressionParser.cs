using System;
using System.Collections.Generic;

public static class ExpressionParser
{
    

    private static HashSet<char> GetAllOperators()
    {
        HashSet<char> operators = new HashSet<char>(spliters[0]);

        for (int i = 1; i < spliters.Length; i++)
        {
            operators.UnionWith(spliters[i]);
        }

        return operators;
    }

    public static string[] SplitBy(this string expression, int position)
    {
        string[] parts = new string[]
        {
            expression.Substring(0, position),
            expression.Substring(position + 1, expression.Length - position - 1)
        };

        return parts;
    }

    public static Expression ToExpressionIn(this string expression, string targetVariable, VariableSet refToMem)
    {
        expression = expression.Replace(" ", String.Empty);

        var buffer = new ExpressionParsingBuffer(expression, refToMem);

        int operations = 0;

        while (buffer.expressions.Count != 0)
        {
            buffer.rest = buffer.expressions.Pop();
            buffer.memory = buffer.shifts.Pop();

            foreach (var spliterGroup in spliters)
            {
                if (buffer.ShiftBy(spliterGroup))
                {
                    operations++;
                    break;
                }
            }
        }

        if (operations != buffer.acts.Count)
        {
            throw new Exception(string.Format("Wrong expression format: [{0}]", expression));
        }

        return new Expression(targetVariable, targetSet: refToMem, acts: buffer.GetActionSequence());
    }

    private static bool ShiftBy(this ExpressionParsingBuffer buffer, HashSet<char> spliters)
    {
        bool isInvertedExpression = unars.Contains(buffer.rest[0]) && pass.Contains(buffer.rest[1]);

        if (isInvertedExpression)
        {
            buffer.rest = '0' + buffer.rest;
        }

        BracketsModel bm = new BracketsModel();

        for (int index = 0; index < buffer.rest.Length; index++)
        {
            char ch = buffer.rest[index];

            if (ch == '(')
            {
                bm.BracketsOpened++;

                continue;
            }

            if (ch == ')')
            {
                try
                {
                    bm.BracketsClosed++;
                }
                catch
                {
                    throw new Exception(string.Format("Wrong brackets sequence in expression: [{0}]", buffer.rest));
                }

                continue;
            }

            if (!bm.FirstLevelExpression)
            {
                continue;
            }

            if (spliters.Contains(ch))
            {
                if (unars.Contains(ch) && IsUnarOperator(buffer.rest, index))
                {
                    continue;
                }

                buffer.CreateOperands(index);
                return true;
            }
        }

        return false;
    }

    private static bool IsUnarOperator(string expression, int index)
    {
        bool r = CheckNeighbour(expression, index + 1);
        bool l = CheckNeighbour(expression, index - 1, invertDirection: true);

        return r || l;
    }

    private static bool CheckNeighbour(string expression, int index, bool invertDirection = false)
    {
        HashSet<char> operators = GetAllOperators();

        char? neighbour = null;

        int i = index;

        Func<bool> breakpoint = () => { return invertDirection ? (i >= 0) : (i < expression.Length); };

        Action increment = () => { i += invertDirection ? -1 : 1; };

        while (breakpoint())
        {
            if (pass.Contains(expression[i]))
            {
                increment();
                continue;
            }

            neighbour = expression[i];
            break;
        }

        bool outOfRange = neighbour == null;

        if (!outOfRange && unars.Contains((char)neighbour))
        {
            throw new Exception(string.Format("Wrong expression format in expression [{0}] position : [{1}] double (or more) unar operators must be in brackets", expression, index));
        }

        return outOfRange || operators.Contains((char)neighbour);
    }

    public static string TrimToExpression(this string str)
    {
        if (str[0] != '(' || str[str.Length - 1] != ')')
        {
            return str;
        }
        else
        {
            try
            {
                BracketsModel bm = new BracketsModel();

                for (int i = 1; i < str.Length - 1; i++)
                {
                    if (str[i] == '(')
                    {
                        bm.BracketsOpened++;
                    }

                    if (str[i] == ')')
                    {
                        bm.BracketsClosed++;
                    }
                }
            }

            catch
            {
                return str;
            }

            return str.Substring(1, str.Length - 2);
        }
    }
}
