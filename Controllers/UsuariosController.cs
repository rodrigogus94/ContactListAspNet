using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebMySQL.Models;


namespace WebMySQL.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;

        public UsuariosController(Contexto context)
        {
            _context = context;
        }

        // Listar todos os usuários cadastrados e Buscar
        public async Task<IActionResult> Index(string searchString)
        {   
            
            var searchDESTINATION = from m in _context.Usuario select m;

            if(!String.IsNullOrEmpty(searchString))
            {
                searchDESTINATION = searchDESTINATION.Where(s => 
                s.DTN_ID.Contains(searchString) );
            }
            else if(!String.IsNullOrEmpty(searchString))
            {
                searchDESTINATION = searchDESTINATION.Where(x => 
                x.DTN_DESTINATION.Contains(searchString) );
            }

            return View(await searchDESTINATION.ToListAsync());
        }


        // Listar um usuário espescífico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario

                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
            
        }
        //Buscar 
        

        // Cadastro de um novo Usuário
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DTN_ID,Nome,DTN_DESTINATION,Celular,Email")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // Editar um usuário
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DTN_ID,Nome, DTN_DESTINATION,Celular,Email")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // Deletar um Usuário
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }

        [HttpPost]
        public FileResult Exportar()
        {
            Contexto db = _context;

            //Obtem uma lista de objetos 
            List<object> listUsuarios = (from usuario in db.Usuario.ToList().Take(_context.Usuario.Count())
                                         select new[] {usuario.DTN_ID, usuario.Nome, usuario.DTN_DESTINATION, usuario.Celular, usuario.Email
                }).ToList<object>();

            //Insere o nome das colunas
            listUsuarios.Insert(0, new string[5] { "DTN_ID", "Nome", "DTN_DESTINATION", "Celular", "Email" });

            StringBuilder sb = new StringBuilder();

            //Percere os funcionarios e gera o CSV
            for (int i = 0; i < listUsuarios.Count; i++)
            {
                string[] usuario = (string[])listUsuarios[i];
                for (int j = 0; j < usuario.Length; j++)
                {
                    //Anexa dados com separador
                    sb.Append(usuario[j] + ",");
                }
                //Anexa uma nova linha
                sb.Append("\r\n");
            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "ListaUsuario.csv");
        }

    }
}
