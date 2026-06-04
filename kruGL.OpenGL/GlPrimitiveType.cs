namespace kruGL.OpenGL;

/// <summary>
/// Тип примитивов, который OpenGL должен собрать из переданных вершин.
/// Для текущего этапа важнее всего треугольники, потому что на них строится почти весь базовый рендер.
/// </summary>
public enum GlPrimitiveType : uint
{
    Triangles = 0x0004,
    TriangleStrip = 0x0005
}
