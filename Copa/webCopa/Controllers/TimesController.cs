using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entity;
using Modelo;

namespace webCopa.Controllers
{
    public class TimesController : Controller
    {

        private Context db = new Context();

        List<Time> ListaSorteados = new List<Time>();
        List<Pontuacao> pontuacao = new List<Pontuacao>();
        List<Pontuacao> listaGanhadoresPrimeiraRodada = new List<Pontuacao>();

        // GET: Times
        public ActionResult Index()
        {
            var lista = db.Times.ToList();
            ViewBag.Lista = lista;
            return View("details");
        }

        // GET: Times/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Time time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        // GET: Times/Create
        public ActionResult Create()
        {
            //ViewBag.Lista = db.Times.ToList();
            return View();
        }

        // GET: Times/Sortear
        public ActionResult Sortear()
        {

            List<Time> lista = db.Times.ToList();
            ListaSorteados = new List<Time>();

            List<Pontuacao> listaExistePontuacao = db.Pontuacao.ToList();
            var listaExisteJaJogou = listaExistePontuacao.Where(p => p.pontos != null).ToList();
            var listaExisteJaJogouFinal = listaExistePontuacao.Where(p => p.pontos != null && p.fase == 2).ToList();
            var listaExisteFase2 = listaExistePontuacao.Where(p => p.fase == 2 && p.resultado == "Vencedor" && p.pontos != null).ToList();

            if (lista.Count < 4 && listaExisteJaJogou.Count == 0)
            {
                ViewBag.ErroSorteio = "Não é possível sortear, pois o número de times á inferior a 4!";
            }
            else if (listaExisteJaJogouFinal.Count > 0)
            {
                ViewBag.ErroSorteio = "A copa do Mundo está Finalizada!";
            }

            else
            {
                int i = 0;
                while (i < lista.Count)
                {

                    if (listaExisteJaJogou.Count > 0)
                    {
                        foreach (var item in listaExisteJaJogou)
                        {
                            if (item.resultado == "Perdedor" && (item.fase == 1))
                            {
                                Time time = new Time();
                                time.ID = item.Id;
                                lista.Remove(time);
                            }

                            if ((item.resultado == "Vencedor") && (item.fase == 1))
                            {
                                item.fase = 2;
                                item.resultado = string.Empty;
                                item.dtAtualizacao = DateTime.Now;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }


                    if (listaExisteFase2.Count > 0)
                        ViewBag.Status = "Este torneiro se enconta-se Finalizado!";
                    else
                    {

                        Random random = new Random();
                        int posicao = random.Next(0, lista.Count - 1);

                        Time time = new Time();
                        time.Nome = lista[posicao].Nome;
                        time.ID = lista[posicao].ID;

                        lista.RemoveAt(posicao);
                        ListaSorteados.Add(time);
                    }
                }


                int count = 0;
                foreach (var item in ListaSorteados)
                {
                    count = count + 1;
                    Pontuacao pontuacao = new Pontuacao();

                    var existePontuacao = db.Pontuacao.Where(c => c.nomeTime.Contains(item.Nome)).FirstOrDefault();

                    if (existePontuacao != null)
                    {
                        if (existePontuacao.resultado == null)
                        {
                            if (count <= 2)

                                pontuacao.nomeGrupo = "A";

                            else
                                pontuacao.nomeGrupo = "B";

                            pontuacao.idTime = item.ID;
                            pontuacao.nomeTime = item.Nome;
                            pontuacao.dtCadastro = DateTime.Now;
                            pontuacao.dtAtualizacao = DateTime.Now;
                            pontuacao.fase = 1;

                            if (existePontuacao == null)
                            {
                                db.Pontuacao.Add(pontuacao);
                                db.SaveChanges();
                            }
                            else
                            {
                                if (existePontuacao.Id > 0)
                                {
                                    existePontuacao.nomeTime = pontuacao.nomeTime;
                                    existePontuacao.idTime = pontuacao.idTime;
                                    existePontuacao.nomeGrupo = pontuacao.nomeGrupo;
                                    existePontuacao.dtAtualizacao = pontuacao.dtAtualizacao;
                                    existePontuacao.fase = pontuacao.fase;

                                    db.Entry(existePontuacao).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }

                        }

                        else
                        {

                        }
                    }
                    else
                    {
                        if (count <= 2)

                            pontuacao.nomeGrupo = "A";

                        else
                            pontuacao.nomeGrupo = "B";

                        pontuacao.idTime = item.ID;
                        pontuacao.nomeTime = item.Nome;
                        pontuacao.dtCadastro = DateTime.Now;
                        pontuacao.dtAtualizacao = DateTime.Now;
                        pontuacao.fase = 1;
                        db.Pontuacao.Add(pontuacao);
                        db.SaveChanges();

                    }
                }
            }
            ViewBag.Times = ListaSorteados;
            return View("Details");

        }

        // GET: Times/Jogar
        public ActionResult Jogar()
        {
            List<int> listInteiros = new List<int>();
            pontuacao = db.Pontuacao.ToList();
            ViewBag.Lista = pontuacao;

            listInteiros.Add(0);
            listInteiros.Add(1);
            listInteiros.Add(3);
            listInteiros.Add(4);

            if (pontuacao.Count < 4)
            {
                ViewBag.ErroSorteio = "Não é possível Jogar, pois o número de times á inferior a 4!";
            }
            else
            {
                foreach (var item in pontuacao)
                {
                    Random random = new Random();
                    int pontos = random.Next(0, listInteiros.Count - 1);

                    if ((item.fase == 1) && (item.pontos == null))
                    {
                        if (item.nomeGrupo == "A")
                        {
                            item.pontos = listInteiros[pontos].ToString();
                            listInteiros.RemoveAt(pontos);
                        }

                        else if (item.nomeGrupo == "B")
                        {
                            item.pontos = listInteiros[pontos].ToString();
                            listInteiros.RemoveAt(pontos);
                        }
                    }

                    else if (item.fase == 2)
                    {
                        if (listInteiros.Count == 4)
                        {
                            listInteiros.RemoveAt(1);
                            listInteiros.RemoveAt(2);
                        }

                        int pontos2 = random.Next(0, listInteiros.Count - 1);

                        if (item.nomeGrupo == "A")
                        {

                            item.pontos = listInteiros[pontos2].ToString();
                            listInteiros.RemoveAt(pontos2);
                        }

                        else if (item.nomeGrupo == "B")
                        {
                            item.pontos = listInteiros[pontos2].ToString();
                            listInteiros.RemoveAt(pontos2);
                        }

                    }
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                }

                Pontuacao GrupoATimeAnt = new Pontuacao();
                Pontuacao GrupoATimeBnt = new Pontuacao();

                Pontuacao time1 = new Pontuacao();
                Pontuacao time2 = new Pontuacao();

                foreach (var item in pontuacao)
                {

                    if ((item.fase == 1) && (item.resultado == null))
                    {
                        if (item.nomeGrupo == "A")
                        {

                            if (GrupoATimeAnt.Id == 0)
                                GrupoATimeAnt = item;

                            else
                            {
                                if ((Convert.ToInt32(GrupoATimeAnt.pontos)) > (Convert.ToInt32(item.pontos)))
                                {
                                    GrupoATimeAnt.resultado = "Vencedor";
                                    GrupoATimeAnt.dtAtualizacao = DateTime.Now;
                                    db.Entry(GrupoATimeAnt).State = EntityState.Modified;
                                    db.SaveChanges();

                                    item.resultado = "Perdedor";
                                    item.dtAtualizacao = DateTime.Now;
                                    db.Entry(item).State = EntityState.Modified;
                                    db.SaveChanges();


                                }
                                else
                                {
                                    item.resultado = "Vencedor";
                                    item.dtAtualizacao = DateTime.Now;
                                    db.Entry(item).State = EntityState.Modified;
                                    db.SaveChanges();

                                    GrupoATimeAnt.resultado = "Perdedor";
                                    GrupoATimeAnt.dtAtualizacao = DateTime.Now;
                                    db.Entry(GrupoATimeAnt).State = EntityState.Modified;
                                    db.SaveChanges();

                                }

                            }
                        }

                        else if (item.nomeGrupo == "B")
                        {
                            if (GrupoATimeBnt.Id == 0)
                                GrupoATimeBnt = item;

                            else
                            {
                                if ((Convert.ToInt32(GrupoATimeBnt.pontos)) > (Convert.ToInt32(item.pontos)))
                                {
                                    GrupoATimeBnt.resultado = "Vencedor";
                                    GrupoATimeBnt.dtAtualizacao = DateTime.Now;
                                    db.Entry(GrupoATimeBnt).State = EntityState.Modified;
                                    db.SaveChanges();

                                    item.resultado = "Perdedor";
                                    item.dtAtualizacao = DateTime.Now;
                                    db.Entry(item).State = EntityState.Modified;
                                    db.SaveChanges();

                                }
                                else
                                {
                                    item.resultado = "Vencedor";
                                    item.dtAtualizacao = DateTime.Now;
                                    db.Entry(item).State = EntityState.Modified;
                                    db.SaveChanges();

                                    GrupoATimeBnt.resultado = "Perdedor";
                                    GrupoATimeBnt.dtAtualizacao = DateTime.Now;
                                    db.Entry(GrupoATimeBnt).State = EntityState.Modified;
                                    db.SaveChanges();


                                }

                            }

                        }
                    }

                }


                Pontuacao ganhadorGrupoA = db.Pontuacao.Where(c => c.nomeGrupo == "A" && c.resultado == "Vencedor").FirstOrDefault();
                Pontuacao ganhadorGrupoB = db.Pontuacao.Where(c => c.nomeGrupo == "B" && c.resultado == "Vencedor").FirstOrDefault();

                time1 = db.Pontuacao.Where(c => c.nomeGrupo == "A" && c.fase == 2).FirstOrDefault();
                time2 = db.Pontuacao.Where(c => c.nomeGrupo == "B" && c.fase == 2).FirstOrDefault();


                if ((time1 != null) && (time2 != null))
                {

                    if ((Convert.ToInt32(time1.pontos)) > (Convert.ToInt32(time2.pontos)))
                    {
                        time1.resultado = "Vencedor";                       
                        time1.dtAtualizacao = DateTime.Now;
                        time2.resultado = "Perdedor";
                        time2.dtAtualizacao = DateTime.Now;
                        ViewBag.GanhadorCopa = time1.nomeTime;

                    }
                    else
                    {
                        time2.resultado = "Vencedor";
                        time2.dtAtualizacao = DateTime.Now;
                        time1.dtAtualizacao = DateTime.Now;
                        time1.resultado = "Perdedor";
                        ViewBag.GanhadorCopa = time2.nomeTime;
                        
                    }

                    if (time1.Id > 0)
                    {
                        ViewBag.Time1 = time1;
                        db.Entry(time1).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (time2.Id > 0)
                    {
                        ViewBag.Time2 = time2;
                        db.Entry(time2).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    
                }


                if (ganhadorGrupoA != null)
                    ViewBag.GanhadorA = ganhadorGrupoA.nomeTime;

                if (ganhadorGrupoB != null)
                    ViewBag.GanhadorB = ganhadorGrupoB.nomeTime;

                ViewBag.ResultadoPrimeiraRodada = pontuacao;
                ViewData["Pontuacao"] = pontuacao;

            }
            ViewBag.Lista = null;
            return View("Details");
        }


        // POST: Times/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,dtCadastro,dtAtualizacao")] Time time)
        {
            if (ModelState.IsValid)
            {
                List<Time> ListaTimes = new List<Time>();
                ListaTimes = db.Times.ToList();

                string nomeTimes = time.Nome;
                Time existe = ListaTimes.Where(c => c.Nome == time.Nome).FirstOrDefault();

                if (ListaTimes.Count == 4)
                    ViewBag.Status = "A inscrição de times já atingiu a quantidade Máxima para este Torneiro!";



                else if ((existe != null) && (!string.IsNullOrEmpty(existe.Nome)))
                    ViewBag.Status = "Este time já está cadastrado!";

                else
                {

                    //comentar esta linha quando for rodas pela primeira vez, para que as bases sejam criadas. não esquecer de deletar os dados da tabela history do entyti na base de dados.
                    db.Times.Add(time);
                    db.SaveChanges();


                    //ViewBag.Lista = db.Times.ToList();
                    ViewBag.Status = "Cadastro Efetuado com Sucesso!";
                    return View();

                    //this.FlashInfo(this, "Mensagem de Informação.");
                    // this.FlashWarning("Mensagem de Aviso.");
                    //this.FlashError("Mensagem de Erro.");

                }
            }

            return View();
        }

        // GET: Times/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Time time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        // POST: Times/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,dtCadastro,dtAtualizacao")] Time time)
        {
            if (ModelState.IsValid)
            {
                db.Entry(time).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Status = "Time Alterado com Sucesso.";
                ViewBag.Lista = db.Times.ToList();
                return View("Create");
            }
            return View("Create");
        }

        // GET: Times/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Time time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Time time = db.Times.Find(id);
            db.Times.Remove(time);
            db.SaveChanges();

            ViewBag.Status = "Time excluído com Sucesso.";
            ViewBag.Lista = db.Times.ToList();
            return View("Details");

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
