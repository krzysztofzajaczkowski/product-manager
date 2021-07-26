[![LinkedIn][linkedin-shield-zajaczkowski]][linkedin-url-zajaczkowski] [![Build Status](https://travis-ci.com/krzysztofzajaczkowski/product-manager.svg?branch=develop)](https://travis-ci.com/krzysztofzajaczkowski/product-manager) 
[![codecov](https://codecov.io/gh/krzysztofzajaczkowski/product-manager/branch/infra/34_code-coverage-workflow/graph/badge.svg?token=0IC3R4RHQC)](https://codecov.io/gh/krzysztofzajaczkowski/product-manager)


# Product Manager

## Table of Contents

* [About the Project](#about-the-project)
  * [Built With](#built-with)
* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Development](#development)
* [Deployment](#deployment)
	* [Build](#build)
* [Functionality](#functionality)
* [Screenshots](#screenshots)

## About the project

Desktop application built using Electron<area>.NET, Angular and ASP<area>.NET Core 5, having JWT authentication and role based access control with static separation of roles(user can be logged in only with one role at a time). 
Back-end was created with TDD, using unit and integration tests. All use cases are tested with acceptance tests using Docker and FluentDocker. ValueObjects are used to stop relying on purely primitive types in domain.

In this app, users can create new products or edit them in 3 contexts(each accessed by corresponding role):
* Catalog
* Warehouse
* Sales

### Built with
* [ASP.NET Core 5](https://asp.net)
* [Electron.NET](https://github.com/ElectronNET/Electron.NET)
* Angular
* Test-driven development (back-end)
* SQLite
* Dapper
* AutoMapper
* xUnit
	* Unit tests
	* Integration tests with TestServer
	* Acceptance tests with [FluentDocker](https://github.com/mariotoffia/FluentDocker)
* ValueObjects

## Getting Started
These instructions will get you a copy of this project up and running on your local machine, for development and testing purposes. See deployment for notes on how to deploy the project.

### Prerequisites
- .NET 5.0 
- Node.js with npm

### Development

#### Clone repository
```
git clone https://github.com/krzysztofzajaczkowski/product-manager
```
#### Visual Studio or Rider
Open .sln file and run/debug project
#### Visual Studio Code
See: [https://code.visualstudio.com/docs/languages/csharp](https://code.visualstudio.com/docs/languages/csharp)
#### CLI
```
cd src/ProductManager/ProductManager.Web
dotnet run
```

## Deployment

### Build
For deployment see [Electron.NET Build](https://github.com/ElectronNET/Electron.NET#-build)

## Functionality
At first, user has to log in before interacting with the system. On login page, one can provide email, password, and choose one role from select list. 
Application is bootstrapped with one user with credentials `admin@admin.com:secret`, that has all 3 roles.
After successful login, user is presented with a list of all products, where he can edit one of existing products or create a new one(if he has permissions to do so).  
When creating new product, sku, name and description has to be provided (other contexts are initialized with default values).
When editing existing product user is presented with 3 tabs, one for each context having form for Catalog/Warehouse/Sales product in read-only mode, if user is not authorized to edit specific context, with all inputs disabled or in normal mode with all inputs enabled if he has permissions.

## Screenshots
![login](https://user-images.githubusercontent.com/48659621/123517920-f8603500-d6a3-11eb-8a26-80685c76a6a7.png)
![login_touched_invalid](https://user-images.githubusercontent.com/48659621/123518132-cc917f00-d6a4-11eb-8fac-c09be6d59a6f.png)

![login_invalid_credentials](https://user-images.githubusercontent.com/48659621/123517936-07df7e00-d6a4-11eb-8f92-0135b2705e43.png)
![catalog_manager_browse](https://user-images.githubusercontent.com/48659621/123518151-e0d57c00-d6a4-11eb-9bc5-30e108556b3b.png)

![add_product_untouched](https://user-images.githubusercontent.com/48659621/123518166-f6e33c80-d6a4-11eb-9f69-3ccecbe66f18.png)

![add_product_touched_invalid](https://user-images.githubusercontent.com/48659621/123518178-06fb1c00-d6a5-11eb-945c-a6f8b3b67968.png)

![add_product_sku_exists](https://user-images.githubusercontent.com/48659621/123518185-14b0a180-d6a5-11eb-8ae5-2327e7c6f6a1.png)

![catalog_manager_cat_edit](https://user-images.githubusercontent.com/48659621/123518204-24c88100-d6a5-11eb-8313-a8d256038adb.png)

![catalog_manager_sales_edit](https://user-images.githubusercontent.com/48659621/123518219-3a3dab00-d6a5-11eb-9590-a073ba16db7d.png)

![sales_manager_browse](https://user-images.githubusercontent.com/48659621/123517944-0f068c00-d6a4-11eb-8775-e5164e2e3ae1.png)
![sales_manager_sales_edit](https://user-images.githubusercontent.com/48659621/123517987-3d846700-d6a4-11eb-894f-6894d9c2c053.png)
![sales_manager_sales_edit_touched_invalid](https://user-images.githubusercontent.com/48659621/123517989-407f5780-d6a4-11eb-88f3-aaaab56c12d8.png)
![sales_manager_sales_edit_touched_invalid_2](https://user-images.githubusercontent.com/48659621/123517990-42491b00-d6a4-11eb-9145-5556a0728243.png)
![sales_manager_warehouse_edit](https://user-images.githubusercontent.com/48659621/123517992-437a4800-d6a4-11eb-8071-0b8b9bb4963f.png)



[linkedin-shield-zajaczkowski]: https://img.shields.io/badge/LinkedIn-Zajączkowski-blue?logo=linkedin
[linkedin-url-zajaczkowski]: https://www.linkedin.com/in/krzysztof-m-zajaczkowski/