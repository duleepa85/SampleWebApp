using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.IdentityModel.Tokens;
using Sample.Auth.Enum;
using Sample.Auth.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sample.Auth
{
    internal class AuthorizationValueProvider : IValueProvider
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";
        private const string TENANT_ID_HEADER = "TenantId";
        private HttpRequest _request;
        private readonly IConfiguration _configuration;

        public AuthorizationValueProvider(HttpRequest request)
        {
            _request = request;

            var configBuilder = new ConfigurationBuilder().AddEnvironmentVariables();
            _configuration = configBuilder.Build();
        }

        public Type Type => typeof(ClaimsPrincipal);

        public Task<object> GetValueAsync()
        {
            try
            {
                if (_request.Headers.ContainsKey(AUTH_HEADER_NAME) &&
                   _request.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX, StringComparison.InvariantCultureIgnoreCase))
                {
                    var token = _request.Headers["Authorization"].ToString().Substring(BEARER_PREFIX.Length);

                    var handler = new JwtSecurityTokenHandler();
                    var tokendata = handler.ReadToken(token) as JwtSecurityToken;
                    var tokenUser = GetTokenUser(tokendata);


                    if (tokenUser.UserDBConnectionString == null)
                    {
                        tokenUser = GetUserAuthorizationDetails(tokenUser);
                    }

                    tokenUser.Token = token;

                    if (tokenUser.HasAccessToSubOperation)
                    {
                        return Task.FromResult<object>(AuthorizedRequest.Success(tokenUser));
                    }
                    else
                    {
                        return Task.FromResult<object>(AuthorizedRequest.Unauthorized());
                    }
                }
                else
                {
                    return Task.FromResult<object>(AuthorizedRequest.NoToken());
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return Task.FromResult<object>(AuthorizedRequest.Expired());
            }
            catch (Exception ex)
            {
                return Task.FromResult<object>(AuthorizedRequest.Unauthorized());
            }
        }

        public TokenUser GetTokenUser(JwtSecurityToken token)
        {
            TokenUser tokenUser = new TokenUser();
            var tid = token.Claims.FirstOrDefault(x => x.Type == "tid");
            return new TokenUser
              {
                 AzureUserId = Guid.Parse(token.Claims.FirstOrDefault(x => x.Type == "oid").Value),
                 TenantId = Guid.Parse(token.Claims.FirstOrDefault(x => x.Type == "tid").Value),
              };
        }

        public TokenUser GetUserAuthorizationDetails(TokenUser user)
        {
            var conString = string.Empty;
            try
            {
                var dbConnection = _configuration["CatalogDBConnectionString"];
                user = new AuthorizationCatalogDBSupport(user).Execute(dbConnection);
                if (user.TenantServer == null) return user;

                conString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                    user.TenantServer, user.TenantDatabase, user.TenantCommonLogin, user.TenantCommonPassword);

                user = new AuthorizationGetAccessAction(user, (int)System.Enum.Parse(typeof(OperationMethod), _request.Method),
                       GetRouteTemplate()).Execute(conString);
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        public string GetRouteTemplate()
        {
            try
            {
                if (_request.RouteValues.Keys.Count != _request.RouteValues.Values.Count)
                {
                    return _request.Path.Value;
                }
                string requestingRoute = _request.Path.Value;
                if (!requestingRoute.EndsWith("/"))
                {
                    requestingRoute = string.Concat(_request.Path.Value, "/");
                }
                List<string> routeValues = _request.RouteValues.Values.Select(x => x.ToString()).ToList();
                List<string> routeKeys = _request.RouteValues.Keys.Select(x => x.ToString()).ToList();
                for (int i = 0; i < _request.RouteValues.Keys.Count; i++)
                {
                    Regex regexTerm = new Regex($"/{routeValues[i]}/");
                    if (regexTerm.IsMatch(requestingRoute))
                    {
                        requestingRoute = regexTerm.Replace(requestingRoute, $"/{{{routeKeys[i]}}}/", 1);
                    }
                    else
                    {
                        throw new Exception("did not match any template parameters");
                    }
                }
                return Regex.Replace(requestingRoute, @"/$", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ToInvokeString() => string.Empty;
    }
}
