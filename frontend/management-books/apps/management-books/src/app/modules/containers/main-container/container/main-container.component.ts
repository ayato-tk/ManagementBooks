import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../components/navbar/navbar.component';

@Component({
  selector: 'app-main-container',
  templateUrl: './main-container.component.html',
  styleUrls: ['./main-container.component.scss'],
  imports: [RouterModule, NavbarComponent]
})
export class MainContainerComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
