using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Dtos;
using VPark_Models.Models;

namespace VPark_Helper
{
    public class ServiceFee : IServiceFee
    {
        //private readonly AppDbContext _context;
        public string GetParkingSpaceFee(ServiceType serviceType, int parkingDuration)
        {
            decimal serviceFee = 0m;
            if (serviceType == ServiceType.Hour)
            {
                if (parkingDuration > 0 && parkingDuration <= 5)
                {
                    serviceFee = parkingDuration * 500;
                }
                else if (parkingDuration > 5)
                {
                    var firstFiveHoursCharges = 500 * 5;
                    var hourAboveFive = parkingDuration - 5;
                    var chargesAfterfirstFiveHours = hourAboveFive * 250;
                    serviceFee = firstFiveHoursCharges + chargesAfterfirstFiveHours;
                }
            }
            else if (serviceType == ServiceType.Day)
            {
                serviceFee = 7500m;
            }

            return serviceFee.ToString();
        }
    }

    public interface IServiceFee
    {
        string GetParkingSpaceFee(ServiceType serviceType, int parkingDuration);
    }
}
