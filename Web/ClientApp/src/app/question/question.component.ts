import {Component, Input, OnInit} from '@angular/core';
import {Question, QuestionCategoryOptions, QuestionDifficultyOptions, QuestionTypeOptions} from "../../types/Question";

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {

  @Input() question: Question
  @Input() openControls: boolean
  @Input() openAnswers: boolean

  get Difficulty() {
    let option = QuestionDifficultyOptions.find(value => value.value == this.question.difficulty)
    return option == undefined ? "" : option.label
  }

  get Type() {
    let option = QuestionTypeOptions.find(value => Number.parseInt(value.value) === +this.question.type)
    return option == undefined ? "" : option.label
  }

  get Category() {
    let option = QuestionCategoryOptions.find(value => value.value == this.question.category)
    return option == undefined ? "" : option.label
  }

  constructor() { }

  ngOnInit() {
  }
}
