using System.Runtime.InteropServices;

namespace kruGL.Native;

/// <summary>
/// Вспомогательные методы для преобразования сырых адресов функций
/// в типизированные .NET delegates.
/// </summary>
public static class NativeFunctionLoaderExtensions
{
    public static TDelegate LoadDelegate<TDelegate>(this INativeFunctionLoader loader, string functionName)
        where TDelegate : Delegate
    {
        var pointer = loader.LoadFunctionPointer(functionName);
        return Marshal.GetDelegateForFunctionPointer<TDelegate>(pointer);
    }
}
