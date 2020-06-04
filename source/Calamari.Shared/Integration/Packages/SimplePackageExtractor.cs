namespace Calamari.Integration.Packages
{
    public abstract class SimplePackageExtractor : IPackageExtractor
    {
        public abstract int Extract(string packageFile, string directory, bool suppressNestedScriptWarning);
        public abstract string[] Extensions { get; }
    }
}