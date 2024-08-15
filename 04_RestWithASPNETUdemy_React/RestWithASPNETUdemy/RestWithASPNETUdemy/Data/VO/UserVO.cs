using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace RestWithASPNETUdemy.Data.VO
{
    public class UserVO
    {
        // UserVO só precisa dos aributos que são inseridos na tela.
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
