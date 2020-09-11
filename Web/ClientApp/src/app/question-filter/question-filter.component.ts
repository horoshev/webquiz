import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {QuestionQuery} from "../../types/QuestionQuery";
import {QuestionCategoryOptions, QuestionDifficultyOptions, QuestionTypeOptions} from "../../types/Question";

@Component({
  selector: 'app-question-filter',
  templateUrl: './question-filter.component.html',
  styleUrls: ['./question-filter.component.css']
})
export class QuestionFilterComponent implements OnInit {

  @Input()  query: QuestionQuery = new QuestionQuery("", "", "")
  @Output() queryChange = new EventEmitter<QuestionQuery>()

  typeOptions = QuestionTypeOptions
  difficultyOptions = QuestionDifficultyOptions
  categoryOptions = QuestionCategoryOptions

  constructor() { }

  ngOnInit() {
  }

  onTypeChange(value: string) {
    this.query.type = value
    this.queryChange.emit(this.query)
  }

  onDifficultyChange(value: string) {
    this.query.difficulty = value
    this.queryChange.emit(this.query)
  }

  onCategoryChange(value: string) {
    this.query.category = value
    this.queryChange.emit(this.query)
  }
}
