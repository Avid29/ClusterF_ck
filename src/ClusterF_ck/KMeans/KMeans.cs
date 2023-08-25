// Adam Dernis © 2022

using ClusterF_ck.Spaces.Properties;
using CommunityToolkit.Diagnostics;
using System;

namespace ClusterF_ck.KMeans;

/// <summary>
/// A static class containing KMeans methods.
/// </summary>
public static class KMeans
{
    /// <summary>
    /// Runs KMeans cluster on a list of <typeparamref name="T"/> points.
    /// </summary>
    /// <typeparam name="T">The type of points to cluster.</typeparam>
    /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
    /// <param name="points">A list of points to cluster.</param>
    /// <param name="k">The number of clusters to make.</param>
    /// <param name="shape">The shape to use on the points to cluster.</param>
    /// <returns>An array of weighted clusters based on their prevalence in the points.</returns>
    public static KMeansCluster<T, TShape>[] Cluster<T, TShape>(
        ReadOnlySpan<T> points,
        int k,
        TShape shape)
        where TShape : struct, IDistanceSpace<T>, IAverageSpace<T>
    {
        // Split to arbitrary clusters
        KMeansCluster<T, TShape>[] clusters = Split<T, TShape>(points, k);

        // Run no items change cluster on iteration.
        bool changed = true;
        while (changed)
        {
            changed = false;

            // For each point in each cluster
            for (int i = 0; i < clusters.Length; i++)
            {
                KMeansCluster<T, TShape> cluster = clusters[i];
                for (int pointIndex = 0; pointIndex < cluster.Points.Count; pointIndex++)
                {
                    T point = cluster[pointIndex];

                    // Find the nearest cluster and move the item to it.
                    // Skip if already in nearest cluster
                    int nearestClusterIndex = FindNearestClusterIndex(point, clusters, shape);
                    if (nearestClusterIndex == i)
                        continue;

                    // A cluster can't be made empty. Leave the item in place if the cluster would be empty
                    if (cluster.Points.Count > 1)
                    {
                        T removedPoint = cluster.RemoveAt(pointIndex);
                        clusters[nearestClusterIndex].Add(removedPoint);
                        changed = true;
                    }
                }
            }
        }

        return clusters;
    }

    /// <summary>
    /// Find the nearest cluster to a point.
    /// </summary>
    /// <typeparam name="T">The type of points to cluster.</typeparam>
    /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
    /// <param name="point">The point to find a nearest cluster for.</param>
    /// <param name="clusters">The list of clusters.</param>
    /// <param name="shape">The shape to use on the points to cluster.</param>
    /// <returns>The index in <paramref name="clusters"/> of the nearest cluster to <paramref name="point"/>.</returns>
    private static int FindNearestClusterIndex<T, TShape>(
        T point,
        KMeansCluster<T, TShape>[] clusters,
        TShape shape)
        where TShape : struct, IDistanceSpace<T>, IAverageSpace<T>
    {
        // Track nearest seen value and its index.
        double minimumDistance = double.PositiveInfinity;
        int nearestClusterIndex = -1;

        for (int k = 0; k < clusters.Length; k++)
        {
            T? centroid = clusters[k].Centroid;

            // This should never happen because the cluster should never be empty.
            Guard.IsNotNull(centroid);

            double distance = shape.FindDistanceSquared(point, centroid);

            // Don't update tracking if further
            if (minimumDistance < distance)
                continue;

            minimumDistance = distance;
            nearestClusterIndex = k;
        }

        // Return index of nearest cluster
        return nearestClusterIndex;
    }

    /// <summary>
    /// Splits a list of points in to arbitrary clusters.
    /// </summary>
    /// <typeparam name="T">The type of points to cluster.</typeparam>
    /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
    /// <param name="points">The list of points to place into clusters.</param>
    /// <param name="clusterCount">The amount of clusters to create.</param>
    /// <returns>A list of arbitrary clusters of size <paramref name="clusterCount"/> made out of the points in <paramref name="points"/>.</returns>
    private static KMeansCluster<T, TShape>[] Split<T, TShape>(
        ReadOnlySpan<T> points,
        int clusterCount)
        where TShape : struct, IDistanceSpace<T>, IAverageSpace<T>
    {
        Guard.IsGreaterThanOrEqualTo(clusterCount, 2);

        var clusters = new KMeansCluster<T, TShape>[clusterCount];
        int subSize = points.Length / clusterCount;

        int iterationPos = 0;
        for (int i = 0; i < clusterCount; i++)
        {
            var currentCluster = new KMeansCluster<T, TShape>();

            // Until the cluster is full or the enumerator is out of elements.
            for (int j = 0; j < subSize && iterationPos < points.Length; j++)
            {
                // Add element to current cluster and advance iteration.
                currentCluster.Add(points[iterationPos]);
                iterationPos++;
            }

            clusters[i] = currentCluster;
        }

        return clusters;
    }
}