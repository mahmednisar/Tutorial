using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutorial.Dto;
using Tutorial.Utils;

namespace Tutorial.Services.Infrastructure
{
    public interface IAuthService
    {

        /// <summary>
        /// This method is used to Login user.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        TResponse Login(LoginDto login);
    }
}
