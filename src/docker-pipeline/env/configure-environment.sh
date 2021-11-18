#!/bin/bash

set -e

log(){
  echo "INFO:  $(date): ${*}"
}

log 'Configuring machine, Installing dotnet sdk'

yum_update() {
  log '1 - Updating Yum'
  yum update -y

  yum install -y \
   sudo
}

install_netcore(){
  log '2 - Installing dot net core sdk'
  rpm -Uvh https://packages.microsoft.com/config/rhel/7/packages-microsoft-prod.rpm
  yum update -y
  yum install -y dotnet-sdk-2.2
}

yum_update
install_netcore

log '-- Initial Environment installation completed --'
