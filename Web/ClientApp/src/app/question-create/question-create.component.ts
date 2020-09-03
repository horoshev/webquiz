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
      type: ['', [Validators.required]],
      difficulty: ['', [Validators.required]],
      category: ['', [Validators.required]],
      text: ['', [Validators.required]],
      correctAnswers: fb.array([fb.control('')]),
      explanation: ['', [Validators.required]],
    })
  }

  get answersFormArray() {
    return (<FormArray> this.questionForm.get('correctAnswers'));
  }

  @Input()
  set question(question: Question) {

    if (!question) return

    let value = {
      text: question.text,
      correctAnswers: question.correctAnswers,
      explanation: question.explanation,
    }

    let answersArray = this.questionForm.get('correctAnswers') as FormArray
    answersArray.clear()

    value.correctAnswers.forEach(() => this.addAnswer())
    this.questionForm.patchValue(value)
  }

  @Output() modifiedQuestion = new EventEmitter<any>();

  typeOptions: { value: number, label: string }[];
  difficultyOptions: { value: number, label: string }[];
  categoryOptions: { value: number, label: string }[];

  onChange() {
    this.modifiedQuestion.emit(this.questionForm.value)
  }

  ngOnInit() {
    this.http.get<{ value: number, label: string }[]>(this.baseUrl + 'api/question/types').subscribe(value => this.typeOptions = value)
    this.http.get<{ value: number, label: string }[]>(this.baseUrl + 'api/question/difficulties').subscribe(value => this.difficultyOptions = value)
    this.http.get<{ value: number, label: string }[]>(this.baseUrl + 'api/question/categories').subscribe(value => this.categoryOptions = value)
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
