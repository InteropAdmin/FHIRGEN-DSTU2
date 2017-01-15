using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fhir.Publication.Tests
{
    public class IntegrationTestAttribute : TestCategoryBaseAttribute
    {
        public override IList<string> TestCategories => new List<string> { "IntegrationTest" };
    }
}
