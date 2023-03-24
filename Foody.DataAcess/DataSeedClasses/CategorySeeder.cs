using Foody.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DataAcess.DataSeedClasses
{
    public static class CategorySeeder
    {
        public async static Task AddCategories(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                //read json file
                using StreamReader categoryToRead = new StreamReader("C:\\Users\\HP\\Desktop\\VegeFoods\\Foody\\Foody.DataAcess\\JsonFiles\\Category.json");
                var categoryData = await categoryToRead.ReadToEndAsync();


                // deserilization of Json object
                var categoryInfo = JsonConvert.DeserializeObject<List<Category>>(categoryData);

                context.Categories.AddRange(categoryInfo);
                await context.SaveChangesAsync();
            }
        }
    }
}
