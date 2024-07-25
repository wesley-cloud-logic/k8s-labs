# Dev Ops Course


## Running K8s Install

> You must install the manifests in this order 

1. `` kubectl apply -f ./k8s/app/1.namespace.yaml ``
2. `` kubectl apply -f ./k8s/app/2.deployment.yaml ``
3. `` kubectl apply -f ./k8s/app/3.configMap.yaml ``
4. `` kubectl apply -f ./k8s/app/4.service.yaml ``
5. `` kubectl apply -f ./k8s/redis/redis.yaml ``
6.`` kubectl apply -f ./k8s/redis/service.yaml ``

## Running Helm Install 

### Install Helm Chart
To install the chart I have included go to the helm folder in your terminal. 
Then run the following command
```
``
helm upgrade mydemo demoapp -n test --create-namespace --install ``

Un-install Helm Chart
To remove the helm chart run the following

``
helm uninstall mydemo -n test
``
```
