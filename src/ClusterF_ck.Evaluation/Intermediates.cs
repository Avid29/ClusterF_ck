// Adam Dernis © 2023

using ClusterF_ck.IO.Interfaces;
using ClusterF_ck.Spaces.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusterF_ck.Evaluation;

/// <summary>
/// A static class containing intermediate methods for cluster evaluation.
/// </summary>
public static class Intermediates
{
    /// <summary>
    /// Calculates the intracluster distances of a list of clusters.
    /// </summary>
    /// <typeparam name="T">The type of point in the clusters.</typeparam>
    /// <typeparam name="TCluster">The type of the clusters.</typeparam>
    /// <typeparam name="TShape">The shape used to analyze the clusters.</typeparam>
    /// <param name="clusters">The list of clusters to get the intra distances.</param>
    /// <param name="shape">The shape to use to compare the points in the clusters.</param>
    /// <returns>The intra distances of the clusters.</returns>
    public static double[] IntraDistances<T, TCluster, TShape>(List<TCluster> clusters, TShape shape = default)
        where T : unmanaged
        where TCluster : ICentroidCluster<T>, IPointsCluster<T>
        where TShape : struct, IDistanceSpace<T>
    {
        // Track the intracluster distance of each cluster
        // The intracluster distance is the average distance of a point from the centroid.
        var results = new double[clusters.Count];
        
        // Calculate intracluster distance of each cluster
        int i = 0;
        foreach (var cluster in clusters)
        {
            results[i] = IntraDistance<T, TCluster, TShape>(cluster, shape);
            i++;
        }

        return results;
    }

    /// <summary>
    /// Calculates the intracluster distance of a cluster.
    /// </summary>
    /// <typeparam name="T">The type of point in the cluster.</typeparam>
    /// <typeparam name="TCluster">The type of the cluster.</typeparam>
    /// <typeparam name="TShape">The shape used to analyze the cluster.</typeparam>
    /// <param name="cluster">The cluster to get the intra distance.</param>
    /// <param name="shape">The shape to use to compare the points in the cluster.</param>
    /// <returns></returns>
    public static double IntraDistance<T, TCluster, TShape>(TCluster cluster, TShape shape = default)
        where T : unmanaged
        where TCluster : ICentroidCluster<T>, IPointsCluster<T>
        where TShape : struct, IDistanceSpace<T>
        => cluster.Points.Average(x => Math.Sqrt(shape.FindDistanceSquared(cluster.Centroid, x)));
}
