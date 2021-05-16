using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Tutorial.Dto;
using Tutorial.Services.Infrastructure;
using Tutorial.Utils;
using Microsoft.Extensions.Options;
using Tutorial.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Tutorial.Services
{
    public class AuthService : IAuthService
    {
        #region Constructor


        public AuthService(IConfiguration configuration, IOptions<AppSetting> options)
        {
            _appSetting = options.Value;
            _dataManager = new DataManager(configuration);
        }

        #endregion

        #region ClassMember

        private readonly AppSetting _appSetting;
        private TResponse _response = new TResponse();
        private readonly DataManager _dataManager;
        private DataSet _dataSet = new DataSet();
        private DataTable _dataTable = new DataTable();
        private List<Parameter> _list = new List<Parameter>();
        #endregion

        /// <summary>
        /// This method is used to Login user.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public TResponse Login(LoginDto login)
        {

            _dataTable = _dataManager.GetTable("select Usr_kid, Usr_Code, Usr_Passkey, Usr_Password from H_Usr where Usr_Status =1 and  Usr_Code = '" + login.Username + "'");
            if (_dataTable != null && _dataTable.Rows.Count > 0)
            {
                var password = _dataTable.Rows[0]["Usr_Password"].ToString();
                var passkey = _dataTable.Rows[0]["Usr_Passkey"].ToString();

                if (string.Equals(password, Cryptography.Encrypt(login.Password, passkey)))
                {
                    var userInfo = new UserInfo { AuthKey = new Guid().ToString(), Token = GenerateToken(_dataTable.Rows[0]["Usr_kid"].ToString()) };

                    _response.ResponseCode = (int)HttpStatusCode.OK;
                    _response.ResponseStatus = true;
                    _response.ResponseMessage = ResponseMessage.LoginSuccess;
                    _response.ResponsePacket = userInfo;
                }
                else
                {
                    _response.ResponseCode = (int)HttpStatusCode.Unauthorized;
                    _response.ResponseStatus = false;
                    _response.ResponseMessage = ResponseMessage.invalidPassword;
                }
            }
            else
            {
                _response.ResponseCode = (int)HttpStatusCode.NotFound;
                _response.ResponseStatus = false;
                _response.ResponseMessage = ResponseMessage.userNotFound;
            }
            return _response;
        }

        public string GenerateToken(string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secBytes = Encoding.ASCII.GetBytes(_appSetting.Secret);
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", id) }),
                Expires = DateTime.Now.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(descriptor));

        }
    }
}
