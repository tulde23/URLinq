using System;
using System.Collections.Generic;
using System.Net.Http;
using URLinq.Contracts;

namespace URLinq.Models
{
    internal class ControllerAction : IControllerAction
    {
        private readonly List<string> _routeSegments = new List<string>();
        public string Controller { get; }
        public string ActionName { get; }
        public Type ReturnType { get; }
        public HttpMethod Method { get; }
        public List<IControllerActionParameter> ActionParameters { get; }

        public IReadOnlyCollection<string> RouteSegments
        {
            get
            {
                return _routeSegments;
            }
        }

        internal ControllerAction(string controller, string action, Type returnType, HttpMethod method)
        {
            Controller = controller;
            ActionName = action;
            ReturnType = returnType;
            ActionParameters = new List<IControllerActionParameter>();
            Method = method;
        }

        public void AddRouteSegment(string segment)
        {
            if (string.IsNullOrEmpty(segment))
            {
                throw new ArgumentNullException($"{nameof(segment)} can't be null.");
            }

            if (segment.StartsWith("/"))
            {
                segment = segment.Substring(1);
            }
            if (!_routeSegments.Contains(segment))
            {
                _routeSegments.Add(segment);
            }
        }
    }
}