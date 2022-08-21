 <img align="left" width="116" height="116" alt="kit.CleanArchitecture logo" src="https://raw.githubusercontent.com/Kit086/CleanArchitecture/main/.github/icon.png" />

# Clean Architecture Solution Template

这是一个 Web API 的 Clean Architecture 的解决方案模板，可用于学习、个人项目和企业项目。

## Table Of Content

- [Clean Architecture Solution Template](#clean-architecture-solution-template)
  - [Table Of Content](#table-of-content)
  - [Technologies](#technologies)
  - [Give a Star! ⭐](#give-a-star-)
  - [Getting Started](#getting-started)
    - [使用该模板构建自己的项目](#使用该模板构建自己的项目)
    - [使用内存数据库](#使用内存数据库)
    - [使用 docker compose](#使用-docker-compose)
    - [使用实例数据库](#使用实例数据库)
    - [如何添加 Migration](#如何添加-migration)
  - [Tests](#tests)
    - [使用 docker 的干净集成测试](#使用-docker-的干净集成测试)
    - [使用 docker-compose 的接收测试](#使用-docker-compose-的接收测试)
  - [Todo](#todo)
  - [Not todo](#not-todo)
  - [这个项目的故事](#这个项目的故事)

## Technologies

该项目使用了以下技术和库：
1. ASP.NET Core 6
2. Entity Framework Core 6 - postgres
3. Microsoft ASP.NET Core Identity with JWT
4. MediatR
5. AutoMapper
6. FluentValidation
7. XUnit, FluentAssertions, NSubstitute, SpecFlow & TestContainers
8. Docker & Docker Compose

如果您初入 dotnet 的坑不久，相信该项目中用到的一些技术和库会是您感兴趣的，有助于入门。

## Give a Star! ⭐

如果您喜欢这个项目，或正在使用这个项目来学习，或使用这个模板构建你的解决方案，请给它一个 Star，谢谢! 

每个人对 Clean Architecture 都有自己的需求，如果您觉得该项目有可取之处，强烈建议您 fork 这个项目来构建您自己的 Clean Architecture 模板。

如果您喜欢我的代码，欢迎关注我的微信公众号：

<img width="128" alt="my wechat subscription account qr code" src="https://raw.githubusercontent.com/Kit086/CleanArchitecture/main/.github/wechat_gzh_qrcode.png" />

以及我的博客：[https://blog.kitlau.dev/](https://blog.kitlau.dev/)

## Getting Started

### 使用该模板构建自己的项目

该模板尚未上传到 Nuget，如果您想安装该模板到 dotnet new 的菜单中，可以按如下步骤操作：
1. 确定您的设备已经安装 [dotnet 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. clone 该仓库，在解决方案的根目录下运行 `dotnet new --install .\` 命令，即可安装该模板：
    ```powershell
    PS C:\Users\kit\Documents\GitHub\CleanArchitecture> dotnet new --install .\
    The following template packages will be installed:
       C:\Users\kit\Documents\GitHub\CleanArchitecture
    
    Success: C:\Users\kit\Documents\GitHub\CleanArchitecture installed the following templates:
    Template Name                    Short Name  Language  Tags
    -------------------------------  ----------  --------  ------------------------------
    Kit Clean Architecture Solution  kit-ca-sln  [C#]      Web/ASP.NET/Clean Architecture
    ```
3. 为您的解决方案创建一个文件夹，并 cd 到该文件夹下，模板将使用该文件夹名作为项目名称
4. 运行 `dotnet new kit-ca-sln` 即可创建基于该模板的项目
5. 继续往下看该文档，选择一种方式运行该项目

### 使用内存数据库

只需要以下环境：
- [dotnet 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

在解决方案根目录中运行 `dotnet run --project .\src\WebAPI\` 即可。默认情况下使用 InMemoryDatabase 内存数据库。

因为在 Development 环境下，会生成基于 swagger-ui 的 API 文档，访问 `https://localhost:7279/swagger/index.html` 查看该文档。

如果想使用实例数据库，可以继续往下看。

### 使用 docker compose

也可以选择完全使用 docker compose 运行该项目，程序与数据库均运行于 docker 容器中：
1. 根据您自己的需求，调整 src/WepAPI 目录中 Dockerfile 和 .dockerignore 的各项配置
2. 根据您自己的需求，调整解决方案根目录中 docker-compose.yml 中的各项配置
3. 在解决方案根目录中运行命令 `docker compose up` 即可
如果您自定义配置后，环境变量 ASPNETCORE_ENVIRONMENT 依然为 Development，则依旧会生成 API 文档。

### 使用实例数据库

首先需要有一个 Postgres 数据库实例，没有也没关系，可以用 Docker 方便快捷地解决：
1. 如果运行在 Windows 系统，建议 Docker 使用基于 WSL2 的引擎（Use the WSL 2 based engine），因为该项目默认的 DockerDefaultTargetOS 是 Linux，不确定不使用 WSL2 会出什么奇怪问题
2. 方案 1：运行以下命令跑一个 Postgres 的容器。请将 -e 的环境变量的值替换为您自己想要的值：
    ```powershell
    docker run --name postgres_default -e POSTGRES_USER=kitlau -e POSTGRES_PASSWORD=password -p 5432:5432 -d postgres:latest
    ```
3. 方案 2：在解决方案根目录中运行以下命令：
    ```powershell
    docker compose up -d clean_architecture.database
    ```
   会启动 docker-compose.yml 中配置的 Postgres 容器，默认 Port: 10002，Username: kitlau, Password: password。您也可以手动编辑 docker-compose.yml 进行配置。
4. 修改 src/WebAPI/ 目录中的 appsettings.json 的配置：
    ```json
    "UseInMemoryDatabase": false, // true 改为 false，表示不启用内存数据库
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Username=kitlau;Password=password;Database=CleanArchitecture" // 按照您使用的数据库参数修改连接字符串
    },
    ```
在解决方案根目录中运行 `dotnet run --project .\src\WebAPI\` 即可。此时已经使用实例数据库。

### 如何添加 Migration

如果您使用该项目为模板，进行了需要添加新 Migration 的改动，可以在解决方案根目录下运行以下命令添加 Migration：

```powershell
dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\WebUI --output-dir Persistence\Migrations
```

## Tests

该项目非常适合入门单元测试，集成测试和接收测试。在 tests 目录中的各个测试项目使用了以下常用技术：
1. XUnit - 测试框架
2. FluentAssertions - 非常方便的 Assert 库
3. NSubstitute - 用于代替 Moq 的库，我个人认为比 Moq 更易于使用
4. SpecFlow - 常用于集成测试和接收测试

测试项目还有一些特点：

### 使用 docker 的干净集成测试

tests/Application.Tests.Integration 使用 postgres docker 容器作为测试数据库，每个测试类/方法使用单独的数据库容器，使各测试类/方法之间互不影响，可以尽情对数据进行任何操作。

各容器会在运行测试时自动创建，使用完毕后自动停止自动删除，干净利落。

### 使用 docker-compose 的接收测试

tests/接收测试的环境和数据库会通过 docker compose 创建，你不需要手动运行项目，只运行测试即可。

## Todo

未来计划：
1. 替换掉常规业务逻辑中抛异常的报错模式，改为直接返回 Error - 这种方式正日趋流行
2. 优化 EF Core 配置的方式 - 目前的配置方式略显笨拙
3. 优化授权认证代码 - 目前在认证出错时返回值很乱

## Not todo

本解决方案将不会：
1. 引入 Serilog 或 NLog 等日志库 - 引入日志库即使对初学者来说也并非难事，而且每个人的选择可能不同，引入我自己喜欢的库反而使这个解决方案模板不够 clean

## 这个项目的故事

该项目最初使用 [jasontaylordev/CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture) 模板构建，虽重写了大部分代码，但总体的 Architecture 还是相似的。

目前有很多优秀的 Clean Architecture 模板，例如 [ardalis/CleanArchitecture](https://github.com/ardalis/CleanArchitecture)。这些项目中我最喜欢 jason taylor 的架构和分层方式，所以选择了基于他的模板。

我起初只是想使用 Clean Architecture 的模板构建项目，节省时间。后来我在使用模板过程中发现它依赖了 Microsoft.AspNetCore.ApiAuthorization.IdentityServer 这个库，该库又依赖了 Duende.IdentityServer，它是一个付费的库。 很多像我一样的贫困开发者承担这些付费库很困难。

<center><img width="768" alt="ip man said I am not gonna give you the money" src="https://raw.githubusercontent.com/Kit086/CleanArchitecture/main/.github/ip_man01.jpg" /></center>

而且 jason taylor 的项目直接在 WebUI 层加了一个 Angular 的 SPA 模板，swagger 的生成方式也很奇怪，调用了本地的 C 盘里的一个 dll。可能因为我本地环境有点乱，我运行的前 4 次就失败了两次，无法直接运行起来使我难以接受。

后来我又看了各个流行的 Clean Architecture 模板，但是它们都有各种不合我胃口的地方，例如 ardalis 的模板使用了 Autofac，它很好，只是我觉得引入了它使不符合我对 clean 的期待了。

这几个项目都有各自合与不合我胃口的点，所以对我来说：

<center><img width="768" alt="ip man said each master has his own strengths and weaknesses" src="https://raw.githubusercontent.com/Kit086/CleanArchitecture/main/.github/ip_man02.jpg" /></center>

最终只能自己造这个轮子。重写了 Identity 相关功能和各个测试项目。该项目本身代码是 clean 的，仅仅在运行测试时比较依赖 docker。如果您不喜欢 docker，可以轻松地改写或删除对 TodoList 和 TodoItem 功能的这些依赖 docker 的测试，毕竟您的项目大概率用不到这些功能。

在我看来，写这个项目的学习意义大于实用意义。只有少量功能的项目不需要 Clean Architecture，用它只会多写很多 Query/Command 和 Handler，徒增代码的复杂程度，反而直接三层，一层，甚至 MinimalAPI 都可以；写大型项目又有为大型项目准备的框架。但是在写它时让我思考了很多问题，通过解决这些问题，又学会了很多东西，增长了很多经验。