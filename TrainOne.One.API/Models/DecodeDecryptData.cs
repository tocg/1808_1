using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TrainOne.One.API.Models
{
    public class DecodeDecryptData
    {
        /// <summary>
        /// 微信小程序解密算法
        /// </summary>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">初始向量</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns></returns>
        public string Decrypt(string encryptedData, string iv, string sessionKey)
        {
            try
            {
                //创建解密器生成工具实例
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                //设置解密器参数
                aes.Mode = CipherMode.CBC;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                //格式化待处理字符串
                byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
                byte[] byte_iv = Convert.FromBase64String(iv);
                byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);

                aes.IV = byte_iv;
                aes.Key = byte_sessionKey;
                //根据设置好的数据生成解密器实例
                ICryptoTransform transform = aes.CreateDecryptor();

                //解密
                byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);
                //生成结果
                string result = Encoding.UTF8.GetString(final);
                return result;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "SnsProcessing", "Decrypt");
            }
            return string.Empty;
        }
    }
}
