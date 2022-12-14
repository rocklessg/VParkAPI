using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;

namespace VPark_Core.Repositories.Implementation
{
    public class PaystackRepository : IPaystackRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaystackRepository> _logger;
        public PaystackRepository(AppDbContext context, ILogger<PaystackRepository> logger)
        {
            _context = context;
            _logger = logger;
        }



    }
}
