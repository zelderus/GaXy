using UnityEngine;
using System.Collections;

namespace ZelderFramework.Helpers
{
    /// <summary>
    /// Работа с координатами экрана.
    /// </summary>
    public static class DisplayHelper
    {
        /// <summary>
        /// Рабочий размер экрана.
        /// </summary>
        public static Vector2 WorkingScreenSize { get { return new Vector2(768, 1280); } } //! portrait
        /// <summary>
        /// Масштаб реального размера экрана к отношению 768x1280.
        /// </summary>
        public static float ScreenScale { get; private set; }
        /// <summary>
        /// Обновление размера экрана.
        /// </summary>
        public static void UpdateScreenSacle()
        {
            ScreenScale = 1;
            ScreenScale = (float)Screen.width / WorkingScreenSize.x;
        }

        /// <summary>
        /// Координаты Unity-экрана к нормальным (0,0 - слева-вверху).
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public static Vector3 UnityToNormalCoord(Vector3 coord)
        {
            return new Vector3(coord.x, Screen.height - coord.y, coord.z);
        }
        /// <summary>
        /// Координаты Unity-экрана к нормальным (0,0 - слева-вверху).
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public static Vector2 UnityToNormalCoord(Vector2 coord)
        {
            return new Vector2(coord.x, Screen.height - coord.y);
        }

        /// <summary>
        /// Координаты Unity-экрана к нормальным и к виртуальному экрану 768x1280.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public static Vector3 UnityToScreenCoord(Vector3 coord)
        {
            coord = UnityToNormalCoord(coord);
            return new Vector3(coord.x / ScreenScale, coord.y / ScreenScale, coord.z / ScreenScale);
        }
        /// <summary>
        /// Координаты Unity-экрана к нормальным и к виртуальному экрану 768x1280.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public static Vector2 UnityToScreenCoord(Vector2 coord)
        {
            coord = UnityToNormalCoord(coord);
            return new Vector2(coord.x / ScreenScale, coord.y / ScreenScale);
        }
        ///// <summary>
        ///// Координаты Unity-экрана к нормальным и к виртуальному экрану 768x1280.
        ///// </summary>
        ///// <param name="rect"></param>
        ///// <returns></returns>
        //public static Rect UnityToScreenCoord(Rect rect)
        //{
        //    var s = UnityToNormalCoord(rect.size);
        //    s.y = rect.y;
        //    return new Rect(UnityToNormalCoord(rect.position), s);
        //}

    }


}
