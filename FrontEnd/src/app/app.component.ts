import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ProductAddEditComponent } from './prod-add-edit/prod-add-edit/prod-add-edit.component';
import { ProdService } from './services/prod.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CoreService } from './core/core.service';
import { Product } from './model/product.model';
import Decimal from 'decimal.js';

// Define the interface for the paginated response
interface ProductResponse {
  resultat: {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;
    items: Product[];
  };
  status: number;
  fail_Messages: any;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'description', 'price', 'stock', 'actions'];
  dataSource = new MatTableDataSource<Product>(); // Initialize as an empty MatTableDataSource

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _dialog: MatDialog,
    private _prodService: ProdService,
    private _coreService: CoreService
  ) {}

  ngOnInit(): void {
    this.getProductList();
  }

  openAddProductForm() {
    const dialogRef = this._dialog.open(ProductAddEditComponent);
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getProductList();
        }
      },
    });
  }

  getProductList() {
    this._prodService.getProductList().subscribe({
      next: (res: ProductResponse) => {
        console.log(res); // Log the response to verify structure
        // Check if the response has the expected structure
        if (res.resultat && res.resultat.items) {
          const products = res.resultat.items;

          // Convert price from number to Decimal if necessary
          products.forEach(product => {
            product.price = new Decimal(product.price);
          });

          this.dataSource = new MatTableDataSource(products);
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
        } else {
          console.error('Unexpected response structure:', res);
        }
      },
      error: (err) => {
        console.error('Error fetching product list:', err);
      }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  deleteProduct(id: string) {
    this._prodService.deleteProduct(id).subscribe({
      next: () => {
        this._coreService.openSnackBar('Product deleted!', 'done');
        this.getProductList();
      },
      error: (err) => {
        console.error('Error deleting product:', err);
      },
    });
  }

  openEditForm(data: Product) {
    const dialogRef = this._dialog.open(ProductAddEditComponent, { data });
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getProductList();
        }
      },
    });
  }
}
