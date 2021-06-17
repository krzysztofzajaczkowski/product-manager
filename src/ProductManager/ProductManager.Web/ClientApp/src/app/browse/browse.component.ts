import { Component, OnInit } from '@angular/core';
import { ProductBlockDto } from '../models/productBlockDto';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-browse',
  templateUrl: './browse.component.html',
  styleUrls: ['./browse.component.css']
})
export class BrowseComponent implements OnInit {
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

  ngOnInit() {
  }

}
