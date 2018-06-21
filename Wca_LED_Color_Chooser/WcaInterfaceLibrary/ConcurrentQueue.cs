using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WcaInterfaceLibrary
{
    class ConcurrentQueue<T> 
    {
        private readonly Object lockobj = new Object();
        private Queue<T> _queue;

        public ConcurrentQueue()
        {
            _queue = new Queue<T>();
        }
        
        public void Clear()
        {
            lock(lockobj)
            {
                _queue.Clear();
            }
        }

        public void Enqueue(T item)
        {
            lock (lockobj)
            {
                _queue.Enqueue(item);
            }
        }

        public T Dequeue()
        {
            T ret;
            lock (lockobj)
            {
                ret = _queue.Dequeue();
            }
            return ret;
        }

        public int Count
        {
            get
            {
                lock (lockobj)
                {
                    return _queue.Count;
                }
            }
        }
    }
}
