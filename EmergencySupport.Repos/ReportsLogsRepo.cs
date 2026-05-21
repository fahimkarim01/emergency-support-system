using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmergencySupport.Data;
using EmergencySupport.Entities;
using EmergencySupport.Shared;

namespace EmergencySupport.Repos
{
    public class ReportsLogsRepo(EsupportDbContext context)
    {
        public Result<List<ReportsLogs>> GetAll()
        {
            var result = new Result<List<ReportsLogs>>();

            try
            {
                result.Data = context.ReportsLogs.ToList();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<ReportsLogs> Save(ReportsLogs model)
        {
            var result = new Result<ReportsLogs>();

            try
            {
                var obj = context.ReportsLogs.Find(model.LogId);

                if (obj == null)
                {
                    obj = new ReportsLogs();

                    context.ReportsLogs.Add(obj);
                }

                obj.UserId = model.UserId;
                obj.Action = model.Action;
                obj.Timestamp = DateTime.Now;
                obj.Details = model.Details;

                context.SaveChanges();

                result.Data = obj;
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<bool> Delete(int id)
        {
            var result = new Result<bool>();

            try
            {
                var data = context.ReportsLogs.Find(id);

                if (data != null)
                {
                    context.ReportsLogs.Remove(data);

                    context.SaveChanges();
                }

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
