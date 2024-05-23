using Address_Book.Models;
using Microsoft.EntityFrameworkCore;

namespace Address_Book.Data;

public class AddressBookDbContext : DbContext
{
    public AddressBookDbContext(DbContextOptions<AddressBookDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<AddressBookEntry> AddressBookEntries { get; set; }
}