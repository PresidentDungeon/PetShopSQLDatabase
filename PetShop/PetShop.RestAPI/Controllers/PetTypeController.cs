using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entities;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetTypeController : ControllerBase
    {
        private IPetTypeService PetTypeService;

        public PetTypeController(IPetTypeService petTypeService)
        {
            this.PetTypeService = petTypeService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PetType), 201)]
        [ProducesResponseType(400)][ProducesResponseType(500)]
        public ActionResult<PetType> CreatePetType([FromBody] PetType petType)
        {
            try
            {
                PetType petTypeToAdd = PetTypeService.CreatePetType(petType.Name);
                PetType addedPetType = PetTypeService.AddPetType(petTypeToAdd);

                if (addedPetType == null)
                {
                    return StatusCode(500, "Error saving pet to Database");
                }

                return Created("", addedPetType);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PetType>), 200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<PetType>> Get([FromQuery]Filter filter)
        {
            try
            {
                IEnumerable<PetType> petTypeEnumerable = PetTypeService.GetPetTypesFilterSearch(filter);
                return Ok(petTypeEnumerable);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Error loading pet types. Please try again...");
            }
        }

        [HttpGet("{ID}")]
        [ProducesResponseType(typeof(PetType), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<PetType> GetByID(int ID)
        {
            try
            {
                PetType type = PetTypeService.GetPetTypeByID(ID);
                if (type != null)
                {
                    return Ok(type);
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error loading pet type with ID: {ID}\nPlease try again...");
            }
        }

        [HttpPut("{ID}")]
        [ProducesResponseType(typeof(PetType), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<PetType> UpdateByID(int ID, [FromBody] PetType type)
        {
            try
            {
                if (PetTypeService.GetPetTypeByID(ID) == null)
                {
                    return NotFound("No pet type with such ID found");
                }

                PetType petTypeToAUpdate = PetTypeService.CreatePetType(type.Name);
                PetType updatedPetType = PetTypeService.UpdatePetType(petTypeToAUpdate, ID);

                if(updatedPetType == null)
                {
                    return StatusCode(500, "Error updating pet type in Database");
                }
                return Accepted(updatedPetType);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(typeof(PetType), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<PetType> DeleteByID(int ID)
        {
            if (PetTypeService.GetPetTypeByID(ID) == null)
            {
                return NotFound("No pet type with such ID found");
            }

            try
            {
                PetType petType = PetTypeService.DeletePetType(ID);
                return (petType != null) ? Accepted(petType) : StatusCode(500, $"Server error deleting pet type with Id: {ID}");
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error deleting pet type with Id: {ID}");
            }
        }
    }
}
