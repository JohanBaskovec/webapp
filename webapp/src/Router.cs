using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace webapp
{
    public class Router
    {
        private readonly Dictionary<string, RoutesHolder> _routes = new Dictionary<string, RoutesHolder>();

        public void AddGetRoute(string regex, Action<RequestContext> function)
        {
            AddRoute("GET", regex, function);
        }
        
        public void AddPostRoute(string regex, Action<RequestContext> function)
        {
            AddRoute("POST", regex, function);
        }

        public void AddRoute(string method, string regex, Action<RequestContext> function)
        {
            if (_routes.ContainsKey(regex))
            {
                _routes[regex].MapMethodToFunction(method, function);
            }
            else
            {
                RoutesHolder routesHolder = new RoutesHolder(regex);
                routesHolder.MapMethodToFunction(method, function);
                _routes.Add(regex, routesHolder);
            }
        }
        
        public void RouteRequestToMethod(RequestContext requestContext)
        {
            HttpRequest request = requestContext.Request;
            
            foreach (string regex in _routes.Keys)
            {
                RoutesHolder routesHolder = _routes[regex];

                Regex compiledRegex = routesHolder.Regex;
                MatchCollection matches = compiledRegex.Matches(request.Url.AbsolutePath);
                if (matches.Count != 0)
                {
                    string method = request.HttpMethod;
                    Action<RequestContext> func;
                    routesHolder.TryGetFunctionForMethod(method, out func);
                    if (func != null)
                    {
                        func.Invoke(requestContext);
                        return;
                    }

                }
            }

            requestContext.Response.StatusCode = 404;
        }
    }
}