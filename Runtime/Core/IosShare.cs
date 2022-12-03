using UnityEngine;
using System.IO;
using System;
using GameWarriors.EngagementDomain.Abstraction;
using UnityEngine.Networking;

#if UNITY_IOS || UNITY_EDITOR
using UnityEngine.iOS;
using System.Runtime.InteropServices;


namespace GameWarriors.EngagementDomain.Core
{
    public class IosShare : IShareContent
    {
        [DllImport("__Internal")]
        static extern void _openInstagramPage(string pageName);
        [DllImport("__Internal")]
        static extern bool _isAppExist(string packageName);
        [DllImport("__Internal")]
        static extern void _sendTextMessage(string message);
        [DllImport("__Internal")]
        static extern void _postToInstagram(string message, string imagePath);
        [DllImport("__Internal")]
        static extern void _postToTelegram(string message, string imagePath);
        [DllImport("__Internal")]
        static extern void _postToWhatsapp(string message, string imagePath);
        [DllImport("__Internal")]
        static extern void _shareImageAndMessage(string message, string imagePath);
        [DllImport("__Internal")]
        static extern void _postToEmail(string subject, string message, string imagePath);


        public float Version => float.TryParse(Device.systemVersion, out var version) ? version : System.Environment.OSVersion.Version.Major;

        public bool DisplayGamePage(string packageName)
        {
            return false;
        }

        public bool GetAppExistence(ESocialNetworkType socialNetwork)
        {
            if (socialNetwork == ESocialNetworkType.Instagram)
            {
                return _isAppExist("instagram://");
            }

            if (socialNetwork == ESocialNetworkType.Telegram)
            {
                return _isAppExist("tg://");
            }

            if (socialNetwork == ESocialNetworkType.Whatsapp)
            {
                return _isAppExist("whatsapp://");
            }
            return false;
        }

        public bool GetAppExistence(string packageName)
        {
            return _isAppExist(packageName);
        }

        public bool OpenGoldenTelegramPage(string channelName)
        {
            return false;
        }

        public bool OpenInstagramPage(string pageName)
        {
            _openInstagramPage($"instagram://user?username={pageName}");
            return true;
        }

        public bool OpenTelegramPage(string channelName)
        {

            return true;
        }

        public void ShareForTelegram(Texture2D pic, string message = null)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();
            byte[] bytes = pic.EncodeToPNG();
            string imagePath = Application.persistentDataPath + "/MyImage.png";
            /*
            try
            {
                File.WriteAllBytes(imagePath, bytes);
            }
            catch (Exception E)
            {
#if DEVELOPMENT
                Debug.LogError(E.ToString());
#endif
                imagePath = null;
                return;
            }*/
            _postToTelegram(message, imagePath);
        }

        public void SharePicture(Texture2D pic, ESocialNetworkType socialNetwork = ESocialNetworkType.None, string message = null)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();
            switch (socialNetwork)
            {
                case ESocialNetworkType.Email:
                    SendEmail("@.com", "Invitation", message); break;
                //_postToEmail("Invitation", message, ""); break;
                case ESocialNetworkType.Instagram:
                    {
                        byte[] bytes = pic.EncodeToPNG();
                        string imagePath = Application.persistentDataPath + "/MyImage.png";

                        try
                        {
                            File.WriteAllBytes(imagePath, bytes);
                        }
                        catch (Exception E)
                        {
#if DEVELOPMENT
                            Debug.LogError(E.ToString());
#endif
                            imagePath = null;
                            return;
                        }
                        _postToInstagram(message, imagePath);
                        break;
                    }
                case ESocialNetworkType.Whatsapp:
                    _postToWhatsapp(message, "");
                    break;
                case ESocialNetworkType.Telegram: _postToTelegram(message, ""); break;
                default:
                    {
                        byte[] bytes = pic.EncodeToPNG();
                        string path = Application.persistentDataPath + "/tmp";
                        if (!Directory.Exists(path))
                        {
                            Directory.Exists(path);
                        }
                        string imagePath = Application.persistentDataPath + "/MyImage.png";

                        try
                        {
                            File.WriteAllBytes(imagePath, bytes);
                        }
                        catch (Exception E)
                        {
#if DEVELOPMENT
                            Debug.LogError(E.ToString());
#endif
                            imagePath = null;
                            return;
                        }
                        _shareImageAndMessage(message, imagePath);
                    }
                    break;
            }
        }

        public void ShareSmsText(string message)
        {
            _sendTextMessage(message);
        }

        public void ShareText(string message, ESocialNetworkType socialNetwork)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();

            return;
        }

        private void SendEmail(string email, string subject, string body)
        {
            subject = MyEscapeURL(subject);
            body = MyEscapeURL(body);
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        private string MyEscapeURL(string url)
        {
            return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
        }

        public void ShareFile(string filePath, ESocialNetworkType socialNetwork, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();
            switch (socialNetwork)
            {
                case ESocialNetworkType.Email:
                    SendEmail("@.com", "Invitation", message); break;
                //_postToEmail("Invitation", message, ""); break;
                case ESocialNetworkType.Instagram:
                    {
                        _postToInstagram(message, filePath);
                        break;
                    }
                case ESocialNetworkType.Whatsapp:
                    _postToWhatsapp(message, "");
                    break;
                case ESocialNetworkType.Telegram: _postToTelegram(message, ""); break;
                default:
                    {
                        _shareImageAndMessage(message, filePath);
                    }
                    break;
            }
        }
    }
}
#endif
