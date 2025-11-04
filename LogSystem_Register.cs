#if UNITY_EDITOR

using UnityEditor;

using MyCompany.ModuleSymbols;

namespace Marmary.LogSystem

{

    // Registro de símbolos para LogSystem
    // Este registro solo se ejecutará si MODULE_SYMBOLS_SYSTEM_ENABLED está definido

#if MODULE_SYMBOLS_SYSTEM_ENABLED

    [InitializeOnLoad]

    public static class LogSystem_Register

    {

        static LogSystem_Register()

        {

            var desc = new ModuleSymbolDescriptor

            {

                moduleName = "Log System",

                options = new SymbolOption[]

                {

                    new SymbolOption { symbol = "LOG_SYSTEM_ENABLED", description = "Habilita el sistema de logging con Serilog", enabledByDefault = false }

                }

            };

            ModuleSymbolRegistry.Register(desc);

        }

    }

#endif

}

#endif

