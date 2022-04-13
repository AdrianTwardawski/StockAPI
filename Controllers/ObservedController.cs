﻿using AutoMapper;
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
    [ApiController]
    [Route("api/market/observed")]
    public class ObservedController : ControllerBase
    {
        private readonly IObservedService _service;

        public ObservedController(IObservedService service)
        {
            _service = service;
        }


        [HttpPost]
        public ActionResult CreateObserved([FromBody] CreateObservedDto dto)
        {
            var id = _service.CreateObserved(dto);
            return Created($"/api/observed/{id}", null);
        }

        [HttpGet]
        public ActionResult GetObserved()
        {
            var observed = _service.GetAll();
            return Ok(observed);
        }


        [HttpGet("{id}")]
        public ActionResult GetObserved([FromRoute] int id)
        {
            var observed = _service.GetById(id);
            return Ok(observed);
        }

        [HttpDelete("{id}")]
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
