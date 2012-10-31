using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenPadMvvm.Core
{
    public class MultiDictionary<T, K> : Dictionary<T, List<K>>
    {
        private void EnsureKeyExists(T key)
        {
            if (!ContainsKey(key))
            {
                this[key] = new List<K>();
            }
            else
            {
                if (this[key] == null)
                {
                    this[key] = new List<K>();
                }
            }
        }

        public void AddValue(T key, K newItem)
        {
            EnsureKeyExists(key);
            this[key].Add(newItem);
        }
    }
}
