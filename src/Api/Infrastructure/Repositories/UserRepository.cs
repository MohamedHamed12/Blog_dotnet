// using Core.Entities;
// using Core.Interfaces;
// using Infrastructure.Data;
// using Microsoft.EntityFrameworkCore;

// namespace Infrastructure.Repositories
// {
//     public class UserRepository : IUserRepository
//     {
//         private readonly BlogDbContext _context;

//         public UserRepository(BlogDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<User> GetByUsernameAsync(string username)
//         {
//             return await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
//         }

//         public async Task<User> GetByEmailAsync(string email)
//         {
//             return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
//         }

//         public async Task AddUserAsync(User user)
//         {
//             await _context.Users.AddAsync(user);
//             await _context.SaveChangesAsync();
//         }

//         public async Task<bool> UserExistsAsync(string username, string email)
//         {
//             return await _context.Users.AnyAsync(u => u.UserName == username || u.Email == email);
//         }

//         // public async Task<User> GetByRefreshTokenAsync(string refreshToken)
//         // {
//         //     return await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
//         // }

//         public async Task UpdateUserAsync(User user)
//         {
//             _context.Users.Update(user);
//             await _context.SaveChangesAsync();
//         }
//     }
// }
