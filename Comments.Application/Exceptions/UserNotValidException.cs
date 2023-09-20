using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comments.Application.Exceptions
{
    public class UserNotValidException : Exception
    {
        public UserNotValidException(string userId, string operation) : base($"User '{userId}' not valid for operation")
        {
        }
    }
}
