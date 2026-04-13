# USBWatcher - U盘占用检查工具

一款简洁高效的 Windows 工具，用于检测并解除 U 盘占用进程。

## 功能特性

- 自动检测插入的可移动U盘
- 扫描占用U盘的进程
- 一键结束占用进程
- 安全弹出U盘提醒

## 系统要求

- Windows 10/11 (x64)
- 无需额外安装 .NET 运行时（已内置）

## 使用方法

1. 运行 `USBWatcher.exe`
2. 选择要检查的U盘
3. 点击「扫描占用」查看占用进程
4. 可选择「结束进程」强制关闭
5. 点击「安全弹出U盘」确认无误后拔出

## 权限说明

部分操作需要管理员权限，如无法结束进程请右键以管理员身份运行。

## 下载

前往 [Releases](https://github.com/BTCUP99/USBWatcher/releases) 页面下载最新版本。

## 构建

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```
