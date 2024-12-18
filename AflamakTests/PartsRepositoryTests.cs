namespace AflamakTests
{
    public class PartsRepositoryTests
    {
        [Fact]
        public void GetAllParts_WhereValidInputs_ReturnParts()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new PartsRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);
            context.SaveChanges();

            var parts = new List<Part>()
            {
                new Part(){Name="Part 1",EpisodesNo = 5,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 2",EpisodesNo = 10,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 3",EpisodesNo = 20,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 4",EpisodesNo = 15,Date = 2024,Month = 3,TvShowId = tvshow.Id},
            };
            context.Parts.AddRange(parts);
            context.SaveChanges();

            //Act
            var result = sut.GetAllParts(1, 10);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetAllParts_WhereThePageSizeIsThree_ReturnThreeParts()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new PartsRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);
            context.SaveChanges();

            var parts = new List<Part>()
            {
                new Part(){Name="Part 1",EpisodesNo = 5,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 2",EpisodesNo = 10,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 3",EpisodesNo = 20,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 4",EpisodesNo = 15,Date = 2024,Month = 3,TvShowId = tvshow.Id},
            };
            context.Parts.AddRange(parts);
            context.SaveChanges();

            //Act
            var result = sut.GetAllParts(1, 3);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAllParts_WhereValidInputAndNoData_ReturnNoData()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new PartsRepository(context);

            //Act
            var result = sut.GetAllParts(1, 10);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetAllPartsForSelectList_WhereExistData_ReturnParts()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new PartsRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);
            context.SaveChanges();

            var parts = new List<Part>()
            {
                new Part(){Name="Part 1",EpisodesNo = 5,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 2",EpisodesNo = 10,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 3",EpisodesNo = 20,Date = 2024,Month = 3,TvShowId = tvshow.Id},
                new Part(){Name="Part 4",EpisodesNo = 15,Date = 2024,Month = 3,TvShowId = tvshow.Id},
            };
            context.Parts.AddRange(parts);
            context.SaveChanges();

            //Act
            var result = sut.GetAllPartsForSelectList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetAllPartsForSelectList_WhereNoData_ReturnNoData()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new PartsRepository(context);

            //Act
            var result = sut.GetAllPartsForSelectList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilteredPartsWithTvShowId_WhereTvShowIdIsCorrectAndThereIsRelatedParts_ReturnRelatedParts()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new PartsRepository(context);

            var tvshow1 = new TvShow()
            {
                Name = "Test 1",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            var tvshow2 = new TvShow()
            {
                Name = "Test 2",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.AddRange(tvshow1, tvshow2);
            context.SaveChanges();

            var parts = new List<Part>()
            {
                new Part(){Name="Part 1",EpisodesNo = 5,Date = 2024,Month = 3,TvShowId = tvshow1.Id},
                new Part(){Name="Part 2",EpisodesNo = 10,Date = 2024,Month = 3,TvShowId = tvshow1.Id},
                new Part(){Name="Part 3",EpisodesNo = 20,Date = 2024,Month = 3,TvShowId = tvshow2.Id},
                new Part(){Name="Part 4",EpisodesNo = 15,Date = 2024,Month = 3,TvShowId = tvshow2.Id},
            };
            context.Parts.AddRange(parts);
            context.SaveChanges();

            //Act
            var result = sut.GetFilteredPartsWithTvShowId(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredPartsWithTvShowId_WhereTvShowIdIsCorrectAndThereIsNoRelatedData_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new PartsRepository(context);

            var tvshow1 = new TvShow()
            {
                Name = "Test 1",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            var tvshow2 = new TvShow()
            {
                Name = "Test 2",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.AddRange(tvshow1, tvshow2);
            context.SaveChanges();

            var parts = new List<Part>()
            {
                new Part(){Name="Part 1",EpisodesNo = 5,Date = 2024,Month = 3,TvShowId = tvshow1.Id},
                new Part(){Name="Part 2",EpisodesNo = 10,Date = 2024,Month = 3,TvShowId = tvshow1.Id},
                new Part(){Name="Part 3",EpisodesNo = 20,Date = 2024,Month = 3,TvShowId = tvshow2.Id},
                new Part(){Name="Part 4",EpisodesNo = 15,Date = 2024,Month = 3,TvShowId = tvshow2.Id},
            };
            context.Parts.AddRange(parts);
            context.SaveChanges();

            //Act
            var result = sut.GetFilteredPartsWithTvShowId(100);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }
    }
}
