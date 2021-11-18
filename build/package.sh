#!/bin/bash

#
# Packages the neccesary dependancies and uploads them to the build plan path in S3.
#
# The build plan path convention is as follows;
# s3://ofx-pipeline-dev/builds/[application_name]/[branch_name]/[commit_id]
#
# Required arguments are used to build the S3 path;
# ./package.sh [source-folder] [app-name] [branch-name] [commit-id] [S3PipelinePassword]
#    eg...
# ./package.sh . soe Windows2012 00000000-0000-0000-0000-000000000000 user:accesskey:secretkey
# ./package.sh ../beep soe Windows2012 test user:accesskey:secretkey
#


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

upload_to_s3(){  
  # s3://ofx-pipeline-dev/builds/[application_name]/[branch_name]/[commit_id]
  declare SOURCE_FILE=${PACKAGE_TARGET_PATH}
  declare TARGET_KEY=$(echo "s3://${S3_BUCKET_NAME}/builds/${APP_NAME_TRANSFORMED}/${BRANCH_NAME_TRANSFORMED}/${COMMIT_ID}/${ZIP_PACKAGE_NAME}.tar.gz" | tr '[:upper:]' '[:lower:]')
  log "Attempting to copy to S3. source:[${SOURCE_FILE}] target:[${TARGET_KEY}]"
  (
    aws s3 cp ${SOURCE_FILE} ${TARGET_KEY}
    log "Successfully copied to S3"
  ) || die "${BUILD_DNS}: Failed to copy to S3"
}

#-----------------------------------------------------------------------------------
# setup logging
BASE_PATH="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
LOG_FILE="${BASE_PATH}/ofx-package-sh.log"
exec > >(tee -a ${LOG_FILE}); exec 2>&1


#-----------------------------------------------------------------------------------
# validate params
log "Validating parameters..."
log "Using BASE_PATH=[${BASE_PATH}]"
PACKAGE_SOURCE_FOLDER=${1}
if [ ! "${PACKAGE_SOURCE_FOLDER}" ];then
   die "You must provide a value for PACKAGE_SOURCE_FOLDER as argument 1"
fi
log "Using PACKAGE_SOURCE_FOLDER=[${PACKAGE_SOURCE_FOLDER}]"
# The pipeline extracts the application name from the Bamboo short plan name,
# but because Bamboo appends a random number for each plan branch, we strip all
# numbers from the value to keep it the same between different plan branches.
#
# Also note that on OSX we use 'gsed' for 'sed' because it takes different args (brew install gnu-sed). 
APP_NAME=${2}
ZIP_PACKAGE_NAME=${6}
if [ ! "${APP_NAME}" ];then
   die "You must provide a value for APP_NAME as argument 2"
fi
log "Using APP_NAME=[${APP_NAME}]"
APP_NAME_TRANSFORMED=$(echo ${APP_NAME} | sed -r 's/[0-9]+$//g')
if [ ! "${APP_NAME_TRANSFORMED}" ];then
  APP_NAME_TRANSFORMED=$(echo ${APP_NAME} | gsed -r 's/[0-9]+$//g')
fi
if [ ! "${APP_NAME_TRANSFORMED}" ];then
   die "Failed to transform the value for APP_NAME_TRANSFORMED!"
fi
log "Using APP_NAME_TRANSFORMED=[${APP_NAME_TRANSFORMED}]"
BRANCH_NAME=${3}
if [ ! "${BRANCH_NAME}" ];then
   die "You must provide a value for BRANCH_NAME as argument 3"
fi
log "Using BRANCH_NAME=[${BRANCH_NAME}]"
BRANCH_NAME_TRANSFORMED=$(echo ${BRANCH_NAME} | tr / -)
if [ ! "${BRANCH_NAME_TRANSFORMED}" ];then
   die "Failed to transform the value for BRANCH_NAME_TRANSFORMED!"
fi
log "Using BRANCH_NAME_TRANSFORMED=[${BRANCH_NAME_TRANSFORMED}]"
COMMIT_ID=${4}
if [ ! "${COMMIT_ID}" ];then
   die "You must provide a value for COMMIT_ID as argument 4"
fi
S3_PIPELINE_PASSWORD=${5}
if [ ! "${S3_PIPELINE_PASSWORD}" ];then
   die "You must provide a value for S3_PIPELINE_PASSWORD as argument 5 in the format [aws-user:aws-accesskey:aws-secretkey]"
fi

log "Using COMMIT_ID=[${COMMIT_ID}]"
PACKAGE_TARGET_PATH="${BASE_PATH}/${ZIP_PACKAGE_NAME}.tar.gz"
log "Using PACKAGE_TARGET_PATH=[${PACKAGE_TARGET_PATH}]"
S3_BUCKET_NAME="ofx-pipeline-dev"
log "Using S3_BUCKET_NAME=[${S3_BUCKET_NAME}]"

#-----------------------------------------------------------------------------------
# extract AWS credentials from commandline argument and set AWS Environment variables. FORMAT - user:accesskey:secretkey
log "Attempting to extract AWS credentials from commandline argument"
IFS=':' read -a AWS_CREDENTIALS_ARRAY <<< "${S3_PIPELINE_PASSWORD}"
AWS_USER=${AWS_CREDENTIALS_ARRAY[0]}
AWS_ACCESS_KEY=${AWS_CREDENTIALS_ARRAY[1]}
AWS_SECRET_KEY=${AWS_CREDENTIALS_ARRAY[2]}
export AWS_ACCESS_KEY_ID=$AWS_ACCESS_KEY
export AWS_SECRET_ACCESS_KEY=$AWS_SECRET_KEY
log "Successfully extracted AWS credentials from commandline argument"


#-----------------------------------------------------------------------------------
# check that source folder exists
log "Checking that the source folder exists"
if [ -d "${PACKAGE_SOURCE_FOLDER}" ]; then
    log "Folder exists at [${PACKAGE_SOURCE_FOLDER}]"
else
    die "Folder missing at [${PACKAGE_SOURCE_FOLDER}]"
fi


#-----------------------------------------------------------------------------------
# pack up source files
log "Packing up source files from [${PACKAGE_SOURCE_FOLDER}] to [${PACKAGE_TARGET_PATH}]"
tar -cvzf ${PACKAGE_TARGET_PATH} -C ${PACKAGE_SOURCE_FOLDER} .


#-----------------------------------------------------------------------------------
# upload package so s3
log "Uploading package to S3"
upload_to_s3


log "Completed execution of: \"$(basename $0) ${*}\""