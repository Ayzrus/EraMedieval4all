using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Class
{
    static class MySqlExtensions
    {
        /// <summary>
        /// Verifica se o resultado é DBNull antes de fazer o cast para o tipo <typeparamref name="T"/>.
        /// </summary>
        public static T Cast<T>(this MySqlDataReader reader, string field, T defaultValue = default)
        {
            if (reader[field] == DBNull.Value)
                return defaultValue;
            else
                return (T)reader[field];
        }

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}
