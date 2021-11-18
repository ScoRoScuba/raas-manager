#!/bin/bash

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


delete_old_curl()
{
    log '1 - Delete Old Curl'

    rm -f /usr/bin/curl
    rm -f rm /lib64/libcurl*
}

install_prerequisites()
{
    log '2 - Install curl open ssl  prerequisites'

    yum install -y \
        wget \
        epel-release 

    yum install -y \
        openssl-devel \
        libnghttp2-devel \
        libidn-devel \
        gcc 
}

download_and_install_curl_with_open_ssl()
{
    log '5 - Download CURL required for System.Net.Http'

    wget https://curl.haxx.se/download/curl-7.56.1.tar.gz

    log '6 - un-tar curl'
    tar -xf curl-7.56.1.tar.gz

    cd curl-7.56.1

    log '7 - Compile Curl'

    ./configure \
        --disable-dict \
        --disable-file \
        --disable-ftp \
        --disable-gopher \
        --disable-imap \
        --disable-ldap \
        --disable-ldaps \
        --disable-libcurl-option \
        --disable-manual \
        --disable-pop3 \
        --disable-rtsp \
        --disable-smb \
        --disable-smtp \
        --disable-telnet \
        --disable-tftp \
        --enable-ipv6 \
        --enable-optimize \
        --enable-symbol-hiding \
        --with-ca-bundle=/etc/pki/tls/certs/ca-bundle.crt \
        --with-nghttp2 \
        --with-gssapi \
        --with-ssl \
        --without-librtmp \
        --prefix=$PWD/install/usr/local \
    && \
    make install

    log '8 - Now pack the compiled CURL'
    cd install
    tar -czf curl-7.56.1-RHEL6-x64.tgz *

    log '9 - Extract compiled curl into /'
    tar -xf curl-7.56.1-RHEL6-x64.tgz -C /
}

export_LD_LIBRARY_PATH()
{
    echo '# Export LD_LIBRARY_PATH' >> curl-openssl.sh
    echo "export LD_LIBRARY_PATH=/usr/local/lib" >> curl-openssl.sh

    log 'Copying dotnet.sh to profile.d so it gets executed for each user'
    cp curl-openssl.sh /etc/profile.d/curl-openssl.sh
}



delete_old_curl
install_prerequisites
download_and_install_curl_with_open_ssl
export_LD_LIBRARY_PATH