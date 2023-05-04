using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceClientWpf.Model
{
    public partial class Client
    {
        public string ClientFullName
        {
            get 
            {
                return $"{LastName} {FirstName.ToCharArray()[0]}. {Patronymic.ToCharArray()[0]}.";
            }
        }
    }
}
