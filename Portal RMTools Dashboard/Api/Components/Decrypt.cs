using Api.Models.SQLServer;
using Api.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Api.Components
{
    public interface IDecryptManager
    {
        DecryptUID Decrypt(string UID);

    }
    public class DecryptManager : IDecryptManager
    {

        private readonly CredentialAttr _appSettings;
        private readonly dbRmTools_Context _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;

        public DecryptManager(IOptions<CredentialAttr> appSettings, IConfiguration config, dbRmTools_Context context,
            IHttpContextAccessor accessor)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _configuration = config;
            _accessor = accessor;
        }

        public DecryptUID Decrypt(string UID)
        {

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    var secretKey = GetConfig.AppSetting["AppSettings:GlobalSettings:SecretKey"];

                    aesAlg.Key = Encoding.UTF8.GetBytes(secretKey);
                    aesAlg.IV = new byte[16];

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(UID)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                string decryptedIdString = srDecrypt.ReadToEnd();

                                if (int.TryParse(decryptedIdString, out int decryptedId))
                                {
                                    return new DecryptUID()
                                    {
                                        Id = decryptedId,
                                        status = true,
                                        message = ""
                                    };
                                }
                                else
                                {
                                    return new DecryptUID()
                                    {
                                        Id = 0,
                                        status = false,
                                        message = "UID not valid decrypted"
                                    };
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return new DecryptUID
                {
                    Id = 0,
                    status = false,
                    message = ex.Message
                };
            }


        }

    }

}
