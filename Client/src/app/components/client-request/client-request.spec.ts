import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientRequest } from './client-request';

describe('ClientRequest', () => {
  let component: ClientRequest;
  let fixture: ComponentFixture<ClientRequest>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientRequest]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientRequest);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
