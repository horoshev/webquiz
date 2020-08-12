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
using NUnit.Framework;
using Moq;

namespace Tests.Application.Services
{
    public class QuestionServiceTests
    {
        private IQuestionService _questionService;
        private IQuestionService _emptyQuestionService;
        private Mock<IQuestionRepository> _repositoryMock;
        private Mock<IQuestionRepository> _emptyRepositoryMock;

        private static readonly IMapper Mapper = Utils.Mapper;
        private static readonly User Author1 = new User {Id = "1"};
        private static readonly User Author2 = new User {Id = "2"};
        private static readonly User Author3 = new User {Id = "3"};

        private static readonly Dictionary<string, Question> Questions = new Dictionary<string, Question>
        {
            ["1"] = new Question { Id = 1, Author = Author1},
            ["2"] = new Question { Id = 2, Author = Author1},
            ["3"] = new Question { Id = 3, Author = Author2},
            ["4"] = new Question { Id = 4, Author = Author1},
            ["5"] = new Question { Id = 5, Author = Author2},
        };

        private static readonly Question[] TestQuestions = Questions.Select(pair => pair.Value).ToArray();
        private static readonly QuestionDto[] FakeQuestions =
            TestQuestions.Select(question =>
            {
                var dto = Mapper.Map<QuestionDto>(question);

                dto.Id += TestQuestions.Length;

                return dto;
            }).ToArray();

        private static readonly Question[] QuestionsAuthor1 = {Questions["1"], Questions["2"], Questions["4"]};
        private static readonly Question[] QuestionsAuthor2 = {Questions["3"], Questions["5"]};
        private static readonly Question[] QuestionsAuthor3 = { };

        private static object[] _cases =
        {
            new object[] {Author1.Id, QuestionsAuthor1},
            new object[] {Author2.Id, QuestionsAuthor2},
            new object[] {Author3.Id, QuestionsAuthor3}
        };

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = QuestionRepositoryMock(Questions.Select(pair => pair.Value));
            _emptyRepositoryMock = QuestionRepositoryMock(Enumerable.Empty<Question>());

            _questionService = new QuestionService(_repositoryMock.Object, Mapper);
            _emptyQuestionService = new QuestionService(_emptyRepositoryMock.Object, Mapper);
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
                .Setup(repository => repository.Update(It.IsAny<Question>()));

            questionRepositoryMock
                .Setup(repository => repository.Delete(It.IsAny<int>()));

            questionRepositoryMock
                .Setup(repository => repository.Count())
                .Returns(questions.Count);

            questionRepositoryMock
                .Setup(repository => repository.GetByIndex(It.IsAny<int>()))
                .Returns<int>(index => questions.Skip(index).FirstOrDefault());

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

        [Test, TestCaseSource(nameof(_cases))]
        public void ReturnsAllQuestionsOfAuthor(string authorId, Question[] expectedAuthorQuestions)
        {
            var repositoryMock = new Mock<IQuestionRepository>();
            repositoryMock.Setup(repository => repository.Query(q => q.Author.Id == authorId))
                .Returns<Expression<Func<Question, bool>>>(query => Questions.Select(pair => pair.Value).Where(query.Compile()).ToArray());

            _questionService = new QuestionService(repositoryMock.Object, Mapper);

            var actualAuthorQuestions = _questionService.GetQuestionByAuthorId(authorId);

            foreach (var question in actualAuthorQuestions)
            {
                expectedAuthorQuestions.Should().ContainEquivalentOf(question);
            }
        }

        [Test]
        public void ShouldReturnAllQuestions()
        {
            var actualQuestions = _questionService.GetAll();

            foreach (var question in actualQuestions)
            {
                TestQuestions.Should().ContainEquivalentOf(question);
            }
            // actualQuestions.Should().BeAssignableTo<IEnumerable<Question>>().Which.Should().BeSameAs(TestQuestions);
        }

        [Test]
        public void ShouldReturnAllQuestionsEmptyRepository()
        {
            var actualQuestions = _emptyQuestionService.GetAll();

            actualQuestions.Should().BeEmpty();
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
                .And.Match<Question>(actual => expected.Id == actual.Id && expected.AuthorId == actual.Author.Id);
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
                .And.Match<Question>(actual => expected.Id == actual.Id && expected.AuthorId == actual.Author.Id);
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
            _repositoryMock.Verify(repository => repository.Update(It.IsAny<Question>()), () => Times.Exactly(0));
        }

        [Test, TestCaseSource(nameof(TestQuestions))]
        public void ShouldDeleteQuestion(Question question)
        {
            var expected = Mapper.Map<QuestionDto>(question);

            _questionService.Delete(expected).Should().NotBeNull()
                .And.Match<Question>(actual => expected.Id == actual.Id && expected.AuthorId == actual.Author.Id);
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