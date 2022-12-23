using Microsoft.Extensions.Logging;
using SynchronousShops.Libraries.Testing.Logging.Xunit;
using System;
using Xunit.Abstractions;

namespace SynchronousShops.Libraries.Testing.Logging.Xunit
{
    public class XunitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XunitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
            => new XunitLogger(_testOutputHelper, categoryName);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //nothing
            }
        }
    }
}
