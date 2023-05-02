using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WeatherCityBot;

public partial class NotificationsDbContext : DbContext
{
    public NotificationsDbContext()
    {
    }

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=C:\\Users\\Barsuk\\source\\repos\\Проба пера\\TelegramBot\\PogodaCityBot\\PogodaCityBot\\notificationsDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
