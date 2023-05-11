// Adam Dernis © 2022

namespace ClusterF_ck.DBSCAN;

/// <summary>
/// A class containing constant values relevant to DBSCAN.
/// </summary>
internal static class DBSConstants
{
    /// <summary>
    /// The cluster id that indicates a point is noise.
    /// </summary>
    internal const int NOISE_ID = -1;

    /// <summary>
    /// The cluster id that indicates a point is unclassified.
    /// </summary>
    internal const int UNCLASSIFIED_ID = 0;
}