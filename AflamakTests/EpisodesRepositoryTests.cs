namespace AflamakTests
{
    public class EpisodesRepositoryTests
    {
        [Fact]
        public void GetAllEpisodes_WhereValidInputs_ReturnEpisodes()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new EpisodesRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);

            var part = new Part()
            {
                Name = "Part 1",
                EpisodesNo = 5,
                Date = 2024,
                Month = 3,
                TvShowId = tvshow.Id
            };
            context.Parts.Add(part);

            var episodes = new List<Episode>()
            {
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=1,PartId=part.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=2,PartId=part.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=3,PartId=part.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=4,PartId=part.Id}
            };
            context.Episodes.AddRange(episodes);
            context.SaveChanges();

            //Act
            var result = sut.GetAllEpisodes(1, 10);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetAllEpisodes_WhereThePageSizeIsThree_ReturnThreeEpisodes()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new EpisodesRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);

            var part = new Part()
            {
                Name = "Part 1",
                EpisodesNo = 5,
                Date = 2024,
                Month = 3,
                TvShowId = tvshow.Id
            };
            context.Parts.Add(part);

            var episodes = new List<Episode>()
            {
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=1,PartId=part.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=2,PartId=part.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=3,PartId=part.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=4,PartId=part.Id}
            };
            context.Episodes.AddRange(episodes);
            context.SaveChanges();

            //Act
            var result = sut.GetAllEpisodes(1, 3);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAllEpisodes_WhereValidInputAndNoData_ReturnNoData()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new EpisodesRepository(context);

            //Act
            var result = sut.GetAllEpisodes(1, 10);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetFilteredEpisodesWithPartId_WherePartIdIsCorrectAndThereIsRelatedParts_ReturnRelatedTvShows()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new EpisodesRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);

            var part = new Part()
            {
                Name = "Part 1",
                EpisodesNo = 5,
                Date = 2024,
                Month = 3,
                TvShowId = tvshow.Id
            };
            context.Parts.Add(part);

            var episodes = new List<Episode>()
            {
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=1,PartId=part.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=2,PartId=part.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=3,PartId=5},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=4,PartId=5}
            };
            context.Episodes.AddRange(episodes);
            context.SaveChanges();

            //Act
            var result = sut.GetFilteredEpisodesWithPartId(part.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetFilteredEpisodesWithPartId_WherePartIdIsCorrectAndThereIsNoRelatedData_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new EpisodesRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);

            var part = new Part()
            {
                Name = "Part 1",
                EpisodesNo = 5,
                Date = 2024,
                Month = 3,
                TvShowId = tvshow.Id
            };
            context.Parts.Add(part);

            var episodes = new List<Episode>()
            {
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=1,PartId=5},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=2,PartId=2},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=3,PartId=2},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=4,PartId=5}
            };
            context.Episodes.AddRange(episodes);
            context.SaveChanges();

            //Act
            var result = sut.GetFilteredEpisodesWithPartId(100);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetRecentEpisodes_WhereThereIsRecentEpisodes_ReturnRecentEpisodes()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new EpisodesRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);

            var part1 = new Part()
            {
                Name = "Part 1",
                EpisodesNo = 5,
                Date = 2024,
                Month = 11,
                TvShowId = tvshow.Id
            };

            var part2 = new Part()
            {
                Name = "Part 2",
                EpisodesNo = 5,
                Date = 2024,
                Month = 12,
                TvShowId = tvshow.Id
            };
            context.Parts.AddRange(part1, part2);

            var episodes = new List<Episode>()
            {
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=1,PartId=part1.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=2,PartId=part1.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=3,PartId=part1.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=4,PartId=part1.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=1,PartId=part2.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=2,PartId=part2.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=3,PartId=part2.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=4,PartId=part2.Id},
            };
            context.Episodes.AddRange(episodes);
            context.SaveChanges();

            //Act
            var result = sut.GetRecentEpisodes();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Count());
        }

        [Fact]
        public void GetRecentEpisodes_WhereThereIsNoRecentEpisodes_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new EpisodesRepository(context);

            var tvshow = new TvShow()
            {
                Name = "Test",
                Description = "This is description",
                Language = 1,
                Type = 1
            };
            context.TvShows.Add(tvshow);

            var part1 = new Part()
            {
                Name = "Part 1",
                EpisodesNo = 5,
                Date = 2023,
                Month = 12,
                TvShowId = tvshow.Id
            };

            var part2 = new Part()
            {
                Name = "Part 2",
                EpisodesNo = 5,
                Date = 2024,
                Month = 8,
                TvShowId = tvshow.Id
            };
            context.Parts.AddRange(part1, part2);

            var episodes = new List<Episode>()
            {
                new Episode() {Description="This is Description",Date=2023,EpisodeNo=1,PartId=part1.Id},
                new Episode() {Description="This is Description",Date=2023,EpisodeNo=2,PartId=part1.Id},
                new Episode() {Description="This is Description",Date=2023,EpisodeNo=3,PartId=part1.Id},
                new Episode() {Description="This is Description",Date=2023,EpisodeNo=4,PartId=part1.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=1,PartId=part2.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=2,PartId=part2.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=3,PartId=part2.Id},
                new Episode() {Description="This is Description",Date=2024,EpisodeNo=4,PartId=part2.Id},
            };
            context.Episodes.AddRange(episodes);
            context.SaveChanges();

            //Act
            var result = sut.GetRecentEpisodes();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }
    }
}
