﻿using Microsoft.AspNetCore.Mvc;
using MSPApplication.Data.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSPApplication.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountryController : Controller
	{
		private readonly ICountryRepository _countryRepository;

		public CountryController(ICountryRepository countryRepository)
		{
			_countryRepository = countryRepository;
		}

		// GET: api/<controller>
		[HttpGet]
		public IActionResult GetCountries()
		{
			return Ok(_countryRepository.GetAllCountries());
		}

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public IActionResult GetCountryById(int id)
		{
			return Ok(_countryRepository.GetCountryById(id));
		}
	}
}
