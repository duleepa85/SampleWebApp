using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataAccess.Exceptions
{
    internal class SampleException : System.Exception
    {
        private string _errorMessage = string.Empty;

        public void logg(string loger)
        {
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }

        public override string Message
        {
            get
            {
                return _errorMessage;
            }
        }
    }
}
