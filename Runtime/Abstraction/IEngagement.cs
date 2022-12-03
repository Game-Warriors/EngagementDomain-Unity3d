using GameWarriors.EngagementDomain.Data;
using UnityEngine;

namespace GameWarriors.EngagementDomain.Abstraction
{
    public interface IEngagement 
    {
        string SendNotification(NotificationData data);
        void SendNotification(string id, NotificationData data);
        void UpdateNotification(string id, NotificationData data);
        void ClearNotification(string id);
        void ClearAllNotifications();
        void SendSms(string context);
        void SharePicture(Texture2D shareTexture, ESocialNetworkType email, string shareContext);
        bool IsAppExist(ESocialNetworkType whatsapp);
        void ShareFile(string filePath, ESocialNetworkType type, string shareContext);
    }
}