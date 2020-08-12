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

  answerValue: string = ''

  isError: boolean = false
  isLoading: boolean = true
  question: Question
  userAnswer: QuestionAnswer = QuestionAnswer.Empty

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.nextQuestion()
  }

  onSkip() {
    this.cleanUp()
    this.nextQuestion()
  }

  onSurrender() {
    this.userAnswer = QuestionAnswer.Surrender
  }

  onGuess(guess: string) {
    if (!guess || this.userAnswer === QuestionAnswer.Surrender) return

    this.userAnswer = this.question.answers
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
