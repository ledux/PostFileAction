FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
ARG USERNAME_VAR
ARG PASSWORD_VAR

WORKDIR /app
COPY . ./
RUN dotnet nuget add source "https://nuget.pkg.github.com/ILC-Technology/index.json" --name "github" --username $USERNAME_VAR --password $PASSWORD_VAR --store-password-in-clear-text
RUN dotnet publish ./EF.Language.PostFileAction/EF.Language.PostFileAction.csproj --configuration Release --output out --no-self-contained

LABEL maintainer="EF CloudOps <cse-cloudops@ef.com>"
LABEL repository="https://github.com/ILC-Technology/PostFileAction"

LABEL com.github.actions.name="PostFileAction"
LABEL com.github.actions.description="Sends the contents of a file to a web api"

FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "/EF.Language.PostFileAction.dll" ]
