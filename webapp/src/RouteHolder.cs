using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace webapp
{
    public class RoutesHolder
    {
        public Regex Regex { get; }

        private readonly Dictionary<string, Action<RequestContext>> _methodToFunctionMap =
            new Dictionary<string, Action<RequestContext>>();

        public RoutesHolder(string regex)
        {
            Regex = new Regex(regex);
        }

        internal void MapMethodToFunction(string method, Action<RequestContext> function)
        {
            _methodToFunctionMap.Add(method, function);
        }

        internal void TryGetFunctionForMethod(string method, out Action<RequestContext> func)
        {
            _methodToFunctionMap.TryGetValue(method, out func);
        }
    }
}