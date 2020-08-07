import {Component, Inject, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-question-create',
  templateUrl: './question-create.component.html',
  styleUrls: ['./question-create.component.css']
})
export class QuestionCreateComponent implements OnInit {

  questionForm: FormGroup

  constructor(private fb: FormBuilder, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.questionForm = fb.group({
      text: ['', Validators.required],
      answers: fb.array([fb.control('')]),
      explanation: ['', Validators.required],
    })
  }

  get answersFormArray() {
    return (<FormArray> this.questionForm.get('answers'));
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
    console.log(this.questionForm.value)

    this.http.post(this.baseUrl + 'api/question', this.questionForm.value).subscribe(
      result => console.log('success'),
      error => console.log(error)
    )
  }
}
