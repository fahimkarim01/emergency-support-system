using EmergencySupport.Data;
using EmergencySupport.Entities;
using EmergencySupport.Shared;
using Microsoft.EntityFrameworkCore;

namespace EmergencySupport.Repos
{
    public class EmergencyRequestsRepo(EsupportDbContext context, CurrentUserHelper currentUserHelper)
    {
        public Result<List<EmergencyRequests>> GetAll()
        {
            var result = new Result<List<EmergencyRequests>>();

            try
            {
                result.Data = context.EmergencyRequests
                    .Include(e => e.User)
                    .ToList();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<EmergencyRequests> GetById(int id)
        {
            var result = new Result<EmergencyRequests>();

            try
            {
                result.Data = context.EmergencyRequests
                    .Include(e => e.User)
                    .FirstOrDefault(e => e.RequestId == id);

                if (result.Data == null)
                {
                    result.HasError = true;
                    result.Message = "Invalid Request ID";
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<EmergencyRequests> Save(EmergencyRequests model)
        {
            var result = new Result<EmergencyRequests>();

            try
            {
                var objToSave = context.EmergencyRequests
                    .Find(model.RequestId);

                if (objToSave == null)
                {
                    objToSave = new EmergencyRequests();
                    context.EmergencyRequests.Add(objToSave);

                    objToSave.UserId = currentUserHelper.UserId;
                    objToSave.CreatedAt = DateTime.Now;

                    objToSave.Status = "Pending";
                    objToSave.PriorityLevel = "Pending";
                }
                else
                {
                    if (objToSave.UserId != currentUserHelper.UserId)
                    {
                        result.HasError = true;
                        result.Message = "You can only edit your own request.";
                        return result;
                    }
                }

                objToSave.UserId = model.UserId;
                objToSave.EmergencyType = model.EmergencyType;
                objToSave.Description = model.Description;
                objToSave.Latitude = model.Latitude;
                objToSave.Longitude = model.Longitude;
                if (objToSave.RequestId == 0)
                {
                    objToSave.PriorityLevel = "Pending";
                    objToSave.Status = "Pending";
                }
                else
                {
                    objToSave.PriorityLevel = model.PriorityLevel;
                }

                objToSave.UpdatedAt = DateTime.Now;

                context.SaveChanges();

                result.Data = objToSave;
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        public Result<bool> UpdateStatus(int id, string status)
        {
            var result = new Result<bool>();

            try
            {
                var request = context.EmergencyRequests
                    .FirstOrDefault(e => e.RequestId == id);

                if (request == null)
                {
                    result.HasError = true;
                    result.Message = "Emergency request not found";

                    return result;
                }

                request.Status = status;

                request.UpdatedAt = DateTime.Now;

                context.SaveChanges();

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message =
                    ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        public Result<bool> SetPriority(int id, string priority)
        {
            var result = new Result<bool>();

            try
            {
                var request = context.EmergencyRequests
                    .FirstOrDefault(e => e.RequestId == id);

                if (request == null)
                {
                    result.HasError = true;
                    result.Message = "Emergency request not found";

                    return result;
                }

                request.PriorityLevel = priority;

                request.UpdatedAt = DateTime.Now;

                context.SaveChanges();

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message =
                    ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        public Result<bool> Delete(int id)
        {
            var result = new Result<bool>();

            try
            {
                var data = context.EmergencyRequests.Find(id);

                if (data == null)
                {
                    result.HasError = true;
                    result.Message = "Not found";
                    return result;
                }

                if (data.UserId != currentUserHelper.UserId)
                {
                    result.HasError = true;
                    result.Message = "You can only delete your own request.";
                    return result;
                }

                context.EmergencyRequests.Remove(data);
                context.SaveChanges();

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