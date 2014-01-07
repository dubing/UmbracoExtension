using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace AKQA.Common.Unity
{
    public class LifetimeContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var strategy = new MyBuilderStrategy(Container);

            Context.Strategies.Add(strategy, UnityBuildStage.Creation);
        }

        class MyBuilderStrategy : BuilderStrategy
        {
            private readonly IUnityContainer _container;

            public MyBuilderStrategy(IUnityContainer container)
            {
                _container = container;
            }

            public override void PreBuildUp(IBuilderContext context)
            {
                context.PersistentPolicies.Set<ILifetimePolicy>(new ContainerControlledLifetimeManager(), context.BuildKey);
            }
        }
    }
}
