using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entities;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        private IOwnerService OwnerService;

        public OwnerController(IOwnerService ownerService)
        {
            this.OwnerService = ownerService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Owner), 201)]
        [ProducesResponseType(400)][ProducesResponseType(500)]
        public ActionResult<Owner> CreateOwner([FromBody] Owner owner)
        {
            try
            {
                Owner ownerToAdd = OwnerService.CreateOwner(owner.FirstName,owner.LastName,owner.Address,owner.PhoneNumber,owner.Email);
                Owner addedOwner = OwnerService.AddOwner(ownerToAdd);

                if (addedOwner == null)
                {
                    return StatusCode(500, "Error saving pet to Database");
                }

                return Created("", addedOwner);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Owner>), 200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<Owner>> Get([FromQuery] Filter filter)
        {
            try
            {
                IEnumerable<Owner> ownerEnumerable = OwnerService.GetOwnersFilterSearch(filter);
                return Ok(ownerEnumerable);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, "Error loading owners. Please try again...");
            }
        }

        [HttpGet("{ID}")]
        [ProducesResponseType(typeof(Owner), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Owner> GetByID(int ID)
        {
            try
            {
                Owner owner = OwnerService.GetOwnerByIDIncludePets(ID);
                if (owner != null)
                {
                    return Ok(owner);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error loading owner with ID: {ID}\nPlease try again...");
            }
        }

        [HttpPut("{ID}")]
        [ProducesResponseType(typeof(Owner), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Owner> UpdateByID(int ID, [FromBody] Owner owner)
        {
            try
            {
                if (OwnerService.GetOwnerByID(ID) == null)
                {
                    return NotFound("No owner with such ID found");
                }

                Owner ownerToUpdate = OwnerService.CreateOwner(owner.FirstName, owner.LastName, owner.Address, owner.PhoneNumber, owner.Email);
                Owner updatedOwner = OwnerService.UpdateOwner(ownerToUpdate, ID);

                if (updatedOwner == null)
                {
                    return StatusCode(500, "Error updating owner in Database");
                }
                return Accepted(updatedOwner);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(typeof(Owner), 202)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Owner> DeleteByID(int ID)
        {
            if (OwnerService.GetOwnerByID(ID) == null)
            {
                return NotFound("No owner with such ID found");
            }
            try
            {
                Owner owner = OwnerService.DeleteOwner(ID);
                return (owner != null) ? Accepted(owner) : StatusCode(500, $"Server error deleting owner with Id: {ID}");
            }
            catch(ArgumentException ex)
            {
                return StatusCode(500, $"Server error deleting owner with Id: {ID}");
            }
        }
    }
}
