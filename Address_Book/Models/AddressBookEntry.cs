using System.ComponentModel.DataAnnotations;
namespace Address_Book.Models;

public class AddressBookEntry
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Incorrect email format")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "Phone number must have exactly 9 digits")]
    public string PhoneNumber { get; set; }
    
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    
    [Required(ErrorMessage = "Birth date is required")]
    public DateTime BirthDate { get; set; }
}