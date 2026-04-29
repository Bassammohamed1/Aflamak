using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace AflamakTests
{
    public class ActorsServiceTests
    {
        [Fact]
        public async Task GetActorByID_WhenIDIsZero_ReturnNull()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.GetById(A<int>.That.IsEqualTo(0))).Returns(Task.FromResult<Actor>(null));

            var sut = new ActorsService(uow, null, null);

            //act
            var result = await sut.GetActorByID(0);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetActorByID_WhenIDIsValid_ReturnActor()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var actor = new Actor
            {
                Id = 1,
                Name = "Test",
                AnotherLangName = "Test"
            };

            A.CallTo(() => uow.Actors.GetById(A<int>.Ignored)).Returns(Task.FromResult(actor));

            var sut = new ActorsService(uow, null, null);

            //act
            var result = await sut.GetActorByID(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Test");
        }

        [Fact]
        public async Task GetAllActors_WhenThereIsNoActors_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<IEnumerable<Actor>>(Enumerable.Empty<Actor>()));

            var sut = new ActorsService(uow, null, null);

            //act

            var result = await sut.GetAllActors(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAllActors_WhenThereIsActors_ReturnActorsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var actors = new List<Actor>()
            {
                new Actor(){Id = 1,Name = "Test 1"},
                new Actor(){Id = 2,Name = "Test 2"},
                new Actor(){Id = 3,Name = "Test 3"},
                new Actor(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Actors.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult(actors.AsEnumerable()));

            var sut = new ActorsService(uow, null, null);

            //act

            var result = await sut.GetAllActors(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task AddActor_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var actor = new Actor
            {
                Id = 1,
                Name = "Test",
                AnotherLangName = "Test",
                clientFile = null
            };

            var sut = new ActorsService(uow, null, null);

            //act
            var result = await sut.AddActor(actor);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task AddActor_WhenThereIsAnErrorWhileAdding_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.Add(A<Actor>.Ignored))
                .Returns(Task.FromResult<Actor>(null));

            var sut = new ActorsService(uow, null, null);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddActor(new Actor() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while adding.", result.Error);
        }

        [Fact]
        public async Task AddActor_WhenThereIsNoErrorsWhileAdding_AddSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.Add(A<Actor>.Ignored))
                .Returns(Task.FromResult<Actor>(new Actor()));

            var sut = new ActorsService(uow, null, null);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddActor(new Actor() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateActor_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var actor = new Actor
            {
                Id = 1,
                Name = "Test",
                AnotherLangName = "Test",
                clientFile = null
            };

            var sut = new ActorsService(uow, null, null);

            //act
            var result = await sut.UpdateActor(actor);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task UpdateActor_WhenThereIsAnErrorWhileUpdating_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.Update(A<Actor>.Ignored)).Returns(null);

            var sut = new ActorsService(uow, null, null);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdateActor(new Actor() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while updating.", result.Error);
        }

        [Fact]
        public async Task UpdateActor_WhenThereIsNoErrorsWhileUpdating_UpdateSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.Update(A<Actor>.Ignored)).Returns(new Actor());

            var sut = new ActorsService(uow, null, null);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdateActor(new Actor() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteActor_WhenThereIsAnErrorWhileDeleting_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.Delete(A<Actor>.Ignored)).Returns(null);

            var sut = new ActorsService(uow, null, null);

            //act
            var result = sut.DeleteActor(new Actor());

            //assert
            Assert.NotNull(result);
            Assert.Equal("An error ouccered while deleting.", result.Error);
        }

        [Fact]
        public async Task DeleteActor_WhenThereIsNoErrorsWhileDeleting_DeleteSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.Delete(A<Actor>.Ignored)).Returns(new Actor());

            var sut = new ActorsService(uow, null, null);

            //act
            var result = sut.DeleteActor(new Actor() { });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetActorsForSearch_WhenThrereIsNoActors_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Actor>>(Enumerable.Empty<Actor>()));

            var sut = new ActorsService(uow, null, null);

            //act
            var result = sut.GetActorsForSearch("Test");

            //assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetActorsForSearch_WhenThrereIsActors_ReturnActorsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var actors = new List<Actor>()
            {
                new Actor(){Id = 1,Name = "Test 1"},
                new Actor(){Id = 2,Name = "Test 2"},
                new Actor(){Id = 3,Name = "Test 3"},
                new Actor(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Actors.GetAllWithoutPagination())
                .Returns(Task.FromResult(actors.AsEnumerable()));

            var sut = new ActorsService(uow, null, null);

            //act
            var result = sut.GetActorsForSearch("Test");

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task GetActorDetails_WhenActorIsNull_ThrowNullReferenceException()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.GetById(A<int>.Ignored))
                .Returns(Task.FromResult<Actor>(null));

            var sut = new ActorsService(uow, null, null);

            //act
            Func<int, Task<PersonDetailsDTO<Actor>>> result = async (x) => await sut.GetActorDetails(x);

            //assert
            await Assert.ThrowsAsync<NullReferenceException>(() => result(1));
        }

        [Fact]
        public async Task GetActorDetails_WhenActorExists_ReturnActorDetails()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();
            var filmService = A.Fake<IFilmsService>();
            var tvShowService = A.Fake<ITvShowsService>();

            A.CallTo(() => uow.Actors.GetById(A<int>.Ignored))
                .Returns(new Actor()
                {
                    Id = 1,
                    Name = "Test",
                    AnotherLangName = "Test"
                });

            A.CallTo(() => uow.ActorFilms.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<ActorFilms>().AsEnumerable()));

            A.CallTo(() => uow.ActorTvShows.GetAllWithoutPagination())
               .Returns(Task.FromResult(new List<ActorTvShows>().AsEnumerable()));

            A.CallTo(() => filmService.GetFilteredFilmsWithKey(A<int>.Ignored, A<Keys>.Ignored))
                .Returns(new List<Film>().AsQueryable());

            A.CallTo(() => tvShowService.GetFilteredTvShowsWithKey(A<int>.Ignored, A<Keys>.Ignored))
                .Returns(new List<TvShow>().AsQueryable());

            var sut = new ActorsService(uow, filmService, tvShowService);

            //act
            var result = await sut.GetActorDetails(1);

            //assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Person.Id);
            Assert.Equal("Test", result.Person.Name);
            Assert.NotNull(result.Works);
        }
    }
}