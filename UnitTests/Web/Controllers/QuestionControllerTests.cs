using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Dto;
using Application.Entities;
using Application.Interfaces;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Web.Common;
using Web.Controllers;

namespace Tests.Web.Controllers
{
    public class QuestionControllerTests
    {
        private static readonly IMapper Mapper = Utils.Mapper;

        private Mock<IQuestionService> _questionServiceMock;
        private Mock<UserManager<User>> _userManagerMock;
        private QuestionController _testController;
        private Mock<IQuestionService> _questionServiceMockWithoutData;
        private QuestionController _testControllerWithoutData;

        private static readonly  User[] TestUsers =
        {
            new User {Id = "TestUser1"},
            new User {Id = "TestUser2"},
            new User {Id = "TestUser3"}
        };

        private static readonly Dictionary<string, User> Users = new Dictionary<string, User>
        {
            [TestUsers[0].Id] = TestUsers[0],
            [TestUsers[1].Id] = TestUsers[1],
            [TestUsers[2].Id] = TestUsers[2]
        };

        // private static ClaimsPrincipal[] _testUsersClaims = _testUsers.Select(GetClaims).ToArray();

        private static Question[] _testQuestions =
        {
            new Question{Id = 1, Text = "Test 1", Explanation = "Exp 1", Author = TestUsers[0]},
            new Question{Id = 2, Text = "Test 2", Explanation = "Exp 2", Author = TestUsers[1]},
            new Question{Id = 3, Text = "Test 3", Explanation = "Exp 3", Author = TestUsers[0]},
            new Question{Id = 4, Text = "Test 4", Explanation = "Exp 4", Author = TestUsers[0]},
            new Question{Id = 5, Text = "Test 5", Explanation = "Exp 5", Author = TestUsers[1]}
        };

        private static object[] _questionsByAuthorCases =
        {
            new object[] {TestUsers[0], _testQuestions.Where(question => question.Author.Id == TestUsers[0].Id).ToArray()},
            new object[] {TestUsers[1], _testQuestions.Where(question => question.Author.Id == TestUsers[1].Id).ToArray()},
            new object[] {TestUsers[2], _testQuestions.Where(question => question.Author.Id == TestUsers[2].Id).ToArray()}
        };

        private static object[] _questionCreateCases =
        {
            new object[] {_testQuestions[0], TestUsers[0]},
            new object[] {_testQuestions[1], TestUsers[1]},
            new object[] {_testQuestions[2], TestUsers[2]}
        };

        [SetUp]
        public void SetUp()
        {
            _questionServiceMockWithoutData = EmptyQuestionServiceMock();
            _questionServiceMock = QuestionServiceMock();
            _userManagerMock = UserManagerMock();

            _testController = new QuestionController(Mapper, _questionServiceMock.Object, _userManagerMock.Object);
            _testControllerWithoutData = new QuestionController(Mapper, _questionServiceMockWithoutData.Object, _userManagerMock.Object);
        }

        [Test]
        public void ShouldReturnAllQuestions()
        {
            var actualQuestions = _testController.GetQuestions();

            actualQuestions.Should().BeAssignableTo<IEnumerable<QuestionDto>>()
                .Which.Select(q => q.Id).Should().BeEquivalentTo(_testQuestions.Select(q => q.Id));
        }

        [Test]
        public void ShouldReturnAllQuestionsEmpty()
        {
            var actualQuestions = _testControllerWithoutData.GetQuestions();

            actualQuestions.Should().BeAssignableTo<IEnumerable<QuestionDto>>().Which.Should().BeEmpty();
        }

        [Test]
        public void GetRandomQuestionShouldReturnOkResponse()
        {
            var response = _testController.GetRandomQuestion();

            response.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<QuestionDto>();
        }

        [Test]
        public void GetRandomQuestionShouldReturnNotFoundResponse()
        {
            var response = _testControllerWithoutData.GetRandomQuestion();

            response.Should().BeOfType<NotFoundResult>();
        }

        [Test, TestCaseSource(nameof(_testQuestions))]
        public void GetQuestionByIdShouldReturnOkResponseWithQuestion(Question question)
        {
            var response = _testController.GetQuestionById(question.Id);

            response.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<QuestionDto>()
                .Which.Id.Should().Be(question.Id);
        }

