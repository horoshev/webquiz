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
