namespace RPG.Core
{
    public interface IEnumerableValueSetter<T> : IValueSetter<T>
    {
        void AddItem(T item);
        void ClearEnumerable();
    }
}