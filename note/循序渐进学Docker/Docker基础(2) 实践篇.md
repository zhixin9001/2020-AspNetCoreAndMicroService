Docker基础(2) 实践篇

- Docker的指令系统
- 全局指令
- Docker仓库管理
- Docker镜像管理
  - Dockerfile
- Docker容器管理
  - Docker Compose
- 命令的嵌套

### Docker的指令系统
Docker指令的操作对象主要针对四个方面：
- 针对守护进程的系统资源设置和全局信息的获取。比如：docker info、docker deamon等。
- 针对Docker仓库的查询、下载操作。比如：docker search、docker pull等。
- 针对Docker镜像的查询、创建、删除操作。比如：docker images、docker build等。
- 针对Docker容器的查询、创建、开启、停止操作。比如：docker ps、docker run、docker start等。

具体信息可以通过在终端输入docker可以查看，使用docker COMMAND --help还可以进一步查看某条指令的使用方式。
接下来学习一些常用的基本指令。

### 守护进程的系统资源设置和全局信息的获取
查看Docker版本信息：
```
$ docker version
```
```
Client:
 Version:           18.09.7
 API version:       1.39
 Go version:        go1.10.4
 Git commit:        2d0083d
 Built:             Fri Aug 16 14:19:38 2019
 OS/Arch:           linux/amd64
 Experimental:      false

Server:
 Engine:
  Version:          18.09.7
  API version:      1.39 (minimum version 1.12)
  Go version:       go1.10.4
  Git commit:       2d0083d
  Built:            Thu Aug 15 15:12:41 2019
  OS/Arch:          linux/amd64
  Experimental:     false

```

查看设备信息，比如服务器版本、存储驱动程序、内核版本、操作系统、总内存等：
```
$ docker info
```
```
Containers: 0
 Running: 0
 Paused: 0
 Stopped: 0
Images: 14
Server Version: 18.09.7
Storage Driver: overlay2
 Backing Filesystem: extfs
 Supports d_type: true
 Native Overlay Diff: true
Logging Driver: json-file
Cgroup Driver: cgroupfs
...
```

### Docker仓库管理
Docker hub是Docker的公有仓库，一般需要的镜像都可以从这里下载。用户还也可以上传自制的镜像。

查找镜像，下面这条命令将搜索与centos相关的镜像：
```
$ docker search centos
```

下载镜像，冒号可以指定版本号，如果省略，默认下载最新版本，相当于:latest
```
$ docker pull imageA:1.0
```

上传镜像，前提要注册Docker Hub账号，并使用*docker login*登录
```
$ docker pull imageA:1.1
```

### Docker镜像管理
查看所有本地镜像
```
$ docker images 
```
或者
```
$ docker image ls
```
每个镜像都有一个唯一的ImageID，全长128位，默认只显示12位缩写，加上-a还可以看到build命令生成的中间镜像。

Docker镜像具有分层的结构，使用docker history可以查询一个镜像分了多少层，以及每一层所作的变更
```
$ docker history dockerapitestimage:latest
```
```

IMAGE               CREATED             CREATED BY                                      SIZE                COMMENT
0112b74edea7        2 weeks ago         /bin/sh -c #(nop)  ENTRYPOINT ["dotnet" "Web…   0B
5185fbd03612        2 weeks ago         /bin/sh -c #(nop) COPY dir:9a2de08b90587d600…   294kB
d2f4b4e061f1        2 weeks ago         /bin/sh -c #(nop)  EXPOSE 5000                  0B
ca882651894c        2 weeks ago         /bin/sh -c #(nop) WORKDIR /app                  0B
e28362768eed        3 weeks ago         /bin/sh -c aspnetcore_version=3.1.1     && c…   17.8MB
<missing>           3 weeks ago         /bin/sh -c dotnet_version=3.1.1     && curl …   76.7MB
<missing>           3 weeks ago         /bin/sh -c apt-get update     && apt-get ins…   2.28MB
<missing>           3 weeks ago         /bin/sh -c #(nop)  ENV ASPNETCORE_URLS=http:…   0B
<missing>           3 weeks ago         /bin/sh -c apt-get update     && apt-get ins…   41.3MB
<missing>           3 weeks ago         /bin/sh -c #(nop)  CMD ["bash"]                 0B
<missing>           3 weeks ago         /bin/sh -c #(nop) ADD file:ba0c39345ccc4a882…   69.2MB

```

删除镜像命令为
```
$ docker image rm [Image Name]
```

