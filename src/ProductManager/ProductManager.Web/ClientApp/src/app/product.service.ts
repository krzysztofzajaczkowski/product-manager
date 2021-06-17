import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { CreateProductDto } from './models/createProductDto';
import { ProductBlockDto } from './models/productBlockDto';
import { ProductDto } from './models/productDto';
import { UpdateCatalogProductDto } from './models/updateCatalogProductDto';
import { UpdateSalesProductDto } from './models/UpdateSalesProductDto';
import { UpdateWarehouseProductDto } from './models/UpdateWarehouseProductDto';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(
    private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private authService: AuthService
  ) { }

  browse(): Observable<any> {
    return this.httpClient.get<ProductBlockDto[]>(this.baseUrl + 'products/browse');
  }

  get(sku: string): Observable<any> {
    return this.httpClient.get<ProductDto>(this.baseUrl + `products/browse/${sku}`);
  }

  create(name: string, sku: string, description: string) : Observable<any> {
    var createProductDto: CreateProductDto = {
      name: name,
      sku: sku,
      description: description
    };
    return this.httpClient.post(this.baseUrl + 'products/create', createProductDto);
  }

  get canUpdateCatalog() {
    return this.authService.role == "CatalogManager";
  }

  get canUpdateSales() {
    return this.authService.role == "SalesManager";
  }

  get canUpdateWarehouse() {
    return this.authService.role == "WarehouseManager";
  }

  updateCatalog(id: string, name: string, sku: string, description: string) : Observable<any> {
    var updateProductCatalogDto: UpdateCatalogProductDto = {
      id: id,
      name: name,
      sku: sku,
      description: description
    };
    return this.httpClient.post(this.baseUrl + 'products/update/catalog', updateProductCatalogDto);
  }

  updateWarehouse(id: string, sku: string, stock: number, weight: number) : Observable<any> {
    var updateProductWarehouseDto: UpdateWarehouseProductDto = {
      id: id,
      sku: sku,
      stock: stock,
      weight: weight
    };
    return this.httpClient.post(this.baseUrl + 'products/update/warehouse', updateProductWarehouseDto);
  }

  updateSales(id: string, sku: string, cost: number, netPrice: number, taxPercentage: number) : Observable<any> {
    var updateProductSalesDto: UpdateSalesProductDto = {
      id: id,
      sku: sku,
      cost: cost,
      netPrice: netPrice,
      taxPercentage: taxPercentage
    };
    return this.httpClient.post(this.baseUrl + 'products/update/sales', updateProductSalesDto);
  }
}
