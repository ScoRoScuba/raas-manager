#!/bin/bash
set -e

log(){
  echo "INFO:  $(date): ${*}"
}

export NEW_RELIC_CONFIG=/etc/newrelic-infra.yml

#Include the variables from the ssm context
source $1

log "Installing the NewRelic Infrastructure agent..., with Licence key ${NewRelicKey}"
log "   Documentation : https://docs.newrelic.com/docs/infrastructure/new-relic-infrastructure/installation/install-infrastructure-linux"
log "   - creating the license key file at ... ${NEW_RELIC_CONFIG}"
echo "license_key: ${NewRelicKey}" | sudo tee -a ${NEW_RELIC_CONFIG}
log "   - creating yum repo for new relic..."
sudo curl -o /etc/yum.repos.d/newrelic-infra.repo https://download.newrelic.com/infrastructure_agent/linux/yum/el/6/x86_64/newrelic-infra.repo
log "   - updating yum cache..."
sudo yum -q makecache -y --disablerepo='*' --enablerepo='newrelic-infra'
log "   - running installer script..."
sudo yum install newrelic-infra -y

sudo rpm -Uvh http://yum.newrelic.com/pub/newrelic/el5/x86_64/newrelic-repo-5-3.noarch.rpm
cat << REPO | sudo tee "/etc/yum.repos.d/newrelic-netcore20-agent.repo"
[newrelic-netcore20-agent-repo]
name=New Relic .NET Core Agent packages for Enterprise Linux
baseurl=http://yum.newrelic.com/pub/newrelic/el7/\$basearch
enabled=1
gpgcheck=1
gpgkey=file:///etc/pki/rpm-gpg/RPM-GPG-KEY-NewRelic
REPO

sudo yum -y install newrelic-netcore20-agent