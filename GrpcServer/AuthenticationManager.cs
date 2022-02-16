using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrpcServer
{
    public static class AuthenticationManager
    {
        public const string JWT_TOKEN_KEY = "grpc@2022";
        private const int JWT_TOKEN_VALIDITY = 30;
        public static AuthResponse Authenticate(AuthRequest request)
        {
            if(request.Email.Equals("admin@grpc.com") && request.Password.Equals("admin123"))
            {
                var token = CreateAndReturnToken(request, out int expiresIn);
                return new AuthResponse()
                {
                    AccessToken = token,
                    ExpiresIn = expiresIn
                };
            }
            return null;
        }


        private static string CreateAndReturnToken(AuthRequest request, out int expiryDate)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(JWT_TOKEN_KEY);
            var tokenExpiryDate = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY);
            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>
                {
                    new Claim("email", request.Email),
                }),
                Expires = tokenExpiryDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            expiryDate = (int)tokenExpiryDate.Subtract(DateTime.Now).TotalSeconds;
            return token;

        }
    }
}
