using Address_Book.Models;
using Address_Book.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Address_Book.Data;
using Microsoft.EntityFrameworkCore;

namespace Address_Book.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly AddressBookDbContext _context;
        
        private readonly List<AddressBookEntry> _addressBook = new List<AddressBookEntry>();

        public HomeController(IMapper mapper, AddressBookDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public IActionResult Index()
        {
            var addressBookEntriesDto = _mapper.Map<List<AddressBookEntryDto>>(_addressBook);
            return View();
        }

        public IActionResult Privacy()
        {
            var addressBookEntries = _context.AddressBookEntries.ToList();
            var addressBookEntriesDto = _mapper.Map<List<AddressBookEntryDto>>(addressBookEntries);
            return View(addressBookEntriesDto);
        }

        // POST: /Home/AddAddress
        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressBookEntryDto entryDto)
        {
            if (ModelState.IsValid)
            {
                var entry = _mapper.Map<AddressBookEntry>(entryDto);
                _context.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Index", entryDto);
        }



        // GET: /Home/GetLastAddress
        [HttpGet("Home/GetLastAddress")]
        public async Task<IActionResult> GetLastAddress()
        {
            var lastEntry = await _context.AddressBookEntries.LastOrDefaultAsync();
            if (lastEntry != null)
            {
                var lastEntryDto = _mapper.Map<AddressBookEntryDto>(lastEntry);
                return Ok(lastEntryDto);
            }
            return NotFound();
        }

        // GET: /Home/GetAddressesByCity/{city}
        [HttpGet("Home/GetAddressesByCity/{city}")]
        public async Task<IActionResult> GetAddressesByCity(string city)
        {
            var entriesInCity = await _context.AddressBookEntries
                .Where(entry => entry.City.Equals(city, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (entriesInCity.Count > 0)
            {
                var entriesInCityDto = _mapper.Map<List<AddressBookEntryDto>>(entriesInCity);
                return Ok(entriesInCityDto);
            }
            return NotFound();
        }

    }
}
