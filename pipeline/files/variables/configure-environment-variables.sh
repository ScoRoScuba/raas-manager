#!/bin/bash
# This script will set in the EC2 machine 
set -e

log(){
  echo "INFO:  $(date): ${*}"
}

configure-environment-variables(){

  log 'Copying context_vars.sh to profile.d so it gets executed for each user...'
  # /etc/profile.d is a directory were the contents of it gets executed for each user
  cp /tmp/app/variables/context_vars.sh /etc/profile.d/context_vars.sh

  log "Context variables added to environment"
}

log '1. Setting environment variables'

configure-environment-variables
