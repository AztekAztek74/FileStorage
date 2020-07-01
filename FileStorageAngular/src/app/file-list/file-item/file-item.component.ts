import { Component, OnInit, Input } from '@angular/core';
import { Dataset } from 'src/app/_interfaces/dataset.model';
import { FileListService } from '../../services/file-list.service';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-file-item',
  templateUrl: './file-item.component.html',
  styleUrls: ['./file-item.component.scss'],
})
export class FileItemComponent implements OnInit {
  @Input() item: Dataset;
  @Input() getList: Function;
  constructor(private fileListService: FileListService) {}

  ngOnInit(): void {}

  onDelete(id: number) {
    this.fileListService.deleteFile(id).subscribe((event) => {
      this.getList();
    });
  }

  onDownload(id: number, name: string) {
    this.fileListService.downloadFile(id).subscribe((resp) => {
      console.log(resp);
      this.downLoadFile(resp, name);
    });
    console.log(id);
  }

  downLoadFile(data: any, name: string) {
    let blob = new Blob([data]);
    let url = window.URL.createObjectURL(blob);
    let anchor = document.createElement('a');
    anchor.download = name;
    anchor.href = url;
    anchor.click();
  }

  onClick(item: Dataset) {
    console.log(item);
  }
}
