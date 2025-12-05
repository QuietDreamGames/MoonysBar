namespace Features.Factory.Interfaces
{
    public interface IObject
    {
        IModel Model { get; set; }
        IView View { get; set; }
        IPresenter Presenter { get; set; }

        void Initialize();
    }
}
