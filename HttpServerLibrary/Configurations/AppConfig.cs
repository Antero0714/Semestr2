﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpServerLibrary.Configurations
{
    /// <summary>
    /// Модель конфигурационного файла сервера
    /// </summary>
    public sealed class AppConfig
    {
        /// <summary>
        /// Имя файла настроек сервера
        /// </summary>
        public const string FILE_NAME = "config.json";

        // Поля для хранения настроек
        public string Domain { get; set; } = "+";
        public uint Port { get; set; } = 8888;
        public string StaticDirectoryPath { get; set; } = "public/";


        public Dictionary<string, string> ConnectionStrings { get; set; }

        // SMTP настройки
        public EmailServiceConfiguration EmailConfig { get; set; } = new();

        // Singleton
        private static AppConfig? _instance;
        private static readonly object _lock = new();

        /// <summary>
        /// Получить единственный экземпляр конфигурации
        /// </summary>
        public static AppConfig Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = LoadConfiguration();
                    }
                }

                return _instance!;
            }


        }



        [JsonConstructor]
        private AppConfig() { }

        /// <summary>
        /// Загрузка конфигурации из файла или создание новой конфигурации по умолчанию
        /// </summary>
        /// <returns>Экземпляр AppConfig</returns>
        private static AppConfig LoadConfiguration()
        {
            if (File.Exists(FILE_NAME))
            {
                try
                {
                    var configFile = File.ReadAllText(FILE_NAME);
                    return JsonSerializer.Deserialize<AppConfig>(configFile) ?? new AppConfig();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при чтении конфигурации: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Файл настроек {FILE_NAME} не найден. Используются настройки по умолчанию.");
            }

            return new AppConfig();
        }

        public static AppConfig GetInstance()
        {
            if (_instance is null)
            {
                _instance = new AppConfig();
                _instance.Initialize();
            }

            return _instance;
        }


        /// <summary>
        /// Сохранение текущей конфигурации в файл
        /// </summary>
        public void SaveConfiguration()
        {
            try
            {
                var configJson = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FILE_NAME, configJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении конфигурации: {ex.Message}");
            }
        }

        private void Initialize()
        {
            if (File.Exists(AppConfig.FILE_NAME))
            {
                var configFile = File.ReadAllText(AppConfig.FILE_NAME);
                _instance = JsonSerializer.Deserialize<AppConfig>(configFile);
            }
            else
            {
                Console.WriteLine($"Файл настроек {AppConfig.FILE_NAME} не найден");
            }
        }
    }

    /// <summary>
    /// Настройки EmailService
    /// </summary>
    public class EmailServiceConfiguration
    {
        public string Host { get; set; } = "smtp.mail.ru";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "andrey051407@mail.ru";
        public string Password { get; set; } = "cWj423B54kJm37uM6N2v";
    }
}
