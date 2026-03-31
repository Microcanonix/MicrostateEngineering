using System;
using System.Collections.Generic;
using System.Text;

namespace CommonDomain
{
    public class ServiceException : ApplicationException
    {
        public ServiceException(string serviceName, string message)
                : base($"{serviceName}: {message}")
        {

        }

        public ServiceException(string serviceName, string message, Exception innerException)
                : base($"{serviceName}: {message}", innerException)
        {

        }


    }
}
