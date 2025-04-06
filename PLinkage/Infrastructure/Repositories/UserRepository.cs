using Newtonsoft.Json;
using PLinkage.Domain.Interfaces;
using PLinkage.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace PLinkage.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _userFilePath;

        public UserRepository()
        {
            // Get project base path
            string _projectPath = AppDomain.CurrentDomain.BaseDirectory;

            // Get the full path to the JSON folder
            string _jsonFolderPath = Path.GetFullPath(Path.Combine(_projectPath, @"..\..\..\..\..\json"));

            // Combine folder path with the file name for users
            _userFilePath = Path.Combine(_jsonFolderPath, "Users.txt");
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            if (File.Exists(_userFilePath))
            {
                var json = await File.ReadAllTextAsync(_userFilePath);
                return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            return new List<User>();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var users = await GetAllUsersAsync();
            return users.Find(u => u.UserId == userId);
        }

        public async Task AddUserAsync(User user)
        {
            var users = await GetAllUsersAsync();
            users.Add(user);
            await SaveToFileAsync(users);
        }

        public async Task UpdateUserAsync(User user)
        {
            var users = await GetAllUsersAsync();
            var existingUser = users.Find(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                users.Remove(existingUser);
                users.Add(user);
                await SaveToFileAsync(users);
            }
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var users = await GetAllUsersAsync();
            var user = users.Find(u => u.UserId == userId);
            if (user != null)
            {
                users.Remove(user);
                await SaveToFileAsync(users);
            }
        }

        private async Task SaveToFileAsync(List<User> users)
        {
            var json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
            await File.WriteAllTextAsync(_userFilePath, json);
        }
    }
}
