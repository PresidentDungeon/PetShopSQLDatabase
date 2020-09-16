using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entities;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : ControllerBase
    {
        private IPetService PetService;
        private IOwnerService OwnerService;
        private IPetExchangeService PetExchangeService;

        public ExchangeController(IPetService petService, IOwnerService ownerService, IPetExchangeService petExchangeService)
        {
            this.PetService = petService;
            this.OwnerService = ownerService;
            this.PetExchangeService = petExchangeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Pet>), 200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<Pet>> Get()
        {
            try
            {
                IEnumerable<Pet> petEnumerable;
                petEnumerable = PetExchangeService.ListAllPetsWithOwner();

                return Ok(petEnumerable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error loading pets. Please try again...");
            }
        }

        [HttpGet("{ID}")]
        [ProducesResponseType(typeof(IEnumerable<Pet>), 200)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<IEnumerable<Pet>> GetPetsByOwner(int ID)
        {
            try
            {
                IEnumerable<Pet> petEnumerable;
                Owner owner = OwnerService.GetOwnerByID(ID);
                if (owner == null)
                {
                    return BadRequest("No owner with such an ID exists");
                }

                petEnumerable = PetExchangeService.ListAllPetsRegisteredToOwner(ID);

                if (petEnumerable.Count() <= 0)
                {
                    return NotFound();
                }
                return Ok(petEnumerable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error loading pets registered to owner. Please try again...");
            }

        }

        [HttpPost("{petID},{ownerID}")]
        [ProducesResponseType(typeof(Pet), 202)]
        [ProducesResponseType(400)][ProducesResponseType(500)]
        public ActionResult<Pet> RegisterPet(int petID, int ownerID)
        {
            Pet petToRegister = PetService.GetPetByID(petID);
            Owner ownerToRegister = OwnerService.GetOwnerByID(ownerID);

            if(petToRegister == null || ownerToRegister == null)
            {
                return BadRequest("No pet or owner with such an ID exists");
            }
            try
            {
                Pet updatedPet = PetExchangeService.RegisterPet(petToRegister, ownerToRegister);
                return (updatedPet != null) ? Accepted(updatedPet) : StatusCode(500, $"Server error when registering owner with Id: {ownerID} to pet with Id: {petID}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error when registering owner with Id: {ownerID} to pet with Id: {petID}");
            }
        }

        [HttpPost("{petID}")]
        [ProducesResponseType(typeof(Pet), 202)]
        [ProducesResponseType(400)][ProducesResponseType(500)]
        public ActionResult<Pet> UnregisterPet(int petID)
        {
            Pet petToUnregister = PetService.GetPetByID(petID);

            if (petToUnregister == null)
            {
                return BadRequest("No pet with such an ID exists");
            }
            try
            {
                Pet updatedPet = PetExchangeService.UnregisterPet(petToUnregister);
                return (updatedPet != null) ? Accepted(updatedPet) : StatusCode(500, $"Server error when unregistering pet with Id: {petID}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error when unregistering pet with Id: {petID}");
            }
        }
    }
}
