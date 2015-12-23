using System;
using UnityEngine;
using System.Collections;


namespace ZelderFramework.Helpers
{
    /// <summary>
    /// Помощь с отладкой.
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// RECT.
        /// </summary>
        /// <param name="rect"></param>
        public static void LogRect(Rect rect)
        {
            Debug.Log(String.Format("RECT --->   x:{0}; y:{1}   w:{2}; h:{3}", rect.x, rect.y, rect.width, rect.height));
        }
    }

}
