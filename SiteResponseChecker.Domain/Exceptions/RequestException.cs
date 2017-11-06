using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteResponseChecker.Domain.Exceptions
{
    public class RequestException : Exception
    {
        public RequestException(string webExceptionResponse, System.Net.WebException ex) : base(webExceptionResponse, ex)
        {
        }

        public RequestException(Exception ex) : base(ex.Message, ex)
        {
        }
    }
}
