import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-empty-content',
  templateUrl: './empty-content.component.html',
  styleUrls: ['./empty-content.component.css']
})
export class EmptyContentComponent implements OnInit {

  private emodjis: string[] = ['¯\\_(ツ)_/¯', '(´。＿。｀)', '(╯°□°）╯︵ ┻━┻', '(⊙ˍ⊙)']
  emodji: string = this.emodjis[Math.random() * this.emodjis.length | 0]

  constructor() { }

  ngOnInit() {
  }
}
