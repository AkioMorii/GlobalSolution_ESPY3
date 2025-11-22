Pedro Henrique Alves Guariente RM550301
Orlando Akio Morii Cardoso RM98067
David de Medeiros Pacheco Junior RM551462

Especificações: Programa desenvolvido na linguagem de C# .NETCORE 8, usando Migration;
Certifique-se de que possui instalado:

    .NET 8 SDK;
    PostgreSQL (ou o banco configurado no projeto);
    Visual Studio 2022 (ou VS Code + CLI);
    Extensão do EF Core (caso use CLI);


Arquitetura Geral
├── GS2_API              (Camada de apresentação - REST API)
├── GS2_Domain           (Regra de negócio e entidades)
├── GS2_Infrastructure   (Acesso a dados, EF Core, Migrations)
└── GS2_APP              (Front-End Razor totalmente desacoplado)

Camadas da Aplicação
============================
GS2_API (REST API)
============================
NET 8 Web API	API moderna e performática
Versionamento via Header	api-Version: 1.0
AutoMapper	Mapping entre Entities ↔ DTOs
Exception Middleware	Captura erros e retorna Json padronizado
ApiVersion Middleware	Validação de versões via cabeçalho
DTOs	Entrada e saída desacopladas da camada de domínio

-Responsabilidades
*Expõe endpoints REST
*Valida entrada de dados
*Executa regras via Domain
*Converte entidades para DTOs (AutoMapper)
*Aplica versionamento e padronização de erros
SWAGGER
Recurso	Descrição
.NET 8 Web API	API moderna e performática
Versionamento via Header	X-Api-Version: 1.0
AutoMapper	Mapping entre Entities ↔ DTOs
Exception Middleware	Captura erros e retorna Json padronizado
ApiVersion Middleware	Validação de versões via cabeçalho
DTOs	Entrada e saída desacopladas da camada de domínio
============================
GS2_Infrastructure
============================
Mapear entidades do domínio para tabelas
* Repositórios concretos
* Execução de queries no PostgreSQL
* Execução de Migrations
* Conexões e providers
============================
Banco de Dados – PostgreSQL
============================
*A camada GS2_Infrastructure contém:
*DbContext central
*DbSets por entidade
*Configurações usando Fluent API
*Scripts gerados via dotnet ef migrations add ...
============================
Principais Padrões Aplicados
============================
*DDD (Domain-Driven Design)
*Clean Architecture
*SOLID
*Repository Pattern
*DTO Pattern
*Exception Handling Middleware
*Versionamento por Header
*AutoMapper Mapping Profiles
============================
Como executar o projeto:
============================
---GIT HUB ---
Clonar o repositório: git clone https://github.com/AkioMorii/GlobalSolution_ESPY3.git

---Migrations---
Startup Project :GS2_API
Default Project (Package Manager Console): GS2_Infrastructure
Criar a migration inicial (se ainda não existir): No Package Manager Console do Visual Studio:
        Defina GS2_Infrastructure como Default Project
        Defina GS2_API como Startup Project
        Execute no "Console do Gerenciador de Pacotes": Add-Migration InitialCreate -StartupProject GS2_API

    Atualizar o banco de dados Ainda no Console do Gerenciador de Pacotes, execute: Update-Database -Verbose


---AppSettings---
    Configurar a connection string: No arquivo: GS2_API/appsettings.Development.json, configure a string de conexão conforme sua instalação local do PostgreSQL ou outro banco de dados: "ConnectionStrings": { "DefaultConnection": "Host=localhost;Port=5432;Database=GS2;Username=postgres;Password=SuaSenha" }

---Outras---    

    Restaurar as dependências do projeto (NuGet): Se estiver usando Visual Studio 2022:
        Abra a solução (.sln);
        No Solution Explorer, clique com o botão direito na Solution;
        Clique em Restore NuGet Packages (ou Restaurar Pacotes NuGet).

Se estiver usando CLI / VS Code:
    Execute no terminal: dotnet restore;

Para se logar, usar: 
    login: administrador
    senha: Adm123456!
