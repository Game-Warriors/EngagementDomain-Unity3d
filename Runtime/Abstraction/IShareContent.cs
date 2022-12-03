using UnityEngine;

namespace GameWarriors.EngagementDomain.Abstraction
{
    public enum ESocialNetworkType { None, Telegram, Line, Instagram, GoldenTelegram, Email, Myket, Whatsapp, Sms }

    public interface IShareContent
    {
        float Version { get; }
        void ShareSmsText(string message);
        void ShareText(string message, ESocialNetworkType socialNetwork);
        void SharePicture(Texture2D pic, ESocialNetworkType socialNetwork = ESocialNetworkType.None, string message = null);
        void ShareForTelegram(Texture2D pic, string message = null);
        bool OpenTelegramPage(string channelName);
        bool OpenGoldenTelegramPage(string channelName);
        bool OpenInstagramPage(string pageName);
        bool GetAppExistence(ESocialNetworkType socialNetwork);
        bool GetAppExistence(string packageName);
        bool DisplayGamePage(string packageName);
        void ShareFile(string filePath, ESocialNetworkType type, string shareContext);
    }
}
