using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;

namespace AflamakTests
{
    public class CountriesServiceTests
    {
        [Fact]
        public async Task GetCountryByID_WhenIDIsZero_ReturnNull()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Countries.GetById(A<int>.That.IsEqualTo(0))).Returns(Task.FromResult<Country>(null));

            var sut = new CountriesService(uow);

            //act
            var result = await sut.GetCountryByID(0);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCountryByID_WhenIDIsValid_ReturnCountry()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Country = new Country
            {
                Id = 1,
                Name = "Test"
            };

            A.CallTo(() => uow.Countries.GetById(A<int>.Ignored)).Returns(Task.FromResult(Country));

            var sut = new CountriesService(uow);

            //act
            var result = await sut.GetCountryByID(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Test");
        }

        [Fact]
        public async Task GetAllCountries_WhenThereIsNoCountries_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Countries.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<IEnumerable<Country>>(Enumerable.Empty<Country>()));

            var sut = new CountriesService(uow);

            //act

            var result = await sut.GetAllCountries(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAllCountries_WhenThereIsCountries_ReturnCountriesList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Countries = new List<Country>()
            {
                new Country(){Id = 1,Name = "Test 1"},
                new Country(){Id = 2,Name = "Test 2"},
                new Country(){Id = 3,Name = "Test 3"},
                new Country(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Countries.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult(Countries.AsEnumerable()));

            var sut = new CountriesService(uow);

            //act

            var result = await sut.GetAllCountries(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task AddCountry_WhenThereIsAnErrorWhileAdding_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Countries.Add(A<Country>.Ignored))
                .Returns(Task.FromResult<Country>(null));

            var sut = new CountriesService(uow);

            //act
            var result = await sut.AddCountry(new Country());

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while adding.", result.Error);
        }

        [Fact]
        public async Task AddCountry_WhenThereIsNoErrorsWhileAdding_AddSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Countries.Add(A<Country>.Ignored))
                .Returns(Task.FromResult<Country>(new Country()));

            var sut = new CountriesService(uow);

            //act
            var result = await sut.AddCountry(new Country());

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateCountry_WhenThereIsAnErrorWhileUpdating_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Countries.Update(A<Country>.Ignored)).Returns(null);

            var sut = new CountriesService(uow);

            //act
            var result = await sut.UpdateCountry(new Country());

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while updating.", result.Error);
        }

        [Fact]
        public async Task UpdateCountry_WhenThereIsNoErrorsWhileUpdating_UpdateSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Countries.Update(A<Country>.Ignored)).Returns(new Country());

            var sut = new CountriesService(uow);

            //act
            var result = await sut.UpdateCountry(new Country());

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteCountry_WhenThereIsAnErrorWhileDeleting_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Countries.Delete(A<Country>.Ignored)).Returns(null);

            var sut = new CountriesService(uow);

            //act
            var result = await sut.DeleteCountry(new Country());

            //assert
            Assert.NotNull(result);
            Assert.Equal("An error ouccered while deleting.", result.Error);
        }

        [Fact]
        public async Task DeleteCountry_WhenThereIsNoErrorsWhileDeleting_DeleteSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Countries.Delete(A<Country>.Ignored)).Returns(new Country());

            var sut = new CountriesService(uow);

            //act
            var result = await sut.DeleteCountry(new Country() { });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}
