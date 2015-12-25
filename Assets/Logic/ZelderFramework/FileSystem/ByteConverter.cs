using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ZelderFramework.FileSystem
{

    /// <summary>
    /// Конвертирует данные.
    /// </summary>
    public static class ByteConverter
    {
        /// <summary>
        /// Биты на основе данных для сохранения.
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public static byte[] GetBytes(FileManagerData fileData)
        {
            switch (fileData.DataType)
            {
                case FileManagerTypes.Int32: return GetBytes((Int32)fileData.DataValue);
                case FileManagerTypes.Int64: return GetBytes((Int64)fileData.DataValue);
                case FileManagerTypes.Single: return GetBytes((Single)fileData.DataValue);
                case FileManagerTypes.String: return GetBytes(fileData.DataValue.ToString());
                case FileManagerTypes.Boolean: return GetBytes((Boolean)fileData.DataValue);
            }
            return GetBytes(fileData.DataValue.ToString());
        }
        /// <summary>
        /// Загрузка значения из байт.
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="byteValue"></param>
        public static void GetData(FileManagerData fileData, byte[] byteValue)
        {
            switch (fileData.DataType)
            {
                case FileManagerTypes.Int32: fileData.DataValue = GetInt32(byteValue); break;
                case FileManagerTypes.Int64: fileData.DataValue = GetInt64(byteValue); break;
                case FileManagerTypes.Single: fileData.DataValue = GetSingle(byteValue); break;
                case FileManagerTypes.String: fileData.DataValue = GetString(byteValue); break;
                case FileManagerTypes.Boolean: fileData.DataValue = GetBoolean(byteValue); break;
            }
        }
        /// <summary>
        /// Размерность типа.
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public static Int32 GetSize(FileManagerData fileData)
        {
            switch (fileData.DataType)
            {
                case FileManagerTypes.Int32: return sizeof(Int32); break;
                case FileManagerTypes.Int64: return sizeof(Int64); break;
                case FileManagerTypes.Single: return sizeof(Single); break;
                case FileManagerTypes.Boolean: return sizeof(Boolean); break;
                case FileManagerTypes.String: return 0; break;
            }
            return 0;
        }


        public static byte[] GetBytes(String str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static String GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }


        public static byte[] GetBytes(Int32 num)
        {
            return BitConverter.GetBytes(num);
        }
        public static Int32 GetInt32(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }


        public static byte[] GetBytes(Int64 num)
        {
            return BitConverter.GetBytes(num);
        }
        public static Int64 GetInt64(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes, 0);
        }


        public static byte[] GetBytes(Single num)
        {
            return BitConverter.GetBytes(num);
        }
        public static Single GetSingle(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes, 0);
        }

        public static byte[] GetBytes(Boolean val)
        {
            return BitConverter.GetBytes(val);
        }
        public static Boolean GetBoolean(byte[] bytes)
        {
            return BitConverter.ToBoolean(bytes, 0);
        }

    }
}
