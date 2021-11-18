#!/bin/bash

# This script will configure the EC2 machine so it can run the asp.net core website
# The script will install the following
# 1 - dotnet core 2.2
set -e

log(){
  echo "INFO:  $(date): ${*}"
}

warn(){
  echo "WARN:  $(date): ${*}"
}

err(){
  echo "ERROR: $(date): ${*}"
}

die(){
  echo "FATAL: $(date): ${*} - exiting ..."
  exit 1
}


install_dotnet_dependencies(){
  log '1 - Install dotnet core dependencies libunwind & libicu'
  rpm -Uvh https://packages.microsoft.com/config/rhel/7/packages-microsoft-prod.rpm
  yum install libunwind libicu -y

  yum -x nginx,nginx-module-lua-amzn update -y
  yum install aspnetcore-runtime-2.2 -y --skip-broken

}

download_dotnet_and_setup(){
  log '2 - Download dotnet core'
  curl -sSL -o dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/1057e14e-16cc-410b-80a4-5c2420c8359c/004dc3ce8255475d4723de9a011ac513/dotnet-runtime-2.2.0-linux-x64.tar.gz

  log '3 - un-tar the dotnet package and put under "/usr/bin/dotnet"'
  rm -rf /usr/bin/dotnet
  mkdir -p /usr/bin/dotnet && tar zxf dotnet.tar.gz -C /usr/bin/dotnet
}

update_path_for_all_users_and_current_session(){
  log 'Make dotnet available to all users'
  # /etc/profile.d is a directory were the contents of it gets executed for each user

  echo '# Export dot net to the Path' >> dotnet.sh
  echo "export PATH=\"/usr/bin/dotnet:\$PATH\"" >> dotnet.sh

  log 'Copying dotnet.sh to profile.d so it gets executed for each user'
  cp dotnet.sh /etc/profile.d/dotnet.sh
}

log 'Configuring machine, Installing dotnet core 2.2.0'

install_dotnet_dependencies
download_dotnet_and_setup
update_path_for_all_users_and_current_session

log '-- dotnet core 2.2.0 installation completed --'
