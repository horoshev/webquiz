export interface IQuestionQuery {
  type: string
  category: string
  difficulty: string
  [key: string]: string
}

export class QuestionQuery implements IQuestionQuery {
  type = ''
  category = ''
  difficulty = ''

  startPage = '';
  pageSize = '';

  constructor(_type: string = '', _category: string = '', _difficulty: string = '') {
    this.type = _type;
    this.category = _category;
    this.difficulty = _difficulty;
    this.startPage = '0'
    this.pageSize = '10'
  }

  [key: string]: string;
}
