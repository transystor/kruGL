namespace kruGL.Abstractions;

/// <summary>
/// Общий контракт для handle-а buffer-ресурса.
/// Это abstraction-слой, чтобы верхний код не зависел напрямую от конкретной реализации OpenGL-объекта.
/// </summary>
public interface IBufferHandle : IDisposable
{
    uint Id { get; }
}
