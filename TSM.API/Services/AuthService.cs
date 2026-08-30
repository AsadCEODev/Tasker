using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMS.API.Data;
using TMS.Shared.Model.Auth;
using TMS.Shared.Model.Setup;
using TSM.API.Data;

namespace TSM.API.Services
{
    // 1. Yahan BaseClassService se inherit karein
    public class AuthService : BaseClassService, IAuthService
    {
        public AuthService(
            ApplicationDbContext dbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(dbContext, configuration, httpContextAccessor)
        {
        }

        public async Task<string?> LoginAsync(LoginDto model)
        {
            try
            {
                var user = await dbContext.SetupUsers.Include(x => x.DepartmentObj)
                .Include(y => y.DesignationObj)
                .FirstOrDefaultAsync(x => x.UserName == model.UserName);

                if (user == null)
                    return null;

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.HashPassword);

                if (!isPasswordValid)
                    return null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                        new Claim(ClaimTypes.Name, user.FullName ?? string.Empty)
                    }),

                    Issuer = configuration["Jwt:Issuer"],
                    Audience = configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SetupUser?> GetCurrentUserInfo()
        {
            try
            {
                if (LoginUserId <= 0)
                    return null;

                // 3. Ab yahan BaseClassService wala 'LoginUserId' seedha use ho ga
                var found = await dbContext.SetupUsers
                    .Include(x => x.DepartmentObj)
                    .Include(y => y.DesignationObj)
                    .FirstOrDefaultAsync(x => x.Id == LoginUserId);

                return found;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

public interface IAuthService
{
    Task<string?> LoginAsync(LoginDto model);
    Task<SetupUser?> GetCurrentUserInfo(); // Interface mein bhi add kar dein
}