- 创建数据卷
- 挂载Host目录作为数据卷
- 挂载Host的文件作为数据卷
- 数据卷容器

Docker容器一旦被删除，容器本身对应的rootfs文件系统就会被删除，容器中的所有数据也将随之消失。
Docker提供了数据卷的方式来持久话容器产生的数据，通过数据卷，还可以在容器之间共享数据。

创建容器时，通过-v参数可以数据卷，-v参数的格式为
```
[host-dir]:[container-dir]:[rw|ro]
```
其中
- host-dir表示Host机器上的目录或文件，如果目录不存在Docker会自动在Host上创建
- container-dir表示容器内部与host-dir对应的目录或文件，如果不存在Docker同样会自动创建
- rw|ro用于控制数据卷的读写权限，默认rw（可读写）

### 创建数据卷
如果不指定host-dir，Docker也会在容器内部创建目录
```
$ docker run -it --rm -v /volume1 --name testbox docker.neg/neso/busybox
```
在另一个终端执行inspect命令可以看到这种方式下，Docker会在Host的/var/lib/docker/volumes/目录生成一个随机的目录来挂载/volume1。
```
$ docker inspect -f {{.Mounts}} docker.neg/neso/busybox
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

### 挂载Host目录作为数据卷
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

### 挂载Host的文件作为数据卷
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

### 数据卷容器

参考资料
李金榜 尹烨 刘天斯 陈纯 著 《循序渐进学Docker》








