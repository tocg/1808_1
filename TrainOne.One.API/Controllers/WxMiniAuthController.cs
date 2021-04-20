/*
 * 微信小程序
 * 
 * **/
using LitJson;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainOne.Lib.Common;
using TrainOne.One.API.Models;

namespace TrainOne.One.API.Controllers
{
    [Route("mini/auth")]
    [ApiController]
    public class WxMiniAuthController : ControllerBase
    {
        /// <summary>
        /// 获取微信用户唯一标识
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("code2session"), HttpGet]
        public IActionResult Code2Session(string code)
        {
            //根据wx.login的code去获取唯一标识信息（openid及secret）
            var obj = GetOpenidAndSessionkey(code);

            return Ok(new
            {
                openid = obj.Item1,
                session_key = obj.Item2
            });
        }

        /// <summary>
        /// 对手机号进行解码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("decodephone"), HttpPost]
        public IActionResult DecodePhone(DecodePhoneModel model)
        {
            var phone = new DecodeDecryptData()?.Decrypt(model.EncryptedData, model.IV, model.SessionKey);
            return Ok(phone);
        }

        #region 与微信服务器的请求

        private string _appId = "wx6369949d89e2a9ea";
        private string _secret = "ddb8445fcb3b64ca6aa62e9a6dc7902c";

        /// <summary>
        /// 获取openid和session_key
        /// </summary>
        private Tuple<string, string> GetOpenidAndSessionkey(string code)
        {
            try
            {
                string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={this._appId}&secret={this._secret}&js_code={code}&grant_type=authorization_code";

                string result = HttpService.Get(url);

                //Nuget包LitJson
                JsonData jd = JsonMapper.ToObject(result);
                var openid = (string)jd["openid"];
                var session_key = (string)jd["session_key"];

                return new Tuple<string, string>(openid, session_key);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
    }
}
