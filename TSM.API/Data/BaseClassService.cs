using TMS.API.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace TSM.API.Data
{
 

    public class BaseClassService
    {
        protected readonly ApplicationDbContext dbContext;
        protected readonly IConfiguration configuration;
        protected readonly IHttpContextAccessor httpContextAccessor;

        public BaseClassService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        // Current Logged-in User ID (Claim se automatic milegi)
        protected long LoginUserId
        {
            get
            {
                var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (long.TryParse(userIdClaim, out long userId))
                {
                    return userId;
                }
                return 0; 
            }
        }

        // Current Logged-in User Name
        protected string LoginUserName
        {
            get
            {
                return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Admin";
            }
        }
        // 🔥 Yeh rahi nayi property DesignationTitle ke liye
        protected string LoginUserDesignation
        {
            get
            {
                return httpContextAccessor.HttpContext?.User?.FindFirst("DesignationTitle")?.Value ?? string.Empty;
            }
        }
    }
}
