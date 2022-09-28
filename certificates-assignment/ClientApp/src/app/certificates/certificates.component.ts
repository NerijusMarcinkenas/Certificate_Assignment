import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Certificate } from '../models/certificate';

@Component({
  selector: 'app-certificates',
  templateUrl: './certificates.component.html'
})
export class CertificatesComponent implements OnInit {
  public certificates: Certificate[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit(): void {
    this.http.get<Certificate[]>(this.baseUrl + 'certificates').subscribe(result => {
      this.certificates = result;
    }, error => console.error(error));
  }

}
