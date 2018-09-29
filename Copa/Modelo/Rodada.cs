using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modelo
{
    public class Rodada
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public Time IdTimeA { get; set; }
        public Time IdTimeB { get; set; }
        public DateTime dtRodada { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }
   }
}