﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.Tmdb;
using Rankomizer.Domain.User;
using Rankomizer.Infrastructure.Database;

namespace Rankomizer.Server.Api.Seeding;

public static class SeedApplication
{
    public static void SeedWebApplication( WebApplication app, IConfiguration config )
    {
        using var scope = app.Services.CreateScope();
        var appContextDb = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        appContextDb.Database.Migrate();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var userSeeding = new UserSeeding( userManager, app.Configuration );
        userSeeding.SeedDatabase().GetAwaiter().GetResult();
        var tmdbManager = scope.ServiceProvider.GetRequiredService<ITmdbManager>();
        ItemSeeding.SeedItemsAsync( appContextDb, userManager, tmdbManager, config ).GetAwaiter().GetResult();
        //Seed all other data
        //var dataSeeding = new DataSeeding();
        //dataSeeding.SeedDatabase().GetAwaiter().GetResult();
    }
}