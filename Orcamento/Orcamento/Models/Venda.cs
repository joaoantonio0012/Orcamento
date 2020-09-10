using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orcamento.Models
{
    public class Venda
    {

        public int Id { get; set; }

        public int CodigoOrcamento { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto{ get; set; }
    }
}
