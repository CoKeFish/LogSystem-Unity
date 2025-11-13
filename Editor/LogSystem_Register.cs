#if UNITY_EDITOR

using Marmary.Utils.Editor.ModuleSymbols;
using UnityEditor;

namespace Marmary.LogSystem.Editor

{
    // Symbol registration for LogSystem.
    // Only runs when MODULE_SYMBOLS_SYSTEM_ENABLED is defined.

#if MODULE_SYMBOLS_SYSTEM_ENABLED

    /// <summary>
    ///     Static class responsible for registering symbols for the LogSystem.
    ///     This code path runs only when the <c>MODULE_SYMBOLS_SYSTEM_ENABLED</c> symbol is defined.
    /// </summary>
    [InitializeOnLoad]
    public static class LogSystemRegister

    {
        #region Constructors and Injected

        static LogSystemRegister()

        {
            var desc = new ModuleSymbolDescriptor
            {
                ModuleName = "Log System",
                Options = new[]
                {
                    new SymbolOption
                    {
                        symbol = "LOG_SYSTEM_ENABLED",
                        description = "Enables the logging system powered by Serilog.",
                        enabledByDefault = false
                    }
                }
            };

            ModuleSymbolRegistry.Register(desc);
        }

        #endregion
    }

#endif
}

#endif