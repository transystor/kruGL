namespace kruGL.Native;

public interface INativeFunctionLoader
{
    nint LoadFunctionPointer(string functionName);
}
