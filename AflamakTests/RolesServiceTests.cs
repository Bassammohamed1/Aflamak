using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace AflamakTests
{
    public class RolesServiceTests
    {
        [Fact]
        public void AllRoles_WhenThereIsNoRoles_ReturnEmptyList()
        {
            //arrange
            var roleManager = A.Fake<RoleManager<IdentityRole>>();

            A.CallTo(() => roleManager.Roles).Returns(Enumerable.Empty<IdentityRole>().AsQueryable());

            var sut = new RolesService(roleManager);

            //act
            var result = sut.AllRoles();

            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AllRoles_WhenThereIsRoles_ReturnRolesList()
        {
            //arrange
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

            A.CallTo(() => roleManager.Roles)
                .Returns(roles.AsQueryable());

            var sut = new RolesService(roleManager);

            //act
            var result = sut.AllRoles();

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateRole_WhenRoleExists_ReturnResultWithSuccedFalse()
        {
            //arrange
            var roleManager = A.Fake<RoleManager<IdentityRole>>();

            A.CallTo(() => roleManager.RoleExistsAsync(A<string>.Ignored))
                .Returns(Task.FromResult(true));

            var sut = new RolesService(roleManager);

            //act
            var result = await sut.CreateRole("Role");

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("هذا الدور موجود بالفعل.", result.Error);
        }

        [Fact]
        public async Task CreateRole_WhenRoleDosenotExist_ReturnResultWithSuccedTrueAndCreateRole()
        {
            //arrange
            var roleManager = A.Fake<RoleManager<IdentityRole>>();

            A.CallTo(() => roleManager.RoleExistsAsync(A<string>.Ignored))
                .Returns(Task.FromResult(false));

            A.CallTo(() => roleManager.CreateAsync(A<IdentityRole>.Ignored))
                .Returns(Task.FromResult(new IdentityResult()));


            var sut = new RolesService(roleManager);

            //act
            var result = await sut.CreateRole("Role");

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}
