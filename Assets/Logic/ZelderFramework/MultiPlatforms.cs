using System;
using UnityEngine;
using System.Collections;

namespace ZelderFramework
{
    /// <summary>
    /// Кроссплатформа.
    /// </summary>
    public class MultiPlatforms
    {



        public MultiPlatforms()
        {
            // TODO: android keys, etc..


        }


        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceName()
        {
            return GetDeviceNameDo();
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void Purchase(String productId, Action doFulfillment, Action<String> onError)
        {
            PurchaseDo(productId, doFulfillment, onError);
        }
        /// <summary>
        /// Текущий язык в устройстве.
        /// </summary>
        /// <returns></returns>
        public GameLanguages GetDeviceLanguage()
        {
            return GetDeviceLanguageDo();
        }





#if UNITY_EDITOR //|| UNITY_EDITOR_WIN || UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_STANDALONE_MAC
        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceNameDo()
        {
            return "Editor";
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void PurchaseDo(String productId, Action doFulfillment, Action<String> onError)
        {
            doFulfillment();
            return;
        }
        /// <summary>
        /// Текущий язык в устройстве.
        /// </summary>
        /// <returns></returns>
        public GameLanguages GetDeviceLanguageDo()
        {
            return GameLanguages.English;
        }
#endif

#if (UNITY_WP8 || UNITY_METRO) && !UNITY_EDITOR
        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceNameDo()
        {
            return "";//WP8Zelder.Device.GetDeviceName;
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void PurchaseDo(String productId, Action doFulfillment, Action<String> onError)
        {
            //WP8Zelder.Market.Purchase(productId, doFulfillment, onError);
            doFulfillment();
        }
        /// <summary>
        /// Текущий язык в устройстве.
        /// </summary>
        /// <returns></returns>
        public GameLanguages GetDeviceLanguageDo()
        {
            //var culture = WP8Zelder.Device.GetDeviceLanguage();

            //if (culture.StartsWith("ru")) return GameLanguages.Russian;

            return GameLanguages.English;
        }
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceNameDo()
        {
            return "IOS";
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void PurchaseDo(String productId, Action doFulfillment, Action<String> onError)
        {
            // TODO:
            Debug.Log("Not implemented");
            onError("not implemented yet!");
        }
        /// <summary>
        /// Текущий язык в устройстве.
        /// </summary>
        /// <returns></returns>
        public GameLanguages GetDeviceLanguageDo()
        {
            return GameLanguages.English;
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceNameDo()
        {
            return "Android";
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void PurchaseDo(String productId, Action doFulfillment, Action<String> onError)
        {
            // TODO:
            Debug.Log("Not implemented");
            onError("not implemented yet!");
        }
        /// <summary>
        /// Текущий язык в устройстве.
        /// </summary>
        /// <returns></returns>
        public GameLanguages GetDeviceLanguageDo()
        {
            return GameLanguages.English;
        }
#endif




    }
}
