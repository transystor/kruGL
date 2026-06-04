namespace kruGL.Platform;

/// <summary>
/// Минимальный набор настроек для создания окна.
/// Пока здесь только самое базовое, но позже сюда можно расширять flags,
/// версию context, fullscreen mode и прочие параметры.
/// </summary>
public sealed class WindowSettings
{
    public string Title { get; init; } = "kruGL";
    public int Width { get; init; } = 1280;
    public int Height { get; init; } = 720;
}
