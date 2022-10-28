// Adam Dernis © 2022

using ClusterF_ck.Tests.Data.DataSet.Abstract;
using System.Numerics;

namespace ClusterF_ck.Tests.Data.DataSet
{
    public class FixedSet<T> : DataSet<T>
    {
        public FixedSet(string name, params T[] data) :
            base(name)
        {
            Data = data;
        }

        public override string Type => "Fixed";

        public override T[] Data { get; }
    }

    public static class FixedSets
    {
        public static FixedSet<double> Double_Test1 
            = new("Double test 1",
                0, 1,
                8, 10, 12,
                22, 24);

        public static FixedSet<Vector2> Vector2_Test1 =
            new("Vector2 Test 1",
            new Vector2(0, 2), new Vector2(1, 1), new Vector2(2, 0),
            new Vector2(7, 5), new Vector2(5, 7), new Vector2(6, 6));
    }
}
