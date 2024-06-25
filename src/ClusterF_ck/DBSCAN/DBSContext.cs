// Adam Dernis © 2022

using ClusterF_ck.Spaces.Interfaces.Properties;
using System;

namespace ClusterF_ck.DBSCAN;

/// <summary>
/// A struct used to wrap the stack for DBSCAN.
/// </summary>
/// <typeparam name="T">The type of points to cluster.</typeparam>
/// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
internal ref struct DBSContext<T, TShape>
    where TShape : struct, IDistanceSpace<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DBSContext{T, TShape}"/> struct.
    /// </summary>
    /// <param name="config">The configuration DBSCAN is running with.</param>
    /// <param name="shape">The shape to use on the points to cluster.</param>
    /// <param name="points">The points being clustered.</param>
    public DBSContext(DBSConfig config, TShape shape, ReadOnlySpan<T> points)
    {
        NextClusterId = 1;
        ClusterIds = new int[points.Length];

        Epsilon2 = config.Epsilon * config.Epsilon;
        MinPoints = config.MinPoints;

        NoiseCluster = config.ReturnNoise ? new DBSCluster<T>(DBSConstants.NOISE_ID) : null;

        Points = points;

        Shape = shape;
    }

    /// <summary>
    /// Gets or sets the next clusterId.
    /// </summary>
    public int NextClusterId { get; set; }

    /// <summary>
    /// Gets an array containing the cluster ids of each point in <see cref="Points"/>.
    /// </summary>
    public int[] ClusterIds { get; }

    /// <summary>
    /// Gets epsilon squared. The max distance squared to consider two points connected.
    /// </summary>
    public double Epsilon2 { get; }

    /// <summary>
    /// Gets the minimum number of connected points required to make a cluster.
    /// </summary>
    public int MinPoints { get; }

    /// <summary>
    /// Gets a cluster containing all noise points.
    /// </summary>
    /// <remarks>
    /// Null if not tracking noise.
    /// </remarks>
    public DBSCluster<T>? NoiseCluster { get; }

    /// <summary>
    /// Gets a value indicating whether or not to return the noise cluster with the results.
    /// </summary>
    public readonly bool ReturnNoise => NoiseCluster != null;

    /// <summary>
    /// Gets a pointer to a span containing all the points being clustered.
    /// </summary>
    public ReadOnlySpan<T> Points { get; }

    /// <summary>
    /// Gets or sets the shape to use on the points to cluster. 
    /// </summary>
    public TShape Shape { get; }
}