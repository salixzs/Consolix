namespace Consolix
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConsoleOptionAttribute : Attribute
    {
        public ConsoleOptionAttribute(string name, string description, string shortName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Option name should be specified.", nameof(name));
            }

            if (name.Contains(" "))
            {
                throw new ArgumentException("Option name should be a single word.", nameof(name));
            }

            if (name.Length > 12)
            {
                throw new ArgumentException("Option name should have no more than 12 letters.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description/Help should be specified.", nameof(description));
            }

            this.Name = name;
            this.Description = description;
            this.ShortName = shortName;
        }

        public string Name { get; }
        public string ShortName { get; }
        public string Description { get; }
    }
}
