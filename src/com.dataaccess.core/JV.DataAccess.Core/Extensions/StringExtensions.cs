using System.Collections.Generic;
using System.Linq;

namespace JV.DataAccess.Core.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Splits the provided connection string and splits it into parts.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SplitDataAccessConnectionString(this string connectionString)
        {
            var connectionStringParts = new Dictionary<string, string>();
            bool foundKey = false;
            bool inQuoteValue = false;
            string key = string.Empty, value = string.Empty;
            int startIndex = 0;

            for (int index = 0; index < connectionString.Length; ++index)
            {
                switch (connectionString[index])
                {
                    case '=':
                        if (!inQuoteValue)
                        {
                            // Keys are case insensitive
                            key = connectionString.Substring(startIndex, index - startIndex).Trim().ToLower();
                            foundKey = true;
                            startIndex = index + 1;
                        }
                        break;

                    case ';':
                        if (!inQuoteValue)
                        {
                            // values are case sensitive so do not convert them to lower cases
                            value = connectionString.Substring(startIndex, index - startIndex).Trim();
                            connectionStringParts.Add(key, value);
                            foundKey = false;
                            key = string.Empty;
                            value = string.Empty;
                            startIndex = index + 1;
                        }
                        break;

                    case '\'':
                        inQuoteValue = !inQuoteValue;
                        break;
                }
            }

            if (foundKey) // if key was found but value not added to collection and string ended
            {
                value = connectionString.Substring(startIndex, connectionString.Length - startIndex).Trim();
                connectionStringParts.Add(key, value);
            }

            foreach(string mappedKey in connectionStringParts.Keys.ToList()) // Remove any enclosing quotes in vallue
            {
                connectionStringParts[mappedKey] = connectionStringParts[mappedKey].RemoveEnclosingQuotes();
            }

            return connectionStringParts;
        }

        /// <summary>
        /// If the string is enclosed in single quotes the removes the single quotes and returns the string content
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string RemoveEnclosingQuotes(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            string updatedValue = value;
            if ((value[0] == '\'') && (value[value.Length-1] == '\''))
            {
                updatedValue = value.Substring(1, value.Length - 2);
            }

            return updatedValue;
        }

    }
}
