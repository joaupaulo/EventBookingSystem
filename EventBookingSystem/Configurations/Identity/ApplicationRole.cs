using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace EventBookingSystem.Configurations.Identity;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{
  
}