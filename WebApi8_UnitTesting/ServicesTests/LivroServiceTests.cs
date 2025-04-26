using Microsoft.EntityFrameworkCore;
using WebApi8.Data;
using WebApi8.Dto.Livro;
using WebApi8.Models;
using WebApi8.Services.Livro;

namespace WebApi8_UnitTesting.ServicesTests;

[TestClass]
public class LivroServiceTests
{
    private AppDbContext _context;
    private LivroService _livroService;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // banco isolado para cada teste
            .Options;

        _context = new AppDbContext(options);
        _livroService = new LivroService(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted(); // Apaga o banco depois de cada teste
        _context.Dispose();
    }

    [TestMethod]
    public async Task BuscarLivroPorId_DeveRetornarLivroQuandoExistir()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _livroService.BuscarLivroPorId(1);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        Assert.IsNotNull(resultado.Dados);
        Assert.AreEqual("Dom Casmurro", resultado.Dados.Titulo);
    }

    [TestMethod]
    public async Task BuscarLivroPorId_DeveRetornarMensagemQuandoNaoExistir()
    {
        // Act
        var resultado = await _livroService.BuscarLivroPorId(99);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.Status);
        Assert.AreEqual("Nenhum registro localizado!", resultado.Mensagem);
    }

    [TestMethod]
    public async Task BuscarLivrosPorIdAutor_DeveRetornarLivrosQuandoAutorExistir()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _livroService.BuscarLivrorPorIdAutor(1);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        Assert.IsTrue(resultado.Dados.Count > 0);
        Assert.AreEqual("Dom Casmurro", resultado.Dados[0].Titulo);
    }

    [TestMethod]
    public async Task CriarLivro_DeveCriarNovoLivro()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        var livroCriacaoDto = new LivroCriacaoDto
        {
            Titulo = "Memórias Póstumas de Brás Cubas",
            idAutor = 1
        };

        // Act
        var resultado = await _livroService.CriarLivro(livroCriacaoDto);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        Assert.IsTrue(resultado.Dados.Exists(l => l.Titulo == "Memórias Póstumas de Brás Cubas"));
    }

    [TestMethod]
    public async Task EditarLivro_DeveEditarLivroExistente()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        var livroEdicaoDto = new LivroEdicaoDto
        {
            Id = 1,
            Titulo = "Memórias Póstumas de Brás Cubas",
            idAutor = 1
        };

        // Act
        var resultado = await _livroService.EditarLivro(livroEdicaoDto);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        var livroEditado = await _context.Livros.FirstOrDefaultAsync(l => l.Id == 1);
        Assert.AreEqual("Memórias Póstumas de Brás Cubas", livroEditado.Titulo);
    }

    [TestMethod]
    public async Task ExcluirLivro_DeveRemoverLivro()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _livroService.ExcluirLivro(1);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        var livroRemovido = await _context.Livros.FirstOrDefaultAsync(l => l.Id == 1);
        Assert.IsNull(livroRemovido);
    }

    [TestMethod]
    public async Task ListarLivros_DeveRetornarListaDeLivros()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        var livro1 = new LivroModel { Id = 1, Titulo = "Dom Casmurro", Autor = autor };
        var livro2 = new LivroModel { Id = 2, Titulo = "Memórias Póstumas de Brás Cubas", Autor = autor };
        _context.Autores.Add(autor);
        _context.Livros.AddRange(livro1, livro2);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _livroService.ListarLivros();

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        Assert.AreEqual(2, resultado.Dados.Count);
    }
}
