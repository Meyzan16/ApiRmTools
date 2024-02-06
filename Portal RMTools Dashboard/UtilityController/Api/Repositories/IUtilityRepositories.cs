using Api.ViewModels;
using Api.Models.SQLServer;
using Api.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Transactions;
using Oracle.ManagedDataAccess.Client;
using Api.Components;

namespace Api.Repositories
{
    public interface IUtilityRepo
    {
        Task<(bool status, string error, List<DropdownDataResponse> list, int resultCount)> LoadDataAsyncRmTransaksi();
        Task<(bool status, string error, DropdownDataResponse data)> LoadDataByParamAsyncRmTransaksi(int userId);

        Task<(bool status, string error, List<DropdownDataResponse> list, int resultCount)> LoadDataAsyncRMBAKomersial();
        Task<(bool status, string error, DropdownDataResponse data)> LoadDataByParamAsyncRMBAKomersial(int userId);

        Task<(bool status, string error, List<DropdownDataResponse> list, int resultCount)> LoadDataAsyncRMBACorporasi();
        Task<(bool status, string error, DropdownDataResponse data)> LoadDataByParamAsyncRMBACorporasi(int userId);
    }

    public class UtilityRepo : IUtilityRepo
    {
        public readonly ITokenManager _tokenManager;
        private readonly dbRmTools_Context _context;
        public UtilityRepo(ITokenManager tokenManager,dbRmTools_Context context)
        {
            _tokenManager = tokenManager;
            _context = context;
        }