#### Dockerfile
通过Dockerfile可以指定build镜像的步骤、每一步所执行的操作，只需一次编写，之后直接就可以方便地运行了。
下面便是一个最简单的Dockerfile:
```
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000
COPY . .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
```

Dockerfile的语法规则及常用的关键字为：
- 每行都以一个关键字为行首，如果一行内容过长，可使用反斜杠'\'换行
- FROM关键字，指定了build所使用的基础镜像
- MAINTAINER关键字：指定该镜像创建者
- ENV关键字：设置环境变量
- RUN关键字：运行shell命令，如果有多条命令可以用“&&”连接
- COPY关键字：将编译机本地文件拷贝到镜像文件系统中
- EXPOSE关键字：指定监听的端口
- WORKDIR关键字：指定在创建容器后，终端默认登陆进来的工作目录
- ENTRYPOINT关键字：这个关键字和前面关键字的区别在于，之前的关键字都是在创建镜像时执行，而ENTRYPOINT指定的内容在启动容器时才会执行

#### Dockerfile的执行
Dockerfile编写好后，在它所在的目录通过docker build就可以执行了，也可以通过-f指定Dockerfile所在的路径，使用-t 可以为要build的镜像指定一个名称。
```
$ docker build -t test:1.0
```
docker build会根据Dockerfile的内容，逐行根据关键字执行，每一步都会生成一个临时的中间镜像。这些中间镜像可以使用docker image ls -a查看。

### Docker容器管理
查看本机现有的容器，默认只显示正在运行的，增加-a可显示所有容器，包括停止的
```
$ docker ps -a
```

基于镜像创建并启动容器，-d 可以让容器在后台运行，--name可以指定容器名称，通过--env传递环境变量，
```
$ docker run -d --name api1 [Image Name]
```

容器创建后，Docker会给它分配一个Container ID，也是128位，默认显示前12位，通过Container ID或者创建容器时指定的name，就可以控制容器的启动停止了。

```
$ docker stop <container id/name>
```
```
$ docker start <container id/name>
```

查看容器运行日志可以使用：
```
$ docker logs <container id/name>
```

下面这条命令可以查询容器的所有基本信息，包括运行情况、存储位置、配置参数、网络设置等
```
$ docker inspect <container id/name>
```

docker inspect以JSON格式展示全部信息，也可以通过"-f"使用Golang模板来提取指定的信息
```
$ docker inspect -f {{.NetworkSettings.Bridge}} <container id/name>
```

实时查看容器所占用的系统资源，如CPU使用率、内存、网络和磁盘开销：
```
$ docker stats <container id/name>
```

在容器内部执行命令的方式
```
$ docker exec <container id/name> <cmd>
```

#### Docker Compose
稍微复杂的应用都会使用多个容器，如果要一个一个容器地启动会非常麻烦，再加上集群和容器间的依赖，将使应用的维护变得非常困难。Docker compose提供了一种最基本的多容器管理的方式。
比如，WordPress应用的创建需要下面两条指令：
```
$ docker run --name db --env MYSQL_ROOT_PASSWORD=example -d mariadb
```
```
$ docker run --name MyWordPress --link db:mysql -p 8080:80 -d wordpress
```
由于MyWordPress容器依赖其db容器，所以要先启动db容器。
停止的时候先停止MuWordPress容器，再停止db。

而使用Docker Compose就可以方便的管理存在多容器依赖的应用了。
首先需要单独安装docker-compose。
并编写docker-compose.yml文件，上面WordPress的命令改造为docker-compose.yml后的内容如下，使用yml语法：
```
wordpress: 
  image:wordpress
  links:
    - db:mysql
  ports:
    - 8080:80
db:
  image:mariadb
  environment:
    MYSQL ROOT PASSWORD:example
```
然后在docker-compose.yml文件所在目录执行
```
$ docker-compose up
```
就可以一次启动WordPress所依赖的所有容器了。

之后应用的启停也非常方便：
```
$ docker-compose stop
```
```
$ docker-compose start
```
docker-compose在启停时，会自动识别容器间的依赖顺序，根据依赖顺序逐个启停容器。

### 命令的嵌套
Docker还支持命令的嵌套执行，比如
根据容器ID查找：
```
$ docker ps -a |grep 0453e874jh93
```
只显示容器相关的镜像和容器名称：
```
$ docker ps |awk '{pring $2,$NF}'
```
批量删除已经停止运行的容器：
```
$ docker rm $(docker ps -a -q)

```

参考资料
李金榜 尹烨 刘天斯 陈纯 著 《循序渐进学Docker》








