using Api.ViewModels;
using Api.Models.SQLServer;
using Api.Services;
using Api.Tools;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Transactions;

namespace Api.Repositories
{
    public interface IMasterMenuRepo
    {
        Task<(bool status, string error)> CreateAsync(TblMasterNavigation req);

        Task<(bool status, string error)> DeleteAsync(int[] ids);
        Task<(bool status, string error)> UpdateAsync(TblMasterNavigation req);

        Task<(bool status, string error, TblMasterNavigation data)> ViewAsync(int id);

        Task<(bool status, string error, List<NavigationRes_VM> data, int recordTotals)>
           LoadDataAsync(string sortColumn, string sortColumnDir, int pageNumber,
               int pageSize, string Name, string Type, string Role, string Parent);

    }

    public class MasterMenuRepo : IMasterMenuRepo
    {
        public readonly ITokenManager _tokenManager;
        private readonly dbRmTools_Context _context;
        public MasterMenuRepo(ITokenManager tokenManager,dbRmTools_Context context)
        {
            _tokenManager = tokenManager;
            _context = context;
        }
        #region CREATE
        public async Task<(bool status, string error)> CreateAsync(TblMasterNavigation req)
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

                await _context.TblMasterNavigations.AddAsync(req);
                await _context.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString());
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
                            var _data = await _context.TblMasterNavigations.Where(x => x.Id == id).FirstOrDefaultAsync();

                            if (_data == null)
                            {
                                return (false, "Id not found in database");
                            }

                            _data.IsDeleted = true;
                            _data.DeletedById = _tokenManager.GetPrincipal().Result.data.Id;
                            _data.DeletedAt = DateTime.Now;
                            _context.TblMasterNavigations.Update(_data);
                            await _context.SaveChangesAsync();

                            //deleted menu di navigationAssignment
                            var _NavAssignment = await  _context.TblMasterNavigationAssignments.Where(x => x.NavigationId == _data.Id).ToListAsync();

                            foreach (var _item in _NavAssignment)
                            {
                                _item.IsDeleted = true;
                                _item.DeletedById = _tokenManager.GetPrincipal().Result.data.Id;
                                _item.DeletedAt = DateTime.Now;
                                _item.IsActive = false;
                                _context.TblMasterNavigationAssignments.Update(_item);
                                await _context.SaveChangesAsync();
                            }
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

        #region View
        public async Task<(bool status, string error, TblMasterNavigation data)>
            ViewAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    return (false, "Request Id Not found", new TblMasterNavigation());
                }
                var _ = await _context.TblMasterNavigations.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (_ == null)
                {
                    return (false, "Id not found", new TblMasterNavigation());
                }

                return (true, "", _);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new TblMasterNavigation());
            }
        }
        #endregion

        #region UPDATE
        public async Task<(bool status, string error)> UpdateAsync(TblMasterNavigation req)
        {
            try
            {

                if (req == null)
                {
                    return (false, "Request not found");
                }

                var data = await  _context.TblMasterNavigations.Where(x => x.Id == req.Id).FirstOrDefaultAsync();
                if (data == null)
                {
                    return (false, "Id not found in database");
                }

                data.Type = req.Type;
                data.Name = req.Name;
                data.Route = req.Route;
                data.Order = req.Order;
                data.Visible = req.Visible;
                data.ParentNavigationId = req.ParentNavigationId;
                data.UpdatedById = _tokenManager.GetPrincipal().Result.data.Id;
                data.UpdatedAt = DateTime.Now;
                data.IsDeleted = false;

                _context.TblMasterNavigations.Update(data);
                await _context.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString());
            }
        }
        #endregion

        #region LOADDATA
        public async Task<(bool status, string error, List<NavigationRes_VM> data, int recordTotals)>
            LoadDataAsync(string sortColumn, string sortColumnDir, int pageNumber,
                int pageSize, string Name, string Type, string Role, string Parent)
        {
            try
            {
                sortColumn = sortColumn == "" ? null : sortColumn;
                sortColumnDir = sortColumnDir == "" ? null : sortColumnDir;
                Name = Name == "" ? null : Name;
                Type = Type == "" ? null : Type;
                Role = Role == "" ? null : Role;
                Parent = Parent == "" ? null : Parent;

                var list = await StoredProcedureExecutor.ExecuteSPListAsync<NavigationRes_VM>
                    (_context, "[sp_PengaturanMenu_View]", new SqlParameter[] {
                           new SqlParameter("@PageNumber", pageNumber),
                           new SqlParameter("@RowsPage", pageSize),
                           new SqlParameter("@sortColumn", sortColumn),
                           new SqlParameter("@sortColumnDir", sortColumnDir),

                           new SqlParameter("@Name", Name),
                           new SqlParameter("@Type", Type),
                           new SqlParameter("@Role", Role),
                           new SqlParameter("@Parent", Parent)
                });

                var recordsTotal = StoredProcedureExecutor.ExecuteScalarInt
                    (_context, "[sp_PengaturanMenu_Count]", new SqlParameter[] {
                           new SqlParameter("@Name", Name),
                           new SqlParameter("@Type", Type),
                           new SqlParameter("@Role", Role),
                           new SqlParameter("@Parent", Parent)
                });

                return (true, "", list, recordsTotal);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new List<NavigationRes_VM>(), 0);
            }
        }
        #endregion

    }
}
