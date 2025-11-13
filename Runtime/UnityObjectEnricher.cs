#if LOG_SYSTEM_ENABLED
using Serilog.Core;
using Serilog.Events;
using UnityEngine;

namespace Marmary.LogSystem.Runtime
{
    /// <summary>
    ///     Enriches Serilog events with a Unity context object.
    /// </summary>
    public sealed class UnityObjectEnricher : ILogEventEnricher
    {
        /// <summary>
        ///     Property name used to store Unity context references inside Serilog events.
        /// </summary>
        public const string UnityContextKey = "%_DO_NOT_USE_UNITY_ID_DO_NOT_USE%";

        #region Fields

        /// <summary>
        ///     Cached log event property to avoid allocations on every enrichment.
        /// </summary>
        private readonly LogEventProperty _property;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Creates a new enricher that attaches <paramref name="context" /> to log events.
        /// </summary>
        /// <param name="context">Unity object that will be added to events.</param>
        public UnityObjectEnricher(Object context)
        {
            _property = new LogEventProperty(UnityContextKey, new ScalarValue(context));
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