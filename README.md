# kruGL

Тонкая C#-библиотека для practical OpenGL-wrapper под нужды игры и связанных тулов.

## Принципы

- не покрывать весь OpenGL API подряд
- расти от реальных use-case'ов игры
- держать abstraction тонким
- не смешивать graphics layer с game logic

## Текущая структура

- `kruGL.Abstractions` — базовые интерфейсы и типы
- `kruGL.Core` — общие utility/helper pieces
- `kruGL.OpenGL` — OpenGL-специфичные реализации

## Текущий статус

Сейчас это уже не просто пустой bootstrap: добавлен `kruGL.Native` с native function loader abstraction и заложен v0 API surface для low-level OpenGL wrapper в `kruGL.OpenGL`.

## V0 surface

Первая версия целится в минимальный набор, достаточный для:

- `ClearColor`
- `Clear`
- `Viewport`
- buffers
- vertex arrays
- shaders / programs
- `DrawArrays`
