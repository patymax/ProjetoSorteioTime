using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modelo
{
    public class Time
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtAtualizacao { get; set; }
    }
}