namespace Blog.Application.Tests.Users.Commands;

using System.Threading;
using System.Threading.Tasks;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IBlogDbContext> _mockDbContext;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _mockDbContext = new Mock<IBlogDbContext>();
        _handler = new CreateUserCommandHandler(_mockDbContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddUserAndReturnResponse()
    {
        // Arrange
        var request = new CreateUserCommand("Janak","janak@example.com","12345");

        var mockDbSet = new Mock<DbSet<User>>();
        _mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);

        _mockDbContext
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("User created successfully", response.Message);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.UserId);
        Assert.Equal("Janak", response.Data.Name);
        Assert.Equal("janak@example.com", response.Data.Email);
        
        mockDbSet.Verify(d => d.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
