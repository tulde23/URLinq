using System;
using System.Linq.Expressions;
using System.Net.Http;
using URLinq.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace URLinq
{
    /// <summary>
    /// A utility class for interacting with routes
    /// </summary>
    public static class RouteHelper
    {
        /// <summary>
        /// Builds the request message
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static HttpRequestMessage BuildRequestMessage<TController, TResponse>(
          Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException(nameof(expression));
            }
            var controllerAction = ControllerActionFactory.GetAction(expression);
            var route = ControllerActionRouteFactory.CreateRoute(controllerAction);
            return route.BuildRequestMessage(controllerAction);
        }

        /// <summary>
        /// Creates a route instance
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static IControllerActionRoute GetRoute<TController, TResponse>(
          Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException(nameof(expression));
            }
            var controllerAction = ControllerActionFactory.GetAction(expression);
            var route = ControllerActionRouteFactory.CreateRoute(controllerAction);
            return route;
        }
    }
}