import { Component, OnInit, Input } from '@angular/core';
import { Dataset } from '../_interfaces/dataset.model';

@Component({
  selector: 'app-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss'],
})
export class FileListComponent implements OnInit {
  constructor() {}
  @Input() dataset: Dataset[];
  @Input() getList: Function;

  ngOnInit() {}
}
