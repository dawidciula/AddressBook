using Address_Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace Address_Book.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<AddressBookEntry> _addressBook = new List<AddressBookEntry>();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // POST: /AddressBook/AddAddress
        [HttpPost]
        public IActionResult AddAddress(AddressBookEntry entry)
        {
            if (ModelState.IsValid)
            {
                entry.Id = _addressBook.Count + 1;
                _addressBook.Add(entry);
                return RedirectToAction("Index");
            }
            return View("Index", entry);
        }

        // GET: /AddressBook/GetLastAddress
        [HttpGet]
        public IActionResult GetLastAddress()
        {
            if (_addressBook.Count > 0)
            {
                var lastAddress = _addressBook[_addressBook.Count - 1];
                return Ok(lastAddress);
            }
            return NotFound();
        }

        // GET: /AddressBook/GetAddressesByCity/{city}
        [HttpGet("{city}")]
        public IActionResult GetAddressesByCity(string city)
        {
            var addressesInCity = new List<AddressBookEntry>();
            foreach (var entry in _addressBook)
            {
                if (entry.Address.Contains(city, StringComparison.OrdinalIgnoreCase))
                {
                    addressesInCity.Add(entry);
                }
            }

            if (addressesInCity.Count > 0)
            {
                return Ok(addressesInCity);
            }
            return NotFound();
        }
    }
}