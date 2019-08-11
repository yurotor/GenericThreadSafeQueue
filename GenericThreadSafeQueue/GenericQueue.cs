using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericThreadSafeQueue
{
    public class GenericQueue
    {
        ConcurrentDictionary<Type, ConcurrentQueue<object>> _items = new ConcurrentDictionary<Type, ConcurrentQueue<object>>();

        public int Count => _items.Sum(i => i.Value.Count);

        public GenericQueue()
        {

        }

        public GenericQueue(IEnumerable<object> collection)
        {
            foreach (var item in collection)
            {
                Enqueue(item);
            }
        }

        public void Enqueue<T>(T item)
        {
            if (_items.TryGetValue(typeof(T), out var q))
            {
                q.Enqueue(item);
            }
            else
            {
                _items.TryAdd(typeof(T), new ConcurrentQueue<object>(new object[] { item }));
            }
        }

        public bool TryDequeue<T>(out T item)
        {
            item = default(T);
            if (_items.TryGetValue(typeof(T), out var q))
            {
                if(q.TryDequeue(out var i))
                {
                    item = (T)i;
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool TryPeek<T>(out T item)
        {
            item = default(T);
            if (_items.TryGetValue(typeof(T), out var q))
            {
                if (q.TryPeek(out var i))
                {
                    item = (T)i;
                    return true;
                }
                return false;
            }
            return false;
        }

        public T Dequeue<T>()
        {
            if (_items.TryGetValue(typeof(T), out var q))
            {
                q.TryDequeue(out var item);
                return (T)item;
            }
            throw new KeyNotFoundException($"Could not find key: {nameof(T)}");
        }        
        
    }
}
