namespace Shelve.Core.Tests
{
    using System;
    using Shelve.Core;
    using NUnit.Framework;

    [TestFixture]
    public class HashedCircularConcurrentQueueTests
    {
        [Test]
        public void Initialization()
        {
            var hashedPriorityQueue1 = new HashedCircularConcurrentQueue<int>(4);

            try
            {
                var hashedPriorityQueue2 = new HashedCircularConcurrentQueue<double>(-5);
            }
            catch (ArgumentException)
            {
                hashedPriorityQueue1.Add(1, 0);

                Assert.IsTrue(hashedPriorityQueue1.Count == 1);
                Assert.IsTrue(hashedPriorityQueue1.Capacity == 4);

                hashedPriorityQueue1.Add(2, 4);

                Assert.IsTrue(hashedPriorityQueue1.Count == 2);
                Assert.IsTrue(hashedPriorityQueue1.Capacity == 5);
            }

            try
            {
                hashedPriorityQueue1.Add(4, -3);
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(hashedPriorityQueue1.Count == 2);
                Assert.IsTrue(hashedPriorityQueue1.Capacity == 5);
            }
        }

        /// <summary>
        /// {1, 3, 4, 2}
        /// </summary>
        private HashedCircularConcurrentQueue<int> GetSimpleObject()
        {
            var hashedPriorityQueue = new HashedCircularConcurrentQueue<int>(4);

            hashedPriorityQueue.Add(1, 0);

            hashedPriorityQueue.Add(3, 0);

            hashedPriorityQueue.Add(2, 4);

            hashedPriorityQueue.Add(4, 3);

            return hashedPriorityQueue;
        }

        [Test]
        public void Hash()
        {
            var hashedPriorityQueue = GetSimpleObject();

            var firstHashedNode = new HashedNode<int>(1, 0);

            Assert.IsTrue(firstHashedNode.ID == hashedPriorityQueue.CircularInspect().ID);

            var secondHashedNode = new HashedNode<int>(0, 3);

            Assert.IsTrue(secondHashedNode.ID == hashedPriorityQueue.CircularInspect().ID);
        }

        [Test]
        public void CircularMove()
        {
            MoveCircleAndAssert(GetSimpleObject(), new int[] { 1, 3, 4, 2 });
        }

        private void MoveCircleAndAssert(HashedCircularConcurrentQueue<int> hashQueue, int[] values)
        {
            foreach (var value in values)
            {
                Assert.IsTrue(hashQueue.CircularInspect().Value == value);
            }
        }

        [Test]
        public void CircularMoveWithAddRemoveValue()
        {
            var hashedPriorityQueue = GetSimpleObject();

            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 1);
            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 3);

            var addedNode1 = hashedPriorityQueue.Add(2, 0);

            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 2);
  
            hashedPriorityQueue.Remove(addedNode1);

            Assert.IsFalse(hashedPriorityQueue.Contains(addedNode1));

            var addedNode2 = hashedPriorityQueue.Add(2, 0);

            hashedPriorityQueue.Remove(new HashedNode<int>(2, 0));

            Assert.IsFalse(hashedPriorityQueue.Contains(addedNode2));
            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 4);

            Assert.IsTrue(hashedPriorityQueue.Count == 4);
            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 2);
            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 1);

            hashedPriorityQueue.Add(8, 3);
            hashedPriorityQueue.Add(3, 4);

            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 3);
            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 4);
            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 8);
            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 2);
            Assert.IsTrue(hashedPriorityQueue.CircularInspect().Value == 3);

            hashedPriorityQueue.Add(11, 1);
            hashedPriorityQueue.Add(12, 2);

            MoveCircleAndAssert(hashedPriorityQueue, new int[] { 1, 3,  11,  12,  4, 8,  2, 3 });
        }
    }
}
