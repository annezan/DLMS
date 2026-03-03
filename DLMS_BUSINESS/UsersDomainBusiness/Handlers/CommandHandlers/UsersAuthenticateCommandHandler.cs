using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersAuthenticateCommandHandler : IRequestHandler<UsersAuthenticateCommand, AuthentificationResponse>
    {
        private readonly ServiceConfiguration _appSettings;
        private readonly IUsersTokenCommandRepository _repository;

        public UsersAuthenticateCommandHandler(IUsersTokenCommandRepository repository,
            IOptions<ServiceConfiguration> settings)
        {
            _repository = repository;
            _appSettings = settings.Value;
        }

        public async Task<AuthentificationResponse> Handle(UsersAuthenticateCommand request, CancellationToken cancellationToken)
        {
            // authentication successful so generate jwt token
            AuthentificationResponse authenticationResult = new AuthentificationResponse();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);

                ClaimsIdentity subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimsKey.UtilisateurId, request.Users.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, request.Users.Id.ToString()), // ✅ Ajout pour compatibilité
                    new Claim("UserId", request.Users.Id.ToString()), // ✅ Ajout pour compatibilité
                    new Claim(ClaimsKey.Nom, request.Users.Nom),
                    new Claim(ClaimsKey.Prenoms,request.Users.Prenoms),
                    new Claim(ClaimsKey.Email, request.Users.Email==null?"":request.Users.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    });

                if (request.Users.Role != null)
                {
                    subject.AddClaim(new Claim(ClaimTypes.Role, request.Users.Role?.Code?.ToString()));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = DateTime.UtcNow.Add(_appSettings.JwtSettings.TokenLifetime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                authenticationResult.Token = tokenHandler.WriteToken(token);

                var refreshToken = new TokenUser
                {
                    Token = Guid.NewGuid().ToString(),
                    JwtId = token.Id,
                    UsersId = request.Users.Id,
                    CreatedAt = DateTime.UtcNow,
                    ExpiryAt = DateTime.UtcNow.AddMonths(6)
                };

                await _repository.AddAsync(refreshToken);

                authenticationResult.RefreshToken = refreshToken.Token;
                authenticationResult.Success = true;
                return authenticationResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }

}
