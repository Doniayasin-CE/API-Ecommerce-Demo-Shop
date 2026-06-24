using Azure.Core;
using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace DemoShop.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserManager<ApplicationUser> userManager, 
            IEmailSender emailSender, IConfiguration configuration, IHttpContextAccessor HttpContextAccessor)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _httpContextAccessor = HttpContextAccessor;
        }

        public async Task<RegisterResponse> Register(DAL.DTO.Request.RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user,request.Password);
            if (!result.Succeeded)
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "Error",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };

            await _userManager.AddToRoleAsync(user,"User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);

            var scheme = _httpContextAccessor.HttpContext?.Request.Scheme;
            var host = _httpContextAccessor.HttpContext?.Request.Host;
            var emailURL = $"{scheme}://{host}/api/Account/confirmEmail?token={token}&userId={user.Id}";

            var htmlMessage = $"<h1> Welcome {request.UserName} </h1>" +
                $"" + $"<a href='{emailURL}'> Click here to confirm your email </a>";

            await _emailSender.SendEmailAsync(user.Email!, "Verifying the Email",htmlMessage);

            return new RegisterResponse() { Success = true, Message = "success" };
        }

        public async Task<LoginResponse> Login(DAL.DTO.Request.LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
                return new LoginResponse() { Success = false, Message= "Invalid Email" };

            if(!await _userManager.IsEmailConfirmedAsync(user))
                return new LoginResponse() { Success = false, Message = "your email not confirmed"};
            
            var result = await _userManager.CheckPasswordAsync(user,request.Password);
            if (!result)
                return new LoginResponse() { Success = false, Message = "Invalid password" };

            //GenerateRefreshToken
            var refreshToken = await GenerateRefreshToken(user);
            SetRefreshTokenInCookie(refreshToken);

            return new LoginResponse() 
            { 
                Success = true, 
                Message= "success Login",
                AccessToken = await GenerateAccessToken(user) //GenerateAccessToken
            };
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>() { 
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!)
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(15);
            await _userManager.UpdateAsync(user);
            return refreshToken;
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies
                .Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, //true for production and false for development
                    SameSite = SameSiteMode.None, //Strict for production and None for development
                    Expires = DateTime.UtcNow.AddDays(15)
                });
        }

        public async Task<LoginResponse> RefreshTokenReissue()
        {
            var refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"];
            if( refreshToken == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "The refresh token does not exist"
                };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if(user!.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "The refresh token expired"
                };
            }

            var newRefreshToken = await GenerateRefreshToken(user);
            SetRefreshTokenInCookie(newRefreshToken);
            return new LoginResponse()
            {
                Success = true,
                Message = "success Login",
                AccessToken = await GenerateAccessToken(user) //GenerateAccessToken
            };
        }
        public async Task<bool> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null) return false;

            var result = await _userManager.ConfirmEmailAsync(user,token);
            if(!result.Succeeded) return false;

            return true;
        }

        public async Task<ForgetPasswordResponse> RequestPasswordReset(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new ForgetPasswordResponse()
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }

            var code = new Random().Next(1000, 9999).ToString();
            user.CodeResetPassword = code;
            user.CodeResetPasswordExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(user.Email!, "Reset your Password", $"<h2> Code Is {code} </h2>");
            return new ForgetPasswordResponse()
            {
                Success = true,
                Message = "The Code sent to your Email"
            };
        }

        public async Task<ResetPasswordResponse> ResetPassword(DAL.DTO.Request.ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }
            else if(user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Invalid Code"
                };
            }
            else if(user.CodeResetPasswordExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Code was Expired"
                };
            }
            var isSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if (isSamePassword)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "New password must be different from the old password"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result =  await _userManager.ResetPasswordAsync(user,token,request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Password reset has failed"
                };
            }
            
            string message = $"<h3> Your password has been changed successfully</h3>";
            await _emailSender.SendEmailAsync(user.Email!, "Change Password Request", message);
            
            return new ResetPasswordResponse()
            {
                Success = true,
                Message = "New password has been added successfully"
            };
        }
    }
}
