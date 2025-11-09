# 🔐 AuthSystem API

API de autenticação de usuários desenvolvida em **.NET Core**, utilizando **Entity Framework Core**, **MariaDB**, **ASP.NET Core Identity**, **JWT** e **Clean Architecture** com **CQRS (MediatR)** e **AutoMapper**.

## 🚀 Tecnologias Utilizadas

- **.NET 9 / ASP.NET Core Web API**
- **Entity Framework Core**
- **MariaDB**
- **ASP.NET Core Identity**
- **JWT (JSON Web Token)**
- **CQRS com MediatR**
- **AutoMapper**
- **FluentResults**
- **BCrypt.Net (para hash de senha)**

## 🏗️ Estrutura do Projeto (Clean Architecture)

```
src/
├── Api/ # Camada de apresentação (Controllers)
│ ├── Controllers/
│ └── Program.cs
│
├── Application/ # Regras de negócio e casos de uso
│ ├── Commands/
│ ├── DTOs/
│ ├── Handlers/
│ ├── Interfaces/
│ ├── Mappings/
│ └── Validators/
│
├── Domain/ # Entidades e regras de domínio
│ ├── Entities/
│ └── Interfaces/
│
└── Infrastructure/ # Acesso a dados e serviços externos
├── Context/
├── Configurations/
├── Repositories/
└── Services/
```

## 🧱 Modelos Principais

### `User`
Representa o usuário autenticado, herdando de `IdentityUser<int>`.

## ⚙️ Configuração do Banco de Dados

1. Crie o banco no MariaDB:

   ```
   sql CREATE DATABASE authsystem CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```
2. Configure a connection string no appsettings.json da API:

```
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=authsystem;user=root;password=suasenha"
}
```

3. Aplique as migrations:

```
Add-Migration InitialMigration -StartupProject Api -Project Infrastructure -Context AppDbContext
Update-Database -StartupProject Api -Project Infrastructure -Context AppDbContext
```

## 🔑 Autenticação e Autorização

A API utiliza:

- JWT (Bearer Token) para autenticação.
- Refresh Tokens para renovação segura do acesso. (Á implementar)
- ASP.NET Core Identity para gerenciamento de usuários, senhas e roles.

## ✉️ Recuperação de Senha

- Envio de e-mails configurado via Mailtrap (ambiente de testes).
- Envio de e-mails configurado via Gmail (STMP) (Produção).

> Usuário solicita redefinição → recebe token por e-mail → realiza reset via endpoint.

## 🧩 Endpoints Principais
| Método | Rota                       |	Descrição                                        |
|:-----  |:---------------------------|:-------------------------------------------------|
| POST   |	/api/auth/register        |	Registra um novo usuário                         |
| POST   |	/api/auth/login	          | Realiza login e retorna JWT + RefreshToken       |
| POST   |	/api/auth/refresh         |	Gera novo JWT a partir de um RefreshToken válido |
| POST   |	/api/auth/forgot-password |	Envia e-mail com token de recuperação            |
| POST   |	/api/auth/reset-password  |	Redefine a senha utilizando o token recebido     |

## 🔄 Fluxo de Autenticação

1. Usuário cadastra ou faz login.

2. Recebe JWT (válido por X minutos) e RefreshToken (válido por mais tempo).

3. Quando o JWT expira, o cliente envia o RefreshToken para /api/auth/refresh.

4. O servidor valida e retorna um novo JWT.

## 🧰 Dependências Importantes

```
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package FluentResults
dotnet add package BCrypt.Net-Next
```

## 🧠 Padrões e Boas Práticas Aplicadas

- Clean Architecture
- CQRS + MediatR
- DTOs e AutoMapper
- Soft Delete nas entidades
- Mensagens de erro padronizadas com FluentResults

## 👨‍💻 Autor

Nicolas Liberatto Nunes
API Desenvolvida como parte das atividades curriculares do curso de Sistemas de Informação e estudo de boas práticas com .NET Core.