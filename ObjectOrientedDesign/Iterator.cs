namespace ObjectOrientedDesign
{
    public interface IIterator<T>
    {
        T First();
        T Next();
        bool HasNext { get; }
        BinaryTree<T>.Node Current { get; }
    }


    public interface IAbstractList<T>
    {
        IIterator<T> CreateIterator();
        IIterator<T> CreateRevIterator();
    }
}