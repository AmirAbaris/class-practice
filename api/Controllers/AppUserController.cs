// using api.Interfaces;
using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppUserController : ControllerBase
{
    private const string _collectionName = "users";
    private readonly IMongoCollection<AppUser> _collection;
    // // private readonly IAppUserRepository _appUserRepository;

    // Dependency Injection
    public AppUserController(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>(_collectionName);
        // _appUserRepository = appUserRepository;
    }

    [HttpPost("register")]
    public ActionResult<AppUser> Create(AppUser userInput)
    {
        // Check passwords match
        if (userInput.Password != userInput.ConfirmPassword)
        {
            return BadRequest(" passwords not match");
        }

        // Check email already exist
        bool emailExist = _collection.Find<AppUser>(user => user.Email == userInput.Email.ToLower().Trim()).Any();

        if (emailExist == true)
        {
            return BadRequest("user already exist");
        }

        // Create a obj
        AppUser appUser = new(
            Id: null,
            Email: userInput.Email.ToLower().Trim(),
            Password: userInput.Password,
            ConfirmPassword: userInput.ConfirmPassword
        );

        _collection.InsertOne(appUser);

        return appUser;
    }

    [HttpPost("login")]
    public ActionResult<AppUser> Login(AppUser userInput)
    {
        AppUser appUser = _collection.Find<AppUser>(user => user.Email == userInput.Email.ToLower().Trim()).FirstOrDefault();

        // Check if email exist
        if (appUser is not null)
        {
            return appUser;
        }
        else
        {
            return NotFound();
        }
    }


}