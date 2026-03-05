using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Exception
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object? key) : base($"{name} with {key} ")
        {

        }
    }
}
