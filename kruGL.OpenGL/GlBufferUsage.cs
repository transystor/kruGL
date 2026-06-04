namespace kruGL.OpenGL;

/// <summary>
/// Подсказка драйверу о предполагаемом характере использования buffer-а.
/// Это влияет не на API-поведение, а на то, как драйвер может оптимизировать хранение данных.
/// </summary>
public enum GlBufferUsage : uint
{
    StaticDraw = 0x88E4,
    DynamicDraw = 0x88E8
}
