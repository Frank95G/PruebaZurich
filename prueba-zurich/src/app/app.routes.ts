import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'policy/list',
    pathMatch: 'full'
  },
  {
    path: '404',
    loadComponent: () => import('./views/auth/page404/page404.component').then(m => m.Page404Component),
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    loadComponent: () => import('./views/auth/page500/page500.component').then(m => m.Page500Component),
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    loadComponent: () => import('./views/auth/login/login.component').then(m => m.LoginComponent),
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'register',
    loadComponent: () => import('./views/auth/register/register.component').then(m => m.RegisterComponent),
    data: {
      title: 'Register Page'
    }
  },
  {
    path: 'policy',
    canActivate: [AuthGuard],
    loadComponent: () => import('./layout').then(m => m.DefaultLayoutComponent),
    data: {
      title: 'Policy List'
    },
    children: [
      {
        path: 'list',
        loadComponent: () => import('./views/policy/policy-list/policy-list.component').then(m => m.PolicyListComponent),
      },
      {
        canActivate: [RoleGuard],
        path: 'form',
        loadComponent: () => import('./views/policy/policy-form/policy-form.component').then(m => m.PolicyFormComponent),
      },
      {
        canActivate: [RoleGuard],
        path: 'form/:id',
        loadComponent: () => import('./views/policy/policy-form/policy-form.component').then(m => m.PolicyFormComponent),
      },
      {
        path: 'view/:id',
        loadComponent: () => import('./views/policy/policy-view/policy-view.component').then(m => m.PolicyViewComponent),
      }
    ]
  },
  {
    path: 'client',
    canActivate: [AuthGuard, RoleGuard],
    loadComponent: () => import('./layout').then(m => m.DefaultLayoutComponent),
    data: {
      title: 'Client List'
    },
    children: [
      {
        path: 'list',
        loadComponent: () => import('./views/client/client-list/client-list.component').then(m => m.ClientListComponent),
      },
      {
        path: 'form',
        loadComponent: () => import('./views/client/client-form/client-form.component').then(m => m.ClientFormComponent),
      },
      {
        path: 'form/:id',
        loadComponent: () => import('./views/client/client-form/client-form.component').then(m => m.ClientFormComponent),
      },
      {
        path: 'view/:id',
        loadComponent: () => import('./views/client/client-view/client-view.component').then(m => m.ClientViewComponent),
      }
    ]
  },
  { path: '**', redirectTo: 'policy/list' }
];
