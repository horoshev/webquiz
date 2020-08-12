export interface Question {
  id?: number
  text: string
  answers: string[]
  explanation: string
  [key: string]: any
}
