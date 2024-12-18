﻿using Aflamak.Data;
using Aflamak.Models;
using Aflamak.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Aflamak.Repository
{
    public class EpisodesRepository : Repository<Episode>, IEpisodesRepository
    {
        private readonly AppDbContext _context;
        public EpisodesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Episode> GetAllEpisodes(int pageNumber, int pageSize)
        {
            var result = _context.Episodes.OrderBy(x => x.Part.Name).Include(m => m.Part).ThenInclude(m => m.TvShow).ThenInclude(m => m.ActorTvShows).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);

            return result.Any() ? result : Enumerable.Empty<Episode>();
        }
        public IEnumerable<Episode> GetAllEpisodes()
        {
            var result = _context.Episodes.Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(t => t.Producer).Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.Category).Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.Country).Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.ActorTvShows).ThenInclude(p => p.Actor);

            return result.Any() ? result : Enumerable.Empty<Episode>();
        }
        public IQueryable<Episode> GetFilteredEpisodesWithPartId(int id)
        {
            var result = _context.Episodes.Where(e => e.PartId == id).Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(t => t.Producer).Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.Category).Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.Country).Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.ActorTvShows).ThenInclude(p => p.Actor);

            return result.Any() ? result : Enumerable.Empty<Episode>().AsQueryable();
        }
        public IQueryable<Episode> GetRecentEpisodes()
        {
            var data = new List<Episode>();

            var episodes = _context.Episodes.Where(e => (e.Part.Month == DateTime.Now.Month || e.Part.Month == DateTime.Now.Month - 1) && e.Part.Date == DateTime.Now.Year).OrderByDescending(x => x.Part.Month).Include(m => m.Part).ThenInclude(m => m.TvShow).ThenInclude(m => m.ActorTvShows).ThenInclude(m => m.Actor).AsNoTracking();

            if (episodes.Any())
            {
                data.AddRange(episodes);

                return data.AsQueryable();
            }

            return Enumerable.Empty<Episode>().AsQueryable();
        }
    }
}