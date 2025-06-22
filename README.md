# 🐾 Cadastro de Animais

Este é um projeto de cadastro de animais desenvolvido com **Windows Forms (WinForms)** em C# e utilizando banco de dados **SQL Server**.

## 📋 Funcionalidades

- Cadastro de novos animais (nome, tutor, tipo, data de nascimento e foto)
- Edição de registros existentes
- Exclusão de registros
- Listagem de todos os animais cadastrados
- Upload de imagem do animal
- Pesquisa por nome
- Interface com DataGridView estilizado

## 🛠️ Tecnologias utilizadas

- C# com Windows Forms (.NET Framework 4.7.2)
- SQL Server (com suporte alternado entre instância local e servidor da escola)
- ADO.NET com OleDb e SqlClient
- Visual Studio
- Banco de dados com tabela `animal`

## 🧠 Lógica de Conexão

O projeto está configurado para **reconhecer automaticamente** se está rodando em casa ou na escola, usando:

```csharp
Environment.MachineName
```

## 📁 Estrutura esperada da Tabela animal
CREATE TABLE animal (
    id INT PRIMARY KEY IDENTITY,
    nome VARCHAR(100),
    tutor VARCHAR(100),
    tipo VARCHAR(50),
    datanasc DATE,
    foto VARCHAR(MAX)
);

## 📌 Observações
- O projeto pode apresentar erro de carregamento se o banco de dados correto não estiver disponível.
- Para rodar o projeto corretamente, verifique se você tem o SQL Server instalado ou substitua pelas suas conexões locais no App.config.
- A imagem do animal é carregada diretamente do caminho selecionado no momento do upload.

## 👩‍💻 Desenvolvido por
- Beatriz Andrade
- Sophia Rosa

*Projeto desenvolvido para fins educacionais.*
