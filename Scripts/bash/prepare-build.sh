#!/bin/sh
 
echo "TRAVIS_BRANCH=$TRAVIS_BRANCH, PR=$PR, BRANCH=$BRANCH, TAG=$IMAGE_TAG"

sudo mysql -e "use mysql; update user set authentication_string=PASSWORD('r00t') where User='root'; update user set plugin='mysql_native_password';FLUSH PRIVILEGES;"
sudo mysql_upgrade -u root -pr00t
./Scripts/bash/init-db.sh


