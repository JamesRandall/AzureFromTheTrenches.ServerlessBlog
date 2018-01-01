using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessBlog.DataAccess
{
    class ReservedNameException : Exception
    {
        public ReservedNameException(string message) : base(message)
        {
        }
    }
}
