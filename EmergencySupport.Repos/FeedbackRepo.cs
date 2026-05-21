using EmergencySupport.Data;
using EmergencySupport.Entities;
using EmergencySupport.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencySupport.Repos
{
    public class FeedbackRepo(EsupportDbContext context, CurrentUserHelper currentUserHelper)
    {
        public Result<List<Feedback>> GetAll()
        {
            var result = new Result<List<Feedback>>();

            try
            {
                result.Data = context.Feedback.Include(e => e.Request).Include(e => e.User).ToList();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Feedback> GetById(int id)
        {
            var result = new Result<Feedback>();

            try
            {
                result.Data = context.Feedback.Find(id);

                if (result.Data == null)
                {
                    result.HasError = true;
                    result.Message = "Invalid Feedback";
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Feedback> Save(Feedback model)
        {
            var result = new Result<Feedback>();

            try
            {
                var request = context.EmergencyRequests.FirstOrDefault(e => e.RequestId == model.RequestId);

                if (request == null)
                {
                    result.HasError = true;
                    result.Message = "Emergency request not found.";

                    return result;
                }

                if (request.Status != "Completed")
                {
                    result.HasError = true;
                    result.Message = "Feedback can only be submitted for completed requests.";

                    return result;
                }

                if (request.UserId != currentUserHelper.UserId)
                {
                    result.HasError = true;
                    result.Message = "You can only give feedback to your own requests.";

                    return result;
                }

                if (context.Feedback.Any(e => (e.RequestId == model.RequestId) && (e.UserId == model.UserId) && (e.FeedbackId != model.FeedbackId)))
                {
                    result.HasError = true;
                    result.Message = "Feedback already exist.";

                    return result;
                }

                var objToSave = context.Feedback.Find(model.FeedbackId);
                if (objToSave == null)
                {
                    objToSave = new Feedback();
                    context.Feedback.Add(objToSave);
                }

                objToSave.RequestId = model.RequestId;
                objToSave.UserId = currentUserHelper.UserId; // Current User
                objToSave.Rating = model.Rating;
                objToSave.Comments = model.Comments;
                objToSave.CreatedAt = DateTime.Now;

                context.SaveChanges();

                result.Data = objToSave;
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
                var data = context.Feedback.Find(id);

                if (data == null)
                {
                    result.HasError = true;
                    result.Message = "Not found";
                    return result;
                }

                if (data.UserId != currentUserHelper.UserId)
                {
                    result.HasError = true;
                    result.Message = "You can only delete your own feedback.";
                    return result;
                }

                context.Feedback.Remove(data);
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
