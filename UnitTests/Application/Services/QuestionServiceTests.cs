using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Dto;
using Application.Entities;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using static Application.Entities.QuestionCategory;

namespace Tests.Application.Services
{
    public class QuestionServiceTests
    {
        private IQuestionService _questionService;
        private IQuestionService _emptyQuestionService;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IUnitOfWork> _emptyUnitOfWork;
        private Mock<IQuestionRepository> _repositoryMock;
        private Mock<IQuestionRepository> _emptyRepositoryMock;

        private static readonly IMapper Mapper = Utils.Mapper;
        private static readonly User Author1 = new User {Id = "1"};
        private static readonly User Author2 = new User {Id = "2"};
        private static readonly User Author3 = new User {Id = "3"};

        private static readonly Dictionary<string, Question> QuestionsData = new Dictionary<string, Question>
        {
            ["1"] = new Question { Id = 1, AuthorId = Author1.Id, CorrectAnswers = "", Type = QuestionType.Choice, Difficulty = QuestionDifficulty.None, Category = None},
            ["2"] = new Question { Id = 2, AuthorId = Author1.Id, CorrectAnswers = "", Type = QuestionType.Boolean, Difficulty = QuestionDifficulty.Medium, Category = None},
            ["3"] = new Question { Id = 3, AuthorId = Author2.Id, CorrectAnswers = "", Type = QuestionType.Boolean, Difficulty = QuestionDifficulty.None, Category = Anime},
            ["4"] = new Question { Id = 4, AuthorId = Author1.Id, CorrectAnswers = "", Type = QuestionType.NoChoice, Difficulty = QuestionDifficulty.Easy, Category = Animals},
            ["5"] = new Question { Id = 5, AuthorId = Author2.Id, CorrectAnswers = "", Type = QuestionType.NoChoice, Difficulty = QuestionDifficulty.Medium, Category = Anime},
            ["6"] = new Question { Id = 6, AuthorId = Author1.Id, CorrectAnswers = "", Type = QuestionType.NoChoice, Difficulty = QuestionDifficulty.Hard, Category = Anime},
        };

        private static readonly IList<Question> TestQuestions = QuestionsData.Select(pair => pair.Value).ToList();
        private static readonly QuestionDto[] FakeQuestions =
            TestQuestions.Select(question =>
            {
                var dto = Mapper.Map<QuestionDto>(question);
                dto.Id += TestQuestions.Count;
                return dto;
            }).ToArray();

        private static readonly Question[] QuestionsAuthor1 = {QuestionsData["1"], QuestionsData["2"], QuestionsData["4"], QuestionsData["6"]};
        private static readonly Question[] QuestionsAuthor2 = {QuestionsData["3"], QuestionsData["5"]};
        private static readonly Question[] QuestionsAuthor3 = { };

        private static (string, IEnumerable<Question>)[] _authorTestCases = {
            (Author1.Id, QuestionsAuthor1),
            (Author2.Id, QuestionsAuthor2),
            (Author3.Id, QuestionsAuthor3)
        };

