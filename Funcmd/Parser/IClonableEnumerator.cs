using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Parser
{
    public interface ICloneableEnumerator<T> : IEnumerator<T>, ICloneable
    {
        ICloneableEnumerator<T> CloneEnumerable();
    }

    public interface ICloneableEnumerable<T> : IEnumerable<T>
    {
        ICloneableEnumerator<T> CreateCloneableEnumerator();
    }

    public static class ClonableEnumerableExtensions
    {
        private class ListEnumerator<T> : ICloneableEnumerator<T>
        {
            private IList<T> list = null;
            private int index = -1;

            public ListEnumerator(IList<T> list)
            {
                this.list = list;
            }

            public ICloneableEnumerator<T> CloneEnumerable()
            {
                return new ListEnumerator<T>(list)
                {
                    index = index
                };
            }

            public T Current
            {
                get
                {
                    return list[index];
                }
            }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public bool MoveNext()
            {
                index++;
                return index >= 0 && index < list.Count;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public object Clone()
            {
                return CloneEnumerable();
            }
        }

        private class ListEnumerable<T> : ICloneableEnumerable<T>
        {
            private IList<T> list = null;

            public ListEnumerable(IList<T> list)
            {
                this.list = list;
            }

            public ICloneableEnumerator<T> CreateCloneableEnumerator()
            {
                return new ListEnumerator<T>(list);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return CreateCloneableEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return CreateCloneableEnumerator();
            }
        }

        public static ICloneableEnumerable<T> GetClonableEnumerable<T>(this IList<T> list)
        {
            return new ListEnumerable<T>(list);
        }

        public static ICloneableEnumerable<T> GetClonableEnumerable<T>(this IEnumerable<T> enumerable)
        {
            return new ListEnumerable<T>(enumerable.ToList());
        }
    }
}
