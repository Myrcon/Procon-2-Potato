﻿using System;
using NUnit.Framework;

namespace Procon.Service.Shared.Test {
    [TestFixture]
    public class TestServiceObserver {
        /// <summary>
        /// Tests the initial values are set when instantiating a new observer.
        /// </summary>
        [Test]
        public void TestInitalValues() {
            var observer = new ServiceObserver();

            Assert.AreEqual(ServiceStatusType.Stopped, observer.Status);

            // Test that we are "down"
            Assert.GreaterOrEqual(observer.StopTime, DateTime.Now.AddSeconds(-5));
            Assert.GreaterOrEqual(observer.Downtime(), TimeSpan.FromSeconds(0));

            // Test that we are not "up"
            Assert.IsNull(observer.StartTime);
            Assert.AreEqual(new TimeSpan(0), observer.Uptime());
        }

        /// <summary>
        /// Test that setting the status to started will show correct uptime/downtime.
        /// </summary>
        [Test]
        public void TestStatusStarted() {
            var observer = new ServiceObserver {
                Status = ServiceStatusType.Started
            };

            Assert.AreEqual(ServiceStatusType.Started, observer.Status);

            // Test that we are not "down"
            Assert.IsNotNull(observer.StopTime);
            Assert.AreEqual(new TimeSpan(0), observer.Downtime());

            // Test that we are "up"
            Assert.GreaterOrEqual(observer.StartTime, DateTime.Now.AddSeconds(-5));
            Assert.GreaterOrEqual(observer.Uptime(), TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Tests that if the downtime has not exceeded fifteen minutes the panic callback will not be called.
        /// </summary>
        [Test]
        public void TestPanicNotInitiatedUnderFifteenMinutes() {
            var paniced = false;
            var observer = new ServiceObserver() {
                Panic = () => { paniced = true; }
            };

            // We've only been down for less than a second so it shouldn't fire.
            observer.PanicTask_Tick(null);

            Assert.IsFalse(paniced);
        }

        /// <summary>
        /// Tests that if the downtime has exceeded fifteen minutes the panic callback will be called.
        /// </summary>
        [Test]
        public void TestPanicDowntimeOverFifteenMinutes() {
            var paniced = false;
            var observer = new ServiceObserver() {
                Panic = () => { paniced = true; },
                StopTime = DateTime.Now.AddMinutes(-16)
            };

            // We've only been down for less than a second so it shouldn't fire.
            observer.PanicTask_Tick(null);

            Assert.IsTrue(paniced);
        }
    }
}