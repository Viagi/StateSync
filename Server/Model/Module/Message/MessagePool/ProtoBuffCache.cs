using System;
using System.Collections.Generic;

namespace ETModel
{
    public static class ProtoBuffCache<T> where T : IMessage
    {
        private static readonly Queue<T> Caches = new Queue<T>();

        public static T Create()
        {
            return Caches.Count > 0 ? Caches.Dequeue() : (T)Activator.CreateInstance(typeof(T));
        }

        public static void Recycle(T obj)
        {
            Caches.Enqueue(obj);
        }
    }
}
