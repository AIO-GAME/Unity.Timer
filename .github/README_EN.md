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

- **This is a high-performance timer tool class based on the efficient time wheel.**
- **Support Unity 2019.4 LTS or higher.**
- **Support .NET 4.x and .NET Standard 2.0.**
- **Support custom timing times.**
- **Support sub-thread background operation or main thread coroutine operation.**
- **Support custom time precision unit.**
- **Support custom time wheel size.**
- **Support time wheel dynamic expansion.**
- **Support adding 1000000+ timing tasks at the same time.**

## 📚 Usage

<h4>Initialize</h4>

```csharp 
TimerSystem.Initialize("updateLimit:long=10","capacity:int=8196");
``` 

<h4>Custom time wheel accuracy</h4>

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

<h4>Add timing task</h4>

```csharp 
TimerSystem.Push(1, () => { Debug.Log("1s"); });
TimerSystem.Push(2, () => { Debug.Log("2s"); });
``` 

<h4>Remove timing task</h4>

```csharp
TimerSystem.PushLoop(tid, 3, () => { Debug.Log("3s"); });
``` 

<h4>Remove timing task</h4>

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