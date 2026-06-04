using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using kruGL.OpenGL;
using kruGL.TestHost;

var options = WindowOptions.Default;
options.Title = "kruGL TestHost";
options.Size = new Vector2D<int>(960, 540);

using var window = Window.Create(options);

GL? silkGl = null;
GlApi? gl = null;

window.Load += () =>
{
    silkGl = GL.GetApi(window);
    gl = new GlApi(new SilkGlFunctionLoader(silkGl));

    var size = window.Size;
    gl.Viewport(0, 0, size.X, size.Y);
};

window.Resize += size =>
{
    gl?.Viewport(0, 0, size.X, size.Y);
};

window.Render += _ =>
{
    if (gl is null)
    {
        return;
    }

    gl.ClearColor(0.08f, 0.12f, 0.18f, 1.0f);
    gl.Clear(GlClearMask.ColorBufferBit);
};

window.Run();
