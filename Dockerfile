FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV BOOKING_COLLECTION="reserva-collection"
ENV CONNECTION_STRING="mongodb+srv://joaopaulo123:1799jp@cluster0.8rok3.mongodb.net/?authSource=admin"
ENV EVENT_COLLECTION="event-collection"
ENV USER_REGISTER="UserRegister"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EventBookingSystem/EventBookingSystem.csproj", "EventBookingSystem/"]
RUN dotnet restore "EventBookingSystem/EventBookingSystem.csproj"
COPY . .
WORKDIR "/src/EventBookingSystem"
RUN dotnet build "EventBookingSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventBookingSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventBookingSystem.dll"]
