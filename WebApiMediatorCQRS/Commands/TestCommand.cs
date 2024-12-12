using FluentValidation;
using MediatR;

namespace WebApiMediatorCQRS.Commands;

public record TestCommand : IRequest<TestCommandResponse>
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class TestCommandValidator : AbstractValidator<TestCommand>
{
    public TestCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty().Must(IsValidMessage);
    }

    private bool IsValidMessage(string message) =>
        !string.IsNullOrWhiteSpace(message) && message.Length >= 3;
}

public record TestCommandResponse(string Message);

public class TestCommandHandler : IRequestHandler<TestCommand, TestCommandResponse>
{
    public Task<TestCommandResponse> Handle(
        TestCommand request,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new TestCommandResponse(request.Message));
    }
}