        [Test]
        public void GetQuestionByIdShouldReturnNotFoundResponse()
        {
            var question = _testQuestions[0];
            var response = _testControllerWithoutData.GetQuestionById(question.Id);

            response.Should().BeOfType<NotFoundResult>();
        }
        // TODO: Update tests for controller
/*
        [Test, TestCaseSource(nameof(_questionsByAuthorCases))]
        public void GetAuthorQuestionsTest(User user, IEnumerable<Question> questions)
        {
            _testController.ControllerContext.HttpContext = new DefaultHttpContext {User = GetClaims(user)};
            var response = _testController.GetAuthorQuestions();

            response.Should().BeAssignableTo<IEnumerable<QuestionDto>>()
                .Which.Select(q => q.Id ?? 0).Should().BeEquivalentTo(questions.Select(q => q.Id));
        }

        [Test]
        public async Task ShouldNotAllowAnonymousUserToCreateQuestion()
        {
            //var result = await _httpClient.GetAsync("http://localhost:8000/api/values");
            // Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);

            var question = _testQuestions[0];
            var questionDto = Mapper.Map<QuestionDto>(question);

            await _testController.InsertQuestion(questionDto)
                .ContinueWith(task => task.Result.Should().BeOfType<BadRequestObjectResult>()
                    .Which.Value.Should().BeOfType<string>()
                    .And.BeEquivalentTo(ErrorMessage.UserIdentifierNotFound));
        }

        [Test]
        public async Task ShouldNotAllowFakeUserToCreateQuestion()
        {
            var question = _testQuestions[0];
            var questionDto = Mapper.Map<QuestionDto>(question);

            _testController.ControllerContext.HttpContext = new DefaultHttpContext {User = GetClaims(new User {Id = "FakeUser"})};
            await _testController.InsertQuestion(questionDto)
                .ContinueWith(task => task.Result.Should().BeOfType<BadRequestObjectResult>()
                    .Which.Value.Should().BeOfType<string>()
                    .And.BeEquivalentTo(ErrorMessage.UserNotFound));
        }

        [Test]
        public async Task ShouldNotAllowToCreateNullQuestion()
        {
            await _testController.InsertQuestion(null!)
                .ContinueWith(task => task.Result.Should().BeOfType<BadRequestObjectResult>()
                    .Which.Value.Should().BeOfType<string>()
                    .And.BeEquivalentTo(ErrorMessage.InvalidRequestData));
        }

        [Test, TestCaseSource(nameof(_questionCreateCases))]
        public async Task ShouldCreateQuestion(Question question, User user)
        {
            var questionDto = Mapper.Map<QuestionDto>(question);

            _testController.ControllerContext.HttpContext = new DefaultHttpContext {User = GetClaims(user)};
            await _testController.InsertQuestion(questionDto)
                .ContinueWith(task => task.Result.Should().BeOfType<OkObjectResult>()
                    .Which.Value.Should().BeOfType<string>().And.Be(user.Id.Last().ToString()));
        }

        [Test]
        public void Delete()
        {
            var questionDto = Mapper.Map<QuestionDto>(question);

            _testController.ControllerContext.HttpContext = new DefaultHttpContext {User = GetClaims(user)};
            await _testController.DeleteQuestion(questionDto)
                .ContinueWith(task => task.Result.Should().BeOfType<OkObjectResult>()
                    .Which.Value.Should().BeOfType<string>().And.Be(user.Id.Last().ToString()));
        }

        [Test]
        public void Update()
        {

        }
*/

        [Ignore("Failed")]
        public void AnswerPolicyReturnForbiddenOnDeleteQuestionOfOtherAuthor()
        {
            _testController.ControllerContext.HttpContext = new DefaultHttpContext{User = GetClaims(new User {Id = "FakeId"})};

            var response = _testController.DeleteQuestion(1);

            response.Should().BeOfType<ForbidResult>();
        }

        private static ClaimsPrincipal GetClaims(User user)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, "TestName")
            },"TestAuthentication"));
        }

        private static Mock<IQuestionService> EmptyQuestionServiceMock()
        {
            var questionServiceMock = new Mock<IQuestionService>();

            questionServiceMock
                .Setup(service => service.GetRandomQuestion())
                .Returns(() => null);

            questionServiceMock
                .Setup(service => service.GetAll())
                .Returns(() => new Question[] {});

            // questionServiceMock
            //     .Setup(service => service.GetRandomQuestion())
            //     .Returns(() => new Question());
            //
            // questionServiceMock
            //     .Setup(service => service.GetAll())
            //     .Returns(() => _testQuestions);
            //
            // questionServiceMock
            //     .Setup(service => service.Get(It.IsAny<int>()))
            //     .Returns<int>(id => _testQuestions.First(question => question.Id == id));
            //
            // questionServiceMock
            //     .Setup(service => service.GetQuestionByAuthorId(It.IsAny<string>()))
            //     .Returns<string>(id => _testQuestions.Where(question => question.Author.Id == id).ToArray());

            return questionServiceMock;
        }

        private static Mock<IQuestionService> QuestionServiceMock()
        {
            var questionServiceMock = new Mock<IQuestionService>();

            questionServiceMock
                .Setup(service => service.GetRandomQuestion())
                .Returns(() => new Question());

            questionServiceMock
                .Setup(service => service.GetAll())
                .Returns(() => _testQuestions);

            questionServiceMock
                .Setup(service => service.Get(It.IsAny<int>()))
                .Returns<int>(id => _testQuestions.First(question => question.Id == id));

            questionServiceMock
                .Setup(service => service.GetQuestionByAuthorId(It.IsAny<string>()))
                .Returns<string>(id => _testQuestions.Where(question => question.Author.Id == id).ToArray());

            questionServiceMock
                .Setup(service => service.Create(It.IsAny<QuestionDto>()))
                .Returns<Question>(question => {
                    question.Id = int.Parse(question.Author.Id.Last().ToString());
                    return question;
                });

            return questionServiceMock;
        }

        private static Mock<UserManager<User>> UserManagerMock()
        {
            var userStorage = new UserStore<User>(new DbContext(new DbContextOptions<DbContext>()));
            var userManagerMock = new Mock<UserManager<User>>(
                userStorage,
                null,
                new PasswordHasher<User>(),
                new IUserValidator<User>[]{},
                new IPasswordValidator<User>[]{},
                null,
                new IdentityErrorDescriber(),
                null,
                new NullLogger<UserManager<User>>()
            );

            userManagerMock.Setup(manager => manager.FindByIdAsync(It.IsAny<string>()))
                .Returns<string>(id => Task.FromResult(Users.GetValueOrDefault(id)));

            return userManagerMock;
        }
    }
}