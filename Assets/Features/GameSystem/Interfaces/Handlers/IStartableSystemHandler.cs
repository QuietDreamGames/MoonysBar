using Features.System.Interfaces;

namespace Features.GameSystem.Interfaces.Handlers
{
    public interface IStartableSystemHandler : ISystem
    {
        void Initialize();
    }
}