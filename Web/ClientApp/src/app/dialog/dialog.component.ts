import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit {

  isOpen: boolean = false

  onConfirm: any
  onCancel: any

  constructor() { }

  ngOnInit() {
  }

  open(onConfirm, onCancel = () => {}) {
    this.isOpen = true
    this.onConfirm = onConfirm
    this.onCancel = onCancel
  }

  confirm() {
    this.onConfirm()
    this.isOpen = false
  }

  cancel() {
    this.onCancel()
    this.isOpen = false
  }
}
