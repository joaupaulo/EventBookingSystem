using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace EventBookingSystem.Configurations.Identity;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
}