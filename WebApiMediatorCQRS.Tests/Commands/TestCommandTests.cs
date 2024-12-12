using FluentValidation.TestHelper;
using WebApiMediatorCQRS.Commands;
using Xunit;

namespace WebApiMediatorCQRS.Tests.Commands;

public class TestCommandTests
{
    [Fact]
    public void TestCommandValidator_Should_Have_Error_When_Message_Is_Empty()
    {
        var validator = new TestCommandValidator();
        var command = new TestCommand { Message = string.Empty };
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Message);
    }

    [Fact]
    public void TestCommandValidator_Should_Not_Have_Error_When_Message_Is_Valid()
    {
        var validator = new TestCommandValidator();
        var command = new TestCommand { Message = "Valid Message" };
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Message);
    }

    [Fact]
    public async Task TestCommandHandler_Should_Return_Response_With_Same_Message()
    {
        var handler = new TestCommandHandler();
        var command = new TestCommand { Message = "Test Message" };
        var response = await handler.Handle(command, CancellationToken.None);
        Assert.Equal(command.Message, response.Message);
    }
}
