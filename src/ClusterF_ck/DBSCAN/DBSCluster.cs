// Adam Dernis © 2022

using ClusterF_ck.IO;
using ClusterF_ck.IO.Interfaces;
using ClusterF_ck.Spaces.Properties;
using System.Collections.Generic;

namespace ClusterF_ck.DBSCAN
{
    /// <summary>
    /// A <see cref="Cluster{T,TShape}"/> implementation for the DBSCAN algorithm.
    /// </summary>
    /// <typeparam name="T">The type of data in the cluster.</typeparam>
    /// <typeparam name="TShape">A shape to describe to provide comparison methods for <typeparamref name="T"/>.</typeparam>
    public class DBSCluster<T, TShape> : Cluster<T, TShape>, IWeightedCluster
        where T : unmanaged
        where TShape : struct, IDistanceSpace<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBSCluster{T, TShape}"/> class.
        /// </summary>
        /// <param name="id">The cluster's ID within the DBSCAN.</param>
        internal DBSCluster(int id)
        {
            ClusterId = id;
            Points = new List<T>();
        }

        /// <summary>
        /// Gets the Cluster's id.
        /// </summary>
        public int ClusterId { get; }

        /// <summary>
        /// Gets a value indicating whether or not this cluster contains the noise of the data set.
        /// </summary>
        public bool IsNoise => ClusterId == DBSConstants.NOISE_ID;

        /// <summary>
        /// Gets a list of all points in the cluster.
        /// </summary>
        public List<T> Points { get; }

        /// <inheritdoc/>
        public double Weight => Points.Count;
    }
}
