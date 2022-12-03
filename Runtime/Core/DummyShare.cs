using GameWarriors.EngagementDomain.Abstraction;
using System;
using UnityEngine;

namespace GameWarriors.EngagementDomain.Core
{
    public class DummyShare : IShareContent
    {
        public float Version => Environment.OSVersion.Version.Major;


        public bool DisplayGamePage(string packageName)
        {
            Debug.Log("DisplayGamePage");
            return true;
        }

        public bool GetAppExistence(ESocialNetworkType socialNetwork)
        {
            Debug.Log("GetAppExistence");
            return true;
        }

        public bool GetAppExistence(string packageName)
        {
            Debug.Log("GetAppExistence");
            return true;
        }

        public bool OpenGoldenTelegramPage(string channelName)
        {
            Debug.Log("OpenGoldenTelegramPage");
            return true;
        }

        public bool OpenInstagramPage(string pageName)
        {
            Application.OpenURL("www.instagram.com/"+ pageName);
            return true;
        }

        public bool OpenTelegramPage(string channelName)
        {
            Debug.Log("OpenTelegramPage");
            return true;
        }

        public bool RateToApp(string packageName)
        {
            Debug.Log("RateToApp");
            return true;
        }

        public void ShareFile(string filePath, ESocialNetworkType type, string shareContext)
        {
            Debug.Log("ShareFile");
        }

        public void ShareForTelegram(Texture2D pic, string message = null)
        {
            Debug.Log("ShareForTelegram");
        }

        public void SharePicture(Texture2D pic, ESocialNetworkType socialNetwork = ESocialNetworkType.None, string message = null)
        {
            Debug.Log("SharePicture");
        }

        public void ShareSmsText(string message)
        {
            Debug.Log("ShareSMSText");
        }

        public void ShareText(string message, ESocialNetworkType socialNetwork)
        {
            Debug.Log("ShareText");
        }
    }
}
