using Family.Api.Helpers;
using Family.Core.DTOs;
using Family.Core.Entities;
using Family.Core.Repository.Interfaces;
using Family.Core.Specifications.PhotoSpecifications;
using Family.Core.Specifications.SliderSpecifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Family.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliderController : ControllerBase
    {
        private readonly IGenericRepository<SliderItem> _sliderItemRepo;

        public SliderController(IGenericRepository<SliderItem> sliderItemRepo)
        {
            _sliderItemRepo = sliderItemRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SliderItemDto>>> GetAllSliders()
        {
            var spec = new SliderItemSpecification();
            var sliderItems = await _sliderItemRepo.ListAsync(spec);
            return Ok(sliderItems.ToDtos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SliderItemDto>> GetSlider(int id)
        {
            var spec = new SliderItemSpecification(id);
            var sliderItem = await _sliderItemRepo.GetBySpecification(spec);

            if (sliderItem == null)
                return NotFound($"Slider item with ID {id} not found");

            return Ok(sliderItem.ToDto());
        }

    }
}
