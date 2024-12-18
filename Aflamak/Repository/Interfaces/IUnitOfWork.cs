using Aflamak.Models;

namespace Aflamak.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IActorsRepository Actors { get; }
        IProducersRepository Producers { get; }
        IRepository<ActorFilms> ActorFilms { get; }
        IRepository<ActorTvShows> ActorTvShows { get; }
        IFilmsRepository Films { get; }
        ITvShowsRepository TvShows { get; }
        IRepository<Category> Categories { get; }
        IRepository<Country> Countries { get; }
        IPartsRepository Parts { get; }
        IEpisodesRepository Episodes { get; }
        IRepository<Interaction> Interactions { get; }
        void SaveChanges();
    }
}
