# 📚 Guia de Arquitetura Hexagonal - Inferno API

## 🏗️ Estrutura do Projeto

```
src/
├── Adapters/
│   ├── Inbound/
│   │   └── Controllers/          ← Recebe requisições HTTP
│   │       └── Demon/
│   │           └── DemonController.cs
│   └── Outbound/
│       └── Persistence/
│           ├── HellDbContext.cs  ← Acesso ao banco
│           └── Repositories/     ← Implementação de persistência
│               └── Demon/
│                   └── DemonRepository.cs
├── Core/
│   ├── Domain/
│   │   ├── Entities/             ← Modelos de negócio
│   │   ├── Enums/
│   │   ├── Interfaces/           ← Contratos (abstração)
│   │   │   ├── IRepository.cs
│   │   │   ├── IDemonRepository.cs
│   │   │   └── UseCases/
│   │   │       └── ICreatePersecutionUseCase.cs
│   │   └── Exceptions/           ← Exceções de negócio
│   └── Application/
│       ├── DTOs/                 ← Objetos de transferência
│       │   ├── Requests/         ← Entrada (do Controller)
│       │   └── Responses/        ← Saída (para o Controller)
│       └── UseCases/             ← Lógica de negócio
│           └── CreatePersecutionUseCase.cs
└── Configuration/                ← Configuração de entidades
    └── (EntityTypeConfigurations)
```

---

## 🔄 Fluxo de uma Requisição

```
HTTP Request
    ↓
┌─────────────────────────────────────┐
│ Controller (Adapter - Inbound)      │ ← Valida INPUT (ID vazio, etc)
│ - Recebe Request                    │
│ - Chama UseCase                     │
└─────────────────────────────────────┘
    ↓
┌─────────────────────────────────────┐
│ UseCase (Application - Core)        │ ← Valida REGRAS DE NEGÓCIO
│ - Processa lógica                   │   (Entidade existe, limite, etc)
│ - Chama Repository                  │
│ - Retorna Response                  │
└─────────────────────────────────────┘
    ↓
┌─────────────────────────────────────┐
│ Repository (Adapter - Outbound)     │ ← SEM validações
│ - Acessa dados                      │   (Apenas CRUD)
│ - Retorna Entity                    │
└─────────────────────────────────────┘
    ↓
HTTP Response (JSON)
```

---

## 📍 Onde Colocar Cada Coisa

### 1. **Controllers** (`src/Adapters/Inbound/Controllers/`)

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<DemonResponse>> GetByIdAsync(Guid id)
{
    // ✅ Valida INPUT
    if (id == Guid.Empty)
        return BadRequest("ID não pode estar vazio");
    
    var response = await _useCase.ExecuteAsync(new GetDemonRequest(id));
    return Ok(response);
}
```

**Por quê:** Primeira barreira - rejeita dados malformados antes de processar

---

### 2. **Use Cases** (`src/Core/Application/UseCases/`)

```csharp
public class CreatePersecutionUseCase
{
    public async Task<CreatePersecutionResponse> ExecuteAsync(CreatePersecutionRequest request)
    {
        // ✅ Valida REGRAS DE NEGÓCIO
        var demon = await _demonRepository.GetByIdAsync(request.IdDemon);
        if (demon == null)
            throw new NotFoundException("Demônio não encontrado");
        
        var soul = await _soulRepository.GetByIdAsync(request.IdSoul);
        if (soul == null)
            throw new NotFoundException("Alma não encontrada");
        
        // Verifica duplicação
        var exists = await _persecutionRepository.ExistsAsync(request.IdDemon, request.IdSoul);
        if (exists)
            throw new InvalidOperationException("Esta perseguição já existe");
        
        var persecution = new Persecution { ... };
        await _persecutionRepository.AddAsync(persecution);
        
        return new CreatePersecutionResponse(...);
    }
}
```

**Por quê:** Centraliza toda lógica de negócio, independente da interface (HTTP, CLI, etc)

---

### 3. **Repositories** (`src/Adapters/Outbound/Persistence/Repositories/`)

```csharp
public class DemonRepository : IDemonRepository
{
    public async Task<Demon?> GetByIdAsync(Guid id)
    {
        // ❌ SEM validações - apenas CRUD
        return _context.Demons.FirstOrDefault(d => d.IdDemon == id);
    }
    
