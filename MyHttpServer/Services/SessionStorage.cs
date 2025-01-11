public static class SessionStorage
{
    private static readonly Dictionary<string, string> _sessions = new Dictionary<string, string>();

    // Сохранение токена
    public static void SaveSession(string token, string userId)
    {
        _sessions[token] = userId;
        Console.WriteLine($"Session saved: Token={token}, UserId={userId}");
    }

    // Проверка токена
    public static bool ValidateToken(string token)
    {
        bool exists = _sessions.ContainsKey(token);
        Console.WriteLine($"Validating Token: {token}, Exists={exists}");
        return exists;
    }

    // Получение имени пользователя по токену
    public static string GetUserId(string token)
    {
        return _sessions.TryGetValue(token, out var userId) ? userId : null;
    }

    public static string GetUserNameByToken(string token)
    {
        return _sessions.TryGetValue(token, out string userName) ? userName : null;
    }

    // Удаление токена
    public static bool DeleteSession(string token)
    {
        if (_sessions.Remove(token))
        {
            Console.WriteLine($"Session removed: Token={token}");
            return true;
        }

        Console.WriteLine($"Failed to remove session: Token={token} not found.");
        return false;
    }
}