        private static (QuestionQuery, IEnumerable<Question>)[] _queryTestCases = {
            // without query
            (new QuestionQuery(), TestQuestions),

            // query by type
            (new QuestionQuery {Type = (int) QuestionType.Boolean}, TestQuestions.Where(q => q.Type == QuestionType.Boolean).ToArray()),
            (new QuestionQuery {Type = (int) QuestionType.Choice}, TestQuestions.Where(q => q.Type == QuestionType.Choice).ToArray()),
            (new QuestionQuery {Type = (int) QuestionType.NoChoice}, TestQuestions.Where(q => q.Type == QuestionType.NoChoice).ToArray()),

            // query by difficulty
            (new QuestionQuery {Difficulty = (int) QuestionDifficulty.None}, TestQuestions.Where(q => q.Difficulty == QuestionDifficulty.None).ToArray()),
            (new QuestionQuery {Difficulty = (int) QuestionDifficulty.Easy}, TestQuestions.Where(q => q.Difficulty == QuestionDifficulty.Easy).ToArray()),
            (new QuestionQuery {Difficulty = (int) QuestionDifficulty.Medium}, TestQuestions.Where(q => q.Difficulty == QuestionDifficulty.Medium).ToArray()),
            (new QuestionQuery {Difficulty = (int) QuestionDifficulty.Hard}, TestQuestions.Where(q => q.Difficulty == QuestionDifficulty.Hard).ToArray()),

            // query by category
            (new QuestionQuery {Category = (int) None}, TestQuestions.Where(q => q.Category == None).ToArray()),
            (new QuestionQuery {Category = (int) Anime}, TestQuestions.Where(q => q.Category == Anime).ToArray()),
            (new QuestionQuery {Category = (int) Animals}, TestQuestions.Where(q => q.Category == Animals).ToArray()),

            // query by type and difficulty
            (new QuestionQuery {Type = (int) QuestionType.Boolean, Difficulty = (int) QuestionDifficulty.Easy},
                TestQuestions
                    .Where(q => q.Type == QuestionType.Boolean)
                    .Where(q => q.Difficulty == QuestionDifficulty.Easy).ToArray()),

            // query by type and category
            (new QuestionQuery {Type = (int) QuestionType.NoChoice, Category = (int) Anime},
                TestQuestions
                    .Where(q => q.Type == QuestionType.NoChoice)
                    .Where(q => q.Category == Anime).ToArray()),

            // query by difficulty and category
            (new QuestionQuery {Difficulty = (int) QuestionDifficulty.Easy, Category = (int) Animals},
                TestQuestions
                    .Where(q => q.Difficulty == QuestionDifficulty.Easy)
                    .Where(q => q.Category == Animals).ToArray()),

            // query by type and difficulty and category
            (new QuestionQuery
            {
                Type = (int) QuestionType.Boolean,
                Difficulty = (int) QuestionDifficulty.Easy,
                Category = (int) None
            }, Array.Empty<Question>()),
            (new QuestionQuery
            {
                Type = (int) QuestionType.Choice,
                Difficulty = (int) QuestionDifficulty.None,
                Category = (int) None
            }, new[] {QuestionsData["1"]}),
            (new QuestionQuery
            {
                Type = (int) QuestionType.NoChoice,
                Difficulty = (int) QuestionDifficulty.Hard,
                Category = (int) Anime
            }, new[] {QuestionsData["6"]}),
        };

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = QuestionRepositoryMock(TestQuestions);
            _emptyRepositoryMock = QuestionRepositoryMock(Enumerable.Empty<Question>());

            _unitOfWorkMock = UnitOfWorkMock(_repositoryMock.Object);
            _emptyUnitOfWork = UnitOfWorkMock(_emptyRepositoryMock.Object);

            _questionService = new QuestionService(_unitOfWorkMock.Object, Mapper);
            _emptyQuestionService = new QuestionService(_emptyUnitOfWork.Object, Mapper);
        }

        private static Mock<IUnitOfWork> UnitOfWorkMock(IBaseRepository<Question> repository)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock
                .Setup(unit => unit.GetBaseRepository<Question>())
                .Returns(() => repository);

