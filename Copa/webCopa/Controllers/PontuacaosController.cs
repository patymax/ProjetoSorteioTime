using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entity;
using Modelo;

namespace webCopa.Controllers
{
    public class PontuacaosController : Controller
    {
        private Context db = new Context();

        // GET: Pontuacaos
        public async Task<ActionResult> Index()
        {
            return View(await db.Pontuacao.ToListAsync());
        }

        // GET: Pontuacaos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pontuacao pontuacao = await db.Pontuacao.FindAsync(id);
            if (pontuacao == null)
            {
                return HttpNotFound();
            }
            return View(pontuacao);
        }

        // GET: Pontuacaos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pontuacaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,pontos,dtCadastro,dtAtualizacao")] Pontuacao pontuacao)
        {
            if (ModelState.IsValid)
            {
                db.Pontuacao.Add(pontuacao);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pontuacao);
        }

        // GET: Pontuacaos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pontuacao pontuacao = await db.Pontuacao.FindAsync(id);
            if (pontuacao == null)
            {
                return HttpNotFound();
            }
            return View(pontuacao);
        }

        // POST: Pontuacaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,pontos,dtCadastro,dtAtualizacao")] Pontuacao pontuacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pontuacao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pontuacao);
        }

        // GET: Pontuacaos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pontuacao pontuacao = await db.Pontuacao.FindAsync(id);
            if (pontuacao == null)
            {
                return HttpNotFound();
            }
            return View(pontuacao);
        }

        // POST: Pontuacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pontuacao pontuacao = await db.Pontuacao.FindAsync(id);
            db.Pontuacao.Remove(pontuacao);            
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
