using System;
using System.Text;
using UnityEngine;
using System.Collections;

public static class StringHelper
{
    /// <summary>
    /// Случайная строка размерности указанной.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static String GetRandomString(Int32 min, Int32 max)
    {
        return GetRandomAnsiString(UnityEngine.Random.Range(min, max));
    }
    /// <summary>
    /// Возврат случайной строки.
    /// </summary>
    /// <param name="stringLenght"></param>
    /// <returns></returns>
    private static String GetRandomAnsiString(Int32 stringLenght)
    {
        StringBuilder str = new StringBuilder();
        for (Int32 step = 0; step <= stringLenght; step++)
        {
            str.Append(GetRandomAnsiCodeForEng());
        }
        return str.ToString();
    }
    /// <summary>
    /// Случайный символ.
    /// <remarks>В диапазоне кодов 97-122 (a-z)</remarks>
    /// </summary>
    /// <returns></returns>
    private static Char GetRandomAnsiCodeForEng()
    {
        return (Char)UnityEngine.Random.Range(97, 122);
    }
}
