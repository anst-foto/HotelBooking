using System;

namespace HotelBooking.DAL;

/// <summary>
/// Ошибка конфигурации
/// </summary>
public class ConfigurationException : Exception
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="message">Текст ошибки</param>
    public ConfigurationException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="message">Текст ошибки</param>
    /// <param name="innerException">Исходная ошибка</param>
    public ConfigurationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}