#if LOG_SYSTEM_ENABLED
#nullable enable
using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace Marmary.LogSystem.Runtime
{
    /// <summary>
    ///     Provides extension methods to route Serilog events toward Unity logging.
    /// </summary>
    public static class UnitySinkExtensions
    {
        /// <summary>
        ///     The default output template used for debug logging in the Unity3D logger sink.
        ///     Provides a pre-defined format for rendering log messages, including message
        ///     content and exceptions, separated by new lines.
        /// </summary>
        private const string DefaultDebugOutputTemplate = "{Message:lj}{NewLine}{Exception}";

        #region Methods

        /// <summary>
        ///     Writes log events to <see cref="UnityEngine.ILogger" />. Defaults to <see cref="UnityEngine.Debug.unityLogger" />.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">
        ///     The minimum level for
        ///     events passed through the sink. Ignored when <paramref name="levelSwitch" /> is specified.
        /// </param>
        /// <param name="levelSwitch">
        ///     A switch allowing the pass-through minimum level
        ///     to be changed at runtime.
        /// </param>
        /// <param name="outputTemplate">
        ///     A message template describing the format used to write to the sink.
        ///     the default is <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</code>.
        /// </param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <remarks>
        ///     This overload accepts an output template and internally creates a <see cref="MessageTemplateTextFormatter" />.
        /// </remarks>
        public static LoggerConfiguration Unity3D(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultDebugOutputTemplate,
            IFormatProvider? formatProvider = null,
            LoggingLevelSwitch? levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (outputTemplate == null) throw new ArgumentNullException(nameof(outputTemplate));


            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.Unity3D(formatter, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        ///     Writes log events to <see cref="UnityEngine.ILogger" />. Defaults to <see cref="UnityEngine.Debug.unityLogger" />.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="formatter">
        ///     Controls the rendering of log events into text, for example to log JSON. To
        ///     control plain text formatting, use the overload that accepts an output template.
        /// </param>
        /// <param name="restrictedToMinimumLevel">
        ///     The minimum level for
        ///     events passed through the sink. Ignored when <paramref name="levelSwitch" /> is specified.
        /// </param>
        /// <param name="levelSwitch">
        ///     A switch allowing the pass-through minimum level
        ///     to be changed at runtime.
        /// </param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <remarks>
        ///     Use this overload when you need full control of the log rendering process, for example to format JSON payloads.
        /// </remarks>
        private static LoggerConfiguration Unity3D(
            this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch? levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));


            return sinkConfiguration.Sink(new Unity3DLogEventSink(formatter), restrictedToMinimumLevel, levelSwitch);
        }

        #endregion
    }
}
#endif