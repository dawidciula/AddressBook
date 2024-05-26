using Address_Book.Controllers;
using Address_Book.Models;
using Address_Book.ViewModels;
using Address_Book.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class HomeControllerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<HomeController>>();
        var mockContext = new Mock<AddressBookDbContext>();
        _controller = new HomeController(_mockMapper.Object, mockContext.Object, _mockLogger.Object);
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        var result = _controller.Index();
        
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void ContactsList_ReturnsViewResult()
    {
        _mockMapper.Setup(m => m.Map<List<AddressBookEntryDto>>(It.IsAny<List<AddressBookEntry>>()))
            .Returns(new List<AddressBookEntryDto>());
        
        var result = _controller.ContactsList();
        
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task AddAddress_ValidModel_RedirectsToIndex()
    {
        var entryDto = new AddressBookEntryDto();
        
        var result = await _controller.AddAddress(entryDto);
        
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task GetLastAddress_ReturnsOkObjectResult()
    {
        _mockMapper.Setup(m => m.Map<AddressBookEntryDto>(It.IsAny<AddressBookEntry>()))
            .Returns(new AddressBookEntryDto());
        
        var result = await _controller.GetLastAddress();
        
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAddressesByCity_ReturnsOkObjectResult()
    {
        var city = "TestCity";
        _mockMapper.Setup(m => m.Map<List<AddressBookEntryDto>>(It.IsAny<List<AddressBookEntry>>()))
            .Returns(new List<AddressBookEntryDto>());
        
        var result = await _controller.GetAddressesByCity(city);
        
        Assert.IsType<OkObjectResult>(result);
    }
}
