#!/bin/sh

echo "TRAVIS_BRANCH=$TRAVIS_BRANCH, PR=$PR, BRANCH=$BRANCH, TAG=$IMAGE_TAG"

docker build -t $REGISTRY_NAME/balance-webapp:$IMAGE_TAG -f Docker/Dockerfile .
docker images
echo "$REGISTRY_PWD" | docker login -u "$REGISTRY_USER" --password-stdin $REGISTRY_NAME
docker push $REGISTRY_NAME/balance-webapp:$IMAGE_TAG
