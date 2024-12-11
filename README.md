# Aplicação do Curso 3054 - balta.io

Este repositório contém a aplicação desenvolvida durante o curso 3054 do balta.io. O objetivo do curso é ensinar os fundamentos do desenvolvimento web utilizando tecnologias modernas e boas práticas de programação.

## Tecnologias Utilizadas

- **.NET Core**: Framework para desenvolvimento backend.
- **Entity Framework Core**: ORM para acesso ao banco de dados.
- **ASP.NET Core**: Framework para construção de APIs e aplicações web.
- **Swagger**: Ferramenta para documentação de APIs.
- **SQLite**: Banco de dados utilizado no projeto.
- **Angular/React/Vue.js**: Frameworks/libraries para desenvolvimento frontend (escolha uma conforme sua preferência).
- **Docker**: Para containerização da aplicação.

## Estrutura do Projeto

A estrutura do projeto segue uma arquitetura em camadas, dividida em:

- **API**: Camada responsável por expor os endpoints da aplicação.
- **Domain**: Camada que contém as entidades de domínio e regras de negócio.
- **Infrastructure**: Camada que contém os repositórios e contexto do banco de dados.
- **Application**: Camada que contém os serviços e casos de uso da aplicação.
- **Presentation**: Camada responsável pelo frontend da aplicação.

## Configuração do Ambiente

### Requisitos

- .NET Core SDK
- Node.js e npm (para o frontend)
- Docker (opcional, para containerização)
- SQLite (ou outro banco de dados, conforme preferência)

### Passos para Configuração

1. **Clone o repositório:**

```bash
git clone https://github.com/edpachecojr/dima-dev-3054-balta
cd dima-dev-3054-balta
```

2. **Restaurar pacotes do .NET:**

```bash
dotnet restore
```

3. **Configurar o banco de dados:**

   - Para o desenvolvimento das atividades foi utilizado uma imagem docker do SQL Server. Caso queira seguir com MSSQLServer e não souber como fazê-lo pode seguir os passos do artigo [SQL Server Docker](https://blog.balta.io/sql-server-docker/)
   - Também é possível utilizar o EF em memória ou SQLite

4. **Aplicar migrações do banco de dados:**

```bash
dotnet ef database update
```

5. **Iniciar a API:**

```bash
dotnet run --project ./src/Dima.Api
```

---

Para mais informações, visite o [balta.io](balta.io) e confira outros cursos disponíveis!
