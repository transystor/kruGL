namespace kruGL.Abstractions;

/// <summary>
/// Общий контракт для shader program handle.
/// Нужен как верхнеуровневая абстракция на случай, если библиотека позже получит несколько graphics-платформ или реализаций.
/// </summary>
public interface IShaderProgram : IDisposable
{
    uint Id { get; }
}
