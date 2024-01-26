using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Migrations;

public class TicTacToeContext : IdentityDbContext
{
    public TicTacToeContext(DbContextOptions<TicTacToeContext> options): base(options) 
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().Ignore(x => x.Rate);

        base.OnModelCreating(builder);
    }
}