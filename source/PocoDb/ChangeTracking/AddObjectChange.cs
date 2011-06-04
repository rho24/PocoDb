namespace PocoDb.ChangeTracking
{
    public class AddObjectChange
    {
        public object Object { get; private set; }

        public AddObjectChange(object o) {
            Object = o;
        }
    }
}