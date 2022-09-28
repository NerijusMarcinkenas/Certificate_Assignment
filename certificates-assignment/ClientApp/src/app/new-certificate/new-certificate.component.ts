import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  templateUrl: './new-certificate.component.html',
})
export class NewCertificateComponent implements OnInit {

  form: FormGroup;
  nameValidation = '';
  birtdateValidation = '';
  itemValidation = '';
  sumValidation = '';
  error = '';

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.form = this.fb.group({
      customerName: ['', [Validators.required]],
      customerDateOfBirth: ['', [Validators.required]],
      insuredItem: ['', [Validators.required]],
      insuredSum: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
  }

  onSubmit() {
    const { valid, value } = this.form;
    this.resteFields();    

    if (valid) {
      this.http.post(this.baseUrl + 'certificates', value)
        .subscribe(result => {
          this.router.navigateByUrl('/');
        }, error => {
          this.error = error.error.message;
          
        });
    } else {
      this.validateFields(value);      
    }
  }

  resteFields() {
    this.error = '';
    this.nameValidation = '';
    this.birtdateValidation = '';
    this.itemValidation = '';
    this.sumValidation = '';
  }
  validateFields(value: { customerName: string; customerDateOfBirth: string; insuredItem: string; insuredSum: number | boolean; }) {
    if (value.customerName == '') {
      this.nameValidation = "Name is required";
    }
    if (value.customerDateOfBirth == '') {
      this.birtdateValidation = "Please enter birth date"
    }
    if (value.insuredItem == '') {
      this.itemValidation = "Please enter insured item";
    }
    if (value.insuredSum <= 0) {
      this.sumValidation = "Sum must be greater than 0";
    }
  }
}

