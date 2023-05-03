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

        public string FormattedPhoneNumber
        {
            get
            {
                if (Phone == null)
                    return string.Empty;

                switch (Phone.Length)
                {
                    case 7:
                        return Regex.Replace(Phone, @"(\d{3})(\d{4})", "$1-$2");
                    case 10:
                        return Regex.Replace(Phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                    case 11:
                        return Regex.Replace(Phone, @"(\d{1})(\d{3})(\d{3})(\d{4})", "$1-$2-$3-$4");
                    default:
                        return Phone;
                }
            }
        }
    }
}
