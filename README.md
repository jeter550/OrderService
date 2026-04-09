# OrderService

API de pedidos construída em .NET 8 com arquitetura em camadas, persistência em PostgreSQL e uso de MediatR para organizar comandos e consultas.

## Estrutura das camadas

### `OrderService.API`
Camada de entrada da aplicação.

- Expõe os endpoints HTTP.
- Configura autenticação, Swagger, DI e pipeline do ASP.NET Core.
- Aplica migrations na inicialização.
- Executa o seeding inicial do banco.

### `OrderService.Application`
Camada de casos de uso.

- Contém `Commands`, `Queries`, `Handlers` e contratos de repositório.
- Orquestra regras de aplicação sem depender de detalhes de infraestrutura.
- Usa MediatR para desacoplar os controladores da lógica de negócio.

### `OrderService.Domain`
Camada central de negócio.

- Contém entidades, enums e exceções de domínio.
- Representa as regras e invariantes do sistema.
- Não deve depender das outras camadas.

### `OrderService.Infrastructure`
Camada de persistência e integração.

- Implementa os repositórios.
- Contém o `AppDbContext`, migrations e seed de dados.
- Faz a integração com Entity Framework Core e PostgreSQL.

### `OrderService.Tests`
Camada de testes automatizados.

- Reúne testes das regras e casos de uso.

## Fluxo da aplicação

1. A requisição entra pela API.
2. O controller envia um command ou query via MediatR.
3. Um handler da camada `Application` executa o caso de uso.
4. O handler acessa os repositórios por interfaces.
5. A `Infrastructure` persiste e consulta os dados no PostgreSQL.
6. O resultado volta para a API e é retornado ao cliente.

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- MediatR
- Swagger / OpenAPI
- xUnit

## Pré-requisitos

- .NET SDK 8
- Docker Desktop ou Docker Engine

## Como executar

### 1. Subir o banco de dados

Na raiz do projeto:

```powershell
docker compose up -d
```

O container do PostgreSQL será iniciado com:

- Banco: `ordersdb`
- Usuário: `postgres`
- Senha: `postgres`
- Porta: `5432`

### 2. Conferir a connection string

O arquivo [appsettings.json](/c:/Users/jeter/Documents/Projetos/OrderService/OrderService.API/appsettings.json) já está configurado com:

```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=ordersdb;Username=postgres;Password=postgres"
}
```

### 3. Restaurar dependências

```powershell
dotnet restore OrderService.sln
```

### 4. Executar a API

```powershell
dotnet run --project OrderService.API
```

Ao iniciar, a aplicação:

- aplica as migrations automaticamente;
- popula produtos iniciais com `DbSeeder`;
- expõe Swagger no ambiente de desenvolvimento.

## Endpoints principais

### Pedidos

- `POST /orders`
- `POST /orders/{id}/confirm`
- `POST /orders/{id}/cancel`
- `GET /orders/{id}`

### Produtos

- `POST /products`

### Health check

- `GET /health/db`

## Dados iniciais

Na subida da aplicação, o seed cadastra alguns produtos automaticamente para facilitar testes locais.

## Executando os testes

```powershell
dotnet test OrderService.sln
```

## Observações

- A API usa PostgreSQL local via Docker.
- O Swagger é habilitado em ambiente de desenvolvimento.
- A autenticação Bearer já está configurada no pipeline da API.
