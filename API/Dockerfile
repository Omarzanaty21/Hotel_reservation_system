# ─── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files first for layer-cached restore
COPY HotelReservation.sln ./
COPY src/HotelReservation.API/HotelReservation.API.csproj             src/HotelReservation.API/
COPY src/HotelReservation.Application/HotelReservation.Application.csproj src/HotelReservation.Application/
COPY src/HotelReservation.Domain/HotelReservation.Domain.csproj       src/HotelReservation.Domain/
COPY src/HotelReservation.Infrastructure/HotelReservation.Infrastructure.csproj src/HotelReservation.Infrastructure/

# Restore dependencies
RUN dotnet restore HotelReservation.sln

# Copy the rest of the source code
COPY src/ src/

# Publish in Release mode
RUN dotnet publish src/HotelReservation.API/HotelReservation.API.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

# ─── Stage 2: Runtime ────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create the wwwroot/images/rooms directory so the volume mount point exists
RUN mkdir -p wwwroot/images/rooms

# Copy published output
COPY --from=build /app/publish .

# Expose HTTP port
EXPOSE 8080

# ASP.NET Core listens on port 8080 inside the container
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "HotelReservation.API.dll"]
