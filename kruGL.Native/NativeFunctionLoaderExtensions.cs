using System.Runtime.InteropServices;

namespace kruGL.Native;

public static class NativeFunctionLoaderExtensions
{
    public static TDelegate LoadDelegate<TDelegate>(this INativeFunctionLoader loader, string functionName)
        where TDelegate : Delegate
    {
        var pointer = loader.LoadFunctionPointer(functionName);
        return Marshal.GetDelegateForFunctionPointer<TDelegate>(pointer);
    }
}
