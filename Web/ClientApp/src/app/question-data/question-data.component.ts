import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Question} from "../../types/Question";

@Component({
  selector: 'app-question-data',
  templateUrl: './question-data.component.html',
  styleUrls: ['./question-data.component.css']
})
export class QuestionDataComponent implements OnInit {

  questions: Question[]

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get<Question[]>(baseUrl + 'api/question').subscribe(result => {
      this.questions = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

  onGenerate() {
    this.http.post<void>(this.baseUrl + 'api/question/10', {}).subscribe(() => { }, error => console.error(error))
  }
}
