// Adam Dernis © 2022

using ClusterF_ck.Spaces.Properties;
using System.Collections.Generic;
using System;

namespace ClusterF_ck.DBSCAN;

/// <summary>
/// A static class containing DBSCAN methods.
/// </summary>
public static class DBSCAN
{
    /// <summary>
    /// Clusters a set of points using DBScan.
    /// </summary>
    /// <typeparam name="T">The type of points to cluster.</typeparam>
    /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
    /// <param name="points">The set of points to cluster.</param>
    /// <param name="config">A configuration for DBSCAN including epsilon and minPoints.</param>
    /// <param name="shape">The shape to use on the points to cluster.</param>
    /// <returns>A list of clusters.</returns>
    public static unsafe List<DBSCluster<T, TShape>> Cluster<T, TShape>(
        ReadOnlySpan<T> points,
        DBSConfig<T, TShape> config,
        TShape shape = default)
        where T : unmanaged
        where TShape : struct, IDistanceSpace<T>
    {
        List<DBSCluster<T, TShape>> clusters = new();

        fixed (T* p = points)
        {
            // Create a DBSContext to avoid passing too many values between functions
            DBSContext<T, TShape> context = new(config, shape, p, points.Length);

            for (int i = 0; i < points.Length; i++)
            {
                // Skip point if already classified
                if (context.ClusterIds[i] != DBSConstants.UNCLASSIFIED_ID)
                    continue;

                // Attempt to create a new cluster, and add it the list if successful
                T point = p[i];
                var cluster = CreateCluster(point, i, context);
                if (cluster != null)
                {
                    clusters.Add(cluster);
                }
            }

            // Add noise (if applicable)
            // Noise cluster is not null in this condition.
            if (context.ReturnNoise)
            {
                clusters.Add(context.NoiseCluster!);
            }
        }

        return clusters;
    }

    private static DBSCluster<T, TShape>? CreateCluster<T, TShape>(
        T p,
        int i,
        DBSContext<T, TShape> context)
        where T : unmanaged
        where TShape : struct, IDistanceSpace<T>
    {
        // Create an empty cluster with the next cluster Id.
        // Get the connected points to the cluster, then populate the cluster
        DBSCluster<T, TShape> cluster = new(context.NextClusterId);
        List<(T, int)> seeds = GetSeeds(p, context);
        if (seeds.Count < context.MinPoints)
        {
            // Not a core point
            // Assign noise id and return null
            context.ClusterIds[i] = DBSConstants.NOISE_ID;
            if (context.ReturnNoise)
            {
                context.NoiseCluster?.Points.Add(p);
            }

            // Return without incremented next cluster id because this was not a cluster
            return null;
        }

        // This is a valid cluster. Increment the cluster id.
        context.NextClusterId++;

        // Expand the cluster to include all seeds, and their seeds recursively
        // Use seeds as a queue of seeds to add to the cluster
        for (int j = 0; j < seeds.Count; j++)
        {
            cluster.Points.Add(seeds[j].Item1);
            context.ClusterIds[seeds[j].Item2] = cluster.ClusterId;
        }

        seeds.Remove((p, i));
        ExpandCluster(cluster, seeds, context);
        return cluster;
    }

    private static void ExpandCluster<T, TShape>(
        DBSCluster<T, TShape> cluster,
        List<(T, int)> seeds,
        DBSContext<T, TShape> context)
        where T : unmanaged
        where TShape : struct, IDistanceSpace<T>
    {
        // Seeds is used as a queue for breadth-first graph traversal
        while (seeds.Count > 0)
        {
            // Find each point connected to this seed
            (T, int) p = seeds[0];
            List<(T, int)> pSeeds = GetSeeds(p.Item1, context);
            if (pSeeds.Count >= context.MinPoints)
            {
                for (int i = 0; i < pSeeds.Count; i++)
                {
                    (T, int) iP = seeds[0];
                    ref int iPId = ref context.ClusterIds[iP.Item2];

                    // If unclassified or noise, add to cluster
                    if (iPId is DBSConstants.UNCLASSIFIED_ID or DBSConstants.NOISE_ID)
                    {
                        // If unclassified, add to search queue
                        if (iPId == DBSConstants.UNCLASSIFIED_ID) seeds.Add(iP);

                        cluster.Points.Add(iP.Item1);
                        iPId = cluster.ClusterId;
                    }
                }
            }

            seeds.RemoveAt(0);
        }
    }

    private static unsafe List<(T, int)> GetSeeds<T, TShape>(
        T p,
        DBSContext<T, TShape> context)
        where T : unmanaged
        where TShape : struct, IDistanceSpace<T>
    {
        List<(T, int)> seeds = new();
        for (int i = 0; i < context.PointsLength; i++)
        {
            if (context.Shape.FindDistanceSquared(p, context.Points[i]) <= context.Episilon2)
            {
                seeds.Add((context.Points[i], i));
            }
        }

        return seeds;
    }
}