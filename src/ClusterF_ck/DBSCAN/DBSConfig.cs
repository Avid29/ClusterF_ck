// Adam Dernis © 2022

using CommunityToolkit.Diagnostics;

namespace ClusterF_ck.DBSCAN;

/// <summary>
/// A struct containing configuration info for DBSCAN Cluster to run.
/// </summary>
public struct DBSConfig
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DBSConfig"/> struct.
    /// </summary>
    /// <param name="eps">The maximum distance designated as a connected point.</param>
    /// <param name="minPts">The minimum number of points to form a region.</param>
    /// <param name="returnNoise">Indicates if a Cluster containing all noise points should be appended to the resultant cluster list.</param>
    public DBSConfig(double eps, int minPts, bool returnNoise = false)
    {
        Guard.IsGreaterThan(eps, 0, nameof(eps));
        Guard.IsGreaterThanOrEqualTo(minPts, 0, nameof(minPts));

        ReturnNoise = returnNoise;
        Epsilon = eps;
        MinPoints = minPts;
    }

    /// <summary>
    /// Gets a value indicating whether or not to append the Cluster list with a cluster containing all noise points.
    /// </summary>
    public bool ReturnNoise { get; }

    /// <summary>
    /// Gets the maximum distance to consider two connected points.
    /// </summary>
    public double Epsilon { get; }

    /// <summary>
    /// Gets the minimum number of points to consider a cluster.
    /// </summary>
    public int MinPoints { get; }
}