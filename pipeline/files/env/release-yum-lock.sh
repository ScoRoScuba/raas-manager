cd /var/run/

echo 'removing lock file'

sudo rm -f yum.pid

echo 'lock file removed. Killing yum process'

sudo killall -9 yum

echo 'yum process killed'
