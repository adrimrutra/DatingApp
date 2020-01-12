using System;
using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    { 
        public static void SeedUsers(UserManager<User> userManager)
        {
            if(userManager.Users.FirstOrDefault() == null)
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password").Wait();
                }
            } 
        }
    }
}