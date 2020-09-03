import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {QuestionQuery} from "../../types/QuestionQuery";

@Component({
  selector: 'app-question-filter',
  templateUrl: './question-filter.component.html',
  styleUrls: ['./question-filter.component.css']
})
export class QuestionFilterComponent implements OnInit {

  @Input()  query: QuestionQuery = new QuestionQuery("", "", "")
  @Output() queryChange = new EventEmitter<QuestionQuery>()

  typeOptions = [
    {label: "Any Type", value: ""},
    {label: "Binary (Yes or No)", value: "0"},
    {label: "With Choice", value: "1"},
    {label: "Without Choice", value: "2"},
  ]

  difficultyOptions = [
    {label: "Any Difficulty", value: ""},
    {label: "Easy", value: "0"},
    {label: "Medium", value: "1"},
    {label: "Hard", value: "2"},
  ]

  categoryOptions = [
    {label: "Any Category", value: ""},
    {label: "None", value: "0"},
    {label: "General", value: "1"},
    {label: "Books", value: "2"},
    {label: "Films", value: "Films"},
    {label: "Music", value: "Music"},
    {label: "VideoGames", value: "VideoGames"},
    {label: "Science", value: "Science"},
    {label: "Nature", value: "Nature"},
    {label: "Computers", value: "Computers"},
    {label: "Mathematics", value: "Mathematics"},
    {label: "Mythology", value: "Mythology"},
    {label: "Sports", value: "Sports"},
    {label: "Geography", value: "Geography"},
    {label: "History", value: "History"},
    {label: "Politics", value: "Politics"},
    {label: "Art", value: "Art"},
    {label: "Animals", value: "Animals"},
    {label: "Vehicles", value: "Vehicles"},
    {label: "Comics", value: "Comics"},
    {label: "Anime", value: "Anime"},
    {label: "Cartoons", value: "Cartoons"},
  ]

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
