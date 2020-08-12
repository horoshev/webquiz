import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { QuestionDataComponent } from './question-data/question-data.component';
import { QuestionViewComponent } from './question-view/question-view.component';
import { QuestionRandomComponent } from './question-random/question-random.component';
import { QuestionCreateComponent } from './question-create/question-create.component';
import { ProfileComponent } from './profile/profile.component';
import { QuestionComponent } from './question/question.component';
import { DialogComponent } from './dialog/dialog.component';
import { StatsComponent } from './stats/stats.component';
import { SettingsComponent } from './settings/settings.component';
import { QuestionListComponent } from './question-list/question-list.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent,
    QuestionDataComponent,
    QuestionViewComponent,
    QuestionRandomComponent,
    QuestionCreateComponent,
    ProfileComponent,
    QuestionComponent,
    DialogComponent,
    StatsComponent,
    SettingsComponent,
    QuestionListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'question', component: QuestionDataComponent },
      { path: 'question/create', component: QuestionCreateComponent },
      { path: 'question/:id', component: QuestionViewComponent },
      { path: 'quiz', component: QuestionRandomComponent },
      {
        path: 'profile',
        component: ProfileComponent,
        children: [
          {path: '', redirectTo: 'stats', pathMatch: 'full'},
          {path: 'stats', component: StatsComponent},
          {path: 'questions', component: QuestionRandomComponent},
          {path: 'notifications', component: StatsComponent},
          {path: 'settings', component: SettingsComponent}
        ]
      },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
