using EmergencySupport.Data;
using EmergencySupport.Entities;
using EmergencySupport.Shared;
using Microsoft.EntityFrameworkCore;

namespace EmergencySupport.Repos
{
    public class AssignmentsRepo(EsupportDbContext context, CurrentUserHelper currentUserHelper)
    {
        public Result<List<Assignments>> GetAll()
        {
            var result = new Result<List<Assignments>>();

            try
            {
                result.Data = context.Assignments
                    .Include(x => x.Request)
                    .Include(x => x.Responder)
                    .ThenInclude(r => r.User)
                    .ToList();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Assignments> GetById(int id)
        {
            var result = new Result<Assignments>();

            try
            {
                result.Data = context.Assignments
                    .Include(x => x.Request)
                    .Include(x => x.Responder)
                    .FirstOrDefault(x => x.AssignmentId == id);

                if (result.Data == null)
                {
                    result.HasError = true;
                    result.Message = "Assignment not found";
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Assignments> Save(Assignments model)
        {
            var result = new Result<Assignments>();

            try
            {
                var obj = context.Assignments.Find(model.AssignmentId);

                if (obj == null)
                {
                    obj = new Assignments();
                    context.Assignments.Add(obj);
                }

                obj.RequestId = model.RequestId;
                obj.ResponderId = model.ResponderId;
                obj.Status = model.Status;

                obj.AssignedBy = currentUserHelper.UserId;
                obj.AssignedAt = DateTime.Now;

                obj.ArrivalTime = model.ArrivalTime;
                obj.CompletionTime = model.CompletionTime;

                var request = context.EmergencyRequests.Find(obj.RequestId);
                if (request != null)
                {
                    request.Status = obj.Status;
                    request.UpdatedAt = DateTime.Now;
                }

                context.SaveChanges();
                result.Data = obj;

                var log = new ReportsLogs();
                log.UserId = currentUserHelper.UserId;
                log.Action = model.AssignmentId == 0 ? "Assignment Created" : "Assignment Updated";
                log.Details = "Assignment #" + obj.AssignmentId + " - Request #" + obj.RequestId + " - Status: " + obj.Status;
                log.Timestamp = DateTime.Now;
                context.ReportsLogs.Add(log);
                context.SaveChanges();
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
                var data = context.Assignments.Find(id);

                if (data != null)
                {
                    context.Assignments.Remove(data);
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