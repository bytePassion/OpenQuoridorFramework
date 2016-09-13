using System;
using System.Collections;
using System.Collections.Generic;
using Lib.FrameworkExtension;

namespace Lib.Utils
{
	public static class Guard
    {

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////                                                                                   ///////////
        /////////                                   Not null                                        ///////////
        /////////                                                                                   ///////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void ArgumentNotNull(object argumentValue)
        {
            if (argumentValue == null)
            {
                var valueType = argumentValue.GetType();
                throw new ArgumentNullException(valueType.Name);
            }
        }

        public static void ArgumentNotNull(params object[] argumentValues)
        {
            argumentValues.Do(ArgumentNotNull);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////                                                                                   ///////////
        /////////                                   casting                                         ///////////
        /////////                                                                                   ///////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        public static T SafeCast<T>(object instance) where T : class
        {
            var instanceAsT = instance as T;
            ArgumentNotNull(instanceAsT);
            return instanceAsT;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////                                                                                   ///////////
        /////////                                 Dictionaries                                      ///////////
        /////////                                                                                   ///////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void KeyInDictionary(IDictionary dictionary, object key)
        {
            ArgumentNotNull(key, dictionary);

            if (!dictionary.Contains(key))
            {
                var valueType = dictionary.GetType();
                throw new KeyNotFoundException($"{key} is not found in {valueType.Name}");
            }
        }

        public static void KeyNotInDictionary(IDictionary dictionary, object key)
        {
            ArgumentNotNull(key, dictionary);

            if (dictionary.Contains(key))
            {
                var valueType = dictionary.GetType();
                throw new ArgumentException($"{key} is already defined in {valueType.Name}");
            }
        }

        public static void KeyInDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            ArgumentNotNull(key, dictionary);

            if (!dictionary.ContainsKey(key))
            {
                var valueType = dictionary.GetType();
                throw new KeyNotFoundException($"{key} is not found in {valueType.Name}");
            }
        }

        public static void KeyNotInDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            ArgumentNotNull(key, dictionary);

            if (dictionary.ContainsKey(key))
            {
                var valueType = dictionary.GetType();
                throw new ArgumentException($"{key} is already defined in {valueType.Name}");
            }
        }
    }
}
