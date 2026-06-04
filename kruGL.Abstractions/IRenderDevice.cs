namespace kruGL.Abstractions;

/// <summary>
/// Минимальная абстракция над устройством рендера.
/// Пока здесь только очистка кадра, но сам интерфейс зарезервирован под дальнейший рост общего graphics API.
/// </summary>
public interface IRenderDevice
{
    void Clear(ColorRgba color);
}
