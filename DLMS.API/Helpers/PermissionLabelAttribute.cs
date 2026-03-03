namespace DLMS.API.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionLabelAttribute : Attribute
    {
        public string Label { get; }

        public PermissionLabelAttribute(string label)
        {
            Label = label;
        }
    }
}

