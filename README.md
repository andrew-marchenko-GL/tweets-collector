# Tweets Collector Service

## Overview

The Tweets Collector Service (TCS) is a simple web service that utilizes Twitter API to pull random tweets using a sample stream endpoint and adds them to the storage.

> You can find the detailed task description in this repo's `Documentation` folder.

This service has endpoints that allow us to control the storage, start pulling tweets from the Twitter API, and provides statistic.

## Description

The project is built using .NET 6 with C#. The type of the project is ASP.Net Web API.

The `ConcurrentDictionary` has been chosen as in-memory storage that follows the `IRepository` pattern allowing replacement it with other storage if needed.

The [Hangfire Framework](https://www.hangfire.io) has been chosen as background jobs processor.

### Used components

Most service components like a logger, configuration, Swagger, etc. default from the ASP.Net.

Additionally added:

- **Hangfire** - the background jobs processor with MemoryStorage.
- **AspNetCore.Mvc.Versioning** - to support API versioning.
- **NUnit** - for Unit tests.

## How to run the project

After you cloned the repo, try to build it using `dotnet` or using any IDE of your preference.

Then you need to replace the Twitter API Bearer token with your in the `appsettings.json` file by path `Twitter:BearerToken` (replace text '`<your bearer token>`' with your token value).

Then you can start the service.

## How to use the service

You can control the service using the endpoints that the service provides. You can explore endpoints using the swagger page: https://localhost:7055/swagger/index.html.

These endpoints are pretty self-explanatory.

You can use the `Tweet` controller to perform CRUD operations on the storage.

The `Twitter` controller allows to control the service itself:

- `Count` returns total tweets in the storage.
- `Clear` removes all tweets from the storage.
- `PullTweets` starts the background process that pulls tweets using the Twitter API sample stream endpoint into the in-memory storage.
- `Statistic` provides with the statistic data at the moment of the call.

> If you use a free Twitter Dev license, you will be prohibited from running multiple background jobs at the same time and will see that the background job may fail due to the `Too many requests` error. It's not limited to a single instance at this moment to have the freedom of choice.

> One call of the `PullTweets` action will add one batch of tweets.

You can track the progress of the service either see the application output logs or using the Hangfire Dashboard (you will get the link to the job in the response): https://localhost:7055/hangfire.

## Unit tests

Unit tests are nominal here, the coverage is low and added just to get the perspective that the written code is testable and be familiar with my code style.
