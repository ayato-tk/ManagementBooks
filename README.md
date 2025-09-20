# 📚 Gestão de Livros

## 1. Visão Geral

Gestão de livros desenvolvido com **Angular no front-end** e **.NET 9 (C#) no back-end** e banco de dados SQL Server.  
O sistema permite cadastrar, listar, buscar, atualizar, excluir livros e emitir relatórios em PDF, com autenticação via JWT.

---

## 2. Tecnologias Utilizadas

**Front-end:**  
- Angular 20 
- Angular Material  
- Reactive Forms  
- NX

**Back-end:**  
- .NET 9 Web API  
- Entity Framework Core (Code First)  
- SQL Server  
- Automapper (Mapeamento DTO)  
- FluentValidation  
- JWT Authentication  
- Swagger (Documentação da API)  
- Postman Collection (Documentação da API no Postman)

---

## 3. Arquitetura do Sistema

**Frontend (Angular)**  
- Componentes  
- Serviços HTTP  
- Guards  
- Formulários  

**Backend (.NET 9)**  

- **Presentation Layer (API)**  
  - Controllers: Auth, Books, Genres e Publishers

- **Application Layer**  
  - Services  
  - Validators  
  - Mappers / Automapper  
  - Mediator / Handlers  
  - Regras de negócio para Services 

- **Domain Layer**  
  - Entities  
  - Regras de negócio para Repositories  

- **Infra / Data Layer**  
  - Repositories  
  - DbContext  
  - Migrations  
  - Serviço de geração de relatórios (PDF)


**Fluxo de autenticação JWT:**  
1. Usuário realiza login com e-mail e senha.  
2. API retorna um token JWT.  
3. Angular armazena o token no `localStorage`.  
4. Rotas protegidas só podem ser acessadas com token válido.


**Fluxo de recuperação de senha JWT:**  
1. Usuário realiza requisição com e-mail email válido.  
2. API envia um email contendo uma informação callback.  
3. Quando clicado no link disponível no email, retorna para a aplicação com a possibilidade definir uma nova senha.  
4. Quando a nova senha é aplicada, retorna para a tela de autenticação.

---

## 6. Setup / Instruções de Build

### Backend (.NET 9)

1. Clone o repositório:
```bash
git clone https://github.com/ayato-tk/ManagementBooks.git
cd backend
```

2. Configure a conexão com SQL Server no appsettings.json:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GestaoLivrosDb;User Id=sa;Password=your_password;"
}
```

3. Caso queira configurar o SMTP, basta preencher as informações do appsettings com algum serviço SMTP. Ex:
```json
  "PasswordReset": {
    "ResetUrl": "http://localhost:4200/auth/reset-password"
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "User": "example@gmail.com",
    "Pass": "example",
    "From": "examplegmail.com",
    "Ssl": true
  },
```

4. Aplicar migrations e criar banco de dados:
```bash
dotnet ef database update
```

5. Executar a API no projeto `GestaoLivros.Presentation`:
```bash
dotnet run
```

6. Swagger estará disponível em: `https://localhost:5000/swagger/index.html`

### Frontend (Angular 20) 

1. Navegar para a pasta do frontend:
```bash
cd frontend
```

2. Instalar dependências:
```bash
npm install
```

3. Configurar variáveis de ambiente ( Se necessário ) `environment.development.ts`
```typescript
export const environment = {
    api_base_url: "#{apiBaseUrl}#"
};
```

4. Executar o projeto:
```bash
ng serve
```

5. Abrir no navegador:
```bash
http://localhost:4200
```


### 10. Observações

- Os campos possuem validação via FluentValidation;

- JWT expira após o tempo configurado e rotas protegidas requerem token válido;

- O relatório PDF é gerado apenas para os livros do usuário logado;

- Deixei todos os projetos referenciados na presentation por conta de um bug na minha IDE, por isso pode não fazer muito sentido;

- Docker Compose para auxiliar no banco de dados e serviço SMTP já incluso para desenvolvimento.