using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace MovieReservation
{
    internal class UserRepository
    {
        private readonly MovieDB _context;
        private const int WORK_FACTOR = 11;


        public UserRepository(MovieDB context)
        {
            _context = context;
        }
        public User Register(string username , string password, userrole role) {

            var user = new User
            {
                username = username,
                hashPassword = HashPassword(password),
                role = role

            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.username == username);

            if(user != null)
            {
                var passwordHash = ValidatePassword(password, user.hashPassword);
                return user;
            }
            return null;
        }

        public bool UpdatePassword(string username, string oldPassword, string newPassword)
        {
            var user = Login(username, oldPassword);
            if(user != null)
            {
                user.hashPassword = HashPassword(newPassword);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WORK_FACTOR);
        }

        private bool ValidatePassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception)
            {
                
                return false;
            }
        }


    }
}
