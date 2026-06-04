namespace kruGL.Abstractions;

/// <summary>
/// Общий контракт для texture handle.
/// Пока texture path ещё не развит, но интерфейс оставлен как часть будущего graphics abstraction слоя.
/// </summary>
public interface ITextureHandle : IDisposable
{
    uint Id { get; }
}
