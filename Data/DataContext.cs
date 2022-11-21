using GymTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Session> Sessions { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
}