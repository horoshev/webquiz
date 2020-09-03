import {Component, HostListener, Inject} from '@angular/core';
import {Question} from "../../types/Question";
import {HttpClient, HttpParams} from "@angular/common/http";
import {QuestionQuery} from "../../types/QuestionQuery";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  isEnd = false
  isLoading = false
  questions: Question[] = []
  questionQuery = new QuestionQuery()

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.loadQuestions()
  }

  loadQuestions() {
    console.log("LOADING!!!")
    if (this.isEnd) return
    this.isLoading = true
    this.http.get<Question[]>(this.baseUrl + 'api/question', {params: this.questionQuery}).subscribe(
      result => {
        if (result.length < +this.questionQuery.pageSize) this.isEnd = true
        this.questions.push(...result)
      },
      error => console.error(error)
    ).add(() => this.isLoading = false)
  }

  onFilterChange(query: QuestionQuery) {
    this.questions = []
    this.questionQuery = query
    this.questionQuery.startPage = '0'
    this.questionQuery.pageSize = '10'
    this.isEnd = false
    this.loadQuestions();
  }

  onResetFilter() {
    this.questionQuery = new QuestionQuery()
    this.isEnd = false
    this.loadQuestions()
  }

  @HostListener('window:scroll', ['$event']) // for window scroll events
  onScroll(event) {
    const lastQuestion = document.querySelector<HTMLElement>('#lastQuestion');
    const threshold = window.scrollY + window.innerHeight * 1.2;

    // console.log(threshold, lastQuestion.offsetTop, 'loaddd')

    if (threshold > lastQuestion.offsetTop && !this.isLoading && !this.isEnd) {
      // console.log(threshold, lastQuestion.offsetTop, 'boom')
      this.questionQuery.startPage = String(+this.questionQuery.startPage + 1)
      this.loadQuestions()
    }
  }
}
