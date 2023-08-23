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
        // Track the intracluster distance of each cluster
        // The intracluster distance is the average distance of a point from the centroid.
        var clusterIntraDistances = new double[clusters.Count];
        
        // Calculate intracluster distance of each cluster
        int i = 0;
        foreach (var cluster in clusters)
        {
            clusterIntraDistances[i] = cluster.Points.Average(x => Math.Sqrt(shape.FindDistanceSquared(cluster.Centroid, x)));
            i++;
        }


        i = 0;
        //foreach ()
        //{

        //}

        return 0;
    }
}