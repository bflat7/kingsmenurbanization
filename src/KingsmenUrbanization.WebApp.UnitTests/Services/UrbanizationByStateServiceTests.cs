using KingsmenUrbanization.Data.UnitTests.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urbanization.Data.Abstractions;
using Urbanization.Data.Models;
using Urbanization.Data.Stores;
using Urbanization.WebApp.Abstractions;
using Urbanization.WebApp.Services;

namespace KingsmenUrbanization.WebApp.UnitTests
{
    [TestClass]
    public class UrbanizationByStateServiceTests
    {
        private IServiceCollection _ServiceCollection;
        private IUrbanizationByStateService _UrbStateService;
        private readonly UrbanizationByState urbStateOne = new UrbanizationByState
        {
            Id = 1, StateFips = 1, StateName = "Alabama", GISJoin = "G0100770011502", Latitude = 35, Longditude = -88, Population = 5486, UrbanIndex = 9,
        };
        private readonly UrbanizationByState urbStateTwo = new UrbanizationByState
        {
            Id = 2, StateFips = 5, StateName = "Arkansas", GISJoin = "G0501190004202", Latitude = 35, Longditude = -93, Population = 3842, UrbanIndex = 8,
        };
        private readonly UrbanizationByState urbStateThree = new UrbanizationByState
        {
            Id = 3, StateFips = 6, StateName = "California", GISJoin = "G0600190004215", Latitude = 37, Longditude = -120, Population = 4706, UrbanIndex = 12,
        };
        private List<UrbanizationByState> Data;

        [TestInitialize]
        public void Initialize()
        {
            Data = new List<UrbanizationByState> { urbStateOne, urbStateTwo, urbStateThree };
            _ServiceCollection = new ServiceCollection();
            _ServiceCollection.AddSingleton(MockDbContextFactory.CreateMockDbContext(Data));
            _ServiceCollection.AddSingleton<IUrbanizationByStateStore, UrbanizationByStateStore>();
            _ServiceCollection.AddSingleton<IUrbanizationByStateService, UrbanizationByStateService>();
            _UrbStateService = _ServiceCollection.BuildServiceProvider().GetRequiredService<IUrbanizationByStateService>();
        }

        [TestMethod]
        public async Task GetCountyUrbanizationCount_ReturnsExpectedCount()
        {
            var expectedCount = Data.Count;
            var actualCount = await _UrbStateService.GetCountyUrbanizationCount();
            Assert.AreEqual(expectedCount, actualCount, $"{expectedCount} objects expected, instead recieved {actualCount}");
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPaged_AllDataSinglePageEmptyOrderByOrder_ReturnsDefaultList()
        {
            int page = 0;
            int rowsPerPage = Data.Count;

            var actualData = (await _UrbStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, string.Empty, string.Empty)).ToList();
            Assert.AreEqual(urbStateOne.Id, actualData[0].Id, $"Expected object with id: {urbStateOne.Id} at index 0, recieved {actualData[0].Id}");
            Assert.AreEqual(urbStateTwo.Id, actualData[1].Id, $"Expected object with id: {urbStateTwo.Id} at index 0, recieved {actualData[1].Id}");
            Assert.AreEqual(urbStateThree.Id, actualData[2].Id, $"Expected object with id: {urbStateThree.Id} at index 0, recieved {actualData[2].Id}");
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPaged_AllDataSinglePageNullOrderByOrder_ReturnsDefaultList()
        {
            int page = 0;
            int rowsPerPage = Data.Count;

            var actualData = (await _UrbStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, null, null)).ToList();
            Assert.AreEqual(urbStateOne.Id, actualData[0].Id, $"Expected object with id: {urbStateOne.Id} at index 0, recieved {actualData[0].Id}");
            Assert.AreEqual(urbStateTwo.Id, actualData[1].Id, $"Expected object with id: {urbStateTwo.Id} at index 0, recieved {actualData[1].Id}");
            Assert.AreEqual(urbStateThree.Id, actualData[2].Id, $"Expected object with id: {urbStateThree.Id} at index 0, recieved {actualData[2].Id}");
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPage_InvalidOrderByStringError_ReturnsArgumentException()
        {
            try
            {
                int page = 0;
                int rowsPerPage = Data.Count;

                var actualData = await _UrbStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, "nonexistentcolumnname", string.Empty) as IList<UrbanizationByState>;
            } catch (ArgumentException)
            {
                // do nothing here, this argument is expected so the test should pass
            } catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPage_NotSortedTwoPerPage_ReturnTwoItemsDefaultOrder()
        {
            int page = 0;
            int rowsPerPage = 2;

            var pagedData = (await _UrbStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, string.Empty, string.Empty)).ToList();
            Assert.AreEqual(rowsPerPage, pagedData.Count());
            Assert.AreEqual(urbStateOne.Id, pagedData.First().Id);
            Assert.AreEqual(urbStateTwo.Id, pagedData.Last().Id);
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPage_NotSortedTwoPerPagePage2_ReturnSingleItemDefaultOrder()
        {
            int page = 1;
            int rowsPerPage = 2;
            int expectedRows = Data.Count % rowsPerPage;

            var pagedData = (await _UrbStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, string.Empty, string.Empty)).ToList();
            Assert.AreEqual(expectedRows, pagedData.Count());
            Assert.AreEqual(urbStateThree.Id, pagedData.First().Id);
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPage_OrderByNoOrder_ReturnsItemsSorted()
        {
            int page = 0;
            int rowsPerPage = Data.Count;
            string orderBy = "population";


            var sortedData = (await _UrbStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, orderBy, string.Empty)).ToList();
            int index = 0;
            foreach(var popCount in Data.Select(d => d.Population).OrderBy(i => i))
            {
                Assert.AreEqual(popCount, sortedData[index].Population);
                index++;
            }
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPage_OrderByOrderAsc_ReturnsItemsSorted()
        {
            int page = 0;
            int rowsPerPage = Data.Count;
            string orderBy = "population";
            string order = "asc";


            var sortedData = (await _UrbStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, orderBy, order)).ToList();
            int index = 0;
            foreach (var popCount in Data.Select(d => d.Population).OrderBy(i => i))
            {
                Assert.AreEqual(popCount, sortedData[index].Population);
                index++;
            }
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPage_OrderByOrderDesc_ReturnsItemsSorted()
        {
            int page = 0;
            int rowsPerPage = Data.Count;
            string orderBy = "population";
            string order = "desc";


            var sortedData = (await _UrbStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, orderBy, order)).ToList();
            int index = 0;
            foreach (var popCount in Data.Select(d => d.Population).OrderByDescending(i => i))
            {
                Assert.AreEqual(popCount, sortedData[index].Population);
                index++;
            }
        }

        [TestMethod]
        public async Task GetStateUrbanizationSortedPage_InvalidOrder_ReturnsArgumentException()
        {
            try
            {
                await _UrbStateService.GetStateUrbanizationSortedPaged(1, 1, string.Empty, "invalidOrderString");
            } catch (ArgumentException)
            {

            }
            catch (Exception)
            {
                Assert.Fail("Expected an ArgumentException to be thrown");
            }
        }
    }
}
