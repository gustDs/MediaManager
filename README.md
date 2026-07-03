# MediaManager

Sistema pessoal de gerenciamento de mídias — filmes, séries, jogos e livros. Permite registrar sessões de consumo com status, notas, resenhas e histórico completo.

---

## Stack

**Backend**
- .NET 10 / ASP.NET Core
- EF Core + SQLite (dev) / PostgreSQL ou SQL Server (prod)
- MediatR (CQRS lógico)
- FluentValidation
- Protobuf via Grpc.Tools (geração de classes, transporte JSON/REST)
- Repository pattern

**Frontend**
- Vue 3 + Vite
- PrimeVue 4
- Vue Router

---

## Estrutura do projeto
MediaManager/

├── MediaManager.API/          # Controllers, middlewares, Program.cs

├── MediaManager.Application/  # Use cases (CQRS), interfaces de repositório

├── MediaManager.Domain/       # Entidades e enums

├── MediaManager.Infrastructure/ # EF Core, DbContext, repositórios

├── MediaManager.Sdk/          # Contratos Protobuf (.proto)

└── media-manager-frontend/    # Vue 3

---

## Modelo de dados

**MediaItem**
| Campo | Tipo | Descrição |
|---|---|---|
| Id | Guid | PK |
| Nome | string | Obrigatório |
| Tipo | enum | Filme, Serie, Jogo, Livro |
| CriadoEm | DateTime | Gerado automaticamente |

**ConsumptionRecord** (1-N com MediaItem)
| Campo | Tipo | Descrição |
|---|---|---|
| Id | Guid | PK |
| MediaItemId | Guid | FK |
| Status | enum | Do, Doing, Done |
| DataInicio | DateTime? | |
| DataFim | DateTime? | |
| Nota | decimal? | 0.5 a 5.0 |
| Resenha | string? | |
| HorasJogadas | int? | Só para Jogo |
| PaginasLidas | int? | Só para Livro |
| CriadoEm | DateTime | Gerado automaticamente |

> O status atual de um MediaItem é sempre derivado do ConsumptionRecord mais recente — não existe campo de status redundante no MediaItem.

---

## Decisões arquiteturais

- **CQRS lógico**: Commands e Queries separados via MediatR, sem duas bases de dados
- **Protobuf**: usado apenas para geração de classes C# via Grpc.Tools — o transporte é JSON/REST convencional
- **Repository pattern**: handlers da Application acessam dados via interfaces, implementadas na Infrastructure
- **Status derivado**: o status de um item é sempre calculado a partir do registro de consumo mais recente

---

## Como rodar localmente

### Pré-requisitos
- .NET 10 SDK
- Node 18+ (recomendado via nvm)

### Backend

```bash
# Na raiz do projeto
dotnet run --project MediaManager.API
# API disponível em http://localhost:5264
```

### Frontend

```bash
cd media-manager-frontend
npm install
npm run dev
# Frontend disponível em http://localhost:5173
```

---

## Roadmap

- [x] CRUD de MediaItem
- [x] CRUD de ConsumptionRecord
- [x] Frontend Vue 3 com dark mode
- [ ] Autenticação com JWT (login e senha)
- [ ] Hospedagem
- [ ] Kanban view (v2 de UI)
- [ ] Campos específicos por tipo de mídia (perfumaria)
