using System.Collections.Generic;
using Orchard.Environment.Extensions;
using Orchard.Events;

namespace Contrib.ImportExportBindings.ImportExport
{
    public interface ICustomExportStep : IEventHandler
    {
        void Register(IList<string> steps);
    }

    [OrchardFeature("Contrib.ImportExportBindings")]
    public class BindingsCustomExportStep : ICustomExportStep
    {
        public const string ExportStep = "MemberBindings";

        public void Register(IList<string> steps)
        {
            steps.Add(ExportStep);
        }
    }
}