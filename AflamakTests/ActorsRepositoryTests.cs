namespace AflamakTests
{
    public class ActorsRepositoryTests
    {
        [Fact]
        public void GetActorsForSearch_WhereTheKeyIsValidAndGoToName_ReturnActor()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new ActorsRepository(context);

            var actors = new List<Actor>()
            {
            new Actor { Name = "Test 1",AnotherLangName = "1 Test"},
            new Actor { Name = "Test 2",AnotherLangName = "2 Test"},
            new Actor { Name = "Test 3",AnotherLangName = "3 Test"},
            new Actor { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Actors.AddRange(actors);
            context.SaveChanges();

            //Act 
            var result = sut.GetActorsForSearch("Test 3");

            //Assert 
            Assert.NotNull(result);
            Assert.Contains("Test 3", result.First().Name);
            Assert.Contains("3 Test", result.First().AnotherLangName);
        }

        [Fact]
        public void GetActorsForSearch_WhereTheKeyIsValidAndGoToAnotherLangName_ReturnActor()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new ActorsRepository(context);

            var actors = new List<Actor>()
            {
            new Actor { Name = "Test 1",AnotherLangName = "1 Test"},
            new Actor { Name = "Test 2",AnotherLangName = "2 Test"},
            new Actor { Name = "Test 3",AnotherLangName = "3 Test"},
            new Actor { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Actors.AddRange(actors);
            context.SaveChanges();

            //Act 
            var result = sut.GetActorsForSearch("2 Test");

            //Assert 
            Assert.NotNull(result);
            Assert.Contains("2 Test", result.First().AnotherLangName);
            Assert.Contains("Test 2", result.First().Name);
        }

        [Fact]
        public void GetActorsForSearch_WhereTheKeyIsValid_ReturnActors()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new ActorsRepository(context);

            var actors = new List<Actor>()
            {
            new Actor { Name = "Test 1",AnotherLangName = "1 Test"},
            new Actor { Name = "Test 2",AnotherLangName = "2 Test"},
            new Actor { Name = "Test 3",AnotherLangName = "3 Test"},
            new Actor { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Actors.AddRange(actors);
            context.SaveChanges();

            //Act 
            var result = sut.GetActorsForSearch("Test").ToList();

            //Assert 
            Assert.NotNull(result);
            Assert.Contains("Test 1", result.First().Name);
            Assert.Contains("Test 2", result[1].Name);
            Assert.Contains("Test 3", result[2].Name);
            Assert.Contains("Test 4", result.Last().Name);
        }

        [Fact]
        public void GetActorsForSearch_WhereTheKeyIsValidAndNoMatchResult_ReturnNoData()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new ActorsRepository(context);

            var actors = new List<Actor>()
            {
            new Actor { Name = "Test 1",AnotherLangName = "1 Test"},
            new Actor { Name = "Test 2",AnotherLangName = "2 Test"},
            new Actor { Name = "Test 3",AnotherLangName = "3 Test"},
            new Actor { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Actors.AddRange(actors);
            context.SaveChanges();

            //Act 
            var result = sut.GetActorsForSearch("Hello");

            //Assert 
            Assert.False(result.Any());
        }
    }
}