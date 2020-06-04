using System;

namespace Calamari.Integration.FileSystem
{
    public interface IFileSystemInfo
    {
        string Name { get; }
        string FullName { get; }
        string Extension { get; }
        DateTime LastAccessTimeUtc { get; }
        DateTime LastWriteTimeUtc { get; }
        bool IsDirectory { get; }
    }
}