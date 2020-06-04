﻿using System;
using System.IO;

namespace Calamari.Integration.FileSystem
{
    public class TemporaryDirectory : IDisposable
    {
        public readonly string DirectoryPath;
        readonly ICalamariFileSystem fileSystem = CalamariPhysicalFileSystem.GetPhysicalFileSystem();

        public TemporaryDirectory(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        public void Dispose()
        {
            fileSystem.DeleteDirectory(DirectoryPath, FailureOptions.IgnoreFailure);
        }

        public static TemporaryDirectory Create()
        {
            var dir = Path.Combine(Path.GetTempPath(), "Test_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return new TemporaryDirectory(dir);
        }
    }
}