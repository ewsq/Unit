using Drifting.JWT.Provider;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Unit.Data
{
    public class STokenProvider
    {
        private readonly TokenProviderOptions _options;
        public STokenProvider(TokenProviderOptions options)
        {
            _options = options;
        }
        public TokenEntity GenerateToken(params Claim[] otherClaims)
        {
            var now = DateTime.Now;
            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Iat,ToUnixEpochDate(now).ToString(),ClaimValueTypes.Integer64)
            };
            if (otherClaims != null)
            {
                claims = claims.Concat(otherClaims);
            }
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: _options.Expiration,
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new TokenEntity()
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_options.ValidFor.TotalSeconds
            };
        }
        public static long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }
    }

}
