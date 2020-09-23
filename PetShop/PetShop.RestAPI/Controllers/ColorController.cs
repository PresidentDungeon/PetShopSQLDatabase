using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entities;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorController : ControllerBase
    {
        private IColorService ColorService;

        public ColorController(IColorService colorService)
        {
            this.ColorService = colorService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Color), 201)]
        [ProducesResponseType(400)][ProducesResponseType(500)]
        public ActionResult<Color> CreateColor([FromBody] Color color)
        {
            try
            {
                Color colorToAdd = ColorService.CreateColor(color.ColorDescription);
                Color addedColor = ColorService.AddColor(colorToAdd);

                if (addedColor == null)
                {
                    return StatusCode(500, "Error saving color to Database");
                }

                return Created("", addedColor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Color>), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<IEnumerable<Color>> Get([FromQuery]Filter filter)
        {
            try
            {
                IEnumerable<Color> colorEnumerable = ColorService.GetColorsFilterSearch(filter);
                return Ok(colorEnumerable);
            }
            catch (InvalidDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error loading colors. Please try again...");
            }
        }

        [HttpGet("{ID}")]
        [ProducesResponseType(typeof(Color), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Color> GetByID(int ID)
        {
            try
            {
                Color color = ColorService.GetColorByID(ID);
                if (color != null)
                {
                    return Ok(color);
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error loading color with ID: {ID}\nPlease try again...");
            }
        }

        [HttpPut("{ID}")]
        [ProducesResponseType(typeof(Color), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Color> UpdateByID(int ID, [FromBody] Color color)
        {
            try
            {
                if (ColorService.GetColorByID(ID) == null)
                {
                    return NotFound("No color with such ID found");
                }

                Color colorToUpdate = ColorService.CreateColor(color.ColorDescription);
                Color updatedColor = ColorService.UpdateColor(colorToUpdate, ID);

                if(updatedColor == null)
                {
                    return StatusCode(500, "Error updating color in Database");
                }
                return Accepted(updatedColor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(typeof(Color), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Color> DeleteByID(int ID)
        {
            if (ColorService.GetColorByID(ID) == null)
            {
                return NotFound("No color with such ID found");
            }

            try
            {
                Color color = ColorService.DeleteColor(ID);
                return (color != null) ? Accepted(color) : StatusCode(500, $"Server error deleting color with Id: {ID}");
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error deleting color with Id: {ID}");
            }
        }
    }
}
