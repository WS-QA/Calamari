namespace Calamari
{
    public class EnvironmentVariable
    {
        public EnvironmentVariable(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public string Value { get; }
    }
}