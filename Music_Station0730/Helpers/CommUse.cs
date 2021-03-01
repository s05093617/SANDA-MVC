using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Music_Station0730.Helpers
{
    public class CommUse
    {
        private SqlConnection _conn = null;
        private SqlCommand _comm = null;
        private SqlDataAdapter _adapter = null;
        private SqlTransaction _trans = null;
        private SqlCommandBuilder _builder = null;
        private string _constr = string.Empty;
        private const int _timeout = 600;

        public CommUse()
        {
            _constr = ConfigurationManager.ConnectionStrings["GMSConnection"].ToString();
        }
        public DataTable GetData(string SqlCmd, bool transNull = true)
        {
            DataTable _result = new DataTable();

            using (_conn = new SqlConnection(_constr))
            {
                using (_comm = new SqlCommand(SqlCmd, _conn))
                {
                    _comm.CommandTimeout = _timeout;
                    using (_adapter = new SqlDataAdapter(_comm))
                    {
                        _adapter.Fill(_result);
                        _result.TableName = "Annonynuse";
                        _adapter.Dispose();
                    }
                    _comm.Dispose();
                    _conn.Close();
                    _conn.Dispose();
                }
            }

            if (transNull)
            {
                _result = DtTransNullColumn(_result);
            }

            return _result;
        }

        public DataTable GetData(string as_SqlCmd, List<SqlParameter> aa_Paras, bool transNull = true)
        {
            DataTable _result = new DataTable();

            using (_conn = new SqlConnection(_constr))
            {
                using (_comm = new SqlCommand(as_SqlCmd, _conn))
                {
                    _comm.CommandTimeout = _timeout;
                    _comm.Parameters.AddRange(aa_Paras.ToArray());
                    using (_adapter = new SqlDataAdapter(_comm))
                    {
                        _adapter.Fill(_result);
                        _comm.Parameters.Clear();
                        _result.TableName = "Annonynuse";
                        _adapter.Dispose();
                    }
                    _comm.Dispose();
                    _conn.Close();
                    _conn.Dispose();
                }
            }

            if (transNull)
            {
                _result = DtTransNullColumn(_result);
            }

            return _result;
        }

        private DataTable DtTransNullColumn(DataTable dt_Temp)
        {
            foreach (DataRow _dr in dt_Temp.Rows)
            {
                foreach (DataColumn _dl in dt_Temp.Columns)
                {
                    if (_dr[_dl] == DBNull.Value)
                    {
                        if (_dl.DataType == typeof(int))
                        {
                            _dr[_dl] = 0;
                        }

                        if (_dl.DataType == typeof(decimal))
                        {
                            _dr[_dl] = decimal.Zero;
                        }

                        if (_dl.DataType == typeof(string))
                        {
                            _dr[_dl] = string.Empty;
                        }

                        if (_dl.DataType == typeof(bool))
                        {
                            _dr[_dl] = false;
                        }
                    }
                    else if (_dl.DataType == typeof(string) && _dr[_dl].ToString().Trim().Equals("null"))
                    {
                        _dr[_dl] = string.Empty;
                    }
                }
            }

            return dt_Temp;
        }
        public string ExecuteNonQuery(string sqlCmd)
        {
            using (_conn = new SqlConnection(_constr))
            {
                _conn.Open();
                using (_trans = _conn.BeginTransaction())
                {
                    try
                    {
                        using (_comm = new SqlCommand(sqlCmd, _conn, _trans))
                        {
                            _comm.CommandTimeout = _timeout;
                            _comm.ExecuteNonQuery();
                            _trans.Commit();
                            //_conn.Close();
                            _comm.Dispose();
                            _conn.Close();
                            _conn.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        _trans.Rollback();
                        return ex.Message;
                    }
                }
            }
            return string.Empty;
        }
        public string ExecuteNonQuery(string sqlCmd, List<SqlParameter> aa_Paras)
        {
            using (_conn = new SqlConnection(_constr))
            {
                _conn.Open();
                using (_trans = _conn.BeginTransaction())
                {
                    try
                    {
                        using (_comm = new SqlCommand(sqlCmd, _conn, _trans))
                        {
                            _comm.CommandTimeout = _timeout;
                            _comm.Parameters.AddRange(aa_Paras.ToArray());
                            _comm.ExecuteNonQuery();
                            _trans.Commit();
                            _comm.Parameters.Clear();
                            //_conn.Close();
                            _comm.Dispose();
                            _conn.Close();
                            _conn.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        _trans.Rollback();
                        return ex.Message;
                    }
                }
            }
            return string.Empty;
        }
        public void SyncTable(string table)
        {
            ExecuteNonQuery("update SyncTables set Changed='Y',UpdateDate=GETDATE() where TableName='" + table + "' ");
        }
    }
}