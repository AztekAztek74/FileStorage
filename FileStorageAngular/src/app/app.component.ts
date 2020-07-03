import { Component } from '@angular/core';
import { Dataset } from './_interfaces/dataset.model';
import { FileListService } from './services/file-list.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  public dataset: Dataset[];
  constructor(private fileListService: FileListService) {}

  ngOnInit() {
    this.getList();
  }

  getList = () => {
    this.fileListService.getData().subscribe((data) => {
      const inner = data as Dataset[];
      this.dataset = inner.reverse();
    });
  };
}
