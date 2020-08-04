import {Component, Inject, OnInit} from '@angular/core';
import {Question} from "../../types/Question";
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-question-view',
  templateUrl: './question-view.component.html',
  styleUrls: ['./question-view.component.css']
})
export class QuestionViewComponent implements OnInit {

  isError: boolean = false
  isLoading: boolean = true
  question: Question = null

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.getQuestionById(+params.get('id'))
    });
  }

  getQuestionById(id: number) {
    this.isLoading = true
    this.isError = false

    this.http.get<Question>(this.baseUrl + `api/question/${id}`).subscribe(
      result => this.question = result,
      error => {
        this.isError = true
        console.error(error)
      }
    ).add(() => this.isLoading = false);
  }
}
