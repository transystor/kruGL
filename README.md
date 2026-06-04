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
- `kruGL.Native` — загрузка native function pointers
- `kruGL.OpenGL` — low-level OpenGL API surface
- `kruGL.Platform` — platform abstractions
- `kruGL.Platform.Glfw` — текущая platform-реализация окна и context bootstrap через прямой GLFW .NET binding

Для Windows сборок project дополнительно копирует native `glfw3.dll` в output под именем `glfw.dll`, потому что `GLFW-Net` ожидает именно такое имя native-библиотеки.

## Текущий статус

Сейчас это уже не просто пустой bootstrap: добавлен `kruGL.Native` с native function loader abstraction и заложен v0 API surface для low-level OpenGL wrapper в `kruGL.OpenGL`.

## V0 surface

Первая версия целится в минимальный набор, достаточный для:

- `ClearColor`
- `Clear`
- `Viewport`
- buffers + delete
- vertex arrays + delete
- shaders / programs
- compile/link status
- info logs
- checked compile/link helpers
- `DrawArrays`

## Smoke test

Добавлен `kruGL.TestHost`, который использует общий platform API библиотеки и текущую platform-реализацию `kruGL.Platform.Glfw`. Он уже умеет не только `Clear()`, но и первый минимальный triangle render path через shader + buffer + vertex array.

Сейчас `kruGL` умеет не только low-level GL вызовы, но и через platform-слой:
- создать окно
- поднять GL context
- достать адреса GL-функций из context

При этом GLFW-специфика теперь вынесена из общего platform API, чтобы позже можно было допилить и подставить альтернативную platform-реализацию без ломки верхнего слоя.

Silk.NET из текущего platform-слоя убран.

В headless Linux окружении без доступной desktop platform/graphics session запуск окна может падать на инициализации GLFW. Сборка smoke-test host при этом проходит, а реальный runtime smoke-test нужно прогонять в desktop окружении.
