import {Component, Input, OnInit} from '@angular/core';
import {Question} from "../../types/Question";
import {ActivatedRoute, Data} from "@angular/router";
import {Observable} from "rxjs";

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css'],
  providers: []
})
export class QuestionListComponent implements OnInit {

  @Input() questions: Question[]

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
  }
}
