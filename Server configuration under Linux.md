1、Configure the compilation environment of the operating system

- yum install -y gcc gcc-c++ kernel-devel
- yum install -y openssl-devel
- yum install -y mysql-devel
- yum install -y autoconf
- yum install -y automake
- yum install -y libtool
- yum install -y libffi-devel
- yum install -y unzip zip
- yum install -y zlib-devel
- yum install -y bzip2-devel
- yum install -y ncurses-devel
- yum install -y sqlite-devel
- yum install -y readline-devel
- yum install -y tk-devel
- yum install -y gdbm-devel
- yum install -y db4-devel
- yum install -y libpcap-devel
- yum install -y xz-devel

2、Install Python

- wget https://www.python.org/ftp/python/3.7.3/Python-3.7.3.tgz
- tar -zxvf [Python-3.7.3.tgz](https://www.python.org/ftp/python/3.7.2/Python-3.7.2.tgz)
- mkdir /usr/local/python3
- cd Python-3.7.3
- ./configure --prefix=/usr/local/python3
- make && make install
- ln -s /usr/local/python3/bin/python3.7 /usr/bin/python3
- ln -s /usr/local/python3/bin/pip3.7 /usr/bin/pip3

3、Update the MySQL version in the yum repository

- wget https://dev.mysql.com/get/mysql57-community-release-el7-11.noarch.rpm
- yum localinstall mysql57-community-release-el7-11.noarch.rpm

4、Install MySQL

- rpm --import https://repo.mysql.com/RPM-GPG-KEY-mysql-2022
- yum install -y mysql-community-server

5、Start the MySQL service

- systemctl start mysqld.service

6、Change MySQL default configuration

- vi /etc/my.cnf
  - Turn off password verification: validate-password=OFF
  - Set case sensitivity: lower_case_table_names=2

7、View default password

- sudo grep 'temporary password' /var/log/mysqld.log

8、Restart the database to make the configuration take effect

- systemctl restart mysqld.service

9、Compile KBEngine engine

- Download KBE engine source code
  - The engine must use v2.3.6 (or the corresponding 1.X stable version) and above (embedded Python3.7.2)
  - When compiling the engine, select the 64-bit release version
- cd kbengine/kbe/src
- chmod -R 755 **.**
- make

10、Add an account for KBEngine to access the database