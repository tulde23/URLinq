using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// A fixrtur
    /// </summary>
    public abstract class AbstractFixture<TEntryPoint> where TEntryPoint:class
    {
        /// <summary>
        /// Gets the factroy.
        /// </summary>
        /// <value>
        /// The factroy.
        /// </value>
        public WebApplicationFactory<TEntryPoint> Factroy { get; private set; }
        public ITestOutputHelper Logger { get; private set; }
        /// <summary>
        /// Sets the factory.
        /// </summary>
        /// <param name="webBuilderFactory">The web builder factory.</param>
        public void SetFactory(WebApplicationFactory<TEntryPoint> webBuilderFactory)
        {
            Factroy = webBuilderFactory;
        }
        /// <summary>
        /// Sets the logger.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public void SetLogger(ITestOutputHelper testOutputHelper)
        {
            Logger = testOutputHelper;
        }
    }
}
