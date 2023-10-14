using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Trees;
using Umbraco.Cms.Web.BackOffice.Trees;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.ModelBinders;

namespace UmbracoConfigurationViewer.BackOffice.Trees
{
    [Tree(Constants.Applications.Settings, "configurationViewer", IsSingleNodeTree = true, TreeGroup = Constants.Trees.Groups.Settings, TreeTitle = "Configuration Viewer", SortOrder = 10)]
    [PluginController("UmbracoConfigurationViewer")]
    public class ConfigurationViewerTreeController : TreeController
    {
        private readonly IMenuItemCollectionFactory _menuItemCollectionFactory;

        public ConfigurationViewerTreeController(
            IMenuItemCollectionFactory menuItemCollectionFactory,
            ILocalizedTextService localizedTextService,
            UmbracoApiControllerTypeCollection umbracoApiControllerTypeCollection,
            IEventAggregator eventAggregator) :
            base(localizedTextService, umbracoApiControllerTypeCollection, eventAggregator)
        {
            _menuItemCollectionFactory = menuItemCollectionFactory;
        }

        protected override ActionResult<MenuItemCollection> GetMenuForNode(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormCollection queryStrings) =>
            _menuItemCollectionFactory.Create();

        protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormCollection queryStrings) =>
            new TreeNodeCollection();

        protected override ActionResult<TreeNode?> CreateRootNode(FormCollection queryStrings)
        {
            var rootResult = base.CreateRootNode(queryStrings);
            if (rootResult.Result is not null)
            {
                return rootResult;
            }

            var root = rootResult.Value;
            if (root is not null)
            {
                root.RoutePath = $"{Constants.Applications.Settings}/configurationViewer/overview";
                root.Icon = "icon-settings-alt";
                root.HasChildren = false;
                root.MenuUrl = null;
            }

            return root;
        }
    }
}
