using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial.Utils
{
    public  class TResponse
    {
        public int ResponseCode{ get; set; }
        public bool ResponseStatus{ get; set; }
        public string ResponseMessage{ get; set; }
        public object ResponsePacket{ get; set; }
    }

    public  static class ResponseMessage
    {
        public static string LoginSuccess = "User Logged in successfully";
        public static string invalidPassword= "Invalid Password";
        public static string userNotFound= "User not found";
    }
}
