import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CunitComponent } from './cunit.component';

describe('CunitComponent', () => {
  let component: CunitComponent;
  let fixture: ComponentFixture<CunitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CunitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CunitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
