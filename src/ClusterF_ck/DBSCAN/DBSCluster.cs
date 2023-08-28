// Adam Dernis © 2022

using ClusterF_ck.IO;
using ClusterF_ck.IO.Interfaces;
using System.Collections.Generic;

namespace ClusterF_ck.DBSCAN;

/// <summary>
/// A <see cref="Cluster{T}"/> implementation for the DBSCAN algorithm.
/// </summary>
/// <typeparam name="T">The type of data in the cluster.</typeparam>
public class DBSCluster<T> : Cluster<T>, IWeightedCluster
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DBSCluster{T}"/> class.
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
    /// <remarks>
    /// Cluster ids are 1 indexed. The id of the noise cluster is -1.
    /// </remarks>
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