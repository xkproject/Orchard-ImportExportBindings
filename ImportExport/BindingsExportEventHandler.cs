using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Orchard.Environment.Extensions;
using Orchard.Events;
using Orchard.Projections.Services;

namespace Contrib.ImportExportBindings.ImportExport
{
    public interface IExportEventHandler : IEventHandler
    {
        void Exporting(dynamic context);
        void Exported(dynamic context);
    }

    [OrchardFeature("Contrib.ImportExportBindings")]
    public class BindingsExportHandler : IExportEventHandler
    {
        private readonly IMemberBindingProvider _memberBindingProvider;

        public BindingsExportHandler(IMemberBindingProvider memberBindingProvider)
        {
            _memberBindingProvider = memberBindingProvider;
        }

        public void Exporting(dynamic context)
        {
        }

        public void Exported(dynamic context)
        {
            if (!((IEnumerable<string>)context.ExportOptions.CustomSteps).Contains(BindingsCustomExportStep.ExportStep))
            {
                return;
            }

            var memberBindings = new XElement(BindingsCustomExportStep.ExportStep);
            context.Document.Element("Orchard").Add(memberBindings);

            var bindingBuilder = new BindingBuilder();
            _memberBindingProvider.GetMemberBindings(bindingBuilder);

            foreach (var bindingItem in bindingBuilder.Build())
            {
                var declaringType = bindingItem.Property.DeclaringType;
                Debug.Assert(declaringType != null, "declaringType != null");

                var memberBinding = new XElement("MemberBinding",
                    new XAttribute("Type", declaringType.FullName),
                    new XAttribute("Member", bindingItem.Property.Name),
                    new XAttribute("Description", bindingItem.Description),
                    new XAttribute("DisplayName", bindingItem.DisplayName));

                memberBindings.Add(memberBinding);
            }
        }
    }
}

