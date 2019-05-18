namespace Shelve.Core
{
    using System;
    using System.Text;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal static class MathWrapper
    {
        private static Dictionary<string, MethodInfo> reflectedMethods;

        static MathWrapper()
        {
            reflectedMethods = new Dictionary<string, MethodInfo>();

            ReflectMethods<double>(typeof(Math));
        }

        public static IFunctor GetFunctorFor(string name) => new LibMethodWrapper(reflectedMethods[name]);

        private static void ReflectMethods<T>(Type targetLib)
        {
            var bindingFlags = BindingFlags.Static | BindingFlags.Public;
            var reflected = targetLib.GetMethods(bindingFlags);

            foreach (var method in reflected)
            {
                var isTypeClear = true;

                if (method.ReturnType != typeof(T))
                {
                    continue;
                }

                foreach (var parameter in method.GetParameters())
                {
                    if (parameter.ParameterType != typeof(T))
                    {
                        isTypeClear = false;
                        break;
                    }
                }

                if (isTypeClear)
                {
                    try
                    {
                        reflectedMethods.Add(method.Name, method);
                    }
                    catch (ArgumentException)
                    {
                        reflectedMethods[method.Name] = method;
                    }
                }
            }
        }

        public static Regex GetMethodsRegex(RegexOptions options)
        {
            var regularExpression = new StringBuilder();
            regularExpression.Append(@"^(");

            foreach (var method in reflectedMethods.Values)
            {
                regularExpression.Append($"{method.Name.ToLower()}|");
            }

            regularExpression.Remove(regularExpression.Length - 1, 1);
            regularExpression.Append(@")");

            return new Regex(regularExpression.ToString(), options);
        }
    }
}
