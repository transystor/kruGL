namespace kruGL.Native;

public sealed class NativeFunctionNotFoundException : Exception
{
    public NativeFunctionNotFoundException(string functionName)
        : base($"Native function was not found: {functionName}")
    {
        FunctionName = functionName;
    }

    public string FunctionName { get; }
}
