using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmergencySupport.Shared
{
    public class CurrentUserHelper(IHttpContextAccessor accessor)
    {
        public bool IsAuthenticated
        {
            get
            {
                try
                {
                    return (bool)accessor.HttpContext?.User?.Identity?.IsAuthenticated;
                }
                catch
                {
                    return false;
                }
            }
        }

        public int UserId
        {
            get
            {
                try
                {
                    var id = accessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                    return id != null ? int.Parse(id) : -1;
                }
                catch
                {
                    return -1;
                }
            }
        }

    }
}
