using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace RestWithASPNETUdemy.Services.Implementations
{
    public class PersonServiceImplementation : IPersonService
    {
        private MySQLContext _context;

        public PersonServiceImplementation(MySQLContext context)
        {
            _context = context;
        }

        public List<Person> FindAll()
        {
            // SELECT NO BANCO PASSANDO SEM APARAMETRO TRAZENDO TODOS REGISTROS DA TABELA
            return _context.Persons.ToList();
        }

        public Person FindByID(long id)
        {
            // SELECT NO BANCO PASSANDO O ID COMO PARAMETRO
            return _context.Persons.SingleOrDefault(p => p.Id.Equals(id));
        }

        public Person Create(Person person)
        {
            try
            {
                // INSERT NA TABELA PERSON. ATRIBUTOS DO OBJETO SÃO PASSADOS VIA JSON.
                _context.Add(person);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return person;
        }

        public Person Update(Person person)
        {
            if (!Exists(person.Id)) // VERIFICA SE O ID DA PESSOA NÃO EXISTE. "SE ESSA PESSOAL NÃO EXISTE..."
            {
                return new Person(); // CRIA UMA NOVA PESSOA COM OS PARAMETROS PASSADOS VIA JSON
            }

            var result = _context.Persons.SingleOrDefault(p => p.Id.Equals(person.Id)); // a Variavel result recebe o objeto "antigo"

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(person); // compara o objeto antigo com o novo e atualiza os campos diferentes.
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return person;
        }

        public void Delete(long id)
        {
            var result = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));

            if (result != null)
            {
                try
                {
                    _context.Persons.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        private bool Exists(long id)
        {
            // FAZ UM SELECT NA TABELA PERSON PASSANDO O ID COMO PARAMETRO.
            return _context.Persons.Any(p => p.Id.Equals(id));
        }
    }
}
