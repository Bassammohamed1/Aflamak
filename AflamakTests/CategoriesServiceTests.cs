using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;

namespace AflamakTests
{
    public class CategoriesServiceTests
    {
        [Fact]
        public async Task GetCategoryByID_WhenIDIsZero_ReturnNull()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Categories.GetById(A<int>.That.IsEqualTo(0))).Returns(Task.FromResult<Category>(null));

            var sut = new CategoriesService(uow);

            //act
            var result = await sut.GetCategoryByID(0);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCategoryByID_WhenIDIsValid_ReturnCategory()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Category = new Category
            {
                Id = 1,
                Name = "Test"
            };

            A.CallTo(() => uow.Categories.GetById(A<int>.Ignored)).Returns(Task.FromResult(Category));

            var sut = new CategoriesService(uow);

            //act
            var result = await sut.GetCategoryByID(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Test");
        }

        [Fact]
        public async Task GetAllCategories_WhenThereIsNoCategories_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Categories.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<IEnumerable<Category>>(Enumerable.Empty<Category>()));

            var sut = new CategoriesService(uow);

            //act

            var result = await sut.GetAllCategories(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAllCategories_WhenThereIsCategories_ReturnCategoriesList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Categories = new List<Category>()
            {
                new Category(){Id = 1,Name = "Test 1"},
                new Category(){Id = 2,Name = "Test 2"},
                new Category(){Id = 3,Name = "Test 3"},
                new Category(){Id = 4,Name = "Test 4"}
            };

            A.CallTo(() => uow.Categories.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult(Categories.AsEnumerable()));

            var sut = new CategoriesService(uow);

            //act

            var result = await sut.GetAllCategories(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task AddCategory_WhenThereIsAnErrorWhileAdding_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Categories.Add(A<Category>.Ignored))
                .Returns(Task.FromResult<Category>(null));

            var sut = new CategoriesService(uow);

            //act
            var result = await sut.AddCategory(new Category());

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while adding.", result.Error);
        }

        [Fact]
        public async Task AddCategory_WhenThereIsNoErrorsWhileAdding_AddSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Categories.Add(A<Category>.Ignored))
                .Returns(Task.FromResult<Category>(new Category()));

            var sut = new CategoriesService(uow);

            //act
            var result = await sut.AddCategory(new Category());

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateCategory_WhenThereIsAnErrorWhileUpdating_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Categories.Update(A<Category>.Ignored)).Returns(null);

            var sut = new CategoriesService(uow);

            //act
            var result = await sut.UpdateCategory(new Category());

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while updating.", result.Error);
        }

        [Fact]
        public async Task UpdateCategory_WhenThereIsNoErrorsWhileUpdating_UpdateSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Categories.Update(A<Category>.Ignored)).Returns(new Category());

            var sut = new CategoriesService(uow);

            //act
            var result = await sut.UpdateCategory(new Category());

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteCategory_WhenThereIsAnErrorWhileDeleting_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Categories.Delete(A<Category>.Ignored)).Returns(null);

            var sut = new CategoriesService(uow);

            //act
            var result = await sut.DeleteCategory(new Category());

            //assert
            Assert.NotNull(result);
            Assert.Equal("An error ouccered while deleting.", result.Error);
        }

        [Fact]
        public async Task DeleteCategory_WhenThereIsNoErrorsWhileDeleting_DeleteSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Categories.Delete(A<Category>.Ignored)).Returns(new Category());

            var sut = new CategoriesService(uow);

            //act
            var result = await sut.DeleteCategory(new Category() { });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}