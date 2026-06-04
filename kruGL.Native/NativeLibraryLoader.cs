using System.Runtime.InteropServices;

namespace kruGL.Native;

public sealed class NativeLibraryLoader : INativeFunctionLoader, IDisposable
{
    private readonly nint _libraryHandle;
    private bool _disposed;

    public NativeLibraryLoader(string libraryName)
    {
        _libraryHandle = NativeLibrary.Load(libraryName);
    }

    public nint LoadFunctionPointer(string functionName)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!NativeLibrary.TryGetExport(_libraryHandle, functionName, out var functionPointer) || functionPointer == nint.Zero)
        {
            throw new NativeFunctionNotFoundException(functionName);
        }

        return functionPointer;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        NativeLibrary.Free(_libraryHandle);
        _disposed = true;
    }
}
