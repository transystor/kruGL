using kruGL.Abstractions;

namespace kruGL.OpenGL;

public sealed class GlTextureHandle : ITextureHandle
{
    public GlTextureHandle(uint id)
    {
        Id = id;
    }

    public uint Id { get; }

    public void Dispose()
    {
    }
}
