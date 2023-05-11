// Adam Dernis © 2023

using ClusterF_ck.IO.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using ClusterF_ck.Spaces.Properties;

namespace ClusterF_ck.Evaluation.Internal;

public static class DaviesBouldin
{
    public static double Evaluate<T, TCluster, TShape>(List<TCluster> clusters, TShape shape = default)
        where T : unmanaged
        where TCluster : ICentroidCluster<T>, IPointsCluster<T>
        where TShape : struct, IDistanceSpace<T>
    {
        var intraDistances = new double[clusters.Count];

        int i = 0;
        foreach (var cluster in clusters)
        {
            intraDistances[i] = cluster.Points.Average(x => shape.FindDistanceSquared(cluster.Centroid, x));
            i++;
        }

        var pairwiseDistances = new Dictionary<(TCluster, TCluster), double>();
        foreach (var cluster1 in clusters)
        {
            foreach (var cluster2 in clusters)
            {
                if (cluster1.Equals(cluster2))
                    continue;

                pairwiseDistances.Add((cluster1, cluster2),
                    shape.FindDistanceSquared(cluster1.Centroid, cluster2.Centroid));
            }
        }

        return 0;
    }
}