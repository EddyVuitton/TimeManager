﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;
using System.Globalization;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.Extensions;

public static class DbContextExtension
{
    private static readonly string _databaseConnectionString = ConfigurationHelper.DatabaseConnectionString;

    #region PublicMethods

    public static async Task<List<T>?> SqlQueryAsync<T>(this DbContext db, string sql, object[]? parameters = null, CancellationToken cancellationToken = default) where T : class
    {
        parameters ??= [];

        if (typeof(T).GetProperties().Any())
        {
            return await db.Set<T>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
        }
        else
        {
            await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
            return default;
        }
    }

    public static async Task SqlQueryAsync(this DbContext db, string sql, object[]? parameters = null, CancellationToken cancellationToken = default)
    {
        parameters ??= [];
        await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }

    public static async Task<Hashtable> SqlQuerySingleRowAsync(this DbContext db, string query, Hashtable? parameters = null)
    {
        var dT = new DataTable();
        using var connection = new SqlConnection(_databaseConnectionString);
        await connection.OpenAsync();

        using var cmd = new SqlCommand(query, connection);
        PrepareParams(cmd, parameters);
        using var reader = await cmd.ExecuteReaderAsync();
        dT.Load(reader);
        await connection.CloseAsync();

        var columns = dT.Columns;
        var rowCount = dT.Rows.Count;
        var hT = new Hashtable();

        if (rowCount <= 1)
        {
            if (parameters is not null)
            {
                foreach (object key in parameters.Keys)
                {
                    var paramName = key.ToString() ?? string.Empty;

                    if (paramName.StartsWith("@out_", false, CultureInfo.CurrentCulture))
                    {
                        var columnName = paramName;
                        var row = cmd.Parameters[paramName].Value;

                        hT.Add(columnName, row);
                    }
                }
            }

            if (rowCount == 0)
                return hT;

            foreach (DataColumn c in columns)
            {
                var columnName = c.ToString();
                var row = dT.Rows[0];

                hT.Add(columnName, row[columnName].ToString());
            }

            return hT;
        }
        else
            throw new Exception($"Not expected more than 1 row");
    }

    public static async Task<List<Hashtable>> SqlQueryMultipleRowsAsync(this DbContext db, string query, Hashtable? parameters = null)
    {
        var dT = new DataTable();
        using (var connection = new SqlConnection(_databaseConnectionString))
        {
            await connection.OpenAsync();

            using var cmd = new SqlCommand(query, connection);
            PrepareParams(cmd, parameters);
            using var reader = await cmd.ExecuteReaderAsync();
            dT.Load(reader);
        }

        var rowCount = dT.Rows.Count;
        var list = new List<Hashtable>();

        if (rowCount >= 0)
        {
            if (rowCount == 0)
                return list;

            for (int i = 0; i < rowCount; i++)
            {
                var row = dT.Rows[i];
                var hT = new Hashtable();

                foreach (DataColumn c in dT.Columns)
                {
                    var columnName = c.ToString();

                    hT.Add(columnName, row[columnName].ToString());
                }

                list.Add(hT);
            }
        }
        else
        {
            var hT = new Hashtable();
            list.Add(hT);
        }

        return list;
    }

    public static object CreateParameter(this DbContext db, string name, object? value, SqlDbType type)
    {
        return new SqlParameter()
        {
            ParameterName = name,
            Value = value ?? DBNull.Value,
            SqlDbType = type
        };
    }

    #endregion PublicMethods

    #region PrivateMethods

    private static void PrepareParams(SqlCommand cmd, Hashtable? parameters)
    {
        if (parameters is null)
            return;

        foreach (object key in parameters.Keys)
        {
            SqlParameter param = new();
            string typeName = "DBNull";

            if (parameters[key] != null && parameters[key] != DBNull.Value)
                typeName = parameters[key]!.GetType().Name;

            var pName = key.ToString();
            int pSize = 0;

            if (pName!.Contains(':'))
            {
                string[] arr = pName.Split(':');
                pName = arr[0];
                pSize = Convert.ToInt32(arr[1], System.Globalization.CultureInfo.CurrentCulture);
            }

            switch (typeName)
            {
                case "String":
                    param = new SqlParameter(pName, SqlDbType.NVarChar);
                    pSize = (pSize == 0 ? 4000 : pSize);
                    break;

                case "Int32":
                    param = new SqlParameter(pName, SqlDbType.Int);
                    break;

                case "Int16":
                    param = new SqlParameter(pName, SqlDbType.SmallInt);
                    break;

                case "Int64":
                    param = new SqlParameter(pName, SqlDbType.BigInt);
                    break;

                case "Decimal":
                    param = new SqlParameter(pName, SqlDbType.Decimal);
                    break;

                case "Double":
                    param = new SqlParameter(pName, SqlDbType.Float);
                    break;

                case "DateTime":
                    param = new SqlParameter(pName, SqlDbType.DateTime);
                    break;

                case "Boolean":
                    param = new SqlParameter(pName, SqlDbType.Bit);
                    break;

                case "Byte[]":
                    param = new SqlParameter(pName, SqlDbType.NText);
                    break;

                case "DBNull":
                    param = new SqlParameter(pName, SqlDbType.NVarChar);
                    break;

                case "SqlBytes":
                    param = new SqlParameter(pName, SqlDbType.Binary);
                    break;

                case "DataTable":
                    param = new SqlParameter(pName, SqlDbType.Structured);
                    break;

                case "Guid":
                    param = new SqlParameter(pName, SqlDbType.UniqueIdentifier);
                    break;
            }

            if (param is not null)
            {
                if (typeName == "DBNull")
                {
                    param.Value = DBNull.Value;
                }
                else if (pName.StartsWith("@OUT_", true, CultureInfo.CurrentCulture))
                {
                    param.Direction = ParameterDirection.Output;
                    param.Value = parameters[key];
                    if (pSize != 0)
                    {
                        param.Size = pSize;
                    }
                }
                else
                {
                    param.Value = parameters[key];
                }

                cmd.Parameters.Add(param);
            }
            else
                throw new Exception($"Unsupported type [{typeName}] for parameter [{pName}]");
        }
    }

    #endregion PrivateMethods
}