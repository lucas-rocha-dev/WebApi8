# WebApi8_Biblioteca - Gerenciamento de Autores e Livros

Esta API permite gerenciar autores e seus respectivos livros, oferecendo operações para listar, buscar, criar, editar e excluir ambos.

**Para documentação completa e interativa da API, acesse:** [https://app.swaggerhub.com/apis-docs/lucasrocha-3ec/web-api_8_Biblioteca/1.0#/](https://app.swaggerhub.com/apis-docs/lucasrocha-3ec/web-api_8_Biblioteca/1.0#/)

## Endpoints Principais:

### Autores:

* `GET /api/Autor/ListarAutores`: Lista todos os autores.
* `GET /api/Autor/BuscarAutorPorId/{idAutor}`: Busca um autor por ID.
* `GET /api/Autor/BuscarAutorPorIdLivro/{idLivro}`: Busca o autor de um livro pelo ID do livro.
* `POST /api/Autor/CriarAutor`: Cria um novo autor.
* `PUT /api/Autor/EditarAutor`: Edita um autor existente.
* `DELETE /api/Autor/ExcluirAutor?idAutor={idAutor}`: Exclui um autor por ID.

### Livros:

* `GET /api/Livro/ListarLivros`: Lista todos os livros.
* `GET /api/Livro/BuscarLivroPorId/{idLivro}`: Busca um livro por ID.
* `GET /api/Livro/BuscarLivrosPorIdAutor/{idAutor}`: Busca livros por ID do autor.
* `POST /api/Livro/CriarLivro`: Cria um novo livro.
* `PUT /api/Livro/EditarLivro`: Edita um livro existente.
* `DELETE /api/Livro/ExcluirLivro?idLivro={idLivro}`: Exclui um livro por ID.

Consulte a **documentação completa no SwaggerHub** para detalhes sobre request bodies, response schemas e exemplos.
