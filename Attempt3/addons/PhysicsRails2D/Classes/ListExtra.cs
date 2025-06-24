using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicsRails2D;

public class ListExtra<T> : List<T>
{
        /// <summary>
        /// Cuts items from end untill specified number of items is left
        /// </summary>
        /// <param name="count">number of items to leave in list</param>
        public void LeftAtStart(int count = 1)
        {
            int RemoveCount = Count - count;
            RemoveRange(count, RemoveCount);
        }

        /// <summary>
        /// Cuts items from start untill specified number of items is left
        /// </summary>
        /// <param name="count">number of items to leave in list</param>
        public void LeftAtEnd(int count = 1)
        {
            int RemoveCount = Count - count;
            RemoveRange(0, RemoveCount);
        }
}