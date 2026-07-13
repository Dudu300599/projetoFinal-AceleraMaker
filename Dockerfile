# Etapa 1: Build da API em .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de projeto e restaura as dependências
COPY ["CooperativaAlfa.Api/CooperativaAlfa.Api.csproj", "CooperativaAlfa.Api/"]
RUN dotnet restore "CooperativaAlfa.Api/CooperativaAlfa.Api.csproj"

# Copia todo o restante do código da solução
COPY . .

# Publica a API compilada na pasta /app/publish
WORKDIR "/src/CooperativaAlfa.Api"
RUN dotnet publish "CooperativaAlfa.Api.csproj" -c Release -o /app/publish

# Etapa 2: Configuração do Runtime (Ambiente de Execução)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instala o GnuCOBOL no ambiente Linux do contêiner
RUN apt-get update && apt-get install -y gnucobol libdb-dev && rm -rf /var/lib/apt/lists/*

# Copia a API compilada da Etapa 1
COPY --from=build /app/publish .

# Cria o diretório para o COBOL, copia o código-fonte e compila o executável legado
RUN mkdir -p /app/cobol
COPY ["Cobol/CLIENTES.cbl", "/app/cobol/"]
WORKDIR /app/cobol
RUN cobc -x -free -o CLIENTES CLIENTES.cbl

# Volta para o diretório raiz da aplicação .NET
WORKDIR /app

# Expõe a porta padrão da API no .NET 8
EXPOSE 8080

# Define o comando de inicialização do contêiner
ENTRYPOINT ["dotnet", "CooperativaAlfa.Api.dll"]