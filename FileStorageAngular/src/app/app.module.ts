import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { UploadComponent } from './upload/upload.component';
import { FileListComponent } from './file-list/file-list.component';
import { FileItemComponent } from './file-list/file-item/file-item.component';

@NgModule({
  declarations: [
    AppComponent,
    UploadComponent,
    FileListComponent,
    FileItemComponent,
  ],
  imports: [BrowserModule, FormsModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
