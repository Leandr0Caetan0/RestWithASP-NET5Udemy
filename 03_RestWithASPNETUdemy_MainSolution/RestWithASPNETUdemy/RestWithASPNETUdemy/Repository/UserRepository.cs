using Microsoft.EntityFrameworkCore;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNETUdemy.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLContext _context;

        public UserRepository(MySQLContext context)
        {
            _context = context;
        }

        public User ValidateCredentials(UserVO user)
        {
            var pass = ComputeHash(user.Password, new SHA256Managed ());// encriptar senha para na linha de baixo comparar, passa a senha do usuario e uma instancia do SHA256 como parametro.
            return _context.Users.FirstOrDefault(u => (u.UserName == user.UserName) && (u.Password == pass));// faz select no banco passando o user_name e a senha encriptografada como parametro.
        }

        public User UpdateUser(User user)
        {
            if (!Exists(user.Id))
            {
                return null;
            }

            var itemToUpdate = _context.Users.SingleOrDefault(u => u.Id.Equals(user.Id)); // variavel itemToUpdate  recebe o usuário "antigo", via select no banco.

            if (itemToUpdate != null)
            {
                try
                {
                    _context.Entry(itemToUpdate).CurrentValues.SetValues(user); // compara o objeto antigo com o novo e atualiza os campos diferentes.
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return itemToUpdate; // retorna o objeto "antigo" que agora foi atualizado na linha 59.
        }

        private bool Exists(long id)
        {
            return _context.Users.Any(u => u.Id.Equals(id));
        }

        private string ComputeHash(string input, SHA256 sha256)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input); // chama o GetBytes para codificar a senha em um vetor de bytes, salva na variavel inputBytes do tipo byte.
            Byte[] hashedBytes = sha256.ComputeHash(inputBytes); // chama o ComputeHash que trasnforma o vetor de bytes em um Hash Code, salva na variável hashedBytes.
            return BitConverter.ToString(hashedBytes); // invoca a classe BitConverter utilizando ToString para converter o HashCode em uma String.
        }
    }
}
