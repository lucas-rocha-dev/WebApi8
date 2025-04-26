using Microsoft.EntityFrameworkCore;
using WebApi8.Data;
using WebApi8.Dto.Autor;
using WebApi8.Models;

namespace WebApi8.Services.Autor
{
    public class AutorService : IAutorInterface

    {
        private readonly AppDbContext _context;

        public AutorService(AppDbContext context) 
        {
            _context = context;
        
        }
        public async Task<ResponseModel<AutorModel>> BuscarAutorPorId(int idAutor)
        {
            ResponseModel<AutorModel> resposta = new ResponseModel<AutorModel>();
            try 
            {
                var autor = await _context.Autores.FirstOrDefaultAsync(autorBanco => autorBanco.Id == idAutor);
                if (autor == null) {
                    resposta.Status = false;
                    resposta.Mensagem = "Nenhum registro localizado!";
                    return resposta;
                }
                resposta.Dados = autor;
                resposta.Mensagem = "Autor localizado!";
                return resposta;
            }
            catch(Exception ex) 
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;

            }

        }

        public async Task<ResponseModel<AutorModel>> BuscarAutorPorIdLivro(int idLivro)
        {
            ResponseModel<AutorModel> resposta = new ResponseModel<AutorModel>();
            try {
                var livro = await _context.Livros
                   .Include(a => a.Autor)
                    .FirstOrDefaultAsync(livroBanco => livroBanco.Id == idLivro);
                if (livro == null) {
                    resposta.Mensagem = "Nenhum registro localizado!";
                    return resposta;
                }
                resposta.Dados = livro.Autor;
                resposta.Mensagem = "Autor localizado!";
                return resposta;

            }
            catch (Exception ex) 
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }


        public async Task<ResponseModel<List<AutorModel>>> ListarAutores()
        {
            ResponseModel<List<AutorModel>> resposta = new ResponseModel<List<AutorModel>>();
            try {
                var autores = await _context.Autores.ToListAsync();

                resposta.Dados = autores;
                resposta.Mensagem = "Todos os autores foram coletados!";
                return resposta;

            }
            catch (Exception ex) {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<AutorModel>>> CriarAutor(AutorCriacaoDto autorCriacaoDto)
        {
            ResponseModel<List<AutorModel>> resposta = new ResponseModel<List<AutorModel>>();

            try 
            {
                var autor = new AutorModel() {
                    Nome = autorCriacaoDto.Nome,
                    Sobrenome = autorCriacaoDto.Sobrenome
                };
                _context.Add(autor);
                await _context.SaveChangesAsync();
                resposta.Dados = await _context.Autores.ToListAsync();
                resposta.Mensagem = "Autor criado com sucesso!";
                return resposta;

            }
            catch(Exception ex) {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }  
        }

        public async Task<ResponseModel<List<AutorModel>>> EditarAutor(AutorEdicaoDto autorEdicaoDto)
        {
            ResponseModel<List<AutorModel>> resposta = new ResponseModel<List<AutorModel>>();

            try {
                var autorBanco = await _context.Autores
                    .FirstOrDefaultAsync(autor => autor.Id == autorEdicaoDto.Id);
                if (autorBanco == null) {
                    resposta.Mensagem = "Nenhum autor localizado!";
                    return resposta;
                }
                autorBanco.Nome = autorEdicaoDto.Nome;
                autorBanco.Sobrenome = autorEdicaoDto.Sobrenome;
                _context.Update(autorBanco);

                await _context.SaveChangesAsync();
                resposta.Dados = await _context.Autores.ToListAsync();
                resposta.Mensagem = "Autor editado com sucesso!";
                return resposta;
            }
            catch (Exception ex) {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }

        }

        public async Task<ResponseModel<List<AutorModel>>> ExcluirAutor(int idAutor)
        {
            ResponseModel<List<AutorModel>> resposta = new ResponseModel<List<AutorModel>>();

            try {
                var autorBanco = await _context.Autores
                    .FirstOrDefaultAsync(autor => autor.Id == idAutor);
                if (autorBanco == null) {
                    resposta.Mensagem = "Nenhum autor localizado!";
                    return resposta;
                }

                _context.Remove(autorBanco);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Autores.ToListAsync();

                resposta.Mensagem = "Autor removido com sucesso!";
                return resposta;
            }
            catch (Exception ex) {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }


    }
}
