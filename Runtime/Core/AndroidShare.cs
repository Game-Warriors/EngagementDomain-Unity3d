using GameWarriors.EngagementDomain.Abstraction;
using System;
using System.IO;
using UnityEngine;

#if UNITY_ANDROID
namespace GameWarriors.EngagementSystem.Core
{
    public class AndroidShare : IShareContent
    {
        private readonly int _androidAPIVersion;

        public float Version => _androidAPIVersion;

        public AndroidShare()
        {
            _androidAPIVersion = GetSDKLevel();
        }

        public void ShareSmsText(string message)
        {
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent");
            androidJavaObject.Call<AndroidJavaObject>("setAction", androidJavaClass.GetStatic<string>("ACTION_VIEW"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            androidJavaObject.Call<AndroidJavaObject>("setData",
                uriClass.CallStatic<AndroidJavaObject>("parse", "smsto:"));
            androidJavaObject.Call<AndroidJavaObject>("setType", "vnd.android-dir/mms-sms");
            androidJavaObject.Call<AndroidJavaObject>("putExtra", "sms_body", message);

            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            unityObject.Call("startActivity", androidJavaObject);
        }

        public void ShareText(string message, ESocialNetworkType socialNetwork)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();

            AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent");
            androidJavaObject.Call<AndroidJavaObject>("setAction", androidJavaClass.GetStatic<string>("ACTION_SEND"));
            androidJavaObject.Call<AndroidJavaObject>("setType", "text/plain");
            androidJavaObject.Call<AndroidJavaObject>("putExtra", androidJavaClass.GetStatic<string>("EXTRA_TEXT"),
                message);
            if (GetAppExistence(socialNetwork))
            {
                string socialComName = GetSocialComName(socialNetwork);
                androidJavaObject.Call<AndroidJavaObject>("setPackage", socialComName);
            }

            AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
            @static.Call("startActivity", androidJavaObject);
        }

        public void ShareForTelegram(Texture2D pic, string message = null)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();

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
            }

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            string actionView = intentClass.GetStatic<string>("ACTION_VIEW");

            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", actionView);
            intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intent.Call<AndroidJavaObject>("setType", "image/*");

            intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);


            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            if (imagePath != null)
            {
                AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", imagePath);
                AndroidJavaObject uri = null;
                if (_androidAPIVersion > 22)
                {
                    AndroidJavaObject unityContext = unityActivity.Call<AndroidJavaObject>("getApplicationContext");
                    string packageName = unityContext.Call<string>("getPackageName");
                    string authority = packageName + ".fileprovider";
                    AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
                    uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority,
                        fileObject);
                }
                else
                {
                    AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                    uri = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
                }

