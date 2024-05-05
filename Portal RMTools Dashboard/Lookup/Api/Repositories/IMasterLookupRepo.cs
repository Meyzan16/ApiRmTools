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
    public interface IMasterLookupRepo
    {
        Task<(bool status, string error)> CreateAsync(RequestCreated req);

        Task<(bool status, string error)> DeleteAsync(int[] ids);
        Task<(bool status, string error)> UpdateAsync(TblMasterLookup req);

        Task<(bool status, string error, TblMasterLookup data)> ViewAsync(int id);

        Task<(bool status, string error, List<MasterLookupRes_VM> data, int recordTotals)>
                LoadDataAsync(string sortColumn, string sortColumnDir, int pageNumber,
                    int pageSize, string Type, string Name);

    }

    public class MasterLookupRepo : IMasterLookupRepo
    {
        public readonly ITokenManager _tokenManager;
        private readonly dbRmTools_Context _context;
        public MasterLookupRepo(ITokenManager tokenManager,dbRmTools_Context context)
        {
            _tokenManager = tokenManager;
            _context = context;
        }
        #region CREATE
        public async Task<(bool status, string error)> CreateAsync(RequestCreated req)
        {
            try
            {
                if (req == null)
                {
                    return (false, "Request not found");
                }

                var OrderBy = await _context.TblMasterLookups.Where(x => x.Type == req.Type).OrderByDescending(x => x.OrderBy).Select(x => x.OrderBy).FirstOrDefaultAsync();
                var Value = await _context.TblMasterLookups.Where(x => x.Type == req.Type).OrderByDescending(x => x.Value).Select(x => x.Value).FirstOrDefaultAsync();

                var TblLookup = new TblMasterLookup();

                TblLookup.Type = req.Type;
                TblLookup.Name = req.Name;
                TblLookup.Value = Value != null ? Value + 1 : 1;
                TblLookup.OrderBy = OrderBy != null ?  OrderBy + 1 : 1;
                TblLookup.CreatedById = _tokenManager.GetPrincipal().Result.data.Id;
                TblLookup.CreatedAt = DateTime.Now;
                TblLookup.IsDeleted = false;

                await _context.TblMasterLookups.AddAsync(TblLookup);
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
        public async Task<(bool status, string error, TblMasterLookup data)>
            ViewAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    return (false, "Request Id Not found", new TblMasterLookup());
                }
                var _ = await _context.TblMasterLookups.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (_ == null)
                {
                    return (false, "Data not found", new TblMasterLookup());
                }

                return (true, "", _);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new TblMasterLookup());
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
                            var _data = await _context.TblMasterLookups.Where(x => x.Id == id).FirstOrDefaultAsync();

                            if (_data == null)
                            {
                                return (false, "Id not found in database");
                            }

                            _data.IsDeleted = true;
                            _data.DeletedById = _tokenManager.GetPrincipal().Result.data.Id;
                            _data.DeletedAt = DateTime.Now;
                            _data.IsActive = false;


                            _context.TblMasterLookups.Update(_data);
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
        public async Task<(bool status, string error)> UpdateAsync(TblMasterLookup req)
        {
            try
            {

                if (req == null)
                {
                    return (false, "Request not found");
                }

                var data = await _context.TblMasterLookups.Where(x => x.Id == req.Id).FirstOrDefaultAsync();
                if (data == null)
                {
                    return (false, "Data not found in database");
                }

                data.Type = req.Type;
                data.Name = req.Name;
                data.Value = req.Value;
                data.OrderBy = req.OrderBy;
                data.UpdatedById = _tokenManager.GetPrincipal().Result.data.Id;
                data.UpdatedAt = DateTime.Now;
                data.IsDeleted = false;

                _context.TblMasterLookups.Update(data);
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
        public async Task<(bool status, string error, List<MasterLookupRes_VM> data, int recordTotals)>
            LoadDataAsync(string sortColumn, string sortColumnDir, int pageNumber,
                int pageSize, string Type, string Name)
        {
            try
            {
                sortColumn = sortColumn == "" ? null : sortColumn;
                sortColumnDir = sortColumnDir == "" ? null : sortColumnDir;
                Type = Type == "" ? null : Type;
                Name = Name == "" ? null : Name;
                var list = await StoredProcedureExecutor.ExecuteSPListAsync<MasterLookupRes_VM>
                    (_context, "[sp_PengaturanLookup_View]", new SqlParameter[] {
                           new SqlParameter("@PageNumber", pageNumber),
                           new SqlParameter("@RowsPage", pageSize),
                           new SqlParameter("@sortColumn", sortColumn),
                           new SqlParameter("@sortColumnDir", sortColumnDir),

                           new SqlParameter("@Type", Type),
                           new SqlParameter("@Name", Name)
                });

                var recordsTotal = StoredProcedureExecutor.ExecuteScalarInt
                    (_context, "[sp_PengaturanLookup_Count]", new SqlParameter[] {
                           new SqlParameter("@Type", Type),
                           new SqlParameter("@Name", Name)
                });

                return (true, "", list, recordsTotal);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new List<MasterLookupRes_VM>(), 0);
            }
        }
        #endregion
    }
}
