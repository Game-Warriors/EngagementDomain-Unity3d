using GameWarriors.EngagementDomain.Data;
using System.Threading.Tasks;

namespace GameWarriors.EngagementDomain.Abstraction
{
    public interface INotificationHandler
    {
        Task Setup();
        string AddNotification(NotificationData data);
        void AddNotification(string id, NotificationData data);
        void UpdateNotification(string id, NotificationData data);
        void RemoveNotification(string id);
        void RemoveAllNotification();
    }
}
