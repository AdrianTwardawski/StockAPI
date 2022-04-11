using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Entities;
using StockAPI.Models;
using StockAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Controllers
{
    [Route("[controller]")]
    public class ObservedController : ControllerBase
    {
        private readonly IObservedService _service;

        public ObservedController(IMapper mapper, IObservedService service)
        {
            _service = service;
        }


        [HttpPost]
        public ActionResult CreateObserved([FromBody] CreateObservedDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _service.CreateObserved(dto);

            if (id == 0)
            {
                return NotFound($"Stock {dto.Name} doesn't exists");
            }

            return Created($"/api/observed/{id}", null);
        }

        [HttpGet("{id}")]
        public ActionResult GetObserved([FromRoute]int id)
        {
            var observed = _service.GetById(id);
            return Ok(observed);

        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            var isDeleted = _service.DeleteById(id);

            if(isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody]UpdateObservedDto dto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = _service.Update(dto, id);
            
            if(!isUpdated)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
