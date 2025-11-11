using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Role == role)
            .OrderBy(u => u.Email)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email, cancellationToken);
    }
}
