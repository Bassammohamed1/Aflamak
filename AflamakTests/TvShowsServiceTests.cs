using BusinessLogicLayer.DTOs;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace AflamakTests
{
    public class TvShowsServiceTests
    {
        [Fact]
        public async Task GetTvShowByID_WhenIDIsZero_ReturnNull()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetTvShowById(A<int>.That.IsEqualTo(0)))
                .Returns(Task.FromResult<TvShow>(null));

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.GetTvShowByID(0);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTvShowByID_WhenIDIsValid_ReturnTvShow()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShow = new TvShow
            {
                Id = 1,
                Name = "Test"
            };

            A.CallTo(() => uow.TvShows.GetTvShowById(A<int>.Ignored)).Returns(Task.FromResult(TvShow));

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.GetTvShowByID(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Test");
        }

        [Fact]
        public async Task GetAllTvShows_WhenThereIsNoTvShows_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetAllTvShows(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<IEnumerable<TvShow>>(Enumerable.Empty<TvShow>()));

            var sut = new TvShowsService(uow);

            //act

            var result = await sut.GetAllTvShows(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAllTvShows_WhenThereIsTvShows_ReturnTvShowsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow(){Id = 1,Name = "Test 1"},
                new TvShow(){Id = 2,Name = "Test 2"},
                new TvShow(){Id = 3,Name = "Test 3"},
                new TvShow(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.TvShows.GetAllTvShows(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult(TvShows.AsEnumerable()));

            var sut = new TvShowsService(uow);

            //act

            var result = await sut.GetAllTvShows(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public void GetTvShows_WhenLanguageIsNullAndItemTypeIsTvShowAndkeyIsNotNullAndIsNotRamadan_ReturnTvShows()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2
                },
            };

            A.CallTo(() => uow.TvShows.GetTvShows()).Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetTvShows(Keys.ID, null, ItemType.مسلسل, false);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetTvShows_WhenLanguageIsNotNullAndItemTypeIsTvShowAndIsNotRamadan_ReturnLanguageTvShows()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2,
                    Language = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2,
                    Language = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1,
                    Language = 2
                },
            };

            A.CallTo(() => uow.TvShows.GetTvShows()).Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetTvShows(null, Languages.عربي, ItemType.مسلسل, false);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetTvShows_WhenKeyIsNullLanguageIsNotNullAndItemTypeIsNotNullAndIsRamadan_ReturnCartoonTvShows()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2,
                    IsRamadan = true,
                    Year = 2026,
                    Month = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2,
                    IsRamadan = true,
                    Year = 2025,
                    Month = 10
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2,
                    IsRamadan = true,
                    Year = 2025,
                    Month = 1
                }
            };

            A.CallTo(() => uow.TvShows.GetTvShows()).Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetTvShows(null, Languages.عربي, ItemType.مسلسل, true);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public void GetTvShows_WhenLanguageIsNullAndItemTypeIsNull_ReturnAllTvShows()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2
                },
            };

            A.CallTo(() => uow.TvShows.GetTvShows()).Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetTvShows(null, null, null, false);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetFilteredTvShowsWithKey_WhenKeyIsID_ReturnTvShowsFilteredByID()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2
                },
            };

            A.CallTo(() => uow.TvShows.GetFilteredTvShowsWithID(A<int>.Ignored)).Returns(TvShows.AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetFilteredTvShowsWithKey(1, Keys.ID);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetFilteredTvShowsWithKey_WhenKeyIsProducer_ReturnTvShowsFilteredByProducerID()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2
                },
            };

            A.CallTo(() => uow.TvShows.GetFilteredTvShowsWithProducerID(A<int>.Ignored)).Returns(TvShows.AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetFilteredTvShowsWithKey(1, Keys.Producer);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetFilteredTvShowsWithKey_WhenKeyIsNotIDAndProducer_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetFilteredTvShowsWithKey(1, Keys.Year);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task GetAllTvShowsOrderedByKey_WhenThereIsNoTvShows_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetAllTvShowsOrderedByKey(A<int>.Ignored, A<int>.Ignored, A<Keys>.Ignored))
                .Returns(Task.FromResult(Enumerable.Empty<TvShow>()));

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.GetAllTvShowsOrderedByKey(1, 9, Keys.Year);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task GetAllTvShowsOrderedByKey_WhenThereIsTvShows_ReturnTvShowsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2
                },
            };

            A.CallTo(() => uow.TvShows.GetAllTvShowsOrderedByKey(A<int>.Ignored, A<int>.Ignored, A<Keys>.Ignored))
                .Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.GetAllTvShowsOrderedByKey(1, 9, Keys.Year);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetFilteredTvShows_WhenIsArabicAndIsNotRamadan_ReturnArabicTvShows()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2,
                    Year = 2026,
                    Language = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2,
                    Year = 2025,
                    Language = 2
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2,
                    Year = 2026,
                    Language = 1
                },
            };

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetFilteredTvShows(null, null, null, 2026, true, false);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFilteredTvShows_WhenIsNotArabicAndIsRamadan_ReturnRamadanTvShows()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2,
                    IsRamadan = true,
                    Year = 2026,
                    Month = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2,
                    IsRamadan = true,
                    Year = 2025,
                    Month = 10
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2,
                    IsRamadan = true,
                    Year = 2025,
                    Month = 1
                }
            };

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetFilteredTvShows(null, null, null, null, true, true);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetFilteredTvShows_WhenIsNotArabicAndIsNotRamadan_ReturnAllTvShows()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 2,
                    Language = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2,
                    Language = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 2,
                    Language = 5
                },
            };

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetFilteredTvShows(null, null, 1, null, true, false);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetRelatedTvShowsByKey_WhenKeyIsNoOfLikes_ReturnTvShowsFilteredByLikes()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    NoOfLikes = 5500
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    NoOfLikes = 10500
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    NoOfLikes = 1500
                },
            };

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetRelatedTvShowsByKey(Keys.NoOfLikes);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetRelatedTvShowsByKey_WhenKeyIsYear_ReturnTvShowsFilteredByYear()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 1",
                    Year = 2025,
                    Month = 10
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 2",
                    Year = 2025,
                    Month = 1
                },
                new TvShow()
                {
                    Id = 1,
                    Name = "Test 3",
                    Year = 2026,
                    Month = 1
                },
            };

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(TvShows);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetRelatedTvShowsByKey(Keys.Year);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetRelatedTvShowsByKey_WhenKeyIsNeitherLikesNorYear_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(Enumerable.Empty<TvShow>());

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetRelatedTvShowsByKey(Keys.ID);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task AddTvShow_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShow = new TvShowDTO
            {
                Id = 1,
                Name = "Test",
                clientFile = null
            };

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.AddTvShow(TvShow);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task AddTvShow_WhenThereIsAnErrorWhileAdding_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.AddTvShow(A<TvShow>.Ignored, A<List<int>>.Ignored))
                .Returns(Task.FromResult<TvShow>(null));

            var sut = new TvShowsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddTvShow(new TvShowDTO() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while adding.", result.Error);
        }

        [Fact]
        public async Task AddTvShow_WhenThereIsNoErrorsWhileAdding_AddSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.AddTvShow(A<TvShow>.Ignored, A<List<int>>.Ignored))
                .Returns(Task.FromResult<TvShow>(new TvShow()));

            var sut = new TvShowsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddTvShow(new TvShowDTO() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateTvShow_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShow = new TvShowDTO
            {
                Id = 1,
                Name = "Test",
                clientFile = null
            };

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.UpdateTvShow(TvShow);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task UpdateTvShow_WhenThereIsAnErrorWhileUpdating_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.UpdateTvShow(A<TvShow>.Ignored, A<List<int>>.Ignored))
                 .Returns(Task.FromResult<TvShow>(null));

            var sut = new TvShowsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdateTvShow(new TvShowDTO() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while updating.", result.Error);
        }

        [Fact]
        public async Task UpdateTvShow_WhenThereIsNoErrorsWhileUpdating_UpdateSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.UpdateTvShow(A<TvShow>.Ignored, A<List<int>>.Ignored))
                .Returns(Task.FromResult<TvShow>(new TvShow()));

            var sut = new TvShowsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdateTvShow(new TvShowDTO() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteTvShow_WhenThereIsAnErrorWhileDeleting_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.Delete(A<TvShow>.Ignored)).Returns(null);

            var sut = new TvShowsService(uow);

            //act
            var result = sut.DeleteTvShow(new TvShow());

            //assert
            Assert.NotNull(result);
            Assert.Equal("An error ouccered while deleting.", result.Error);
        }

        [Fact]
        public async Task DeleteTvShow_WhenThereIsNoErrorsWhileDeleting_DeleteSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.Delete(A<TvShow>.Ignored)).Returns(new TvShow());

            var sut = new TvShowsService(uow);

            //act
            var result = sut.DeleteTvShow(new TvShow() { });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task TvShowDetails_WhenIsNotAuthenticatedAndTvShowIsNull_ThrowsNullReferenceException()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetTvShowById(A<int>.Ignored))
                .Returns(Task.FromResult<TvShow>(null));

            var sut = new TvShowsService(uow);

            //act
            Func<int, bool, string, Task<TvShowDetailsDTO>> result = async (x, y, z) => await sut.TvShowDetails(x, y, z);

            //assert
            await Assert.ThrowsAsync<NullReferenceException>(() => result(1, false, "userID"));
        }

        [Fact]
        public async Task TvShowDetails_WhenIsNotAuthenticatedAndTvShowIsNotNull_ReturnData()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetTvShowById(A<int>.Ignored))
                .Returns(Task.FromResult(new TvShow()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.TvShowDetails(1, false, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.Equal(1, result.TvShow.Id);
            Assert.Equal("Test", result.TvShow.Name);
            Assert.NotNull(result.RelatedTvShows);
            Assert.False(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task TvShowDetails_WhenIsAuthenticatedAndTvShowIsNull_ThrowsNullReferenceException()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetTvShowById(A<int>.Ignored))
                .Returns(Task.FromResult<TvShow>(null));

            var sut = new TvShowsService(uow);

            //act
            Func<int, bool, string, Task<TvShowDetailsDTO>> result = async (x, y, z) => await sut.TvShowDetails(x, y, z);

            //assert
            await Assert.ThrowsAsync<NullReferenceException>(() => result(1, true, "userID"));
        }

        [Fact]
        public async Task TvShowDetails_WhenIsAuthenticatedAndTvShowIsNotNullAndInteractionIsNull_ReturnDataWithInteractionIsFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetTvShowById(A<int>.Ignored))
                .Returns(Task.FromResult(new TvShow()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
              .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.TvShowDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.Equal(1, result.TvShow.Id);
            Assert.Equal("Test", result.TvShow.Name);
            Assert.False(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
            Assert.NotNull(result.RelatedTvShows);
        }

        [Fact]
        public async Task TvShowDetails_WhenIsAuthenticatedAndTvShowIsNotNullAndInteractionIsNotNull_ReturnDataWithInteraction()
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

            A.CallTo(() => uow.TvShows.GetTvShowById(A<int>.Ignored))
                .Returns(Task.FromResult(new TvShow()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.TvShowDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.Equal(1, result.TvShow.Id);
            Assert.Equal("Test", result.TvShow.Name);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
            Assert.NotNull(result.RelatedTvShows);
        }

        [Fact]
        public async Task LikeTvShow_WhenInteractionIsNull_ReturnIsLikedTrueAndDisLikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Interactions.Add(new Interaction()))
               .Returns(Task.FromResult(new Interaction()));

            A.CallTo(() => uow.TvShows.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new TvShow()));

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.LikeTvShow(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.NotNull(result.RelatedTvShows);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task LikeTvShow_WhenInteractionIsNotNullAndUserIsDisliked_ReturnIsLikedTrueAndDisLikedFalse()
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

            A.CallTo(() => uow.TvShows.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new TvShow()));

            A.CallTo(() => uow.TvShows.GetTvShows())
                   .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.LikeTvShow(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.NotNull(result.RelatedTvShows);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task LikeTvShow_WhenInteractionIsNotNullAndUserIsliked_ReturnIsLikedFalse()
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

            A.CallTo(() => uow.TvShows.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new TvShow()));

            A.CallTo(() => uow.Interactions.Delete(new Interaction()))
               .Returns(new Interaction());

            A.CallTo(() => uow.TvShows.GetTvShows())
                 .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.LikeTvShow(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.NotNull(result.RelatedTvShows);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeTvShow_WhenInteractionIsNull_ReturnIsLikedFalseAndDislikedTrue()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Interactions.Add(new Interaction()))
               .Returns(Task.FromResult(new Interaction()));

            A.CallTo(() => uow.TvShows.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new TvShow()));

            A.CallTo(() => uow.TvShows.GetTvShows())
                   .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.DisLikeTvShow(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.NotNull(result.RelatedTvShows);
            Assert.True(result.HasUserDisliked);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeTvShow_WhenInteractionIsNotNullAndUserIsLiked_ReturnIsLikedFalseAndDislikedTrue()
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

            A.CallTo(() => uow.TvShows.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new TvShow()));

            A.CallTo(() => uow.TvShows.GetTvShows())
                 .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.DisLikeTvShow(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.NotNull(result.RelatedTvShows);
            Assert.True(result.HasUserDisliked);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeTvShow_WhenInteractionIsNotNullAndUserIsDisliked_ReturnIsDislikedFalse()
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

            A.CallTo(() => uow.TvShows.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new TvShow()));

            A.CallTo(() => uow.Interactions.Delete(new Interaction()))
               .Returns(new Interaction());

            A.CallTo(() => uow.TvShows.GetTvShows())
                 .Returns(new List<TvShow>().AsQueryable());

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.DisLikeTvShow(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.TvShow);
            Assert.NotNull(result.RelatedTvShows);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task GetTvShowsForSearch_WhenThrereIsNoTvShows_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.TvShows.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<TvShow>>(Enumerable.Empty<TvShow>()));

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetTvShowsForSearch("Test");

            //assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetTvShowsForSearch_WhenThrereIsActors_ReturnActorsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var TvShows = new List<TvShow>()
            {
                new TvShow(){Id = 1,Name = "Test 1"},
                new TvShow(){Id = 2,Name = "Test 2"},
                new TvShow(){Id = 3,Name = "Test 3"},
                new TvShow(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.TvShows.GetTvShows())
                .Returns(TvShows.AsEnumerable());

            var sut = new TvShowsService(uow);

            //act
            var result = sut.GetTvShowsForSearch("Test");

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetTvShowDataForSelectLists_WhenThrereIsNoData_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Actor>>(Enumerable.Empty<Actor>()));

            A.CallTo(() => uow.Producers.GetAllWithoutPagination())
              .Returns(Task.FromResult<IEnumerable<Producer>>(Enumerable.Empty<Producer>()));

            A.CallTo(() => uow.Categories.GetAllWithoutPagination())
              .Returns(Task.FromResult<IEnumerable<Category>>(Enumerable.Empty<Category>()));

            A.CallTo(() => uow.Countries.GetAllWithoutPagination())
              .Returns(Task.FromResult<IEnumerable<Country>>(Enumerable.Empty<Country>()));

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.GetTvShowDataForSelectLists();

            //assert
            Assert.NotNull(result);
            Assert.False(result.Actors.Any());
            Assert.False(result.Producers.Any());
            Assert.False(result.Categories.Any());
            Assert.False(result.Countries.Any());
        }

        [Fact]
        public async Task GetTvShowDataForSelectLists_WhenThrereIsData_ReturnDataList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Actors.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Actor>().AsEnumerable()));

            A.CallTo(() => uow.Producers.GetAllWithoutPagination())
              .Returns(Task.FromResult(new List<Producer>().AsEnumerable()));

            A.CallTo(() => uow.Categories.GetAllWithoutPagination())
              .Returns(Task.FromResult(new List<Category>().AsEnumerable()));

            A.CallTo(() => uow.Countries.GetAllWithoutPagination())
              .Returns(Task.FromResult(new List<Country>().AsEnumerable()));

            var sut = new TvShowsService(uow);

            //act
            var result = await sut.GetTvShowDataForSelectLists();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Actors);
            Assert.NotNull(result.Producers);
            Assert.NotNull(result.Categories);
            Assert.NotNull(result.Countries);
        }
    }
}