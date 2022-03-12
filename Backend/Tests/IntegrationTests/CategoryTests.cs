namespace IntegrationTests
{
    [TestClass]
    public class CategoryTests : IntegrationTest
    {
        [TestMethod]
        public async Task Crud()
        {
            var categorySvc = Container.Resolve<ICategoryAppService>();

            using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

            var symbol1 = await AddSymbolAsync(HostOrganizationId);
            var symbol2 = await AddSymbolAsync(HostOrganizationId);
            var symbol3 = await AddSymbolAsync(HostOrganizationId);
            var symbol4 = await AddSymbolAsync(HostOrganizationId);

            // Create
            var tree = new TreeInputDto();
            tree.Children.Add(new TreeInputDto());
            tree.Children[0].Children.Add(new TreeInputDto());
            tree.Children[0].Children.Add(new TreeInputDto());

            tree.Children[0].Category = new CategoryInputDto(new CategoryId(), "category1")
            {
                SymbolId = symbol1,
                Color = "#000000",
                IsActive = true
            };

            tree.Children[0].Children[0].Category = new CategoryInputDto(new CategoryId(), "category2")
            {
                SymbolId = null,
                Color = null,
                IsActive = true
            };
            tree.Children[0].Children[1].Category = new CategoryInputDto(new CategoryId(), "category3")
            {
                SymbolId = null,
                Color = null,
                IsActive = true
            };

            await categorySvc.SetCategoryTreeAsync(HostOrganizationId, tree);

            // Get
            var actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.ActiveOnly);
            AssertTreeEqual(tree, actualTree);
            tree = ConvertTree(actualTree);

            // Add parent
            var cat1 = tree.Children[0].Category!;
            cat1.SymbolId = null;
            cat1.Color = null;
            var cat1Children = tree.Children[0].Children;

            var cat4Id = new CategoryId();
            tree.Children[0].Category = new CategoryInputDto(cat4Id, "category4")
            {
                SymbolId = symbol2,
                Color = "#FFFFFF",
                IsActive = true
            };

            tree.Children[0].Children = new List<TreeInputDto>
            {
                new TreeInputDto()
            };
            tree.Children[0].Children[0].Category = cat1;
            tree.Children[0].Children[0].Children = cat1Children;
            await categorySvc.SetCategoryTreeAsync(HostOrganizationId, tree);

            actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.ActiveOnly);
            AssertTreeEqual(tree, actualTree, checkIds: false);
            tree = ConvertTree(actualTree);

            // Move indices
            var cat2 = tree.Children[0].Children[0].Children[0];
            tree.Children[0].Children[0].Children.RemoveAt(0);
            tree.Children[0].Children[0].Children.Add(cat2);

            await categorySvc.SetCategoryTreeAsync(HostOrganizationId, tree);

            actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.ActiveOnly);
            AssertTreeEqual(tree, actualTree);

            // Update properties
            tree.Children[0].Category!.Name = "category4-1";
            tree.Children[0].Category!.SymbolId = symbol3;
            tree.Children[0].Category!.Color = "#CCCCCC";
            await categorySvc.SetCategoryTreeAsync(HostOrganizationId, tree);

            actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.ActiveOnly);
            AssertTreeEqual(tree, actualTree);

            // Move child to root
            tree.Children[0].Children[0].Children.RemoveAt(1);
            tree.Children.Add(cat2);
            cat2.Category!.SymbolId = symbol4;
            cat2.Category.Color = "#AAAAAA";
            await categorySvc.SetCategoryTreeAsync(HostOrganizationId, tree);

            actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.ActiveOnly);
            AssertTreeEqual(tree, actualTree);

            // Deactivate
            tree.Children[0].Children[0].Category!.IsActive = false;
            tree.Children[0].Children[0].Children[0].Category!.IsActive = false;
            await categorySvc.SetCategoryTreeAsync(HostOrganizationId, tree);

            actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.All);
            AssertTreeEqual(tree, actualTree);

            tree.Children[0].Children = new List<TreeInputDto>();
            actualTree = await categorySvc.GetCategoryTreeAsync(HostOrganizationId, ActiveFilter.ActiveOnly);
            AssertTreeEqual(tree, actualTree);
        }

        private TreeInputDto ConvertTree(TreeDto tree)
        {
            var treeInput = new TreeInputDto();
            if (tree.Category != null)
            {
                treeInput.Category = new CategoryInputDto(tree.Category.Id, tree.Category.Name)
                {
                    SymbolId = tree.Category.SymbolId,
                    Color = tree.Category.Color,
                    IsActive = tree.Category.IsActive
                };
            }

            foreach (var subTree in tree.Children) treeInput.Children.Add(ConvertTree(subTree));

            return treeInput;
        }

        private void AssertTreeEqual(TreeInputDto expectedTree, TreeDto actualTree, bool checkIds = true)
        {
            // Category
            if (checkIds) Assert.AreEqual(expectedTree.Category?.Id.Guid, actualTree.Category?.Id.Guid);
            Assert.AreEqual(expectedTree.Category?.Name, actualTree.Category?.Name);
            Assert.AreEqual(expectedTree.Category?.SymbolId?.Guid, actualTree.Category?.SymbolId?.Guid);
            Assert.AreEqual(expectedTree.Category?.Color, actualTree.Category?.Color);
            Assert.AreEqual(expectedTree.Category?.IsActive, actualTree.Category?.IsActive);

            // Children
            Assert.AreEqual(expectedTree.Children.Count, actualTree.Children.Count);
            for (var i = 0; i < expectedTree.Children.Count; i++)
            {
                AssertTreeEqual(expectedTree.Children[i], actualTree.Children[i], checkIds);
            }
        }
    }
}
