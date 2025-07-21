import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';

// Acciones
export class Login {
  static readonly type = '[Auth] Login';
  constructor(public payload: { token: string; usuario: any }) {}
}

export class Logout {
  static readonly type = '[Auth] Logout';
}

export class LoginFailed {
  static readonly type = '[Auth] Login Failed';
}

// Modelo
interface AuthStateModel {
  token: string | null;
  usuario: {
    usuarioId: string;
    email: string;
    nombreUsuario: string;
    rol: string;
    clienteId: number | null;
  } | null;
}

@State<AuthStateModel>({
  name: 'auth',
  defaults: {
    token: null,
    usuario: null
  }
})
@Injectable()
export class AuthState {
  @Selector()
  static token(state: AuthStateModel) {
    return state.token;
  }

  @Selector()
  static usuario(state: AuthStateModel) {
    return state.usuario;
  }

  @Selector()
  static isAuthenticated(state: AuthStateModel) {
    return !!state.token;
  }

  @Action(Login)
  login(ctx: StateContext<AuthStateModel>, action: Login) {
    const state = {
      token: action.payload.token,
      usuario: action.payload.usuario
    };
    
    ctx.setState(state);
    
    // Guardar en localStorage
    localStorage.setItem('auth', JSON.stringify(state));
  }

  @Action(Logout)
  logout(ctx: StateContext<AuthStateModel>) {
    ctx.setState({
      token: null,
      usuario: null
    });
    
    // Limpiar localStorage
    localStorage.removeItem('auth');
  }

  @Action(LoginFailed)
  loginFailed(ctx: StateContext<AuthStateModel>) {
    ctx.patchState({
      token: null,
      usuario: null
    });
    
    // Limpiar localStorage en caso de fallo
    localStorage.removeItem('auth');
  }

  // Método especial de NGXS que se ejecuta al inicializar el estado
  ngxsOnInit(ctx: StateContext<AuthStateModel>) {
    const storedAuth = localStorage.getItem('auth');
    if (storedAuth) {
      try {
        const parsedAuth = JSON.parse(storedAuth);
        ctx.patchState(parsedAuth);
      } catch (e) {
        console.error('Error al parsear auth de localStorage', e);
        localStorage.removeItem('auth');
      }
    }
  }
}