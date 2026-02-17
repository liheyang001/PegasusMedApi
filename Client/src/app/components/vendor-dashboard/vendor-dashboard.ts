import { Component, OnInit, signal, inject, effect, computed } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vendor-dashboard',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule],
  templateUrl: './vendor-dashboard.html',
  styleUrl: './vendor-dashboard.css',
})
export class VendorDashboard implements OnInit {
  private http = inject(HttpClient);

  vendorList = signal<string[]>([]); 
  selectedVendorId = signal<string>('');
  allRequests = signal<any[]>([]);

  pendingActionCount = computed(() => {
    return this.allRequests().filter(req => !req.isFlagged).length;
  });

  constructor() {
    effect(() => {
      const vendorId = this.selectedVendorId();
      if (vendorId) {
        this.loadRequests(vendorId);
      }
    });
  }

  ngOnInit() {
    this.loadVendors();
  }

  loadVendors() {
    this.http.get<string[]>('http://localhost:5000/api/requests/vendors').subscribe({
      next: (data) => {
        this.vendorList.set(data);
        if (data.length > 0 && !this.selectedVendorId()) {
          this.selectedVendorId.set(data[0]); 
        }
      },
      error: (err) => console.error("Error fetching vendors:", err)
    });
  }

  loadRequests(vendorId: string) {
    this.http.get<any[]>(`http://localhost:5000/api/requests/vendor/${vendorId}`).subscribe({
      next: (data) => this.allRequests.set(data),
      error: (err) => this.allRequests.set([])
    });
  }

  acknowledgeRequest(requestId: number) {
    const vendorId = this.selectedVendorId();
    const url = `http://localhost:5000/api/requests/${requestId}/vendor/${vendorId}/acknowledge`;

    this.http.patch(url, {}).subscribe({
      next: () => this.loadRequests(vendorId), 
      error: (err) => alert("Failed to acknowledge request")
    });
  }

  onVendorChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    this.selectedVendorId.set(value);
  }
}