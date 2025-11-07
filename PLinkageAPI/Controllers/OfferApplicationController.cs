using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
using PLinkageShared.DTOs;
using AutoMapper;
using PLinkageShared.ApiResponse;
using PLinkageShared.Enums;
using PLinkageAPI.Services;
using PLinkageAPI.ValueObject;

namespace PLinkageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferApplicationController: ControllerBase
    {
        private readonly IOfferApplicationService _offerApplicationService;
        private readonly IMapper _mapper;

        public OfferApplicationController(IOfferApplicationService offerApplicationService, IMapper mapper)
        {
            _offerApplicationService = offerApplicationService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplicationOfferAsync([FromBody] OfferApplicationCreationDto offerApplicationCreationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<Guid>.Fail("Invalid project data."));

            var response = await _offerApplicationService.CreateApplicationOffer(offerApplicationCreationDto);
            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetSpecific),
            new { offerApplicationId = response.Data },
            response);
            //return Ok(response);

        }

        [HttpGet("{offerApplicationId}")]
        public async Task<IActionResult> GetSpecific(Guid offerApplicationId)
        {
            var response = await _offerApplicationService.GetSpecificOfferApplication(offerApplicationId);
            if (!response.Success)
                return NotFound(response);

            var offerApplicationDto = _mapper.Map<OfferApplicationDto>(response.Data);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOfferApplicationsOfUser([FromQuery] Guid userId, [FromQuery] UserRole userRole)
        {
            var response = await _offerApplicationService.GetOfferApplicationOfUser(userId, userRole);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("process")] // Approve, Reject, Negotiate
        public async Task<IActionResult> ProcessOfferApplication([FromBody] OfferApplicationProcessDto offerApplicationProcessDto)
        {
            var response = await _offerApplicationService.ProcessOfferApplication(offerApplicationProcessDto);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
