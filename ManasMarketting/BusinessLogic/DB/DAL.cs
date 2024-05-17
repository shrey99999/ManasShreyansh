using BusinessLogic.Repository;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace BusinessLogic.Concrete
{
    public class DAL : IDAL
    {
        protected readonly string _ConStr;
        public DAL(string ConStr) => _ConStr = ConStr;

        #region IDAL
        public void Execute(string CommandName)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }
            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn)
                {
                    CommandType = CommandType.Text
                };
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public void Execute(string CommandName, SqlParameter[] param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }
            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn)
                {
                    CommandType = CommandType.Text
                };
                try
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }
        public void Execute(string CommandName, Hashtable param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }
            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn)
                {
                    CommandType = CommandType.Text
                };
                try
                {
                    foreach (DictionaryEntry de in param)
                    {
                        cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                    }
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public void ExecuteProcedure(string CommandName)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }

            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (cmd.Connection.State == ConnectionState.Closed)
                        cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public void ExecuteProcedure(string CommandName, SqlParameter[] param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }

            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }
                    if (cmd.Connection.State == ConnectionState.Closed)
                        cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public DataTable Get(string CommandName)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }

            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    DataTable dt = new DataTable();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        dt.Load(dataReader);
                    }
                    return dt;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public DataTable Get(string CommandName, SqlParameter[] param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }

            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.Text;
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    DataTable dt = new DataTable();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        dt.Load(dataReader);
                    }
                    return dt;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public DataTable GetByProcedure(string CommandName)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }

            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    DataTable dt = new DataTable();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        dt.Load(dataReader);
                    }
                    return dt;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public DataTable GetByProcedure(string CommandName, SqlParameter[] param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }

            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    DataTable dt = new DataTable();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        dt.Load(dataReader);
                    }
                    return dt;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public DataTable GetByProcedureAdapter(string CommandName, SqlParameter[] param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }
            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                    return dt;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }

        public async Task<DataTable> GetByProcedureAdapterAsync(string CommandName, SqlParameter[] param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }
            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        await Task.Run(() =>
                        {
                            da.Fill(dt);
                        });
                    }
                    return dt;

                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }
        public DataSet GetByProcedureAdapterDS(string CommandName, SqlParameter[] param)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }
            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    return ds;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }
        public DataSet GetByProcedureAdapterDS(string CommandName)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                throw new ArgumentException("Cannot be empty", nameof(CommandName));
            }
            using (SqlConnection conn = new SqlConnection(_ConStr))
            {
                SqlCommand cmd = new SqlCommand(CommandName, conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    return ds;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Execption from db:" + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    conn.Close();
                }
            }
        }
        #endregion
    }

    public class ConnectionStr : IConnectionString
    {
        public readonly IConfiguration Configuration;
        private readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;

        public ConnectionStr(IHttpContextAccessor accessor, IWebHostEnvironment env)
        {
            _accessor = accessor;
            _env = env;
            bool IsProd = _env.IsProduction();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile((IsProd ? "appsettings.json" : "appsettings.Development.json"));
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public string GetConnectionString()
        {
            //string datasource = "DESKTOP-Q0OPBG9\\SQLEXPRESS";
            //string dbname = "ManasMarketing";
            //string username = "sa";
            //string password = "sql4me";

            //string datasource = "23.88.33.9";
            //string dbname = "mmdb";
            //string username = "admin_mm";
            //string password = "rhet7436574##";
            //var connectionString = string.Format(Configuration["ConnectionStrings:dbcon"], datasource, dbname, username, password);
            string datasource = "DESKTOP-JM854VE";
            string dbname = "ERP3";
            var connectionString = string.Format(Configuration["ConnectionStrings:dbcon"], datasource, dbname);
            return connectionString;
        }

    }
}