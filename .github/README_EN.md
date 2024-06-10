<p align="center"> 
<img src="RES/Logo.svg" width="256" height="256" alt="https://github.com/AIO-GAME"> 
</p>
<p align="center" style="font-size: 24px;"> 
<b>Unity Timer</b>
</p>
<p align="center"><a href="README_EN.md">简体中文</a> | English</p>
<p align="center">
<a href="https://github.com/AIO-GAME/Unity.Timer/security/policy"> 
<img alt="" src="https://img.shields.io/github/package-json/unity/AIO-GAME/Unity.Timer"> 
</a>
<a href="https://github.com/AIO-Game/Unity.Timer">
<img src="https://img.shields.io/github/license/AIO-Game/Unity.Timer" alt=""/>
</a>
<a href="https://github.com/AIO-Game/Unity.Timer">
<img src="https://img.shields.io/github/languages/code-size/AIO-Game/Unity.Timer?label=size" alt=""/>
</a>
<a href="https://openupm.com/packages/com.aio.timer/">
<img src="https://img.shields.io/npm/v/com.aio.timer?label=openupm&amp;registry_uri=https://package.openupm.com" alt=""/>
</a>
</p>

## ⚙ Install

<details>
<summary>
<span style="color: deepskyblue; "><b>Packages Manifest</b></span>
</summary>

````json
{
  "dependencies": {
    "com.aio.timer": "latest"
  },
  "scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.aio.timer"
      ]
    }
  ]
}
````

</details>

<details>
<summary>
<span style="color: deepskyblue; "><b>Unity PackageManager</b></span>
</summary>

> open upm *Chinese Version*

~~~
Name: package.openupm.cn
URL: https://package.openupm.cn
Scope(s): com.aio.timer
~~~

> open upm *International Version*

~~~
Name: package.openupm.com
URL: https://package.openupm.com
Scope(s): com.aio.timer
~~~

</details>

<details>
<summary>
<span style="color: deepskyblue; "><b>Command Line</b></span>
</summary>

> open *upm-cli*

~~~
openupm add com.aio.timer
~~~

</details>

## ⭐ About

- **这是一个 高效时间轮 定时器工具类**
- ✅ **支持 Unity 2019.4 LTS 或更高版本**
- ✅ **支持 .NET 4.x 和 .NET Standard 2.0**
- ✅ **支持 自定义定时次数**
- ✅ **支持 子线程后台运行或主线程协程运行**
- ✅ **支持 自定义时间精度单位**
- ✅ **支持 自定义时间轮大小**
- ✅ **支持 时间轮动态扩容**
- ✅ **支持 同时添加1000000+ 定时任务**

## 📚 Usage

<h4>添加定时任务</h4>

```csharp 
TimerSystem.Push(1, () => { Debug.Log("1s"); });
TimerSystem.Push(2, () => { Debug.Log("2s"); });
``` 

<h4>添加循环定时任务</h4>

```csharp
TimerSystem.PushLoop(tid, 3, () => { Debug.Log("3s"); });
``` 

<h4>移除循环定时任务</h4>

```csharp
TimerSystem.Pop(tid);
```  

## ✨ Contributors

<!-- readme: collaborators,contributors -start -->
<table>
	<tbody>
		<tr>
            <td align="center">
                <a href="https://github.com/xinansky">
                    <img src="https://avatars.githubusercontent.com/u/45371089?v=4" width="64;" alt="xinansky"/>
                    <br />
                    <sub><b>xinansky</b></sub>
                </a>
            </td>
		</tr>
	<tbody>
</table>
<!-- readme: collaborators,contributors -end -->

## 📢 Thanks

- **Thanks for using this software.**
- **If this package is useful to you.**
- **Please ⭐ this repository to support the project.**