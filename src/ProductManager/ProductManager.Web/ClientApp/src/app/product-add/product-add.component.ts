import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CreateProductDto } from '../models/createProductDto';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-product-add',
  templateUrl: './product-add.component.html',
  styleUrls: ['./product-add.component.css']
})
export class ProductAddComponent implements OnInit {
  createForm: FormGroup;
  error: string;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private productService: ProductService
  ) { }

  ngOnInit() {
    this.createForm = this.formBuilder.group({
      sku: ['',[
        Validators.required
      ]],
      name: ['',[
        Validators.required
      ]],
      description: ['', [

      ]]
    });
  }

  get formValid() {
    return this.createForm.valid;
  }

  createProduct() {
    let formValues = this.createForm.value;
    this.productService.create(formValues.name, formValues.sku, formValues.description).subscribe(
      (data) => {
        this.router.navigate(['/browse']);
      },
      (error) => {
        this.error = error.error.message;
      }
    );
  }

}
