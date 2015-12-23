using System;
using UnityEngine;
using System.Collections;
//using ZelderFramework.Controls;
using ZelderFramework.Helpers;


namespace ZelderFramework
{

    /// <summary>
    /// Языки в игре.
    /// </summary>
    public enum GameLanguages
    {
        Russian = 1,
        English = 2
    }

    /// <summary>
    /// Жизнь игры.
    /// <remarks>Создать в главном статическом классе.</remarks>
    /// </summary>
    public class GameLife
    {

        /// <summary>
        /// Кросплатформа.
        /// </summary>
        public MultiPlatforms Platforms;

        private Texture2D _loadingTexture;
        //private UIRenderable _back;
        private bool _inited;
        private bool _isLoadingScreen;

        private Action _onBackPress;

        /// <summary>
        /// Масштаб реального размера экрана к отношению 768x1280.
        /// </summary>
        public float ScreenScale { get { return DisplayHelper.ScreenScale; } }


        /// <summary>
        /// Инициализация движка. Необходимо выполнить в первой сцене при старте.
        /// </summary>
        public void Init(String assetLoadingString)
        {
            if (_inited) return;

            Platforms = new MultiPlatforms();
            InitScreenSacle();
            LoadingScreen(assetLoadingString);
            _inited = true;
        }
        /// <summary>
        /// Инициализация для каждой сцены.
        /// <remarks>Вызывать в каждой сцене в Start()</remarks>
        /// </summary>
        /// <param name="onBackPress"></param>
        public void ScreenInit(Action onBackPress)
        {
            _onBackPress = onBackPress;
        }


        /// <summary>
        /// Переход на экран.
        /// </summary>
        /// <param name="screenIndex"></param>
        public void GoToScreen(Int32 screenIndex)
        {
            _isLoadingScreen = true;
            Application.LoadLevel(screenIndex);
        }
        /// <summary>
        /// Вызывает каждое окно по завершении загрузки.
        /// </summary>
        public void OnScreenLoaded()
        {
            _isLoadingScreen = false;
        }

        public void TestLoading()
        {
            _isLoadingScreen = true;
        }

        /// <summary>
        /// Suspending.
        /// <remarks>вызывать в событии OnApplicationPause() каждой сцены.</remarks>
        /// </summary>
        /// <param name="pausing"></param>
        public void OnApplicationPause(bool pausing)
        {
            //Debug.LogWarning("OnApplicationPause " + pausing);

        }


        /// <summary>
        /// Обновление игры.
        /// </summary>
        /// <param name="delta"></param>
        public void Update(float delta)
        {

            //+ back key
            if (Input.GetKey(KeyCode.Escape))
            {
                if (_onBackPress != null) _onBackPress();
            }




            if (_isLoadingScreen)
            {
                //+ front screen
                //_back.Update(delta);
            }
            else
            {
                //////+ hover
                ////var hover = GestHelpers.GetHover(true);
                ////if (hover != null)
                ////{
                    
                ////}
            }
        }


        /// <summary>
        /// Выполнять в каждой сцене.
        /// </summary>
        public void OnGUI()
        {
            if (!_inited) return;
            if (_isLoadingScreen)
            {
                //+ reset states
                GUI.depth = 1;
                GUI.color = Color.white;

                //+ draw texture
                //_back.Draw();
                GUI.DrawTexture(new Rect(0, 0, DisplayHelper.WorkingScreenSize.x * DisplayHelper.ScreenScale, DisplayHelper.WorkingScreenSize.y * DisplayHelper.ScreenScale), _loadingTexture, ScaleMode.ScaleAndCrop);
            }
        }




        #region Helpers

        private void InitScreenSacle()
        {
            DisplayHelper.UpdateScreenSacle();
        }

        private void LoadingScreen(String assetLoadingString)
        {
            //_back = new UIRenderable();
            //_back.SetTexture(assetLoadingString, new Rect(0, 0, 0, 0), Vector2.zero);
            //_back.Depth = 1;

            if (_loadingTexture != null) return;
            ImageHelper.LoadTexture(assetLoadingString, out _loadingTexture);
        }



        #endregion



        #region devices


#if UNITY_EDITOR
        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceName()
        {
            return "Editor";
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void Purchase(String productId, Action doFulfillment)
        {
            Debug.Log("Not implemented in Editor");
            return;
        }
#endif

#if UNITY_WP8 && !UNITY_EDITOR
        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceName()
        {
            return WP8Market.WP8Market.GetDeviceName;
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void Purchase(String productId, Action doFulfillment)
        {
            WP8Market.WP8Market.Purchase(productId, doFulfillment);
        }
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceName()
        {
            return "IOS";
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void Purchase(String productId, Action doFulfillment)
        {
            // TODO:
            
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// Название устрйоства.
        /// </summary>
        /// <returns></returns>
        public String GetDeviceName()
        {
            return "Android";
        }
        /// <summary>
        /// Покупка продукта.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="doFulfillment"></param>
        public void Purchase(String productId, Action doFulfillment)
        {
            // TODO:
            
        }
#endif

        #endregion




    }

}
