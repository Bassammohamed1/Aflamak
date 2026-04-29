using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace AflamakTests
{
    public class EpisodesServiceTests
    {
        [Fact]
        public async Task GetEpisodeByID_WhenIDIsZero_ReturnNull()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.GetById(A<int>.That.IsEqualTo(0))).Returns(Task.FromResult<Episode>(null));

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.GetEpisodeByID(0);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEpisodeByID_WhenIDIsValid_ReturnEpisode()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Episode = new Episode
            {
                Id = 1
            };

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored)).Returns(Task.FromResult(Episode));

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.GetEpisodeByID(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public async Task GetAllEpisodes_WhenThereIsNoEpisodes_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult<IEnumerable<Episode>>(Enumerable.Empty<Episode>()));

            var sut = new EpisodesService(uow);

            //act

            var result = await sut.GetAllEpisodes(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAllEpisodes_WhenThereIsEpisodes_ReturnEpisodesList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var Episodes = new List<Episode>()
            {
                new Episode(){Id = 1 },
                new Episode(){Id = 2 },
                new Episode(){Id = 3 },
                new Episode(){Id = 4 }
            };

            A.CallTo(() => uow.Episodes.GetAll(A<int>.Ignored, A<int>.Ignored))
                .Returns(Task.FromResult(Episodes.AsEnumerable()));

            var sut = new EpisodesService(uow);

            //act

            var result = await sut.GetAllEpisodes(1, 9);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 4);
        }

        [Fact]
        public async Task AddEpisode_WhenThereIsAnErrorWhileAdding_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.Add(A<Episode>.Ignored))
                .Returns(Task.FromResult<Episode>(null));

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.AddEpisode(new Episode());

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while adding.", result.Error);
        }

        [Fact]
        public async Task AddEpisode_WhenThereIsNoErrorsWhileAdding_AddSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.Add(A<Episode>.Ignored))
                .Returns(Task.FromResult<Episode>(new Episode()));

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.AddEpisode(new Episode());

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateEpisode_WhenThereIsAnErrorWhileUpdating_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.Update(A<Episode>.Ignored)).Returns(null);

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.UpdateEpisode(new Episode());

            //assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("An error ouccered while updating.", result.Error);
        }

        [Fact]
        public async Task UpdateEpisode_WhenThereIsNoErrorsWhileUpdating_UpdateSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.Update(A<Episode>.Ignored)).Returns(new Episode());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.UpdateEpisode(new Episode());

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteEpisode_WhenThereIsAnErrorWhileDeleting_ReturnFalseInResultSucceed()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.Delete(A<Episode>.Ignored)).Returns(null);

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.DeleteEpisode(new Episode());

            //assert
            Assert.NotNull(result);
            Assert.Equal("An error ouccered while deleting.", result.Error);
        }

        [Fact]
        public async Task DeleteEpisode_WhenThereIsNoErrorsWhileDeleting_DeleteSuccessfully()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.Delete(A<Episode>.Ignored)).Returns(new Episode());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.DeleteEpisode(new Episode() { });

            //assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetRecentEpisodes_WhenThereIsEpisodes_ReturnEpisodes()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();
            var context = new InMemoryDB();

            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var parts = new List<Part>()
            {
                new Part()
                {
                    Id = 1,
                    Name = "Test1",
                    EpisodesNo = 10,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    TvShowId = 1,
                    Date = 2026,
                    Month = 4,
                    clientFile = file
                },
                new Part()
                {
                    Id = 2,
                    Name = "Test2",
                    EpisodesNo = 10,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    TvShowId = 1,
                    Date = 2026,
                    Month = 3,
                    clientFile = file
                },
                new Part()
                {
                    Id = 3,
                    Name = "Test3",
                    EpisodesNo = 10,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    TvShowId = 1,
                    Date = 2025,
                    Month = 4,
                    clientFile = file
                }
            };

            context.Parts.AddRange(parts);
            context.SaveChanges();

            var Episodes = new List<Episode>()
            {
                new Episode()
                {
                    Id = 1,
                    PartId = 1,
                    EpisodeNo = 1,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    Date = 2026,
                    Description = "test"
                },
                new Episode()
                {
                    Id = 2,
                    PartId = 2,
                    EpisodeNo = 2,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    Date = 2026,
                    Description = "test"
                },
                new Episode()
                {
                    Id = 3,
                    PartId = 1,
                    EpisodeNo = 3,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    Date = 2026,
                    Description = "test"
                },
                new Episode()
                {
                    Id = 4,
                    PartId= 3,
                    EpisodeNo = 4,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    Date = 2025,
                    Description = "test"
                }
            };

            context.Episodes.AddRange(Episodes);
            context.SaveChanges();

            A.CallTo(() => uow.Episodes.GetAllEpisodes())
                .Returns(context.Episodes);

            var sut = new EpisodesService(uow);

            //act
            var result = sut.GetRecentEpisodes(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Episodes.Any());
            Assert.Equal(3, result.Episodes.Count());
        }

        [Fact]
        public async Task LoadMoreEpisodes_WhenThereIsEpisodes_ReturnEpisodes()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();
            var context = new InMemoryDB();

            var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var parts = new List<Part>()
            {
                new Part()
                {
                    Id = 1,
                    Name = "Test1",
                    EpisodesNo = 10,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    TvShowId = 1,
                    Date = 2026,
                    Month = 4,
                    clientFile = file
                },
                new Part()
                {
                    Id = 2,
                    Name = "Test2",
                    EpisodesNo = 10,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    TvShowId = 1,
                    Date = 2026,
                    Month = 3,
                    clientFile = file
                },
                new Part()
                {
                    Id = 3,
                    Name = "Test3",
                    EpisodesNo = 10,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    TvShowId = 1,
                    Date = 2025,
                    Month = 4,
                    clientFile = file
                }
            };

            context.Parts.AddRange(parts);
            context.SaveChanges();

            var Episodes = new List<Episode>()
            {
                new Episode()
                {
                    Id = 1,
                    PartId = 1,
                    EpisodeNo = 1,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    Date = 2026,
                    Description = "test"
                },
                new Episode()
                {
                    Id = 2,
                    PartId = 2,
                    EpisodeNo = 2,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    Date = 2026,
                    Description = "test"
                },
                new Episode()
                {
                    Id = 3,
                    PartId = 1,
                    EpisodeNo = 3,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    Date = 2026,
                    Description = "test"
                },
                new Episode()
                {
                    Id = 4,
                    PartId= 3,
                    EpisodeNo = 4,
                    NoOfLikes = 10,
                    NoOfDisLikes = 0,
                    Date = 2025,
                    Description = "test"
                }
            };

            context.Episodes.AddRange(Episodes);
            context.SaveChanges();

            A.CallTo(() => uow.Episodes.GetAllEpisodes())
                .Returns(context.Episodes);

            var sut = new EpisodesService(uow);

            //act
            var result = sut.LoadMoreEpisodes(1);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Episodes.Any());
            Assert.Equal(3, result.Episodes.Count());
        }

        [Fact]
        public async Task EpisodeDetails_WhenIsNotAuthenticatedAndEpisodeIsNull_ReturnEmptyObject()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
                .Returns(Task.FromResult<Episode>(null));

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.EpisodeDetails(1, false, "userID");

            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EpisodeDetails_WhenIsNotAuthenticatedAndEpisodeIsNotNull_ReturnData()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
                .Returns(Task.FromResult(new Episode()
                {
                    Id = 1
                }));

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
                .Returns(new List<Episode>().AsQueryable());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.EpisodeDetails(1, false, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.Equal(1, result.Episode.Id);
            Assert.NotNull(result.Episodes);
        }

        [Fact]
        public async Task EpisodeDetails_WhenIsAuthenticatedAndEpisodeIsNull_ReturnEmptyObject()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
                .Returns(Task.FromResult<Episode>(null));

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.EpisodeDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EpisodeDetails_WhenIsAuthenticatedAndEpisodeIsNotNullAndInteractionIsNull_ReturnDataWithInteractionIsFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
                .Returns(Task.FromResult(new Episode()
                {
                    Id = 1
                }));

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
              .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
                .Returns(new List<Episode>().AsQueryable());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.EpisodeDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.Equal(1, result.Episode.Id);
            Assert.False(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
            Assert.NotNull(result.Episodes);
        }

        [Fact]
        public async Task EpisodeDetails_WhenIsAuthenticatedAndEpisodeIsNotNullAndInteractionIsNotNull_ReturnDataWithInteraction()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    Id = 1,
                    IsLiked = true,
                    IsDisLiked = false,
                    UserId = "userID",
                    ItemId = 1
                }
            };

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
                .Returns(Task.FromResult(new Episode()
                {
                    Id = 1
                }));

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
                .Returns(new List<Episode>().AsQueryable());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.EpisodeDetails(1, true, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.Equal(1, result.Episode.Id);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
            Assert.NotNull(result.Episodes);
        }

        [Fact]
        public async Task LikeEpisode_WhenInteractionIsNull_ReturnIsLikedTrueAndDisLikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Interactions.Add(new Interaction()))
               .Returns(Task.FromResult(new Interaction()));

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Episode()));

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.LikeEpisode(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.NotNull(result.Episodes);
            Assert.NotNull(result.Episodes);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task LikeEpisode_WhenInteractionIsNotNullAndUserIsDisliked_ReturnIsLikedTrueAndDisLikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    ItemId = 1,
                    UserId= "userID",
                    IsDisLiked =true
                }
            };

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Episode()));

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.LikeEpisode(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.NotNull(result.Episodes);
            Assert.NotNull(result.Episodes);
            Assert.True(result.HasUserLiked);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task LikeEpisode_WhenInteractionIsNotNullAndUserIsliked_ReturnIsLikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    ItemId = 1,
                    UserId= "userID",
                    IsLiked = true
                }
            };

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Episode()));

            A.CallTo(() => uow.Interactions.Delete(new Interaction()))
               .Returns(new Interaction());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.LikeEpisode(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.NotNull(result.Episodes);
            Assert.NotNull(result.Episodes);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeEpisode_WhenInteractionIsNull_ReturnIsLikedFalseAndDislikedTrue()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult(new List<Interaction>().AsEnumerable()));

            A.CallTo(() => uow.Interactions.Add(new Interaction()))
               .Returns(Task.FromResult(new Interaction()));

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Episode()));

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.DisLikeEpisode(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.NotNull(result.Episodes);
            Assert.NotNull(result.Episodes);
            Assert.True(result.HasUserDisliked);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeEpisode_WhenInteractionIsNotNullAndUserIsLiked_ReturnIsLikedFalseAndDislikedTrue()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    ItemId = 1,
                    UserId= "userID",
                    IsLiked =true
                }
            };

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Episode()));

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());

            var sut = new EpisodesService(uow);

            //act
            var result = await sut.DisLikeEpisode(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.NotNull(result.Episodes);
            Assert.NotNull(result.Episodes);
            Assert.True(result.HasUserDisliked);
            Assert.False(result.HasUserLiked);
        }

        [Fact]
        public async Task DisLikeEpisode_WhenInteractionIsNotNullAndUserIsDisliked_ReturnIsDislikedFalse()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            var interactions = new List<Interaction>()
            {
                new Interaction()
                {
                    ItemId = 1,
                    UserId= "userID",
                    IsDisLiked = true
                }
            };

            A.CallTo(() => uow.Interactions.GetAllWithoutPagination())
                .Returns(Task.FromResult<IEnumerable<Interaction>>(interactions));

            A.CallTo(() => uow.Episodes.GetById(A<int>.Ignored))
               .Returns(Task.FromResult(new Episode()));

            A.CallTo(() => uow.Interactions.Delete(new Interaction()))
               .Returns(new Interaction());

            A.CallTo(() => uow.Episodes.GetFilteredEpisodesWithPartId(A<int>.Ignored))
               .Returns(new List<Episode>().AsQueryable());


            var sut = new EpisodesService(uow);

            //act
            var result = await sut.DisLikeEpisode(1, "userID");

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Episode);
            Assert.NotNull(result.Episodes);
            Assert.NotNull(result.Episodes);
            Assert.False(result.HasUserDisliked);
        }

        [Fact]
        public async Task GetAllPartsForSelectList_WhenThereIsNoTvShows_ReturnEmptyList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.GetAllPartsForSelectList())
                .Returns(Enumerable.Empty<Part>());

            var sut = new EpisodesService(uow);

            //act
            var result = sut.GetAllPartsForSelectList();

            //assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetAllPartsForSelectList_WhenThereIsTvShows_ReturnTvShowsList()
        {
            //arrange
            var uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uow.Parts.GetAllPartsForSelectList())
                .Returns(new List<Part>()
                {
                    new Part()
                    {
                        Id = 1,
                        Name = "Test"
                    }
                }.AsEnumerable());

            var sut = new EpisodesService(uow);

            //act
            var result = sut.GetAllPartsForSelectList();

            //assert
            Assert.NotNull(result);
            Assert.True(result.Any());
        }
    }
}
