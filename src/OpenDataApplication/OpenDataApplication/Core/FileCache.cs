namespace OpenDataApplication.Core
{
    using Mentula.Utilities.Logging;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class FileCache
    {
        private Dictionary<Type, IList> data;

        public FileCache()
        {
            data = new Dictionary<Type, IList>();
        }

        public bool TryGetPool<T>(out List<T> result)
        {
            Type key = typeof(T);
            if (data.ContainsKey(key))
            {
                result = (List<T>)data[key];
                return true;
            }
            else
            {
                result = new List<T>();
                return false;
            }
        }

        public void AddPool<T>(List<T> fromFile, bool overridePool)
        {
            Type key = typeof(T);
            if (data.ContainsKey(key))
            {
                if (overridePool)
                {
                    Log.Verbose(nameof(FileCache), $"Overriding cache pool {key.Name}, {((IList<T>)data[key]).Count} to {fromFile.Count} entries");
                    data[key] = fromFile;
                }
            }
            else
            {
                Log.Verbose(nameof(FileCache), $"Adding cache pool {key.Name}({fromFile.Count} entries)");
                data.Add(key, fromFile);
            }
        }

        public void Clear()
        {
            data.Clear();
        }
    }
}