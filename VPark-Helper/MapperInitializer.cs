using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Dtos.AccountDto;
using VPark_Models.Dtos.BookingDtos;
using VPark_Models.Dtos.CardDetailsDtos;
using VPark_Models.Dtos.ParkingSpaceDto;
using VPark_Models.Models;

namespace VPark_Helper
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<AppUser, UserRegisterationDto>().ReverseMap();
            CreateMap<Booking, BookingRequestDto>().ReverseMap();
            CreateMap<Booking, BookingResponseDto>().ReverseMap();

            CreateMap<ParkingSpace, ParkingSpaceRequestDto>().ReverseMap();
            CreateMap<CardDetails, CardDetailsDto>().ReverseMap();
        }
    }
}
