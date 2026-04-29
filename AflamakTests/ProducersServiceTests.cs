using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace AflamakTests
{
    public class ProducersServiceTests
    {
        [Fact]
        public async Task GetProducerByID_WhenIDIsZero_ReturnNull()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.GetById(A<int>.That.IsEqualTo(0))).Returns(Task.FromResult<Producer>(null));

            var sut = new ProducersService(uow);

            //act
            var result = await sut.GetProducerByID(0);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProducerByID_WhenIDIsValid_ReturnProducer()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Producer = new Producer
            {
                Id = 1,
                Name = "Test",
                AnotherLangName = "Test"
            };

            A.CallTo(() => uow.Producers.GetById(A<int>.Ignored)).Returns(Task.FromResult(Producer));

            var sut = new ProducersService(uow);

            //act
            var result = await sut.GetProducerByID(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Test");
        }

        [Fact]
        public async Task GetAllProducers_WhenThereIsNoProducers_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<IEnumerable<Producer>>(Enumerable.Empty<Producer>()));

            var sut = new ProducersService(uow);

            //act

            var result = await sut.GetAllProducers(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAllProducers_WhenThereIsProducers_ReturnProducersList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Producers = new List<Producer>()
            {
                new Producer(){Id = 1,Name = "Test 1"},
                new Producer(){Id = 2,Name = "Test 2"},
                new Producer(){Id = 3,Name = "Test 3"},
                new Producer(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Producers.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult(Producers.AsEnumerable()));

            var sut = new ProducersService(uow);

            //act

            var result = await sut.GetAllProducers(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task AddProducer_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Producer = new Producer
            {
                Id = 1,
                Name = "Test",
                AnotherLangName = "Test",
                clientFile = null
            };

            var sut = new ProducersService(uow);

            //act
            var result = await sut.AddProducer(Producer);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task AddProducer_WhenThereIsAnErrorWhileAdding_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.Add(A<Producer>.Ignored))
                .Returns(Task.FromResult<Producer>(null));

            var sut = new ProducersService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddProducer(new Producer() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while adding.", result.Error);
        }

        [Fact]
        public async Task AddProducer_WhenThereIsNoErrorsWhileAdding_AddSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.Add(A<Producer>.Ignored))
                .Returns(Task.FromResult<Producer>(new Producer()));

            var sut = new ProducersService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddProducer(new Producer() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateProducer_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Producer = new Producer
            {
                Id = 1,
                Name = "Test",
                AnotherLangName = "Test",
                clientFile = null
            };

            var sut = new ProducersService(uow);

            //act
            var result = await sut.UpdateProducer(Producer);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task UpdateProducer_WhenThereIsAnErrorWhileUpdating_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.Update(A<Producer>.Ignored)).Returns(null);

            var sut = new ProducersService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdateProducer(new Producer() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while updating.", result.Error);
        }

        [Fact]
        public async Task UpdateProducer_WhenThereIsNoErrorsWhileUpdating_UpdateSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.Update(A<Producer>.Ignored)).Returns(new Producer());

            var sut = new ProducersService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdateProducer(new Producer() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteProducer_WhenThereIsAnErrorWhileDeleting_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.Delete(A<Producer>.Ignored)).Returns(null);

            var sut = new ProducersService(uow);

            //act
            var result = sut.DeleteProducer(new Producer());

            //assert
            Assert.NotNull(result);
            Assert.Equal("An error ouccered while deleting.", result.Error);
        }

        [Fact]
        public async Task DeleteProducer_WhenThereIsNoErrorsWhileDeleting_DeleteSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.Delete(A<Producer>.Ignored)).Returns(new Producer());

            var sut = new ProducersService(uow);

            //act
            var result = sut.DeleteProducer(new Producer() { });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetProducersForSearch_WhenThrereIsNoProducers_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Producer>>(Enumerable.Empty<Producer>()));

            var sut = new ProducersService(uow);

            //act
            var result = sut.GetProducersForSearch("Test");

            //assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetProducersForSearch_WhenThrereIsProducers_ReturnProducersList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Producers = new List<Producer>()
            {
                new Producer(){Id = 1,Name = "Test 1"},
                new Producer(){Id = 2,Name = "Test 2"},
                new Producer(){Id = 3,Name = "Test 3"},
                new Producer(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Producers.GetAllWithoutPagination())
                .Returns(Task.FromResult(Producers.AsEnumerable()));

            var sut = new ProducersService(uow);

            //act
            var result = sut.GetProducersForSearch("Test");

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task GetProducerDetails_WhenProducerIsNull_ThrowNullReferenceException()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.GetById(A<int>.Ignored))
                .Returns(Task.FromResult<Producer>(null));

            var sut = new ProducersService(uow);

            //act
            Func<int, Task<PersonDetailsDTO<Producer>>> result = async (x) => await sut.ProducerDetails(x);

            //assert
            await Assert.ThrowsAsync<NullReferenceException>(() => result(1));
        }

        [Fact]
        public async Task GetProducerDetails_WhenProducerExists_ReturnProducerDetails()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Producers.GetById(A<int>.Ignored))
                .Returns(new Producer()
                {
                    Id = 1,
                    Name = "Test",
                    AnotherLangName = "Test"
                });

            A.CallTo(() => uow.Films.GetFilteredFilmsWithProducerID(A<int>.Ignored))
                .Returns(new List<Film>().AsQueryable());

            A.CallTo(() => uow.TvShows.GetFilteredTvShowsWithProducerID(A<int>.Ignored))
                .Returns(new List<TvShow>().AsQueryable());

            var sut = new ProducersService(uow);

            //act
            var result = await sut.ProducerDetails(1);

            //assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Person.Id);
            Assert.Equal("Test", result.Person.Name);
            Assert.NotNull(result.Works);
        }
    }
}
