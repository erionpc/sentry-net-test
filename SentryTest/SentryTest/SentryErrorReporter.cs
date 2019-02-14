using Microsoft.Extensions.Options;
using SharpRaven.Core;
using SharpRaven.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SentryTest
{
    public class SentryErrorReporter : IErrorReporter
    {
        private readonly IRavenClient _client;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SentryErrorReporter" /> class.
        /// </summary>
        /// <param name="dsn">The dsn.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     options
        ///     or
        ///     Can not construct a SentryErrorReporter without a valid DSN!
        /// </exception>
        public SentryErrorReporter(string dsn)
        {
            if (string.IsNullOrEmpty(dsn))
                throw new ArgumentNullException("Can not construct a SentryErrorReporter without a valid DSN!");

            _client = new RavenClient(dsn);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SentryErrorReporter" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     options
        ///     or
        ///     Can not construct a SentryErrorReporter without a valid DSN!
        /// </exception>
        public SentryErrorReporter(IOptions<SentrySetup> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(options.Value.Dsn))
                throw new ArgumentNullException("Can not construct a SentryErrorReporter without a valid DSN!");

            _client = new RavenClient(options.Value.Dsn);
        }

        /// <summary>
        ///     Captures the specified exception asynchronously and hands it off to an error handling service.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">exception</exception>
        public async Task CaptureAsync(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            await _client.CaptureAsync(new SentryEvent(exception));
        }

        /// <summary>
        ///     Captures the specified message asynchronously and hands it off to an error handling service.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">message</exception>
        public async Task CaptureAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            await _client.CaptureAsync(new SentryEvent(new SentryMessage(message, new {
                event_type = "my event type",
                tags = new Dictionary<object, object>() { { "app", "015" }, { "captureError", "custom" }, { "authId", "1" }, {"username", "user" } },
                date = DateTime.Now,
                time_spent = 221
            })));
        }
    }
}
