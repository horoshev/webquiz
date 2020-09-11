import {Component, Inject, OnInit, ViewChild} from '@angular/core';
import {Question} from "../../types/Question";
import {HttpClient} from "@angular/common/http";
import {DialogComponent} from "../dialog/dialog.component";
import {AuthorizeService} from "../../api-authorization/authorize.service";

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

  isAdmin: boolean
  modifiedQuestion: any = null
  questionToEdit: Question
  createdQuestions: Question[] = []

  @ViewChild('confirmation', {static: false}) private confirmationDialog: DialogComponent
  @ViewChild('edit', {static: false}) private editDialog: DialogComponent

  constructor(private authorizeService: AuthorizeService, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    // this.isAdmin =

    let user = authorizeService.getUser().subscribe(value => console.log(value))
    console.log(user)
  }

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
      type: this.modifiedQuestion.type,
      category: this.modifiedQuestion.category,
      difficulty: this.modifiedQuestion.difficulty,
      text: this.modifiedQuestion.text,
      explanation: this.modifiedQuestion.explanation,
      correctAnswers: this.modifiedQuestion.correctAnswers,
      incorrectAnswers: this.modifiedQuestion.incorrectAnswers,
    };

    this.modifiedQuestion.id = questionId

    this.http.put(this.baseUrl + `api/question`, this.modifiedQuestion).subscribe(
      result => this.createdQuestions.splice(index, 1, remappedQuestion),
      error => console.error(error)
    )
  }

  onGenerate() {

    console.log('GENERATING')

    this.http.post<void>(this.baseUrl + `api/seed/${10}`, {}).subscribe(
      result => console.log(result),
      error => console.error(error)
    )
  }
}
