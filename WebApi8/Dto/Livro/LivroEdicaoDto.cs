using WebApi8.Models;

namespace WebApi8.Dto.Livro
{
    public class LivroEdicaoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int idAutor { get; set; }
    }
}
