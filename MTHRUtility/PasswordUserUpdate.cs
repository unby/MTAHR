using System;
using System.Data.Entity;
using System.Linq;
using BaseType;
using WebMTHR;

namespace MTHRUtility
{
    public sealed class PasswordUserUpdate:IUtility
    {
        private readonly KeyAndValues _keyAndValues;
        readonly ApplicationDbContext _context=new ApplicationDbContext("MTHRData");
        public PasswordUserUpdate(KeyAndValues keyAndValues)
        {
            
            _keyAndValues = keyAndValues;
        }
        public async void Execute()
        {
            var users =await _context.Users.ToListAsync();
            var store = new ApplicationUserStore(_context);
            var userManager = new ApplicationUserManager(store);
            foreach (var user in users)
            {
                var hashedNewPassword = userManager.PasswordHasher.HashPassword("test@123");

                await store.SetPasswordHashAsync(user, hashedNewPassword);
                await store.UpdateAsync(user);
            }
            await _context.SaveChangesAsync();
            Console.WriteLine("The utility {0} is executed with parameters {1}", _keyAndValues.Key, _keyAndValues.Values.Aggregate((current, next) => current + ", " + next));
//String userId = User.Identity.GetUserId();//"<YourLogicAssignsRequestedUserId>";
//String newPassword = "test@123"; //"<PasswordAsTypedByUser>";
//hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);                    
//ApplicationUser cUser = await store.FindByIdAsync(userId);
//await store.SetPasswordHashAsync(cUser, hashedNewPassword);
//await store.UpdateAsync(cUser);
        }
    }
}
