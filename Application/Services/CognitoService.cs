using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Domain.Gateways;

namespace Application.Services
{
    public class CognitoService : ICognitoGateway
    {
        private readonly string UserPoolId;
        private readonly string UserPoolClientId;

        public CognitoService(IConfiguration configuration)
        {
            UserPoolId = configuration["Aws:PoolId"];
            UserPoolClientId = configuration["Aws:PoolClientId"];
        }

        public async Task<string> CreateUser(Cliente cliente)
        {
            try
            {
                using (var provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast2))
                {
                    var userAttributes = new List<AttributeType>
                    {
                        new AttributeType
                        {
                            Name = "name",
                            Value = cliente.Nome
                        }
                    };

                    var signUpResult = await provider.SignUpAsync(new SignUpRequest
                    {
                        ClientId = UserPoolClientId,
                        Username = cliente.Cpf.ToString(),
                        Password = cliente.Cpf.ToString(),
                        UserAttributes = userAttributes,
                    });

                    await provider.AdminConfirmSignUpAsync(new AdminConfirmSignUpRequest
                    {
                        UserPoolId = UserPoolId,
                        Username = cliente.Cpf.ToString(),
                    });

                    return signUpResult.UserSub;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task DeleteUser(string userId)
        {
            try
            {
                using (var provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast2))
                {
                    await provider.AdminDeleteUserAsync(new AdminDeleteUserRequest
                    {
                        Username = userId,
                        UserPoolId = UserPoolId
                    });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}