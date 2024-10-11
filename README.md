# Fx
Clean architecture


## 创建项目模块： Clean Architecture 通常将项目分成以下几个模块：

#### Core (Domain)：包含业务逻辑和领域对象。

#### Application：包含用例、接口和 DTO。

#### Infrastructure：包含外部基础设施实现（如数据库、文件系统等）。

#### WebAPI：包含 Web API 层。

## Openiddict 集成

#### IdentityDbContext 概述
IdentityDbContext 是一个继承自 DbContext 的类，并且在数据库中定义了与用户、角色及其关系相关的表结构。它通常会被用作管理应用程序中的身份验证系统，包括：

用户信息 (AspNetUsers 表)

角色信息 (AspNetRoles 表)

用户和角色的关联 (AspNetUserRoles 表)

用户登录信息 (AspNetUserLogins 表)

用户声明 (AspNetUserClaims 表)

角色声明 (AspNetRoleClaims 表)

用户令牌 (AspNetUserTokens 表)


