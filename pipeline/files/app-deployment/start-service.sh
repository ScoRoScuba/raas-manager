#!/bin/bash

# This script will install the RaaS Manager Service site as a daemon
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

start_website_service(){
  /etc/init.d/$SERVICE_NAME start
}

log "Starting ${SERVICE_NAME} app"

start_website_service

log "${SERVICE_NAME} started"