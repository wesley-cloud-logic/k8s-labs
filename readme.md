# Dev Ops Course
<!--toc:start-->
- [Dev Ops Course](#dev-ops-course)
  - [Running K8s Install](#running-k8s-install)
  - [Running Helm Install](#running-helm-install)
    - [Install Helm Chart](#install-helm-chart)
    - [Un-install Helm Chart](#un-install-helm-chart)
<!--toc:end-->
## Running K8s Install

> You must install the manifests in this order

1. ` kubectl apply -f ./k8s/app/1.namespace.yaml `  
2. `` kubectl apply -f ./k8s/app/2.deployment.yaml ``
3. `` kubectl apply -f ./k8s/app/3.configMap.yaml ``
4. `` kubectl apply -f ./k8s/app/4.service.yaml ``
5. `` kubectl apply -f ./k8s/redis/redis.yaml ``
6. `shell kubectl apply -f ./k8s/redis/service.yaml`

## Running Helm Install

### Install Helm Chart

To install the chart I have included go to the helm folder in your terminal.
Then run the following command

```
```shell
helm upgrade mydemo demoapp -n test --create-namespace --install
```

### Un-install Helm Chart

To remove the helm chart run the following

```shell
helm uninstall mydemo -n test
```
