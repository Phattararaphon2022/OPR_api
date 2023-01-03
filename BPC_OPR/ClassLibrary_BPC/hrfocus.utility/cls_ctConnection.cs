using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ClassLibrary_BPC
{
    public class cls_ctConnection
    {
        public string Server {get;set;}
        public string Database { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }

        public string ConnectionString { get; set; }

        SqlConnection Oj_conn = new SqlConnection();
        SqlTransaction Oj_tran;

        public SqlConnection getConnection() { return this.Oj_conn; }
        public SqlTransaction getTransaction() { return this.Oj_tran; }

        public string Message { get; set; }

        public cls_ctConnection()
        {
            
            this.Server = Config.Server;
            this.Database = Config.Database;
            this.Userid = Config.Userid;
            this.Password = Config.Password;

            this.ConnectionString = "Server = " + this.Server + "; Database = " + this.Database + "; User Id = " + this.Userid + ";Password = " + this.Password + ";";

        }
        
        public cls_ctConnection(string server, string database, string userid, string password)
        {
            this.Server = server;
            this.Database = database;
            this.Userid = userid;
            this.Password = password;

            this.ConnectionString = "Server = " + this.Server + "; Database = " + this.Database + "; User Id = " + this.Userid + ";Password = " + this.Password + ";";
        }
        
        public bool doConnect()
        {
            bool blnResult = true;
            try
            {
                if (this.ConnectionString.Equals(""))
                {
                    Message = "ERR-CON001:Not define connection";
                    return false;
                }

                if (Oj_conn.State == ConnectionState.Closed)
                {
                    Oj_conn = new SqlConnection(this.ConnectionString);
                    Oj_conn.Open();
                    Message = "Connection success";
                }

                
            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERR-CON001:" + ex.ToString();
            }
            return blnResult;
        }

        public bool doClose()
        {
            bool blnResult = true;
            try
            {
                Oj_conn.Close();
                Oj_conn.Dispose();
            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERR-CON002:" + ex.ToString();
            }
            return blnResult;
        }

        public bool doOpenTransaction()
        {
            bool blnResult = true;
            try
            {
                
                // Start a local transaction.
                Oj_tran = Oj_conn.BeginTransaction("SampleTransaction");

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERR-CON003:" + ex.ToString();
            }
            return blnResult;
        }

        public bool doRollback()
        {
            bool blnResult = true;
            try
            {

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERR-CON004:" + ex.ToString();
            }
            return blnResult;
        }

        public bool doCommit()
        {
            bool blnResult = true;
            try
            {
                Oj_tran.Commit();
            }
            catch (Exception ex)
            {
                Oj_tran.Rollback();
                blnResult = false;
                Message = "ERR-CON005:" + ex.ToString();
            }
            return blnResult;
        }

        public bool doExecuteSQL(string sql)
        {
            bool blnResult = true;

            doConnect();

            try
            {
                
                SqlCommand cmd = new SqlCommand();
                Int32 rowsAffected;

                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = Oj_conn;

                rowsAffected = cmd.ExecuteNonQuery();

                //if(rowsAffected > 0)
                    blnResult = true;
                //else
                //    blnResult = false;

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERR-CON006:" + ex.ToString();
            }
            finally
            {
                doClose();
            }

            return blnResult;
        }

        public bool doExecuteSQL_transaction(string sql)
        {
            bool blnResult = true;
            try
            {
                //doConnect();

                SqlCommand cmd = new SqlCommand();
                Int32 rowsAffected;

                cmd.Transaction = this.Oj_tran;

                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = Oj_conn;

                rowsAffected = cmd.ExecuteNonQuery();

                //if(rowsAffected > 0)
                blnResult = true;
                //else
                //    blnResult = false;

            }
            catch (Exception ex)
            {
                blnResult = false;
                Message = "ERR-CON006:" + ex.ToString();
            }
            return blnResult;
        }

        public DataTable doGetTable(string sql)
        {
            DataTable dt = new DataTable();

            doConnect();

            try
            {                
                using (SqlCommand cmd = new SqlCommand(sql, Oj_conn))
                {
                    //Console.WriteLine("command created successfuly");
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                                        
                    //Console.WriteLine("connection opened successfuly");
                    adapt.Fill(dt);
                    //conn.Close();
                    //Console.WriteLine("connection closed successfuly");
                }

            }
            catch (Exception ex)
            {
                dt = null;
                Message = "ERR-CON007:" + ex.ToString();
            }
            finally
            {
                doClose();
            }

            return dt;
        }

        public DataTable doGetTableTransaction(string sql)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, Oj_conn))
                {
                    cmd.Transaction = this.Oj_tran;
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    adapt.Fill(dt);                    
                }

            }
            catch (Exception ex)
            {
                dt = null;
                Message = "ERR-CON009:" + ex.ToString();
            }
            
            return dt;
        }

        public DataSet doGetDataSource(string sql)
        {
            DataSet ds = null;
            try
            {

            }
            catch (Exception ex)
            {
                ds = null;
                Message = "ERR-CON008:" + ex.ToString();
            }
            return ds;
        }


    }
}
