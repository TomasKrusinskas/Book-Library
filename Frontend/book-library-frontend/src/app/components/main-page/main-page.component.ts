import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import {MatToolbar} from '@angular/material/toolbar';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatToolbar],
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent {
  public currentYear = new Date().getFullYear();
}
