import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CoreService } from 'src/app/core/core.service';
import { ProdService } from 'src/app/services/prod.service';

@Component({
  selector: 'app-prod-add-edit',
  templateUrl: './prod-add-edit.component.html',
  styleUrls: ['./prod-add-edit.component.scss'],
})
export class ProductAddEditComponent implements OnInit {
  productForm: FormGroup;

  constructor(
    private _fb: FormBuilder,
    private _prodService: ProdService,
    private _dialogRef: MatDialogRef<ProductAddEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _coreService: CoreService
  ) {
    this.productForm = this._fb.group({
      productId:[''],
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(0)]],
      stock: ['', [Validators.required, Validators.min(0)]],
    });
  }

  ngOnInit(): void {
    if (this.data) {
      this.productForm.patchValue(this.data);
      this.productForm.get('productId')?.patchValue(this.data.id)
    }
  }

  onFormSubmit() {
    if (this.productForm.valid) {
      const formValue = { 
        ...this.productForm.value
      };
  
      console.log('Form Value:', formValue,this.productForm.value);
  
      if (this.data && this.data.id) {
        this._prodService.updateProduct(this.data.id, this.productForm.value).subscribe({
          next: (response) => {
            console.log('Product Updated Response:', response);
            this._coreService.openSnackBar('Product details updated!');
            this._dialogRef.close(true);
          },
          error: (err) => {
            console.error('Update Product Error:', err);
            this._coreService.openSnackBar('Failed to update product. Please try again.');
          },
        });
      } else {
        this._prodService.addProduct(formValue).subscribe({
          next: (response) => {
            console.log('Product Added Response:', response);
            this._coreService.openSnackBar('Product added successfully');
            this._dialogRef.close(true);
          },
          error: (err) => {
            console.error('Add Product Error:', err);
            this._coreService.openSnackBar('Failed to add product. Please try again.');
          },
        });
      }
    }
  }
  
  


  onDelete() {
    if (this.data) {
      this._prodService.deleteProduct(this.data.id).subscribe({
        next: () => {
          this._coreService.openSnackBar('Product deleted successfully');
          this._dialogRef.close(true);
        },
        error: (err) => {
          console.error(err);
        },
      });
    }
  }
}
