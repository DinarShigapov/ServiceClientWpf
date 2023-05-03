using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ServiceClientWpf.Model
{
    public partial class ClientService
    {
        public string EndTime
        {
            get 
            {
                var time = StartTime - DateTime.Now;
                return $"{time.ToString("hh")}ч. " +
                    $"{time.ToString("mm")}мин";
            }
        }

        public string ColorTime
        {
            get
            {
                var time = StartTime - DateTime.Now;
                if (time < new TimeSpan(1, 0, 0))
                    return "#ff0000";
                else
                    return "#000000";


            }
        }
    }
}
