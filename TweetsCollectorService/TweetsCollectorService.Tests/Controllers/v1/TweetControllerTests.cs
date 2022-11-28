namespace Jha.Services.TweetsCollectorService.Tests.Controllers.v1;

using System;
using System.Linq.Expressions;
using Jha.Services.TweetsCollectorService.Controllers.v1;
using Jha.Services.TweetsCollectorService.Models.Twitter;
using Jha.Services.TweetsCollectorService.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class TweetControllerTests
{
    #region Mocks

    private Mock<IRepository<Tweet>> tweetsRepositoryMock;
    private Mock<ILogger<TwitterController>> loggerMock;
    private TweetController controller;

    #endregion

    #region Setup

    [SetUp]
    public void Setup()
    {
        this.tweetsRepositoryMock = new Mock<IRepository<Tweet>>();
        this.loggerMock = new Mock<ILogger<TwitterController>>();
        this.controller = new TweetController(this.tweetsRepositoryMock.Object, this.loggerMock.Object);
    }

    #endregion

    #region Constructor tests

    [Test]
    public void Constructor_RepositoryIsNull_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new TweetController(null, this.loggerMock.Object));
    }

    [Test]
    public void Constructor_LoggerIsNull_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new TweetController(this.tweetsRepositoryMock.Object, null));
    }

    [Test]
    public void Constructor_InitSuccessfull()
    {
        Assert.DoesNotThrow(() => new TweetController(this.tweetsRepositoryMock.Object, this.loggerMock.Object));
    }

    #endregion

    #region GetTweet

    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void GetTweet_IdIsInvalid_BadRequestReturns(string id)
    {
        var response = this.controller.GetTweet(id);

        Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);
    }

    [Test]
    public void GetTweet_ResourceNotFound_NotFoundReturns()
    {
        var response = this.controller.GetTweet("123");

        Assert.IsInstanceOf<NotFoundObjectResult>(response.Result);
    }

    [Test]
    public void GetTweet_ValidRequest_OkResponse()
    {
        var tweet = new Tweet(new TweetBase { Id = "123", Text = "Test tweet text." });
        this.tweetsRepositoryMock.Setup(m => m.GetFirstOrDefault(It.IsAny<Func<Tweet, bool>>())).Returns(tweet);

        var response = this.controller.GetTweet("123");
        var result = (OkObjectResult)response?.Result;
        var resultTweet = (Tweet)result?.Value;

        Assert.That(response, Is.Not.Null);
        Assert.IsInstanceOf<OkObjectResult>(response.Result);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.Not.Null);
        Assert.IsInstanceOf<Tweet>(result.Value);
        Assert.That(resultTweet, Is.Not.Null);
        Assert.That(resultTweet.Id, Is.EqualTo(tweet.Id));
        Assert.That(resultTweet.Text, Is.EqualTo(tweet.Text));
    }

    #endregion

    #region GetTweet

    [Test]
    public void GetTweet_PayloadIsNull_BadRequest()
    {
        var response = this.controller.CreateTweet(null);

        Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);
    }

    [Test]
    public void GetTweet_PayloadIsInvalid_BadRequest()
    {
        var tweetText = new TweetText();
        controller.ModelState.AddModelError("Text", "Required");

        var response = this.controller.CreateTweet(tweetText);

        Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);
    }

    [Test]
    public void GetTweet_PayloadIsVadid_OkResponse()
    {
        var tweetText = new TweetText("Test text");
        this.tweetsRepositoryMock.Setup(m => m.Add(It.IsAny<Tweet>())).Returns(new Tweet(tweetText) { Id = "123" });

        var response = this.controller.CreateTweet(tweetText);
        var result = (OkObjectResult)response?.Result;
        var resultTweet = (Tweet)result?.Value;

        Assert.That(response, Is.Not.Null);
        Assert.IsInstanceOf<OkObjectResult>(response.Result);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.Not.Null);
        Assert.IsInstanceOf<Tweet>(result.Value);
        Assert.That(resultTweet, Is.Not.Null);
        Assert.That(resultTweet.Id, Is.EqualTo("123"));
        Assert.That(resultTweet.Text, Is.EqualTo(tweetText.Text));
    }

    #endregion

    #region DeleteTweet

    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void DeleteTweet_IdIsInvalid_BadRequestReturns(string id)
    {
        var response = this.controller.DeleteTweet(id);

        Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);
    }

    [Test]
    public void DeleteTweet_ResourceNotFound_NotFoundReturns()
    {
        var response = this.controller.DeleteTweet("123");

        Assert.IsInstanceOf<NotFoundObjectResult>(response.Result);
    }

    [Test]
    public void DeleteTweet_ValidRequest_OkResponse()
    {
        var tweet = new Tweet(new TweetBase { Id = "123", Text = "Test tweet text." });
        this.tweetsRepositoryMock.Setup(m => m.RemoveFirstOrDefault(It.IsAny<Func<Tweet, bool>>())).Returns(tweet);

        var response = this.controller.DeleteTweet("123");
        var result = (OkObjectResult)response?.Result;
        var resultTweet = (Tweet)result?.Value;

        Assert.That(response, Is.Not.Null);
        Assert.IsInstanceOf<OkObjectResult>(response.Result);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.Not.Null);
        Assert.IsInstanceOf<Tweet>(result.Value);
        Assert.That(resultTweet, Is.Not.Null);
        Assert.That(resultTweet.Id, Is.EqualTo(tweet.Id));
        Assert.That(resultTweet.Text, Is.EqualTo(tweet.Text));
    }

    #endregion
}

