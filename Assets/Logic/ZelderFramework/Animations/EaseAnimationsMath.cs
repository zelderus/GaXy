using System;
using UnityEngine;
using System.Collections;


namespace ZelderFramework.Animations
{

    /// <summary>
    /// Типы анимаций.
    /// </summary>
    public enum EaseAnimationTypes
    {
        /// <summary>
        /// Отсутствие анимации.
        /// </summary>
        Nothing,
        /// <summary>
        /// Простое по прямой
        /// </summary>
        EaseNo,
        /// <summary>
        /// Плавное затухание
        /// </summary>
        EaseOutQuad,
        /// <summary>
        /// Плавное затухание и отскоки
        /// </summary>
        EaseOutBounce,
        /// <summary>
        /// С выскоком
        /// </summary>
        EaseOutElastic,
        /// <summary>
        /// С затуханием
        /// </summary>
        EaseOut,
        /// <summary>
        /// С выскоком
        /// </summary>
        EaseOutBack
    }

    
    /// <summary>
    /// Расчет анимаций.
    /// </summary>
	public static class EaseAnimationsMath
	{
        /// <summary>
        /// Ссылка на функцию анимации.
        /// </summary>
        /// <param name="animationType"></param>
        /// <returns></returns>
        public static Func<float, float, float, float, float> GetAnimationFunction(EaseAnimationTypes animationType)
        {
            switch (animationType)
            {
                case EaseAnimationTypes.Nothing: return EaseAnimationsMath.Nothing;
                case EaseAnimationTypes.EaseNo: return EaseAnimationsMath.EaseNo;
                case EaseAnimationTypes.EaseOut: return EaseAnimationsMath.EaseOut;
                case EaseAnimationTypes.EaseOutBack: return EaseAnimationsMath.EaseOutBack;
                case EaseAnimationTypes.EaseOutQuad: return EaseAnimationsMath.EaseOutQuad;
                case EaseAnimationTypes.EaseOutBounce: return EaseAnimationsMath.EaseOutBounce;
                case EaseAnimationTypes.EaseOutElastic: return EaseAnimationsMath.EaseOutElastic;
            }
            return EaseAnimationsMath.EaseNo;
        }

        #region maths
        /// <summary>
        /// Отсутствие анимации.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="b">значение</param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float Nothing(float t, float b, float c, float d)
        {
            return b;
        }

        /// <summary>
        /// Простое по прямой
		/// </summary>
        /// <param name="t">текущее время от начала анимации</param>
        /// <param name="b">начальное значение</param>
        /// <param name="c">разница между начальным и конечным значением</param>
        /// <param name="d">всего времени на анимацию</param>
		/// <returns></returns>
        public static float EaseNo(float t, float b, float c, float d)
		{
			t /= d;
			return c * t + b;
		}
		/// <summary>
        /// Плавное затухание
		/// </summary>
        /// <param name="t">текущее время от начала анимации</param>
        /// <param name="b">начальное значение</param>
        /// <param name="c">разница между начальным и конечным значением</param>
        /// <param name="d">всего времени на анимацию</param>
		/// <returns></returns>
        public static float EaseOutQuad(float t, float b, float c, float d)
		{
			t /= d;
			return -c *(t)*(t-2) + b;
		}
		/// <summary>
        /// Плавное затухание и отскоки
		/// </summary>
        /// <param name="t">текущее время от начала анимации</param>
        /// <param name="b">начальное значение</param>
        /// <param name="c">разница между начальным и конечным значением</param>
        /// <param name="d">всего времени на анимацию</param>
		/// <returns></returns>
        public static float EaseOutBounce(float t, float b, float c, float d)
		{
            t /= d;
            if (t < (1 / 2.75))
            {
                return c * (float)(7.5625 * t * t) + b;
            }
            else if (t < (2 / 2.75))
            {
                return c * (float)(7.5625 * (t -= (float)(1.5 / 2.75)) * t + .75) + b;
            }
            else if (t < (2.5 / 2.75))
            {
                return c * (float)(7.5625 * (t -= (float)(2.25 / 2.75)) * t + .9375) + b;
            }
            else
            {
                return c * (float)(7.5625 * (t -= (float)(2.625 / 2.75)) * t + .984375) + b;
            }
		}
		/// <summary>
        /// С выскоком
		/// </summary>
        /// <param name="t">текущее время от начала анимации</param>
        /// <param name="b">начальное значение</param>
        /// <param name="c">разница между начальным и конечным значением</param>
        /// <param name="d">всего времени на анимацию</param>
		/// <returns></returns>
        public static float EaseOutElastic(float t, float b, float c, float d)
		{
            if (t == 0) return b; if ((float)(t /= d) == 1) return b + c;  
			float p=d*.3f;
			float a=c; 
			float s=p/4;
            return (float)((a * Math.Pow(2, -10 * t) * Math.Sin((t * d - s) * (2 * Math.PI) / p) + c + b));
        }
        /// <summary>
        /// С затуханием
        /// </summary>
        /// <param name="t"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float EaseOut(float t, float b, float c, float d)
        {
            t /= d/2;
            if (t < 1) return c/2*t*t + b;
            t--;
            return -c/2*(t*(t - 2) - 1) + b;
        }
        /// <summary>
        /// С выскоком
        /// </summary>
        /// <param name="t"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float EaseOutBack(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            return c*((t=t/d-1)*t*((s+1)*t + s) + 1) + b;
        }

        #endregion


    }


}