Docker基础(2) 实践篇

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
在另一个终端执行inspect命令可以看到这种方式下，Docker会在Host的/var/lib/docker/volumes/目录生成一个随机的目录来挂在/volume1
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

```
docker run -it --rm -v /home/es2/dotnet2/:/volume1:ro --name testbox docker.neg/neso/busybox
```
```
docker run -it --rm -v /home/es2/dotnet2/:/volume1:ro --name testbox docker.neg/neso/busybox
```

参考资料
李金榜 尹烨 刘天斯 陈纯 著 《循序渐进学Docker》








