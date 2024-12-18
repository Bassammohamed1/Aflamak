namespace AflamakTests
{
    public class ProducersRepositoryTests
    {
        [Fact]
        public void GetProducersForSearch_WhereTheKeyIsValidAndGoToName_ReturnProducer()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new ProducersRepository(context);

            var producers = new List<Producer>()
            {
            new Producer { Name = "Test 1",AnotherLangName = "1 Test"},
            new Producer { Name = "Test 2",AnotherLangName = "2 Test"},
            new Producer { Name = "Test 3",AnotherLangName = "3 Test"},
            new Producer { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Producers.AddRange(producers);
            context.SaveChanges();

            //Act 
            var result = sut.GetProducersForSearch("Test 3");

            //Assert 
            Assert.NotNull(result);
            Assert.Contains("Test 3", result.First().Name);
            Assert.Contains("3 Test", result.First().AnotherLangName);
        }

        [Fact]
        public void GetProducersForSearch_WhereTheKeyIsValidAndGoToAnotherLangName_ReturnProducer()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new ProducersRepository(context);

            var producers = new List<Producer>()
            {
            new Producer { Name = "Test 1",AnotherLangName = "1 Test"},
            new Producer { Name = "Test 2",AnotherLangName = "2 Test"},
            new Producer { Name = "Test 3",AnotherLangName = "3 Test"},
            new Producer { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Producers.AddRange(producers);
            context.SaveChanges();

            //Act 
            var result = sut.GetProducersForSearch("2 Test");

            //Assert 
            Assert.NotNull(result);
            Assert.Contains("2 Test", result.First().AnotherLangName);
            Assert.Contains("Test 2", result.First().Name);
        }

        [Fact]
        public void GetProducersForSearch_WhereTheKeyIsValid_ReturnProducers()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new ProducersRepository(context);

            var producers = new List<Producer>()
            {
            new Producer { Name = "Test 1",AnotherLangName = "1 Test"},
            new Producer { Name = "Test 2",AnotherLangName = "2 Test"},
            new Producer { Name = "Test 3",AnotherLangName = "3 Test"},
            new Producer { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Producers.AddRange(producers);
            context.SaveChanges();

            //Act 
            var result = sut.GetProducersForSearch("Test").ToList();

            //Assert 
            Assert.NotNull(result);
            Assert.Contains("Test 1", result.First().Name);
            Assert.Contains("Test 2", result[1].Name);
            Assert.Contains("Test 3", result[2].Name);
            Assert.Contains("Test 4", result.Last().Name);
        }

        [Fact]
        public void GetProducersForSearch_WhereTheKeyIsValidAndNoMatchResult_ReturnNoData()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new ProducersRepository(context);

            var producers = new List<Producer>()
            {
            new Producer { Name = "Test 1",AnotherLangName = "1 Test"},
            new Producer { Name = "Test 2",AnotherLangName = "2 Test"},
            new Producer { Name = "Test 3",AnotherLangName = "3 Test"},
            new Producer { Name = "Test 4",AnotherLangName = "4 Test" }
            };
            context.Producers.AddRange(producers);
            context.SaveChanges(); 

            //Act 
            var result = sut.GetProducersForSearch("Hello");

            //Assert 
            Assert.False(result.Any());
        }
    }
}
