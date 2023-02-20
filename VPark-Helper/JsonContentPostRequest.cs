using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Helper
{
    public class JsonContentPostRequest <T>
    {
        public string Url { get; set; }
        public T Data { get; set; }
        public string AccessToken { get; set; }
    }
}
