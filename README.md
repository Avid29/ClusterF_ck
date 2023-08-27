# ClusterF_ck

ClusterF_ck is a .NET Cluster Analysis library. It contains a wide variety of algorithms for Cluster Analysis, and their varieties. 

## Samples

### KMeans
The code below will use KMeans to cluster the points `{0, 1, 8, 10, 12, 22, 24}` where `k = 3`.

```cs
using ClusterF_ck.Shapes;

// Make an alias "KM" for the static class KMeans, which contains the KMeans clustering methods.
using KM = ClusterF_ck.KMeans.KMeans;

int k = 3;
double[] points = new double[] {0, 1, 8, 10, 12, 22, 24};
KMeansCluster<double, DoubleShape>[] clusters = KM.Cluster<double, DoubleShape>(points, k);
```

The value of `clusters` will be 3 clusters containing the points `{0, 1}`, `{8, 10, 12}`, and `{22, 24}`. 

The namespace `ClusterF_ck.Shapes` is included because it contains a collection of `ISpace<T>` implementations that can be used to compare points. Currently (as of 2023-08-26), it only contains definitions of Euclidean space for `float`, `double`, `Vector2`, `Vector3`, and `Vector4`. If you want to cluster over a different type or in a Non-Euclidean space, you can always define your own `ISpace<T>`.
