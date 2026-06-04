using kruGL.Native;
using kruGL.OpenGL;

namespace kruGL.Platform;

/// <summary>
/// Абстракция над конкретным оконным backend-ом.
///
/// Смысл этого интерфейса в том, чтобы верхний platform API не зависел жёстко
/// от GLFW, Silk или будущей собственной реализации.
/// Backend обязан уметь:
/// - открыть окно,
/// - держать GL context,
/// - вернуть адреса OpenGL-функций,
/// - крутить render loop.
/// </summary>
public interface IWindowBackend : IDisposable, INativeFunctionLoader
{
    int Width { get; }
    int Height { get; }
    bool ShouldClose { get; }

    event Action? Load;
    event Action<int, int>? Resize;
    event Action<double>? Render;

    void Run();
    GlApi CreateGlApi();
}
