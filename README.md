# ZDZCode - Inventory Manager

Solução completa de gerenciamento de estoque com ASP.NET Core e Nuxt.js. Utilizei algumas técnicas que já conhecia como o gerenciamento de dados 
multi-tenant, para isso foi necessário implementar uma autenticação e autorização básica.

## Problemas encontrados

- O menu as vezes não aparece no modo de desenvolvedor ao fazer hot reload. Para voltar a aparecer é necessário recarregar a página.  

## Observações

- Último update significativo: 26-09-2023 11:21

Para sincronizar execute:

```bash
git pull origin main
```

## Estrutura do projeto

- `InventoryManager.API/` - Backend REST API
- `InventoryManager.Web/` - Frontend Web Application

## Tecnologias

### Backend
- .NET 8.0
- Entity Framework Core
- SQLite

### Frontend
- Vue.js 2
- Nuxt.js 2
- Vuetify 2


## Funcionalidades

- Autenticação e Autorização de usuário
- Suporte Multi-Tenant
- Controle e gerenciamento de estoque


## Como executar o projeto


### Backend Setup

1. Navegar para o projeto backend:
```bash
cd InventoryManager.API
```

2. Restaurar as dependências:
```bash
dotnet restore
```
3. Rodar as migrações:
```bash
dotnet ef database update --context AppDbContext
dotnet ef database update --context UserDbContext
```
4. Rodar a API:
```bash
dotnet run dev
```
5. Pegar a porta do servidor da API para adicionar no frontend.


### Frontend Setup

1. Navegar para o projeto frontend:
```bash
cd InventoryManager.Web
```

2. Instalar dependências:
```bash
npm install
```
or
```bash
yarn install
```

3. Criar um arquivo `.env` na raiz do projeto e adicionar o seguinte conteúdo:
```bash
VUE_APP_API_URL=http://localhost:<YOUR-API-PORT>/api
VUE_APP_AUTH_URL=http://localhost:<YOUR-API-PORT>/auth
```

4. Rodar servidor de desenvolvimento:
```bash
npm run dev
```

5. Verificar porta do frontend:

A política de CORS da API está configurada para aceitar a porta 3000 do localhost, caso o frontend rode em outra porta, configure o
Program.cs da API com a porta do seu frontend.

