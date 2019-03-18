import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'; 
import { FormsModule } from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';

import { AppComponent } from './app.component';
import { CurrencyFilterComponent } from './currency-rate-list/currency-filter/currency-filter.component';
import { ExchangeRateListComponent } from './currency-rate-list/exchange-rate-list.component';

@NgModule({
  declarations: [
    AppComponent,
    ExchangeRateListComponent,
    CurrencyFilterComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    NgSelectModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
