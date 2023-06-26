using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Activation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Jose;
using System.Security.Principal;

namespace HRFocusWCF
{
    public class Authen
    {
        private byte[] Base64UrlDecode(string arg) // This function is for decoding string to   
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding  
            s = s.Replace('_', '/'); // 63rd char of encoding  
            switch (s.Length % 4) // Pad with trailing '='s  
            {
                case 0: break; // No pad chars in this case  
                case 2: s += "=="; break; // Two pad chars  
                case 3: s += "="; break; // One pad char  
                default:
                    throw new System.Exception(
                "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder  
        }
        private long ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }


        static string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1889";

        public string GetJwt(string user, string pass ,string com) //function for JWT Token  
        {
            byte[] secretKey = Base64UrlDecode(key);//pass key to secure and decode it  
            DateTime issued = DateTime.Now.Date;
            var User = new Dictionary<string, object>()  
                    {  
                        {"company_code", com}, 
                        {"user_aabbcc", user},  
                        {"pass_qwer", pass},  
                         
                         {"iat", ToUnixTime(issued).ToString()}  
                    };

            string token = JWT.Encode(User, secretKey, JwsAlgorithm.HS256);
            return token;
        }


        public bool doCheckExpireToken(string iat)
        {

            DateTime issued = DateTime.Now.Date;
            string temp = ToUnixTime(issued).ToString();

            if (iat.Equals(temp))
                return true;
            else
                return false;
        }



        public string Encrypt(string input)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(input)));
        }
        private byte[] Encrypt(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("hjiweykaksd", new byte[] { 0x43, 0x87, 0x23, 0x72, 0x45, 0x56, 0x68, 0x14, 0x62, 0x84 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }
        public string Decrypt(string input)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(input)));
        }
        private byte[] Decrypt(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("hjiweykaksd", new byte[] { 0x43, 0x87, 0x23, 0x72, 0x45, 0x56, 0x68, 0x14, 0x62, 0x84 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }

        public bool ValidateToken(string authToken)
        {
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var validationParameters = GetValidationParameters();

            //SecurityToken validatedToken;
            //IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        //private TokenValidationParameters GetValidationParameters()
        //{
        //    return new TokenValidationParameters()
        //    {
        //        ValidateLifetime = true,
        //        RequireSignedTokens = true,
        //        RequireExpirationTime = true,
        //        ValidateAudience = false,
        //        ValidateIssuer = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // The same key as the one that generate the token
        //    };
        //}



    }
}