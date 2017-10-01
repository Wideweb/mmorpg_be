using Identity.Api.DataAccess;
using Identity.Api.DataAccess.Models;
using Identity.Api.Services.Exceptions;
using System;

namespace Identity.Api.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly UserRepository userRepository;
        private readonly IEncryptionService encryptionService;

        private const int loginAttempts = 3;

        public MembershipService(UserRepository userRepository,
            IEncryptionService encryptionService)
        {
            this.userRepository = userRepository;
            this.encryptionService = encryptionService;
        }

        public User Create(string userName, string password)
        {
            var existingUser = GetUserByName(userName);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(userName);
            }

            var saltKey = encryptionService.CreateSaltKey(5);

            var user = new User
            {
                UserName = userName,
                PasswordSalt = saltKey,
                Password = encryptionService.CreatePasswordHash(password, saltKey),
                Role = new Role { Name = "User" },
                FailedLoginAttemptsCount = 0
            };

            userRepository.Add(user);

            return user;
        }

        public User GetUserByName(string userName)
        {
            return userRepository.GetByName(userName);
        }

        public User GetUserById(long userId)
        {
            return userRepository.GetById(userId);
        }

        public User LoginUser(string userName, string password)
        {
            return ValidateUser(userName, password);
        }

        private User ValidateUser(string userName, string password)
        {
            var user = GetUserByName(userName);

            if (user == null) throw new UserLogonException();

            if (user.FailedLoginAttemptsCount >= loginAttempts)
            {
                throw new UserLockedOutException();
            }

            var passwordHash = encryptionService.CreatePasswordHash(password, user.PasswordSalt);
            if (string.CompareOrdinal(user.Password, passwordHash) == 0)
            {
                if (user.FailedLoginAttemptsCount != 0)
                {
                    ClearFailedLoginAttempts(user);
                }

                return user;
            }

            AddFailedLoginAttempt(user);

            throw new UserLogonException();
        }

        private void ClearFailedLoginAttempts(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.FailedLoginAttemptsCount = 0;

            userRepository.Update(user);
        }

        private void AddFailedLoginAttempt(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.FailedLoginAttemptsCount++;

            userRepository.Update(user);

            if (user.FailedLoginAttemptsCount == loginAttempts)
            {
                throw new UserLockedOutException();
            }
        }
    }
}
