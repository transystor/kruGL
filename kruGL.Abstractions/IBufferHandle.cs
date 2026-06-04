namespace kruGL.Abstractions;

public interface IBufferHandle : IDisposable
{
    uint Id { get; }
}
