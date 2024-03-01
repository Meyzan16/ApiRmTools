using Api.Models.SQLServer;
using Api.Services;
using Api.Tools;
using Api.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Api.Repositories
{
    public interface INavigationAssignmentRepo
    {
        Task<(bool status, string error)> CreateAsync(TblMasterNavigationAssignment req);

        Task<(bool status, string error)> DeleteAsync(int[] ids);
        Task<(bool status, string error)> UpdateAsync(TblMasterNavigationAssignment req);

        Task<(bool status, string error, DataTableRes_VM data)> ViewAsync(int id);

        Task<(bool status, string message, List<AccessNavigateResponse> data)> AccessNavigateAsync(int userId);

        Task<(bool status, string error, List<DataTableRes_VM> data, int recordTotals)>
         LoadDataAsync(string sortColumn, string sortColumnDir, int pageNumber,
             int pageSize, string NamaMenu, string NamaUser);
    }

    public class NavigationAssignmentRepo : INavigationAssignmentRepo
    {
        public readonly ITokenManager _tokenManager;
        private readonly dbRmTools_Context _context;
        public NavigationAssignmentRepo(ITokenManager tokenManager, dbRmTools_Context context)
        {
            _tokenManager = tokenManager;
            _context = context;
        }

        #region CREATE
        public async Task<(bool status, string error)> CreateAsync(TblMasterNavigationAssignment req)
        {
            try
            {
                if (req == null)
                {
                    return (false, "Request not found");
                }

                req.IsDeleted = false;
                req.CreatedById = _tokenManager.GetPrincipal().Result.data.Id;
                req.CreatedAt = DateTime.Now;
                req.DeletedAt = null;
                req.UpdatedAt = null;

                await _context.TblMasterNavigationAssignments.AddAsync(req);
                await _context.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString());
            }
        }
        #endregion

        #region View
        public async Task<(bool status, string error, DataTableRes_VM data)>
            ViewAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    return (false, "Request Id Not found", new DataTableRes_VM());
                }

                var _ = await StoredProcedureExecutor.ExecuteSPSingleAsync<DataTableRes_VM>(_context, "[sp_NavigationAssignment_detail]", new SqlParameter[] {
                               new SqlParameter("@Id", id),
                });

                if (_ == null)
                {
                    return (false, "Id not found", new DataTableRes_VM());
                }

                return (true, "", _);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new DataTableRes_VM());
            }
        }
        #endregion

        #region DELETE
        public async Task<(bool status, string error)> DeleteAsync(int[] ids)
        {
            try
            {
                if (ids.Count() > 0)
                {
                    using (TransactionScope Trx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        foreach (var id in ids)
                        {
                            var _data = await _context.TblMasterNavigationAssignments.Where(x => x.Id == id).FirstOrDefaultAsync();

                            if (_data == null)
                            {
                                return (false, "Id not found in database");
                            }

                            _data.IsDeleted = true;
                            _data.DeletedById = _tokenManager.GetPrincipal().Result.data.Id;
                            _data.DeletedAt = DateTime.Now;
                            _data.IsActive = false;


                            _context.TblMasterNavigationAssignments.Update(_data);
                            await _context.SaveChangesAsync();
                        }
                        Trx.Complete();
                    }
                }

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString());
            }
        }
        #endregion

        #region UPDATE
        public async Task<(bool status, string error)> UpdateAsync(TblMasterNavigationAssignment req)
        {
            try
            {

                if (req == null)
                {
                    return (false, "Request not found");
                }

                var data = await _context.TblMasterNavigationAssignments.Where(x => x.Id == req.Id).FirstOrDefaultAsync();

                if (data == null)
                {
                    return (false, "Id not found in database");
                }

                data.NavigationId = req.NavigationId; //Id Master Menu
                data.UserId = req.UserId; //Id User
                data.UpdatedById = _tokenManager.GetPrincipal().Result.data.Id;
                data.UpdatedAt = DateTime.Now;
                data.IsActive = req.IsActive;
                data.IsDeleted = false;

                _context.TblMasterNavigationAssignments.Update(data);
                await _context.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString());
            }
        }
        #endregion

        #region ACCESSNAVIGATE
        public async Task<(bool status, string message, List<AccessNavigateResponse> data)> AccessNavigateAsync(int userId)
        {
            try
            {
                var list = await StoredProcedureExecutor.ExecuteSPListAsync<AccessNavigateResponse>
                    (_context, "[sp_PengaturanAccessMenu]", new SqlParameter[] {
                           new SqlParameter("@userId", userId)
                });
                if(list == null)
                {
                return (false, "Not found data", list);

                }
                else
                {
                return (true, "", list);
                }

            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new List<AccessNavigateResponse>());
            }
        }
        #endregion

        #region LOADDATA
        public async Task<(bool status, string error, List<DataTableRes_VM> data, int recordTotals)> LoadDataAsync(string sortColumn, string sortColumnDir, int pageNumber,
                int pageSize, string NamaMenu, string NamaUser)
        {
            try
            {
                sortColumn = sortColumn == "" ? null : sortColumn;
                sortColumnDir = sortColumnDir == "" ? null : sortColumnDir;
                NamaMenu = NamaMenu == "" ? null : NamaMenu;
                NamaUser = NamaUser == "" ? null : NamaUser;

                var list = await StoredProcedureExecutor.ExecuteSPListAsync<DataTableRes_VM>
                    (_context, "[sp_NavigationAssignment_view]", new SqlParameter[] {
                           new SqlParameter("@PageNumber", pageNumber),
                           new SqlParameter("@RowsPage", pageSize),
                           new SqlParameter("@sortColumn", sortColumn),
                           new SqlParameter("@sortColumnDir", sortColumnDir),

                           new SqlParameter("@NamaMenu", NamaMenu),
                           new SqlParameter("@NamaUser", NamaUser)
                });

                var recordsTotal = StoredProcedureExecutor.ExecuteScalarInt
                    (_context, "[sp_NavigationAssignment_count]", new SqlParameter[] {
                           new SqlParameter("@NamaMenu", NamaMenu),
                           new SqlParameter("@NamaUser", NamaUser)
                });

                return (true, "", list, recordsTotal);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new List<DataTableRes_VM>(), 0);
            }
        }
        #endregion
    }



}
