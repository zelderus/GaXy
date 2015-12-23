using System;
using UnityEngine;
using System.Collections;


namespace ZelderFramework.Helpers
{
    /// <summary>
    /// Работа с изображениями.
    /// </summary>
    public static class ImageHelper
    {

        /// <summary>
        /// Загрузка текстуры.
        /// </summary>
        /// <param name="assetPath">'pic'</param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static void LoadTexture(String assetPath, out Texture2D texture)
        {
            //if (!assetPath.Contains("."))
            //{
            //    assetPath = String.Format("{0}.png", assetPath);
            //}
            //var filePath = String.Format("{0}/{1}", Application.dataPath, assetPath);
            //if (System.IO.File.Exists(filePath))
            //{
            //    var bytes = System.IO.File.ReadAllBytes(filePath);
            //    texture = new Texture2D(1, 1);
            //    texture.LoadImage(bytes);
            //}
            //else
            //{
            //    texture = new Texture2D(100, 100, TextureFormat.RGB24, false);  // error
            //}

            //assetPath = String.Format("{0}\\{1}", Application.dataPath, assetPath);
            assetPath = assetPath.Replace("\\", "/");
            texture = Resources.Load(assetPath, typeof(Texture2D)) as Texture2D;

        }
        /// <summary>
        /// Загрузка текстуры.
        /// </summary>
        /// <param name="assetPath">'pic'</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(String assetPath)
        {
            Texture2D tex;
            LoadTexture(assetPath, out tex);
            return tex;
        }

    }


}