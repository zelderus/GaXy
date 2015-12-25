using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ZelderFramework.FileSystem
{
    /// <summary>
    /// Типы данных.
    /// </summary>
    public enum FileManagerTypes
    {
        Int32 = 0,
        Int64 = 1,
        Single = 2,
        String = 3,
        Boolean = 4
    }
    /// <summary>
    /// Данные для сохранения.
    /// </summary>
    public class FileManagerData
    {
        public FileManagerTypes DataType { get; set; }
        public System.Object DataValue { get; set; }

        public FileManagerData()
        {
            DataType = FileManagerTypes.String;
            DataValue = null;
        }
        public FileManagerData(FileManagerTypes dataType)
        {
            DataType = dataType;
        }
        public FileManagerData(FileManagerTypes dataType, System.Object dataValue)
            : this(dataType)
        {
            DataValue = dataValue;
        }
    }


    /// <summary>
    /// Класс для записи.
    /// </summary>
    public abstract class FileManagedClass
    {
        /// <summary>
        /// Список данных для сохранения.
        /// </summary>
        /// <returns></returns>
        public abstract List<FileManagerData> ConvertToSaveData();
        /// <summary>
        /// Загрузка из данных.
        /// </summary>
        /// <param name="datas"></param>
        public abstract void LoadFromSaveData(List<FileManagerData> datas);

        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(String fileName)
        {
            FileManager.Save(fileName, ConvertToSaveData());
        }
        public virtual void Load(String fileName)
        {
            var datas = FileManager.Load(fileName, ConvertToSaveData());
            LoadFromSaveData(datas);
        }

    }



}
