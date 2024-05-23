using Address_Book.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Address_Book.Data
{
    public class AddressBookDbContextInitializer
    {
        public static void Initialize(AddressBookDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.AddressBookEntries.Any())
            {
                return;
            }

            var addressBookEntries = new AddressBookEntry[]
            {
                new AddressBookEntry
                {
                    Id = 1,
                    FirstName = "Jan",
                    EmailAddress = "jan@mail.com",
                    PhoneNumber = "478293204",
                    City = "Warszawa",
                    BirthDate = new DateTime(1999, 07, 21)
                },
                new AddressBookEntry
                {
                    Id = 2,
                    FirstName = "Adam",
                    EmailAddress = "adam@mail.com",
                    PhoneNumber = "478293204",
                    City = "Krakow",
                    BirthDate = new DateTime(1999, 07, 21)
                }
            };

            context.AddressBookEntries.AddRange(addressBookEntries);
            context.SaveChanges();
        }
    }
}