
# Build stage

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution
COPY ClassUP.sln .

# Copy project files 
COPY ClassUP.API/ClassUP.API.csproj ClassUP.API/
COPY ClassUP.ApplicationCore/ClassUP.ApplicationCore.csproj ClassUP.ApplicationCore/
COPY ClassUP.Domain/ClassUP.Domain.csproj ClassUP.Domain/
COPY ClassUP.Infrastructure/ClassUP.Infrastructure.csproj ClassUP.Infrastructure/

# Restore dependencies
RUN dotnet restore ClassUP.API/ClassUP.API.csproj

# Copy full source
COPY . .

# Publish 
RUN dotnet publish ClassUP.API/ClassUP.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# Runtime stage

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Security: create non-root user
RUN useradd -m appuser
USER appuser

# Copy published output
COPY --from=build /app/publish .

# Render uses dynamic port
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

# Expose port
EXPOSE 10000

# Health check 
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
 CMD curl --fail http://localhost:10000/health || exit 1

# Start app
ENTRYPOINT ["dotnet", "ClassUP.API.dll"]