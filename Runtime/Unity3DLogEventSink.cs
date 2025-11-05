#if LOG_SYSTEM_ENABLED
#nullable enable
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;
using UnityEngine;

#if EXTENDED_DEBUG_LOGS_ASSET_STORE_RELEASE
using DTT.ExtendedDebugLogs;
#endif

namespace Serilog.Sinks.Unity3D
{
    public sealed class Unity3DLogEventSink : ILogEventSink
    {
        private readonly ITextFormatter _formatter;

        public Unity3DLogEventSink(ITextFormatter formatter)
        {
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            using var buffer = new StringWriter();

            _formatter.Format(logEvent, buffer);
            var logType = logEvent.Level switch
            {
                LogEventLevel.Verbose or LogEventLevel.Debug or LogEventLevel.Information => LogType.Log,
                LogEventLevel.Warning => LogType.Warning,
                LogEventLevel.Error or LogEventLevel.Fatal => LogType.Error,
                _ => throw new ArgumentOutOfRangeException(nameof(logEvent.Level), "Unknown log level"),
            };

            object message = buffer.ToString().Trim();

            UnityEngine.Object? unityContext = null;
            if (logEvent.Properties.TryGetValue(UnityObjectEnricher.UnityContextKey, out var contextPropertyValue) &&
                contextPropertyValue is ScalarValue contextScalarValue)
            {
                unityContext = contextScalarValue.Value as UnityEngine.Object;
            }

#if EXTENDED_DEBUG_LOGS_ASSET_STORE_RELEASE
            Tag? unityTag = null;
            if (logEvent.Properties.TryGetValue(UnityTagEnricher.UnityTagKey, out var tagPropertyValue) &&
                tagPropertyValue is ScalarValue tagScalarValue)
            {
                unityTag = tagScalarValue.Value as Tag;
            }
#else
            object? unityTag = null;
            if (logEvent.Properties.TryGetValue(UnityTagEnricher.UnityTagKey, out var tagPropertyValue) &&
                tagPropertyValue is ScalarValue tagScalarValue)
            {
                unityTag = tagScalarValue.Value;
            }
#endif

            switch (logType)
            {
#if EXTENDED_DEBUG_LOGS_ASSET_STORE_RELEASE
                case LogType.Error:
                {
                    if (unityContext != null)
                    {
                        if (unityTag != null)
                        {
                            DebugEx.LogError(message, unityContext, unityTag);
                        }
                        else
                        {
                            DebugEx.LogError(message, unityContext);
                        }
                    }
                    else if (unityTag != null)
                    {
                        DebugEx.LogError(message, unityTag);
                    }
                    else
                    {
                        DebugEx.LogError(message);
                    }
                }

                    break;
                case LogType.Warning:
                {
                    if (unityContext != null)
                    {
                        if (unityTag != null)
                        {
                            DebugEx.LogWarning(message, unityContext, unityTag);
                        }
                        else
                        {
                            DebugEx.LogWarning(message, unityContext);
                        }
                    }
                    else if (unityTag != null)
                    {
                        DebugEx.LogWarning(message, unityTag);
                    }
                    else
                    {
                        DebugEx.LogWarning(message);
                    }
                }

                    break;
                case LogType.Log:
                {
                    if (unityContext != null)
                    {
                        if (unityTag != null)
                        {
                            DebugEx.Log(message, unityContext, unityTag);
                        }
                        else
                        {
                            DebugEx.Log(message, unityContext);
                        }
                    }
                    else if (unityTag != null)
                    {
                        DebugEx.Log(message, unityTag);
                    }
                    else
                    {
                        DebugEx.Log(message);
                    }
                }

                    break;


                case LogType.Exception:
                {
                    if (logEvent.Exception is { } exception)
                    {
                        if (unityContext != null)
                        {
                            if (unityTag != null)
                            {
                                DebugEx.LogException(exception, unityContext, unityTag);
                            }
                            else
                            {
                                DebugEx.LogException(exception, unityContext);
                            }
                        }
                        else if (unityTag != null)
                        {
                            DebugEx.LogException(exception, unityTag);
                        }
                        else
                        {
                            DebugEx.LogException(exception);
                        }
                    }
                }

                    break;
#else
                case LogType.Error:
                {
                    if (unityContext != null)
                    {
                        if (unityTag != null)
                        {
                            UnityEngine.Debug.LogError(message);
                        }
                        else
                        {
                            UnityEngine.Debug.LogError(message, unityContext);
                        }
                    }
                    else if (unityTag != null)
                    {
                        UnityEngine.Debug.LogError(message);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError(message);
                    }
                }

                    break;
                case LogType.Warning:
                {
                    if (unityContext != null)
                    {
                        if (unityTag != null)
                        {
                            UnityEngine.Debug.LogWarning(message);
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning(message, unityContext);
                        }
                    }
                    else if (unityTag != null)
                    {
                        UnityEngine.Debug.LogWarning(message);
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning(message);
                    }
                }

                    break;
                case LogType.Log:
                {
                    if (unityContext != null)
                    {
                        if (unityTag != null)
                        {
                            UnityEngine.Debug.Log(message);
                        }
                        else
                        {
                            UnityEngine.Debug.Log(message, unityContext);
                        }
                    }
                    else if (unityTag != null)
                    {
                        UnityEngine.Debug.Log(message);
                    }
                    else
                    {
                        UnityEngine.Debug.Log(message);
                    }
                }

                    break;


                case LogType.Exception:
                {
                    if (logEvent.Exception is { } exception)
                    {
                        if (unityContext != null)
                        {
                            if (unityTag != null)
                            {
                                UnityEngine.Debug.LogException(exception);
                            }
                            else
                            {
                                UnityEngine.Debug.LogException(exception, unityContext);
                            }
                        }
                        else if (unityTag != null)
                        {
                            UnityEngine.Debug.LogException(exception);
                        }
                        else
                        {
                            UnityEngine.Debug.LogException(exception);
                        }
                    }
                }

                    break;
#endif
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
#endif