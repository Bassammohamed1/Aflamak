using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace AflamakTests
{
    public class PartsServiceTests
    {
        [Fact]
        public async Task GetPartByID_WhenIDIsZero_ReturnNull()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.GetById(A<int>.That.IsEqualTo(0))).Returns(Task.FromResult<Part>(null));

            var sut = new PartsService(uow);

            //act
            var result = await sut.GetPartByID(0);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPartByID_WhenIDIsValid_ReturnPart()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Part = new Part
            {
                Id = 1,
                Name = "Test"
            };

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored)).Returns(Task.FromResult(Part));

            var sut = new PartsService(uow);

            //act
            var result = await sut.GetPartByID(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Test");
        }

        [Fact]
        public async Task GetAllParts_WhenThereIsNoParts_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<IEnumerable<Part>>(Enumerable.Empty<Part>()));

            var sut = new PartsService(uow);

            //act

            var result = await sut.GetAllParts(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAllParts_WhenThereIsParts_ReturnPartsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Parts = new List<Part>()
            {
                new Part(){Id = 1,Name = "Test 1"},
                new Part(){Id = 2,Name = "Test 2"},
                new Part(){Id = 3,Name = "Test 3"},
                new Part(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Parts.GetAllParts(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult(Parts.AsEnumerable()));

            var sut = new PartsService(uow);

            //act

            var result = await sut.GetAllParts(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task AddPart_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Part = new Part
            {
                Id = 1,
                Name = "Test",
                clientFile = null
            };

            var sut = new PartsService(uow);

            //act
            var result = await sut.AddPart(Part);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task AddPart_WhenThereIsAnErrorWhileAdding_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.Add(A<Part>.Ignored))
                .Returns(Task.FromResult<Part>(null));

            var sut = new PartsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddPart(new Part() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while adding.", result.Error);
        }

        [Fact]
        public async Task AddPart_WhenThereIsNoErrorsWhileAdding_AddSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.Add(A<Part>.Ignored))
                .Returns(Task.FromResult<Part>(new Part()));

            var sut = new PartsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddPart(new Part() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdatePart_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Part = new Part
            {
                Id = 1,
                Name = "Test",
                clientFile = null
            };

            var sut = new PartsService(uow);

            //act
            var result = await sut.UpdatePart(Part);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task UpdatePart_WhenThereIsAnErrorWhileUpdating_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.Update(A<Part>.Ignored)).Returns(null);

            var sut = new PartsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdatePart(new Part() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while updating.", result.Error);
        }

        [Fact]
        public async Task UpdatePart_WhenThereIsNoErrorsWhileUpdating_UpdateSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.Update(A<Part>.Ignored)).Returns(new Part());

            var sut = new PartsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdatePart(new Part() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeletePart_WhenThereIsAnErrorWhileDeleting_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.Delete(A<Part>.Ignored)).Returns(null);

            var sut = new PartsService(uow);

            //act
            var result = await sut.DeletePart(new Part());

            //assert
            Assert.NotNull(result);
            Assert.Equal("An error ouccered while deleting.", result.Error);
        }

        [Fact]
        public async Task DeletePart_WhenThereIsNoErrorsWhileDeleting_DeleteSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.Delete(A<Part>.Ignored)).Returns(new Part());

            var sut = new PartsService(uow);

            //act
            var result = await sut.DeletePart(new Part() { });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task PartDetails_WhenIsNotAuthenticatedAndPartIsNull_ReturnEmptyObject()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
                .Returns(Task.FromResult<Part>(null));

            var sut = new PartsService(uow);

            //act
            var result = await sut.PartDetails(1, false, "userID");

            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PartDetails_WhenIsNotAuthenticatedAndPartIsNotNull_ReturnData()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
                .Returns(Task.FromResult(new Part()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
                .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
              .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.PartDetails(1, false, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.Equal(1, result.Part.Id);
            Assert.Equal("Test", result.Part.Name);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
        }

        [Fact]
        public async Task PartDetails_WhenIsAuthenticatedAndPartIsNull_ReturnEmptyObject()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
                .Returns(Task.FromResult<Part>(null));

            var sut = new PartsService(uow);

            //act
            var result = await sut.PartDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PartDetails_WhenIsAuthenticatedAndPartIsNotNullAndInteractionIsNull_ReturnDataWithInteractionIsFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
                .Returns(Task.FromResult(new Part()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
              .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
                .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
              .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.PartDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.Equal(1, result.Part.Id);
            Assert.Equal("Test", result.Part.Name);
            Assert.False(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
        }

        [Fact]
        public async Task PartDetails_WhenIsAuthenticatedAndPartIsNotNullAndInteractionIsNotNull_ReturnDataWithInteraction()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Id = 1,
                    IsLiked = true,
                    IsDisLiked = false,
                    UserId = "userID",
                    ItemId = 1
                }
            };

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
                .Returns(Task.FromResult(new Part()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
                .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
              .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.PartDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.Equal(1, result.Part.Id);
            Assert.Equal("Test", result.Part.Name);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
        }

        [Fact]
        public async Task LikePart_WhenInteractionIsNull_ReturnIsLikedTrueAndDisLikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Interactions.Add(new Interaction()))
               .Returns(Task.FromResult(new Interaction()));

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Part()));

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
               .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.LikePart(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task LikePart_WhenInteractionIsNotNullAndUserIsDisliked_ReturnIsLikedTrueAndDisLikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    ItemId = 1,
                    UserId= "userID",
                    IsDisLiked =true
                }
            };

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Part()));

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
               .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.LikePart(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task LikePart_WhenInteractionIsNotNullAndUserIsliked_ReturnIsLikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    ItemId = 1,
                    UserId= "userID",
                    IsLiked = true
                }
            };

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Part()));

            A.CallTo(() => uow.Interactions.Delete(new Interaction()))
               .Returns(new Interaction());

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
               .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.LikePart(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikePart_WhenInteractionIsNull_ReturnIsLikedFalseAndDislikedTrue()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Interactions.Add(new Interaction()))
               .Returns(Task.FromResult(new Interaction()));

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Part()));

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
               .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.DisLikePart(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
            Assert.True(result.HasUserDisliked);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikePart_WhenInteractionIsNotNullAndUserIsLiked_ReturnIsLikedFalseAndDislikedTrue()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    ItemId = 1,
                    UserId= "userID",
                    IsLiked =true
                }
            };

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Part()));

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
               .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.DisLikePart(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
            Assert.True(result.HasUserDisliked);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikePart_WhenInteractionIsNotNullAndUserIsDisliked_ReturnIsDislikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    ItemId = 1,
                    UserId= "userID",
                    IsDisLiked = true
                }
            };

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Parts.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Part()));

            A.CallTo(() => uow.Interactions.Delete(new Interaction()))
               .Returns(new Interaction());

            A.CallTo(() => uow.Parts.GetFilteredPartsWithTvShowId(A<int>.Ignored))
               .Returns(new List<Part>().AsQueryable());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new PartsService(uow);

            //act
            var result = await sut.DisLikePart(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Part);
            Assert.NotNull(result.Parts);
            Assert.NotNull(result.Episodes);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task GetAllTvShowsForSelectList_WhenThereIsNoTvShows_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetAllTvShowsForSelectList())
                .Returns(Enumerable.Empty<TvShow>());

            var sut = new PartsService(uow);

            //act
            var result = sut.GetAllTvShowsForSelectList();

            //assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetAllTvShowsForSelectList_WhenThereIsTvShows_ReturnTvShowsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetAllTvShowsForSelectList())
                .Returns(new List<TvShow>()
                {
                    new TvShow()
                    {
                        Id = 1,
                        Name = "Test"
                    }
                }.AsEnumerable());

            var sut = new PartsService(uow);

            //act
            var result = sut.GetAllTvShowsForSelectList();

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
        }
    }
}
