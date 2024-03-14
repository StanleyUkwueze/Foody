using Foody.Model.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.DataSeedClasses
{
    public static class SeedUser
    {
         
        public async static Task AddUser(AppDbContext context, UserManager<Customer> userManager, RoleManager<IdentityRole> roleMgr )
        {
            //add user to a role
          
            
            if (!userManager.Users.Any())
            {
                var roles = new List<IdentityRole>() { new IdentityRole() { Name = "ADMIN" }, new IdentityRole() { Name = "Regular" } };


                foreach(var role in roles) 
                {
                  await roleMgr.CreateAsync(role);
                }
                

                //read json file
                using StreamReader userToRead = new StreamReader("C:\\Users\\HP\\Desktop\\VegeFoods\\Foody\\Foody.DataAcess\\JsonFiles\\Customer.json");
                var userData = await userToRead.ReadToEndAsync();

                // deserilization of Json object
                var usersInfo = JsonConvert.DeserializeObject<IEnumerable<Customer>>(userData);

                int counter = 1;
                string userType;
                string msg = string.Empty;

                foreach (var user in usersInfo!)
                {
                    var userPubId = Guid.NewGuid().ToString();
                    user.publicId = userPubId;
                    if (counter < 2)
                    {
                        userType = roles[0].Name;


                        var result = await userManager.CreateAsync(user, "P@ssword12");

                        var userRole = await userManager.AddToRoleAsync(user, userType);
                    }

                    if(counter>=2)
                    {
                        userType = roles[1].Name;
                        var regularUser = await userManager.CreateAsync(user, "P@assword123");
                        var regularUserRole = await userManager.AddToRoleAsync(user, userType);
                    }

                    counter += 1;
                }

                await context.SaveChangesAsync();
            }

        }
    }
}
