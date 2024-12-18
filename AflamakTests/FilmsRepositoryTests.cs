using Aflamak.Data.Enums;
using Aflamak.Models.ViewModels;

namespace AflamakTests
{
    public class FilmsRepositoryTests
    {
        [Fact]
        public void GetFilms_WhenThereIsFilmsExist_ReturnFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetFilms();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetFilms_WhenThereIsNoFilmsExist_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetFilms();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilteredFilmsWithId_WhereTheIdIsValidAndNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetFilteredFilmsWithId(1, "Test");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilteredFilmsWithId_WhereTheIdIsValidTheKeyIsIDAndFilmsExists_ReturnFilteredFilmsWithId()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
             };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetFilteredFilmsWithId(films[0].Id, "ID");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.Equal("Test 1", result.First().Name);
        }

        [Fact]
        public void GetFilteredFilmsWithId_WhereTheIdIsValidTheKeyIsProducerAndFilmsExists_ReturnFilteredFilmsWithProducer()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = 3,CountryId=country.Id,CategoryId=category.Id}
             };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetFilteredFilmsWithId(producer.Id, "Producer");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Test 1", result.First().Name);
            Assert.Equal("Test 2", result.Last().Name);
        }

        [Fact]
        public void GetAllFilms_WhenPageSizeIsTwo_ReturnTwoFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                 new Film{Name = "Test 1",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new Film{Name = "Test 2",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new Film{Name = "Test 3",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetAllFilms(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetAllFilms_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetAllFilms(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetAllFilmsOrderedByDate_WhenPageSizeIsTwo_ReturnTwoFilmsOrderedByDate()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                 new Film{Name = "Test 1",Description="This is description",Year=2024,IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new Film{Name = "Test 2",Description="This is description",Year=2023,IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new Film{Name = "Test 3",Description="This is description",Year=2022,IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetAllFilmsOrderedByDate(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 1", result.First().Name);
            Assert.Equal("Test 2", result.Last().Name);
        }

        [Fact]
        public void GetAllFilmsOrderedByDate_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetAllFilmsOrderedByDate(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetAllFilmsOrderedByLikes_WhenPageSizeIsTwo_ReturnTwoFilmsOrderedByLikes()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                 new Film{Name = "Test 1",Description="This is description",NoOfLikes=100,IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new Film{Name = "Test 2",Description="This is description",NoOfLikes=1000,IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new Film{Name = "Test 3",Description="This is description",NoOfLikes=450,IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetAllFilmsOrderedByLikes(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 2", result.First().Name);
            Assert.Equal("Test 3", result.Last().Name);
        }

        [Fact]
        public void GetAllFilmsOrderedByLikes_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetAllFilmsOrderedByLikes(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilmById_WhenIdIsValidAndFilmExist_ReturnFilm()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                 new Film{Name = "Test 1",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new Film{Name = "Test 2",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new Film{Name = "Test 3",Description="This is description",IsSeries = false,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetFilmById(films[1].Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 2", result.Name);
        }

        [Fact]
        public void GetFilmById_WhenNoFilmsExists_ReturnNull()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetFilmById(1);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void AddFilm_WhenAddValidData_AddSuccessfully()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var actors = new List<Actor>()
            {
            new Actor { Name = "Test 1",AnotherLangName = "1 Test"},
            new Actor { Name = "Test 2",AnotherLangName = "2 Test"},
            new Actor { Name = "Test 3",AnotherLangName = "3 Test"},
            new Actor { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Actors.AddRange(actors);

            context.SaveChanges();

            var film = new FilmViewModel()
            {
                Id = 1,
                Name = "Test 1",
                Description = "This is description",
                IsSeries = true,
                Type = ItemType.فيلم,
                Language = Languages.انجليزي,
                Month = 3,
                Year = 2024,
                NoOfLikes = 100,
                NoOfDisLikes = 0,
                Root = "Ro",
                PartsNo = 3,
                ActorsId = new List<int> { actors[0].Id, actors[1].Id, actors[2].Id },
                Part = 1,
                ProducerId = producer.Id,
                CountryId = country.Id,
                CategoryId = category.Id
            };

            //Act
            sut.AddFilm(film);

            //Assert
            Assert.True(film.Id > 0);
            Assert.True(context.Films.Any());
            Assert.True(context.ActorFilms.Any());
        }

        [Fact]
        public void UpdateFilm_WhenUpdateValidData_UpdateSuccessfully()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var actors = new List<Actor>()
            {
            new Actor { Name = "Test 1",AnotherLangName = "1 Test"},
            new Actor { Name = "Test 2",AnotherLangName = "2 Test"},
            new Actor { Name = "Test 3",AnotherLangName = "3 Test"},
            new Actor { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Actors.AddRange(actors);

            var film = new Film()
            {
                Name = "Test 1",
                Description = "This is description",
                IsSeries = true,
                Type = 1,
                Language = 1,
                Month = 3,
                Year = 2024,
                NoOfLikes = 100,
                NoOfDisLikes = 0,
                Root = "Ro",
                PartsNo = 3,
                Part = 1,
                ProducerId = producer.Id,
                CountryId = country.Id,
                CategoryId = category.Id
            };
            context.Films.Add(film);

            var result = new List<ActorFilms>()
            {
               new ActorFilms {ActorId=actors[0].Id,FilmId=film.Id},
               new ActorFilms {ActorId=actors[1].Id,FilmId=film.Id},
               new ActorFilms {ActorId=actors[2].Id,FilmId=film.Id}
            };
            context.ActorFilms.AddRange(result);

            context.SaveChanges();

            var updatedFilm = new FilmViewModel()
            {
                Id = film.Id,
                Name = "Test 2",
                Description = "This is description",
                IsSeries = true,
                Type = ItemType.فيلم,
                Language = Languages.انجليزي,
                Month = 3,
                Year = 2024,
                NoOfLikes = 100,
                NoOfDisLikes = 0,
                Root = "Ro",
                PartsNo = 3,
                ActorsId = new List<int> { actors[0].Id, actors[1].Id, actors[2].Id },
                Part = 1,
                ProducerId = producer.Id,
                CountryId = country.Id,
                CategoryId = category.Id
            };

            //Act
            sut.UpdateFilm(updatedFilm);

            //Assert
            Assert.Equal("Test 2", context.Films.Find(film.Id).Name);

            var actorFilms = context.ActorFilms.Where(af => af.FilmId == film.Id).ToList();
            Assert.Equal(3, actorFilms.Count);
            Assert.Contains(actorFilms, af => af.ActorId == actors[0].Id);
            Assert.Contains(actorFilms, af => af.ActorId == actors[1].Id);
            Assert.Contains(actorFilms, af => af.ActorId == actors[2].Id);
        }

        [Fact]
        public void MostWatchedFilms_WhereFilmsExists_ReturnMostWatchedFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=260,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=290,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=300,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.MostWatchedFilms().ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 3", result[0].Name);
        }

        [Fact]
        public void MostWatchedFilms_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.MostWatchedFilms();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void RecentFilms_WhereFilmsExists_ReturnRecentFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,Year=2010,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,Year=2024,Month=11,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Year=2024,Month=1,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.RecentFilms().ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 2", result[0].Name);
            Assert.Equal("Test 3", result[1].Name);
        }

        [Fact]
        public void RecentFilms_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.RecentFilms();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void ArabicFilms_WhereFilmsExists_ReturnArabicFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,Language = 1,Type=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,Language = 1,Type=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Language = 2,Type=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.ArabicFilms();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void ArabicFilms_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.ArabicFilms();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void CartoonFilms_WhereFilmsExists_ReturnCartoonFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=260,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=290,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=300,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.CartoonFilms().ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Test 3", result[0].Name);
        }

        [Fact]
        public void CartoonFilms_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.CartoonFilms();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetRecentFilms_WhereFilmsExists_ReturnRecentFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,Year=2024,Month=3,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,Year=2024,Month=12,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Year=2024,Month=1,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Year=2023,Month=10,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Year=2023,Month=3,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetRecentFilms(1).ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.Equal("Test 1", result[0].Name);
        }

        [Fact]
        public void GetRecentFilms_WhereFilmsExistsAndNoMatchFilms_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,Year=2000,Month=3,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,Year=2016,Month=12,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Year=2029,Month=1,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Year=2019,Month=10,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,Year=2019,Month=3,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetRecentFilms(1).ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetRecentFilms_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetRecentFilms(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetTopRatedFilms_WhereFilmsExists_ReturnTopRatedFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=4999,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=5200,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=30000,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetTopRatedFilms(1).ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Test 2", result[0].Name);
        }

        [Fact]
        public void GetTopRatedFilms_WhereFilmsExistsAndNoMatchFilms_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=4999,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=600,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=300,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetTopRatedFilms(1).ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetTopRatedFilms_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetTopRatedFilms(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilmsForSearch_WhereFilmsExists_ReturnMatchingFilms()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=4999,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=5200,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=30000,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "ABC",Description="This is description",IsSeries = false,NoOfLikes=30000,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "XYZ",Description="This is description",IsSeries = false,NoOfLikes=30000,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetFilmsForSearch("Test").ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal("Test 1", result[0].Name);
        }

        [Fact]
        public void GetFilmsForSearch_WhereFilmsExistsAndNoMatchFilms_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var films = new List<Film>
            {
                new Film{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=4999,Type=1,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=600,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new Film{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=300,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.Films.AddRange(films);
            context.SaveChanges();

            //Act
            var result = sut.GetFilmsForSearch("ABC").ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilmsForSearch_WhenNoFilmsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new FilmsRepository(context);

            //Act
            var result = sut.GetFilmsForSearch("Test");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }
    }
}
