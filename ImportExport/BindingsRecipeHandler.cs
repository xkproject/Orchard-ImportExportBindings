using System;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Logging;
using Orchard.Projections.Models;
using Orchard.Recipes.Models;
using Orchard.Recipes.Services;

namespace Contrib.ImportExportBindings.ImportExport
{
    [OrchardFeature("Contrib.ImportExportBindings")]
    public class BindingsRecipeHandler : IRecipeHandler
    {
        private readonly IRepository<MemberBindingRecord> _repository;

        public BindingsRecipeHandler(IRepository<MemberBindingRecord> repository)
        {
            _repository = repository;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void ExecuteRecipeStep(RecipeContext recipeContext)
        {
            if (!String.Equals(recipeContext.RecipeStep.Name, BindingsCustomExportStep.ExportStep, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var stepElement = recipeContext.RecipeStep.Step;

            foreach (var memberBindingElement in stepElement.Elements("MemberBinding"))
            {
                _repository.Create(new MemberBindingRecord
                {
                    Member = memberBindingElement.Attribute("Member").Value,
                    Type = memberBindingElement.Attribute("Type").Value,
                    DisplayName = memberBindingElement.Attribute("DisplayName").Value,
                    Description = memberBindingElement.Attribute("Description").Value,
                });
            }

            recipeContext.Executed = true;
        }
    }
}
