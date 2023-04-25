using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper) 
        { 
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService= mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!await _cityInfoRepository.CityExistsAsync(cityId))
                {
                    _logger.LogInformation(
                        $"City with id {cityId} wasn.t found when accessing points of interesr.");
                    return NotFound();
                }
                var pointsOfInterestForCity = await _cityInfoRepository
                    .GetPointsOfInterestForCityAsync(cityId);

                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting points of interest for city with id {cityId}.",
                    ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInerest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId, int pointOfInterestId)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if(pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        //[HttpPost]
        //public ActionResult<PointOfInterestDto> CreatePointOfInterest(
        //    int cityId,
        //    PointOfInterestForCreationDto pointOfInterest)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var city = new City("");

        //    if (city == null)
        //        return NotFound();

        //    var maxPointOfInterestId = 1;

        //    var finalPointOfInterest = new PointOfInterestDto()
        //    {
        //        Id = ++maxPointOfInterestId,
        //        Name = pointOfInterest.Name,
        //        Description = pointOfInterest.Description
        //    };

        //    city.PointsOfInterest.Add(finalPointOfInterest);

        //    return CreatedAtRoute("GetPointOfInerest",
        //        new
        //        {
        //            cityId = cityId,
        //            pointOfInterestId = finalPointOfInterest.Id
        //        },
        //        finalPointOfInterest);
        //}

        //[HttpPut("{pointofinterestid}")]
        //public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
        //    PointOfIterestForUpdateDto pointOfInterest)
        //{
        //    var city = new City("");

        //    if (city == null)
        //        return NotFound();

        //    var pointOfInterestFromStore = city.PointsOfInterest
        //        .FirstOrDefault(c => c.Id == pointOfInterestId);

        //    if(pointOfInterestFromStore == null) 
        //        return NotFound();

        //    pointOfInterestFromStore.Name = pointOfInterest.Name;
        //    pointOfInterestFromStore.Description = pointOfInterest.Description;

        //    return NoContent();
        //}

        //[HttpPatch("{pointofinterestid}")]
        //public ActionResult PartiallyUpdatePointOfInterest(
        //    int cityId, int pointOfInterestId,
        //    JsonPatchDocument<PointOfIterestForUpdateDto> patchDocument)
        //{
        //    var city = null;

        //    if (city == null)
        //        return NotFound();

        //    var pointOfInterestFromStore = city.PointsOfInterest
        //        .FirstOrDefault(c => c.Id == pointOfInterestId);

        //    if (pointOfInterestFromStore == null)
        //        return NotFound();

        //    var pointOfInteresrtToPatch =
        //        new PointOfIterestForUpdateDto()
        //        {
        //            Name = pointOfInterestFromStore.Name,
        //            Description = pointOfInterestFromStore.Description
        //        };

        //    patchDocument.ApplyTo(pointOfInteresrtToPatch, ModelState);

        //    if(!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    if(!TryValidateModel(pointOfInteresrtToPatch))
        //        return BadRequest(ModelState);

        //    pointOfInterestFromStore.Name = pointOfInteresrtToPatch.Name;
        //    pointOfInterestFromStore.Description = pointOfInteresrtToPatch.Description;

        //    return NoContent();
        //}

        //[HttpDelete("{pointofinterestid}")]
        //public ActionResult DelePointOfInterest(int cityId, int pointOfInterestId)
        //{
        //    var city = null;

        //    if (city == null)
        //        return NotFound();

        //    var pointOfInterestFromStore = city.PointsOfInterest
        //        .FirstOrDefault(c => c.Id == pointOfInterestId);

        //    if (pointOfInterestFromStore == null)
        //        return NotFound();

        //    city.PointsOfInterest.Remove(pointOfInterestFromStore);
        //    _mailService.Send("Point of interest deleted.",
        //        $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");
        //    return NoContent();
        //}

    }
}
