using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainOne.One.API.Models
{
    /// <summary>
    /// 微信手机号解密
    /// </summary>
    public class DecodePhoneModel
    {
        public string EncryptedData { get; set; }
        public string IV { get; set; }
        public string SessionKey { get; set; }
        public string OpenID { get; set; }
    }
}
