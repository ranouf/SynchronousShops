using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace SynchronousShops.Libraries.Testing.Logging.Xunit
{
    public class XunitLogger<T> : XunitLogger, ILogger<T>
    {
        public XunitLogger(ITestOutputHelper output) : base(output, nameof(T))
        {
        }
    }
}
