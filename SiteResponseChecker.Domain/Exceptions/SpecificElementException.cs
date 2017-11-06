using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteResponseChecker.Domain.Exceptions
{
    public class SpecificElementException : Exception
    {
        public SpecificElementException(Exception ex) : base(ex.Message, ex)
        {
        }
    }
}
