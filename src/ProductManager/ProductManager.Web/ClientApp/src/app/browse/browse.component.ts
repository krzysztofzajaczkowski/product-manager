import { Component, OnInit } from '@angular/core';
import { ProductBlockDto } from '../models/productBlockDto';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-browse',
  templateUrl: './browse.component.html',
  styleUrls: ['./browse.component.css']
})
export class BrowseComponent implements OnInit {

  displayedColumns: string[] = ['sku', 'name', 'net price', 'stock', 'actions'];
  items: ProductBlockDto[];

  constructor(
    private productService: ProductService
  ) {
    this.productService.browse().subscribe(
      (data: ProductBlockDto[]) => {
        this.items = data;
      },
      (error) => {
      }
    );
  }

  get canAddProduct() {
    return this.productService.canUpdateCatalog;
  }

  ngOnInit() {
  }

}
