using GameWarriors.EngagementDomain.Abstraction;
using GameWarriors.EngagementDomain.Data;
using System.Threading.Tasks;

namespace GameWarriors.EngagementDomain.Core
{
    public class DummyNotificationHandler : INotificationHandler
    {
        public string AddNotification(NotificationData data)
        {
            return System.Guid.NewGuid().ToString();
        }

        public void AddNotification(string id, NotificationData data)
        {
            return;
        }

        public void RemoveAllNotification()
        {
            return;
        }

        public void RemoveNotification(string id)
        {
            return;
        }

        public Task Setup()
        {
            return Task.CompletedTask;
        }

        public void UpdateNotification(string id, NotificationData data)
        {
            return;
        }
    }
}