// Adam Dernis © 2022

namespace ClusterF_ck.Tests.Data.DataSet.Abstract
{
    public abstract class DataSet<T>
    {
        public DataSet(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract string Type { get; }

        public abstract T[] Data { get; }
    }
}
