namespace Features.GameSystem.Interfaces.Handlers
{
    public interface IEndableSystemHandler : ISystem
    {
        void Terminate();
    }
}