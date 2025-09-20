# 📚 Fluxos do Sistema de Catálogo de Livros

## 🔑 Fluxo de Trocar Senha
1. Usuário acessa a **tela de login**  
2. Preenche o **e-mail**  
3. Clica no botão **"Trocar Senha"**  
4. O sistema envia um e-mail via **SMTP** (necessário configurar no `appsettings.json`)  
5. Usuário recebe o link no e-mail  
6. Ao clicar no link, é direcionado para a **tela de troca de senha**  
7. Preenche a **nova senha**  
8. Confirma a alteração e a senha é atualizada  

---

## 🔐 Fluxo de Login
1. Usuário acessa a **tela de login**  
2. Preenche o **e-mail**  
3. Preenche a **senha**  
4. Clica no botão **"Login"**  
5. O sistema valida as credenciais  
6. Em caso de sucesso, o usuário é autenticado  
7. Redirecionamento para a **tela de listagem de livros** (rota principal)  

---

## 📝 Fluxo de Registro
1. Usuário acessa a **tela de login**  
2. Clica no botão **"Registrar"**  
3. Preenche os campos:  
   - Nome  
   - E-mail  
   - Senha  
   - Data de nascimento  
4. Clica em **"Registrar"**  
5. O sistema valida os dados  
6. Usuário é registrado e pode realizar login  

---

## 📖 Fluxo de Criar Livro
1. Usuário acessa a **tela de listagem de livros**  
2. Clica no botão **"Criar Livro"**  
3. Preenche os campos:  
   - Título  
   - Autor  
   - ISBN  
   - Sinopse *(opcional)*  
   - Publicadora  
   - Gênero  
   - Imagem *(opcional)*  
4. Clica em **"Criar"**  
5. O livro é salvo no sistema  

---

## 📊 Fluxo de Criar Relatório
1. Em qualquer **tela protegida**  
2. Usuário acessa o **menu de usuário**  
3. Clica em **"Gerar Relatório"**  
4. O sistema gera e disponibiliza o download do relatório  

---

## ✏️ Fluxo de Editar Livro
1. Usuário acessa a **tela de listagem de livros**  
2. Clica no botão **"Editar Livro"**  
3. Edita os dados desejados  
4. Clica em **"Salvar"**  
5. O livro é atualizado no sistema  

---

## 🏷️ Fluxo de Criar Gênero
1. Usuário acessa a **tela de listagem de gêneros**  
2. Clica no botão **"Criar Gênero"**  
3. Preenche o **nome**  
4. Clica em **"Criar"**  
5. O gênero é salvo no sistema  

---

## ✏️ Fluxo de Editar Gênero
1. Usuário acessa a **tela de listagem de gêneros**  
2. Clica no botão **"Editar Gênero"**  
3. Edita o **nome**  
4. Clica em **"Salvar"**  
5. O gênero é atualizado  

---

## 🏢 Fluxo de Criar Publicadora
1. Usuário acessa a **tela de listagem de publicadoras**  
2. Clica no botão **"Criar Publicadora"**  
3. Preenche o **nome**  
4. Clica em **"Criar"**  
5. A publicadora é salva  

---

## ✏️ Fluxo de Editar Publicadora
1. Usuário acessa a **tela de listagem de publicadoras**  
2. Clica no botão **"Editar Publicadora"**  
3. Edita o **nome**  
4. Clica em **"Salvar"**  
5. A publicadora é atualizada  
