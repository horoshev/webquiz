import {Component, Inject, OnInit} from '@angular/core';
import {Question} from "../../types/Question";
import {HttpClient} from "@angular/common/http";
import {QuestionAnswer} from "../../types/QuestionAnswer";

@Component({
  selector: 'app-question-random',
  templateUrl: './question-random.component.html',
  styleUrls: ['./question-random.component.css']
})
export class QuestionRandomComponent implements OnInit {

  _QuestionAnswer = QuestionAnswer

  answerValue: string = ''

  isError: boolean = false
  isLoading: boolean = true
  question: Question
  userAnswer: QuestionAnswer = QuestionAnswer.Empty

  private _answerOptions : string[]

  get answerOptions(): string[] {

    let correctAnswer = this.randomCorrectAnswer
    let incorrectAnswers = this.question.incorrectAnswers.slice(0, 3)
    let answerOptions = incorrectAnswers.concat(correctAnswer)

    if (this._answerOptions == null) {
      this._answerOptions = answerOptions.sort(() => Math.random() - 0.5)
    }

    return this._answerOptions
  }

  get randomCorrectAnswer(): string {
    return this.question.correctAnswers[Math.floor(Math.random() * this.question.correctAnswers.length)]
  };

  get isComplete(): boolean {
    return this.userAnswer === QuestionAnswer.Surrender || this.userAnswer === QuestionAnswer.Correct
  };

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.nextQuestion()
  }

  // nothing happens
  onSkip() {
    this.cleanUp()
    this.nextQuestion()
  }

  // get or lose points
  onNext() {
    this.cleanUp()
    this.nextQuestion()
  }

  onSurrender() {
    this.userAnswer = QuestionAnswer.Surrender
  }

  onGuess(guess: string) {
    if (!guess || this.userAnswer === QuestionAnswer.Surrender) return

    this.userAnswer = this.question.correctAnswers
      .includes(guess) ? QuestionAnswer.Correct : QuestionAnswer.Wrong;

    if (this.userAnswer === QuestionAnswer.Wrong)
      this.cleanUp()
  }

  cleanUp() {
    this.answerValue = ''
  }

  nextQuestion() {
    this.userAnswer = QuestionAnswer.Empty
    this.isLoading = true
    this.isError = false

    this.http.get<Question>(this.baseUrl + `api/question/random`).subscribe(
      result => this.question = result,
      error => {
        this.isError = true
        console.error(error)
      }
    ).add(() => this.isLoading = false);
  }
}
