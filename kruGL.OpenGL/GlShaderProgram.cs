using kruGL.Abstractions;

namespace kruGL.OpenGL;

public sealed class GlShaderProgram : IShaderProgram
{
    public GlShaderProgram(uint id)
    {
        Id = id;
    }

    public uint Id { get; }

    public void Dispose()
    {
    }
}
