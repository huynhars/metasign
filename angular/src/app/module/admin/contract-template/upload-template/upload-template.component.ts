import { Component, Inject, OnInit } from '@angular/core';
import { ContractTemplateType } from '@app/module/contract-templates/enum/contract-template.enum';
import { ContractTemplateService } from '@app/service/api/contract-template.service';
import { CreateTemplateDto } from '../contract-template.component';
import { ContractService } from '@app/service/api/contract.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ContractTemplates } from '@app/module/contract-templates/interface/contract-templates';

@Component({
  selector: 'app-upload-template',
  templateUrl: './upload-template.component.html',
  styleUrls: ['./upload-template.component.css']
})
export class UploadTemplateComponent implements OnInit {

  // Định nghĩa chuẩn theo interface CreateTemplateDto
  newTemplate: Partial<CreateTemplateDto> = {};
  file: File;
  isConverting: boolean = false;
  isUploadComplete: boolean = false;
  title: string = "";

  constructor(
    private contractTemplateService: ContractTemplateService,
    private ref: MatDialogRef<UploadTemplateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ContractTemplates,
    private contractService: ContractService
  ) { }

  ngOnInit(): void {
    if (this.data) {
      this.newTemplate = { ...this.data };
      this.title = `Update template`;
    } else {
      this.title = `Add template`;
    }
  }

  save() {
    // Ép kiểu chuẩn Payload DTO trước khi submit
    const payload: CreateTemplateDto = {
      name: this.newTemplate.name || '',
      fileName: this.newTemplate.fileName || '',
      content: this.newTemplate.content || '',
      htmlContent: this.newTemplate.htmlContent || '',
      isFavorite: this.newTemplate.isFavorite || false,
      massField: this.newTemplate.massField || '',
      massType: 1,
      massWordContent: this.newTemplate.massWordContent || '',
      type: ContractTemplateType.System,
      userId: this.newTemplate.userId || 0
    };

    if (!this.data) {
      this.contractTemplateService.createFileTemplate(payload).subscribe({
        next: () => {
          abp.notify.success("Created new template");
          this.ref.close(true);
        },
        error: (err) => {
          console.error("Create error:", err);
        }
      });
    } else {
      this.contractTemplateService.updateFileTemplate({ ...payload, id: this.data.id } as any).subscribe({
        next: () => {
          abp.notify.success("Updated template");
          this.ref.close(true);
        },
        error: (err) => {
          console.error("Update error:", err);
        }
      });
    }
  }

  onFileSelected(event: any): void {
    this.file = event.target.files[0];
    if (!this.file) return;

    if (this.checkFileUpload(this.file.type)) {
      this.isConverting = true;
      this.isUploadComplete = false;

      this.contractService.ConvertFile(this.file).subscribe({
        next: (rs) => {
          this.isUploadComplete = true;
          this.isConverting = false;

          // Cắt lấy base64 thuần nếu response từ convertFile chứa prefix
          const base64 = rs.result.base64String || '';
          this.newTemplate.content = base64.includes(',') ? base64.split(',')[1] : base64;
          this.newTemplate.fileName = rs.result.fileName;
        },
        error: () => {
          this.isConverting = false;
          this.isUploadComplete = true;
          abp.message.error("Convert file không thành công, hãy thử lại!");
        }
      });
      return;
    }

    // Đọc file PDF/Image trực tiếp
    const reader = new FileReader();
    reader.readAsDataURL(this.file);
    reader.onload = () => {
      this.isUploadComplete = true;
      const rawBase64 = reader.result.toString();

      // CẮT BỎ PREFIX "data:application/pdf;base64," ĐỂ LẤY CHUỖI BASE64 THUẦN
      this.newTemplate.content = rawBase64.includes(',') ? rawBase64.split(',')[1] : rawBase64;
      this.newTemplate.fileName = this.file.name;
    };
  }

  checkFileUpload(type: string): boolean {
    const validTypes = ["application/pdf"];
    // Nếu KHÔNG phải PDF (ví dụ .docx, .doc) -> Trả về true để gọi API ConvertFile
    return !validTypes.includes(type);
  }
}