namespace WebApiMediatorCQRS.ApiModels;

public record NewRequest(Guid Id, string Message);
public record NewResponse(string Message);
