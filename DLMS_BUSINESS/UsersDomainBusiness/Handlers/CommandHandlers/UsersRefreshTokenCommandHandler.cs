using DLMS_DAL.UsersDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DLMS_BUSINESS.UsersDomainBusiness.Handlers.CommandHandlers
{
    public class UsersRefreshTokenCommandHandler : IRequestHandler<UsersRefreshTokenCommand, ResponseBase<TokenResponse>>
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IUsersTokenQueryRepository _tokenUtilisateurCommandRepository;
        private readonly IMediator _mediator;

        public UsersRefreshTokenCommandHandler(
            TokenValidationParameters tokenValidationParameters,
            IUsersTokenQueryRepository tokenUtilisateurCommandRepository,
            IMediator mediator
            )
        {
            _tokenValidationParameters = tokenValidationParameters;
            _tokenUtilisateurCommandRepository = tokenUtilisateurCommandRepository;
            _mediator = mediator;

        }

        public async Task<ResponseBase<TokenResponse>> Handle(UsersRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<TokenResponse> response = new ResponseBase<TokenResponse>();

            try
            {
                // get GetPrincipalFromToken from token
                var validatedToken = GetPrincipalFromToken(request.Token);

                if (validatedToken == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid Token";
                    return response;
                }

                var utilisateur = await _tokenUtilisateurCommandRepository.GetRefreshTokenAsync(validatedToken, request.RefreshToken);

                if (utilisateur == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid User Token";
                    return response;
                }

                var authResponse = await _mediator.Send(new UsersAuthenticateCommand(utilisateur));

                if (!authResponse.Success)
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(",", authResponse.Errors);
                    return response;
                }

                TokenResponse refreshTokenModel = new();
                refreshTokenModel.Token = authResponse.Token;
                refreshTokenModel.RefreshToken = authResponse.RefreshToken;
                response.Data = refreshTokenModel;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong!";
                return response;
            }
        }


        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }


        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }

}
