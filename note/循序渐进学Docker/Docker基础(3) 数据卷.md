- 创建数据卷
- 挂载Host目录作为数据卷
- 挂载Host的文件作为数据卷
- 数据卷容器
- 数据卷的备份和恢复

Docker容器一旦被删除，容器本身对应的rootfs文件系统就会被删除，容器中的所有数据也将随之消失。
Docker提供了数据卷的方式来持久化容器产生的数据，通过数据卷，还可以在容器之间共享数据。

创建容器时，通过-v参数可以数据卷，-v参数的格式为
```
[host-dir]:[container-dir]:[rw|ro]
```
其中
- host-dir表示Host机器上的目录或文件，如果目录不存在Docker会自动在Host上创建
- container-dir表示容器内部与host-dir对应的目录或文件，如果不存在Docker同样会自动创建
- rw|ro用于控制数据卷的读写权限，默认rw（可读写）

## 创建数据卷
如果不指定host-dir，Docker也会在容器内部创建目录
```
$ docker run -it --rm -v /volume1 --name testbox busybox
```
在另一个终端执行inspect命令可以看到这种方式下，Docker会在Host的/var/lib/docker/volumes/目录生成一个随机的目录来挂载/volume1。
```
$ docker inspect -f {{.Mounts}} busybox
```
```
"Mounts": [
	{
		"Type": "volume",
		"Name": "112e8ff579fcda26cd933ea37946bf7a9b53b0f2bf87382184731884990e2746",
		"Source": "/var/lib/docker/volumes/112e8ff579fcda26cd933ea37946bf7a9b53b0f2bf87382184731884990e2746/_data",
		"Destination": "/volume1",
		"Driver": "local",
		"Mode": "",
		"RW": true,
		"Propagation": ""
	}
],
```

## 挂载Host目录作为数据卷
也可以挂载Host目录作为容器的数据卷
```
docker run -it --rm -v /home/dotnet2/:/volume1 --name testbox busybox
```
这时会将host的/home/dotnet2/目录挂载，对应容器的/volume1目录，在目录下执行ls可以看到同样的内容。而且在不管在Host还是容器内修改这个目录中的内容，两边都可以生效。
```
在Host修改
$ echo "hello v124"> testzzx.txt
在容器查看
# cat testzzx.txt
# hello v124
```

挂载时通过ro变量，还可以设置目录为只读
```
docker run -it --rm -v /home/dotnet2/:/volume1:ro --name testbox busybox
```
此时如果在容器内尝试修改这个目录中的内容会失败，但在Host还是可以修改的。
```
# echo "hello v125"> testzzx.txt
sh: can't create testzzx.txt: Read-only file system
```

## 挂载Host的文件作为数据卷
```
docker run -it --rm -v /home/dotnet2/testzzx.txt:/volume1 --name testbox busybox
```
此时容器内的volume1实际上是文件，内容与testzzx.txt一样。而如果这样
```
docker run -it --rm -v /home/dotnet2/testzzx.txt:/volume1/testzzx.txt --name testbox busybox
```
Docker才会在容器内部创建volume1目录并在这个目录下挂载testzzx.txt。
同挂载目录一样，也可以设置只读
```
docker run -it --rm -v /home/dotnet2/testzzx.txt:/volume1/testzzx.txt:ro --name testbox busybox
```

这种挂载Host文件的方式可用来在Host与容器之间共享配置文件，这样只需在Host修改配置，所有挂载的容器都会生效。

## 数据卷容器
数据卷容器提供了一种在容器间共享数据的更强大的方式。

首先创建一个命名的数据卷容器供其他容器挂载。
```
$ docker run -it --rm -v /volume1 --name testbox busybox
```

挂载容器使用--volumes-from参数
```
$ docker run -it --rm --volumes-from testbox --name testboxvf1 busybox
```
这个容器启动后也可以看到volume1目录，而且在数据卷容器的volume1进行的操作在testboxvf1容器可以即时生效。
可以同时使用多个--volumes-from参数，从多个容器挂载多个数据卷。

此外还可以从其他已经挂载容器卷的容器（如testboxvf1）挂载数据卷：
```
$ docker run -it --rm --volumes-from testboxvf1 --name testboxvf2 busybox
```
在容器testboxvf2内部也出现了volume1目录，如果之前在数据卷容器或者testboxvf1新建了文件，在这里可以读取、修改

如果删除挂载了数据卷的容器（包括初始的testbox容器和其他的容器testboxvf1、testboxvf2），数据卷并不会被删除。如果想删除该数据卷，需要在删除最后一个引用该数据卷的时候调用*docker rm -v*显式删除数据卷。

## 数据卷的备份和恢复
除了Docker容器数据的持久化，在使用数据卷容器时，还会面对数据的备份和恢复的问题。

### 备份
备份数据卷可以通过这样的方式：
```
$ docker run --volumes-from testbox -v $(pwd):/backup --name testboxbak busybox tar cvf /backup/backup.tar /volume1
```
这条命令会挂载数据卷容器，并将Host当前目录挂载为容器的backup目录，然后使用tar命令将数据卷容器的volume1目录压缩放置到backup目录下，这样回到Host后，就可以在当前目录拿到backup.tar了。

### 恢复
恢复时可以直接恢复到原有容器或者其他任何容器。假设想把backup.tar的数据恢复到一个新的数据卷容器testbox2，首先启动testbox2：
```
$ docker run -it --rm -v /volume1 --name testbox2 busybox
```
然后开始恢复，下面这条命令会挂载数据卷容器，并将Host当前目录挂载为容器的volume2目录，因为backup.tar在Host的当前目录，所以在容器中执行解压操作时的路径为/volume2/backup.tar，这样就把解压后的volume1目录放在了在容器的根目录，volume1也是数据卷容器的共享目录，于是恢复完成。
```
$ docker run -it --rm --volumes-from testbox2 -v $(pwd):/volume2 --name testboxbak1 busybox tar xvf /volume2/backup.tar
```

参考资料
李金榜 尹烨 刘天斯 陈纯 著 《循序渐进学Docker》








