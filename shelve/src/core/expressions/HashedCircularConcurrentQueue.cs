namespace Shelve.Core
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public sealed class HashedCircularConcurrentQueue<T> where T : IEquatable<T>
    {
        #region Data
        private Dictionary<HashedNode<T>, LinkedListNode<HashedNode<T>>> indexer;
        private LinkedList<HashedNode<T>>[] priorityGroups;
        private LinkedListNode<HashedNode<T>> current;
        private bool firstElement;
        private int currentLayer;
        #endregion

        #region Properties
        public int ElementsOnCurrentLayer => priorityGroups[currentLayer].Count;
        public int Capacity => priorityGroups.Length;
        public int Count { get; private set; }
        #endregion

        #region Lifecycle
        /// <summary>
        /// Initialize instance with given count of "valid" priority levels to memory allocate
        /// </summary>
        public HashedCircularConcurrentQueue(int priorityCount)
        {
            if (priorityCount < 1)
            {
                throw new ArgumentException("Prioriy count must be greater or equal 1.");
            }

            priorityGroups = new LinkedList<HashedNode<T>>[priorityCount];
            indexer = new Dictionary<HashedNode<T>, LinkedListNode<HashedNode<T>>>(priorityCount);

            for (var i = 0; i < priorityCount; i++)
            {
                priorityGroups[i] = new LinkedList<HashedNode<T>>();
            }

            Count = 0;
        }
        /// <summary>
        /// O(p_count)
        /// </summary>
        private void ReallocateMemory(int priorityCount)
        {
            if (priorityCount < 0)
            {
                throw new ArgumentException("Allocating memory size must be a positive value!");
            }

            var memory = priorityGroups;
            priorityGroups = new LinkedList<HashedNode<T>>[priorityCount];

            bool needAllocateMemory = priorityCount > memory.Length;
            var endIndex = needAllocateMemory ? memory.Length : priorityCount;

            for (int i = 0; i < endIndex; i++)
            {
                priorityGroups[i] = memory[i];
            }

            if (needAllocateMemory)
            {
                for (int i = memory.Length; i < priorityCount; i++)
                {
                    priorityGroups[i] = new LinkedList<HashedNode<T>>();
                }
            }
        }
        #endregion

        #region Interface
        /// <summary>
        /// O(1) | O(n) if collision or priority greater than initial.
        /// </summary>
        public HashedNode<T> Add(T @object, int priority)
        {
            if (priority < 0)
            {
                throw new ArgumentException("Priority must be a positive value!");
            }

            var element = new HashedNode<T>(@object, priority);

            if (indexer.ContainsKey(element))
            {
                throw new Exception($"Element: {@object} already exist.");
            }

            if (priority > priorityGroups.Length - 1)
            {
                ReallocateMemory(priority + 1);
            }

            indexer.Add(element, priorityGroups[priority].AddLast(element));
            Count++;

            if (current == null)
            {
                Reset();
            }

            return element;
        }
        /// <summary>
        /// O(1) | O(n) if collision
        /// </summary>
        public void Remove(HashedNode<T> element)
        {
            if (!indexer.ContainsKey(element))
            {
                throw new Exception($"Element: {element.Value} does not exist.");
            }

            var linkedListNode = indexer[element];

            priorityGroups[element.Priority].Remove(linkedListNode);
            indexer.Remove(element);
            Count--;
        }
        /// <summary>
        /// O(1) | O(n) if collision
        /// </summary>
        public bool Contains(HashedNode<T> hashedNode)
        {
            return indexer.ContainsKey(hashedNode);
        }
        /// <summary>
        /// O(1)
        /// </summary>
        public HashedNode<T> CircularInspect()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Cannot inspect empty queue.");
            }

            if (!firstElement && !MoveNext())
            {
                Reset();
            }

            firstElement = false;

            return current.Value;
        }
        #endregion

        #region Service
        private bool MoveNext()
        {
            bool result;

            if (current.Next != null)
            {
                current = current.Next;
                result = true;
            }
            else
            {
                currentLayer = FindFirstNonEmptyGroupIndex(from: currentLayer + 1);

                if (currentLayer != -1)
                {
                    current = priorityGroups[currentLayer].First;
                    result = true;
                }
                else result = false;
            }

            return result;
        }

        private void Reset()
        {
            currentLayer = FindFirstNonEmptyGroupIndex(from: 0);

            if (currentLayer == -1)
            {
                throw new InvalidOperationException("Queue is empty!");
            }

            current = priorityGroups[currentLayer].First;
            firstElement = true;
        }

        private int FindFirstNonEmptyGroupIndex(int from)
        {
            if (from < 0)
            {
                throw new ArgumentException("From index must be a positive value.");
            }

            if (Count == 0)
            {
                new InvalidOperationException("Queue is empty!");
            }

            for (int i = from; i < priorityGroups.Length; i++)
            {
                if (priorityGroups[i].Count > 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var key in indexer.Keys)
            {
                sb.Append($"({key.Value.ToString()}, {key.Priority}), ");
            }

            return sb.ToString().TrimEnd(new char[] { ',', ' ' });
        }
        #endregion
    }
}
