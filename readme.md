# Arquitetura
Projeto desenvolvido utilizando .NET 8.0.
Tanto a API quanto o banco de dados estão dockeirizados.

## Controller
Responsável por receber as requisições HTTP, chamar os serviços apropriados e retornar as respostas HTTP.

## Service
Responsável pela lógica de negócio da aplicação. Ele processa os dados recebidos dos controllers e interage com o repositório para acessar o banco de dados.

## Model
Define as entidades e estruturas de dados utilizadas na aplicação. Representa os objetos do domínio do negócio e suas respectivas tabelas no banco de dados.


# EF Core
```
dotnet ef migrations add InitialMigration
dotnet ef database update
```