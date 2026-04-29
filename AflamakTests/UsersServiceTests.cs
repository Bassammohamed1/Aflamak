using BusinessLogicLayer.DTOs;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace AflamakTests
{
    public class UsersServiceTests
    {
        [Fact]
        public void AllUsers_WhenThereIsNoUsers_ReturnEmptyList()
        {
            //arrange
            var userManager = A.Fake<UserManager<IdentityUser>>();

            A.CallTo(() => userManager.Users).Returns(Enumerable.Empty<IdentityUser>().AsQueryable());

            var sut = new UsersService(userManager, null);

            //act
            var result = sut.AllUsers();

            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AllUsers_WhenThereIsUsers_ReturnUsersList()
        {
            //arrange
            var userManager = A.Fake<UserManager<IdentityUser>>();

            var users = new List<IdentityUser>()
            {
                new IdentityUser
                {
                    Id = "1",
                    UserName = "Test 1"
                },
                new IdentityUser
                {
                    Id = "2",
                    UserName = "Test 2"
                }
            };

            A.CallTo(() => userManager.Users)
                .Returns(users.AsQueryable());

            var sut = new UsersService(userManager, null);

            //act
            var result = sut.AllUsers();

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllRolesWithUserSelectedRoles_WhenRolesExists_ReturnData()
        {
            //arrange
            var userManager = A.Fake<UserManager<IdentityUser>>();
            var roleManager = A.Fake<RoleManager<IdentityRole>>();

            var roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = "Test 1"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Test 2"
                }
            };

            A.CallTo(() => userManager.FindByIdAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new IdentityUser()
                {
                    Id = "1",
                    UserName = "Test"
                }));

            A.CallTo(() => roleManager.Roles)
                .Returns(roles.AsQueryable());

            A.CallTo(() => userManager.IsInRoleAsync(A<IdentityUser>.Ignored, A<string>.Ignored))
               .Returns(Task.FromResult(true));

            var sut = new UsersService(userManager, roleManager);

            //act
            var result = await sut.GetAllRolesWithUserSelectedRoles("1");

            //assert
            Assert.NotNull(result);
            Assert.Equal("1", result.UserId);
            Assert.Equal("Test", result.UserName);
            Assert.NotNull(result.Roles);
            Assert.Equal(2, result.Roles.Count());
        }

        [Fact]
        public async Task ManageRoles_WhenUserIsNull_ReturnResultWithError()
        {
            //arrange
            var userManager = A.Fake<UserManager<IdentityUser>>();

            A.CallTo(() => userManager.FindByIdAsync(A<string>.Ignored))
                .Returns(Task.FromResult<IdentityUser>(null));

            var sut = new UsersService(userManager, null);

            //act
            var result = await sut.ManageRoles(new UserRolesDTO());

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("User is null.", result.Error);
        }

        [Fact]
        public async Task ManageRoles_WhenUserIsNotNull_ReturnResultWithSucceed()
        {
            //arrange
            var userManager = A.Fake<UserManager<IdentityUser>>();

            A.CallTo(() => userManager.FindByIdAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new IdentityUser()));

            A.CallTo(() => userManager.GetRolesAsync(A<IdentityUser>.Ignored))
                .Returns(new List<string>());

            var sut = new UsersService(userManager, null);

            //act
            var result = await sut.ManageRoles(new UserRolesDTO() { UserId = "1", UserName = "Test", Roles = new List<RolesDTO>() });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}
