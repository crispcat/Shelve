public class BracketsModel
{
    private int bracketsOpened;
    private int bracketsClosed;

    public int BracketsOpened
    {
        get;
        set;
    }

    public int BracketsClosed
    {
        get
        {
            return bracketsClosed;
        }
        set
        {
            if (value <= BracketsOpened)
            {
                bracketsClosed = value;
            }
            else
            {
                throw new System.Exception("Wrong brackets sequence!");
            }
        }
    }

    public bool FirstLevelExpression
    {
        get
        {
            return BracketsOpened == BracketsClosed;
        }
    }

    public BracketsModel()
    {
        bracketsOpened = 0;
        bracketsClosed = 0;
    }
}
