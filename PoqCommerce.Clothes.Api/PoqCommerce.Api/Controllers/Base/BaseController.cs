using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PoqCommerce.Api.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private IMapper? _mapper;
        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}