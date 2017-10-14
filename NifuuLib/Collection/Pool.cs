using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Collection
{
    public class Pool<T> where T : class, IPoolable, new()
    {
        //protected ConcurrentBag<T> _Objects;
        protected T[] _Objects;

        public Pool(int capacity, bool setInUse = false)
        {
            _Objects = new T[capacity]; // ConcurrentBag<T>();

            for (var i = 0; i < capacity; i++)
            {
                T t = new T();
                t.InUse = setInUse;
                _Objects[i] = t;
                //PutObject(t);
            }
        }

        public void IterateAction(Action<T> Action)
        {
            Parallel.ForEach(_Objects, (obj) =>
            {
                if (!obj.InUse)
                {
                    obj.InUse = true;
                    obj.Initialise();
                }

                Action(obj);
            });
        }

        public void IterateActionSync(Action<T> Action)
        {
            //lock (_Objects)
            //{ 
                for (var i = 0; i < _Objects.Count(); i++)
                {
                    var obj = _Objects[i];
                    if (!obj.InUse)
                    {
                        obj.InUse = true;
                        obj.Initialise();
                    }

                    Action(obj);
                }
            //}
        }

        public T GetObject()
        {
            while (true)
            {
                foreach (var item in _Objects)
                {
                    if (!item.InUse)
                    {
                        return item;
                    }
                }
            }
        }

        public void PutObject(T item)
        {
            while (true)
            {
                for (var i = 0; i < _Objects.Count(); i++)
                {
                    if (!_Objects[i].InUse)
                    {
                        _Objects[i] = item;
                    }
                }
            }
        }
    }
}
