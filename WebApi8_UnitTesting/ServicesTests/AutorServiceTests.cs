using Microsoft.EntityFrameworkCore;
using WebApi8.Data;
using WebApi8.Dto.Autor;
using WebApi8.Models;
using WebApi8.Services.Autor;

namespace WebApi8_UnitTesting.ServicesTests;

[TestClass]
public class AutorServiceTests
{
    private AppDbContext _context;
    private AutorService _autorService;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // cada teste usa banco isolado
            .Options;

        _context = new AppDbContext(options);
        _autorService = new AutorService(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted(); // apaga o banco depois do teste
        _context.Dispose();
    }

    [TestMethod]
    public async Task BuscarAutorPorId_DeveRetornarAutorQuandoExistir()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Machado", Sobrenome = "de Assis" };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _autorService.BuscarAutorPorId(1);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        Assert.IsNotNull(resultado.Dados);
        Assert.AreEqual("Machado", resultado.Dados.Nome);
    }

    [TestMethod]
    public async Task BuscarAutorPorId_DeveRetornarMensagemQuandoNaoExistir()
    {
        // Act
        var resultado = await _autorService.BuscarAutorPorId(99);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.Status);
        Assert.AreEqual("Nenhum registro localizado!", resultado.Mensagem);
    }

    [TestMethod]
    public async Task BuscarAutorPorIdLivro_DeveRetornarAutorQuandoLivroExistir()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Fulano", Sobrenome = "Silva" };
        var livro = new LivroModel { Id = 1, Titulo = "Livro Teste", Autor = autor };

        _context.Autores.Add(autor);
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _autorService.BuscarAutorPorIdLivro(1);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        Assert.IsNotNull(resultado.Dados);
        Assert.AreEqual("Fulano", resultado.Dados.Nome);
    }

    [TestMethod]
    public async Task CriarAutor_DeveCriarNovoAutor()
    {
        // Arrange
        var autorCriacaoDto = new AutorCriacaoDto
        {
            Nome = "Novo",
            Sobrenome = "Autor"
        };

        // Act
        var resultado = await _autorService.CriarAutor(autorCriacaoDto);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        Assert.IsTrue(resultado.Dados.Exists(a => a.Nome == "Novo" && a.Sobrenome == "Autor"));
    }

    [TestMethod]
    public async Task EditarAutor_DeveEditarAutorExistente()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "Velho", Sobrenome = "Nome" };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        var autorEdicaoDto = new AutorEdicaoDto
        {
            Id = 1,
            Nome = "NovoNome",
            Sobrenome = "NovoSobrenome"
        };

        // Act
        var resultado = await _autorService.EditarAutor(autorEdicaoDto);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        var autorAtualizado = await _context.Autores.FirstOrDefaultAsync(a => a.Id == 1);
        Assert.AreEqual("NovoNome", autorAtualizado.Nome);
    }

    [TestMethod]
    public async Task ExcluirAutor_DeveRemoverAutor()
    {
        // Arrange
        var autor = new AutorModel { Id = 1, Nome = "ParaExcluir", Sobrenome = "Teste" };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _autorService.ExcluirAutor(1);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.Status);
        var autorExcluido = await _context.Autores.FirstOrDefaultAsync(a => a.Id == 1);
        Assert.IsNull(autorExcluido);
    }
}
