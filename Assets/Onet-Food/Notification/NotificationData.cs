using System;

[Serializable]
public class NotificationData
{
    public string message;
    public NotificationColorType colorType;

    public NotificationData(string message, NotificationColorType colorType)
    {
        this.message = message;
        this.colorType = colorType;
    }
}

public enum NotificationColorType
{
    White,
    Red,
    Green,
    Blue,
    Yellow,
    Gray
}