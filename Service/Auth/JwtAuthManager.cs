using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service.Auth
{
    public class Jwt
    {
        public static string SecretKey;
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string UserAgent { get; set; }
    }

    public class JwtAuthToken
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("useragent")]
        public string UserAgent { get; set; }

        [JsonPropertyName("expireAt")]
        public DateTime ExpireAt { get; set; }
    }

    public interface IJwtAuthManager
    {
        string CreateToken(UserInfo User);

        void RemoveExpiredTokens(DateTime now);

        void RemoveTokenByUserName(string userName);
    }

    public class JwtAuthManager : IJwtAuthManager
    {
        public static ConcurrentDictionary<string, JwtAuthToken> _usersTokens;

        public JwtAuthManager()
        {
            //_jwt = jwt;
            _usersTokens = new();
        }

        public string CreateToken(UserInfo info)
        {
            byte[] key = Encoding.ASCII.GetBytes(Jwt.SecretKey);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var expire = DateTime.Now.AddMinutes(5);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, info.Username),
                    new Claim("USERID", info.Id.ToString()),
                    new Claim("USERNAME", info.Username),
                    new Claim("ROLE", "ThanhVien"),
                    new Claim("NAME", info.Name)
                }),
                Expires = expire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var secutiryToken = handler.CreateToken(descriptor);
            var token = handler.WriteToken(secutiryToken);
            var userToken = new JwtAuthToken { AccessToken = token, ExpireAt = expire, UserAgent = info.UserAgent };
            _usersTokens.AddOrUpdate(userToken.AccessToken, userToken, (_, _) => userToken);
            return token;
        }

        public void RemoveExpiredTokens(DateTime now)
        {
            var expiredTokens = _usersTokens.Where(x => x.Value.ExpireAt < now).ToList();
            foreach (var expiredToken in expiredTokens)
            {
                _usersTokens.TryRemove(expiredToken.Key, out _);
            }
        }

        public void RemoveTokenByUserName(string userName)
        {
            var accessTokens = _usersTokens.Where(x => x.Value.UserName == userName).ToList();
            foreach (var accessToken in accessTokens)
            {
                _usersTokens.TryRemove(accessToken.Key, out _);
            }
        }
    }
}