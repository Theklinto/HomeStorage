using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage
{
    public enum EStorageKey
    {
        Undefined = 0,

        //User data
        /// <summary>
        /// Storagetype <see cref="string"/>
        /// </summary>
        [StorageType(typeof(string))]
        Username = 1,
        /// <summary>
        /// Storagetype <see cref="string"/>
        /// </summary>
        [StorageType(typeof(string))]
        Email = 2,
        /// <summary>
        /// Storagetype <see cref="string"/>
        /// </summary>
        [StorageType(typeof(string))]
        Password = 3,
        /// <summary>
        /// Storagetype <see cref="string"/>
        /// </summary>
        [StorageType(typeof(string))]
        JwtToken = 20,
        /// <summary>
        /// Storagetype <see cref="DateTime"/>
        /// </summary>
        [StorageType(typeof(string))]
        JwtTokenExpiration = 21
    }
    public static class SecureStorageExtension
    {
        public static void Remove(EStorageKey key) => Preferences.Default.Remove(key.ToString());

        public static string GetString(EStorageKey key) => Get<string>(key);
        public static DateTime GetDate(EStorageKey key)
        {
            DateTime.TryParse(Get<string>(key), out DateTime result);
            return result;
        }
        private static T? Get<T>(EStorageKey key)
        {
            T result = Preferences.Default.Get(key.ToString(), default(T));
            return result;
        }


        public static void Set(EStorageKey key, string value) => Set<string>(key, value ?? string.Empty);
        //Seems datetime Storage is broken, made a wrapper
        public static void Set(EStorageKey key, DateTime? value) => Set<string>(key, value?.ToString() ?? string.Empty);
        //Private method for overloading on type base
        private static void Set<T>(EStorageKey key, T value)
        {
            Type storageType = key.GetStorageType();
            if (typeof(T) != storageType)
                throw new InvalidStorageTypeException($"Storage key ({key}) expected ({key.GetStorageType().Name}) but got ({typeof(T).Name})");
            Preferences.Default.Set(key.ToString(), value);
        }

        public static Type GetStorageType(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString()).First();

            if (memberInfo == null || !memberInfo.CustomAttributes.Any())
                return typeof(object);

            StorageType? storageType = memberInfo.GetCustomAttribute<StorageType>();

            return storageType?.Type ?? typeof(object);
        }

        public class InvalidStorageTypeException : Exception
        {
            public InvalidStorageTypeException()
            {
            }

            public InvalidStorageTypeException(string message) : base(message)
            {
            }

            public InvalidStorageTypeException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected InvalidStorageTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class StorageType : Attribute
    {
        public Type Type { get; set; }
        public StorageType(Type type)
        {
            Type = type;
        }
    }
}
