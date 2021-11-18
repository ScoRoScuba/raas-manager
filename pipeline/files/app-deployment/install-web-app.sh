#!/bin/bash

# This script will install the ols site as a daemon
set -e

SERVICE_NAME=raasmanagerservice

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

install_website_service(){
  local APP_SRC_DIR=/tmp/raasmanager/linux-x64/
  local APP_DEPLOYMENT_DIR=/var/aspnetcore/raasmanager/
  local EXECUTABLE_NAME=OFX.RAASManager

  log "Site Source Directory $APP_SRC_DIR"
  log "Site Deployment Directory $APP_DEPLOYMENT_DIR"

  log "Create folder location where the app will be installed: $APP_DEPLOYMENT_DIR"
  mkdir -p $APP_DEPLOYMENT_DIR

  log 'Copy RaaS Manager Service contents to deployment location'
  cp -a $APP_SRC_DIR/. $APP_DEPLOYMENT_DIR/

  log 'Copy website service definition to /etc/init.d/'
  cp /tmp/app/app-deployment/$SERVICE_NAME /etc/init.d/$SERVICE_NAME
  chmod 0755 /etc/init.d/$SERVICE_NAME
  chmod 0755 $APP_DEPLOYMENT_DIR/$EXECUTABLE_NAME
}

log 'Install dotnet core application as a daemon'

install_website_service

log 'Finished installing RaaS Manager Service'