﻿namespace Ninject.Extensions.Logging.Tests.Infrastructure
{
    using System;
    using Ninject.Extensions.Logging.Tests.Classes;
    using Ninject.Modules;
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
    using MsTest.Should;
    using Assert = Ninject.SilverlightTests.AssertWithThrows;
    using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven.Should;
    using Assert = Ninject.SilverlightTests.AssertWithThrows;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Xunit;
    using Xunit.Should;
#endif

    public abstract class CommonTests : ILoggingTestContext
    {
        [Fact]
        public void PublicLoggerPropertyIsInjected()
        {
            using (var kernel = this.CreateKernel())
            {
                var loggerClass = kernel.Get<PublicPropertyLoggerClass>();
                loggerClass.Logger.ShouldNotBeNull();
                loggerClass.Logger.Type.ShouldBe(typeof(PublicPropertyLoggerClass));
                loggerClass.Logger.GetType().ShouldBe(this.LoggerType);
            }
        }

        [Fact]
        public void NonPublicLoggerPropertyIsNotInjected()
        {
            using (var kernel = this.CreateKernel())
            {
                var loggerClass = kernel.Get<NonPublicPropertyLoggerClass>();
                loggerClass.Logger.ShouldBeNull();
            }
        }

        [Fact]
        public void CtorLoggerPropertyIsInjected()
        {
            using (var kernel = this.CreateKernel())
            {
                var loggerClass = kernel.Get<CtorPropertyLoggerClass>();
                loggerClass.Logger.ShouldNotBeNull();
                loggerClass.Logger.Type.ShouldBe(typeof(CtorPropertyLoggerClass));
                loggerClass.Logger.GetType().ShouldBe(this.LoggerType);
            }
        }

        [Fact]
        public void ObjectCanGetsItsOwnLogger()
        {
            using (var kernel = this.CreateKernel())
            {
                var loggerClass = kernel.Get<ObjectGetsItsOwnLogger>();
                loggerClass.Logger.ShouldNotBeNull();
                loggerClass.Logger.Type.ShouldBe(typeof(ObjectGetsItsOwnLogger));
                loggerClass.Logger.GetType().ShouldBe(this.LoggerType);
            }
        }

#if !SILVERLIGHT && !NETCF
        [Fact]
        public void ObjectCanGetsItsOwnLoggerUsingGetCurrentClassLogger()
        {
            using (var kernel = this.CreateKernel())
            {
                var loggerClass = kernel.Get<ObjectGetsItsOwnLoggerUsingCurrentClassMethod>();
                loggerClass.Logger.ShouldNotBeNull();
                loggerClass.Logger.Type.ShouldBe(typeof(ObjectGetsItsOwnLoggerUsingCurrentClassMethod));
                loggerClass.Logger.GetType().ShouldBe(this.LoggerType);
            }
        }
#endif

        protected virtual IKernel CreateKernel()
        {
            var settings = this.CreateSettings();
            return new StandardKernel(settings, this.TestModules);
        }

        protected virtual INinjectSettings CreateSettings()
        {
#if SILVERLIGHT && !NETCF
            return new NinjectSettings();
#else
            return new NinjectSettings { LoadExtensions = false };
#endif
        }

        public abstract INinjectModule[] TestModules { get; }
        
        public abstract Type LoggerType { get; }
    }
}