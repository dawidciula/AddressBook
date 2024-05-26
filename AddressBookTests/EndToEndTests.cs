using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AddressBookTests
{
    public class EndToEndTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl;

        public EndToEndTests()
        {
            _driver = new ChromeDriver();
            _baseUrl = "http://localhost:5018";
        }

        [Fact]
        public void AddAddress_FormSubmitsSuccessfully()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
            
            var firstNameField = _driver.FindElement(By.Name("FirstName"));
            var emailAddressField = _driver.FindElement(By.Name("EmailAddress"));
            var phoneNumberField = _driver.FindElement(By.Name("PhoneNumber"));
            var cityField = _driver.FindElement(By.Name("City"));
            var birthDateField = _driver.FindElement(By.Name("BirthDate"));
            var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
            
            firstNameField.SendKeys("John");
            emailAddressField.SendKeys("john@example.com");
            phoneNumberField.SendKeys("123456789");
            cityField.SendKeys("New York");
            birthDateField.SendKeys("1990-01-01, 00:00");
            
            submitButton.Click();
            
            System.Threading.Thread.Sleep(2000);
            
            Assert.Contains("Welcome to your Address book!", _driver.PageSource);
        }


        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
