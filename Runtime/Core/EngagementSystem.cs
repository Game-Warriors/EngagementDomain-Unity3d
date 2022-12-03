using GameWarriors.EngagementDomain.Abstraction;
using GameWarriors.EngagementDomain.Data;
using System.Threading.Tasks;
using UnityEngine;

namespace GameWarriors.EngagementDomain.Core
{
    public class EngagementSystem : IEngagement
    {
        private readonly INotificationHandler _notificationHandler;
        private readonly IShareContent _shareContent;

        [UnityEngine.Scripting.Preserve]
        public EngagementSystem(INotificationHandler notificationHandler, IShareContent shareContent)
        {
            if (notificationHandler != null)
                _notificationHandler = notificationHandler;
            else
                _notificationHandler = new DummyNotificationHandler();

            if (shareContent != null)
                _shareContent = shareContent;
            else
                _shareContent = new DummyShare();
        }

        [UnityEngine.Scripting.Preserve]
        public Task WaitForLoading()
        {
            return _notificationHandler.Setup();
        }

        public string SendNotification(NotificationData data)
        {
            return _notificationHandler.AddNotification(data);
        }

        public void SendNotification(string id, NotificationData data)
        {
            _notificationHandler.AddNotification(id, data);
        }

        public void UpdateNotification(string id, NotificationData data)
        {
            _notificationHandler.UpdateNotification(id, data);
        }

        public void ClearNotification(string id)
        {
            _notificationHandler.RemoveNotification(id);
        }

        public void ClearAllNotifications()
        {
            _notificationHandler.RemoveAllNotification();
        }

        public void SendSms(string context)
        {
            _shareContent.ShareSmsText(context);
        }

        public void SharePicture(Texture2D shareTexture, ESocialNetworkType type, string shareContext)
        {
            _shareContent.SharePicture(shareTexture, type, shareContext);
        }

        public bool IsAppExist(ESocialNetworkType type)
        {
            return _shareContent.GetAppExistence(type);
        }

        public void ShareFile(string filePath, ESocialNetworkType type, string shareContext)
        {
            _shareContent.ShareFile(filePath, type, shareContext);
        }


    }
}