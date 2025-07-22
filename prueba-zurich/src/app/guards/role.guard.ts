// role.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { AuthState } from '../store/auth.state';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private store: Store, private router: Router) {}

  canActivate(
  ): Observable<boolean> | boolean {
    const requiredRoles = ['Administrador'] as Array<string>;
    
    return this.store.select(AuthState.usuario).pipe(
      map(usuario => {
        if (!usuario) {
          this.router.navigate(['/login']);
          return false;
        }
        
        if (requiredRoles && !requiredRoles.includes(usuario.rol)) {
          this.router.navigate(['/404']);
          return false;
        }
        
        return true;
      })
    );
  }
}