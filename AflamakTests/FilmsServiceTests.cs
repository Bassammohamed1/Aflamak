using BusinessLogicLayer.DTOs;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace AflamakTests
{
    public class FilmsServiceTests
    {
        [Fact]
        public async Task GetFilmByID_WhenIDIsZero_ReturnNull()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetFilmById(A<int>.That.IsEqualTo(0))).Returns(Task.FromResult<Film>(null));

            var sut = new FilmsService(uow);

            //act
            var result = await sut.GetFilmByID(0);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFilmByID_WhenIDIsValid_ReturnFilm()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Film = new Film
            {
                Id = 1,
                Name = "Test"
            };

            A.CallTo(() => uow.Films.GetFilmById(A<int>.Ignored)).Returns(Task.FromResult(Film));

            var sut = new FilmsService(uow);

            //act
            var result = await sut.GetFilmByID(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Test");
        }

        [Fact]
        public async Task GetAllFilms_WhenThereIsNoFilms_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetAllFilms(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<IEnumerable<Film>>(Enumerable.Empty<Film>()));

            var sut = new FilmsService(uow);

            //act

            var result = await sut.GetAllFilms(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAllFilms_WhenThereIsFilms_ReturnFilmsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Films = new List<Film>()
            {
                new Film(){Id = 1,Name = "Test 1"},
                new Film(){Id = 2,Name = "Test 2"},
                new Film(){Id = 3,Name = "Test 3"},
                new Film(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Films.GetAllFilms(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult(Films.AsEnumerable()));

            var sut = new FilmsService(uow);

            //act

            var result = await sut.GetAllFilms(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public void GetFilms_WhenLanguageIsNullAndItemTypeIsFilmAndkeyIsNotNull_ReturnFilms()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1
                },
            };

            A.CallTo(() => uow.Films.GetFilms()).Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilms(Keys.ID, null, ItemType.فيلم);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilms_WhenLanguageIsNotNullAndItemTypeIsNull_ReturnLanguageFilms()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1,
                    Language = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2,
                    Language = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1,
                    Language = 2
                },
            };

            A.CallTo(() => uow.Films.GetFilms()).Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilms(null, Languages.انجليزي, null);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public void GetFilms_WhenLanguageIsNullAndItemTypeIsNotNull_ReturnCartoonFilms()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 3
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 3
                },
            };

            A.CallTo(() => uow.Films.GetFilms()).Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilms(null, null, ItemType.كرتون);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilms_WhenLanguageIsNullAndItemTypeIsNull_ReturnAllFilms()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1
                },
            };

            A.CallTo(() => uow.Films.GetFilms()).Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilms(null, null, null);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetFilteredFilmsWithKey_WhenKeyIsID_ReturnFilmsFilteredByID()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1
                },
            };

            A.CallTo(() => uow.Films.GetFilteredFilmsWithID(A<int>.Ignored)).Returns(films.AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilteredFilmsWithKey(1, Keys.ID);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetFilteredFilmsWithKey_WhenKeyIsProducer_ReturnFilmsFilteredByProducerID()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1
                },
            };

            A.CallTo(() => uow.Films.GetFilteredFilmsWithProducerID(A<int>.Ignored)).Returns(films.AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilteredFilmsWithKey(1, Keys.Producer);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetFilteredFilmsWithKey_WhenKeyIsNotIDAndProducer_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilteredFilmsWithKey(1, Keys.Year);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task GetAllFilmsOrderedByKey_WhenThereIsNoFilms_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetAllFilmsOrderedByKey(A<int>.Ignored, A<int>.Ignored, A<Keys>.Ignored))
                .Returns(Task.FromResult(Enumerable.Empty<Film>()));

            var sut = new FilmsService(uow);

            //act
            var result = await sut.GetAllFilmsOrderedByKey(1, 9, Keys.Year);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task GetAllFilmsOrderedByKey_WhenThereIsFilms_ReturnFilmsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 2
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1
                },
            };

            A.CallTo(() => uow.Films.GetAllFilmsOrderedByKey(A<int>.Ignored, A<int>.Ignored, A<Keys>.Ignored))
                .Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = await sut.GetAllFilmsOrderedByKey(1, 9, Keys.Year);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetFilteredFilms_WhenIsArabicAndIsNotCartoon_ReturnArabicFilms()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1,
                    Year = 2026,
                    Language = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 1,
                    Year = 2025,
                    Language = 2
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1,
                    Year = 2026,
                    Language = 1
                },
            };

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilteredFilms(null, null, null, 2026, true, false);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFilteredFilms_WhenIsNotArabicAndIsCartoon_ReturnCartoonFilms()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1,
                    CountryId = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 3,
                    CountryId = 5
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 3,
                    CountryId = 1
                },
            };

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilteredFilms(null, "1", null, null, false, true);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetFilteredFilms_WhenIsNotArabicAndIsNotCartoon_ReturnAllFilms()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Type = 1,
                    Language = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Type = 1,
                    Language = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Type = 1,
                    Language = 5
                },
            };

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilteredFilms(null, null, 1, null, true, false);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetRelatedFilmsByKey_WhenKeyIsNoOfLikes_ReturnFilmsFilteredByLikes()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                     NoOfLikes = 5500
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                     NoOfLikes = 10500
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    NoOfLikes = 1500
                },
            };

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetRelatedFilmsByKey(Keys.NoOfLikes);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetRelatedFilmsByKey_WhenKeyIsYear_ReturnFilmsFilteredByYear()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film()
                {
                    Id = 1,
                    Name = "Test 1",
                    Year = 2025,
                    Month = 10
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 2",
                    Year = 2025,
                    Month = 1
                },
                new Film()
                {
                    Id = 1,
                    Name = "Test 3",
                    Year = 2026,
                    Month = 1
                },
            };

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(films);

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetRelatedFilmsByKey(Keys.Year);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetRelatedFilmsByKey_WhenKeyIsNeitherLikesNorYear_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(Enumerable.Empty<Film>());

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetRelatedFilmsByKey(Keys.ID);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task AddFilm_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Film = new FilmDTO
            {
                Id = 1,
                Name = "Test",
                clientFile = null
            };

            var sut = new FilmsService(uow);

            //act
            var result = await sut.AddFilm(Film);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task AddFilm_WhenThereIsAnErrorWhileAdding_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.AddFilm(A<Film>.Ignored, A<List<int>>.Ignored))
                .Returns(Task.FromResult<Film>(null));

            var sut = new FilmsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddFilm(new FilmDTO() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while adding.", result.Error);
        }

        [Fact]
        public async Task AddFilm_WhenThereIsNoErrorsWhileAdding_AddSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.AddFilm(A<Film>.Ignored, A<List<int>>.Ignored))
                .Returns(Task.FromResult<Film>(new Film()));

            var sut = new FilmsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.AddFilm(new FilmDTO() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateFilm_WhenClientfileIsNull_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Film = new FilmDTO
            {
                Id = 1,
                Name = "Test",
                clientFile = null
            };

            var sut = new FilmsService(uow);

            //act
            var result = await sut.UpdateFilm(Film);

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("clientFile is missing.", result.Error);
        }

        [Fact]
        public async Task UpdateFilm_WhenThereIsAnErrorWhileUpdating_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.UpdateFilm(A<Film>.Ignored, A<List<int>>.Ignored))
                 .Returns(Task.FromResult<Film>(null));

            var sut = new FilmsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdateFilm(new FilmDTO() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while updating.", result.Error);
        }

        [Fact]
        public async Task UpdateFilm_WhenThereIsNoErrorsWhileUpdating_UpdateSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.UpdateFilm(A<Film>.Ignored, A<List<int>>.Ignored))
                .Returns(Task.FromResult<Film>(new Film()));

            var sut = new FilmsService(uow);

            //act
            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var result = await sut.UpdateFilm(new FilmDTO() { clientFile = file });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteFilm_WhenThereIsAnErrorWhileDeleting_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.Delete(A<Film>.Ignored)).Returns(null);

            var sut = new FilmsService(uow);

            //act
            var result = sut.DeleteFilm(new Film());

            //assert
            Assert.NotNull(result);
            Assert.Equal("An error ouccered while deleting.", result.Error);
        }

        [Fact]
        public async Task DeleteFilm_WhenThereIsNoErrorsWhileDeleting_DeleteSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.Delete(A<Film>.Ignored)).Returns(new Film());

            var sut = new FilmsService(uow);

            //act
            var result = sut.DeleteFilm(new Film() { });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task FilmDetails_WhenIsNotAuthenticatedAndFilmIsNull_ThrowsNullReferenceException()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetFilmById(A<int>.Ignored))
                .Returns(Task.FromResult<Film>(null));

            var sut = new FilmsService(uow);

            //act
            Func<int, bool, string, Task<FilmDetailsDTO>> result = async (x, y, z) => await sut.FilmDetails(x, y, z);

            //assert
            await Assert.ThrowsAsync<NullReferenceException>(() => result(1, false, "userID"));
        }

        [Fact]
        public async Task FilmDetails_WhenIsNotAuthenticatedAndFilmIsNotNull_ReturnData()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetFilmById(A<int>.Ignored))
                .Returns(Task.FromResult(new Film()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.FilmDetails(1, false, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.Equal(1, result.Film.Id);
            Assert.Equal("Test", result.Film.Name);
            Assert.NotNull(result.RelatedFilms);
            Assert.False(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task FilmDetails_WhenIsAuthenticatedAndFilmIsNull_ThrowsNullReferenceException()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetFilmById(A<int>.Ignored))
                .Returns(Task.FromResult<Film>(null));

            var sut = new FilmsService(uow);

            //act
            Func<int, bool, string, Task<FilmDetailsDTO>> result = async (x, y, z) => await sut.FilmDetails(x, y, z);

            //assert
            await Assert.ThrowsAsync<NullReferenceException>(() => result(1, true, "userID"));
        }

        [Fact]
        public async Task FilmDetails_WhenIsAuthenticatedAndFilmIsNotNullAndInteractionIsNull_ReturnDataWithInteractionIsFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetFilmById(A<int>.Ignored))
                .Returns(Task.FromResult(new Film()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
              .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.FilmDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.Equal(1, result.Film.Id);
            Assert.Equal("Test", result.Film.Name);
            Assert.False(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
            Assert.NotNull(result.RelatedFilms);
        }

        [Fact]
        public async Task FilmDetails_WhenIsAuthenticatedAndFilmIsNotNullAndInteractionIsNotNull_ReturnDataWithInteraction()
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

            A.CallTo(() => uow.Films.GetFilmById(A<int>.Ignored))
                .Returns(Task.FromResult(new Film()
                {
                    Id = 1,
                    Name = "Test"
                }));

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.FilmDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.Equal(1, result.Film.Id);
            Assert.Equal("Test", result.Film.Name);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
            Assert.NotNull(result.RelatedFilms);
        }

        [Fact]
        public async Task LikeFilm_WhenInteractionIsNull_ReturnIsLikedTrueAndDisLikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Interactions.Add(new Interaction()))
               .Returns(Task.FromResult(new Interaction()));

            A.CallTo(() => uow.Films.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Film()));

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.LikeFilm(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.NotNull(result.RelatedFilms);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task LikeFilm_WhenInteractionIsNotNullAndUserIsDisliked_ReturnIsLikedTrueAndDisLikedFalse()
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

            A.CallTo(() => uow.Films.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Film()));

            A.CallTo(() => uow.Films.GetFilms())
                   .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.LikeFilm(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.NotNull(result.RelatedFilms);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task LikeFilm_WhenInteractionIsNotNullAndUserIsliked_ReturnIsLikedFalse()
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

            A.CallTo(() => uow.Films.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Film()));

            A.CallTo(() => uow.Interactions.Delete(new Interaction()))
               .Returns(new Interaction());

            A.CallTo(() => uow.Films.GetFilms())
                 .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.LikeFilm(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.NotNull(result.RelatedFilms);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeFilm_WhenInteractionIsNull_ReturnIsLikedFalseAndDislikedTrue()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Interactions.Add(new Interaction()))
               .Returns(Task.FromResult(new Interaction()));

            A.CallTo(() => uow.Films.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Film()));

            A.CallTo(() => uow.Films.GetFilms())
                   .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.DisLikeFilm(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.NotNull(result.RelatedFilms);
            Assert.True(result.HasUserDisliked);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeFilm_WhenInteractionIsNotNullAndUserIsLiked_ReturnIsLikedFalseAndDislikedTrue()
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

            A.CallTo(() => uow.Films.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Film()));

            A.CallTo(() => uow.Films.GetFilms())
                 .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.DisLikeFilm(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.NotNull(result.RelatedFilms);
            Assert.True(result.HasUserDisliked);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeFilm_WhenInteractionIsNotNullAndUserIsDisliked_ReturnIsDislikedFalse()
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

            A.CallTo(() => uow.Films.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Film()));

            A.CallTo(() => uow.Interactions.Delete(new Interaction()))
               .Returns(new Interaction());

            A.CallTo(() => uow.Films.GetFilms())
                 .Returns(new List<Film>().AsQueryable());

            var sut = new FilmsService(uow);

            //act
            var result = await sut.DisLikeFilm(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Film);
            Assert.NotNull(result.RelatedFilms);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task GetFilmsForSearch_WhenThrereIsNoFilms_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Films.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Film>>(Enumerable.Empty<Film>()));

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilmsForSearch("Test");

            //assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetFilmsForSearch_WhenThrereIsActors_ReturnActorsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var films = new List<Film>()
            {
                new Film(){Id = 1,Name = "Test 1"},
                new Film(){Id = 2,Name = "Test 2"},
                new Film(){Id = 3,Name = "Test 3"},
                new Film(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Films.GetFilms())
                .Returns(films.AsEnumerable());

            var sut = new FilmsService(uow);

            //act
            var result = sut.GetFilmsForSearch("Test");

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetFilmDataForSelectLists_WhenThrereIsNoData_ReturnEmptyList()
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

            var sut = new FilmsService(uow);

            //act
            var result = await sut.GetFilmDataForSelectLists();

            //assert
            Assert.NotNull(result);
            Assert.False(result.Actors.Any());
            Assert.False(result.Producers.Any());
            Assert.False(result.Categories.Any());
            Assert.False(result.Countries.Any());
        }

        [Fact]
        public async Task GetFilmDataForSelectLists_WhenThrereIsData_ReturnDataList()
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

            var sut = new FilmsService(uow);

            //act
            var result = await sut.GetFilmDataForSelectLists();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Actors);
            Assert.NotNull(result.Producers);
            Assert.NotNull(result.Categories);
            Assert.NotNull(result.Countries);
        }
    }
}