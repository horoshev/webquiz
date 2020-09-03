namespace Application.Entities
{
    public enum QuestionType
    {
        /// <summary>
        /// Answer is only can be 'True' or 'False'
        /// </summary>
        Boolean,

        /// <summary>
        /// Question type where you select 1 correct answer from multiple options
        /// </summary>
        Choice,

        /// <summary>
        /// Question type where you write correct answer
        /// </summary>
        NoChoice
    }
}