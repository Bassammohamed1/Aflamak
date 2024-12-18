namespace AflamakTests
{
    public class RepositoryTests
    {
        [Fact]
        public void GetAll_WhereValidInputs_ReturnData()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

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
            var result = sut.GetAll(1, 10);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetAll_WhereValidInputsAnNoData_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

            //Act
            var result = sut.GetAll(1, 10);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetAll_WherePageSizeIsThree_ReturnThreeActors()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

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
            var result = sut.GetAll(1, 3);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAllWithoutPagination_WhereValidInputs_ReturnData()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

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
            var result = sut.GetAllWithoutPagination();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetAllWithoutPagination_WhereValidInputsAnNoData_ReturnEmptyList()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

            //Act
            var result = sut.GetAllWithoutPagination();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void GetById_WhereValidId_ReturnData()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

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
            var result = sut.GetById(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Test 1", result.Name);
        }

        [Fact]
        public void GetById_WhereValidIdAndNoData_ReturnNull()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

            //Act
            var result = sut.GetById(1);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetById_WhereValidIdAndNoDataMatchesTheId_ReturnNull()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

            var actors = new List<Actor>()
            {
            new Actor { Name = "Test 1",AnotherLangName = "1 Test"},
            new Actor { Name = "Test 2",AnotherLangName = "2 Test"},
            new Actor { Name = "Test 3",AnotherLangName = "3 Test"},
            new Actor { Name = "Test 4",AnotherLangName = "4 Test" }
            };

            //Act
            var result = sut.GetById(10);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Add_WhenAddValidData_AddSuccessfully()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

            var actor = new Actor()
            {
                Name = "Test",
                AnotherLangName = "Test"
            };

            //Act
            sut.Add(actor);

            //Assert
            Assert.True(actor.Id > 0);
        }

        [Fact]
        public void Update_WhenUpdateValidData_UpdateSuccessfully()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

            var actor = new Actor()
            {
                Name = "Test",
                AnotherLangName = "Test"
            };


            //Act
            sut.Update(actor);

            //Assert
            Assert.True(actor.Id > 0);
        }

        [Fact]
        public void Delete_WhenDeleteValidData_DeleteSuccessfully()
        {
            //Arrange
            var context = new InMemoryDB();
            var sut = new Repository<Actor>(context);

            var actor = new Actor()
            {
                Name = "Test",
                AnotherLangName = "Test"
            };


            //Act
            sut.Delete(actor);

            //Assert
            Assert.True(actor.Id > 0);
        }
    }
}
