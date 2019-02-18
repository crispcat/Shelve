using System.Collections.Generic;

public class ExpressionParsingBuffer
{
    private VariableSet refToMem;

    public Stack<string> expressions;

    public float memory;

    public Stack<float> shifts;

    private float i;

    private float Shift
    {
        get
        {
            return ++i;
        }
    }

    public string rest;

    public List<Act> acts;

    public ExpressionParsingBuffer(string str, VariableSet refToMem)
    {
        rest = string.Empty;

        expressions = new Stack<string>();
        shifts = new Stack<float>();
        acts = new List<Act>();

        expressions.Push(str.TrimToExpression());

        memory = 0;

        i = 0;

        shifts.Push(i);

        this.refToMem = refToMem;
    }

    public void CreateOperands(int splitIndex)
    {
        var parts = rest.SplitBy(splitIndex);

        char sign = rest[splitIndex];

        int n = 0;

        var operands = new Operand[2];

        while (n < 2)
        {
            operands[n] = Operand.CreateFrom(parts[n], refToMem);

            if (operands[n] == null)
            {
                float shift = Shift;

                operands[n] = new Operand(shift, isIndex: true);

                ShiftExpression(parts[n], shift);
            }

            n++;
        }

        WriteAct(new Act(operands[0], operands[1], rest[splitIndex]));
    }

    private void ShiftExpression(string expression, float shift)
    {
        expressions.Push(expression.TrimToExpression());

        shifts.Push(shift);
    }

    private void WriteAct(Act act)
    {
        act.SequencePosition = memory;

        acts.Add(act);
    }

    public Queue<Act> GetActionSequence()
    {
        var actionsArray = new Operand[acts.Count];

        acts.Sort((a1, a2) => { return a2.SequencePosition.CompareTo(a1.SequencePosition); });

        Queue<Act> actionsSequence = new Queue<Act>(acts);

        return actionsSequence;
    }
}
