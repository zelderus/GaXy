using System;
using System.Collections.Generic;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using Polenter.Serialization;
using UnityEngine;

namespace Assets.Scripts.ZelderFramework.FileSystem
{

    
    /// <summary>
    /// Работа с файлами.
    /// </summary>
    public static class FileManager
    {
        #region depracated
        ///// <summary>
        ///// Сохранение файла.
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <param name="serializableObject">класс с аттрибутом 'System.Serializable' и все дочерние тоже.</param>
        //public static void _Save(String fileName, System.Object serializableObject)
        //{
        //    //var serializer = new SharpSerializer();
        //    //serializer.Serialize(serializableObject, String.Format("{0}/{1}", Application.persistentDataPath, fileName));

        //    //var bf = new BinaryFormatter();
        //    //FileStream file = File.Create(String.Format("{0}/{1}", Application.persistentDataPath, fileName));
        //    //bf.Serialize(file, serializableObject);
        //    //file.Close();
        //}

        ///// <summary>
        ///// Загрузка файла.
        ///// </summary>
        ///// <typeparam name="T">класс с аттрибутом 'System.Serializable' и все дочерние тоже.</typeparam>
        ///// <param name="fileName"></param>
        ///// <param name="defaultData"></param>
        ///// <returns></returns>
        //public static T _Load<T>(String fileName, T defaultData = default(T))
        //{
        //    T ret = defaultData;//default(T);

        //    //var serializer = new SharpSerializer();
        //    //try
        //    //{
        //    //    ret = (T) serializer.Deserialize(String.Format("{0}/{1}", Application.persistentDataPath, fileName));
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    ret = defaultData;
        //    //}

        //    //if (File.Exists(String.Format("{0}/{1}", Application.persistentDataPath, fileName)))
        //    //{
        //    //    var bf = new BinaryFormatter();
        //    //    FileStream file = File.Open(String.Format("{0}/{1}", Application.persistentDataPath, fileName), FileMode.Open);
        //    //    ret = (T)bf.Deserialize(file);
        //    //    file.Close();
        //    //}
        //    return ret;
        //}
        #endregion


        private static void SaveDatas(FileStream file, List<FileManagerData> datas)
        {
            foreach (var data in datas)
            {
                var bytes = ByteConverter.GetBytes(data);
                //- размер
                if (data.DataType == FileManagerTypes.String)
                {
                    var sizeInt = ByteConverter.GetBytes(bytes.Length);
                    file.Write(sizeInt, 0, sizeInt.Length);
                }
                //- данные
                file.Write(bytes, 0, bytes.Length);
            }
        }
        /// <summary>
        /// Сохранение файла.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="datas"></param>
        public static void Save(String fileName, List<FileManagerData> datas)
        {
            FileStream file = File.Create(String.Format("{0}/{1}", Application.persistentDataPath, fileName));
            SaveDatas(file, datas);
            file.Close();
        }
        /// <summary>
        /// Сохранение множества классов в один файл в строгйо последовательности.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="objs"></param>
        public static void Save(String fileName, List<FileManagedClass> objs)
        {
            FileStream file = File.Create(String.Format("{0}/{1}", Application.persistentDataPath, fileName));
            foreach (var fileManagedClass in objs)
            {
                SaveDatas(file, fileManagedClass.ConvertToSaveData());
            }
            file.Close();
        }

        private static void LoadDatas(FileStream file, List<FileManagerData> datas)
        {
            foreach (var fileManagerData in datas)
            {
                //- размер
                var sizeInt = ByteConverter.GetSize(fileManagerData);
                if (fileManagerData.DataType == FileManagerTypes.String)
                {
                    var sizeBytes = new byte[sizeof (Int32)];
                    var si = file.Read(sizeBytes, 0, sizeBytes.Length);
                    sizeInt = ByteConverter.GetInt32(sizeBytes);
                }
                //- значение
                var dataBytes = new byte[sizeInt];
                var di = file.Read(dataBytes, 0, dataBytes.Length);
                ByteConverter.GetData(fileManagerData, dataBytes);
            }
        }
        /// <summary>
        /// Загрузка файла.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static List<FileManagerData> Load(String fileName, List<FileManagerData> datas)
        {
            if (File.Exists(String.Format("{0}/{1}", Application.persistentDataPath, fileName)))
            {
                FileStream file = File.Open(String.Format("{0}/{1}", Application.persistentDataPath, fileName), FileMode.Open);
                LoadDatas(file, datas);
                file.Close();
            }
            return datas;
        }
        /// <summary>
        /// Загрузка множества классов. Строгая последовательность как и при сохранении в этот файл.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="objs"></param>
        public static void Load(String fileName, List<FileManagedClass> objs)
        {
            if (File.Exists(String.Format("{0}/{1}", Application.persistentDataPath, fileName)))
            {
                FileStream file = File.Open(String.Format("{0}/{1}", Application.persistentDataPath, fileName), FileMode.Open);
                //Debug.Log(String.Format("{0}/{1}", Application.persistentDataPath, fileName));
                foreach (var fileManagedClass in objs)
                {
                    var datas = fileManagedClass.ConvertToSaveData();
                    LoadDatas(file, datas);
                    fileManagedClass.LoadFromSaveData(datas);
                }
                file.Close();
            }
        }



        /// <summary>
        /// Удаление файла.
        /// </summary>
        /// <param name="fileName"></param>
        public static void Delete(String fileName)
        {
            if (File.Exists(String.Format("{0}/{1}", Application.persistentDataPath, fileName)))
            {
                File.Delete(fileName);
            }
        }

    }
}
