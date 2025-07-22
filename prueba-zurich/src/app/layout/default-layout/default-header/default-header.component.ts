import { AsyncPipe, NgTemplateOutlet, CommonModule  } from '@angular/common';
import { Component, computed, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Router } from '@angular/router'; // Añade este import

import {
  AvatarComponent,
  BreadcrumbRouterComponent,
  ColorModeService,
  ContainerComponent,
  DropdownComponent,
  DropdownDividerDirective,
  DropdownHeaderDirective,
  DropdownItemDirective,
  DropdownMenuDirective,
  DropdownToggleDirective,
  HeaderComponent,
  HeaderNavComponent,
  HeaderTogglerDirective,
  NavLinkDirective,
  SidebarToggleDirective
} from '@coreui/angular';

import { IconDirective } from '@coreui/icons-angular';
import { AuthState, Logout } from '../../../store/auth.state';
import { Store } from '@ngxs/store';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
  imports: [ContainerComponent, HeaderTogglerDirective, SidebarToggleDirective, IconDirective, HeaderNavComponent, NavLinkDirective, RouterLink, NgTemplateOutlet, BreadcrumbRouterComponent, DropdownComponent, DropdownToggleDirective, AvatarComponent, DropdownMenuDirective, DropdownHeaderDirective, DropdownItemDirective, DropdownDividerDirective, AsyncPipe, CommonModule]
})
export class DefaultHeaderComponent extends HeaderComponent {

  readonly #colorModeService = inject(ColorModeService);
  readonly colorMode = this.#colorModeService.colorMode;

  readonly colorModes = [
    { name: 'light', text: 'Light', icon: 'cilSun' },
    { name: 'dark', text: 'Dark', icon: 'cilMoon' },
    { name: 'auto', text: 'Auto', icon: 'cilContrast' }
  ];   
  readonly icons = computed(() => {
    const currentMode = this.colorMode();
    return this.colorModes.find(mode => mode.name === currentMode)?.icon ?? 'cilSun';
  });
  
  private store = inject(Store);
  private router= inject(Router); 
  protected usuario = this.store.select(AuthState.usuario);
  
  constructor() {
    super();
  }

  sidebarId = input('sidebar1');

  logout() {
    this.store.dispatch(new Logout()).subscribe({
      complete: () => {
        // Redirige después de limpiar todo
        this.router.navigate(['/login'], {
          queryParams: { logout: 'success' }
        });
      },
      error: () => {
        this.router.navigate(['/login'], {
          queryParams: { logout: 'error' }
        });
      }
    });
  }
  
  profile() {
    this.router.navigate(['/client/form']);
  }
}
