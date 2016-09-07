using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OQF.GameEngine.Loader
{
	public static class TypeLoaderExtension
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types.Where(t => t != null);
            }
        }
    }
}
