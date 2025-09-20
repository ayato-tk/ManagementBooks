using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using GestaoLivros.Infra.Data.Context;

namespace GestaoLivros.Infra.Data.Repositories;

public class GenreRepository(ApplicationDbContext context) : BaseRepository<Genre>(context), IGenreRepository
{
    
}