using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CardDetails>()
                .Property(p => p.CardType)
                .HasConversion(
                    v => v.ToString(),
                    v => (CardType)Enum.Parse(typeof(CardType), v));
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CardDetails> CardDetails { get; set; }
        public DbSet<CardAuthorization> CardAuthorizations { get; set; }

    }
}
