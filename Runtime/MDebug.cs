#if LOG_SYSTEM_ENABLED
#nullable enable
using System;
using DTT.ExtendedDebugLogs;
using Serilog;
using Object = UnityEngine.Object;

namespace Marmary.LogSystem.Runtime
{
    /// <summary>
    ///     Static access point for the Serilog logger configured for Unity.
    /// </summary>
    public static class MDebug
    {
        /// <summary>
        ///     Synchronization primitive guarding access to the shared logger instance.
        /// </summary>
        private static readonly object SyncRoot = new();

        /// <summary>
        ///     Backing field for <see cref="Instance" /> storing the current logger.
        /// </summary>
        private static ILogger _logger = CreateDefaultLogger();

        #region Properties

        /// <summary>
        ///     Gets the current logger instance.
        /// </summary>
        private static ILogger Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    return _logger;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Replaces the current logger with the specified instance.
        /// </summary>
        /// <param name="logger">Logger that will replace the current instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger" /> is null.</exception>
        public static void SetLogger(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            lock (SyncRoot)
            {
                SetLoggerInternal(logger);
            }
        }

        /// <summary>
        ///     Rebuilds the logger using the default configuration optionally mutated by <paramref name="configure" />.
        /// </summary>
        /// <param name="configure">Optional delegate that receives the default configuration prior to creation.</param>
        public static void Configure(Func<LoggerConfiguration, LoggerConfiguration>? configure = null)
        {
            lock (SyncRoot)
            {
                var configuration = BuildDefaultConfiguration();
                if (configure != null) configuration = configure(configuration);

                SetLoggerInternal(configuration.CreateLogger());
            }
        }

        /// <summary>
        ///     Logs an information level message using the default message template.
        /// </summary>
        /// <param name="message">Message that will be logged at information level.</param>
        public static void Log(string message)
        {
            Instance.Log(message);
        }

        /// <summary>
        ///     Logs an information level message using the default template enriched with the provided Unity context.
        /// </summary>
        /// <param name="message">Message that will be logged.</param>
        /// <param name="context">Unity object associated with the log entry.</param>
        public static void Log(string message, Object context)
        {
            Instance.Log(message, context);
        }

        /// <summary>
        ///     Logs an information level message using the default template enriched with the provided log tag.
        /// </summary>
        /// <param name="message">Message that will be logged.</param>
        /// <param name="tag">Tag that will be associated with the log entry.</param>
        public static void Log(string message, Tag tag)
        {
            Instance.Log(message, tag);
        }

        /// <summary>
        ///     Logs an information level message using the default template enriched with the provided Unity context and tag.
        /// </summary>
        /// <param name="message">Message that will be logged.</param>
        /// <param name="context">Unity object associated with the log entry.</param>
        /// <param name="tag">Tag that will be associated with the log entry.</param>
        public static void Log(string message, Object context, Tag tag)
        {
            Instance.Log(message, context, tag);
        }

        /// <summary>
        ///     Writes an information level log entry using the provided message template.
        /// </summary>
        /// <param name="messageTemplate">Message template rendered by Serilog.</param>
        /// <param name="propertyValues">Values that will be bound to the template.</param>
        public static void Information(string messageTemplate, params object?[] propertyValues)
        {
            Instance.Information(messageTemplate, propertyValues);
        }

        /// <summary>
        ///     Writes a warning level log entry using the provided message template.
        /// </summary>
        /// <param name="messageTemplate">Message template rendered by Serilog.</param>
        /// <param name="propertyValues">Values that will be bound to the template.</param>
        public static void Warning(string messageTemplate, params object?[] propertyValues)
        {
            Instance.Warning(messageTemplate, propertyValues);
        }

        /// <summary>
        ///     Writes an error level log entry including <paramref name="exception" /> and the provided message template.
        /// </summary>
        /// <param name="exception">Exception to attach to the log entry.</param>
        /// <param name="messageTemplate">Message template rendered by Serilog.</param>
        /// <param name="propertyValues">Values that will be bound to the template.</param>
        public static void Error(Exception exception, string messageTemplate, params object?[] propertyValues)
        {
            Instance.Error(exception, messageTemplate, propertyValues);
        }

        /// <summary>
        ///     Creates a logger enriched with the provided Unity context.
        /// </summary>
        /// <param name="context">Unity object associated with the logger.</param>
        /// <returns>Logger enriched with the supplied Unity context.</returns>
        public static ILogger ForContext(Object context)
        {
            return Instance.ForContext(context);
        }

        /// <summary>
        ///     Creates a logger enriched with the provided tag.
        /// </summary>
        /// <param name="tag">Tag associated with the logger.</param>
        /// <returns>Logger enriched with the supplied tag.</returns>
        public static ILogger WithUnityTag(Tag tag)
        {
            return Instance.WithUnityTag(tag);
        }

        /// <summary>
        ///     Builds the default logger configuration used when no custom configuration is supplied.
        /// </summary>
        /// <returns>A <see cref="LoggerConfiguration" /> pointing to the Unity sink.</returns>
        private static LoggerConfiguration BuildDefaultConfiguration()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Unity3D();
        }

        /// <summary>
        ///     Creates a logger instance from the default configuration.
        /// </summary>
        /// <returns>Serilog logger configured for Unity output.</returns>
        private static ILogger CreateDefaultLogger()
        {
            return BuildDefaultConfiguration().CreateLogger();
        }

        /// <summary>
        ///     Atomically updates the internal logger instance.
        /// </summary>
        /// <param name="logger">Logger that will replace the current instance.</param>
        private static void SetLoggerInternal(ILogger logger)
        {
            if (ReferenceEquals(_logger, logger)) return;

            if (_logger is IDisposable disposable) disposable.Dispose();

            _logger = logger;
        }

        #endregion
    }
}
#endif