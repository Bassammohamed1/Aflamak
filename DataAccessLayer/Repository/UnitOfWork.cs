using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Actors = new Repository<Actor>(_context);
            Producers = new Repository<Producer>(_context);
            ActorFilms = new Repository<ActorFilms>(_context);
            ActorTvShows = new Repository<ActorTvShows>(_context);
            Films = new FilmsRepository(_context);
            TvShows = new TvShowsRepository(_context);
            Categories = new Repository<Category>(_context);
            Countries = new Repository<Country>(_context);
            Episodes = new EpisodesRepository(_context);
            Parts = new PartsRepository(_context);
            Interactions = new Repository<Interaction>(_context);
        }

        public IRepository<Actor> Actors { get; private set; }

        public IRepository<Producer> Producers { get; private set; }

        public IRepository<ActorFilms> ActorFilms { get; private set; }

        public IRepository<ActorTvShows> ActorTvShows { get; private set; }

        public IFilmsRepository Films { get; private set; }

        public ITvShowsRepository TvShows { get; private set; }

        public IRepository<Category> Categories { get; private set; }

        public IRepository<Country> Countries { get; private set; }

        public IEpisodesRepository Episodes { get; private set; }

        public IPartsRepository Parts { get; private set; }

        public IRepository<Interaction> Interactions { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
