using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoConfigurationViewer.BackOffice.Models;

namespace UmbracoConfigurationViewer.BackOffice.Controllers
{
    [PluginController("UmbracoConfigurationViewer")]
    [Authorize(Policy = AuthorizationPolicies.SectionAccessSettings)]
    public class ConfigurationViewerController : UmbracoAuthorizedApiController
    {
        private readonly IConfiguration _configuration;

        public ConfigurationViewerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Config()
        {
            return Ok(_configuration.GetChildren().SelectMany(Map));
        }

        private IEnumerable<ConfigurationNodeModel> Map(IConfigurationSection configurationSection) => Map(configurationSection, Enumerable.Empty<IConfigurationSection>());

        private IEnumerable<ConfigurationNodeModel> Map(IConfigurationSection configurationSection, IEnumerable<IConfigurationSection> parents)
        {
            var children = configurationSection.GetChildren();

            yield return new ConfigurationNodeModel
            {
                Key = configurationSection.Key,
                Path = configurationSection.Path,
                Value = configurationSection.Value,
                Parents = parents.Select(x => x.Key),
                HasChildren = children.Any()
            };

            foreach (var child in children)
            {
                foreach (var mapped in Map(child, parents.Append(configurationSection)))
                {
                    yield return mapped;
                }
            }
        }
    }
}
