using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modelo
{
    public class Pontuacao
    {
        
        public int Id { get; set; }
        public string pontos { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }
        public int  idTime { get; set; }
        public string   nomeTime { get; set; }
        public string nomeGrupo { get; set; }
        public int fase { get; set; }
        public string resultado { get; set; }
    }
}