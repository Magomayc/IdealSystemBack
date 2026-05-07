# ============================================
# Stage 1: Build
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copiar arquivos de projeto para cache de restore
COPY Gado.Dominio/Gado.Dominio.csproj Gado.Dominio/
COPY Gado.Repositorio/Gado.Repositorio.csproj Gado.Repositorio/
COPY Gado.Aplicacao/Gado.Aplicacao.csproj Gado.Aplicacao/
COPY Gado.Api/Gado.Api.csproj Gado.Api/

# Restore de dependências (camada cacheada)
RUN dotnet restore Gado.Api/Gado.Api.csproj

# Copiar todo o código-fonte
COPY . .

# Publicar a aplicação em modo Release
RUN dotnet publish Gado.Api/Gado.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ============================================
# Stage 2: Runtime
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

# Criar diretório para o banco SQLite persistente
RUN mkdir -p /app/data

# Copiar artefatos publicados
COPY --from=build /app/publish .

# Variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/gado.sqlite"

# Expor a porta
EXPOSE 8080

# Health check básico
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Entrypoint
ENTRYPOINT ["dotnet", "Gado.Api.dll"]
