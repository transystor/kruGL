// Этот проект, это минимальный smoke/integration test для библиотеки.
// Его задача не быть движком или игрой, а доказать, что связка
// window -> GL context -> shader -> buffer -> draw path реально работает.

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
    settings => new GlfwWindowPlatform(settings));

GlProgramHandle shaderProgram = default;
GlBufferHandle vertexBuffer = default;
GlVertexArrayHandle vertexArray = default;

window.Load += () =>
{
    if (window.Gl is null)
    {
        return;
    }

    const string vertexShaderSource = """
#version 330 core
layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}
""";

    const string fragmentShaderSource = """
#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(0.95, 0.55, 0.20, 1.0);
}
""";

    var gl = window.Gl;

    var vertexShader = gl.CreateShader(GlShaderType.VertexShader);
    gl.ShaderSource(vertexShader, vertexShaderSource);
    gl.CompileShaderChecked(vertexShader);

    var fragmentShader = gl.CreateShader(GlShaderType.FragmentShader);
    gl.ShaderSource(fragmentShader, fragmentShaderSource);
    gl.CompileShaderChecked(fragmentShader);

    shaderProgram = gl.CreateProgram();
    gl.AttachShader(shaderProgram, vertexShader);
    gl.AttachShader(shaderProgram, fragmentShader);
    gl.LinkProgramChecked(shaderProgram);

    gl.DeleteShader(vertexShader);
    gl.DeleteShader(fragmentShader);

    var vertices = new float[]
    {
         0.0f,  0.6f, 0.0f,
        -0.6f, -0.4f, 0.0f,
         0.6f, -0.4f, 0.0f
    };

    vertexArray = gl.CreateVertexArray();
    gl.BindVertexArray(vertexArray);

    vertexBuffer = gl.CreateBuffer();
    gl.BindBuffer(GlBufferTarget.ArrayBuffer, vertexBuffer);
    gl.BufferData<float>(GlBufferTarget.ArrayBuffer, vertices, GlBufferUsage.StaticDraw);

    gl.EnableVertexAttribArray(0);
    gl.VertexAttribPointer(0, 3, GlDataType.Float, false, 3 * sizeof(float), 0);
};

window.Render += _ =>
{
    if (window.Gl is null)
    {
        return;
    }

    var gl = window.Gl;

    gl.ClearColor(0.08f, 0.12f, 0.18f, 1.0f);
    gl.Clear(GlClearMask.ColorBufferBit);

    gl.UseProgram(shaderProgram);
    gl.BindVertexArray(vertexArray);
    gl.DrawArrays(GlPrimitiveType.Triangles, 0, 3);
};

window.Run();
