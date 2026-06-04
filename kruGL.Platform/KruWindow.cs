using kruGL.OpenGL;

namespace kruGL.Platform;

/// <summary>
/// Высокоуровневая оконная обёртка библиотеки.
///
/// Это façade над конкретной платформенной реализацией. Снаружи потребитель работает с единым API,
/// а внутри можно подменять реализацию окна и context bootstrap-а.
/// Именно этот слой должен остаться стабильным, если позже GLFW будет заменён.
/// </summary>
public sealed class KruWindow : IDisposable
{
    private readonly IWindowPlatform _platform;

    public KruWindow(WindowSettings settings, Func<WindowSettings, IWindowPlatform> platformFactory)
    {
        _platform = platformFactory(settings);
        _platform.Load += OnLoad;
        _platform.Resize += OnResize;
        _platform.Render += OnRender;
    }

    public GlApi? Gl { get; private set; }

    public event Action? Load;
    public event Action<int, int>? Resize;
    public event Action<double>? Render;

    public void Run() => _platform.Run();

    public void Dispose() => _platform.Dispose();

    private void OnLoad()
    {
        Gl = _platform.CreateGlApi();
        Gl.Viewport(0, 0, _platform.Width, _platform.Height);
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
