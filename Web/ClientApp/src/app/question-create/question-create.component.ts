import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {Question} from "../../types/Question";
import {Router} from "@angular/router";

@Component({
  selector: 'app-question-create',
  templateUrl: './question-create.component.html',
  styleUrls: ['./question-create.component.css']
})
export class QuestionCreateComponent implements OnInit {

  @Input() showTitle: boolean = true
  @Input() showControls: boolean = true

  errors: [string, unknown][] = []
  questionForm: FormGroup

  constructor(private fb: FormBuilder, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) {
    this.questionForm = fb.group({
      text: ['', Validators.required],
      answers: fb.array([fb.control('')]),
      explanation: ['', Validators.required],
    })
  }

  get answersFormArray() {
    return (<FormArray> this.questionForm.get('answers'));
  }

  @Input()
  set question(question: Question) {

    if (!question) return

    let value = {
      text: question.text,
      answers: question.answers,
      explanation: question.explanation,
    }

    let answersArray = this.questionForm.get('answers') as FormArray
    answersArray.clear()

    value.answers.forEach(() => this.addAnswer())
    this.questionForm.patchValue(value)
  }

  @Output() modifiedQuestion = new EventEmitter<any>();

  onChange() {
    this.modifiedQuestion.emit(this.questionForm.value)
  }

  get tt() {

    let value = this.questionForm.value
    let quest: Question

    quest = {
      text: value.text,
      answers: value.answers.join(' '),
      explanation: value.explanation
    }

    return this.questionForm.value
    return quest
  }

  ngOnInit() {
  }

  createAnswerGroup() {
    return this.fb.control('', Validators.required)
  }

  addAnswer() {
    this.answersFormArray.push(this.createAnswerGroup());
  }

  removeAnswer(index: number) {
    this.answersFormArray.removeAt(index)
  }

  onSubmit() {
    this.http.post(this.baseUrl + 'api/question', this.questionForm.value).subscribe(
      result => this.router.navigate([`/question/${result}`]),
      error => {
        console.error(error);
        this.errors = Object.entries(error.error.errors)
      }
    )
  }
}

interface Errors {
  [key: string]: string
}
