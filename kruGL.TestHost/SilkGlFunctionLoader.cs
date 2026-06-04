using Silk.NET.OpenGL;
using kruGL.Native;

namespace kruGL.TestHost;

internal sealed class SilkGlFunctionLoader : INativeFunctionLoader
{
    private readonly GL _gl;

    public SilkGlFunctionLoader(GL gl)
    {
        _gl = gl;
    }

    public nint LoadFunctionPointer(string functionName)
    {
        var pointer = _gl.Context!.TryGetProcAddress(functionName, out var address)
            ? address
            : nint.Zero;

        if (pointer == nint.Zero)
        {
            throw new NativeFunctionNotFoundException(functionName);
        }

        return pointer;
    }
}
