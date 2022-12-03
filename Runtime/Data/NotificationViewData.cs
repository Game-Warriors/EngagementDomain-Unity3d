using System;
using UnityEngine;

namespace GameWarriors.EngagementDomain.Data
{
    [Serializable]
    public struct NotificationDataViewItem
    {
        [SerializeField]
        private string _notificationTitle;
        [SerializeField]
        private string _notificationContext;

        public string NotificationTitle => _notificationTitle;
        public string NotificationContext => _notificationContext;

    }

    [CreateAssetMenu(fileName = ASSET_NAME, menuName = "AssetObjects/Notification View Data")]
    public class NotificationViewData : ScriptableObject
    {
        public const string ASSET_NAME = "NotificationViewData";
        public const string ASSET_PATH = "Assets/AssetData/NotificationViewData.asset";

        [SerializeField]
        private NotificationDataViewItem[] _notificationDataViewItem;

        public NotificationDataViewItem[] NotificationDataViewItem => _notificationDataViewItem;

        public int DataCount => _notificationDataViewItem?.Length ?? 0;

    }
}