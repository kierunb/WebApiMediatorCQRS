using FluentValidation;
using MediatR;

namespace WebApiMediatorCQRS.Commands;

public record NewCommand : IRequest<NewCommandResponse>
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class NewCommandValidator : AbstractValidator<NewCommand>
{
    public NewCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty().Must(IsValidMessage);
    }

    private bool IsValidMessage(string message) =>
        !string.IsNullOrWhiteSpace(message) && message.Length >= 3;
}

public record NewCommandResponse(string Message);

public class NewCommandHandler : IRequestHandler<NewCommand, NewCommandResponse>
{
    public Task<NewCommandResponse> Handle(
        NewCommand request,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new NewCommandResponse(request.Message));
    }
}
