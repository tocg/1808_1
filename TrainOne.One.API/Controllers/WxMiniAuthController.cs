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

namespace TrainOne.One.API.Controllers
{
    [Route("mini/auth")]
    [ApiController]
    public class WxMiniAuthController : ControllerBase
    {
        [Route("info"), HttpGet]
        public IActionResult GetUserInfo(string code)
        {
            //根据wx.login的code去获取用户信息（openid及secret）

            var openid = GetOpenidAndSessionkey(code);
            return Ok(openid);

        }

        #region 与微信服务器的请求

        private string _appId = "wx6369949d89e2a9ea";
        private string _secret = "ddb8445fcb3b64ca6aa62e9a6dc7902c";

        /// <summary>
        /// 获取openid和session_key
        /// </summary>
        private string GetOpenidAndSessionkey(string code)
        {
            try
            {
                string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={this._appId}&secret={this._secret}&js_code={code}&grant_type=authorization_code";

                string result = HttpService.Get(url);

                //Nuget包LitJson
                JsonData jd = JsonMapper.ToObject(result);
                var openid = (string)jd["openid"];
                var session_key = (string)jd["session_key"];

                return openid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
    }
}
