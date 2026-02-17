import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-client-request',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './client-request.html',
  styleUrl: './client-request.css',
})
export class ClientRequest implements OnInit {
  isModalOpen = false;
  modalMode: 'create' | 'view' = 'create';
  
  requests = signal<any[]>([]);
  selectedRequest = signal<any | null>(null);

  pendingCount = computed(() => {
    return this.requests().filter(req => 
      req.vendorAssignments?.some((v: any) => !v.acknowledgedAt)
    ).length;
  });

  newRequest: { 
    clientId: number | null;
    itemDetails: string; 
    vendorIds: string; 
    isFlagged: boolean; 
  } = {
    clientId: null, 
    itemDetails: '',
    vendorIds: '',
    isFlagged: false
  };

  private apiUrl = 'http://localhost:5000/api/requests';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadRequests(); 
  }

  loadRequests() {
    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.requests.set(data);
        console.log("Data loaded and UI updated:", data.length);
      },
      error: (err) => console.error("API Error:", err)
    });
  }

  openCreateModal() {
    this.modalMode = 'create';
    this.isModalOpen = true;
  }

  viewDetails(id: number) {
    this.modalMode = 'view';
    this.isModalOpen = true;
    
    this.selectedRequest.set(null); 
  
    this.http.get<any>(`${this.apiUrl}/${id}`).subscribe({
      next: (data) => {
        this.selectedRequest.set(data); 
      },
      error: (err) => {
        console.error("View error:", err);
        this.closeModal();
      }
    });
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedRequest.set(null);
  }

  submitRequest() {
    const payload = {
      clientId: Number(this.newRequest.clientId), 
      itemDetails: this.newRequest.itemDetails,
      vendorIds: typeof this.newRequest.vendorIds === 'string' 
        ? this.newRequest.vendorIds.split(',').map(id => id.trim()).filter(id => id !== "")
        : this.newRequest.vendorIds
    };
  
    console.log("Sending payload to backend:", payload);
  
    this.http.post(this.apiUrl, payload).subscribe({
      next: () => {
        this.closeModal();
        this.loadRequests(); 
        this.newRequest = { clientId: null, itemDetails: '', vendorIds: '', isFlagged: false };
      },
      error: (err) => {
        console.error("Backend Error Details:", err.error);
        alert("Submission failed. Check the console for details.");
      }
    });
  }
}