                if (fileObject.Call<bool>("exists"))
                {
                    intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uri);
                }
            }

            if (_androidAPIVersion > 22)
            {
                int flagGrantReadUriPermission = intentClass.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");
                intent.Call<AndroidJavaObject>("addFlags", flagGrantReadUriPermission);
            }

            try
            {
                if (GetAppExistence(ESocialNetworkType.GoldenTelegram))
                {
                    intent.Call<AndroidJavaObject>("setPackage", GetSocialComName(ESocialNetworkType.GoldenTelegram));
                    unityActivity.Call("startActivity", intent);
                }
                else if (GetAppExistence(ESocialNetworkType.Telegram))
                {
                    intent.Call<AndroidJavaObject>("setPackage", GetSocialComName(ESocialNetworkType.Telegram));
                    unityActivity.Call("startActivity", intent);
                }
                else
                {
                    intent.Call<AndroidJavaObject>("setType", "message/rfc822");
                    AndroidJavaObject chooser =
                        intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Choose Application");
                    unityActivity.Call("startActivity", chooser);
                }
            }
            catch
            {
                intent.Call<AndroidJavaObject>("setType", "text/plain");
                AndroidJavaObject chooser =
                    intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Choose Application");
                unityActivity.Call("startActivity", chooser);
                return;
            }
        }

        public void SharePicture(Texture2D pic, ESocialNetworkType socialNetwork = ESocialNetworkType.None,
            string message = null)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();

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
            }

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            string actionView = intentClass.GetStatic<string>("ACTION_VIEW");
            //int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");

            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", actionView);
            intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            //intent.Call<AndroidJavaObject>("setType", "vnd.android.cursor.dir/email");
            //intent.Call<AndroidJavaObject>("setType", "message/rfc822");
            //intent.Call<AndroidJavaObject>("setType", "image/png");
            //intent.Call<AndroidJavaObject>("setType", "www/mime");
            //intent.Call<AndroidJavaObject>("setType", "text/email");
            if (socialNetwork != ESocialNetworkType.Email)
                intent.Call<AndroidJavaObject>("setType", "text/plain");
            else
                intent.Call<AndroidJavaObject>("setType", "www/mime");

            intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            if (imagePath != null)
            {
                AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", imagePath);
                AndroidJavaObject uri = null;
                if (_androidAPIVersion > 22)
                {
                    AndroidJavaObject unityContext = unityActivity.Call<AndroidJavaObject>("getApplicationContext");
                    string packageName = unityContext.Call<string>("getPackageName");
                    string authority = packageName + ".fileprovider";
                    AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
                    uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority,
                        fileObject);
                }
                else
                {
                    AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                    uri = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
                }

                if (fileObject.Call<bool>("exists"))
                {
                    intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uri);
                }
            }

            if (_androidAPIVersion > 22)
            {
                int flagGrantReadUriPermission = intentClass.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");
                intent.Call<AndroidJavaObject>("addFlags", flagGrantReadUriPermission);
            }

            try
            {
                if (GetAppExistence(socialNetwork))
                {
                    Debug.Log("set pack " + GetSocialComName(socialNetwork));
                    intent.Call<AndroidJavaObject>("setPackage", GetSocialComName(socialNetwork));
                    unityActivity.Call("startActivity", intent);
                }
                else
                {
                    AndroidJavaObject chooser =
                        intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Choose Application");
                    unityActivity.Call("startActivity", chooser);
                }
            }
            catch
            {
                intent.Call<AndroidJavaObject>("setType", "text/plain");
                AndroidJavaObject chooser =
                    intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Choose Application");
                unityActivity.Call("startActivity", chooser);
                return;
            }
        }

        public bool DisplayGamePage(string packageName)
        {
            return false;
        }

        public bool OpenGoldenTelegramPage(string channelName)
        {
            bool result = true;
            if (GetAppExistence(ESocialNetworkType.GoldenTelegram))
            {
                string text = "https://telegram.me/" + channelName;
                AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent");
                AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri");
                androidJavaObject.Call<AndroidJavaObject>("setAction",
                    new object[1] {androidJavaClass.GetStatic<string>("ACTION_VIEW")});
                androidJavaObject.Call<AndroidJavaObject>("setData",
                    new object[1] {androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[1] {text})});
                androidJavaObject.Call<AndroidJavaObject>("setPackage", new object[1] {"org.ir.talaeii"});
                AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject @static = androidJavaClass3.GetStatic<AndroidJavaObject>("currentActivity");
                @static.Call("startActivity", androidJavaObject);
            }
            else
            {
                result = false;
            }

            return result;
        }

        public bool OpenInstagramPage(string pageName)
        {
            bool result = true;
            if (GetAppExistence(ESocialNetworkType.Instagram))
            {
                string text = "http://instagram.com/_u/" + pageName;
                AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent");
                AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri");
                androidJavaObject.Call<AndroidJavaObject>("setAction",
                    new object[1] {androidJavaClass.GetStatic<string>("ACTION_VIEW")});
                androidJavaObject.Call<AndroidJavaObject>("setData",
                    new object[1] {(androidJavaClass2).CallStatic<AndroidJavaObject>("parse", new object[1] {text})});
                androidJavaObject.Call<AndroidJavaObject>("setPackage", new object[1] {"com.instagram.android"});
                AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject @static = androidJavaClass3.GetStatic<AndroidJavaObject>("currentActivity");
                @static.Call("startActivity", androidJavaObject);
            }
            else
            {
                result = false;
            }

            return result;
        }

        public bool OpenTelegramPage(string channelName)
        {
            bool result = true;
            if (GetAppExistence(ESocialNetworkType.Telegram))
            {
                string text = "https://telegram.me/" + channelName;
                AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent");
                AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri");
                androidJavaObject.Call<AndroidJavaObject>("setAction",
                    new object[1] {androidJavaClass.GetStatic<string>("ACTION_VIEW")});
                androidJavaObject.Call<AndroidJavaObject>("setData",
                    new object[1] {androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[1] {text})});
                androidJavaObject.Call<AndroidJavaObject>("setPackage", new object[1] {"org.telegram.messenger"});
                AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject @static = androidJavaClass3.GetStatic<AndroidJavaObject>("currentActivity");
                @static.Call("startActivity", androidJavaObject);
            }
            else
            {
                result = false;
            }

            return result;
        }

        public bool GetAppExistence(ESocialNetworkType socialNetwork)
        {
            if (socialNetwork == ESocialNetworkType.None)
                return false;
            try
            {
                bool result = true;
                string socialComName = GetSocialComName(socialNetwork);
                if (string.IsNullOrEmpty(socialComName) || !CheckPackageAppIsPresent(socialComName))
                {
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
                return true;
            }
        }


        public bool GetAppExistence(string packageName)
        {
            return CheckPackageAppIsPresent(packageName);
        }

        private static int GetSDKLevel()
        {
            using var version = new AndroidJavaClass("android.os.Build$VERSION");
            return version.GetStatic<int>("SDK_INT");
        }

        private static string GetSocialComName(ESocialNetworkType socialNetwork)
        {
            string result = string.Empty;
            switch (socialNetwork)
            {
                case ESocialNetworkType.Telegram:
                    result = "org.telegram.messenger";
                    break;
                case ESocialNetworkType.Line:
                    result = "jp.naver.line.android";
                    break;
                case ESocialNetworkType.Instagram:
                    result = "com.instagram.android";
                    break;
                case ESocialNetworkType.GoldenTelegram:
                    result = "org.ir.talaeii";
                    break;
                case ESocialNetworkType.Whatsapp:
                    result = "com.whatsapp";
                    break;
                //case SocialNetworkType.Email:
                //    result = "com.google.android.gm";
                //    break;
                case ESocialNetworkType.None:
                    break;
                case ESocialNetworkType.Email:
                    break;
                case ESocialNetworkType.Myket:
                    break;
                case ESocialNetworkType.Sms:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(socialNetwork), socialNetwork, null);
            }

            return result;
        }

        private static bool CheckPackageAppIsPresent(string package)
        {
            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
            AndroidJavaObject androidJavaObject2 =
                androidJavaObject.Call<AndroidJavaObject>("getInstalledPackages", new object[1] {0});
            int num = androidJavaObject2.Call<int>("size", new object[0]);
            for (int i = 0; i < num; i++)
            {
                AndroidJavaObject androidJavaObject3 =
                    androidJavaObject2.Call<AndroidJavaObject>("get", new object[1] {i});
                string text = androidJavaObject3.Get<string>("packageName");
                if (string.Compare(text, package, StringComparison.Ordinal) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void ShareFile(string filePath, ESocialNetworkType socialNetwork, string message)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            string actionView = intentClass.GetStatic<string>("ACTION_VIEW");
            //int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");

            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", actionView);
            intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            //intent.Call<AndroidJavaObject>("setType", "vnd.android.cursor.dir/email");
            //intent.Call<AndroidJavaObject>("setType", "message/rfc822");
            //intent.Call<AndroidJavaObject>("setType", "image/png");
            //intent.Call<AndroidJavaObject>("setType", "www/mime");
            //intent.Call<AndroidJavaObject>("setType", "text/email");

            intent.Call<AndroidJavaObject>("setType",
                socialNetwork != ESocialNetworkType.Email ? "text/plain" : "www/mime");

            intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            if (filePath != null)
            {
                AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", filePath);
                AndroidJavaObject uri;
                if (_androidAPIVersion > 22)
                {
                    AndroidJavaObject unityContext = unityActivity.Call<AndroidJavaObject>("getApplicationContext");
                    string packageName = Application.identifier;
                    string authority = packageName + ".fileprovider";
                    AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
                    uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority,
                        fileObject);
                }
                else
                {
                    AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                    uri = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
                }

                if (fileObject.Call<bool>("exists"))
                {
                    intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uri);
                }
            }

            if (_androidAPIVersion > 22)
            {
                int flagGrantReadUriPermission = intentClass.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");
                intent.Call<AndroidJavaObject>("addFlags", flagGrantReadUriPermission);
            }

            try
            {
                if (GetAppExistence(socialNetwork))
                {
                    Debug.Log("set pack " + GetSocialComName(socialNetwork));
                    intent.Call<AndroidJavaObject>("setPackage", GetSocialComName(socialNetwork));
                    unityActivity.Call("startActivity", intent);
                }
                else
                {
                    AndroidJavaObject chooser =
                        intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Choose Application");
                    unityActivity.Call("startActivity", chooser);
                }
            }
            catch
            {
                intent.Call<AndroidJavaObject>("setType", "text/plain");
                AndroidJavaObject chooser =
                    intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Choose Application");
                unityActivity.Call("startActivity", chooser);
            }
        }
    }
}
#endif