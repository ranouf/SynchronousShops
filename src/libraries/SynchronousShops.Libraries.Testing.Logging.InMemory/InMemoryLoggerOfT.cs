using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SynchronousShops.Libraries.Testing.Logging.InMemory
{
    public class InMemoryLogger<T> : InMemoryLogger, ILogger<T>
    {
        public InMemoryLogger(List<string> output) : base(output, nameof(T))
        {
        }
    }
}
