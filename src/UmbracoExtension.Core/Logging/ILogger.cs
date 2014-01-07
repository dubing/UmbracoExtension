
namespace UmbracoExtension.Core.Logging
{
    public interface ILogger
    {
        void Debug(object message, System.Exception exception);
        void Debug(object message);

        void Error(object message, System.Exception exception);
        void Error(object message);

        void Info(object message, System.Exception exception);
        void Info(object message);
    }
}
