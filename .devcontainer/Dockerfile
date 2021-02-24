
#-------------------------------------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See https://go.microsoft.com/fwlink/?linkid=2090316 for license information.
#-------------------------------------------------------------------------------------------------------------

# Debian and Ubuntu based images are supported. Alpine images are not yet supported.
FROM debian:9-slim

# Configure apt
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get update \
    && apt-get -y install \
    --no-install-recommends apt-utils 2>&1

# *****************************************************
# * Add steps for installing needed dependencies here *
# ****************************************************
RUN apt-get -y install --no-install-recommends \
    unzip \
    curl \
    wget \
    apt-transport-https \
    ca-certificates \
    gnupg-agent \
    software-properties-common \
    git

# Install Azure cli
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

RUN wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg \
   && mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ \
   && wget -q https://packages.microsoft.com/config/debian/9/prod.list \
   && mv prod.list /etc/apt/sources.list.d/microsoft-prod.list \
   && chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg \
   && chown root:root /etc/apt/sources.list.d/microsoft-prod.list \
   && apt-get update && apt-get install dotnet-sdk-3.1 -y --no-install-recommends

   RUN wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg \
   && mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ \
   && wget -q https://packages.microsoft.com/config/debian/9/prod.list \
   && mv prod.list /etc/apt/sources.list.d/microsoft-prod.list \
   && chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg \
   && chown root:root /etc/apt/sources.list.d/microsoft-prod.list \
   && apt-get update && apt-get install dotnet-sdk-2.1 -y --no-install-recommends

WORKDIR /apim

# download APIM Devops Toolkit
ADD https://github.com/Azure/azure-api-management-devops-resource-kit/archive/v0.5.tar.gz /apim 

# Unpack the zip
RUN tar -zxvf v0.5.tar.gz

# Go to the solution folder for the tool
WORKDIR /apim/azure-api-management-devops-resource-kit-0.5/src/APIM_ARMTemplate/apimtemplate/

RUN pwd

# install devops tool kit tool globally
RUN dotnet pack -c Release \
 && dotnet tool install -g --add-source '.\bin\Release' apimtemplate 


ENV DEBIAN_FRONTEND=dialog

# Set the default shell to bash rather than sh
ENV SHELL /bin/bash