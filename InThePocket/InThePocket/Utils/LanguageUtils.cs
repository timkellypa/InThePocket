using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using System.Linq;

namespace InThePocket.Utils
{
    public static class LanguageUtils
    {
        public static List<Type> GetTypesInNamespace(string nameSpace)
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.MemberType == MemberTypes.TypeInfo).ToList();
        }
    }
}
