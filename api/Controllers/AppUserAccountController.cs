// using api.Interfaces;
using api.Interfaces;
using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppUserAccountController : ControllerBase
{
    private readonly IAppUserAccountRepository _appUserAccountRepository;

    // Dependency Injection
    public AppUserAccountController(IAppUserAccountRepository appUserAccountRepository)
    {
        _appUserAccountRepository = appUserAccountRepository;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AppUserDto>> Create(RegisterDto userInput, CancellationToken cancellationToken)
    {
        AppUserDto? appUserDto = await _appUserAccountRepository.Create(userInput, cancellationToken);

        if (appUserDto is null)
            return BadRequest();

        return appUserDto;
    }
}
//     private const string _collectionName = "users";
//     private readonly IMongoCollection<AppUser> _collection;
//     // // private readonly IAppUserRepository _appUserRepository;

//     // Dependency Injection
//     public AppUserController(IMongoClient client, IMongoDbSettings dbSettings)
//     {
//         var dbName = client.GetDatabase(dbSettings.DatabaseName);
//         _collection = dbName.GetCollection<AppUser>(_collectionName);
//         // _appUserRepository = appUserRepository;
//     }

//     // [HttpPost("register")]
//     // public ActionResult<AppUser> Create(AppUser userInput)
//     // {
//     //     // Check passwords match
//     //     if (userInput.Password != userInput.ConfirmPassword)
//     //     {
//     //         return BadRequest(" passwords not match");
//     //     }

//     //     // Check email already exist
//     //     bool emailExist = _collection.Find<AppUser>(user => user.Email == userInput.Email.ToLower().Trim()).Any();

//     //     if (emailExist == true)
//     //     {
//     //         return BadRequest("user already exist");
//     //     }

//     //     // Create a obj
//     //     AppUser appUser = new(
//     //         Id: null,
//     //         Email: userInput.Email.ToLower().Trim(),
//     //         Password: userInput.Password,
//     //         ConfirmPassword: userInput.ConfirmPassword
//     //     );

//     //     _collection.InsertOne(appUser);

//     //     return appUser;
//     // }

//     [HttpPost("login")]
//     public ActionResult<AppUser> Login(AppUser userInput)
//     {
//         AppUser appUser = _collection.Find<AppUser>(user => user.Email == userInput.Email.ToLower().Trim()).FirstOrDefault();

//         // Check if email exist
//         if (appUser is not null)
//         {
//             return appUser;
//         }
//         else
//         {
//             return NotFound();
//         }
//     }

//     [HttpDelete("delete-by-id/{idInput}")]
//     public ActionResult<DeleteResult> DeleteById(string idInput)
//     {
//         DeleteResult deleteResult = _collection.DeleteOne<AppUser>(user => user.Id == idInput);

//         return deleteResult;
//     }

//     [HttpPut("update-by-email/{emailInput}")]
//     public ActionResult<UpdateResult> UpdateByEmail(string emailInput, AppUser appUserInput)
//     {
//         // Check if email exist
//         // bool emailExist = _collection.Find<AppUser>(user => user.Email == emailInput.ToLower().Trim()).Any();

//         // if (emailExist == false)
//         // {
//         //     return NotFound();
//         // }

//         var userUpdate = Builders<AppUser>.Update
//         .Set(user => user.Email, appUserInput.Email.ToLower().Trim())
//         .Set(user => user.Password, appUserInput.Password)
//         .Set(user => user.ConfirmPassword, appUserInput.ConfirmPassword);

//         UpdateResult updateResult = _collection.UpdateOne(user => user.Email == emailInput.ToLower().Trim(), userUpdate);

//         return updateResult;
//     }

//     [HttpGet("get-all")]
//     public ActionResult<IEnumerable<AppUser>> GetAll()
//     {
//         List<AppUser> users = _collection.Find<AppUser>(new BsonDocument()).ToList();

//         return users;

//         // if (users.Any() == false)
//         // {
//         //     NotFound();
//         // }
//     }
// }