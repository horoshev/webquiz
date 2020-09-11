import {Component, HostListener, Inject} from '@angular/core';
import {Question, QuestionOrderOptions} from "../../types/Question";
import {HttpClient, HttpParams} from "@angular/common/http";
import {QuestionQuery} from "../../types/QuestionQuery";

class Page<T> {
  data: Question[]
  pageSize: number
  pageNumber: number
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  isEnd = false
  isLoading = false
  questions: Question[] = []
  questionQuery = new QuestionQuery()

  orderOptions = QuestionOrderOptions
  emptyOrder: {orderBy: '', isAscendingOrder: true}
  selectedOrder: { orderBy: string, isAscendingOrder: boolean } = this.emptyOrder

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.loadQuestions()
  }

  loadQuestions() {
    console.log("LOADING!!!")
    if (this.isEnd) return
    this.isLoading = true
    this.http.get<Page<Question>>(this.baseUrl + 'api/question', {params: this.questionQuery}).subscribe(
      result => {
        if (result.data.length < +this.questionQuery.pageSize) this.isEnd = true
        this.questions.push(...result.data)
      },
      error => console.error(error)
    ).add(() => this.isLoading = false)
  }

  onFilterChange(query: QuestionQuery) {
    this.questions = []
    this.questionQuery = query
    this.questionQuery.pageNumber = '0'
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
      this.questionQuery.pageNumber = String(+this.questionQuery.pageNumber + 1)
      this.loadQuestions()
    }
  }

  onOrderChange() {
    console.log(this.selectedOrder)
    this.questions = []
    this.isEnd = false
    this.questionQuery.orderBy = this.selectedOrder.orderBy
    this.questionQuery.isAscendingOrder = this.selectedOrder.isAscendingOrder
    this.loadQuestions()
  }
}