    public async Task<Demon> AddAsync(Demon entity)
    {
        _context.Demons.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
```

**Por quê:** Repository é apenas um adaptador de dados - a lógica vem do UseCase

---

### 4. **DTOs** (`src/Core/Application/DTOs/`)

**❌ NÃO faça:**
```
Uma classe DTO compartilhada entre Controller, UseCase e Repository
```

**✅ FAÇA:**
```
DTOs/
├── Requests/
│   └── CreatePersecutionRequest.cs    ← Controller → UseCase
│       { IdDemon, IdSoul }
├── Responses/
│   └── CreatePersecutionResponse.cs   ← UseCase → Controller
│       { IdDemon, IdSoul, DataInicio, DataFim }
```

**Por quê:** Cada camada tem responsabilidades diferentes - as DTOs refletem isso

---

### 5. **Interfaces** (`src/Core/Domain/Interfaces/`)

```
Interfaces/
├── IRepository.cs                    ← Contrato genérico de persistência
├── IDemonRepository.cs               ← Contrato específico (métodos custom)
└── UseCases/
    ├── ICreatePersecutionUseCase.cs  ← Contrato do use case
    └── ITransferSoulUseCase.cs
```

**Por quê:** Abstração permite trocar implementação sem afetar o resto do código

---

## 🎯 Resumo - Responsabilidades

| Componente | Responsabilidade | Valida? |
|---|---|---|
| **Controller** | Receber HTTP, validar input | ✅ Formato/vazio |
| **UseCase** | Executar lógica de negócio | ✅ Regras (existe?, duplicado?, limite?) |
| **Repository** | Persistir dados | ❌ Não valida |
| **Entity** | Modelo de domínio | ✅ (Implícito via banco) |
| **DTO** | Transferir dados entre camadas | ❌ Apenas estrutura |

---

## 🔌 Dependency Injection

No `Program.cs`:

```csharp
// Repositories
builder.Services.AddScoped<IDemonRepository, DemonRepository>();
builder.Services.AddScoped<IPersecutionRepository, PersecutionRepository>();

// Use Cases
builder.Services.AddScoped<ICreatePersecutionUseCase, CreatePersecutionUseCase>();

// DbContext
builder.Services.AddDbContext<HellDbContext>();
```

**Por quê:** Desacopla as implementações - fácil trocar Repository, UseCase, etc

---

## 📝 Exemplo Completo: CreatePersecution

### Request (HTTP)
```json
POST /api/persecution
{
    "idDemon": "550e8400-e29b-41d4-a716-446655440000",
    "idSoul": "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
}
```

### Controller
```csharp
[HttpPost]
public async Task<ActionResult<CreatePersecutionResponse>> CreateAsync(
    CreatePersecutionRequest request)
{
    var response = await _useCase.ExecuteAsync(request);
    return CreatedAtAction(nameof(GetByIdAsync), response);
}
```

### UseCase
```csharp
public async Task<CreatePersecutionResponse> ExecuteAsync(CreatePersecutionRequest request)
{
    // Validações de negócio
    var demon = await _demonRepository.GetByIdAsync(request.IdDemon);
    if (demon == null) throw new NotFoundException("Demônio não encontrado");
    
    var persecution = await _persecutionRepository.AddAsync(
        new Persecution { IdDemon = request.IdDemon, IdSoul = request.IdSoul }
    );
    
    return new CreatePersecutionResponse(
        persecution.IdDemon,
        persecution.IdSoul,
        persecution.DataInicio,
        persecution.DataFim
    );
}
```

### Response (HTTP)
```json
201 Created
{
    "idDemon": "550e8400-e29b-41d4-a716-446655440000",
    "idSoul": "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
    "dataInicio": "2025-12-07T10:30:00Z",
    "dataFim": null
}
```

---

## ✅ Checklist de Implementação

- [ ] Controller com validação de input
- [ ] UseCase com lógica de negócio
- [ ] Repository com CRUD simples
- [ ] Interfaces para Repository e UseCase
- [ ] DTOs separados (Request/Response)
- [ ] DI configurado no Program.cs
- [ ] Exceções customizadas

---

**Conclusão:** Cada camada tem um propósito - respeite a separação e seu código será testável, manutenível e escalável! 🚀

PROMPT:
"""
imagine que você é um desenvolvedor experiente, e tem a seguinte tarefa de me instruir para que eu possa desenvolver um código de qualidade e possa desenvolver minhas capacidades de escrita de código, a idéia central e você ser retórico,me incentivando para que eu possa evoluir,porem sem dar as respostas ou o código completo? tambem peço que evite enrolações e seja bem direito evitando poluir muito o chat, entendeu? qualquer duvida pode me perguntar
"""