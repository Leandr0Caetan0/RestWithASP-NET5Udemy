using RestWithASPNETUdemy.Data.Converter.Contract;
using RestWithASPNETUdemy.Data.Converter.Implementations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Repository;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IRepository<Person> _repository;
        //private readonly IParser<PersonVO, Person> _converterVO;
        //private readonly IParser<Person, PersonVO> _converterPerson;
        private readonly PersonConverter _converter;

        public PersonBusinessImplementation(IRepository<Person> repository/*, IParser<PersonVO, Person> converterVO, IParser<Person, PersonVO> converterPerson*/)
        {
            _repository = repository;
            /*_converterVO = converterVO;
            _converterPerson = converterPerson;*/
            _converter = new PersonConverter();
        }

        public List<PersonVO> FindAll()
        {
            var personsEntity = _repository.FindAll(); // chama o FindAll no repositorio generico e guarda na variável persons
            return _converter.Parse(personsEntity); // converte o Person que voltou do repositorio para PersonVO "public List<PersonVO> Parse(List<Person> origin)"
        }

        public PersonVO FindByID(long id)
        {
            var personEntity = _repository.FindByID(id); // chama o FindByID no repositorio generico e guarda na variável person
            return _converter.Parse(personEntity); // converte o Person que voltou do repositorio para PersonVO "public PersonVO Parse(Person origin)"
        }

        public PersonVO Create(PersonVO personVO)
        {
            // PARA ACESSAR O BANCO EU PRECISO QUE A VARIAVEL SEJA DO TIPO Person E NÃO PersonVO
            var personEntity = _converter.Parse(personVO); // converte o objeto PersonVO para Person q seja posivel 'persistir' no banco de dados, guarda na variável personEntity.
            personEntity = _repository.Create(personEntity); // vai ao banco cria uma nova Person e devolve pra variavel personEntity
            return _converter.Parse(personEntity); // converte a Person persistida em PersonVO e retorna para o controller.
        }

        public PersonVO Update(PersonVO personVO)
        {
            var personEntity = _converter.Parse(personVO);
            personEntity = _repository.Update(personEntity);
            return _converter.Parse(personEntity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
