import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DicComponent } from './dic.component';

describe('DicComponent', () => {
  let component: DicComponent;
  let fixture: ComponentFixture<DicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
