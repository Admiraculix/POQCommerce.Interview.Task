using AutoMapper;
using PoqCommerce.Api.Models.Contracts.Requests;
using PoqCommerce.Application.Models;

namespace PoqCommerce.Api.AutoMapper.Profiles
{
    public class RequestToDtoProfile : Profile
    {
        public RequestToDtoProfile()
        {
            MapFilterObject();
        }

        private void MapFilterObject()
        {
            CreateMap<FilterObjectRequest, FilterObject>();
        }
    }
}
