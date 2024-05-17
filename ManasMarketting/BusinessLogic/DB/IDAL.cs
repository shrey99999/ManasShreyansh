using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IDAL
    {
        void Execute(string CommandName);
        void Execute(string CommandName, SqlParameter[] param);
        void Execute(string CommandName, Hashtable param);
        void ExecuteProcedure(string CommandName);
        void ExecuteProcedure(string CommandName, SqlParameter[] param);
        DataTable Get(string CommandName);
        DataTable Get(string CommandName, SqlParameter[] param);
        DataTable GetByProcedure(string CommandName);
        DataTable GetByProcedure(string CommandName, SqlParameter[] param);
        DataTable GetByProcedureAdapter(string CommandName, SqlParameter[] param);

        Task<DataTable> GetByProcedureAdapterAsync(string CommandName, SqlParameter[] param);

        DataSet GetByProcedureAdapterDS(string CommandName, SqlParameter[] param);
        DataSet GetByProcedureAdapterDS(string CommandName);
    }

    public interface IConnectionString
    {
        string GetConnectionString();
       // string GetConnectionStringFrontend();

    }
}
