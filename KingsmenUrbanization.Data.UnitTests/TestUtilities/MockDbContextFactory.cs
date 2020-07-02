using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Urbanization.Data.Abstractions;
using Urbanization.Data.Models;
using Moq;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace KingsmenUrbanization.Data.UnitTests.TestUtilities
{
    public static class MockDbContextFactory
    {
        public static IUrbanizationDbContext CreateMockDbContext(IEnumerable<UrbanizationByState> UrbanizationByStateDataSet)
        {
            IQueryable<UrbanizationByState> sourceList = UrbanizationByStateDataSet.AsQueryable();
            var mockSet = new Mock<DbSet<UrbanizationByState>>();
            mockSet.As<IQueryable<UrbanizationByState>>().Setup(m => m.Provider).Returns(sourceList.Provider);
            mockSet.As<IQueryable<UrbanizationByState>>().Setup(m => m.Expression).Returns(sourceList.Expression);
            mockSet.As<IQueryable<UrbanizationByState>>().Setup(m => m.ElementType).Returns(sourceList.ElementType);
            mockSet.As<IQueryable<UrbanizationByState>>().Setup(m => m.GetEnumerator()).Returns(sourceList.GetEnumerator());

            var mockContext = new Mock<IUrbanizationDbContext>();
            mockContext.SetupGet(m => m.UrbanizationByState).Returns(mockSet.Object);
            return mockContext.Object;
        }
    }
}
