import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import {Question} from "../../types/Question";
import {HttpClient} from "@angular/common/http";
import {DialogComponent} from "../dialog/dialog.component";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  selectedTab = 0
  tabs: string[] = [
    'stats',
    'questions',
    'notifications',
    'settings'
  ]

  modifiedQuestion: any = null
  questionToEdit: Question
  createdQuestions: Question[] = []

  @ViewChild('confirmation', {static: false}) private confirmationDialog: DialogComponent
  @ViewChild('edit', {static: false}) private editDialog: DialogComponent

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.http.get<Question[]>(this.baseUrl + `api/question/author`).subscribe(
      result => this.createdQuestions = result,
      error => console.error(error)
    )
  }

  showTab(i: number) {
    this.selectedTab = i
  }

  onDelete(questionId: number) {
    this.confirmationDialog.open(() => this.deleteQuestion(questionId))
  }

  deleteQuestion(questionId: number) {

    let q = this.createdQuestions.find(x => x.id === questionId);
    let index = this.createdQuestions.indexOf(q);

    this.http.delete(this.baseUrl + `api/question/${questionId}`).subscribe(
      result => this.createdQuestions.splice(index, 1),
      error => console.error(error)
    )
  }

  onEdit(questionId: number) {
    this.questionToEdit = this.createdQuestions.find(value => value.id === questionId)
    this.editDialog.open(() => this.editQuestion(questionId))
  }

  onModify(modifiedQuestion: any) {
    this.modifiedQuestion = modifiedQuestion
  }

  editQuestion(questionId: number) {

    let q = this.createdQuestions.find(x => x.id === questionId);
    let index = this.createdQuestions.indexOf(q);
    let remappedQuestion: Question = {
      id: questionId,
      text: this.modifiedQuestion.text,
      answers: this.modifiedQuestion.answers,
      explanation: this.modifiedQuestion.explanation
    };

    this.modifiedQuestion.id = questionId

    this.http.put(this.baseUrl + `api/question`, this.modifiedQuestion).subscribe(
      result => this.createdQuestions.splice(index, 1, remappedQuestion),
      error => console.error(error)
    )
  }
}