            return unitOfWorkMock;
        }

        private static Mock<IQuestionRepository> QuestionRepositoryMock(IEnumerable<Question> data)
        {
            var questions = data.ToList();
            var questionRepositoryMock = new Mock<IQuestionRepository>();

            questionRepositoryMock
                .Setup(repository => repository.GetAll())
                .Returns(() => questions);

            questionRepositoryMock
                .Setup(repository => repository.Get(It.IsAny<int>()))
                .Returns<int>(id => questions.FirstOrDefault(question => question.Id == id));

            questionRepositoryMock
                .Setup(repository => repository.Create(It.IsAny<Question>()))
                .Returns<Question>(question => question);

            questionRepositoryMock
                .Setup(repository => repository.Update(It.IsAny<Question>()))
                .Returns<Question>(question => TestQuestions.Any(q => q.Id == question.Id) ? question : null);

            questionRepositoryMock
                .Setup(repository => repository.Delete(It.IsAny<int>()))
                .Returns<int>(id => TestQuestions.FirstOrDefault(question => question.Id == id));

            questionRepositoryMock
                .Setup(repository => repository.Count())
                .Returns(questions.Count);

            questionRepositoryMock
                .Setup(repository => repository.GetByIndex(It.IsAny<int>()))
                .Returns<int>(index => questions.Skip(index).FirstOrDefault());

            questionRepositoryMock
                .Setup(repository => repository.Query(It.IsAny<Expression<Func<Question, bool>>>()))
                .Returns<Expression<Func<Question, bool>>>(expression => TestQuestions.Where(expression.Compile()));

            return questionRepositoryMock;
        }

        [Test]
        public void ReturnsRandomQuestion()
        {
            var question = _questionService.GetRandomQuestion();

            TestQuestions.Should().ContainEquivalentOf(question);
        }

        [Test]
        public void ReturnsNullRandomQuestionEmptyRepository()
        {
            var question = _emptyQuestionService.GetRandomQuestion();

            question.Should().BeNull();
        }

        [Test, TestCaseSource(nameof(_authorTestCases))]
        public void ReturnsAllQuestionsOfAuthor((string authorId, IEnumerable<Question> expectedAuthorQuestions) testCase)
        {
            var (authorId, expectedAuthorQuestions) = testCase;

            _questionService.GetQuestionByAuthorId(authorId)
                .Should().BeAssignableTo<IEnumerable<Question>>()
                .And.BeEquivalentTo(expectedAuthorQuestions);
        }

        [Test]
        public void ShouldReturnsAllQuestionsMatchingQuery()
        {
            var query = new QuestionQuery();

            var questions = _questionService.GetQuestionsByQuery(query);

            questions
                .Should().BeAssignableTo<IEnumerable<Question>>()
                .And.HaveCount(TestQuestions.Count, "method should return same number of questions if there no constrains")
                .And.Subject.Should().BeEquivalentTo(TestQuestions, "method should return all questions without filtering");
        }

        [Test, TestCaseSource(nameof(_queryTestCases))]
        public void ShouldFilterQuestions((QuestionQuery, IEnumerable<Question>) testCase)
        {
            var (query, expected) = testCase;

            _questionService.GetQuestionsByQuery(query).Should().BeEquivalentTo(expected);
        }

        [Test]
        [Ignore("Not created")]
        public void ShouldCreateMultipleQuestions()
        {
            // is this should be tested?
        }

        // lower is base operations service test
        [Test]
        public void ShouldReturnAllQuestions()
        {
            _questionService.GetAll().Should().BeEquivalentTo(TestQuestions);
        }

        [Test]
        public void ShouldReturnAllQuestionsEmptyRepository()
        {
            _emptyQuestionService.GetAll().Should().BeEmpty("there is 0 questions");
        }

        [Test, TestCaseSource(nameof(TestQuestions))]
        public void GetQuestionByIdShouldReturnSameQuestion(Question question)
        {
            _questionService.Get(question.Id).Should().BeEquivalentTo(question);
        }

        [Test, TestCaseSource(nameof(TestQuestions))]
        public void GetQuestionByIdShouldReturnNull(Question question)
        {
            _emptyQuestionService.Get(question.Id).Should().BeNull();
        }

        [Test, TestCaseSource(nameof(FakeQuestions))]
        public void ShouldCreateQuestion(QuestionDto expected)
        {
            _questionService.Create(expected).Should().NotBeNull()
                .And.Match<Question>(actual => expected.Id == actual.Id && expected.AuthorId == actual.AuthorId);
            _repositoryMock.Verify(repository => repository.Create(It.IsAny<Question>()), () => Times.Exactly(1));
        }

        [Test]
        public void ShouldNotAllowToCreateNullQuestion()
        {
            _questionService.Create(null!).Should().BeNull();
            _repositoryMock.Verify(repository => repository.Create(It.IsAny<Question>()), () => Times.Exactly(0));
        }

        [Test, TestCaseSource(nameof(TestQuestions))]
        public void ShouldUpdateQuestion(Question question)
        {
            var expected = Mapper.Map<QuestionDto>(question);

            _questionService.Update(expected).Should().NotBeNull()
                .And.Match<Question>(actual => expected.Id == actual.Id && expected.AuthorId == actual.AuthorId);

            _repositoryMock.Verify(repository => repository.Update(It.IsAny<Question>()), () => Times.Exactly(1));
        }

        [Test]
        public void ShouldNotAllowToUpdateNullQuestion()
        {
            _questionService.Update(null!).Should().BeNull();
            _repositoryMock.Verify(repository => repository.Update(It.IsAny<Question>()), () => Times.Exactly(0));
        }

        [Test, TestCaseSource(nameof(FakeQuestions))]
        public void ShouldNotAllowToUpdateFakeQuestion(QuestionDto fakeQuestion)
        {
            _questionService.Update(fakeQuestion).Should().BeNull();
            // _repositoryMock.Verify(repository => repository.Update(It.IsAny<Question>()), () => Times.Exactly(0));
        }

        [Test, TestCaseSource(nameof(TestQuestions))]
        public void ShouldDeleteQuestion(Question question)
        {
            var expected = Mapper.Map<QuestionDto>(question);

            _questionService.Delete(expected).Should().NotBeNull()
                .And.Match<Question>(actual => expected.Id == actual.Id && expected.AuthorId == actual.AuthorId);
            _repositoryMock.Verify(repository => repository.Delete(It.IsAny<int>()), () => Times.Exactly(1));
        }

        [Test]
        public void ShouldNotAllowToDeleteNullQuestion()
        {
            _questionService.Delete(null!).Should().BeNull();
            _repositoryMock.Verify(repository => repository.Delete(It.IsAny<Question>()), () => Times.Exactly(0));
        }

        [Test, TestCaseSource(nameof(FakeQuestions))]
        public void ShouldNotAllowToDeleteFakeQuestion(QuestionDto fakeQuestion)
        {
            _questionService.Delete(fakeQuestion).Should().BeNull();
            _repositoryMock.Verify(repository => repository.Delete(It.IsAny<Question>()), () => Times.Exactly(0));
        }
    }
}