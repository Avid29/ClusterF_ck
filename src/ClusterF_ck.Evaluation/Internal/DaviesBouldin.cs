// Adam Dernis © 2023

using ClusterF_ck.IO.Interfaces;
using ClusterF_ck.Spaces.Interfaces.Properties;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusterF_ck.Evaluation.Internal;

/// <summary>
/// A static class containing methods for evaluating the Davies-Bouldin Index of a set of clusters.
/// </summary>
public static class DaviesBouldin
{
    /// <summary>
    /// Evaluates the Davies-Bouldin Index of a set of clusters.
    /// </summary>
    /// <typeparam name="T">The type of point in the cluster.</typeparam>
    /// <typeparam name="TCluster">The type of the cluster.</typeparam>
    /// <typeparam name="TShape">The shape used to analyze the cluster.</typeparam>
    /// <param name="clusters">The set of clusters to evaluate.</param>
    /// <param name="shape">The shape to use to compare the points in the cluster.</param>
    /// <returns>The Davies-Bouldin Index of the cluster set.</returns>
    public static double Evaluate<T, TCluster, TShape>(List<TCluster> clusters, TShape shape = default)
        where TCluster : ICentroidCluster<T>, IPointsCluster<T>
        where TShape : struct, IDistanceSpace<T>
    {
        // N is a shorthand for the cluster count.
        int n = clusters.Count;

        // Calculate the intracluster distance of each cluster
        double[] clusterIntraDistances = clusters.Select(x => Intermediates.IntraDistance<T, TCluster, TShape>(x, shape)).ToArray();
        
        // Calculate the similarities of each cluster
        double[,] similarities = new double[n, n];
        for (int i = 0; i < n - 1; i++)
        {
            // Get the centroid and intra-distance of cluster i
            T? iCentroid = clusters[i].Centroid;
            double iIntraDist = clusterIntraDistances[i];
            Guard.IsNotNull(iCentroid);

            for (int j = i + 1; j < n; j++)
            {
                // Get the centroid and intra-distance of cluster j
                T? jCentroid = clusters[j].Centroid;
                double jIntraDistance = clusterIntraDistances[j];
                Guard.IsNotNull(jCentroid);

                // Calculate the distance between cluster i and cluster j
                double ijDistance = Math.Sqrt(shape.FindDistanceSquared(iCentroid, jCentroid));

                // Store similarity to the similarities table
                // This is a bidirectional relationship, so we can calculate it once and store it for [i,j] and [j,i]
                similarities[i, j] = similarities[j, i]
                    = (iIntraDist + jIntraDistance) / ijDistance;
            }
        }

        // Find the most greatest similarity for each cluster
        double[] greatestSimilarity = new double[n];
        for (int i = 0; i < n; i++)
        {
            // Find the greatest in the row
            double max = double.NegativeInfinity;
            foreach (double x in similarities.GetRow(i))
                max = Math.Max(max, x);

            greatestSimilarity[i] = max;
        }

        return greatestSimilarity.Average();
    }
}