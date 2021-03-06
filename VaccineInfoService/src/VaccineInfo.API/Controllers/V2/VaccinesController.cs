using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;
using VaccineInfo.Api.Dtos;
using VaccineInfo.Api.Extensions;
using VaccineInfo.Core.Interfaces.Services;
using VaccineInfo.Core.Models;


namespace VaccineInfo.Api.Controllers.V2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")] //OR, [Route("[vaccines]")] 
    [ApiVersion("2.0")]
    public class VaccinesController : ControllerBase
    {
        private readonly IVaccineService _vaccineService;
        private readonly IMapper _mapper;
        private readonly ILogger<VaccinesController> _logger;

        public VaccinesController(IVaccineService vaccineService, IMapper mapper, ILogger<VaccinesController> logger)
        {
            _vaccineService = vaccineService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]  //GET /vaccines
        public async Task<IEnumerable<VaccineDto>> GetVaccinesAsync()
        {
            _logger.LogInformation("STARTED: GetVaccinesAsync()");

            IEnumerable<VaccineDto> vaccines = null;
            using (Operation.Time("DB Operation: GetVaccinesAsync"))
            {
                vaccines = (await _vaccineService.GetVaccinesAsync()).Select(v => v.AsDto()); //[Hint: An alternative way without Automapper]
            }

            _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {vaccines.Count()} vaccines");

            return vaccines;
        }

        [HttpGet("{id}")]  //GET /vaccines/{id}
        public async Task<ActionResult<VaccineDto>> GetVaccineAsync(Guid id)
        {
            _logger.LogInformation("STARTED: GetVaccineAsync(Guid id), ID: {id}", id.ToString());
            var v = await _vaccineService.GetVaccineAsync(id);
            if (v is null)
            {
                return NotFound();
            }
            return Ok(v.AsDto());
        }
        
        [HttpPost] //POST /vaccines
        public async Task<ActionResult<VaccineDto>> CreateVaccineAsync(CreateVaccineDto vaccineDto)
        {
            Vaccine vaccine = new()
            {
                Id = Guid.NewGuid(),
                Name = vaccineDto.Name,
                MaxPrice = vaccineDto.MaxPrice,
                NumberOfDoses = vaccineDto.NumberOfDoses,
                MinDaysBetweenDoses = vaccineDto.MinDaysBetweenDoses,
                ManufacturerName = vaccineDto.ManufacturerName,
                ManufacturerWebsite = vaccineDto.ManufacturerWebsite,
                LocalApprovalDate = vaccineDto.LocalApprovalDate,
                ApprovedBy = vaccineDto.ApprovedBy,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _vaccineService.CreateVaccineAsync(vaccine);
            return CreatedAtAction(nameof(GetVaccineAsync), new { id = vaccine.Id }, vaccine.AsDto());
        }

        [HttpPut("{id}")] //PUT /vaccines/{id}
        public async Task<ActionResult> UpdateVaccineAsync(Guid id, UpdateVaccineDto vaccineDto)
        {
            var existingVaccine = await _vaccineService.GetVaccineAsync(id);
            if (existingVaccine is null)
            {
                return NotFound();
            }

            Vaccine updateVaccine = existingVaccine with
            {
                Name = vaccineDto.Name,
                MaxPrice = vaccineDto.MaxPrice,
                NumberOfDoses = vaccineDto.NumberOfDoses,
                MinDaysBetweenDoses = vaccineDto.MinDaysBetweenDoses,
                ManufacturerWebsite = vaccineDto.ManufacturerWebsite,
                ApprovedBy = vaccineDto.ApprovedBy
            };

            await _vaccineService.UpdateVaccineAsync(updateVaccine);
            return NoContent();
        }

        [HttpPatch("{id}")] //PATCH /vaccines/{id}
        public async Task<ActionResult> UpdateVaccinePartialAsync(Guid id, [FromBody]JsonPatchDocument<PatchVaccineDto> vaccineDto)
        {
            //1. Get the original vaccine object from the repository/database.
            var existingVaccine = await _vaccineService.GetVaccineAsync(id); 
            if (existingVaccine is null)
            {
                return NotFound();
            }

            //2. Use Automapper to map that original object to a new DTO object. [source type: Vaccine, destination type: PatchVaccineDto]
            PatchVaccineDto patchVaccineDto = _mapper.Map<PatchVaccineDto>(existingVaccine);

            //3. Apply the patch to the new DTO object from the received DTO. 
            vaccineDto.ApplyTo(patchVaccineDto);


            //4. Use automapper to map the updated DTO back to the original database object.
            _mapper.Map(patchVaccineDto, existingVaccine);
            //existingVaccine = _mapper.Map<Vaccine>(patchVaccineDto);

            //5. Update our vaccine in the database.
            await _vaccineService.UpdateVaccineAsync(existingVaccine); 
            return NoContent();
        }


        [HttpDelete("{id}")] //DELETE /vaccines/{id}
        public async Task<ActionResult> DeleteVaccineAsync(Guid id)
        {
            var existingVaccine = await _vaccineService.GetVaccineAsync(id);
            if (existingVaccine is null)
            {
                return NotFound();
            }
            await _vaccineService.DeleteVaccineAsync(id);
            return NoContent();
        }        
    }

}