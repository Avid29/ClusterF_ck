// Adam Dernis © 2022

namespace ClusterF_ck.IO.Interfaces;

/// <summary>
/// An interface for Clusters that output with a Weight.
/// </summary>
public interface IWeightedCluster
{
    /// <summary>
    /// Gets the weight of the cluster.
    /// </summary>
    double Weight { get; }
}