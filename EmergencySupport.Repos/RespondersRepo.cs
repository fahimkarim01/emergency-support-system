using EmergencySupport.Data;
using EmergencySupport.Entities;
using EmergencySupport.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmergencySupport.Repos
{
    public class RespondersRepo(EsupportDbContext context)
    {
        public Result<List<Responders>> GetAll()
        {
            var result = new Result<List<Responders>>();

            try
            {
                result.Data = context.Responders
                    .Include(x => x.User)
                    .Where(x => x.User.Role == "responder")
                    .ToList();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Responders> GetById(int id)
        {
            var result = new Result<Responders>();

            try
            {
                result.Data = context.Responders
                    .Include(e => e.User)
                    .FirstOrDefault(e => e.ResponderId == id);

                if (result.Data == null)
                {
                    result.HasError = true;
                    result.Message = "Invalid Responder ID";
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Responders> GetByUserId(int userId)
        {
            var result = new Result<Responders>();

            try
            {
                result.Data = context.Responders
                    .Include(e => e.User)
                    .FirstOrDefault(e => e.UserId == userId);

                if (result.Data == null)
                {
                    result.HasError = true;
                    result.Message = "Responder profile not found";
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Responders> Save(Responders model)
        {
            var result = new Result<Responders>();

            try
            {
                var userExists = context.Users.Any(e =>
                    e.UserId == model.UserId &&
                    e.Role.ToLower() == "responder");

                if (!userExists)
                {
                    result.HasError = true;
                    result.Message = "Invalid responder user.";
                    return result;
                }

                Responders objToSave;

                if (model.ResponderId == 0)
                {
                    objToSave = new Responders();
                    context.Responders.Add(objToSave);
                }
                else
                {
                    objToSave = context.Responders
                        .FirstOrDefault(e => e.ResponderId == model.ResponderId);

                    if (objToSave == null)
                    {
                        result.HasError = true;
                        result.Message = "Responder not found";
                        return result;
                    }
                }

                objToSave.UserId = model.UserId;
                objToSave.ServiceType = model.ServiceType;
                objToSave.AvailabilityStatus = model.AvailabilityStatus;
                objToSave.CurrentLatitude = model.CurrentLatitude;
                objToSave.CurrentLongitude = model.CurrentLongitude;

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

        public Result<bool> Delete(int id)
        {
            var result = new Result<bool>();

            try
            {
                var data = context.Responders
                    .FirstOrDefault(e => e.ResponderId == id);

                if (data != null)
                {
                    context.Responders.Remove(data);
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