export interface Question {
  id?: number
  type: string
  category: string
  difficulty: string
  text: string
  explanation: string
  correctAnswers: string[]
  incorrectAnswers: string[]
  [key: string]: any
}

export const QuestionTypeOptions = [
  {label: "Any Type", value: ""},
  {label: "Binary (Yes or No)", value: "0"},
  {label: "With Choice", value: "1"},
  {label: "Without Choice", value: "2"},
]

export const QuestionOrderOptions = [
  {label: "Type", value: "Type"},
  {label: "Difficulty", value: "Difficulty"},
  {label: "Category", value: "Category"},
]

export const QuestionDifficultyOptions = [
  {label: "Any Difficulty", value: ""},
  {label: "Easy", value: "0"},
  {label: "Medium", value: "1"},
  {label: "Hard", value: "2"},
]

export const QuestionCategoryOptions = [
  {label: "Any Category", value: ""},
  {label: "None", value: "0"},
  {label: "General", value: "1"},
  {label: "Books", value: "2"},
  {label: "Films", value: "3"},
  {label: "Music", value: "4"},
  {label: "VideoGames", value: "5"},
  {label: "Science", value: "6"},
  {label: "Nature", value: "7"},
  {label: "Computers", value: "8"},
  {label: "Mathematics", value: "9"},
  {label: "Mythology", value: "10"},
  {label: "Sports", value: "11"},
  {label: "Geography", value: "12"},
  {label: "History", value: "13"},
  {label: "Politics", value: "14"},
  {label: "Art", value: "15"},
  {label: "Animals", value: "16"},
  {label: "Vehicles", value: "17"},
  {label: "Comics", value: "18"},
  {label: "Anime", value: "19"},
  {label: "Cartoons", value: "20"},
]
