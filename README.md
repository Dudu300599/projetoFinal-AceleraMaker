# Projeto de Modernização de Sistema Legado - Cooperativa Alfa

Este repositório contém o projeto final desenvolvido para o treinamento em modernização de sistemas. A solução implementa um sistema de cadastro de clientes adotando uma arquitetura híbrida conteinerizada baseada em microsserviços e invocação de processos nativos[cite: 2]. 

O objetivo principal é modernizar a camada de atendimento utilizando tecnologias atuais (.NET) enquanto o processamento das regras de negócio e a persistência dos dados permanecem exclusivamente no ambiente legado (COBOL)[cite: 2].

![Diagrama da Arquitetura do Sistema](caminho/para/sua_imagem_arquitetura.png)

---

## 🏗️ Arquitetura e Componentes

A camada de exposição dos serviços foi desenvolvida utilizando ASP.NET Core 8, disponibilizando uma API RESTful[cite: 2]. Esta API recebe requisições HTTP em formato JSON, valida os dados e traduz as informações para o formato esperado pelo legado[cite: 2]. 

A API utiliza a classe `Process.Start` para executar o binário COBOL localmente (Standard Input/Output), preservando integralmente a lógica de negócio e reduzindo riscos de corrupção dos dados[cite: 2]. Toda a aplicação é executada em um único contêiner Docker, utilizando uma estratégia de multi-stage build, o que garante padronização e facilidade de implantação[cite: 2].

### Stack Tecnológica
* **Backend / API:** ASP.NET Core 8 (.NET 8 - C#)[cite: 2].
* **Arquitetura:** RESTful API com Injeção de Dependência (IoC)[cite: 2].
* **Sistema Legado:** GnuCOBOL[cite: 2].
* **Persistência:** Arquivos Indexados (simulação de VSAM)[cite: 2].
* **Frontend:** HTML5, JavaScript (Fetch API) e Bootstrap 5[cite: 2].
* **Testes:** xUnit e Moq[cite: 2].
* **Infraestrutura:** Docker (multi-stage build) utilizando imagens Linux (Debian/Alpine)[cite: 2].

---

## 📂 Estrutura de Pastas

A organização do código-fonte segue as melhores práticas de separação de responsabilidades em projetos .NET:

```text
📁 raiz/
├── 📁 assets/                          # Imagens utilizadas nesta documentação
├── 📁 CooperativaAlfa.Api/             # Projeto principal (Backend + Frontend embutido)
│   ├── 📁 Cobol/                       # Códigos-fonte do sistema legado (.cbl)
│   ├── 📁 Controllers/                 # Endpoints REST da API (Roteamento HTTP)
│   ├── 📁 Models/                      # Classes de domínio e formatação do Copybook
│   ├── 📁 Services/                    # Lógica de negócio e execução do processo GnuCOBOL
│   └── 📁 wwwroot/                     # Interface de usuário (HTML/JS)
├── 📁 CooperativaAlfa.Testes/          # Projeto de testes automatizados (xUnit + Moq)
├── 📄 Dockerfile                       # Receita de infraestrutura do contêiner multi-stage
├── 📄 .gitignore                       # Configuração de exclusão do Git
└── 📄 README.md                        # Documentação técnica do projeto
```

## 💻 Demonstração do Sistema

Abaixo estão algumas demonstrações da interface gráfica construída para facilitar o uso da solução pelos atendentes.

![Tela de Consulta de Cliente](caminho/para/sua_imagem_consulta.png)

![Tela de Cadastro com Sucesso](caminho/para/sua_imagem_cadastro_sucesso.png)

---

## 🔄 Fluxo de Execução

1. O atendente acessa a interface web e informa os dados do cliente[cite: 2].
2. O navegador envia uma requisição JSON para a API REST, que realiza validações básicas (como CPF e tamanho dos campos)[cite: 2].
3. O Service converte o JSON para a string posicional de 99 caracteres e o .NET executa o programa GnuCOBOL enviando a string como entrada[cite: 2].
4. O COBOL executa a operação diretamente sobre o arquivo indexado (`clientes.dat`) e devolve um código de status com os dados processados[cite: 2].
5. A API interpreta o retorno, converte em um código HTTP apropriado (200 OK, 201 Created, 400 Bad Request, 404 Not Found, 409 Conflict) e a interface atualiza a tela[cite: 2].

---

## 🧪 Qualidade e Testes

A solução conta com um plano de testes para assegurar o atendimento aos requisitos funcionais e verificar que alterações não comprometam o sistema[cite: 3].

### Casos de Teste Funcionais
* **CT01 - Inserir novo cliente:** Valida a gravação do registro no arquivo `clientes.dat`[cite: 3].
* **CT02 - Bloqueio de cadastro duplicado:** Valida o Status 05 do COBOL e a conversão para HTTP 409 pela API[cite: 3].
* **CT03 - Consultar cliente existente:** Valida a localização do registro via CPF[cite: 3].
* **CT04 - Consultar cliente inexistente:** Valida a limpeza de tela e emissão de alerta para o usuário[cite: 3].
* **CT05 - Atualizar informações cadastrais:** Valida a persistência de novos dados de telefone e e-mail[cite: 3].
* **CT06 - Excluir cliente:** Valida a remoção física do arquivo[cite: 3].
* **CT07 - Validação de entrada de dados:** Valida o bloqueio no formulário e o retorno HTTP 400 no ASP.NET Core para dados inválidos[cite: 3].

### Testes Automatizados
Além dos testes funcionais, o projeto possui 8 testes automatizados utilizando xUnit e Moq para garantia de regressão[cite: 3]. A suíte garante desde a formatação correta da string de 99 caracteres até os retornos HTTP exatos do Controller[cite: 3].

![Evidência dos Testes Automatizados no xUnit](caminho/para/sua_imagem_xunit.png)
