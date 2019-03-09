#!/bin/sh

export BRANCH=$(if [ "$TRAVIS_PULL_REQUEST" == "false" ]; then echo $TRAVIS_BRANCH; else echo $TRAVIS_PULL_REQUEST_BRANCH; fi)
export IMAGE_TAG=$(echo $BRANCH | tr / _)
echo "TRAVIS_BRANCH=$TRAVIS_BRANCH, PR=$PR, BRANCH=$BRANCH, TAG=$IMAGE_TAG"

docker build -t $REGISTRY_NAME/BalanceWebApp:$IMAGE_TAG -f Docker/Dockerfile .
docker images
echo "$REGISTRY_PWD" | docker login -u "$REGISTRY_USER" --password-stdin $REGISTRY_NAME
docker push $REGISTRY_NAME/BalanceWebApp:$IMAGE_TAG
