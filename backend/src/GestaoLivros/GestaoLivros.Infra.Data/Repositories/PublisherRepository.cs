using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using GestaoLivros.Infra.Data.Context;

namespace GestaoLivros.Infra.Data.Repositories;

public class PublisherRepository(ApplicationDbContext context) : BaseRepository<Publisher>(context), IPublisherRepository
{
    
}