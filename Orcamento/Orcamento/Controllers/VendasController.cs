using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orcamento.Data;
using Orcamento.Models;

namespace Orcamento.Controllers
{
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
             
            var applicationDbContext = await _context.Vendas.Include(v => v.Cliente).Include(v => v.Produto).FromSql("SELECT * FROM consultaOrcamentoIndex()").ToListAsync();



           // var applicationDbContext = _context.Vendas.Include(v => v.Cliente).Include(v => v.Produto);
            return View(  applicationDbContext);
        }

        public async Task<IActionResult> ListaOrcamento(int id)
        {

        
            var applicationDbContext = _context.Vendas.Include(v => v.Cliente).Include(v => v.Produto).Where(c => c.CodigoOrcamento ==id);

            ViewData["Total"] = _context.Vendas.Include(v => v.Produto).Where(c => c.CodigoOrcamento == id).Sum(p => p.Produto.Valor).ToString();
            ViewData["CodigoOrcamento"] = id;

           //int clienteid = _context.Vendas.Where(v => v.CodigoOrcamento == id).FirstAsync().Result.ClienteId;

            //ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", clienteid);
            return View(applicationDbContext);
        }


        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create(int ?Id)
        {

            // se for 0 novo orcamento

            if (Id == null)
            {
                
                Id = _context.Vendas.Max(v => v.CodigoOrcamento) + 1;
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
                ViewData["Editar"] = "NOVO";
            }
            else
            {
                var ClienteId = _context.Vendas.Include(c =>c.Cliente).Where(v => v.CodigoOrcamento == Id);
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", ClienteId.First().ClienteId);
                ViewData["Editar"] = "Editar";
                ViewData["NOME"] = ClienteId.First().Cliente.Nome;
            }
            ViewData["CodigoOrcamento"] = Id;
            ViewData["ProdutoId"] = new SelectList(_context.Produtoss, "Id", "Nome");
            return View();
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodigoOrcamento,ClienteId,ProdutoId")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                var param = new SqlParameter("@CodigoOrcamento", venda.CodigoOrcamento);
                var param2 = new SqlParameter("@ClienteId", venda.ClienteId);
                var param3 = new SqlParameter("@ProdutoId", venda.ProdutoId);


                await _context.Database.ExecuteSqlCommandAsync("CadastroOrcamento @CodigoOrcamento, @ClienteId,@ProdutoId ", param, param2,param3);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtoss, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtoss, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodigoOrcamento,ClienteId,ProdutoId")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var param = new SqlParameter("@id", venda.CodigoOrcamento); 
                    var param2 = new SqlParameter("@ProdutoId", venda.ProdutoId);
                    var param3 = new SqlParameter("@idVenda", venda.Id);


                    await _context.Database.ExecuteSqlCommandAsync("alterarOrcamento @id,  @ProdutoId, @idVenda ", param, param2,param3);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ListaOrcamento", "Vendas", new { id = venda.CodigoOrcamento });
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtoss, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            var codigo = venda.CodigoOrcamento;
            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();

            venda = await _context.Vendas.FindAsync(codigo);

            try
            {
                return RedirectToAction("ListaOrcamento", "Vendas", new { id = venda.CodigoOrcamento });
            }
            catch
            {
                return RedirectToAction("Index", "Vendas");
            }
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}
