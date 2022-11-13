using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models;
using VPark_Models.Models;

namespace VPark_Data
{
    public class AppDbContext : IdentityDbContext
    {
       

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    //connect to postgres with connection string from app settings

        //    //options.UseNpgsql(Configuration.GetConnectionString("Default"));
        //}

        

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
