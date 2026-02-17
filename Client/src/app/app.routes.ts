import { Routes } from '@angular/router';
import { ClientRequest } from './components/client-request/client-request';
import { VendorDashboard } from './components/vendor-dashboard/vendor-dashboard';

export const routes: Routes = [
  { path: 'client', component: ClientRequest },
  { path: 'vendor', component: VendorDashboard },
  { path: '', redirectTo: '/client', pathMatch: 'full' } // Default page
];