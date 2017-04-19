namespace OpenDataApplication.Core
{
    using DeJong.Utilities.Core;
    using DeJong.Utilities.Core.Collections;
    using DeJong.Utilities.Logging;
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    public sealed class ReflectionCache
    {
        public static readonly StreamingContext Context = new StreamingContext();

        private Type[] ctorTypes;
        private ConstructorInfo[] ctors;
        private SerializationInfo[] typeInfo;
        private static FormatterConverter formatConverter = new FormatterConverter();

        public ReflectionCache()
        {
            ctorTypes = new Type[] { typeof(SerializationInfo), typeof(StreamingContext) };
            ctors = new ConstructorInfo[0];
            typeInfo = new SerializationInfo[0];
        }

        public ConstructorInfo GetCtor(Type type)
        {
            ConstructorInfo result;
            if (!TryGetCtor(type, out result)) result = AddCtor(type);
            return result;
        }

        public SerializationInfo GetInfo<T>()
            where T : ISerializable, new()
        {
            SerializationInfo result;
            if (!TryGetInfo(typeof(T), out result)) result = AddInfo<T>();
            return result;
        }

        public void Clear()
        {
            ctors = new ConstructorInfo[0];
            typeInfo = new SerializationInfo[0];
        }

        private bool TryGetCtor(Type t, out ConstructorInfo result)
        {
            result = ctors.FirstOrDefault(c => c.DeclaringType == t);
            return result != null;
        }

        private ConstructorInfo AddCtor(Type t)
        {
            Log.Verbose(nameof(ReflectionCache), $"Adding constructor for type '{t.Name}' to cache");
            ConstructorInfo result = t.GetConstructor(ctorTypes);
            LoggedException.RaiseIf(result == null, $"Cannot find serializable constructor for type: '{t.Name}'");
            ctors = ctors.Concat(result);
            return result;
        }

        private bool TryGetInfo(Type t, out SerializationInfo result)
        {
            result = typeInfo.FirstOrDefault(i => i.ObjectType == t);
            return result != null;
        }

        private SerializationInfo AddInfo<T>()
            where T : ISerializable, new()
        {
            Log.Verbose(nameof(ReflectionCache), $"Adding field info for type '{typeof(T).Name}' to cache");
            SerializationInfo result = new SerializationInfo(typeof(T), formatConverter);
            new T().GetObjectData(result, Context);
            
            typeInfo = typeInfo.Concat(result);
            return result;
        }

        public SerializationInfo GetEmptyInfo<T>()
        {
            return new SerializationInfo(typeof(T), formatConverter);
        }
    }
}