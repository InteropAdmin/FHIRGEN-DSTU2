using System;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.Operation
{
    internal class Factory
    {
        private readonly Log _log;
        private readonly ImplementationGuide.ResourceStore _resourceStore;
        private readonly KnowledgeProvider _knowledgeProvider;
        private readonly HierarchicalTable.Factory _factory;
        private readonly XElement _root;

        public Factory(
            ImplementationGuide.ResourceStore resourceStore, 
            Log log)
        {
            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            _log = log;
            _resourceStore = resourceStore;
            _knowledgeProvider = new KnowledgeProvider(_log);
            _factory = new HierarchicalTable.Factory(null);
            _root = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "operationDefinition-content"));
        }

        public XElement GenerateOperation(OperationDefinition operation)
        {
            _log.Debug(string.Concat("Creating operation ", operation.Name));

            if (Validator.IsValid(operation))
            {
                var meta = new Model.Meta(operation, _knowledgeProvider);

                 TableModel.Model inputParams = new Model.Params(operation.Parameter
                        .Where(
                            param =>
                                param.Use == OperationDefinition.OperationParameterUse.In),
                        _resourceStore,
                        _knowledgeProvider).Table;

                TableModel.Model outputParams = new Model.Params(operation.Parameter
                        .Where(
                            param =>
                                param.Use == OperationDefinition.OperationParameterUse.Out),
                         _resourceStore,
                         _knowledgeProvider).Table;

                _root.Add(HeadedPanel.ToHtml(
                    "Meta",
                    _factory
                        .CreateFrom(meta.Table)
                        .ToHtml()));

                _root.Add(HeadedPanel.ToHtml(
                    "Input Parameters",
                    _factory
                        .CreateFrom(inputParams)
                        .ToHtml()));

                _root.Add(HeadedPanel.ToHtml(
                    "Output Parameters",
                    _factory
                        .CreateFrom(outputParams)
                        .ToHtml()));

                return _root;
            }              

            return null;
        }
    }
}