using System;

namespace GameWarriors.EngagementDomain.Data
{
    public readonly struct NotificationData
    {
        public DateTime ShowTime { get; }
        public string Title { get; }
        public string Context { get; }
        public TimeSpan? RepeatInterval { get; }
        public string ChannelId { get; }
        public string SmallIcon { get; }
        public string BigIcon { get; }

        public NotificationData(DateTime showTime, string title, string context, string channelId = default, string smallIcon = default, string bigIcon = default, TimeSpan? repeatInterval = null)
        {

            ShowTime = showTime;
            Title = title;
            Context = context;
            RepeatInterval = repeatInterval;
            ChannelId = channelId;
            SmallIcon = smallIcon;
            BigIcon = bigIcon;
        }
    }
}