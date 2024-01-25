using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Migrations;

public class TicTacToeContext : IdentityDbContext
{
    public TicTacToeContext(DbContextOptions<TicTacToeContext> options): base(options) {}
    
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}