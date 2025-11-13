#if LOG_SYSTEM_ENABLED
using DTT.ExtendedDebugLogs;
using Serilog.Core;
using Serilog.Events;

namespace Marmary.LogSystem.Runtime
{
    /// <summary>
    ///     Enriches Serilog events with an Extended Debug Logs tag.
    /// </summary>
    public sealed class UnityTagEnricher : ILogEventEnricher
    {
        /// <summary>
        ///     Property name used to store Unity tag references inside Serilog events.
        /// </summary>
        public const string UnityTagKey = "%_DO_NOT_USE_UNITY_TAG_DO_NOT_USE%";

        #region Fields

        /// <summary>
        ///     Cached log event property to avoid allocations on every enrichment.
        /// </summary>
        private readonly LogEventProperty _property;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Creates a new enricher that attaches <paramref name="tag" /> to log events.
        /// </summary>
        /// <param name="tag">Tag that will be added to emitted events.</param>
        public UnityTagEnricher(Tag tag)
        {
            _property = new LogEventProperty(UnityTagKey, new ScalarValue(tag));
        }

        #endregion

        #region ILogEventEnricher Members

        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(_property);
        }

        #endregion
    }
}
#endif