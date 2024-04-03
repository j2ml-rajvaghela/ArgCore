using ArgCore.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Dapper;
using ArgCore.Data;

namespace ArgCore.Helpers
{
    public class DapperUserStore : IUserStore<ApplicationUser>,
                                   IUserPasswordStore<ApplicationUser>,
                                   IUserEmailStore<ApplicationUser>,
                                   IUserRoleStore<ApplicationUser>,
                                   IRoleStore<IdentityRole>
    {
        private DapperContext _dapperContext;

        public DapperUserStore(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }


        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var insertUserSql = "INSERT INTO Users (Id, UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash) " +
                                "VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @PasswordHash)";

            var result = await _dapperContext.CreateConnection().ExecuteAsync(insertUserSql, user);

            return result == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to create user." });
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var updateUserSql = "UPDATE Users SET UserName = @UserName, NormalizedUserName = @NormalizedUserName, " +
                                "Email = @Email, NormalizedEmail = @NormalizedEmail, PasswordHash = @PasswordHash " +
                                "WHERE Id = @Id";

            var result = await _dapperContext.CreateConnection().ExecuteAsync(updateUserSql, user);

            return result == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to update user." });
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var deleteUserSql = "DELETE FROM Users WHERE Id = @Id";

            var result = await _dapperContext.CreateConnection().ExecuteAsync(deleteUserSql, new { user.Id });

            return result == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to delete user." });
        }

        async Task<ApplicationUser> IUserStore<ApplicationUser>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var findUserByIdSql = "SELECT * FROM Users WHERE Id = @Id";
            return await _dapperContext.CreateConnection().QuerySingleOrDefaultAsync<ApplicationUser>(findUserByIdSql, new { Id = userId });
        }

        async Task<ApplicationUser> IUserStore<ApplicationUser>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var findUserByNameSql = "SELECT * FROM AspNetUsers";
            return await _dapperContext.CreateConnection().QuerySingleOrDefaultAsync<ApplicationUser>(findUserByNameSql);
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.Id;
        }

        public async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.UserName;
        }

        public async Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
        }

        public async Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedUserName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedUserName;
        }

        public async Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.Email;
        }

        public async Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
        }

        public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.EmailConfirmed;
        }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var findUserByEmailSql = "SELECT * FROM AspNetUsers WHERE Email = @Email";
            var result = await _dapperContext.CreateConnection().QuerySingleOrDefaultAsync<ApplicationUser>(findUserByEmailSql, new { Email = email });
            return result;
        }

        public async Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.UserName;
        }

        public async Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.UserName = normalizedEmail;
        }

        public async Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return !string.IsNullOrEmpty(user.PasswordHash);
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var insertUserRoleSql = "INSERT INTO UserRoles (UserId, RoleName) VALUES (@UserId, @RoleName)";

            await _dapperContext.CreateConnection().ExecuteAsync(insertUserRoleSql, new { UserId = user.Id, RoleName = roleName });
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var deleteUserRoleSql = "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleName = @RoleName";

            await _dapperContext.CreateConnection().ExecuteAsync(deleteUserRoleSql, new { UserId = user.Id, RoleName = roleName });
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var getUserRolesSql = "SELECT RoleName FROM UserRoles WHERE UserId = @UserId";

            return (await _dapperContext.CreateConnection().QueryAsync<string>(getUserRolesSql, new { user.Id })).ToList();
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var isInRoleSql = "SELECT COUNT(*) FROM UserRoles WHERE UserId = @UserId AND RoleName = @RoleName";

            var count = await _dapperContext.CreateConnection().ExecuteScalarAsync<int>(isInRoleSql, new { UserId = user.Id, RoleName = roleName });

            return count > 0;
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var getUsersInRoleSql = "SELECT u.* FROM Users u JOIN UserRoles r ON u.Id = r.UserId WHERE r.RoleName = @RoleName";

            return (await _dapperContext.CreateConnection().QueryAsync<ApplicationUser>(getUsersInRoleSql, new { RoleName = roleName })).ToList();
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            try
            {
                var insertRoleSql = "INSERT INTO Roles (Id, Name, NormalizedName) VALUES (@Id, @Name, @NormalizedName)";

                var result = await _dapperContext.CreateConnection().ExecuteAsync(insertRoleSql, role);

                return result == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to create role." });
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            try
            {
                var updateRoleSql = "UPDATE Roles SET Name = @Name, NormalizedName = @NormalizedName WHERE Id = @Id";

                var result = await _dapperContext.CreateConnection().ExecuteAsync(updateRoleSql, role);

                return result == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to update role." });
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            try
            {
                var deleteRoleSql = "DELETE FROM Roles WHERE Id = @Id";

                var result = await _dapperContext.CreateConnection().ExecuteAsync(deleteRoleSql, new { Id = role.Id });

                return result == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to delete role." });
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {ex.Message}" });
            }
        }
        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            // Assuming that the IdentityRole class has a property named Id
            return Task.FromResult(role.Id);
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            // Implement setting role name logic
            role.Name = roleName;
            return Task.CompletedTask;
        }
        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            // Implement getting role name logic
            return Task.FromResult(role.Name);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            // Implement getting normalized role name logic
            return Task.FromResult(role.NormalizedName);
        }
        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            // Implement setting normalized role name logic
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        async Task<IdentityRole> IRoleStore<IdentityRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var findRoleByIdSql = "SELECT * FROM Roles WHERE Id = @Id";
            return await _dapperContext.CreateConnection().QuerySingleOrDefaultAsync<IdentityRole>(findRoleByIdSql, new { Id = roleId });
        }

        async Task<IdentityRole> IRoleStore<IdentityRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var findRoleByNameSql = "SELECT * FROM Roles WHERE NormalizedName = @NormalizedName";
            return await _dapperContext.CreateConnection().QuerySingleOrDefaultAsync<IdentityRole>(findRoleByNameSql, new { NormalizedName = normalizedRoleName });
        }
        public void Dispose()
        {
            // Dispose any resources if needed
        }
    }
}
