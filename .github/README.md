<p align="center"> 
<img src="RES/Logo.svg" width="256" height="256" alt="https://github.com/AIO-GAME"> 
</p>
<p align="center" style="font-size: 24px;"> 
<b>Unity Timer</b>
</p>
<p align="center"><a href="README_EN.md">English</a> | 简体中文</p>
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

## ⚙ 安装

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
        "com.aio.timer",
        "com.aio.runner"
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

> open upm *中国版*

~~~
Name: package.openupm.cn
URL: https://package.openupm.cn
Scope(s): com.aio.timer
~~~

> open upm *国际版*

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

## ⭐ 关于

- **这是一个 高效时间轮 定时器工具类**
- ✅ **支持 Unity 2019.4 LTS 或更高版本**
- ✅ **支持 .NET 4.x 和 .NET Standard 2.0**
- ✅ **支持 自定义定时次数**
- ✅ **支持 子线程后台运行或主线程协程运行**
- ✅ **支持 自定义时间精度单位**
- ✅ **支持 自定义时间轮大小**
- ✅ **支持 时间轮动态扩容**
- ✅ **支持 同时添加1000000+ 定时任务**

## 📚 使用

<h4>初始化</h4>

```csharp 
TimerSystem.Initialize("updateLimit:long=10","capacity:int=8196");
``` 

<h4>自定义时间轮精度</h4>

```csharp 
TimerSystemSettings.TimingUnitsEvent += Week;

public static void Week(ICollection<(long, long, long)> units)
{
    var DistanceUnit = 2; // ms
    var MS_SECOND = 1000;
    var MS_MIN = 1000 * 60;
    var MS_HOUR = MS_MIN * 60;
    var MS_DAY = MS_HOUR * 24;
    var MS_WEEK = MS_DAY * 7;
    units.Add((MS_SECOND, DistanceUnit, MS_SECOND / DistanceUnit));
    units.Add((MS_MIN, MS_SECOND, 60));
    units.Add((MS_HOUR, MS_MIN, 60));
    units.Add((MS_DAY, MS_HOUR, 24));
    units.Add((MS_WEEK, MS_DAY, 7));
}
``` 

<h4>添加定时任务</h4>

```csharp 
TimerSystem.Push(1, () => { Debug.Log("1ms"); });
TimerSystem.Push(2, () => { Debug.Log("2ms"); });
TimerSystem.Push(1000, () => { Debug.Log("2s"); });
``` 

<h4>添加循环定时任务</h4>

```csharp
TimerSystem.PushLoop(tid, 3, () => { Debug.Log("3ms"); });
``` 

<h4>移除循环定时任务</h4>

```csharp
TimerSystem.Pop(tid);
```  

## ✨ 贡献者

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
            <td align="center">
                <a href="https://github.com/Starkappa">
                    <img src="https://avatars.githubusercontent.com/u/155533864?v=4" width="64;" alt="Starkappa"/>
                    <br />
                    <sub><b>Starkappa</b></sub>
                </a>
            </td>
		</tr>
	<tbody>
</table>
<!-- readme: collaborators,contributors -end -->

## 📢 致谢

- **谢谢您选择我们的扩展包。**
- **如果此软件包对您有所帮助。**
- **请考虑通过添加⭐来表示支持。**