using kruGL.OpenGL;
using kruGL.Platform;
using kruGL.Platform.Glfw;

using var window = new KruWindow(
    new WindowSettings
    {
        Title = "kruGL TestHost",
        Width = 960,
        Height = 540
    },
    settings => new GlfwWindowBackend(settings));

window.Render += _ =>
{
    if (window.Gl is null)
    {
        return;
    }

    window.Gl.ClearColor(0.08f, 0.12f, 0.18f, 1.0f);
    window.Gl.Clear(GlClearMask.ColorBufferBit);
};

window.Run();
