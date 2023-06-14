using Microsoft.EntityFrameworkCore;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Base;
using RestWithASPNETUdemy.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNETUdemy.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected MySQLContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(MySQLContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public List<T> FindAll()
        {
            return _dbSet.ToList(); 
        }

        public T FindByID(long id)
        {
            return _dbSet.SingleOrDefault(x => x.Id.Equals(id));
        }

        public T Create(T item)
        {
            try
            {
                _dbSet.Add(item);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return item;
        }

        public T Update(T item)
        {
            if (!Exists(item.Id))
            {
                return null;
            }

            var itemToUpdate = _dbSet.SingleOrDefault(x => x.Id.Equals(item.Id)); // variavel itemToUpdate  recebe o objeto "antigo", via select no banco.

            if (itemToUpdate != null)
            {
                try
                {
                    _context.Entry(itemToUpdate).CurrentValues.SetValues(item); // compara o objeto antigo com o novo e atualiza os campos diferentes.
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }
            return itemToUpdate; // retorna o objeto "antigo" que agora foi atualizado na linha 59
        }

        public void Delete(long id)
        {
            var itemToDelete = _dbSet.SingleOrDefault(x => x.Id.Equals(id)); // variavel itemToDelete recebe o objeto via select no banco.

            if (itemToDelete != null)
            {
                try
                {
                    _dbSet.Remove(itemToDelete);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public bool Exists(long id)
        {
            // FAZ UM SELECT EM UMA TABELA GENERICA (QUALQUER ENTIDADE) PASSANDO O ID COMO PARAMETRO.
            return _dbSet.Any(x => x.Id.Equals(id));
        }
    }
}
