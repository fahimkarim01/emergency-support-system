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
    public class UsersRepo(EsupportDbContext context)
    {
        public Result<List<Users>> GetAll()
        {
            var result = new Result<List<Users>>();

            try
            {
                result.Data = context.Users.ToList();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Users> GetById(int id)
        {
            var result = new Result<Users>();

            try
            {
                result.Data = context.Users.Find(id);

                if (result.Data == null)
                {
                    result.HasError = true;
                    result.Message = "Invalid User ID";
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Users> Authenticate(
            string email,
            string password)
        {
            var result = new Result<Users>();

            try
            {
                result.Data = context.Users
                    .FirstOrDefault(e =>
                        e.Email == email &&
                        e.Password == password);

                if (result.Data == null)
                {
                    result.HasError = true;
                    result.Message =
                        "Invalid Email or Password";
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Users> Save(Users model)
        {
            var result = new Result<Users>();

            try
            {
                if (context.Users.Any(e =>
                    e.Email.ToLower() ==
                    model.Email.ToLower()
                ))
                {
                    result.HasError = true;
                    result.Message =
                        "Email already exists.";

                    return result;
                }

                var objToSave = context.Users
                    .Find(model.UserId);

                if (objToSave == null)
                {
                    objToSave = new Users();

                    context.Users.Add(objToSave);

                    objToSave.CreatedAt =
                        DateTime.Now;
                }

                objToSave.Name = model.Name;
                objToSave.Email = model.Email;
                objToSave.Phone = model.Phone;
                objToSave.Password = model.Password;
                objToSave.Address = model.Address;
                objToSave.Role = model.Role;

                context.SaveChanges();

                result.Data = objToSave;
            }
            catch (Exception ex)
            {
                result.HasError = true;

                result.Message =
                    ex.InnerException?.Message ??
                    ex.Message;
            }

            return result;
        }

        public Result<bool> Delete(int id)
        {
            var result = new Result<bool>();

            try
            {
                var data = context.Users.Find(id);

                if (data != null)
                {
                    context.Users.Remove(data);

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
