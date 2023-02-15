using System;
using System.Collections.Generic;
using System.Data;

namespace JV.DataAccess.Core.Helpers
{
    public class Transform<T> where T : class, new()
    {

        private EntityMapping Map { get; set; }

        public Transform(IDataAccess dbConnection, IDataReader reader)
        {
            var mapper = new Mapper<T>();
            var map = mapper.Map((DataAccessBase)dbConnection);
            map.ElementMap = SyncMapWithReader(map.ElementMap, reader);
            Map = map;
        }

        private List<FieldPropertyMapping> SyncMapWithReader(List<FieldPropertyMapping> elementMap, IDataReader reader)
        {
            var updatedEntityMapping = new List<FieldPropertyMapping>();
            var readerFields = extractReaderFields(reader);

            elementMap.ForEach(eleMap =>
            {
                if (readerFields.ContainsKey(eleMap.FieldName.ToUpper()))
                {
                    updatedEntityMapping.Add(eleMap);
                }
            });

            return updatedEntityMapping;
        }

        private Dictionary<string, Type> extractReaderFields(IDataReader reader)
        {
            Dictionary<string, Type> readerFields = new Dictionary<string, Type>();
            for (int fieldIndex = 0; fieldIndex < reader.FieldCount; ++fieldIndex)
            {
                readerFields.Add(reader.GetName(fieldIndex).ToUpper(), reader.GetFieldType(fieldIndex));
            }

            return readerFields;
        }

        public T Read(IDataReader reader)
        {
            T customObject = new T();
            Type objectType = customObject.GetType();

            reader.Read();
            ReadFields(reader, customObject, objectType);

            return customObject;
        }

        public List<T> ReadAll(IDataReader reader)
        {
            T customObject = new T();
            Type objectType = customObject.GetType();
            List<T> customList = new List<T>();

            while (reader.Read())
            {
                customObject = new T();
                ReadFields(reader, customObject, objectType);
                customList.Add(customObject);
            }

            return customList;
        }

        #region Converters

        protected bool ConvertToBoolean(object field) => field == DBNull.Value ? false : Convert.ToBoolean(field);

        protected DateTime ConvertToDateTime(object field) => (field == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(field);

        protected decimal ConvertToDecimal(object field) => field == DBNull.Value ? 0 : Convert.ToDecimal(field);

        protected double ConvertToDouble(object field) => field == DBNull.Value ? 0 : Convert.ToDouble(field);

        #endregion

        private void ReadFields(IDataReader reader, T customObject, Type objectType)
        {
            foreach (FieldPropertyMapping map in Map.ElementMap)
            {
                objectType.GetProperty(map.PropertyName)
                    .SetValue(customObject, ReadValue(reader, map.FieldName, map.TypeName), null);
            }
        }

        private object ReadValue(IDataReader reader, string fieldName, string typeName)
        {
            object field = reader[fieldName];
            object value = null;

            switch (typeName)
            {
                case "Int16":
                    value = field == DBNull.Value ? 0 : Convert.ToInt16(field);
                    break;

                case "Int32":
                    value = field == DBNull.Value ? 0 : Convert.ToInt32(field);
                    break;

                case "Int64":
                    value = field == DBNull.Value ? 0 : Convert.ToInt64(field);
                    break;

                case "Single":
                    value = field == DBNull.Value ? 0 : Convert.ToSingle(field);
                    break;

                case "Double":
                    value = field == DBNull.Value ? 0 : Convert.ToDouble(field);
                    break;

                case "String":
                    value = field == DBNull.Value ? string.Empty : Convert.ToString(field);
                    break;

                case "DateTime":
                    value = field == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(field);
                    break;
            }

            return value;
        }

    }
}
