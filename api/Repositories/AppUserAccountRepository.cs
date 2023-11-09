using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Dtos;
using api.Interfaces;
using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace api.Repositories;

public class AppUserAccountRepository : IAppUserAccountRepository
{
    private const string _collectionName = "users";
    private readonly IMongoCollection<AppUser> _collection;

    // Dependency Injection
    public AppUserAccountRepository(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>(_collectionName);
    }

    public async Task<AppUserDto?> Create(RegisterDto userInput, CancellationToken cancellationToken)
    {
        // Check if user already exist
        bool userExist = await _collection.Find<AppUser>(user => user.Email == userInput.Email.ToLower().Trim()).AnyAsync(cancellationToken);

        if (userExist)
            return null;

        using var hmac = new HMACSHA512();

        // Create a obj of appuser
        AppUser appUser = new(
            Id: null,
            Email: userInput.Email.ToLower().Trim(),
            PasswordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(userInput.Password!)),
            PasswordSalt: hmac.Key
        );

        // Save created obj to database
        await _collection.InsertOneAsync(appUser, null, cancellationToken);

        // Create a return type for output
        AppUserDto appUserDto = new(
            Id: appUser.Id!,
            Email: appUser.Email
        );

        // Return result
        return appUserDto;
    }
}

//     public async Task<AppUserDto?> Create(AppUser userInput)
//     {
//         // Check if user already exist
//         bool userExist = await _collection.Find<AppUser>(user => user.Email == userInput.Email.ToLower().Trim()).AnyAsync();

//         if (userExist == true)
//             return null;

//         // Create a obj for AppUser
//         AppUser appUser = new AppUser(
//             Id: null,
//             Email: userInput.Email.ToLower().Trim(),
//             Password: userInput.Password,
//             ConfirmPassword: userInput.ConfirmPassword
//         );

//         await _collection.InsertOneAsync(appUser);

//         // Create a new obj for output
//         AppUserDto appUserDto = new(
//             Id: appUser.Id!,
//             Email: appUser.Email
//         );

//         return appUserDto;
//     }




//     // private const string _collectionName = "users";
//     // private readonly IMongoCollection<AppUser> _collection;

//     // // Dependency Injection
//     // public AppUserAccountRepository(IMongoClient client, IMongoDbSettings dbSettings)
//     // {
//     //     var dbName = client.GetDatabase(dbSettings.DatabaseName);
//     //     _collection = dbName.GetCollection<AppUser>(_collectionName);
//     // }

//     // public async Task<AppUserDto?> Create(AppUser userInput)
//     // {
//     //     // Check if user already exist
//     //     bool userExist = await _collection.Find<AppUser>(user => user.Email == userInput.Email.ToLower().Trim()).AnyAsync();

//     //     if (userExist == true)
//     //         return null;

//     //     // Create a obj from input
//     //     AppUser appUser = new AppUser(
//     //         Id: null,
//     //         Email: userInput.Email.ToLower().Trim(),
//     //         Password: userInput.Password,
//     //         ConfirmPassword: userInput.ConfirmPassword
//     //     );

//     //     // Return our obj to database
//     //     await _collection.InsertOneAsync(appUser);

//     //     // Create a new obj for return type
//     //     AppUserDto appUserDto = new AppUserDto(
//     //         Id: appUser.Id!,
//     //         Email: appUser.Email
//     //     );

//     //     // Return response
//     //     return appUserDto;
//     // }
// }
