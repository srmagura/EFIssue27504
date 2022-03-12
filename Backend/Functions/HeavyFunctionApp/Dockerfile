FROM mcr.microsoft.com/azure-functions/dotnet:4

# Install Aspose.PDF dependencies
RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update && apt-get install -y libgdiplus ttf-mscorefonts-installer 

ENV AzureWebJobsScriptRoot=/home/site/wwwroot

COPY . /home/site/wwwroot
