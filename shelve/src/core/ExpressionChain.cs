using System;
using System.Collections.Generic;
using System.Text;

namespace Shelve.Core 
{
    internal sealed class ExpressionChain
    {
        private LinkedList<Expression>[] priorityGroups;
        private Dictionary<string, LinkedListNode<Expression>> indexer;

        public ExpressionChain(short priorities)
        {
            priorityGroups = new LinkedList<Expression>[priorities];

            for (var i = 0; i < priorities; i++)
            {
                priorityGroups[i] = new LinkedList<Expression>();
            }
        }

        public void Add(Expression expression)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression expression)
        {
            throw new NotImplementedException();
        }

        public double Slam()
        {
            throw new NotImplementedException();
        }
    }
}
