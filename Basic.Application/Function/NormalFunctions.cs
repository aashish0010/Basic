using Basic.Domain.Entity;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Basic.Application.Function
{
    public static class NormalFunctions
    {

        private static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }

        public static string encrypt(string encryptString)
        {

            string EncryptionKey = "A53639FF6C1525AE31F99ADF3A98E";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            var EncryptionKey = "A53639FF6C1525AE31F99ADF3A98E";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static dynamic CloudImageUpload(IFormFile img)
        {
            try
            {
                var myAccount = new Account { ApiKey = CommonConfig.ConfigValue("CloudinaryApiKey"), ApiSecret = CommonConfig.ConfigValue("CloudinaryApiSecret"), Cloud = CommonConfig.ConfigValue("CloudinaryCloudName") };
                Cloudinary _cloudinary = new Cloudinary(myAccount);
                using (var stream = img.OpenReadStream())
                {
                    var uploadparam = new ImageUploadParams()
                    {
                        File = new FileDescription(img.FileName, stream),
                        // Transformation = new Transformation().Width(200).Height(200).Crop("thumb").Gravity("face")
                    };
                    var uploadResult = _cloudinary.Upload(uploadparam);
                    return uploadResult;
                };

            }
            catch (Exception ex)
            {
                return null;
            }


        }



        public static DashBoard GetUserClaimsData(string token)
        {
            var dash = new DashBoard();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            dash.UserName = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "unique_name").Value);
            dash.Email = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "Email").Value);
            dash.Role = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "Role").Value);
            dash.Isactive = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "Isadmin").Value);
            return dash;
        }


        public static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

    }


}
