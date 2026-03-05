using E_Commerce.App.Application.Abstruction.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Abstruction.Services.Auth
{
    public interface IEmailService
    {
        public void SendEmail(string to , string subject , string boody );    
    }
}
