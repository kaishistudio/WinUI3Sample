namespace WinUI3Sample.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
