using InsecureProject.Controllers;
using InsecureProject.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace ServerTests;

public class WeatherForecastControllerTests
{
    private WeatherForecastController _sut;
    private Mock<DbModelContext> _dbContext;
    private Mock<ILogger<WeatherForecastController>> _logger;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = new Mock<DbModelContext>();
        _logger = new Mock<ILogger<WeatherForecastController>>();
        _sut = new WeatherForecastController(_dbContext.Object, _logger.Object);
    }

    [Test]
    public void Get_Nominal()
    {
        // Arrange
        _dbContext.Setup<DbSet<Forecast>>(x => x.Forecasts)
            .ReturnsDbSet(new Forecast[]
            {
                new()
                {
                    Temperature = 67,
                    City = "Honolulu",
                    Summary = "Rainy"
                },
                new()
                {
                    Temperature = 22,
                    City = "Baltimore",
                    Summary = "Sleet"
                }
            });
        
        // Act
        var results = _sut.Get("honolulu").ToList();
        
        // Assert
        Assert.That(results.Count, Is.EqualTo(1));
        Assert.That(results[0].TemperatureC, Is.EqualTo(67));
    }
}