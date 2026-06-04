using kruGL.OpenGL;

namespace kruGL.Platform;

/// <summary>
/// Высокоуровневая оконная обёртка библиотеки.
///
/// Это façade над конкретным backend-ом. Снаружи потребитель работает с единым API,
/// а внутри можно подменять реализацию окна и context bootstrap-а.
/// Именно этот слой должен остаться стабильным, если позже GLFW будет заменён.
/// </summary>
public sealed class KruWindow : IDisposable
{
    private readonly IWindowBackend _backend;

    public KruWindow(WindowSettings settings, Func<WindowSettings, IWindowBackend> backendFactory)
    {
        _backend = backendFactory(settings);
        _backend.Load += OnLoad;
        _backend.Resize += OnResize;
        _backend.Render += OnRender;
    }

    public GlApi? Gl { get; private set; }

    public event Action? Load;
    public event Action<int, int>? Resize;
    public event Action<double>? Render;

    public void Run() => _backend.Run();

    public void Dispose() => _backend.Dispose();

    private void OnLoad()
    {
        Gl = _backend.CreateGlApi();
        Gl.Viewport(0, 0, _backend.Width, _backend.Height);
        Load?.Invoke();
    }

    private void OnResize(int width, int height)
    {
        Gl?.Viewport(0, 0, width, height);
        Resize?.Invoke(width, height);
    }

    private void OnRender(double deltaTime)
    {
        Render?.Invoke(deltaTime);
    }
}
