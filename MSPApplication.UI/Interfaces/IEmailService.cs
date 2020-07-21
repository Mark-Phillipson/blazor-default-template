using MSPApplication.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Services
{
    public interface IEmailService
    {
        void SendEmail(Email email);
    }
}
