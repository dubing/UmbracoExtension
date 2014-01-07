using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AKQA.Common.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using UnityLog4NetExtension.Log4Net;


namespace UmbracoExtension.Core.Unity
{
    /// <summary>
    /// This class is used to make sure everyone use one container
    /// </summary>
    public static class UnityHelper
    {

        #region Public Properties

        public static IUnityContainer Container;

        #endregion

        #region Constructors

        static UnityHelper()
        {
            Container = new UnityContainer();
            Container.AddNewExtension<LifetimeContainerExtension>();
            Container.AddNewExtension<Log4NetExtension>();
            Container.LoadConfiguration();

        }

        #endregion

    }
}
