namespace TestEventService.Services
{
    public interface IService
    {
        bool Initialized { get; }

        void Initialize();

        void Release();
    }
}