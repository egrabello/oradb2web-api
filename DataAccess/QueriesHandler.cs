using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess
{
    public class QueriesHandler
    {
        
        
                
        public async Task<List<Dictionary<string, string>>> Query(string query, string connectionName)
        {
            connectionName = String.IsNullOrEmpty(connectionName) ? "oracle_db" : connectionName;
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ToString();
            var rowsResult = await Task.Run(() =>
            {
                try
                {
                   
                    using (OracleConnection dbcon = new OracleConnection(connectionString))
                    {
                        dbcon.Open();
                        using (OracleCommand dbcmd = dbcon.CreateCommand())
                        {
                            //dbcmd.CommandTimeout = 0;
                            
                            dbcmd.CommandText = query;

                            using (OracleDataReader reader = dbcmd.ExecuteReader())
                            {
                                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

                                //Stopwatch timer = Stopwatch.StartNew();                                 
                                
                                while (reader.Read())
                                {

                                    var row = new Dictionary<string, string>();
                                    for (var i = 0; i < reader.FieldCount; i++)
                                    {
                                        var columnName = reader.GetName(i);
                                        var columnValue = reader.GetValue(i).ToString(); // reader.GetInt32(i);
                                        row.Add(columnName, columnValue?.ToString());
                                        //MakeRow(reader, row, i);
                                    }
                                    rows.Add(row);
                                }
                                //var tempo = timer.ElapsedMilliseconds;
                                //Debug.WriteLine("Tempo relatório: {0}", tempo);

                                // clean up
                                reader.Close();                               
                                dbcmd.Dispose();                                
                                dbcon.Close();
                               

                                return rows;
                            }                            
                        }                    
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

            });
            return rowsResult;
        }

        
        

       

        private dynamic getDataValue(int index, OracleDataReader reader)
        {
            string dataTypeName = reader.GetFieldType(index)?.Name;
            dynamic dataValue = null;

            try
            {
                switch (dataTypeName)
                {
                    case "String":
                        dataValue = reader.IsDBNull(index) ? null : RemoveSpecialCharacters(reader.GetString(index));
                        break;
                    case "Int16":
                        dataValue = reader.GetInt16(index);
                        break;
                    case "Int32":
                        dataValue = reader.GetInt32(index);
                        break;
                    case "Int64":
                        dataValue = reader.GetInt64(index);
                        break;
                    case "Double":
                        dataValue = reader.GetDouble(index);
                        break;
                    case "Float":
                        dataValue = reader.GetFloat(index);
                        break;
                    case "Decimal":
                        dataValue = reader.GetDecimal(index);
                        break;
                    case "DateTime":
                        dataValue = reader.GetDateTime(index);
                        break;
                    case "Guid":
                        dataValue = reader.GetGuid(index);
                        break;
                    default:
                        dataValue = null;
                        break;
                }

                return dataValue;
            }
            catch (SqlNullValueException)
            {
                return dataValue;
            }



        }

        private static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.\\-\\s]+", "", RegexOptions.Compiled);
        }
            
    }
}
