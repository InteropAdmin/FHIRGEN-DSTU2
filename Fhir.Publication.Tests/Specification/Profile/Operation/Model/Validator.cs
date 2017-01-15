using System;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubValidator = Hl7.Fhir.Publication.Specification.Profile.Operation.Model;

namespace Fhir.Publication.Tests.Specification.Profile.Operation.Model
{
    [TestClass]
    public class Validator
    {
        private OperationDefinition _operationDefinition;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validator_Validate_ArgumentExceptionThrownWhenOperationDefinitionNameIsNull()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = null;

            PubValidator.Validator.Validate(_operationDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validator_Validate_ArgumentExceptionThrownWhenOperationDefinitionNameIsEmpty()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = string.Empty;

            PubValidator.Validator.Validate(_operationDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validator_Validate_ArgumentNullExceptionThrownWhenOperationDefinitionKindIsNull()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = "myOperation";
            _operationDefinition.Kind = null;

            PubValidator.Validator.Validate(_operationDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validator_Validate_ArgumentExceptionThrownWhenOperationDefinitionDescriptionIsNull()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = "myOperation";
            _operationDefinition.Kind = OperationDefinition.OperationKind.Operation;
            _operationDefinition.Description = null;

            PubValidator.Validator.Validate(_operationDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validator_Validate_ArgumentExceptionThrownWhenOperationDefinitionDescriptionIsEmpty()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = "myOperation";
            _operationDefinition.Kind = OperationDefinition.OperationKind.Operation;
            _operationDefinition.Description = string.Empty;

            PubValidator.Validator.Validate(_operationDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validator_Validate_ArgumentExceptionThrownWhenOperationDefinitionCodeIsNull()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = "myOperation";
            _operationDefinition.Kind = OperationDefinition.OperationKind.Operation;
            _operationDefinition.Description = "this is  a description";
            _operationDefinition.Code = null;

            PubValidator.Validator.Validate(_operationDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validator_Validate_ArgumentExceptionThrownWhenOperationDefinitionCodeIsEmpty()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = "myOperation";
            _operationDefinition.Kind = OperationDefinition.OperationKind.Operation;
            _operationDefinition.Description = "this is  a description";
            _operationDefinition.Code = string.Empty;

            PubValidator.Validator.Validate(_operationDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validator_Validate_ArgumentNullExceptionThrownWhenOperationDefinitionSystemIsNull()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = "myOperation";
            _operationDefinition.Kind = OperationDefinition.OperationKind.Operation;
            _operationDefinition.Description = "this is  a description";
            _operationDefinition.Code = "MyCode";
            _operationDefinition.System = null;

            PubValidator.Validator.Validate(_operationDefinition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validator_Validate_ArgumentNullExceptionThrownWhenOperationDefinitionInstanceIsNull()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = "myOperation";
            _operationDefinition.Kind = OperationDefinition.OperationKind.Operation;
            _operationDefinition.Description = "this is  a description";
            _operationDefinition.Code = "MyCode";
            _operationDefinition.System = true;
            _operationDefinition.Instance = null;

            PubValidator.Validator.Validate(_operationDefinition);
        }
    }
}