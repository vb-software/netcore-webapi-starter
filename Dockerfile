FROM mcr.microsoft.com/dotnet/sdk:5.0

RUN mkdir -p /usr/share/man/man1

RUN apt-get update && apt-get install -y openjdk-11-jdk
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console
ENV PATH="$PATH:/root/.dotnet/tools"