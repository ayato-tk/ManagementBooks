using GestaoLivros.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLivros.Infra.Data.Tests;

public static class TestDbContext
{

    public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }
}