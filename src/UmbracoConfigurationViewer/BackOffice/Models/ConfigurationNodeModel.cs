using System.Collections.Generic;
using System.Linq;

namespace UmbracoConfigurationViewer.BackOffice.Models
{
    public class ConfigurationNodeModel
    {
        public string Key { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string? Value { get; set; }
        public IEnumerable<string> Parents { get; set; } = Enumerable.Empty<string>();
        public bool HasChildren { get; set; }
    }
}
