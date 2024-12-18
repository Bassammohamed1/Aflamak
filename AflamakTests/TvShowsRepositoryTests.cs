using Aflamak.Data.Enums;
using Aflamak.Models.ViewModels;

namespace AflamakTests
{
    public class TvShowsRepositoryTests
    {
        [Fact]
        public void GetTvShows_WhenThereIsTvShowsExist_ReturnTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetTvShows();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetTvShows_WhenThereIsNoTvShowsExist_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetTvShows();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilteredTvShowsWithId_WhereTheIdIsValidAndNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetFilteredTvShowsWithId(1, "Test");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilteredTvShowsWithId_WhereTheIdIsValidTheKeyIsIDAndTvShowsExists_ReturnFilteredTvShowsWithId()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
             };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetFilteredTvShowsWithId(TvShows[0].Id, "ID");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.Equal("Test 1", result.First().Name);
        }

        [Fact]
        public void GetFilteredTvShowsWithId_WhereTheIdIsValidTheKeyIsProducerAndTvShowsExists_ReturnFilteredTvShowsWithProducer()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = 3,CountryId=country.Id,CategoryId=category.Id}
             };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetFilteredTvShowsWithId(producer.Id, "Producer");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Test 1", result.First().Name);
            Assert.Equal("Test 2", result.Last().Name);
        }

        [Fact]
        public void GetAllTvShows_WhenPageSizeIsTwo_ReturnTwoTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                 new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetAllTvShows(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetAllTvShows_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetAllTvShows(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetAllTvShowsOrderedByDate_WhenPageSizeIsTwo_ReturnTwoTvShowsOrderedByDate()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                 new TvShow{Name = "Test 1",Description="This is description",Year=2024,IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new TvShow{Name = "Test 2",Description="This is description",Year=2023,IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new TvShow{Name = "Test 3",Description="This is description",Year=2022,IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetAllTvShowsOrderedByDate(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 1", result.First().Name);
            Assert.Equal("Test 2", result.Last().Name);
        }

        [Fact]
        public void GetAllTvShowsOrderedByDate_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetAllTvShowsOrderedByDate(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetAllTvShowsOrderedByLikes_WhenPageSizeIsTwo_ReturnTwoTvShowsOrderedByLikes()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                 new TvShow{Name = "Test 1",Description="This is description",NoOfLikes=100,IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new TvShow{Name = "Test 2",Description="This is description",NoOfLikes=1000,IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new TvShow{Name = "Test 3",Description="This is description",NoOfLikes=450,IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetAllTvShowsOrderedByLikes(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 2", result.First().Name);
            Assert.Equal("Test 3", result.Last().Name);
        }

        [Fact]
        public void GetAllTvShowsOrderedByLikes_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetAllTvShowsOrderedByLikes(1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetTvShowById_WhenIdIsValidAndTvShowExist_ReturnTvShow()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                 new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                 new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetTvShowById(TvShows[1].Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 2", result.Name);
        }

        [Fact]
        public void GetTvShowById_WhenNoTvShowsExists_ReturnNull()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetTvShowById(1);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllTvShowsForSelectList_WhenThereIsTvShowsExist_ReturnTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetAllTvShowsForSelectList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAllTvShowsForSelectList_WhenThereIsNoTvShowsExist_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetAllTvShowsForSelectList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void AddTvShow_WhenAddValidData_AddSuccessfully()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

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

            var TvShow = new TvShowViewModel()
            {
                Id = 1,
                Name = "Test 1",
                Description = "This is description",
                IsSeries = true,
                Type = ItemType.مسلسل,
                Language = Languages.انجليزي,
                Month = 3,
                Year = 2024,
                IsRamadan = false,
                NoOfLikes = 100,
                NoOfDisLikes = 0,
                PartsNo = 3,
                ActorsId = new List<int> { actors[0].Id, actors[1].Id, actors[2].Id },
                ProducerId = producer.Id,
                CountryId = country.Id,
                CategoryId = category.Id
            };

            //Act
            sut.AddTvShow(TvShow);

            //Assert
            Assert.True(TvShow.Id > 0);
            Assert.True(context.TvShows.Any());
            Assert.True(context.ActorTvShows.Any());
        }

        [Fact]
        public void UpdateTvShow_WhenUpdateValidData_UpdateSuccessfully()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

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

            var TvShow = new TvShow()
            {
                Name = "Test 1",
                Description = "This is description",
                IsSeries = true,
                Type = 2,
                Language = 1,
                IsRamadan = false,
                Month = 3,
                Year = 2024,
                NoOfLikes = 100,
                NoOfDisLikes = 0,
                PartsNo = 3,
                ProducerId = producer.Id,
                CountryId = country.Id,
                CategoryId = category.Id
            };
            context.TvShows.Add(TvShow);

            var result = new List<ActorTvShows>()
            {
               new ActorTvShows {ActorId=actors[0].Id,TvShowId=TvShow.Id},
               new ActorTvShows {ActorId=actors[1].Id,TvShowId=TvShow.Id},
               new ActorTvShows {ActorId=actors[2].Id,TvShowId=TvShow.Id}
            };
            context.ActorTvShows.AddRange(result);

            context.SaveChanges();

            var updatedTvShow = new TvShowViewModel()
            {
                Id = TvShow.Id,
                Name = "Test 2",
                Description = "This is description",
                IsSeries = true,
                Type = ItemType.مسلسل,
                Language = Languages.انجليزي,
                IsRamadan = false,
                Month = 3,
                Year = 2024,
                NoOfLikes = 100,
                NoOfDisLikes = 0,
                PartsNo = 3,
                ActorsId = new List<int> { actors[0].Id, actors[1].Id, actors[2].Id },
                ProducerId = producer.Id,
                CountryId = country.Id,
                CategoryId = category.Id
            };

            //Act
            sut.UpdateTvShow(updatedTvShow);

            //Assert
            Assert.Equal("Test 2", context.TvShows.Find(TvShow.Id).Name);

            var actorTvShows = context.ActorTvShows.Where(af => af.TvShowId == TvShow.Id).ToList();
            Assert.Equal(3, actorTvShows.Count);
            Assert.Contains(actorTvShows, af => af.ActorId == actors[0].Id);
            Assert.Contains(actorTvShows, af => af.ActorId == actors[1].Id);
            Assert.Contains(actorTvShows, af => af.ActorId == actors[2].Id);
        }

        [Fact]
        public void MostWatchedTvShows_WhereTvShowsExists_ReturnMostWatchedTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=260,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=290,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=300,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.MostWatchedTvShows().ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 3", result[0].Name);
        }

        [Fact]
        public void MostWatchedTvShows_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.MostWatchedTvShows();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void RecentTvShows_WhereTvShowsExists_ReturnRecentTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Year=2010,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Year=2024,Month=11,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Year=2024,Month=1,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.RecentTvShows().ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 2", result[0].Name);
            Assert.Equal("Test 3", result[1].Name);
        }

        [Fact]
        public void RecentTvShows_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.RecentTvShows();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void ArabicTvShows_WhereTvShowsExists_ReturnArabicTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Language = 1,Type=2,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Language = 1,Type=2,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Language = 2,Type=2,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.ArabicTvShows();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void ArabicTvShows_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.ArabicTvShows();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void RamadanTvShows_WhereTvShowsExists_ReturnRamadanTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = true,IsRamadan=false,Year=2009,Month=2,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,IsRamadan=true,Year=2024,Month=8,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,IsRamadan=true,Year=2024,Month=5,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 4",Description="This is description",IsSeries = false,IsRamadan=true,Year=2023,Month=11,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 5",Description="This is description",IsSeries = false,IsRamadan=true,Year=2023,Month=2,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.RamadanTvShows().ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Test 3", result[0].Name);
        }

        [Fact]
        public void RamadanTvShows_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.RamadanTvShows();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetRecentTvShows_WhereTvShowsExists_ReturnRecentTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Year=2024,Month=3,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Year=2024,Month=12,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Year=2024,Month=1,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Year=2023,Month=10,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Year=2023,Month=3,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetRecentTvShows(1).ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.Equal("Test 1", result[0].Name);
        }

        [Fact]
        public void GetRecentTvShows_WhereTvShowsExistsAndNoMatchTvShows_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,Year=2000,Month=3,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,Year=2016,Month=12,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Year=2029,Month=1,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Year=2019,Month=10,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,Year=2019,Month=3,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetRecentTvShows(1).ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetRecentTvShows_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetRecentTvShows(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetTopRatedTvShows_WhereTvShowsExists_ReturnTopRatedTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=4999,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=5200,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=30000,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetTopRatedTvShows(1).ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Test 2", result[0].Name);
        }

        [Fact]
        public void GetTopRatedTvShows_WhereTvShowsExistsAndNoMatchTvShows_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=4999,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=600,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=300,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetTopRatedTvShows(1).ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetTopRatedTvShows_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetTopRatedTvShows(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetTvShowsForSearch_WhereTvShowsExists_ReturnMatchingTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=4999,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=5200,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=30000,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "ABC",Description="This is description",IsSeries = false,NoOfLikes=30000,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "XYZ",Description="This is description",IsSeries = false,NoOfLikes=30000,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetTvShowsForSearch("Test").ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal("Test 1", result[0].Name);
        }

        [Fact]
        public void GetTvShowsForSearch_WhereTvShowsExistsAndNoMatchTvShows_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            var producer = new Producer { Name = "Test Pr", AnotherLangName = "Pr Test" };
            context.Producers.Add(producer);

            var country = new Country { Name = "Test Co" };
            context.Countries.Add(country);

            var category = new Category { Name = "Test Ca" };
            context.Categories.Add(category);

            var TvShows = new List<TvShow>
            {
                new TvShow{Name = "Test 1",Description="This is description",IsSeries = false,NoOfLikes=4999,Type=2,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 2",Description="This is description",IsSeries = false,NoOfLikes=600,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id},
                new TvShow{Name = "Test 3",Description="This is description",IsSeries = false,NoOfLikes=300,Type=3,Language=1,ProducerId = producer.Id,CountryId=country.Id,CategoryId=category.Id}
            };
            context.TvShows.AddRange(TvShows);
            context.SaveChanges();

            //Act
            var result = sut.GetTvShowsForSearch("ABC").ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetTvShowsForSearch_WhenNoTvShowsExists_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new TvShowsRepository(context);

            //Act
            var result = sut.GetTvShowsForSearch("Test");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }
    }
}
