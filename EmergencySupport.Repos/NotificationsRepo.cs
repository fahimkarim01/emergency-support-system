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
    public class NotificationsRepo(EsupportDbContext context)
    {
        public Result<List<Notifications>> GetAll()
        {
            var result = new Result<List<Notifications>>();

            try
            {
                result.Data = context.Notifications.ToList();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Notifications> GetById(int id)
        {
            var result = new Result<Notifications>();

            try
            {
                result.Data = context.Notifications.Find(id);

                if (result.Data == null)
                {
                    result.HasError = true;
                    result.Message = "Invalid Notification";
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Notifications> Save(Notifications model)
        {
            var result = new Result<Notifications>();

            try
            {
                // 1. Basic validation: check if the input model is null
                if (model == null)
                {
                    result.HasError = true;
                    result.Message = "Notification details cannot be empty.";
                    return result;
                }

                // 2. Business validation: check if the associated User exists in the system
                var userExists = context.Users.Any(u => u.UserId == model.UserId);
                if (!userExists)
                {
                    result.HasError = true;
                    result.Message = $"User with ID {model.UserId} does not exist in the system.";
                    return result;
                }

                // 3. Find if this notification already exists in the database
                var obj = context.Notifications.Find(model.NotificationId);

                // If it doesn't exist, we are creating a new one
                if (obj == null)
                {
                    obj = new Notifications();
                    context.Notifications.Add(obj);
                }

                // 4. Update the properties of the notification
                obj.UserId = model.UserId;
                obj.Message = model.Message;
                obj.Type = model.Type;
                obj.IsRead = model.IsRead;
                obj.CreatedAt = DateTime.Now;

                // 5. Save all changes to the database
                context.SaveChanges();

                // 6. Return the saved notification
                result.Data = obj;
            }
            catch (Exception ex)
            {
                // If any database or unexpected exception happens, catch it here
                result.HasError = true;

                // Get the inner exception message if available, otherwise get the main message
                result.Message = "An error occurred while saving the notification: " +
                                 (ex.InnerException?.Message ?? ex.Message);
            }

            return result;
        }

        public Result<bool> Delete(int id)
        {
            var result = new Result<bool>();

            try
            {
                var data = context.Notifications.Find(id);

                if (data != null)
                {
                    context.Notifications.Remove(data);

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
