# Nova-Dynamic-Island
灵动岛（Nova-Dynamic-Island），基于.NET 6.0开发的Windows平台的桌面辅助软件

# 安装
在文件夹<pre>灵动岛Core</pre>打开终端、命令提示符，然后输入
<pre>dotnet run </pre>

# API接口
您可以POST本地的API接口来实现显示通知帮助您的开发
地址：
<pre>
  Http://localhost:24305/
</pre>

### HTTP 接口参数

| 参数名 | 类型 | 描述 | 必须|
|-------|------|------|------|
| AnimationTime | double | 动画时间（以秒为单位） |✔|
| ShowTime | int | 显示时间（以秒为单位） |✔|
| body | string | 内容主体 |✔|
| color | string | 颜色代码（十六进制格式，例如：#FF0000） |✔|
| icon | string | 图标的 Base64 编码 |✔|

### 功能介绍

该接口用于向服务器发送显示内容的请求。参数描述如下：

- `AnimationTime`: 指定内容显示的动画时间。
- `ShowTime`: 设置内容在屏幕上显示的时间长度。
- `body`: 要显示的内容主体。
- `color`: 内容的文字颜色，使用十六进制表示。
- `icon`: 以 Base64 编码表示的图标。

### 示例

#### 请求示例：

```http
POST /show-content HTTP/1.1
Host: example.com
Content-Type: application/x-www-form-urlencoded

AnimationTime=2.5&ShowTime=10&body=Hello%20world&color=%23FFA500&icon=base64encodedicon
