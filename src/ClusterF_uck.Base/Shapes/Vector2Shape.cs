// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using CommunityToolkit.HighPerformance.Helpers;
using System.Numerics;

namespace ClusterF_ck.Shapes
{
    /// <summary>
    /// A shape defining how to handle <see cref="Vector2"/>s in a geometric space.
    /// </summary>
    public struct Vector2Shape : IGeometricSpace<Vector2, (int, int)>
    {
        /// <inheritdoc/>
        public bool AreEqual(Vector2 it1, Vector2 it2)
        {
            return it1 == it2;
        }

        /// <inheritdoc/>
        public Vector2 Average(Vector2[] items)
        {
            Vector2 sumVector = Vector2.Zero;
            foreach (var item in items)
            {
                sumVector += item;
            }
            sumVector /= items.Length;
            return sumVector;
        }

        /// <inheritdoc/>
        public double FindDistanceSquared(Vector2 it1, Vector2 it2)
        {
            return (it1 - it2).LengthSquared();
        }

        /// <inheritdoc/>
        public (int, int) GetCell(Vector2 value, double window)
        {
            var cell = value / (float)window;
            return ((int)cell.X, (int)cell.Y);
        }

        /// <inheritdoc/>
        public Vector2 GetCellCenter((int, int) cell, double window)
        {
            Vector2 cellScale = new(cell.Item1, cell.Item2);
            cellScale *= (float)window;

            var offset = new Vector2((float)(window / 2));

            return cellScale + offset;
        }

        /// <inheritdoc/>
        public Vector2 WeightedAverage((Vector2, double)[] items)
        {
            Vector2 sumVector = Vector2.Zero;
            double totalWeight = 0;
            foreach (var item in items)
            {
                sumVector += item.Item1 * (float)item.Item2;
                totalWeight += item.Item2;
            }
            sumVector /= (float)totalWeight;
            return sumVector;
        }
    }
}
