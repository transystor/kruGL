using kruGL.Native;
using kruGL.OpenGL;

namespace kruGL.Platform;

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
