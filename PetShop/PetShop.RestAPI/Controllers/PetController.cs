using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entities;
using PetShop.RestAPI.DTO;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetController : ControllerBase
    {
        private IPetService PetService;
        private IOwnerService OwnerService;

        public PetController(IPetService petService, IOwnerService ownerService)
        {
            this.PetService = petService;
            this.OwnerService = ownerService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Pet), 201)]
        [ProducesResponseType(400)][ProducesResponseType(500)]
        public ActionResult<Pet> CreatePet([FromBody] Pet pet)
        {
            try
            {
                Pet petToAdd = PetService.CreatePet(pet.Name, pet.Type, pet.Birthdate, pet.petColors, pet.Price);
                petToAdd.SoldDate = pet.SoldDate;
                Pet addedPet;

                if(pet.Owner != null)
                {
                    if(pet.Owner.ID <= 0)
                    {
                        return BadRequest("Owner ID can't be zero or negative");
                    }

                    Owner owner = OwnerService.GetOwnerByID(pet.Owner.ID);

                    if(owner == null)
                    {
                        return BadRequest("No owner with that ID found");
                    }
                    petToAdd.Owner = owner;
                }

                if(pet.Type == null)
                {
                    return BadRequest("No pet type selected");
                }





                addedPet = PetService.AddPet(petToAdd);

                if (addedPet == null)
                {
                    return StatusCode(500, "Error saving pet to Database");
                }

                return Created("", addedPet);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(typeof(IEnumerable<PetDTO>), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<IEnumerable<PetDTO>> Get([FromQuery]Filter filter)
        {
            try
            {
                IEnumerable<Pet> petEnumerable = PetService.GetPetsFilterSearch(filter);
                List<PetDTO> pets = new List<PetDTO>();

                foreach (Pet pet in petEnumerable)
                {
                    pets.Add(new PetDTO(pet));
                }


                return Ok(pets);
            }
            catch (InvalidDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Error loading pets. Please try again...");
            }
        }

        [HttpGet("{ID}")]
        [ProducesResponseType(typeof(Pet), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Pet> GetByID(int ID)
        {
            try
            {
                Pet pet = PetService.GetPetByID(ID);
                if (pet != null)
                {
                    return Ok(pet);
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error loading pet with ID: {ID}\nPlease try again...");
            }
        }

        [HttpPut("{ID}")]
        [ProducesResponseType(typeof(Pet), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Pet> UpdateByID(int ID, [FromBody] Pet pet)
        {
            try
            {
                Pet existingPet = PetService.GetPetByID(ID);

                if (existingPet == null)
                {
                    return NotFound("No pet with such ID found");
                }

                Pet petToAUpdate = PetService.CreatePet(pet.Name, pet.Type, pet.Birthdate, pet.petColors, pet.Price);
                petToAUpdate.SoldDate = existingPet.SoldDate;

                if (pet.Owner != null)
                {
                    if (pet.Owner.ID <= 0)
                    {
                        return BadRequest("Owner ID can't be zero or negative");
                    }

                    Owner owner = OwnerService.GetOwnerByID(pet.Owner.ID);

                    if (owner == null)
                    {
                        return NotFound("No owner with that ID found");
                    }
                    petToAUpdate.Owner = owner;
                }

                if (pet.Type.ID <= 0)
                {
                    return BadRequest("Pet type ID can't be zero or negative");
                }

                if (pet.Type == null)
                {
                    return BadRequest("No pet type selected");
                }

                Pet updatedPet = PetService.UpdatePet(petToAUpdate, ID);

                if(updatedPet == null)
                {
                    return StatusCode(500, "Error updating pet in Database");
                }
                return Accepted(updatedPet);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(typeof(Pet), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Pet> DeleteByID(int ID)
        {
            if (PetService.GetPetByID(ID) == null)
            {
                return NotFound("No pet with such ID found");
            }

            try
            {
                Pet pet = PetService.DeletePet(ID);
                return (pet != null) ? Accepted(pet) : StatusCode(500, $"Server error deleting pet with Id: {ID}");
            }
            catch(ArgumentException ex)
            {
               return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error deleting pet with Id: {ID}");
            }
        }
    }
}
