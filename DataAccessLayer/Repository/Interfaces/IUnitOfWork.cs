using DataAccessLayer.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Actor> Actors { get; }
        IRepository<Producer> Producers { get; }
        IRepository<ActorFilms> ActorFilms { get; }
        IRepository<ActorTvShows> ActorTvShows { get; }
        IFilmsRepository Films { get; }
        ITvShowsRepository TvShows { get; }
        IRepository<Category> Categories { get; }
        IRepository<Country> Countries { get; }
        IPartsRepository Parts { get; }
        IEpisodesRepository Episodes { get; }
        IRepository<Interaction> Interactions { get; }
        Task SaveChanges();
    }
}
