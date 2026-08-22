
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMS.API.Data;
using TMS.Shared.Model.Auth;

namespace TSM.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;

        public AuthService(
            ApplicationDbContext _dbContext,
            IConfiguration _configuration)
        {
            dbContext = _dbContext;
            configuration = _configuration;
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
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Name, user.FullName ?? string.Empty)
                }),

                    // Expires = DateTime.UtcNow.AddDays( Convert.ToDouble(configuration["Jwt:ExpireMinutes"])),
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

    }

    
}
public interface IAuthService
{
    Task<string?> LoginAsync(LoginDto model);

 
}