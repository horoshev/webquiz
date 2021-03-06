﻿using System;
using System.Linq.Expressions;
using Application.Extensions;

namespace Application.Entities
{
    public class QuestionQuery : PagingQuery<Question>
    {
        public int? Type { get; set; }
        public int? Category { get; set; }
        public int? Difficulty { get; set; }

        public override Expression<Func<Question, bool>> Condition => ToExpression();

        private Expression<Func<Question, bool>> ToExpression()
        {
            Expression<Func<Question, bool>> expression = question => true;

            if (Type.HasValue)
                expression = expression.AndAlso(question => question.Type == (QuestionType) Type.Value);

            if (Category.HasValue)
                expression = expression.AndAlso(question => question.Category == (QuestionCategory) Category.Value);

            if (Difficulty.HasValue)
                expression = expression.AndAlso(question => question.Difficulty == (QuestionDifficulty) Difficulty.Value);

            return expression;
        }
    }
}