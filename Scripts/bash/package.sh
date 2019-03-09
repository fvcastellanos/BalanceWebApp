#!/bin/sh

docker build -t $REGISTRY_NAME/BalanceWebApp:$IMAGE_TAG -f Docker/Dockerfile .
docker images
echo "$REGISTRY_PWD" | docker login -u "$REGISTRY_USER" --password-stdin $REGISTRY_NAME
docker push $REGISTRY_NAME/BalanceWebApp:$IMAGE_TAG
