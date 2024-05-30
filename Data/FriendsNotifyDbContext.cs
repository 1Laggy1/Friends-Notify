﻿using Microsoft.EntityFrameworkCore;
using Friends_Notify.Models;

namespace Friends_Notify.Data
{
    public class FriendsNotifyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TrackUsers> TrackUsers { get; set; }

        public FriendsNotifyDbContext(DbContextOptions<FriendsNotifyDbContext> options) : base(options) {}
    }
}
