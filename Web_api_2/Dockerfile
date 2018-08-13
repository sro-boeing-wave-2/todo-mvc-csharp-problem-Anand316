FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 49701
EXPOSE 44371

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Web_api_2.csproj Web_api_2/
RUN dotnet restore Web_api_2/Web_api_2.csproj
COPY . .
WORKDIR /src
RUN dotnet build Web_api_2.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Web_api_2.csproj -c Release -o /app

#RUN chmod +x ./entrypoint.sh
#CMD /bin/bash ./entrypoint.sh
ENTRYPOINT ["entrypoint.sh"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Web_api_2.dll"]