        #region DropdownRMTransaksi
        public async Task<(bool status, string error, List<DropdownDataResponse> list, int resultCount)> LoadDataAsyncRmTransaksi()
        {
            try
            {
                List<DropdownDataResponse> result = new List<DropdownDataResponse>();

                var connectionString = GetConfig.AppSetting["ConnectionStrings:dbOracle"];
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.CommandText = @"
                                            SELECT 
                                                AU.APPOWNERID, 
                                                AU.USERID, 
                                                AU.USERNAME, 
                                                A.NAME AS ROLENAME 
                                            FROM 
                                                AZ_USER AU
                                            LEFT JOIN 
                                                AZ_ROLEASSIGNMENT AR ON AU.APPOWNERID = AR.APPOWNERID AND AU.USERID = AR.MEMBERID
                                            INNER JOIN 
                                                AZ_ROLE A ON AR.APPOWNERID = A.APPOWNERID AND AR.ROLEID = A.ROLEID
                                            WHERE 
                                                AU.APPOWNERID = 955 
                                                AND A.ROLEID = 1017
                                            ORDER BY 
                                                AU.USERID, 
                                                A.ROLEID";

                    OracleDataReader reader = command.ExecuteReader();


                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DropdownDataResponse role = new DropdownDataResponse()
                            {
                                Appownerid = Convert.IsDBNull(reader["APPOWNERID"]) ? 0 : Convert.ToInt32(reader["APPOWNERID"]),
                                UserId = Convert.IsDBNull(reader["USERID"]) ? 0 : Convert.ToInt32(reader["USERID"]),
                                Username = Convert.IsDBNull(reader["USERNAME"]) ? null : Convert.ToString(reader["USERNAME"]),
                                Rolename = Convert.IsDBNull(reader["ROLENAME"]) ? null : Convert.ToString(reader["ROLENAME"])

                            };

                            result.Add(role);
                        }
                    }
                    connection.Close();
                }
                return (true, "", result, result.Count);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new List<DropdownDataResponse>(), 0);
            }
        }
        #endregion

        #region DropdownRMTransaksiByParams
        public async Task<(bool status, string error, DropdownDataResponse data)> LoadDataByParamAsyncRmTransaksi(int userId)
        {
            try
            {
                DropdownDataResponse result = null;

                var connectionString = GetConfig.AppSetting["ConnectionStrings:dbOracle"];
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.CommandText = @"
                                            SELECT 
                                                AU.APPOWNERID, 
                                                AU.USERID, 
                                                AU.USERNAME, 
                                                A.NAME AS ROLENAME 
                                            FROM 
                                                AZ_USER AU
                                            LEFT JOIN 
                                                AZ_ROLEASSIGNMENT AR ON AU.APPOWNERID = AR.APPOWNERID AND AU.USERID = AR.MEMBERID
                                            INNER JOIN 
                                                AZ_ROLE A ON AR.APPOWNERID = A.APPOWNERID AND AR.ROLEID = A.ROLEID
                                            WHERE 
                                                AU.APPOWNERID = 955 
                                                AND A.ROLEID = 1017
                                                AND AU.USERID = :UserID";
                    command.Parameters.Add(new OracleParameter(":UserID", OracleDbType.Int32)).Value = userId;

                    OracleDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return (false, "User Id not found", result);
                    }
                    else
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = new DropdownDataResponse()
                                {
                                    Appownerid = Convert.IsDBNull(reader["APPOWNERID"]) ? 0 : Convert.ToInt32(reader["APPOWNERID"]),
                                    UserId = Convert.IsDBNull(reader["USERID"]) ? 0 : Convert.ToInt32(reader["USERID"]),
                                    Username = Convert.IsDBNull(reader["USERNAME"]) ? null : Convert.ToString(reader["USERNAME"]),
                                    Rolename = Convert.IsDBNull(reader["ROLENAME"]) ? null : Convert.ToString(reader["ROLENAME"])

                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return (true, "", result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new DropdownDataResponse());
            }
        }
        #endregion

        #region DropdownRMBAKomersial
        public async Task<(bool status, string error, List<DropdownDataResponse> list, int resultCount)> LoadDataAsyncRMBAKomersial()
        {
            try
            {
                List<DropdownDataResponse> result = new List<DropdownDataResponse>();

                var connectionString = GetConfig.AppSetting["ConnectionStrings:dbOracle"];
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.CommandText = @"
                                            SELECT 
                                                AU.APPOWNERID, 
                                                AU.USERID, 
                                                AU.USERNAME, 
                                                A.NAME AS ROLENAME 
                                            FROM 
                                                AZ_USER AU
                                            LEFT JOIN 
                                                AZ_ROLEASSIGNMENT AR ON AU.APPOWNERID = AR.APPOWNERID AND AU.USERID = AR.MEMBERID
                                            INNER JOIN 
                                                AZ_ROLE A ON AR.APPOWNERID = A.APPOWNERID AND AR.ROLEID = A.ROLEID
                                            WHERE 
                                                AU.APPOWNERID = 955 
                                                AND A.ROLEID = 1013
                                            ORDER BY 
                                                AU.USERID, 
                                                A.ROLEID";

                    OracleDataReader reader = command.ExecuteReader();


                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DropdownDataResponse role = new DropdownDataResponse()
                            {
                                Appownerid = Convert.IsDBNull(reader["APPOWNERID"]) ? 0 : Convert.ToInt32(reader["APPOWNERID"]),
                                UserId = Convert.IsDBNull(reader["USERID"]) ? 0 : Convert.ToInt32(reader["USERID"]),
                                Username = Convert.IsDBNull(reader["USERNAME"]) ? null : Convert.ToString(reader["USERNAME"]),
                                Rolename = Convert.IsDBNull(reader["ROLENAME"]) ? null : Convert.ToString(reader["ROLENAME"])

                            };

                            result.Add(role);
                        }
                    }
                    connection.Close();
                }
                return (true, "", result, result.Count);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new List<DropdownDataResponse>(), 0);
            }
        }
        #endregion

        #region DropdownRMBAKomersialByParams
        public async Task<(bool status, string error, DropdownDataResponse data)> LoadDataByParamAsyncRMBAKomersial(int userId)
        {
            try
            {
                DropdownDataResponse result = null;

                var connectionString = GetConfig.AppSetting["ConnectionStrings:dbOracle"];
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.CommandText = @"
                                            SELECT 
                                                AU.APPOWNERID, 
                                                AU.USERID, 
                                                AU.USERNAME, 
                                                A.NAME AS ROLENAME 
                                            FROM 
                                                AZ_USER AU
                                            LEFT JOIN 
                                                AZ_ROLEASSIGNMENT AR ON AU.APPOWNERID = AR.APPOWNERID AND AU.USERID = AR.MEMBERID
                                            INNER JOIN 
                                                AZ_ROLE A ON AR.APPOWNERID = A.APPOWNERID AND AR.ROLEID = A.ROLEID
                                            WHERE 
                                                AU.APPOWNERID = 955 
                                                AND A.ROLEID = 1013
                                                AND AU.USERID = :UserID";
                    command.Parameters.Add(new OracleParameter(":UserID", OracleDbType.Int32)).Value = userId;

                    OracleDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return (false, "User Id not found", result);
                    }
                    else
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = new DropdownDataResponse()
                                {
                                    Appownerid = Convert.IsDBNull(reader["APPOWNERID"]) ? 0 : Convert.ToInt32(reader["APPOWNERID"]),
                                    UserId = Convert.IsDBNull(reader["USERID"]) ? 0 : Convert.ToInt32(reader["USERID"]),
                                    Username = Convert.IsDBNull(reader["USERNAME"]) ? null : Convert.ToString(reader["USERNAME"]),
                                    Rolename = Convert.IsDBNull(reader["ROLENAME"]) ? null : Convert.ToString(reader["ROLENAME"])

                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return (true, "", result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new DropdownDataResponse());
            }
        }
        #endregion

        #region DropdownRMBACorporasi
        public async Task<(bool status, string error, List<DropdownDataResponse> list, int resultCount)> LoadDataAsyncRMBACorporasi()
        {
            try
            {
                List<DropdownDataResponse> result = new List<DropdownDataResponse>();

                var connectionString = GetConfig.AppSetting["ConnectionStrings:dbOracle"];
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.CommandText = @"
                                            SELECT 
                                                AU.APPOWNERID, 
                                                AU.USERID, 
                                                AU.USERNAME, 
                                                A.NAME AS ROLENAME 
                                            FROM 
                                                AZ_USER AU
                                            LEFT JOIN 
                                                AZ_ROLEASSIGNMENT AR ON AU.APPOWNERID = AR.APPOWNERID AND AU.USERID = AR.MEMBERID
                                            INNER JOIN 
                                                AZ_ROLE A ON AR.APPOWNERID = A.APPOWNERID AND AR.ROLEID = A.ROLEID
                                            WHERE 
                                                AU.APPOWNERID = 955 
                                                AND A.ROLEID = 1016
                                            ORDER BY 
                                                AU.USERID, 
                                                A.ROLEID";

                    OracleDataReader reader = command.ExecuteReader();


                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DropdownDataResponse role = new DropdownDataResponse()
                            {
                                Appownerid = Convert.IsDBNull(reader["APPOWNERID"]) ? 0 : Convert.ToInt32(reader["APPOWNERID"]),
                                UserId = Convert.IsDBNull(reader["USERID"]) ? 0 : Convert.ToInt32(reader["USERID"]),
                                Username = Convert.IsDBNull(reader["USERNAME"]) ? null : Convert.ToString(reader["USERNAME"]),
                                Rolename = Convert.IsDBNull(reader["ROLENAME"]) ? null : Convert.ToString(reader["ROLENAME"])

                            };

                            result.Add(role);
                        }
                    }
                    connection.Close();
                }
                return (true, "", result, result.Count);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new List<DropdownDataResponse>(), 0);
            }
        }
        #endregion

        #region DropdownRMBACorporasiByParams
        public async Task<(bool status, string error, DropdownDataResponse data)> LoadDataByParamAsyncRMBACorporasi(int userId)
        {
            try
            {
                DropdownDataResponse result = null;

                var connectionString = GetConfig.AppSetting["ConnectionStrings:dbOracle"];
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.CommandText = @"
                                            SELECT 
                                                AU.APPOWNERID, 
                                                AU.USERID, 
                                                AU.USERNAME, 
                                                A.NAME AS ROLENAME 
                                            FROM 
                                                AZ_USER AU
                                            LEFT JOIN 
                                                AZ_ROLEASSIGNMENT AR ON AU.APPOWNERID = AR.APPOWNERID AND AU.USERID = AR.MEMBERID
                                            INNER JOIN 
                                                AZ_ROLE A ON AR.APPOWNERID = A.APPOWNERID AND AR.ROLEID = A.ROLEID
                                            WHERE 
                                                AU.APPOWNERID = 955 
                                                AND A.ROLEID = 1016
                                                AND AU.USERID = :UserID";

                    command.Parameters.Add(new OracleParameter(":UserID", OracleDbType.Int32)).Value = userId;

                    OracleDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return (false, "User Id not found", result);
                    }
                    else
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = new DropdownDataResponse()
                                {
                                    Appownerid = Convert.IsDBNull(reader["APPOWNERID"]) ? 0 : Convert.ToInt32(reader["APPOWNERID"]),
                                    UserId = Convert.IsDBNull(reader["USERID"]) ? 0 : Convert.ToInt32(reader["USERID"]),
                                    Username = Convert.IsDBNull(reader["USERNAME"]) ? null : Convert.ToString(reader["USERNAME"]),
                                    Rolename = Convert.IsDBNull(reader["ROLENAME"]) ? null : Convert.ToString(reader["ROLENAME"])

                                };
                            }
                        }
                    }
                    connection.Close();
                }
                return (true, "", result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new DropdownDataResponse());
            }
        }
        #endregion



    }
}
