# Семестровая работа №1

## Информация

- **Техническое задание**: Подробное описание функционала и требований проекта доступно в файле `ТЗ.docx`.
- **Figma**: Дизайн и функциональные схемы доступны по [ссылке](https://www.figma.com/design/27MFM5D4OFnPZgZbwPc4tK/Untitled?node-id=1-1413&t=ab9rq5pXGn4wGg5s-0/).

Этот проект представляет собой веб-сервер с использованием `HttpServerLibrary`, поддерживающий шаблонизацию через `TemplateEngine` и взаимодействие с базой данных посредством `MyORMLibrary`.

## Структура проекта

- **HttpServerLibrary**: Библиотека для обработки HTTP-запросов и ответов.
- **TemplateEngine**: Модуль для обработки шаблонов и генерации HTML-страниц.
- **MyORMLibrary**: Библиотека для взаимодействия с базой данных.
- **MyHttpServer**: Основное приложение, использующее вышеперечисленные библиотеки.

## Требования

- **Docker**: Убедитесь, что Docker установлен и запущен на вашей системе.
- **Docker Compose**: Для оркестрации контейнеров используется Docker Compose.

## Установка и запуск

1. Клонируйте репозиторий:

   ```bash
   git clone https://github.com/Antero0714/SiteProject.git
   ```

2. Перейдите в директорию проекта:

   ```bash
   cd SiteProject
   ```

## Настройка базы данных

После запуска контейнеров необходимо создать таблицу `Users` в базе данных. Для этого:

1. Откройте SQL Server Object Explorer в вашей IDE или используйте любой инструмент для управления базой данных.

2. Подключитесь к базе данных, используя следующие параметры подключения:

   - **Сервер**: `localhost`
   - **Порт**: `1433`
   - **Пользователь**: `sa`
   - **Пароль**: `your_password` (замените на фактический пароль, указанный в `docker-compose.yml`)

3. Создайте таблицу `users`, выполнив скрипт из файла `dbo.Users.sql`, расположенного в корне репозитория.


3. Запустите Docker Compose:

   ```bash
   docker-compose up -d
   ```

   Это команда создаст и запустит необходимые контейнеры в фоновом режиме.

## Использование

После успешного запуска контейнеров и настройки базы данных, приложение будет доступно по адресу `http://localhost:8888`.

### Инструкция по переходам через URL:

- [http://localhost:8888/main](http://localhost:8888/main) - переход на главную страницу
- [http://localhost:8888/control](http://localhost:8888/control) - переход на панель управления фильмами (Необходима регистрация на сайте)
- [http://localhost:8888/film?id=""](http://localhost:8888/film?id="") - возможность перейти на конкретный фильм по `id` этого фильма

**P.S.** В консоли будет отображаться отладочная информация, связанная с cookies пользователя.

## Остановка

Чтобы остановить и удалить контейнеры, выполните:

```bash
docker-compose down
```
