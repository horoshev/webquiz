import {Component, Input, OnInit} from '@angular/core';
import {Question} from "../../types/Question";

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {

  @Input() question: Question
  @Input() openControls: boolean
  @Input() openAnswers: boolean

  constructor() { }

  ngOnInit() {
  }
}
