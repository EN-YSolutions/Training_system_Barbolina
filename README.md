# Игра для повторения материала, пройденного на курсах
## Инструкция по запуску
1. Установите Unity версии 2022.3.9f1
2. Измените строчку 15 подключения в ``DatabaseConnector.cs`` на свою базу данных PostgreSQL
```
private static string _connectionString =
            "Port = 5432;" +
            "Server=localhost;" +
            "Database=postgres;" +
            "User ID=postgres;" +
            "Password=meow;";
```
3. Структура базы данных должна быть такой:
   ![](/doc/BD.jpg)
4. Используйте функцию для генерации id :
   ``id uuid primary key default gen_random_uuid()``
5. Добавьте в таблицу users двух пользователей.
6. Добавьте курс в таблицу cources, указав одного из пользователей как автора.
7. Добавьте прогресс в таблицу progress, указав одного из пользователей как проходящего курса.
8. Откройте Unity и включите сцену MainMenu
9. Нажмите кнопку Play
        ![](/doc/Play.JPG)
