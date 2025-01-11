CREATE TABLE [dbo].[UserInfo]
(
	    Id INT IDENTITY(1,1) PRIMARY KEY, -- Уникальный идентификатор
    TitleRus NVARCHAR(255) NOT NULL, -- Название на русском
    TitleEng NVARCHAR(255) NOT NULL, -- Название на английском
    Type NVARCHAR(100) NOT NULL,     -- Тип мультфильма (например, "Полнометражный фильм")
    Sound NVARCHAR(100) NOT NULL,   -- Звук (например, "Субтитры")
    ImagePath NVARCHAR(500) NOT NULL, -- Путь к изображению
    Link NVARCHAR(500) NOT NULL     -- Ссылка на страницу мультфильм
)
