using Address_Book.Models;
using Address_Book.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Address_Book.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly List<AddressBookEntry> _addressBook = new List<AddressBookEntry>()
        {
            new AddressBookEntry()
            {
                Id = 1,
                FirstName = "Jan",
                EmailAddress = "jan@mail.com",
                PhoneNumber = "478293204",
                Address = "Domowa 4, Warszawa",
                BirthDate = new DateTime(1999, 07, 21)
            },
            new AddressBookEntry()
            {
                Id = 2,
                FirstName = "Adam",
                EmailAddress = "adam@mail.com",
                PhoneNumber = "478293204",
                Address = "Domowa 4, Krakow",
                BirthDate = new DateTime(1999, 07, 21)
            }
        };

        public HomeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var addressBookEntriesDto = _mapper.Map<List<AddressBookEntryDto>>(_addressBook);
            return View();
        }

        public IActionResult Privacy()
        {
            var addressBookEntriesDto = _mapper.Map<List<AddressBookEntryDto>>(_addressBook);
            return View(addressBookEntriesDto);
        }

        // POST: /AddressBook/AddAddress
        [HttpPost]
        public IActionResult AddAddress(AddressBookEntryDto entryDto)
        {
            if (ModelState.IsValid)
            {
                var entry = _mapper.Map<AddressBookEntry>(entryDto);
                entry.Id = _addressBook.Count + 1;
                _addressBook.Add(entry);
                return RedirectToAction("Index");
            }
            return View("Index", entryDto);
        }


        // GET: /AddressBook/GetLastAddress
        [HttpGet]
        public IActionResult GetLastAddress()
        {
            if (_addressBook.Count > 0)
            {
                var lastEntry = _addressBook.Last();
                var lastEntryDto = _mapper.Map<AddressBookEntryDto>(lastEntry);
                return Ok(lastEntryDto);
            }
            return NotFound();
        }

        // GET: /AddressBook/GetAddressesByCity/{city}
        [HttpGet("{city}")]
        public IActionResult GetAddressesByCity(string city)
        {
            var entriesInCity = _addressBook
                .Where(entry => entry.Address.Contains(city, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (entriesInCity.Count > 0)
            {
                var entriesInCityDto = _mapper.Map<List<AddressBookEntryDto>>(entriesInCity);
                return Ok(entriesInCityDto);
            }
            return NotFound();
        }
    }
}
