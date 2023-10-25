using System;

namespace ObjectOrientedDesign
{
    public static class Algorithms<T>
    {
        public static object Find(IIterator<T> iterator, Predicate<T> pred)
        {
            while (iterator.HasNext)
            {
                if (pred(iterator.Current.Obj))
                    return iterator.Current.Obj;
                iterator.Next();
            }

            return null;
        }

        public static void ForEach(IIterator<T> iterator, Action<T> func)
        {
            while (iterator.HasNext)
            {
                func(iterator.Current.Obj);
                iterator.Next();
            }
        }

        public static int CountIf(IIterator<T> iterator, Predicate<T> pred)
        {
            int count = 0;
            while (iterator.HasNext)
            {
                if (pred(iterator.Current.Obj))
                {
                    count++;
                }

                iterator.Next();
            }
            return count;
        }
    }
}