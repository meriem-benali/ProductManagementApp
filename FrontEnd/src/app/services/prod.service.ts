import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../model/product.model';

// Define the structure of the paginated response
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

@Injectable({
  providedIn: 'root',
})
export class ProdService {
  private apiUrl = 'https://localhost:7288/api/Product/'; // Ensure trailing slash

  constructor(private _http: HttpClient) {}

  addProduct(data: Product): Observable<Product> {
    return this._http.post<Product>(this.apiUrl, data);
  }

  updateProduct(id: string, data: Product): Observable<Product> {
    return this._http.put<Product>(`${this.apiUrl}${id}`, data);
  }

  getProductList(): Observable<ProductResponse> {
    return this._http.get<ProductResponse>(this.apiUrl);
  }

  deleteProduct(id: string): Observable<void> {
    return this._http.delete<void>(`${this.apiUrl}${id}`);
  }
}
