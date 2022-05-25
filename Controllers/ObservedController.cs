using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Entities;
using StockAPI.Models;
using StockAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StockAPI.Controllers
{
    [ApiController] //odpowiada za walidację modelu w każdym zapytaniu
    [Route("api/market/observed")]
    [Authorize]
    public class ObservedController : ControllerBase
    {
        private readonly IObservedService _service;

        public ObservedController(IObservedService service)
        {
            _service = service;
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager")] //nadpisuje Authorize z poziomu Controllera
        public ActionResult CreateObserved([FromBody] CreateObservedDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id = _service.CreateObserved(dto);
            return Created($"/api/observed/{id}", null);
        }

        [HttpGet]
        //[Authorize(Policy = "Atleast20")]
        [Authorize(Policy = "Atleast2Stocks")]
        public ActionResult GetObserved()
        {
            var observed = _service.GetAll();
            return Ok(observed);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult GetObserved([FromRoute] int id)
        {
            var observed = _service.GetById(id);
            return Ok(observed);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "HasNationality")]
        public ActionResult Delete([FromRoute] int id)
        {
            _service.DeleteById(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateObservedDto dto, [FromRoute] int id)
        {
            _service.Update(dto, id);
            return Ok();
        }

    }
}
