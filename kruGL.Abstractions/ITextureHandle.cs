namespace kruGL.Abstractions;

public interface ITextureHandle : IDisposable
{
    uint Id { get; }
}
