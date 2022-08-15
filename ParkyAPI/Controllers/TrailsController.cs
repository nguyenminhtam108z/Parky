using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version:apiVersion}/Trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private ITrailRepository _trailRepo;
        private readonly IMapper _mapper;
        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;

        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();
            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }
        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="TrailId">The id of trail</param>
        /// <returns></returns>
        [HttpGet("{TrailId:int}",Name ="GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public IActionResult GetTrail(int TrailId)
        {
            var obj = _trailRepo.GetTrail(TrailId);
            if(obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }
        /// <summary>
        /// Get trails in national parks
        /// </summary>
        /// <param name="nationalParkId">The id of national park</param>
        /// <returns></returns>
        [HttpGet("GetTrailInNationalPark/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }
        /// <summary>
        /// Create a trail
        /// </summary>
        /// <param name="TrailDto">trail</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailICreateDto TrailDto)
        {
            if(TrailDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_trailRepo.TrailExists(TrailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var TrailObj = _mapper.Map<Trail>(TrailDto);
            if(!_trailRepo.CreateTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { TrailId = TrailObj.Id }, TrailObj);
        }
        /// <summary>
        /// Update a trail
        /// </summary>
        /// <param name="TrailId">Id of trail</param>
        /// <param name="TrailDto">trail</param>
        /// <returns></returns>
        [HttpPatch("{TrailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int TrailId,[FromBody] TrailUpdateDto TrailDto)
        {
            if(TrailDto==null || TrailId != TrailDto.Id)
                return BadRequest(ModelState);
            var TrailObj = _mapper.Map<Trail>(TrailDto);
            if(!_trailRepo.UpdateTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        /// <summary>
        /// Delete a trail
        /// </summary>
        /// <param name="TrailId">Id of trail</param>
        /// <returns></returns>
        [HttpDelete("{TrailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int TrailId)
        {
            if (!_trailRepo.TrailExists(TrailId))
                return NotFound();
            var TrailObj = _trailRepo.GetTrail(TrailId);
            if (!_trailRepo.DeleteTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when delete the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
