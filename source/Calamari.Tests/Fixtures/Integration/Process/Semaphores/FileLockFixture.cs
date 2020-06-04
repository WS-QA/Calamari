﻿using System;
using System.Threading;
using Calamari.Integration.Processes.Semaphores;
using NUnit.Framework;

namespace Calamari.Tests.Fixtures.Integration.Process.Semaphores
{
    [TestFixture]
    public class FileLockFixture
    {
        [Test]
        [TestCase(123, 456, "ProcessName", 123, 456, "ProcessName", true)]
        [TestCase(123, 456, "ProcessName", 789, 456, "ProcessName", false)]
        [TestCase(123, 456, "ProcessName", 123, 789, "ProcessName", false)]
        [TestCase(123, 456, "ProcessName", 123, 456, "DiffProcessName", false)]
        [TestCase(123, 456, "ProcessName", 789, 246, "ProcessName", false)]
        [TestCase(123, 456, "ProcessName", 123, 246, "DiffProcessName", false)]
        [TestCase(123, 456, "ProcessName", 789, 456, "ProcessName", false)]
        [TestCase(123, 456, "ProcessName", 789, 246, "DiffProcessName", false)]
        public void EqualsComparesCorrectly(int processIdA, int threadIdA, string processNameA, int processIdB, int threadIdB, string processNameB, bool expectedResult)
        {
            var objectA = new FileLock {  ProcessId = processIdA, ProcessName = processNameA, ThreadId = threadIdA, Timestamp = DateTime.Now.Ticks };
            var objectB = new FileLock {  ProcessId = processIdB, ProcessName = processNameB, ThreadId = threadIdB, Timestamp = DateTime.Now.Ticks };
            Assert.That(Equals(objectA, objectB), Is.EqualTo(expectedResult));
        }

        [Test]
        public void EqualsIgnoresTimestamp()
        {
            var objectA = new FileLock { ProcessId = 123, ProcessName = "ProcessName", ThreadId = 456, Timestamp = DateTime.Now.Ticks };
            var objectB = new FileLock { ProcessId = 123, ProcessName = "ProcessName", ThreadId = 456, Timestamp = DateTime.Now.Ticks + 5 };
            Assert.That(Equals(objectA, objectB), Is.True);
        }

        [Test]
        public void EqualsReturnsFalseIfOtherObjectIsNull()
        {
            var fileLock = new FileLock { ProcessId = 123, ProcessName = "ProcessName", ThreadId = 456, Timestamp = DateTime.Now.Ticks };
            Assert.That(fileLock.Equals(null), Is.False);
        }

        [Test]
        public void EqualsReturnsFalseIfOtherObjectIsDifferentType()
        {
            var fileLock = new FileLock { ProcessId = 123, ProcessName = "ProcessName", ThreadId = 456, Timestamp = DateTime.Now.Ticks };
            Assert.That(fileLock.Equals(new object()), Is.False);
        }

        [Test]
        public void EqualsReturnsTrueIfSameObject()
        {
            var fileLock = new FileLock { ProcessId = 123, ProcessName = "ProcessName", ThreadId = 456, Timestamp = DateTime.Now.Ticks };
            // ReSharper disable once EqualExpressionComparison
            Assert.That(fileLock.Equals(fileLock), Is.True);
        }

        [Test]
        public void BelongsToCurrentProcessAndThreadMatchesOnCurrentProcessAndThread()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var fileLock = new FileLock {
                ProcessId = currentProcess.Id,
                ProcessName = currentProcess.ProcessName,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Timestamp = DateTime.Now.Ticks
            };
            Assert.That(fileLock.BelongsToCurrentProcessAndThread(), Is.True);
        }

        [Test]
        public void BelongsToCurrentProcessAndThreadReturnsFalseIfIncorrectProcessId()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var fileLockWithIncorrectProcessId = new FileLock
            {
                ProcessId = -100,
                ProcessName = currentProcess.ProcessName,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Timestamp = DateTime.Now.Ticks
            };
            Assert.That(fileLockWithIncorrectProcessId.BelongsToCurrentProcessAndThread(), Is.False);
        }

        [Test]
        public void BelongsToCurrentProcessAndThreadReturnsFalseIfIncorrectThreadId()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var fileLockWithIncorrectThreadId = new FileLock
            {
                ProcessId = currentProcess.Id,
                ProcessName = currentProcess.ProcessName,
                ThreadId = -200,
                Timestamp = DateTime.Now.Ticks
            };
            Assert.That(fileLockWithIncorrectThreadId.BelongsToCurrentProcessAndThread(), Is.False);
        }

    }
}
