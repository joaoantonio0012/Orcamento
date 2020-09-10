using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orcamento.Models
{
    public class Produto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public Decimal Valor { get; set; }

        public ICollection<Venda> Vendas { get; set; }

    }
}
