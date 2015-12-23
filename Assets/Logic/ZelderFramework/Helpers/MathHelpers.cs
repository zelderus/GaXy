using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Object = System.Object;

namespace ZelderFramework.Helpers
{


    /// <summary>
    /// Математика.
    /// </summary>
    public static class MathHelpers
    {

        // Интерполяция
        // для векторов предпочтительней XMVectorLerp
        public static float Lerp(float v0, float v1, float t)
        {
            return v0 + (v1 - v0)*t;
        }


        /// <summary>
        /// Количество процентов.
        /// </summary>
        /// <param name="fullPercents">число представляющее 100%</param>
        /// <param name="number">число</param>
        /// <returns></returns>
        public static float Percent(float fullPercents, float number)
        {
            float onePercent = fullPercents/100;
            float percents = number/onePercent;
            return percents < 0 ? 0 : percents;
        }

        /// <summary>
        /// Количество процентов в диапазоне (диапазон от minNumber до maxNumber является 100%).
        /// </summary>
        /// <param name="minNumber">стартовое значение</param>
        /// <param name="maxNumber">максимальное значение</param>
        /// <param name="number">число</param>
        /// <returns></returns>
        public static float Percent(float minNumber, float maxNumber, float number)
        {
            number = number < minNumber ? minNumber : number > maxNumber ? maxNumber : number;
            float perc100 = maxNumber - minNumber;
            return Percent(perc100, number - minNumber);
        }

        /// <summary>
        /// Число на основе процентов.
        /// </summary>
        /// <param name="fullPercents">число представляющее 100%</param>
        /// <param name="percents">процентов</param>
        /// <returns></returns>
        public static float ByPercent(float fullPercents, float percents)
        {
            float onePercent = fullPercents/100;
            return onePercent*percents;
        }

        /// <summary>
        /// Случайное число.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Int32 GetRandomNumber(Int32 min, Int32 max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
        /// <summary>
        /// Случайное число.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetRandomNumber(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);// + 1.0f);
        }

        /// <summary>
        /// Объект на основе веса.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Object GetByWeight(List<WeightObject> list)
        {
            Int32 maxCount = list.Count();
            var summ = list.Sum(s => s.Weight);
            //- циклом проходим по алгоритму выбора
            for (Int32 x = 0; x < maxCount; x++)
            {
                float n = 0;
                //var summ = list.Sum(s => s.Weight);
                //- случайное число на отрезке суммы весов
                //var sel2 = rnd.Next(1, summ);
                var sel = GetRandomNumber(0.0f, summ);
                foreach (var d in list)
                {
                    n += d.Weight;
                    if (n >= sel)
                    {
                        return d.Obj;
                        break;
                    }
                }
            }
            return list.FirstOrDefault().Obj;
        }

        /// <summary>
        /// Объект на основе веса.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Object GetByWeight(List<WeightIntObject> list)
        {
            Int32 maxCount = list.Count();
            var summ = list.Sum(s => s.Weight);
            //- циклом проходим по алгоритму выбора
            for (Int32 x = 0; x < maxCount; x++)
            {
                float n = 0;
                //var summ = list.Sum(s => s.Weight);
                //- случайное число на отрезке суммы весов
                //var sel2 = rnd.Next(1, summ);
                var sel = GetRandomNumber(1, summ);
                foreach (var d in list)
                {
                    n += d.Weight;
                    if (n >= sel)
                    {
                        return d.Obj;
                        break;
                    }
                }
            }
            return list.FirstOrDefault().Obj;
        }


    }



    public class WeightObject
    {
        public Object Obj;
        public float Weight;
        public WeightObject(Object obj, float weight)
        {
            Obj = obj;
            Weight = weight;
        }
    }
    public class WeightIntObject
    {
        public Object Obj;
        public Int32 Weight;
        public WeightIntObject(Object obj, Int32 weight)
        {
            Obj = obj;
            Weight = weight;
        }
    }

}