// Adam Dernis © 2022

using ClusterF_ck.IO;
using ClusterF_ck.IO.Interfaces;
using ClusterF_ck.Spaces.Properties;
using System.Collections.Generic;

namespace ClusterF_ck.Intermediate.Boxing
{
    /// <summary>
    /// A <see cref="Cluster{T, TShape}"/> implementation for Boxing cluster.
    /// </summary>
    /// <typeparam name="T">The type of data in the cluster.</typeparam>
    /// <typeparam name="TCell">The type of cell identifier</typeparam>
    /// <typeparam name="TShape">A shape to describe to provide comparison methods for <typeparamref name="T"/>.</typeparam>
    public class BoxingCluster<T, TCell, TShape> : Cluster<T, TShape>, IPointsCluster<T>, IWeightedCluster
        where T : unmanaged
        where TCell : unmanaged
        where TShape : struct, IGridSpace<T, TCell>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoxingCluster{T, TCell, TShape}"/> class.
        /// </summary>
        /// <param name="cell">The cell identify.</param>
        /// <param name="center">The center point of the cell.</param>
        internal BoxingCluster(TCell cell, T center)
        {
            Cell = cell;
            Center = center;
        }

        /// <summary>
        /// Gets the cell identity.
        /// </summary>
        public TCell Cell { get; }

        /// <summary>
        /// Gets the center of the cell.
        /// </summary>
        public T Center { get; }

        /// <inheritdoc/>
        public double Weight => Points.Count;

        /// <inheritdoc/>
        IReadOnlyCollection<T> IPointsCluster<T>.Points => Points;

        /// <summary>
        /// Gets a list of points in the cluster.
        /// </summary>
        internal List<T> Points { get; } = new List<T>();
    }
}
