using PoqCommerce.Application.Extensions;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Application.Models.DTOs;

namespace PoqCommerce.Application.Factories
{
    public class FilterObjectDtoFactory : IFilterObjectDtoFactory
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterObjectDtoFactory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public FilterObjectDto CreateFilterObjectDto()
        {
            var productList = _unitOfWork.Product.GetAll().ToList();

            var filterDto = new FilterObjectDto
            {
                MinPrice = productList.Min(p => p.Price),
                MaxPrice = productList.Max(p => p.Price),
                Sizes = productList.SelectMany(p => p.Sizes).Distinct().ToList(),
                CommonWords = productList.GetCommonWords()
            };

            return filterDto;
        }
    }
}
