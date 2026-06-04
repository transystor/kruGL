using kruGL.Abstractions;

namespace kruGL.OpenGL;

public sealed class GlBufferHandle : IBufferHandle
{
    public GlBufferHandle(uint id)
    {
        Id = id;
    }

    public uint Id { get; }

    public void Dispose()
    {
    }
}
