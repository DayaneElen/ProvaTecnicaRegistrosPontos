using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Data;
using ProvaTecnica.Models;
using static ProvaTecnica.Enums.Enums;

namespace ProvaTecnica.Controllers
{
    public class RegistrosPontosController : BaseController
    {
        private readonly ProvaTecnicaContext _context;

        public RegistrosPontosController(ProvaTecnicaContext context)
        {
            _context = context;
        }

        // GET: RegistrosPontos/IndexReport
        public async Task<IActionResult> IndexReport()
        {
            var usuarios = _context.RegistrosPontos.Select(m => m.NomeUsuario).Distinct();                     

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var usuario in usuarios)
            {
                items.Add(new SelectListItem { Text = usuario, Value = usuario });
            }

            ViewBag.NomeUsuario = items;

            return View();
        }

        // GET: RegistrosPontos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistrosPontos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,NomeUsuario,Tipo,Hora")] RegistrosPontos registrosPontos)
        {
            if (ModelState.IsValid)
            {
                var ultimoRegistroPontoUsuario = await _context.RegistrosPontos.OrderBy(o => o.Id).LastOrDefaultAsync(f => f.NomeUsuario == registrosPontos.NomeUsuario);

                if (ultimoRegistroPontoUsuario == null)
                {
                    registrosPontos.Tipo = "Entrada";
                }
                else
                {
                    if (ultimoRegistroPontoUsuario.Tipo.Trim() == "Entrada")
                    {
                        registrosPontos.Tipo = "Saída";
                    }
                    else
                    {
                        registrosPontos.Tipo = "Entrada";
                    }
                }

                registrosPontos.Data = DateTime.Today;

                TimeSpan timeNow = DateTime.Now.TimeOfDay;
                registrosPontos.Hora = new TimeSpan(timeNow.Hours, timeNow.Minutes, timeNow.Seconds);

                _context.Add(registrosPontos);
                await _context.SaveChangesAsync();

                chamarSweetAlert("Registro de ponto realizado com sucesso", TipoSweetAlert.Sucesso);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: RegistrosPontos/GerarRelatorio
        [HttpPost]
        public async Task<IActionResult> Report(DateTime DataInicial, DateTime DataFinal, string NomeUsuario)
        {
            if (DataInicial.Year > 1 && DataFinal.Year > 1)
            {
                var dataFinal = DateTime.Parse(DataFinal.Year + "-" + DataFinal.Month + "-" + DataFinal.Day + " 23:59:59");

                var relatorio = await _context.RegistrosPontos.FromSqlRaw("SELECT Id, Data, Hora, NomeUsuario, Tipo FROM RegistrosPontos WHERE Data between '" + DataInicial.ToString() + "' AND '" + dataFinal.ToString() + "' AND NomeUsuario = '" + NomeUsuario + "'").ToListAsync();

                if (relatorio.Count == 0)
                {
                    chamarSweetAlert("Sem dados para gerar relatório", TipoSweetAlert.Aviso);
                    return RedirectToAction("IndexReport", "RegistrosPontos");
                }

                return View(relatorio);
            }
            else
            {
                var relatorio = await _context.RegistrosPontos.Where(w => w.NomeUsuario == NomeUsuario).ToListAsync();

                if (relatorio.Count == 0)
                {
                    chamarSweetAlert("Sem dados para gerar relatório", TipoSweetAlert.Aviso);
                    return RedirectToAction("IndexReport", "RegistrosPontos");
                }

                return View(relatorio);
            }
        }
    }
}
