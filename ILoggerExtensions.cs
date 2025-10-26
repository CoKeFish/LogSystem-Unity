using DTT.ExtendedDebugLogs;

namespace Serilog
{
    public static class ILoggerExtensions
    {
        /// <summary>
        /// Add <see cref="UnityEngine.Object"/> context for the log.
        /// </summary>
        /// <param name="logger">Original logger</param>
        /// <param name="context"> <see cref="UnityEngine.Object"/> context for the log</param>
        public static ILogger ForContext(this ILogger logger, UnityEngine.Object context) => logger.ForContext(new UnityObjectEnricher(context));

        /// <summary>
        /// Add Unity tag for the log
        /// </summary>
        /// <param name="logger">Original logger</param>
        /// <param name="tag">Unity log tag</param>
        public static ILogger WithUnityTag(this ILogger logger, Tag tag) => logger.ForContext(new UnityTagEnricher(tag));

        public static void Log(this ILogger logger, string message, UnityEngine.Object contex, Tag tag)
        {
            logger.WithUnityTag(tag).ForContext(contex).Information("{Message}", message);
        }
        
        public static void Log(this ILogger logger, string message, UnityEngine.Object contex)
        {
            logger.ForContext(contex).Information("{Message}", message);
        }
        
        public static void Log(this ILogger logger, string message)
        {
            logger.Information("{Message}", message);
        }
        
        public static void Log(this ILogger logger, string message, Tag tag)
        {
            logger.WithUnityTag(tag).Information("{Message}", message);
        }
    }
}
