using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeeding
{
    public static class GymDbContextSeeding
    {

        // craete function to make seeding => return bool and need db context to make seed


        public static bool SeedData(GymDbContext dbContext) {

            try
            {
                // first => check if the table has data or not 

                var planHasData = dbContext.Plans.Any(); // return false if no data

                var categoryHasData = dbContext.Categories.Any();

                // if tabled has data return false

                if (planHasData && categoryHasData) return false;

                // WE gonna to repeat the code to seed data into plan and category =>
                // then the petterto make private function to reuse the code {dry}

                if (!planHasData)
                {

                    var plandata = LoadDataformJsonFiles<Plan>("plans.json");
                    // check if plandata has data
                    if (plandata.Any()) // if has data then add this data 
                        dbContext.Plans.AddRange(plandata);

                }


                if (!categoryHasData)
                {
                    var categoryData = LoadDataformJsonFiles<Category>("categories.json");
                    if (categoryData.Any())
                        dbContext.Categories.AddRange(categoryData);

                }

                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex}");
                return false;
                
            }

        }



        // make a privte function to make a deserialize for json files => 
        // that return list of T  but we need name_file 

        private static List<T> LoadDataformJsonFiles<T>(string filename) { 
        
            // first we need to make the path of file dynamic

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "WWWroot\\Files" ,filename);
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            // read data from the file 
            var data = File.ReadAllText(filePath);
            // define options 
            var options = new JsonSerializerOptions { 
            
                PropertyNameCaseInsensitive = true //flag
            
            };

            // then we need to make deserialize to json files

            return JsonSerializer.Deserialize <List<T>>(data , options) ?? new List<T> (); // if data null return empty list !!     

        }


    }
}
