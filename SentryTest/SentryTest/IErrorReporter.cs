using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SentryTest
{
    interface IErrorReporter
    {
        Task CaptureAsync(Exception exception);

        Task CaptureAsync(string message);
    }
}
