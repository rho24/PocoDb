namespace PocoDb
{
    public interface IPocoDbServer
    {
        IPocoSession BeginSession();
        IWritablePocoSession BeginWritableSession();
    }
}