using System;
using System.Data;

namespace Gvs.DataAccess.Core
{

    /// <summary>
    /// Implements IDataReader with auto-close feature.
    /// </summary>
    class DataReader : IDataReader
    {

        private IDataReader dataReaderObj;

        public DataReader()
        {
        }

        /// <summary>
        /// Constructor with external Datareader object
        /// </summary>
        /// <param name="dataReader"></param>
        public DataReader(IDataReader dataReader)
        {
            dataReaderObj = dataReader;
        }

        /// <summary>
        /// Close datareader
        /// </summary>
        void IDataReader.Close() => dataReaderObj.Close();

        int IDataReader.Depth => dataReaderObj.Depth;

        System.Data.DataTable IDataReader.GetSchemaTable() => dataReaderObj.GetSchemaTable();

        /// <summary>
        /// Check the data reader is closed or not
        /// </summary>
        bool IDataReader.IsClosed => dataReaderObj.IsClosed;

        bool IDataReader.NextResult()
        {
            bool result = dataReaderObj.NextResult();
            if (!result)
            {
                dataReaderObj.Close();
            }
            return result;
        }

        bool IDataReader.Read()
        {
            bool result = dataReaderObj.Read();
            if (!result)
            {
                dataReaderObj.Close();
            }
            return result;
        }

        int IDataReader.RecordsAffected => dataReaderObj.RecordsAffected;

        /// <summary>
        /// Number of columns in object
        /// </summary>
        int IDataRecord.FieldCount => dataReaderObj.FieldCount;

        bool IDataRecord.GetBoolean(int i) => dataReaderObj.GetBoolean(i);

        byte IDataRecord.GetByte(int i) => dataReaderObj.GetByte(i);

        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => dataReaderObj.GetBytes(i, fieldOffset, buffer, bufferoffset, length);

        char IDataRecord.GetChar(int i) => dataReaderObj.GetChar(i);

        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => dataReaderObj.GetChars(i, fieldoffset, buffer, bufferoffset, length);

        System.Data.IDataReader IDataRecord.GetData(int i) => dataReaderObj.GetData(i);

        string IDataRecord.GetDataTypeName(int i) => dataReaderObj.GetDataTypeName(i);

        System.DateTime IDataRecord.GetDateTime(int i) => dataReaderObj.GetDateTime(i);

        decimal IDataRecord.GetDecimal(int i) => dataReaderObj.GetDecimal(i);

        double IDataRecord.GetDouble(int i) => dataReaderObj.GetDouble(i);

        System.Type IDataRecord.GetFieldType(int i) => dataReaderObj.GetFieldType(i);

        float IDataRecord.GetFloat(int i) => dataReaderObj.GetFloat(i);

        System.Guid IDataRecord.GetGuid(int i) => dataReaderObj.GetGuid(i);

        short IDataRecord.GetInt16(int i) => dataReaderObj.GetInt16(i);

        int IDataRecord.GetInt32(int i) => dataReaderObj.GetInt32(i);

        long IDataRecord.GetInt64(int i) => dataReaderObj.GetInt64(i);

        string IDataRecord.GetName(int i) => dataReaderObj.GetName(i);

        int IDataRecord.GetOrdinal(string name) => dataReaderObj.GetOrdinal(name);

        string IDataRecord.GetString(int i) => dataReaderObj.GetString(i);

        object IDataRecord.GetValue(int i) => dataReaderObj.GetValue(i);

        int IDataRecord.GetValues(object[] values) => dataReaderObj.GetValues(values);

        bool IDataRecord.IsDBNull(int i) => dataReaderObj.IsDBNull(i);

        /// <summary>
        /// Get value based on the column name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object IDataRecord.this[string name] => dataReaderObj[name];

        /// <summary>
        /// Get value based on index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        object IDataRecord.this[int i] => dataReaderObj[i];

        void IDisposable.Dispose() => dataReaderObj.Dispose();

    }
}
