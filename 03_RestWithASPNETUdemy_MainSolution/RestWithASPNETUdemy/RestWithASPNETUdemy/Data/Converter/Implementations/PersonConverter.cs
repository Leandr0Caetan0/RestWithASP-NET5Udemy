using RestWithASPNETUdemy.Data.Converter.Contract;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNETUdemy.Data.Converter.Implementations
{
    public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
    {
        //PersonVO para Person
        public Person Parse(PersonVO origin)
        {
            if (origin == null)
            {
                return null;
            }
            return new Person
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender
            };
        }

        public List<Person> Parse(List<PersonVO> origin)
        {
            if (origin == null)
            {
                return null;
            }
            return origin.Select(x => Parse(x)).ToList(); //chama o Parse para cada PersonVO da lista, e retorna um Person.
        }

        //Person para PersonVO
        public PersonVO Parse(Person origin)
        {
            if (origin == null)
            {
                return null;
            }
            return new PersonVO
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender
            };
        }

        public List<PersonVO> Parse(List<Person> origin)
        {
            if (origin == null)
            {
                return null;
            }
            return origin.Select(x => Parse(x)).ToList();
        }
    }
}
