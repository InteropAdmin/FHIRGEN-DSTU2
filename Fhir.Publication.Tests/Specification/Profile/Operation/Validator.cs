using System;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubOperation = Hl7.Fhir.Publication.Specification.Profile.Operation;

namespace Fhir.Publication.Tests.Specification.Profile.Operation
{
    [TestClass]
    public class Validator
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Validator_IsValid_InvalidOperationExceptionThrownWhenDefinitionHasNoParameters()
        {
            var definition = new OperationDefinition();

            PubOperation.Validator.IsValid(definition);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Validator_IsValid_InvalidOperationExceptionThrownWhenUseIsNotSetInParameter()
        {
            var definition = new OperationDefinition();
            definition.Kind = OperationDefinition.OperationKind.Operation;

            definition.Parameter.Add(new OperationDefinition.ParameterComponent());

            PubOperation.Validator.IsValid(definition);
        }
    }
}