#if LOG_SYSTEM_ENABLED
using DTT.ExtendedDebugLogs;
using UnityEngine;
using ILogger = Serilog.ILogger;

namespace Marmary.LogSystem.Runtime
{
    /// <summary>
    ///     Provides convenience methods to integrate Serilog with Unity specific concepts.
    /// </summary>
    public static class ILoggerExtensions
    {
        #region Methods

        /// <summary>
        ///     Add <see cref="UnityEngine.Object" /> context for the log.
        /// </summary>
        /// <param name="logger">Original logger</param>
        /// <param name="context"> <see cref="UnityEngine.Object" /> context for the log</param>
        public static ILogger ForContext(this ILogger logger, Object context)
        {
            return logger.ForContext(new UnityObjectEnricher(context));
        }

        /// <summary>
        ///     Add Unity tag for the log
        /// </summary>
        /// <param name="logger">Original logger</param>
        /// <param name="tag">Unity log tag</param>
        public static ILogger WithUnityTag(this ILogger logger, Tag tag)
        {
            return logger.ForContext(new UnityTagEnricher(tag));
        }

        /// <summary>
        ///     Logs a message enriched with the provided context and tag.
        /// </summary>
        /// <param name="logger">Logger that will emit the entry.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="contex">Unity context associated with the entry.</param>
        /// <param name="tag">Unity tag associated with the entry.</param>
        public static void Log(this ILogger logger, string message, Object contex, Tag tag)
        {
            logger.WithUnityTag(tag).ForContext(contex).Information("{Message}", message);
        }

        /// <summary>
        ///     Logs a message enriched with the provided context.
        /// </summary>
        /// <param name="logger">Logger that will emit the entry.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="contex">Unity context associated with the entry.</param>
        public static void Log(this ILogger logger, string message, Object contex)
        {
            logger.ForContext(contex).Information("{Message}", message);
        }

        /// <summary>
        ///     Logs a message using the default template.
        /// </summary>
        /// <param name="logger">Logger that will emit the entry.</param>
        /// <param name="message">Message to log.</param>
        public static void Log(this ILogger logger, string message)
        {
            logger.Information("{Message}", message);
        }

        /// <summary>
        ///     Logs a message enriched with the provided tag.
        /// </summary>
        /// <param name="logger">Logger that will emit the entry.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="tag">Unity tag associated with the entry.</param>
        public static void Log(this ILogger logger, string message, Tag tag)
        {
            logger.WithUnityTag(tag).Information("{Message}", message);
        }

        #endregion
    }
}
#endif