// Adam Dernis © 2023

using ClusterF_ck.IO.Interfaces;
using ClusterF_ck.Spaces.Interfaces.Properties;
using CommunityToolkit.Diagnostics;
using System;
using System.Linq;

namespace ClusterF_ck.Evaluation;

/// <summary>
/// A static class containing intermediate methods for cluster evaluation.
/// </summary>
public static class Intermediates
{
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
        where TCluster : ICentroidCluster<T>, IPointsCluster<T>
        where TShape : struct, IDistanceSpace<T>
    {   
        T? centroid = cluster.Centroid;
        Guard.IsNotNull(centroid);

        return cluster.Points.Average(x => Math.Sqrt(shape.FindDistanceSquared(centroid, x)));
    }
}
