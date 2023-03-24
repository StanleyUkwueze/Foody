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
        public async static Task AddUser(AppDbContext context, UserManager<Customer> userManager)
        {
            //add user to a role
            if (!userManager.Users.Any())
            {

                //read json file
                using StreamReader userToRead = new StreamReader("C:\\Users\\HP\\Desktop\\VegeFoods\\Foody\\Foody.DataAcess\\JsonFiles\\Customer.json");
                var userData = await userToRead.ReadToEndAsync();

                // deserilization of Json object
                var usersInfo = JsonConvert.DeserializeObject<IEnumerable<Customer>>(userData);



                int counter = 1;
                string userType;
                string msg = string.Empty;

                foreach (var user in usersInfo)
                {
                    if (counter < 2)
                    {
                        userType = "Admin";

                        var result = await userManager.CreateAsync(user, "P@ssword12");

                        var userRole = await userManager.AddToRoleAsync(user, userType);
                    }

                    else
                    {
                        userType = "Regular";
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
