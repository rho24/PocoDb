using System;

namespace PocoDb.Serialisation
{
    public interface ISerializer
    {
        string Serialize(object obj);
        object Deserialize(string str);
        T Deserialize<T>(string str);
    }
}