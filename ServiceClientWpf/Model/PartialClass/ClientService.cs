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
        public TimeSpan EndTime
        {
            get => StartTime - DateTime.Now;    
        }

        public string ColorTime
        {
            get => StartTime - DateTime.Now < new TimeSpan(1, 0, 0) ? "#ff0000" : "000000";
        }

    }
}
