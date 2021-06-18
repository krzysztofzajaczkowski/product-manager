import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductDto } from '../models/productDto';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit {
  sku: string;
  product: ProductDto;
  productForm: FormGroup;
  error: string;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private router: Router,
    private productService: ProductService
  ) { }

  costAndNetPriceValidator(form: FormGroup) {
    const cost = form.get('cost').value;
    const netPrice = form.get('netPrice').value;
    return netPrice > cost ? null : {
      validation: true
    };
  }

  ngOnInit() {
    this.sku = this.route.snapshot.paramMap.get('sku');
    this.productForm = this.formBuilder.group({
      catalog: this.formBuilder.group({
        name: [{
          value: '',
           disabled: !this.canUpdateCatalog
          }, [
            Validators.required
          ]],
        description: [{
          value: '',
          disabled: !this.canUpdateCatalog
        },[
          Validators.required
        ]]
      }),
      warehouse: this.formBuilder.group({
        stock: [{
          value: null,
          disabled: !this.canUpdateWarehouse
        }, [
          Validators.required,
          Validators.min(0)
        ]],
        weight: [{
          value: null,
          disabled: !this.canUpdateWarehouse
        }, [
          Validators.required,
          Validators.min(0)
        ]],
      }),
      sales: this.formBuilder.group({
        cost: [{
          value: null,
          disabled: !this.canUpdateSales
        }, [
          Validators.required,
          Validators.min(1)
        ]],
        netPrice: [{
          value: null,
          disabled: !this.canUpdateSales
        }, [
          Validators.required,
          Validators.min(2)
        ]],
        taxPercentage: [{
          value: null,
          disabled: !this.canUpdateSales
        }, [
          Validators.required,
          Validators.min(0)
        ]],
      }, {
        validator: this.costAndNetPriceValidator
      })
    });

    this.productService.get(this.sku).subscribe(
      (data: ProductDto) => {
        this.product = data;
        this.productForm.patchValue({
          catalog: {
            name: data.name,
            description: data.description
          },
          warehouse: {
            stock: data.stock,
            weight: data.weight
          },
          sales: {
            cost: data.cost,
            netPrice: data.netPrice,
            taxPercentage: data.taxPercentage
          }
        })
      },
      (error) => {
        this.router.navigate(['/browse']);
      }
    );
  }

  get productCatalog() {
    return this.productForm.get('catalog');
  }

  get productCatalogValid() {
    return this.productCatalog.valid;
  }

  get canUpdateCatalog() {
    return this.productService.canUpdateCatalog;
  }

  updateCatalog() {
    let catalogForm = this.productForm.get('catalog').value;
    this.productService.updateCatalog(this.product.catalogId, catalogForm.name, this.sku, catalogForm.description).subscribe(
      (data) => {
        this.router.navigate(['/browse']);
      },
      (error) => {
        this.error = error.error.message;
      }
    );
  }

  get productWarehouse() {
    return this.productForm.get('warehouse');
  }

  get productWarehouseValid() {
    return this.productWarehouse.valid;
  }

  get canUpdateWarehouse() {
    return this.productService.canUpdateWarehouse;
  }

  updateWarehouse() {
    let warehouseForm = this.productForm.get('warehouse').value;
    this.productService.updateWarehouse(this.product.warehouseId, this.sku, warehouseForm.stock, warehouseForm.weight).subscribe(
      (data) => {
        this.router.navigate(['/browse']);
      },
      (error) => {
        this.error = error.error.message;
      }
    );
  }

  get productSales() {
    return this.productForm.get('sales');
  }

  get productSalesValid() {
    return this.productSales.valid;
  }

  get canUpdateSales() {
    return this.productService.canUpdateSales;
  }

  updateSales() {
    let salesForm = this.productForm.get('sales').value;
    this.productService.updateSales(this.product.salesId, this.sku, salesForm.cost, salesForm.netPrice, salesForm.taxPercentage).subscribe(
      (data) => {
        this.router.navigate(['/browse']);
      },
      (error) => {
        this.error = error.error.message;
      }
    );
  }

}
