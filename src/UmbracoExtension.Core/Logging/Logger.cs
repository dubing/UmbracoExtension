using System;
using UmbracoExtension.Core.Logging;
using log4net;
using Microsoft.Practices.Unity;

namespace UmbracoExtension.Core.Logging
{
    /// <summary>
    /// Instead of using log4net directly, this class could works even log4net will not be used in the future.
    /// </summary>
    public class Logger : ILogger
    {
        #region Private Members

        private readonly ILog _log;

        #endregion

        #region Constructors

        [InjectionConstructor]
        public Logger(ILog log)
        {
            _log = log;

            //IMPORTANT! load log4net configuration from xml file
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        #region Implementation of ILogger

        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        #endregion
    }
}
