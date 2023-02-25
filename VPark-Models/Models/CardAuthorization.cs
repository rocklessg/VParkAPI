using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Models
{
    public class CardAuthorization : BaseEntity
    {
        public string Email { get; set; }
        public string AuthorizationCode { get; set; }
        public string AuthorizationUrl { get; set; }
    }
}
