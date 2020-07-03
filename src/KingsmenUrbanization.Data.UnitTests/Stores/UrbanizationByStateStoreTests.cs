using KingsmenUrbanization.Data.UnitTests.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Urbanization.Data.Abstractions;
using Urbanization.Data.Models;
using Urbanization.Data.Stores;

namespace KingsmenUrbanization.Data.UnitTests
{
    [TestClass]
    public class UrbanizationByStateStoreTests
    {
        private IUrbanizationDbContext _DbContext;
        private UrbanizationByStateStore _UrbStateStore;
        private readonly UrbanizationByState urbStateOne = new UrbanizationByState
        {
            Id = 1,
            StateFips = 1,
            StateName = "Alabama",
            GISJoin = "G0100770011502",
            Latitude = 35,
            Longditude = -88,
            Population = 5486,
            UrbanIndex = 9,
        };
        private readonly UrbanizationByState urbStateTwo = new UrbanizationByState
        {
            Id = 2,
            StateFips = 5,
            StateName = "Arkansas",
            GISJoin = "G0501190004202",
            Latitude = 35,
            Longditude = -93,
            Population = 3842,
            UrbanIndex = 8,
        };
        private readonly UrbanizationByState urbStateThree = new UrbanizationByState
        {
            Id = 3,
            StateFips = 6,
            StateName = "California",
            GISJoin = "G0600190004215",
            Latitude = 37,
            Longditude = -120,
            Population = 4706,
            UrbanIndex = 12,
        };
        private List<UrbanizationByState> Data;
        [TestInitialize]
        public void Initialize()
        {
            Data = new List<UrbanizationByState> { urbStateOne, urbStateTwo, urbStateThree };
            _DbContext = MockDbContextFactory.CreateMockDbContext(Data);
            _UrbStateStore = new UrbanizationByStateStore(_DbContext);
        }

        [TestMethod]
        public async Task GetUrbanizationByStates_ReturnsDbSetData()
        {
            var data = await _UrbStateStore.GetUrbanizationByStates();
            var expectedCount = Data.Count();
            var actualCount = data.Intersect(Data).Count();
            Assert.AreEqual(expectedCount, actualCount, $"Intersection expected {expectedCount} objects but recieved {actualCount}");

        }
    }
}
